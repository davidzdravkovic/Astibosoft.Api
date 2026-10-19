using Astibosoft.Api.Data.Stores.AdminsStore;
using Astibosoft.Api.Domain.Admins;
using Astibosoft.Api.Infrastructure.Auth;
using Microsoft.Extensions.Options;

namespace Astibosoft.Api.Data;

public static class AdminSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var store = scope.ServiceProvider.GetRequiredService<IAdminStore>();
        var options = scope.ServiceProvider.GetRequiredService<IOptions<FoundersOptions>>().Value;
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger("AdminSeeder");

        await SeedOneAsync(store, options.Founder1, logger);
        await SeedOneAsync(store, options.Founder2, logger);
    }

    private static async Task SeedOneAsync(
        IAdminStore store,
        FounderSeedOptions? seed,
        ILogger logger)
    {
        if (seed is null)
            return;

        var email = seed.Email.Trim().ToLowerInvariant();
        var password = seed.Password;
        var name = string.IsNullOrWhiteSpace(seed.Name)
            ? email
            : seed.Name.Trim();

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning("Skipping founder seed: email or password missing.");
            return;
        }

        if (await store.EmailExistsAsync(email))
        {
            logger.LogInformation("Founder {Email} already exists; skip seed.", email);
            return;
        }

        var admin = new Admin
        {
            Id = Guid.NewGuid(),
            Email = email,
            Name = name,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            CreatedAt = DateTime.UtcNow,
        };

        await store.AddAsync(admin);
        await store.SaveChangesAsync();
        logger.LogInformation("Seeded founder admin {Email}.", email);
    }
}
