using Astibosoft.Api.Data.Stores.AdminsStore;
using Astibosoft.Api.Errors.Business;
using Astibosoft.Api.Features.Auth.Login;
using Astibosoft.Api.Infrastructure.Auth;

namespace Astibosoft.Api.Features.Auth;

public class AuthService(IAdminStore adminStore, JwtTokenIssuer jwtTokenIssuer)
{
    public async Task<Result<LoginResponse>> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var admin = await adminStore.FindByEmailAsync(email, cancellationToken);

        if (admin is null || !BCrypt.Net.BCrypt.Verify(request.Password, admin.PasswordHash))
            return Result<LoginResponse>.Fail(ErrorCodes.InvalidCredentials);

        var accessToken = jwtTokenIssuer.CreateAdminToken(admin);

        return Result<LoginResponse>.Success(
            new LoginResponse(accessToken, admin.Id, admin.Email, admin.Name));
    }
}
