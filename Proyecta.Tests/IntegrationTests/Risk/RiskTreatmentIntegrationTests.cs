using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests.Risk;

public class RiskTreatmentIntegrationTests : IdNameBaseIntegrationTests
{
    protected override string BasePath => "/api/risk-treatments";

    public RiskTreatmentIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }
}