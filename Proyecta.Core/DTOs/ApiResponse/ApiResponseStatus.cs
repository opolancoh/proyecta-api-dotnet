namespace Proyecta.Core.DTOs.ApiResponse;

public static class ApiResponseStatus
{
    public const int Ok = 200;
    public const int Created = 201;
    public const int Accepted = 202;
    public const int NoContent = 204;
    public const int MultiStatus = 207;
    public const int BadRequest = 400;
    public const int Unauthorized = 401;
    public const int NotFound = 404;
    public const int Conflict = 409;
    public const int InternalServerError = 500;
}