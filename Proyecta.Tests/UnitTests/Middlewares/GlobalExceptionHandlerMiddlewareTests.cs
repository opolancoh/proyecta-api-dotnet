using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Proyecta.Web.Middlewares;

namespace Proyecta.Tests.UnitTests.Middlewares;

public class GlobalExceptionHandlerMiddlewareTests
{
    private readonly Mock<RequestDelegate> _nextMock;
    private readonly LoggerMock<GlobalExceptionHandlerMiddleware> _loggerMock;
    private readonly GlobalExceptionHandlerMiddleware _middleware;

    public GlobalExceptionHandlerMiddlewareTests()
    {
        _nextMock = new Mock<RequestDelegate>();
        _loggerMock = new LoggerMock<GlobalExceptionHandlerMiddleware>();
        _middleware = new GlobalExceptionHandlerMiddleware(_nextMock.Object, _loggerMock);
    }

    [Fact]
    public async Task InvokeAsync_WhenNoException_ContinuesPipeline()
    {
        // Arrange
        var context = new DefaultHttpContext();

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_WhenException_LogsErrorAndSetsResponse()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var exception = new Exception("Test exception");
        _nextMock.Setup(next => next(It.IsAny<HttpContext>())).ThrowsAsync(exception);

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        var logEntries = _loggerMock.GetLogEntries();
        var logEntry = logEntries.FirstOrDefault();

        Assert.NotNull(logEntry);
        Assert.Equal(LogLevel.Error, logEntry.LogLevel);
        Assert.Equal(exception, logEntry.Exception);
        Assert.Equal("An unhandled exception has occurred.", logEntry.Message);

        Assert.Equal("application/json; charset=utf-8", context.Response.ContentType);
        Assert.Equal(500, context.Response.StatusCode);
    }
}
