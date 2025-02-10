using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.AI.Vision.ImageAnalysis;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class ProgramTests
{
    [Fact]
    public async Task ShouldThrowArgumentExceptionWhenConfigSettingsAreMissing()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c.GetSection(It.IsAny<string>())).Returns(new Mock<IConfigurationSection>().Object);

        Environment.SetEnvironmentVariable("AIServicesEndpoint", null);
        Environment.SetEnvironmentVariable("AIServicesKey", null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            Program.Main(new string[] { }));
    }

    [Fact]
    public async Task ShouldThrowArgumentNullExceptionWhenImageFileIsNull()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c.GetSection(It.IsAny<string>())).Returns(new Mock<IConfigurationSection>().Object);

        Environment.SetEnvironmentVariable("AIServicesEndpoint", "https://example.com");
        Environment.SetEnvironmentVariable("AIServicesKey", "key");

        var mockClientFactory = new Mock<ImageAnalysisClient>(new Uri("https://example.com"), new AzureKeyCredential("key"));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            Program.GetTextRead(null, mockClientFactory.Object));
    }

    [Fact]
    public async Task ShouldThrowFileNotFoundExceptionWhenImageFileDoesNotExist()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c.GetSection(It.IsAny<string>())).Returns(new Mock<IConfigurationSection>().Object);

        Environment.SetEnvironmentVariable("AIServicesEndpoint", "https://example.com");
        Environment.SetEnvironmentVariable("AIServicesKey", "key");

        var mockClientFactory = new Mock<ImageAnalysisClient>(new Uri("https://example.com"), new AzureKeyCredential("key"));

        // Act & Assert
        await Assert.ThrowsAsync<FileNotFoundException>(() =>
            Program.GetTextRead("nonexistentfile.jpg", mockClientFactory.Object));
    }
}