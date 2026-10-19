namespace Astibosoft.Api.Features.Contacts.List;

public record ContactListItemResponse(
    Guid Id,
    string Name,
    string Email,
    string? Company,
    string? Service,
    string Message,
    string? ClientIp,
    DateTime CreatedAt);
