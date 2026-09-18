using Microsoft.AspNetCore.Identity;

namespace Patients.Services.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(IdentityUser user);
    }
}
