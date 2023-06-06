namespace Proyecta.Core.Exceptions;

public sealed class EntityNotFoundException<T> : Exception
{
    public EntityNotFoundException(T entityId)
        : base($"The entity with id '{entityId}' doesn't exist in the database.")
    {
    }
}