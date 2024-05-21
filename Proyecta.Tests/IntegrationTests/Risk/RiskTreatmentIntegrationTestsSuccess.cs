using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests.Risk;

public class RiskTreatmentIntegrationTestsSuccess : IdNameBaseIntegrationTestsSuccess
{
    protected override string BasePath => "/api/risk-treatments";

    public RiskTreatmentIntegrationTestsSuccess(CustomWebApplicationFactory factory) : base(factory)
    {
    }
}