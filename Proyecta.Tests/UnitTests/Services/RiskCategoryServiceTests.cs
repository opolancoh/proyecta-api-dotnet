using Moq;
using Proyecta.Core.Contracts.Repositories.Risk;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.ApiResponses;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Core.Entities.Risk;
using Proyecta.Services.Risk;

namespace Proyecta.Tests.UnitTests.Services;

public class RiskCategoryServiceTests
{
    private readonly Mock<IRiskCategoryRepository> _mockRepository;
    private readonly IRiskCategoryService _service;

    public RiskCategoryServiceTests()
    {
        _mockRepository = new Mock<IRiskCategoryRepository>();
        _service = new RiskCategoryService(_mockRepository.Object);
    }

    [Fact]
    public async Task Create_WhenResultIsZero_ShouldReturnConflictResponse()
    {
        // Arrange
        var dto = new IdNameAddOrUpdateDto { Name = "Test Item" };
        var currentUserId = "user123";

        _mockRepository.Setup(m => m.Create(It.IsAny<RiskCategory>())).ReturnsAsync(0);

        // Act
        var response = await _service.Add(dto, currentUserId);

        // Assert
        Assert.Equal(ApiStatusResponse.Conflict, response.Status);
        Assert.Contains("The resource could not be created", ((ApiBody<IdNameDetailDto<Guid>>)response.Body).Message);
    }

    [Fact]
    public async Task Update_WhenResultIsZero_ShouldReturnConflictResponse()
    {
        // Arrange
        var dto = new IdNameAddOrUpdateDto { Name = "Test Item" };
        var id = Guid.NewGuid();
        var currentUserId = "user123";

        _mockRepository.Setup(m => m.Update(It.IsAny<RiskCategory>())).ReturnsAsync(0);

        // Act
        var response = await _service.Update(id, dto, currentUserId);

        // Assert
        Assert.Equal(ApiStatusResponse.Conflict, response.Status);
        Assert.Contains("The resource with ID", ((ApiBody<IdNameDetailDto<Guid>>)response.Body).Message);
    }

    [Fact]
    public async Task Remove_WhenResultIsZero_ShouldReturnConflictResponse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var currentUserId = "user123";

        _mockRepository.Setup(m => m.Remove(It.IsAny<Guid>())).ReturnsAsync(0);

        // Act
        var response = await _service.Remove(id, currentUserId);

        // Assert
        Assert.Equal(ApiStatusResponse.Conflict, response.Status);
        Assert.Contains("The requested resource with ID", (response.Body).Message);
    }
}
