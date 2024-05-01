namespace Proyecta.Core.DTOs.Auth;

public record ApplicationUserDetailDto : ApplicationUserBaseDto
{
    public DateTime CreatedAt { get; init; }
    public IdNameDto<string>? CreatedBy { get; set; }
    public DateTime UpdatedAt { get; init; }
    public IdNameDto<string>? UpdatedBy { get; set; }
    public IEnumerable<IdNameDto<string>> Roles { get; set; }
}