namespace Astibosoft.Api.Infrastructure.Auth;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string SigningKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = "Astibosoft";
    public string Audience { get; set; } = "Astibosoft";
    public int ExpiryMinutes { get; set; } = 480;
}
