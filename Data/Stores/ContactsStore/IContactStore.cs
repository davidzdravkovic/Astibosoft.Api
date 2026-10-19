using Astibosoft.Api.Domain.Contacts;

namespace Astibosoft.Api.Data.Stores.ContactsStore;

public interface IContactStore
{
    Task AddAsync(Contact contact);

    Task<IReadOnlyList<Contact>> ListRecentAsync(
        int take = 100,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
