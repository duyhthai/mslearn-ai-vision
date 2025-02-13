using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Xunit;

public class ProgramTests
{
    [Fact]
    public async Task Main_ShouldAnalyzeImageAndRemoveBackground()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://example.com/vision");
        mockConfig.SetupGet(c => c["AIServicesKey"]).Returns("key");

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

        var program = new Program();
        var args = new string[] { };

        // Act
        await program.Main(args);

        // Assert
        mockHttpClient.Verify(hc => hc.GetAsync(It.Is<string>(s => s.Contains("/analyze"))), Times.Once);
        mockHttpClient.Verify(hc => hc.PostAsync(It.Is<string>(s => s.Contains("/background/remove")), It.IsAny<HttpContent>()), Times.Once);
    }

    [Fact]
    public async Task Main_ShouldHandleEmptyArgs()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://example.com/vision");
        mockConfig.SetupGet(c => c["AIServicesKey"]).Returns("key");

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

        var program = new Program();
        var args = new string[] { };

        // Act
        await program.Main(args);

        // Assert
        mockHttpClient.Verify(hc => hc.GetAsync(It.Is<string>(s => s.Contains("/analyze"))), Times.Once);
        mockHttpClient.Verify(hc => hc.PostAsync(It.Is<string>(s => s.Contains("/background/remove")), It.IsAny<HttpContent>()), Times.Once);
    }

    [Fact]
    public async Task Main_ShouldThrowExceptionOnConfigError()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Throws(new InvalidOperationException());
        mockConfig.SetupGet(c => c["AIServicesKey"]).Throws(new InvalidOperationException());

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();

        var program = new Program();
        var args = new string[] { };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => program.Main(args));
    }
}