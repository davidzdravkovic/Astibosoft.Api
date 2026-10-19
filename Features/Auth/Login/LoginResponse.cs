namespace Astibosoft.Api.Features.Auth.Login;

public record LoginResponse(
    string AccessToken,
    Guid Id,
    string Email,
    string Name);
