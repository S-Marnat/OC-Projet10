using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Patients.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Patients.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;

        public JwtService(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> GenerateTokenAsync(IdentityUser user)
        {
            // Récupération des rôles de l’utilisateur
            var roles = await _userManager.GetRolesAsync(user);

            // Construction de la liste des claims (information embarquée dans le token)
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

            // Ajout des rôles dans les claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Création de la clé de signature
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]
                    ?? Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                )
            );

            // Création des credentials de signature (garantit que le token n’a pas été modifié)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Construction du JWT
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])
                ),
                signingCredentials: creds
            );

            // Conversion du token en chaîne de caractères
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
