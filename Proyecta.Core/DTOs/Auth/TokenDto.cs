namespace Proyecta.Core.DTOs.Auth;

public record TokenDto
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}