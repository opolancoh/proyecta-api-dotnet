using Proyecta.Tests.IntegrationTests.Fixtures;
using Proyecta.Tests.IntegrationTests.IdName;

namespace Proyecta.Tests.IntegrationTests.Risk;

public class RiskOwnerIntegrationTestsSuccess : IdNameBaseIntegrationTestsSuccess
{
    protected override string BasePath => "/api/risk-owners";

    public RiskOwnerIntegrationTestsSuccess(ApiWebApplicationFactory factory) : base(factory)
    {
    }
}