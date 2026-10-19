using Astibosoft.Api.Domain.Admins;

namespace Astibosoft.Api.Data.Stores.AdminsStore;

public interface IAdminStore
{
    Task<Admin?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

    Task AddAsync(Admin admin);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
