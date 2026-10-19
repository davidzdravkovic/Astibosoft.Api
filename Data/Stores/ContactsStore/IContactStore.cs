using Asibosoft.Api.Domain.Contacts;

namespace Asibosoft.Api.Data.Stores.ContactsStore;

public interface IContactStore
{
    Task AddAsync(Contact contact);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
