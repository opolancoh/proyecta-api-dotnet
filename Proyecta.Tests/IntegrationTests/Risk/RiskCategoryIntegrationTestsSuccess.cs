using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests.Risk;

public class RiskCategoryIntegrationTestsSuccess : IdNameBaseIntegrationTestsSuccess
{
    protected override string BasePath => "/api/risk-categories";

    public RiskCategoryIntegrationTestsSuccess(CustomWebApplicationFactory factory) : base(factory)
    {
    }
}