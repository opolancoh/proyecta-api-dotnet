namespace Proyecta.Core.DTOs;

public record ApplicationUserListDto : ApplicationUserBaseDto
{
    public IEnumerable<string> Roles { get; init; }
}