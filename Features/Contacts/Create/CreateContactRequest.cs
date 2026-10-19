using System.ComponentModel.DataAnnotations;

namespace Asibosoft.Api.Features.Contacts.Create;

// Website is a honeypot: leave empty; bots that fill it are ignored.
public record CreateContactRequest(
    [Required, MinLength(1), MaxLength(200)] string Name,
    [Required, EmailAddress, MaxLength(320)] string Email,
    [Required, MinLength(1), MaxLength(1500)] string Message,
    [MaxLength(300)] string? Company = null,
    [MaxLength(200)] string? Service = null,
    [MaxLength(200)] string? Website = null);
