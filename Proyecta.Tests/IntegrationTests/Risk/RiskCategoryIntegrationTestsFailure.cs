using Proyecta.Tests.IntegrationTests.Fixtures;
using Proyecta.Tests.IntegrationTests.IdName;

namespace Proyecta.Tests.IntegrationTests.Risk;

public class RiskCategoryIntegrationTestsFailure : IdNameBaseIntegrationTestsFailure
{
    protected override string BasePath => "/api/risk-categories";

    public RiskCategoryIntegrationTestsFailure(ApiWebApplicationFactory factory) : base(factory)
    {
    }
}