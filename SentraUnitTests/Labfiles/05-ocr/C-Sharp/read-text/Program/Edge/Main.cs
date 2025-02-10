using Microsoft.Extensions.Configuration;
using NUnit.Framework;

[TestFixture]
public class ProgramTests
{
    [Test]
    public void Main_WithValidInput_ShouldAuthenticateAndProcessImage()
    {
        // Arrange
        var mockConfig = new Dictionary<string, string>
        {
            { "AIServicesEndpoint", "https://fakeendpoint.cognitiveservices.azure.com/" },
            { "AIServicesKey", "fakekey" }
        };
        var mockConfiguration = new ConfigurationBuilder()
            .AddInMemoryCollection(mockConfig)
            .Build();

        var mockClientFactory = new Mock<IImageAnalysisClientFactory>();
        var mockClient = new Mock<ImageAnalysisClient>(new Uri(mockConfig["AIServicesEndpoint"]), new AzureKeyCredential(mockConfig["AIServicesKey"]));
        mockClient.Setup(c => c.ReadAsync(It.IsAny<Uri>(), It.IsAny<ReadableStreamContent>())).Returns(Task.FromResult(new AnalyzeResult()));

        Program.Main(new string[] { "1" });

        // Assert
        mockClient.Verify(c => c.ReadAsync(It.IsAny<Uri>(), It.IsAny<ReadableStreamContent>()), Times.Once);
    }

    [Test]
    public void Main_WithInvalidInput_ShouldCatchAndDisplayException()
    {
        // Arrange
        var mockConfig = new Dictionary<string, string>
        {
            { "AIServicesEndpoint", "https://fakeendpoint.cognitiveservices.azure.com/" },
            { "AIServicesKey", "fakekey" }
        };
        var mockConfiguration = new ConfigurationBuilder()
            .AddInMemoryCollection(mockConfig)
            .Build();

        var mockClientFactory = new Mock<IImageAnalysisClientFactory>();
        var mockClient = new Mock<ImageAnalysisClient>(new Uri(mockConfig["AIServicesEndpoint"]), new AzureKeyCredential(mockConfig["AIServicesKey"]));
        mockClient.Setup(c => c.ReadAsync(It.IsAny<Uri>(), It.IsAny<ReadableStreamContent>())).Throws(new Exception("Fake exception"));

        // Act & Assert
        Assert.Throws<Exception>(() => Program.Main(new string[] { "3" }));
    }

    [Test]
    public void Main_WithEmptyCommand_ShouldNotProcessImage()
    {
        // Arrange
        var mockConfig = new Dictionary<string, string>
        {
            { "AIServicesEndpoint", "https://fakeendpoint.cognitiveservices.azure.com/" },
            { "AIServicesKey", "fakekey" }
        };
        var mockConfiguration = new ConfigurationBuilder()
            .AddInMemoryCollection(mockConfig)
            .Build();

        var mockClientFactory = new Mock<IImageAnalysisClientFactory>();
        var mockClient = new Mock<ImageAnalysisClient>(new Uri(mockConfig["AIServicesEndpoint"]), new AzureKeyCredential(mockConfig["AIServicesKey"]));
        mockClient.Setup(c => c.ReadAsync(It.IsAny<Uri>(), It.IsAny<ReadableStreamContent>())).Returns(Task.FromResult(new AnalyzeResult()));

        // Act
        Program.Main(new string[] { "" });

        // Assert
        mockClient.Verify(c => c.ReadAsync(It.IsAny<Uri>(), It.IsAny<ReadableStreamContent>()), Times.Never);
    }
}