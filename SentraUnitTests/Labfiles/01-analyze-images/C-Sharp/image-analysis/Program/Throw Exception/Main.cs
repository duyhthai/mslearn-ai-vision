using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.AI.Vision.ImageAnalysis;
using Azure.Core;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace UnitTests
{
    public class ProgramTests
    {
        [Fact]
        public async Task Main_ThrowsExceptionOnInvalidConfiguration()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["AIServicesEndpoint"]).Returns((string)null);
            mockConfig.Setup(c => c["AIServicesKey"]).Returns((string)null);

            var mockBuilder = new Mock<IConfigurationBuilder>();
            mockBuilder.Setup(b => b.AddJsonFile("appsettings.json")).Returns(mockBuilder.Object);
            mockBuilder.Setup(b => b.Build()).Returns(mockConfig.Object);

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await Program.Main(new string[] { });
            });
        }

        [Fact]
        public async Task Main_ThrowsExceptionOnFailedHttpClientRequest()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["AIServicesEndpoint"]).Returns("https://example.com");
            mockConfig.Setup(c => c["AIServicesKey"]).Returns("key");

            var mockBuilder = new Mock<IConfigurationBuilder>();
            mockBuilder.Setup(b => b.AddJsonFile("appsettings.json")).Returns(mockBuilder.Object);
            mockBuilder.Setup(b => b.Build()).Returns(mockConfig.Object);

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

            using (var mockHttpClient = new Mock<HttpClient>())
            {
                mockHttpClient.Setup(h => h.GetAsync(It.IsAny<string>())).ThrowsAsync(new HttpRequestException());

                var clientFactoryMock = new Mock<IHttpClientFactory>();
                clientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

                var program = new Program();
                program.ClientFactory = clientFactoryMock.Object;
            }

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await Program.Main(new string[] { });
            });
        }

        [Fact]
        public async Task Main_ThrowsExceptionOnBackgroundForegroundFailure()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["AIServicesEndpoint"]).Returns("https://example.com");
            mockConfig.Setup(c => c["AIServicesKey"]).Returns("key");

            var mockBuilder = new Mock<IConfigurationBuilder>();
            mockBuilder.Setup(b => b.AddJsonFile("appsettings.json")).Returns(mockBuilder.Object);
            mockBuilder.Setup(b => b.Build()).Returns(mockConfig.Object);

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

            using (var mockHttpClient = new Mock<HttpClient>())
            {
                mockHttpClient.Setup(h => h.GetAsync(It.IsAny<string>())).ThrowsAsync(new HttpRequestException());

                var clientFactoryMock = new Mock<IHttpClientFactory>();
                clientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

                var program = new Program();
                program.ClientFactory = clientFactoryMock.Object;
            }

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await Program.Main(new string[] { });
            });
        }
    }
}