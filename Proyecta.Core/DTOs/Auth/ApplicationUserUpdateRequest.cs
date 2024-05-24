namespace Proyecta.Core.DTOs.Auth;

public record ApplicationUserUpdateRequest
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? DisplayName { get; init; }
    public string? UserName { get; set; }
    public ICollection<string>? Roles { get; init; }
}