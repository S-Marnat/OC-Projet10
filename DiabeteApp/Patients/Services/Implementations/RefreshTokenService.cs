using Microsoft.AspNetCore.Identity;
using Patients.Repositories.Interfaces;
using Patients.Services.Interfaces;

namespace Patients.Services.Implementations
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _repository;
        private readonly IJwtService _jwtService;
        private readonly UserManager<IdentityUser> _userManager;

        public RefreshTokenService(
            IRefreshTokenRepository repository,
            IJwtService jwtService,
            UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _jwtService = jwtService;
            _userManager = userManager;
        }

        public async Task<(string jwt, string refreshToken)> GenerateTokensAsync(IdentityUser user)
        {
            var jwt = await _jwtService.GenerateTokenAsync(user);

            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                Expiration = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            await _repository.CreateAsync(refreshToken);

            return (jwt, refreshToken.Token);
        }

        public async Task<(string jwt, string refreshToken)?> RefreshAsync(string refreshToken)
        {
            var token = await _repository.GetAsync(refreshToken);

            if (token == null || token.Expiration < DateTime.UtcNow)
                return null;

            var user = await _userManager.FindByIdAsync(token.UserId.ToString());

            if (user == null)
                return null;

            await _repository.RevokeAsync(token);

            return await GenerateTokensAsync(user);
        }

        public async Task<bool> RevokeAsync(string refreshToken)
        {
            var token = await _repository.GetAsync(refreshToken);

            if (token == null)
                return false;

            await _repository.RevokeAsync(token);
            return true;
        }
    }
}
