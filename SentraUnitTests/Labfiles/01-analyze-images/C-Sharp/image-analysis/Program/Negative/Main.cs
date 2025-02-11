using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Xunit;

public class ProgramTests
{
    [Fact]
    public async Task Main_ThrowsException_When_AIServicesEndpointIsInvalid()
    {
        // Arrange
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "AIServicesEndpoint", "invalid-url" },
                { "AIServicesKey", "valid-key" }
            })
            .Build();

        // Mocking the ImageAnalysisClient constructor
        var mockClientFactory = new Mock<IHttpClientFactory>();
        mockClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

        // Replace the actual ImageAnalysisClient with the mock
        var mockClient = new Mock<ImageAnalysisClient>(new Uri(configuration["AIServicesEndpoint"]), new AzureKeyCredential(configuration["AIServicesKey"]));
        mockClient.Setup(c => c.AnalyzeImageAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("Request failed"));

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => Program.Main(new string[] { }));
    }

    [Fact]
    public async Task Main_ThrowsException_When_AIServicesKeyIsInvalid()
    {
        // Arrange
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "AIServicesEndpoint", "https://example.com" },
                { "AIServicesKey", "" }
            })
            .Build();

        // Mocking the ImageAnalysisClient constructor
        var mockClientFactory = new Mock<IHttpClientFactory>();
        mockClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

        // Replace the actual ImageAnalysisClient with the mock
        var mockClient = new Mock<ImageAnalysisClient>(new Uri(configuration["AIServicesEndpoint"]), new AzureKeyCredential(configuration["AIServicesKey"]));
        mockClient.Setup(c => c.AnalyzeImageAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("Request failed"));

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => Program.Main(new string[] { }));
    }

    [Fact]
    public void Main_ThrowsException_When_ImageFileDoesNotExist()
    {
        // Arrange
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "AIServicesEndpoint", "https://example.com" },
                { "AIServicesKey", "valid-key" }
            })
            .Build();

        // Mocking the ImageAnalysisClient constructor
        var mockClientFactory = new Mock<IHttpClientFactory>();
        mockClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

        // Replace the actual ImageAnalysisClient with the mock
        var mockClient = new Mock<ImageAnalysisClient>(new Uri(configuration["AIServicesEndpoint"]), new AzureKeyCredential(configuration["AIServicesKey"]));
        mockClient.Setup(c => c.AnalyzeImageAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>())).ThrowsAsync(new FileNotFoundException("The file was not found."));

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => Program.Main(new string[] { "nonexistentfile.jpg" }));
    }
}