using Astibosoft.Api.Domain.Admins;
using Microsoft.EntityFrameworkCore;

namespace Astibosoft.Api.Data.Stores.AdminsStore;

public class EfAdminStore(AppDbContext db) : IAdminStore
{
    public Task<Admin?> FindByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return db.Admins.FirstOrDefaultAsync(a => a.Email == email, cancellationToken);
    }

    public Task<bool> EmailExistsAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return db.Admins.AnyAsync(a => a.Email == email, cancellationToken);
    }

    public Task AddAsync(Admin admin)
    {
        db.Admins.Add(admin);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        db.SaveChangesAsync(cancellationToken);
}
