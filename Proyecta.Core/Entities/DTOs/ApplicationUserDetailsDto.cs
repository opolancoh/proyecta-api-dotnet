namespace Proyecta.Core.Entities.DTOs;

public record ApplicationUserDetailsDto : ApplicationUserBaseDto
{
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}