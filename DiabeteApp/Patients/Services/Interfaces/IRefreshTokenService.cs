using Microsoft.AspNetCore.Identity;

namespace Patients.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<(string jwt, string refreshToken)> GenerateTokensAsync(IdentityUser user);
        Task<(string jwt, string refreshToken)?> RefreshAsync(string refreshToken);
        Task<bool> RevokeAsync(string refreshToken);
    }
}
