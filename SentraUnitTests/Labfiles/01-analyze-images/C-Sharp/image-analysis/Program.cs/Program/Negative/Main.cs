using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

[TestFixture]
public class ProgramTests
{
    [Test]
    public void Main_InvalidConfigSettingsThrowsException()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(c => c["AIServicesEndpoint"]).Returns((string)null);
        mockConfiguration.SetupGet(c => c["AIServicesKey"]).Returns((string)null);

        IConfigurationBuilder builder = new ConfigurationBuilder();
        builder.AddMockConfiguration(mockConfiguration.Object);
        IConfigurationRoot configuration = builder.Build();

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await Program.Main(new string[] { });
        });
    }

    [Test]
    public void Main_InvalidImagePathThrowsFileNotFoundException()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://example.com");
        mockConfiguration.SetupGet(c => c["AIServicesKey"]).Returns("key");

        IConfigurationBuilder builder = new ConfigurationBuilder();
        builder.AddMockConfiguration(mockConfiguration.Object);
        IConfigurationRoot configuration = builder.Build();

        // Act & Assert
        Assert.ThrowsAsync<FileNotFoundException>(async () =>
        {
            await Program.Main(new string[] { "nonexistentimage.jpg" });
        });
    }

    [Test]
    public void Main_NullArgumentThrowsArgumentNullException()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://example.com");
        mockConfiguration.SetupGet(c => c["AIServicesKey"]).Returns("key");

        IConfigurationBuilder builder = new ConfigurationBuilder();
        builder.AddMockConfiguration(mockConfiguration.Object);
        IConfigurationRoot configuration = builder.Build();

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await Program.Main(null);
        });
    }
}