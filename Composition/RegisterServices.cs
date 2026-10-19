using System.Threading.RateLimiting;
using Astibosoft.Api.Data;
using Astibosoft.Api.Data.Stores.ContactsStore;
using Astibosoft.Api.Errors.Exceptions;
using Astibosoft.Api.Features.Contacts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

namespace Astibosoft.Api.Composition;

public static class RegisterServices
{
    public static IServiceCollection AddAstibosoftServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // One trusted proxy (Infra nginx). Peels one X-Forwarded-For hop from the right
        // into Connection.RemoteIpAddress for rate limiting + Contact.ClientIp.
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.ForwardLimit = 1;
            // Docker nginx peer is not loopback; clear defaults so the header is applied.
            // Safe only while the API is not publicly exposed (edge network behind nginx).
            options.KnownIPNetworks.Clear();
            options.KnownProxies.Clear();
        });

        services.AddOpenApi();
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

        services.AddAstibosoftCors(configuration);
        services.AddAstibosoftRateLimiting();

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IContactStore, EfContactStore>();
        services.AddScoped<ContactService>();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    private static IServiceCollection AddAstibosoftCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("DevCors", policy =>
                policy.SetIsOriginAllowed(static origin =>
                    {
                        if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                            return false;
                        return uri.IsLoopback;
                    })
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            options.AddPolicy("ProdCors", policy =>
            {
                var origins = configuration.GetSection("Allowed_Cors").Get<string[]>() ?? [];
                policy.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }

    private static IServiceCollection AddAstibosoftRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsJsonAsync(
                    new { message = "Too many requests. Try again shortly." },
                    cancellationToken);
            };

            options.AddPolicy("contact", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                    }));
        });

        return services;
    }
}
