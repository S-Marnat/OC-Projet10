namespace Patients.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> CreateAsync(RefreshToken token);
        Task<RefreshToken?> GetAsync(string token);
        Task RevokeAsync(RefreshToken token);
    }
}
