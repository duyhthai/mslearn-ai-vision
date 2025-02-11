using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.AI.Vision.ImageAnalysis;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class ProgramTests
{
    [Fact]
    public async Task Main_WithValidArgs_ShouldAnalyzeAndProcessImage()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://endpoint.cognitiveservices.azure.com/");
        mockConfig.SetupGet(c => c["AIServicesKey"]).Returns("key");
        
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
        
        var mockImageAnalysisClient = new Mock<ImageAnalysisClient>(new Uri("https://endpoint.cognitiveservices.azure.com/"), new AzureKeyCredential("key"));
        mockImageAnalysisClient.Setup(client => client.AnalyzeAsync(It.IsAny<Stream>(), It.IsAny<ImageAnalysisOptions>()))
                               .Returns(Task.FromResult(new ImageAnalysisResult()));
        
        var program = new Program();
        var args = new string[] { "test.jpg" };

        // Act & Assert
        await program.Main(args);

        mockConfig.VerifyAll();
        mockHttpClientFactory.VerifyAll();
        mockImageAnalysisClient.VerifyAll();
    }

    [Fact]
    public async Task Main_WithoutArgs_ShouldUseDefaultImage()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://endpoint.cognitiveservices.azure.com/");
        mockConfig.SetupGet(c => c["AIServicesKey"]).Returns("key");

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

        var mockImageAnalysisClient = new Mock<ImageAnalysisClient>(new Uri("https://endpoint.cognitiveservices.azure.com/"), new AzureKeyCredential("key"));
        mockImageAnalysisClient.Setup(client => client.AnalyzeAsync(It.IsAny<Stream>(), It.IsAny<ImageAnalysisOptions>()))
                               .Returns(Task.FromResult(new ImageAnalysisResult()));

        var program = new Program();

        // Act & Assert
        await program.Main(null);

        mockConfig.VerifyAll();
        mockHttpClientFactory.VerifyAll();
        mockImageAnalysisClient.VerifyAll();
    }

    [Fact]
    public async Task Main_WithInvalidConfig_ShouldThrowException()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Throws<InvalidOperationException>();

        var program = new Program();
        var args = new string[] { "test.jpg" };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => program.Main(args));
    }
}