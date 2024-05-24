namespace Proyecta.Core.Entities.Auth;

public class RefreshToken
{
    public string UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
}
