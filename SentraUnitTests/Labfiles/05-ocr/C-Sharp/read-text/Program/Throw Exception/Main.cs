using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

[TestFixture]
public class ProgramTests
{
    [Test]
    public void Main_ThrowsException_WhenConfigSettingsMissing()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(c => c["AIServicesEndpoint"]).Returns((string)null);
        mockConfiguration.SetupGet(c => c["AIServicesKey"]).Returns((string)null);

        var mockBuilder = new Mock<IConfigurationBuilder>();
        mockBuilder.Setup(b => b.AddJsonFile("appsettings.json")).Returns(mockBuilder.Object);
        mockBuilder.Setup(b => b.Build()).Returns(mockConfiguration.Object);

        IConfigurationRoot configuration = mockBuilder.Object.Build();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
        {
            Program.Main(new string[0]);
        });
    }

    [Test]
    public void Main_ThrowsException_WhenImageFileNotFound()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://example.com");
        mockConfiguration.SetupGet(c => c["AIServicesKey"]).Returns("key");

        var mockBuilder = new Mock<IConfigurationBuilder>();
        mockBuilder.Setup(b => b.AddJsonFile("appsettings.json")).Returns(mockBuilder.Object);
        mockBuilder.Setup(b => b.Build()).Returns(mockConfiguration.Object);

        IConfigurationRoot configuration = mockBuilder.Object.Build();

        // Mocking file system
        var mockFileSystem = new Mock<IFileSystem>();
        mockFileSystem.Setup(fs => fs.File.Exists(It.IsAny<string>())).Returns(false);

        // Replace original File.Exists with mock
        var originalFileExists = File.Exists;
        File.Exists = (path) => mockFileSystem.Object.File.Exists(path);

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() =>
        {
            Program.Main(new string[0]);
        });

        // Restore original File.Exists
        File.Exists = originalFileExists;
    }

    [Test]
    public void Main_ThrowsException_WhenAzureAIInitializationFails()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://example.com");
        mockConfiguration.SetupGet(c => c["AIServicesKey"]).Returns("key");

        var mockBuilder = new Mock<IConfigurationBuilder>();
        mockBuilder.Setup(b => b.AddJsonFile("appsettings.json")).Returns(mockBuilder.Object);
        mockBuilder.Setup(b => b.Build()).Returns(mockConfiguration.Object);

        IConfigurationRoot configuration = mockBuilder.Object.Build();

        // Mocking Azure AI initialization failure
        var mockImageAnalysisClient = new Mock<ImageAnalysisClient>(new Uri(configuration["AIServicesEndpoint"]), new AzureKeyCredential(configuration["AIServicesKey"]));
        mockImageAnalysisClient.Setup(client => client.AnalyzeAsync(It.IsAny<string>(), It.IsAny<AnalyzeOptions>())).Throws(new Exception("Initialization failed"));

        // Replace original ImageAnalysisClient with mock
        var originalImageAnalysisClient = ImageAnalysisClient;
        ImageAnalysisClient = (endpoint, credential) => mockImageAnalysisClient.Object;

        // Act & Assert
        Assert.Throws<Exception>(() =>
        {
            Program.Main(new string[0]);
        });

        // Restore original ImageAnalysisClient
        ImageAnalysisClient = originalImageAnalysisClient;
    }
}