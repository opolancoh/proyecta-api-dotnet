using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests.Risk;

public class RiskOwnerIntegrationTestsSuccess : IdNameBaseIntegrationTestsSuccess
{
    protected override string BasePath => "/api/risk-owners";

    public RiskOwnerIntegrationTestsSuccess(CustomWebApplicationFactory factory) : base(factory)
    {
    }
}