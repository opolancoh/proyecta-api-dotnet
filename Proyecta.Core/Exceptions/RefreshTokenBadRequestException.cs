namespace Proyecta.Core.Exceptions;

public class RefreshTokenBadRequestException : Exception
{
    public RefreshTokenBadRequestException()
        : base("Invalid client request. The tokenDto has some invalid values.")
    {
    }
}