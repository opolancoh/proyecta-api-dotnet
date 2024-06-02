using Moq;
using Proyecta.Core.Contracts.Repositories.Risk;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.ApiResponses;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Core.Entities.Risk;
using Proyecta.Services.Risk;

namespace Proyecta.Tests.UnitTests.Services;

public class RiskTreatmentServiceTests
{
    private readonly Mock<IRiskTreatmentRepository> _mockRepository;
    private readonly IRiskTreatmentService _service;

    public RiskTreatmentServiceTests()
    {
        _mockRepository = new Mock<IRiskTreatmentRepository>();
        _service = new RiskTreatmentService(_mockRepository.Object);
    }

    [Fact]
    public async Task Create_WhenResultIsZero_ShouldReturnConflictResponse()
    {
        // Arrange
        var dto = new IdNameAddOrUpdateDto { Name = "Test Item" };
        var currentUserId = "user123";

        _mockRepository.Setup(m => m.Create(It.IsAny<RiskTreatment>())).ReturnsAsync(0);

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

        _mockRepository.Setup(m => m.Update(It.IsAny<RiskTreatment>())).ReturnsAsync(0);

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
