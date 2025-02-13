using System;
using System.IO;
using System.Threading.Tasks;
using Azure.AI.Vision.ImageAnalysis;
using Azure.Core;
using Moq;
using Xunit;

public class ProgramTests
{
    [Fact]
    public async Task Main_WithValidArguments_ShouldAnalyzeAndProcessImage()
    {
        // Arrange
        var mockClient = new Mock<ImageAnalysisClient>();
        var mockConfiguration = new Mock<IConfiguration>();

        mockConfiguration.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://fakeendpoint.cognitiveservices.azure.com/");
        mockConfiguration.SetupGet(c => c["AIServicesKey"]).Returns("fakekey");

        var args = new string[] { "images/street.jpg" };

        // Set up mock client methods
        mockClient.Setup(client => client.AnalyzeAsync(It.IsAny<Uri>(), It.IsAny<string>()))
                 .Returns(Task.FromResult(new AnalyzeResult()));

        mockClient.Setup(client => client.CreateBackgroundRemovalJobAsync(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                 .Returns(Task.FromResult(new BackgroundRemovalJob()));

        // Replace original configuration and client creation with mocks
        var program = new Program();
        typeof(Program).GetProperty("_configuration", BindingFlags.NonPublic | BindingFlags.Instance)
                        ?.SetValue(program, mockConfiguration.Object);

        typeof(Program).GetProperty("_clientFactory", BindingFlags.NonPublic | BindingFlags.Static)
                       ?.SetValue(null, () => mockClient.Object);

        // Act
        await program.Main(args);

        // Assert
        mockClient.Verify(client => client.AnalyzeAsync(It.IsAny<Uri>(), It.IsAny<string>()), Times.Once);
        mockClient.Verify(client => client.CreateBackgroundRemovalJobAsync(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public async Task Main_WithoutArguments_ShouldUseDefaultImagePath()
    {
        // Arrange
        var mockClient = new Mock<ImageAnalysisClient>();
        var mockConfiguration = new Mock<IConfiguration>();

        mockConfiguration.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://fakeendpoint.cognitiveservices.azure.com/");
        mockConfiguration.SetupGet(c => c["AIServicesKey"]).Returns("fakekey");

        var args = new string[] { };

        // Set up mock client methods
        mockClient.Setup(client => client.AnalyzeAsync(It.IsAny<Uri>(), It.IsAny<string>()))
                 .Returns(Task.FromResult(new AnalyzeResult()));

        mockClient.Setup(client => client.CreateBackgroundRemovalJobAsync(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                 .Returns(Task.FromResult(new BackgroundRemovalJob()));

        // Replace original configuration and client creation with mocks
        var program = new Program();
        typeof(Program).GetProperty("_configuration", BindingFlags.NonPublic | BindingFlags.Instance)
                        ?.SetValue(program, mockConfiguration.Object);

        typeof(Program).GetProperty("_clientFactory", BindingFlags.NonPublic | BindingFlags.Static)
                       ?.SetValue(null, () => mockClient.Object);

        // Act
        await program.Main(args);

        // Assert
        mockClient.Verify(client => client.AnalyzeAsync(It.Is<Uri>(u => u.AbsolutePath == "/images/street.jpg"), It.IsAny<string>()), Times.Once);
        mockClient.Verify(client => client.CreateBackgroundRemovalJobAsync(It.Is<Uri>(u => u.AbsolutePath == "/images/street.jpg"), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
    }
}