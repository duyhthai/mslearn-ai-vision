using Microsoft.Extensions.Configuration;
using NUnit.Framework;

[TestFixture]
public class ProgramTests
{
    [Test]
    public void Main_WithValidInput_ShouldCallGetTextRead()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(config => config["AIServicesEndpoint"]).Returns("https://example.com/vision");
        mockConfig.Setup(config => config["AIServicesKey"]).Returns("validKey");

        var mockConsole = new Mock<Console>();
        mockConsole.Setup(Console.WriteLine);

        var program = new Program();
        var originalConsole = Console.Out;
        Console.SetOut(mockConsole.Object);

        // Act
        program.Main(new string[] { });

        // Assert
        mockConfig.VerifyAll();
        mockConsole.Verify(console => console.WriteLine(It.IsAny<string>()), Times.AtLeastOnce());
        Console.SetOut(originalConsole);
    }

    [Test]
    public void Main_WithInvalidInput_ShouldNotCallGetTextRead()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(config => config["AIServicesEndpoint"]).Returns("invalidEndpoint");
        mockConfig.Setup(config => config["AIServicesKey"]).Returns("invalidKey");

        var mockConsole = new Mock<Console>();
        mockConsole.Setup(Console.WriteLine);

        var program = new Program();
        var originalConsole = Console.Out;
        Console.SetOut(mockConsole.Object);

        // Act
        program.Main(new string[] { });

        // Assert
        mockConfig.VerifyAll();
        mockConsole.Verify(console => console.WriteLine(It.IsAny<string>()), Times.Once());
        Console.SetOut(originalConsole);
    }

    [Test]
    public void Main_WithDefaultInput_ShouldNotCallGetTextRead()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(config => config["AIServicesEndpoint"]).Returns("defaultEndpoint");
        mockConfig.Setup(config => config["AIServicesKey"]).Returns("defaultKey");

        var mockConsole = new Mock<Console>();
        mockConsole.Setup(Console.WriteLine);

        var program = new Program();
        var originalConsole = Console.Out;
        Console.SetOut(mockConsole.Object);

        // Act
        program.Main(new string[] { "3" });

        // Assert
        mockConfig.VerifyAll();
        mockConsole.Verify(console => console.WriteLine(It.IsAny<string>()), Times.Once());
        Console.SetOut(originalConsole);
    }
}