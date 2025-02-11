using System;
using System.IO;
using System.Threading.Tasks;
using Azure.AI.Vision.ImageAnalysis;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class ProgramTests
{
    [Fact]
    public async Task Main_WithValidInputs_Succeeds()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://fakeendpoint.cognitiveservices.azure.com/");
        mockConfig.SetupGet(c => c["AIServicesKey"]).Returns("fakekey");

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

        var mockImageAnalysisClient = new Mock<ImageAnalysisClient>(new Uri("https://fakeendpoint.cognitiveservices.azure.com/"), new AzureKeyCredential("fakekey"));

        var program = new Program();
        var args = new string[] { "testimage.jpg" };

        // Mock the AnalyzeImage method
        mockImageAnalysisClient.Setup(client => client.AnalyzeImageAsync(It.IsAny<string>(), It.IsAny<ImageAnalysisOptions>()))
            .Returns(Task.FromResult(new ImageAnalysisResult()));

        // Mock the BackgroundForeground method
        mockImageAnalysisClient.Setup(client => client.GenerateForegroundMatteAsync(It.IsAny<string>(), It.IsAny<BackgroundRemovalOptions>()))
            .Returns(Task.FromResult(new ForegroundMatte()));

        // Act
        await program.Main(args);

        // Assert
        mockImageAnalysisClient.Verify(client => client.AnalyzeImageAsync(It.IsAny<string>(), It.IsAny<ImageAnalysisOptions>()), Times.Once);
        mockImageAnalysisClient.Verify(client => client.GenerateForegroundMatteAsync(It.IsAny<string>(), It.IsAny<BackgroundRemovalOptions>()), Times.Once);
    }

    [Fact]
    public void Main_WithNoArguments_Succeeds()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://fakeendpoint.cognitiveservices.azure.com/");
        mockConfig.SetupGet(c => c["AIServicesKey"]).Returns("fakekey");

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

        var mockImageAnalysisClient = new Mock<ImageAnalysisClient>(new Uri("https://fakeendpoint.cognitiveservices.azure.com/"), new AzureKeyCredential("fakekey"));

        var program = new Program();
        var args = new string[] { };

        // Mock the AnalyzeImage method
        mockImageAnalysisClient.Setup(client => client.AnalyzeImageAsync(It.IsAny<string>(), It.IsAny<ImageAnalysisOptions>()))
            .Returns(Task.FromResult(new ImageAnalysisResult()));

        // Mock the BackgroundForeground method
        mockImageAnalysisClient.Setup(client => client.GenerateForegroundMatteAsync(It.IsAny<string>(), It.IsAny<BackgroundRemovalOptions>()))
            .Returns(Task.FromResult(new ForegroundMatte()));

        // Act
        program.Main(args).Wait();

        // Assert
        mockImageAnalysisClient.Verify(client => client.AnalyzeImageAsync(It.IsAny<string>(), It.IsAny<ImageAnalysisOptions>()), Times.Once);
        mockImageAnalysisClient.Verify(client => client.GenerateForegroundMatteAsync(It.IsAny<string>(), It.IsAny<BackgroundRemovalOptions>()), Times.Once);
    }

    [Fact]
    public async Task Main_WithInvalidInputs_ThrowsException()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Returns("");
        mockConfig.SetupGet(c => c["AIServicesKey"]).Returns("");

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

        var mockImageAnalysisClient = new Mock<ImageAnalysisClient>(new Uri(""), new AzureKeyCredential(""));

        var program = new Program();
        var args = new string[] { "nonexistentimage.jpg" };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => program.Main(args));
    }
}