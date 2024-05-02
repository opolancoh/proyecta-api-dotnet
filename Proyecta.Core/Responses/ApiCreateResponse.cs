namespace Proyecta.Core.Responses;

public record ApiCreateResponse<T>
{
    public T Id { get; set; }
}