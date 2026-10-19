using System.Net.Mail;
using Astibosoft.Api.Data.Stores.ContactsStore;
using Astibosoft.Api.Domain.Contacts;
using Astibosoft.Api.Errors.Business;
using Astibosoft.Api.Features.Contacts.Create;

namespace Astibosoft.Api.Features.Contacts;

public class ContactService(
    IContactStore contactStore,
    ILogger<ContactService> logger)
{
    public const int MaxMessageLength = 1500;

    public async Task<Result<ContactResponse>> CreateAsync(
        CreateContactRequest request,
        string? clientIp,
        CancellationToken cancellationToken = default)
    {
        // Honeypot filled → pretend success so bots do not learn the filter.
        if (!string.IsNullOrWhiteSpace(request.Website))
        {
            logger.LogInformation("Contact honeypot triggered from {ClientIp}", clientIp);
            return Result<ContactResponse>.Success(
                new ContactResponse(Guid.Empty, DateTime.UtcNow));
        }

        var email = request.Email.Trim().ToLowerInvariant();
        if (!IsValidEmail(email))
            return Result<ContactResponse>.Fail(ErrorCodes.ContactInvalidEmail);

        var message = request.Message.Trim();
        if (message.Length > MaxMessageLength)
            return Result<ContactResponse>.Fail(ErrorCodes.ContactMessageTooLong);

        var contact = new Contact
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Email = email,
            Company = string.IsNullOrWhiteSpace(request.Company)
                ? null
                : request.Company.Trim(),
            Service = string.IsNullOrWhiteSpace(request.Service)
                ? null
                : request.Service.Trim(),
            Message = message,
            ClientIp = string.IsNullOrWhiteSpace(clientIp) ? null : clientIp.Trim(),
            CreatedAt = DateTime.UtcNow,
        };

        await contactStore.AddAsync(contact);
        await contactStore.SaveChangesAsync(cancellationToken);

        return Result<ContactResponse>.Success(ToResponse(contact));
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var parsed = new MailAddress(email);
            return string.Equals(parsed.Address, email, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    private static ContactResponse ToResponse(Contact contact) =>
        new(contact.Id, contact.CreatedAt);
}
