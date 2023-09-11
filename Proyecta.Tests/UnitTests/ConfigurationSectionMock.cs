using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Proyecta.Tests.UnitTests;

// Mock class for IConfigurationSection
public class ConfigurationSectionMock : IConfigurationSection
{
    private readonly string _value;

    public ConfigurationSectionMock(string value)
    {
        _value = value;
    }

    public string this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string Key => throw new NotImplementedException();

    public string Path => throw new NotImplementedException();

    public string Value { get => _value; set => throw new NotImplementedException(); }

    public IEnumerable<IConfigurationSection> GetChildren()
    {
        throw new NotImplementedException();
    }

    public IChangeToken GetReloadToken()
    {
        throw new NotImplementedException();
    }

    public IConfigurationSection GetSection(string key)
    {
        throw new NotImplementedException();
    }
}