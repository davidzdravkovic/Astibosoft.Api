using Astibosoft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Astibosoft.Api.Composition;

public static class WebApplicationExtensions
{
    public static async Task UseAstibosoftPipelineAsync(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        if (app.Configuration.GetValue<bool>("Database:AutoMigrate"))
        {
            await using var scope = app.Services.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
        }

        await AdminSeeder.SeedAsync(app.Services);

        // Must run before rate limiting / controllers so RemoteIpAddress is the client.
        app.UseForwardedHeaders();

        app.UseExceptionHandler();
        app.UseCors(app.Environment.IsDevelopment() ? "DevCors" : "ProdCors");

        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRateLimiter();
        app.MapControllers();
    }
}
