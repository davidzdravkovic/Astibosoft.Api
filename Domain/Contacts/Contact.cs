namespace Astibosoft.Api.Domain.Contacts;

public class Contact
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Company { get; set; }
    public string? Service { get; set; }
    public string Message { get; set; } = null!;

    public string? ClientIp { get; set; }

    public DateTime CreatedAt { get; set; }
}
