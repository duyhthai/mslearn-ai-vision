using System;
using System.IO;
using System.Threading.Tasks;
using Azure.AI.Vision.ImageAnalysis;
using Azure.Core;
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
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://api.cognitive.microsoft.com/vision/v3.0");
        mockConfig.SetupGet(c => c["AIServicesKey"]).Returns("fakekey");

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

        var mockImageAnalysisClient = new Mock<ImageAnalysisClient>(new Uri(mockConfig.Object["AIServicesEndpoint"]), new AzureKeyCredential(mockConfig.Object["AIServicesKey"]));
        mockImageAnalysisClient.Setup(client => client.AnalyzeAsync(It.IsAny<string>(), It.IsAny<ImageAnalysisOptions>())).Returns(Task.CompletedTask);
        mockImageAnalysisClient.Setup(client => client.GenerateForegroundMatteAsync(It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.CompletedTask);

        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Test");
        var appSettingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        File.WriteAllText(appSettingsPath, $"{{ \"AIServicesEndpoint\": \"{mockConfig.Object["AIServicesEndpoint"]}\", \"AIServicesKey\": \"{mockConfig.Object["AIServicesKey"]}\" }}");

        var args = new string[] { "images/street.jpg" };

        // Act
        await Program.Main(args);

        // Assert
        mockConfig.VerifyAll();
        mockHttpClient.VerifyAll();
        mockImageAnalysisClient.VerifyAll();
    }

    [Fact]
    public async Task Main_WithNoArguments_ShouldUseDefaultImageFile()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://api.cognitive.microsoft.com/vision/v3.0");
        mockConfig.SetupGet(c => c["AIServicesKey"]).Returns("fakekey");

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

        var mockImageAnalysisClient = new Mock<ImageAnalysisClient>(new Uri(mockConfig.Object["AIServicesEndpoint"]), new AzureKeyCredential(mockConfig.Object["AIServicesKey"]));
        mockImageAnalysisClient.Setup(client => client.AnalyzeAsync(It.IsAny<string>(), It.IsAny<ImageAnalysisOptions>())).Returns(Task.CompletedTask);
        mockImageAnalysisClient.Setup(client => client.GenerateForegroundMatteAsync(It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.CompletedTask);

        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Test");
        var appSettingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        File.WriteAllText(appSettingsPath, $"{{ \"AIServicesEndpoint\": \"{mockConfig.Object["AIServicesEndpoint"]}\", \"AIServicesKey\": \"{mockConfig.Object["AIServicesKey"]}\" }}");

        var args = new string[] { };

        // Act
        await Program.Main(args);

        // Assert
        mockConfig.VerifyAll();
        mockHttpClient.VerifyAll();
        mockImageAnalysisClient.VerifyAll();
    }
}