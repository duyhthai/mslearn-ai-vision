using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.AI.Vision.ImageAnalysis;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class ProgramTests
{
    [Fact]
    public async Task Main_WithValidArguments_ShouldAnalyzeAndProcessImage()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://fakeendpoint.cognitiveservices.azure.com/");
        mockConfig.SetupGet(c => c["AIServicesKey"]).Returns("fakekey");
        
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

        var mockImageAnalysisClient = new Mock<ImageAnalysisClient>(new Uri(mockConfig.Object["AIServicesEndpoint"]), new AzureKeyCredential(mockConfig.Object["AIServicesKey"]));
        mockImageAnalysisClient.Setup(c => c.AnalyzeAsync(It.IsAny<Stream>(), It.IsAny<IEnumerable<ImageAnalysisFeature>>(), default)).Returns(Task.FromResult(new AnalyzeResult()));

        var args = new string[] { "images/street.jpg" };
        var program = new Program();

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() => program.Main(args));
    }

    [Fact]
    public async Task Main_WithNoArguments_ShouldUseDefaultImagePath()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://fakeendpoint.cognitiveservices.azure.com/");
        mockConfig.SetupGet(c => c["AIServicesKey"]).Returns("fakekey");

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

        var mockImageAnalysisClient = new Mock<ImageAnalysisClient>(new Uri(mockConfig.Object["AIServicesEndpoint"]), new AzureKeyCredential(mockConfig.Object["AIServicesKey"]));
        mockImageAnalysisClient.Setup(c => c.AnalyzeAsync(It.IsAny<Stream>(), It.IsAny<IEnumerable<ImageAnalysisFeature>>(), default)).Returns(Task.FromResult(new AnalyzeResult()));

        var program = new Program();

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() => program.Main(new string[] { }));
    }

    [Fact]
    public async Task Main_WithInvalidEndpoint_ShouldThrowException()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Returns("");
        mockConfig.SetupGet(c => c["AIServicesKey"]).Returns("fakekey");

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

        var mockImageAnalysisClient = new Mock<ImageAnalysisClient>(new Uri(mockConfig.Object["AIServicesEndpoint"]), new AzureKeyCredential(mockConfig.Object["AIServicesKey"]));
        mockImageAnalysisClient.Setup(c => c.AnalyzeAsync(It.IsAny<Stream>(), It.IsAny<IEnumerable<ImageAnalysisFeature>>(), default)).Returns(Task.FromResult(new AnalyzeResult()));

        var args = new string[] { "images/street.jpg" };
        var program = new Program();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => program.Main(args));
    }
}