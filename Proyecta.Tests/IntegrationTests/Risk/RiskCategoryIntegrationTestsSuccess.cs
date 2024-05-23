using Proyecta.Tests.IntegrationTests.Fixtures;
using Proyecta.Tests.IntegrationTests.IdName;

namespace Proyecta.Tests.IntegrationTests.Risk;

public class RiskCategoryIntegrationTestsSuccess : IdNameBaseIntegrationTestsSuccess
{
    protected override string BasePath => "/api/risk-categories";

    public RiskCategoryIntegrationTestsSuccess(ApiWebApplicationFactory factory) : base(factory)
    {
    }
}