using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Patients.DTOs;
using Patients.Services.Interfaces;

namespace Patients.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IRefreshTokenService _refreshTokenService;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Les informations fournies pour l'utilisateur sont invalides.");

            try
            {
                // Vérifier si l'utilisateur existe
                var user = await _userManager.FindByEmailAsync(dto.Email);

                if (user == null)
                    return Unauthorized("Identifiants invalides.");

                // Vérifier si le mot de passe correspond
                var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

                if (!result.Succeeded)
                    return Unauthorized("Identifiants invalides.");

                var tokens = await _refreshTokenService.GenerateTokensAsync(user);

                return Ok(new TokenResponseDto
                {
                    Token = tokens.jwt,
                    RefreshToken = tokens.refreshToken
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur interne est survenue.");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenDto dto)
        {
            try
            {
                var revoked = await _refreshTokenService.RevokeAsync(dto.RefreshToken);

                return Ok("Déconnexion effectuée.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur interne est survenue.");
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDto dto)
        {
            try
            {
                var result = await _refreshTokenService.RefreshAsync(dto.RefreshToken);

                if (result == null)
                    return Unauthorized("Refresh token invalide ou expiré.");

                var (jwt, refreshToken) = result.Value;

                return Ok(new TokenResponseDto
                {
                    Token = jwt,
                    RefreshToken = refreshToken
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur interne est survenue.");
            }
        }
    }
}
