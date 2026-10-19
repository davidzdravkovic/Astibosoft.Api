using System.ComponentModel.DataAnnotations;

namespace Astibosoft.Api.Features.Auth.Login;

public record LoginRequest(
    [Required, EmailAddress, MaxLength(320)] string Email,
    [Required, MinLength(8), MaxLength(200)] string Password);
