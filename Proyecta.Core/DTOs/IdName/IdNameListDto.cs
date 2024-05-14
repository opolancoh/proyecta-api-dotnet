namespace Proyecta.Core.DTOs.IdName;

public record IdNameListDto<T> : IdNameDto<T>
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}