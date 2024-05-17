namespace Proyecta.Core.DTOs.Auth;

public record RegisterDto
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string DisplayName { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
}