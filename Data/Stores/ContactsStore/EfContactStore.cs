using Astibosoft.Api.Domain.Contacts;

namespace Astibosoft.Api.Data.Stores.ContactsStore;

public class EfContactStore(AppDbContext db) : IContactStore
{
    public Task AddAsync(Contact contact)
    {
        db.Contacts.Add(contact);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        db.SaveChangesAsync(cancellationToken);
}
