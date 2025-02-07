using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.AI.Vision.ImageAnalysis;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace image_analysis.Tests
{
    public class ProgramTests
    {
        [Fact]
        public async Task AnalyzeImage_ShouldDisplayCorrectAnalysisResults()
        {
            // Arrange
            var mockClient = new Mock<ImageAnalysisClient>();
            var mockConfiguration = new Mock<IConfigurationRoot>();
            mockConfiguration.SetupGet(c => c["AIServicesEndpoint"]).Returns("http://mockendpoint");
            mockConfiguration.SetupGet(c => c["AIServicesKey"]).Returns("mockkey");
            var program = new Program();

            var imageFile = Path.Combine(AppContext.BaseDirectory, "test_images", "street.jpg");

            // Mock the Analyze method
            mockClient.Setup(client => client.Analyze(It.IsAny<BinaryData>(), It.IsAny<VisualFeatures>()))
                      .Returns(new ImageAnalysisResult
                      {
                          Caption = new DenseCaption { Text = "Street scene", Confidence = 0.95 },
                          DenseCaptions = new Dictionary<string, DenseCaption>
                          {
                              { "0", new DenseCaption { Text = "City street", Confidence = 0.85 } }
                          },
                          Tags = new Dictionary<string, DetectedTag>
                          {
                              { "0", new DetectedTag { Name = "Street", Confidence = 0.90 } }
                          },
                          Objects = new Dictionary<string, DetectedObject>
                          {
                              { "0", new DetectedObject { BoundingBox = new Rectangle(10, 10, 100, 100) } }
                          }
                      });

            // Mock the File.Exists method
            File.Exists = _ => true;

            // Act
            await program.Main(new string[] { imageFile });

            // Assert
            var output = Console.Out.ToString();
            Assert.Contains(" Caption:", output);
            Assert.Contains(" Dense Captions:", output);
            Assert.Contains(" Tags:", output);
            Assert.Contains(" Objects:", output);
        }

        [Fact]
        public async Task BackgroundForeground_ShouldSaveBackgroundRemovedImage()
        {
            // Arrange
            var mockHttpClient = new Mock<HttpClient>();
            var mockConfiguration = new Mock<IConfigurationRoot>();
            mockConfiguration.SetupGet(c => c["AIServicesEndpoint"]).Returns("http://mockendpoint");
            mockConfiguration.SetupGet(c => c["AIServicesKey"]).Returns("mockkey");
            var program = new Program();

            var imageFile = Path.Combine(AppContext.BaseDirectory, "test_images", "street.jpg");

            // Mock the PostAsync method
            mockHttpClient.Setup(client => client.PostAsync(It.IsAny<Uri>(), It.IsAny<StringContent>()))
                         .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                         {
                             Content = new ByteArrayContent(File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "test_images", "background.png")))
                         });

            // Mock the HttpClientFactory
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(factory => factory.CreateClient())
                                 .Returns(mockHttpClient.Object);

            // Replace the default HttpClient with the mocked one
            var originalCreateClient = Program.CreateClient;
            Program.CreateClient = () => httpClientFactoryMock.Object.CreateClient();

            // Act
            await program.BackgroundForeground(imageFile, "http://mockendpoint", "mockkey");

            // Assert
            var output = Console.Out.ToString();
            Assert.Contains("  Results saved in background.png\n", output);
            Assert.True(File.Exists("background.png"));

            // Restore the original CreateClient method
            Program.CreateClient = originalCreateClient;
        }

        [Fact]
        public async Task Main_ShouldHandleExceptionAndPrintErrorMessage()
        {
            // Arrange
            var mockClient = new Mock<ImageAnalysisClient>();
            var mockConfiguration = new Mock<IConfigurationRoot>();
            mockConfiguration.SetupGet(c => c["AIServicesEndpoint"]).Returns("http://mockendpoint");
            mockConfiguration.SetupGet(c => c["AIServicesKey"]).Returns("mockkey");
            var program = new Program();

            var imageFile = Path.Combine(AppContext.BaseDirectory, "test_images", "nonexistent.jpg");

            // Mock the Analyze method to throw an exception
            mockClient.Setup(client => client.Analyze(It.IsAny<BinaryData>(), It.IsAny<VisualFeatures>()))
                      .Throws(new Exception("Test exception"));

            // Mock the File.Exists method
            File.Exists = _ => false;

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => program.Main(new string[] { imageFile }));
        }
    }
}