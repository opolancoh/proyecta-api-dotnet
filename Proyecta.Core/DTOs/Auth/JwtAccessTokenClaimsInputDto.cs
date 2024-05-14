namespace Proyecta.Core.DTOs.Auth;

public record JwtAccessTokenClaimsInputDto
{
    public string UserId { get; init; }
    public string UserName { get; init; }
    public string UserDisplayName { get; init; }
    
    public List<string> UserRoles { get; init; }
}