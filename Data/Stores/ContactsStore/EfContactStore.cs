using Astibosoft.Api.Domain.Contacts;
using Microsoft.EntityFrameworkCore;

namespace Astibosoft.Api.Data.Stores.ContactsStore;

public class EfContactStore(AppDbContext db) : IContactStore
{
    public Task AddAsync(Contact contact)
    {
        db.Contacts.Add(contact);
        return Task.CompletedTask;
    }

    public async Task<IReadOnlyList<Contact>> ListRecentAsync(
        int take = 100,
        CancellationToken cancellationToken = default)
    {
        return await db.Contacts
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        db.SaveChangesAsync(cancellationToken);
}
