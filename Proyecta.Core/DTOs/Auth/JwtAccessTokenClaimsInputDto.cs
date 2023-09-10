namespace Proyecta.Core.DTOs.Auth;

public record JwtAccessTokenClaimsInputDto
{
    public string userId { get; init; }
    public string userFirstName { get; init; }
    public string userLastName { get; init; }
    public string userName { get; init; }
    public List<string> userRoles { get; init; }
}