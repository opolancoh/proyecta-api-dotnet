using Proyecta.Tests.IntegrationTests.Fixtures;
using Proyecta.Tests.IntegrationTests.IdName;

namespace Proyecta.Tests.IntegrationTests.Risk;

public class RiskOwnerIntegrationTestsFailure : IdNameBaseIntegrationTestsFailure
{
    protected override string BasePath => "/api/risk-owners";

    public RiskOwnerIntegrationTestsFailure(ApiWebApplicationFactory factory) : base(factory)
    {
    }
}