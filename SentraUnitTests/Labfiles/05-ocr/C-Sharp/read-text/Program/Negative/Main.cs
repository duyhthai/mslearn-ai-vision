using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace ProgramTests
{
    public class ProgramTests
    {
        [Fact]
        public async Task ShouldThrowException_WhenInvalidConfig()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["AIServicesEndpoint"]).Returns((string)null);
            mockConfiguration.Setup(c => c["AIServicesKey"]).Returns("somekey");

            var mockBuilder = new Mock<IConfigurationBuilder>();
            mockBuilder.Setup(b => b.AddJsonFile("appsettings.json")).Returns(mockConfiguration.Object);

            var program = new Program();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                program.Main(new string[0]));
        }

        [Fact]
        public async Task ShouldThrowException_WhenInvalidImageFile()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["AIServicesEndpoint"]).Returns("https://example.com");
            mockConfiguration.Setup(c => c["AIServicesKey"]).Returns("somekey");

            var mockBuilder = new Mock<IConfigurationBuilder>();
            mockBuilder.Setup(b => b.AddJsonFile("appsettings.json")).Returns(mockConfiguration.Object);

            var program = new Program();
            var imageFile = "nonexistentfile.jpg";

            // Act & Assert
            await Assert.ThrowsAsync<FileNotFoundException>(() =>
                program.GetTextRead(imageFile, null));
        }

        [Fact]
        public async Task ShouldThrowException_WhenAzureServiceFails()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["AIServicesEndpoint"]).Returns("https://example.com");
            mockConfiguration.Setup(c => c["AIServicesKey"]).Returns("somekey");

            var mockBuilder = new Mock<IConfigurationBuilder>();
            mockBuilder.Setup(b => b.AddJsonFile("appsettings.json")).Returns(mockConfiguration.Object);

            var mockClient = new Mock<ImageAnalysisClient>(new Uri("https://example.com"), new AzureKeyCredential("somekey"));
            mockClient.Setup(client => client.AnalyzeImageInStreamAsync(It.IsAny<Stream>())).ThrowsAsync(new Exception("Test exception"));

            var program = new Program();
            var imageFile = "images/Lincoln.jpg";

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                program.GetTextRead(imageFile, mockClient.Object));
        }
    }
}