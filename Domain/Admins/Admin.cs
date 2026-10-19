namespace Astibosoft.Api.Domain.Admins;

public class Admin
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
