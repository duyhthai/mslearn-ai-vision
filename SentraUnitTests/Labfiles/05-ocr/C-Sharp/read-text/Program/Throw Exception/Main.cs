using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class ProgramTests
{
    [Fact]
    public async Task Main_ThrowsException_WhenConfigSettingsAreMissing()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c.GetSection(It.IsAny<string>())).Returns(new Mock<IConfigurationSection>().Object);
        mockConfiguration.Setup(c => c["AIServicesEndpoint"]).Returns((string)null);
        mockConfiguration.Setup(c => c["AIServicesKey"]).Returns((string)null);

        var mockBuilder = new Mock<IConfigurationBuilder>();
        mockBuilder.Setup(b => b.AddJsonFile(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                   .Returns(mockConfiguration.Object);

        var program = new Program();
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            program.Main(new string[] { }));
    }

    [Fact]
    public async Task Main_ThrowsException_WhenAzureAIVisionClientFailsToAuthenticate()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c.GetSection(It.IsAny<string>())).Returns(new Mock<IConfigurationSection>().Object);
        mockConfiguration.Setup(c => c["AIServicesEndpoint"]).Returns("https://example.com");
        mockConfiguration.Setup(c => c["AIServicesKey"]).Returns("invalidkey");

        var mockBuilder = new Mock<IConfigurationBuilder>();
        mockBuilder.Setup(b => b.AddJsonFile(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                   .Returns(mockConfiguration.Object);

        var mockImageAnalysisClientFactory = new Mock<ImageAnalysisClientFactory>();
        mockImageAnalysisClientFactory.Setup(f => f.CreateClient(It.IsAny<Uri>(), It.IsAny<AzureKeyCredential>()))
                                      .ThrowsAsync(new InvalidOperationException());

        var program = new Program
        {
            _imageAnalysisClientFactory = mockImageAnalysisClientFactory.Object
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            program.Main(new string[] { }));
    }
}