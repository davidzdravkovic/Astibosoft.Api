namespace Astibosoft.Api.Features.Auth.Login;

public record AdminSessionResponse(
    Guid Id,
    string Email,
    string Name);
