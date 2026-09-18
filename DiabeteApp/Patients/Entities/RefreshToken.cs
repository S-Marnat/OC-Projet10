using Microsoft.AspNetCore.Identity;

public class RefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime Expiration { get; set; }

    public bool IsRevoked { get; set; }

    public string UserId { get; set; } = string.Empty;
    public IdentityUser User { get; set; } = default!;
}