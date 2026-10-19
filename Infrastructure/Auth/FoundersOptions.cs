namespace Astibosoft.Api.Infrastructure.Auth;

public class FoundersOptions
{
    public const string SectionName = "Founders";

    public FounderSeedOptions? Founder1 { get; set; }
    public FounderSeedOptions? Founder2 { get; set; }
}

public class FounderSeedOptions
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
