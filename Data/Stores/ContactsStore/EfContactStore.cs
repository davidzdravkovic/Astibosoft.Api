using Asibosoft.Api.Domain.Contacts;

namespace Asibosoft.Api.Data.Stores.ContactsStore;

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
