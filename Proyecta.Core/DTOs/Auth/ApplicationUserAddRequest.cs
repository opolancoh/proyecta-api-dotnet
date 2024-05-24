namespace Proyecta.Core.DTOs.Auth;

public record ApplicationUserAddRequest
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? DisplayName { get; init; }
    public string? UserName { get; set; }
    public string? Password { get; init; }
    public ICollection<string>? Roles { get; init; }
}