namespace Proyecta.Core.DTOs;

/* public interface IValueType
{
    // You could have some common methods or properties here.
}

public class GuidType : IValueType
{
    public Guid Value { get; set; }
}

public class IntType : IValueType
{
    public int Value { get; set; }
} */

public record KeyValueDto<T> // where T : IValueType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}