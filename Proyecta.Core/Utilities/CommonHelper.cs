namespace Proyecta.Core.Utilities;

public static class CommonHelper
{
    public static string GetEnvironmentVariable(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);
        if (string.IsNullOrEmpty(value))
            throw new InvalidOperationException($"The environment variable '{name}' is null or empty.");

        return value;
    }
}