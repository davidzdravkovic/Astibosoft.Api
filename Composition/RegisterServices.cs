using System.Threading.RateLimiting;
using Asibosoft.Api.Data;
using Asibosoft.Api.Data.Stores.ContactsStore;
using Asibosoft.Api.Errors.Exceptions;
using Asibosoft.Api.Features.Contacts;
using Microsoft.EntityFrameworkCore;

namespace Asibosoft.Api.Composition;

public static class RegisterServices
{
    public static IServiceCollection AddAsibosoftServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOpenApi();
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

        services.AddAsibosoftCors(configuration);
        services.AddAsibosoftRateLimiting();

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IContactStore, EfContactStore>();
        services.AddScoped<ContactService>();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    private static IServiceCollection AddAsibosoftCors(
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

    private static IServiceCollection AddAsibosoftRateLimiting(this IServiceCollection services)
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
