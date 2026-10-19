using Astibosoft.Api.Domain.Contacts;

namespace Astibosoft.Api.Data.Stores.ContactsStore;

public interface IContactStore
{
    Task AddAsync(Contact contact);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
