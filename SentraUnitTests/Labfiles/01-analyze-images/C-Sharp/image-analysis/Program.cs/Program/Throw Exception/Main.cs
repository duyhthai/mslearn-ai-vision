using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.AI.Vision.ImageAnalysis;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class ProgramTests
{
    [Fact]
    public async Task Main_ThrowsExceptionIfAppSettingsMissing()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["AIServicesEndpoint"]).Returns((string)null);
        mockConfig.Setup(c => c["AIServicesKey"]).Returns((string)null);

        IConfigurationBuilder builder = new ConfigurationBuilder();
        builder.AddMock(mockConfig.Object);
        IConfigurationRoot configuration = builder.Build();

        var args = new string[] { };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            Program.Main(args)
        );
    }

    [Fact]
    public async Task Main_ThrowsExceptionIfAzureClientInitializationFails()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["AIServicesEndpoint"]).Returns("https://invalidendpoint.cognitiveservices.azure.com/");
        mockConfig.Setup(c => c["AIServicesKey"]).Returns("invalidkey");

        IConfigurationBuilder builder = new ConfigurationBuilder();
        builder.AddMock(mockConfig.Object);
        IConfigurationRoot configuration = builder.Build();

        var args = new string[] { };

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            Program.Main(args)
        );
    }

    [Fact]
    public async Task Main_ThrowsExceptionIfBackgroundForegroundOperationFails()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["AIServicesEndpoint"]).Returns("https://validendpoint.cognitiveservices.azure.com/");
        mockConfig.Setup(c => c["AIServicesKey"]).Returns("validkey");

        IConfigurationBuilder builder = new ConfigurationBuilder();
        builder.AddMock(mockConfig.Object);
        IConfigurationRoot configuration = builder.Build();

        var args = new string[] { "nonexistentimage.jpg" };

        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClient.Setup(h => h.GetAsync(It.IsAny<string>())).ThrowsAsync(new HttpRequestException());

        var mockImageAnalysisClient = new Mock<ImageAnalysisClient>(new Uri(mockConfig.Object["AIServicesEndpoint"]), new AzureKeyCredential(mockConfig.Object["AIServicesKey"]));
        mockImageAnalysisClient.Setup(client => client.BackgroundForegroundAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(default(ImageAnalysisResult)));

        using (var stream = File.OpenRead("path/to/nonexistentimage.jpg"))
        {
            mockImageAnalysisClient.Setup(client => client.ReadAsync(stream)).Returns(Task.FromResult(new ImageAnalysisResult()));
        }

        Program.client = mockImageAnalysisClient.Object;
        Program.httpClient = mockHttpClient.Object;

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            Program.Main(args)
        );
    }
}