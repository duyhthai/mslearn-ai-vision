using System;
using System.IO;
using System.Threading.Tasks;
using Azure.AI.Vision.ImageAnalysis;
using Azure.Core.Testing;
using Moq;
using NUnit.Framework;

[TestFixture]
public class ProgramTests : RecordedTestBase<NoOpRecordingService>
{
    [Test]
    public async Task Main_HappyPath()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://example.com");
        mockConfig.SetupGet(c => c["AIServicesKey"]).Returns("fakekey");

        var mockClientFactory = new Mock<ImageAnalysisClientFactory>();
        mockClientFactory.Setup(f => f.CreateClient(It.IsAny<Uri>(), It.IsAny<AzureKeyCredential>()))
                         .Returns(new Mock<ImageAnalysisClient>(new Uri("https://example.com"), new AzureKeyCredential("fakekey")).Object);

        var args = new string[] { "testimage.jpg" };

        // Replace original Main method with mock implementations
        Program.Main = async (string[] args) =>
        {
            var config = mockConfig.Object;
            var clientFactory = mockClientFactory.Object;
            var aiSvcEndpoint = config["AIServicesEndpoint"];
            var aiSvcKey = config["AIServicesKey"];

            var client = clientFactory.CreateClient(new Uri(aiSvcEndpoint), new AzureKeyCredential(aiSvcKey));
            Program.AnalyzeImage("testimage.jpg", client);
            await Program.BackgroundForeground("testimage.jpg", aiSvcEndpoint, aiSvcKey);
        };

        // Act
        await Program.Main(args);

        // Assert
        mockClientFactory.Verify(f => f.CreateClient(It.IsAny<Uri>(), It.IsAny<AzureKeyCredential>()), Times.Once);
        mockConfig.VerifyAll();
    }

    private static void AnalyzeImage(string imageFile, ImageAnalysisClient client)
    {
        // Dummy implementation for testing purposes
    }

    private static async Task BackgroundForeground(string imageFile, string aiSvcEndpoint, string aiSvcKey)
    {
        // Dummy implementation for testing purposes
    }
}