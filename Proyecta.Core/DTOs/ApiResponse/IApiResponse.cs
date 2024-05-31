namespace Proyecta.Core.DTOs.ApiResponse;

public interface IApiResponse
{
    int Status { get; }
    ApiBody Body { get; }
}