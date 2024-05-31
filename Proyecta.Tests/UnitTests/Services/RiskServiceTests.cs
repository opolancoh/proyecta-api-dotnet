using Moq;
using Proyecta.Core.Contracts.Repositories.Risk;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Core.DTOs.Risk;
using Proyecta.Core.Entities.Risk;
using Proyecta.Services.Risk;

namespace Proyecta.Tests.UnitTests.Services;

public class RiskServiceTests
{
    private readonly Mock<IRiskRepository> _mockRepository;
    private readonly IRiskService _service;

    public RiskServiceTests()
    {
        _mockRepository = new Mock<IRiskRepository>();
        _service = new RiskService(_mockRepository.Object);
    }

    [Fact]
    public async Task Create_WhenResultIsZero_ShouldReturnConflictResponse()
    {
        // Arrange
        var riskDto = new RiskAddOrUpdateDto { Name = "Test Risk" };
        var currentUserId = "user123";

        _mockRepository.Setup(m => m.Add(It.IsAny<Risk>())).ReturnsAsync(0);

        // Act
        var response = await _service.Create(riskDto, currentUserId);

        // Assert
        Assert.Equal(ApiResponseStatus.Conflict, response.Status);
        Assert.Contains("The resource could not be created", ((ApiBody<IdNameDetailDto<Guid>>)response.Body).Message);
    }

    [Fact]
    public async Task Update_WhenResultIsZero_ShouldReturnConflictResponse()
    {
        // Arrange
        var riskDto = new RiskAddOrUpdateDto { Name = "Test Risk" };
        var id = Guid.NewGuid();
        var currentUserId = "user123";

        _mockRepository.Setup(m => m.Update(It.IsAny<Risk>())).ReturnsAsync(0);

        // Act
        var response = await _service.Update(id, riskDto, currentUserId);

        // Assert
        Assert.Equal(ApiResponseStatus.Conflict, response.Status);
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
        Assert.Equal(ApiResponseStatus.Conflict, response.Status);
        Assert.Contains("The requested resource with ID", (response.Body).Message);
    }
}
