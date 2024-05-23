using Proyecta.Tests.IntegrationTests.Fixtures;
using Proyecta.Tests.IntegrationTests.IdName;

namespace Proyecta.Tests.IntegrationTests.Risk;

public class RiskTreatmentIntegrationTestsSuccess : IdNameBaseIntegrationTestsSuccess
{
    protected override string BasePath => "/api/risk-treatments";

    public RiskTreatmentIntegrationTestsSuccess(ApiWebApplicationFactory factory) : base(factory)
    {
    }
}