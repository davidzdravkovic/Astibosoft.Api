using Asibosoft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Asibosoft.Api.Composition;

public static class WebApplicationExtensions
{
    public static async Task UseAsibosoftPipelineAsync(this WebApplication app)
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

        app.UseExceptionHandler();
        app.UseCors(app.Environment.IsDevelopment() ? "DevCors" : "ProdCors");

        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        app.UseRateLimiter();
        app.MapControllers();
    }
}
