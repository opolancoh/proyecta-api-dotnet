namespace Proyecta.Core.DTOs.Auth;

public record ApplicationUserDetailsDto : ApplicationUserBaseDto
{
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public IEnumerable<string> Roles { get; init; }
}