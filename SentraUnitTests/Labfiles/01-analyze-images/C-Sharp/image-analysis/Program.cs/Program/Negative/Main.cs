using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.AI.Vision.ImageAnalysis;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class ProgramTests
{
    [Fact]
    public async Task Main_ThrowsArgumentException_WhenInvalidArgumentProvided()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://example.com");
        mockConfiguration.SetupGet(c => c["AIServicesKey"]).Returns("key");

        var builder = new ConfigurationBuilder();
        builder.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "AIServicesEndpoint", "https://example.com" },
            { "AIServicesKey", "key" }
        });
        IConfigurationRoot configuration = builder.Build();

        string imageFile = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => Program.Main(new string[] { imageFile }));
    }

    [Fact]
    public async Task Main_ThrowsArgumentNullException_WhenNullConfigurationProvided()
    {
        // Arrange
        IConfigurationRoot configuration = null;

        string imageFile = "images/street.jpg";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => Program.Main(new string[] { imageFile }, configuration));
    }

    [Fact]
    public async Task Main_ThrowsAzureRequestFailedException_WhenAzureServiceFails()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://example.com");
        mockConfiguration.SetupGet(c => c["AIServicesKey"]).Returns("key");

        var builder = new ConfigurationBuilder();
        builder.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "AIServicesEndpoint", "https://example.com" },
            { "AIServicesKey", "key" }
        });
        IConfigurationRoot configuration = builder.Build();

        string imageFile = "images/street.jpg";

        var mockClient = new Mock<ImageAnalysisClient>(new Uri(mockConfiguration.Object["AIServicesEndpoint"]), new AzureKeyCredential(mockConfiguration.Object["AIServicesKey"]));
        mockClient.Setup(client => client.AnalyzeImageAsync(It.IsAny<string>())).ThrowsAsync(new Azure.RequestFailedException(400, "Bad Request"));

        // Act & Assert
        await Assert.ThrowsAsync<Azure.RequestFailedException>(() => Program.Main(new string[] { imageFile }, configuration, mockClient.Object));
    }
}