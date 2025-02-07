using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Azure.AI.Vision.ImageAnalysis;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace read_text.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void Main_WithValidInput_ShouldCallGetTextRead()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(config => config["AIServicesEndpoint"]).Returns("https://example.com");
            mockConfiguration.Setup(config => config["AIServicesKey"]).Returns("key");

            var mockClientFactory = new Mock<Func<ImageAnalysisClient>>();
            mockClientFactory.Setup(factory => factory()).Returns(new Mock<ImageAnalysisClient>().Object);

            var mockConsole = new Mock<Console>();

            var inputLines = new List<string> { "1", "" };
            var index = 0;
            mockConsole.SetupSequence(Console.ReadLine).Returns(() => inputLines[index++]);

            var program = new Program();
            program.ConfigureServices = services =>
            {
                services.AddSingleton(mockConfiguration.Object);
                services.AddSingleton(mockClientFactory.Object);
                services.AddSingleton(mockConsole.Object);
            };

            // Act
            program.Main(new string[] { });

            // Assert
            mockClientFactory.Verify(factory => factory(), Times.Once());
            mockConsole.Verify(console => console.WriteLine(It.IsAny<string>()), Times.AtLeastOnce());
        }

        [Fact]
        public void GetTextRead_WithValidImageFile_ShouldDisplayTextAndSaveImage()
        {
            // Arrange
            var mockClient = new Mock<ImageAnalysisClient>();
            mockClient.Setup(client => client.Analyze(
                It.IsAny<BinaryData>(),
                It.IsAny<VisualFeatures>())).Returns(new ImageAnalysisResult
                {
                    Read = new ReadResult
                    {
                        Blocks = new List<Block>
                        {
                            new Block
                            {
                                Lines = new List<Line>
                                {
                                    new Line
                                    {
                                        Text = "Hello World",
                                        BoundingPolygon = new List<PointF>
                                        {
                                            new PointF(0, 0),
                                            new PointF(100, 0),
                                            new PointF(100, 100),
                                            new PointF(0, 100)
                                        }
                                    }
                                }
                            }
                        }
                    }
                });

            var imageFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.jpg");
            File.WriteAllBytes(imageFile, new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }); // PNG header

            var program = new Program();

            // Act
            program.GetTextRead(imageFile, mockClient.Object);

            // Assert
            mockClient.Verify(client => client.Analyze(It.IsAny<BinaryData>(), It.IsAny<VisualFeatures>()), Times.Once());
            Assert.Contains("Hello World", Console.Out.ToString());
            File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "text.jpg")).ShouldBeTrue();
        }

        [Fact]
        public void GetTextRead_WithInvalidImageFile_ShouldHandleException()
        {
            // Arrange
            var mockClient = new Mock<ImageAnalysisClient>();
            mockClient.Setup(client => client.Analyze(
                It.IsAny<BinaryData>(),
                It.IsAny<VisualFeatures>())).Throws(new Exception("Invalid image file"));

            var imageFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "invalid_image.jpg");

            var program = new Program();

            // Act & Assert
            Assert.Throws<Exception>(() => program.GetTextRead(imageFile, mockClient.Object));
        }
    }
}