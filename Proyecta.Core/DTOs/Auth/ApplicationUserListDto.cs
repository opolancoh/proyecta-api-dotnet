namespace Proyecta.Core.DTOs.Auth;

public record ApplicationUserListDto : ApplicationUserBaseDto
{
    public IEnumerable<string> Roles { get; init; }
}