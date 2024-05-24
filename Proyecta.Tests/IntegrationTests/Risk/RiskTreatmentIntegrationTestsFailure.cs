using Proyecta.Tests.IntegrationTests.Fixtures;
using Proyecta.Tests.IntegrationTests.IdName;

namespace Proyecta.Tests.IntegrationTests.Risk;

public class RiskTreatmentIntegrationTestsFailure : IdNameBaseIntegrationTestsFailure
{
    protected override string BasePath => "/api/risk-treatments";

    public RiskTreatmentIntegrationTestsFailure(ApiWebApplicationFactory factory) : base(factory)
    {
    }
}