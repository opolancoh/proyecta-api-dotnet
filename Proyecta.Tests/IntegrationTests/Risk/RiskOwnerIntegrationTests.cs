using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests.Risk;

public class RiskOwnerIntegrationTests : IdNameBaseIntegrationTests
{
    protected override string BasePath => "/api/risk-owners";

    public RiskOwnerIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }
}