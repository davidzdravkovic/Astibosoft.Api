using Astibosoft.Api.Errors.Business;
using Astibosoft.Api.Features.Auth.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Astibosoft.Api.Features.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request, cancellationToken);

        if (!result.Succeeded)
            return ErrorResults.From(result.Error!);

        return Ok(result.Value);
    }

    [HttpGet("me")]
    [Authorize]
    public ActionResult<AdminSessionResponse> Me()
    {
        var session = authService.GetSession(User);
        if (session is null)
            return Unauthorized(new { message = "Not signed in." });

        return Ok(session);
    }
}
