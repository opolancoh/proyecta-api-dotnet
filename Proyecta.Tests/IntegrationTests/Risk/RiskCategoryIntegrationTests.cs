using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests.Risk;

public class RiskCategoryIntegrationTests : IdNameBaseIntegrationTests
{
    protected override string BasePath => "/api/risk-categories";

    public RiskCategoryIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }
}