namespace Sportal.Models;

public class LoginResponse
{
    public Guid UserId { get; set; }
    
    public DateTime AccessTokenExpiresAtUtc { get; set; }
    
    public string AccessToken { get; set; } = null!;
    
    // public string RefreshToken { get; set; } = null!;
    
    public string TokenType { get; set; } = "Bearer";
}