using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

[TestFixture]
public class ProgramTests
{
    [Test]
    public void Main_ReadAPI_CallsCorrectly()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://example.com");
        mockConfig.SetupGet(c => c["AIServicesKey"]).Returns("key");

        var mockImageAnalysisClient = new Mock<ImageAnalysisClient>(new Uri(mockConfig.Object["AIServicesEndpoint"]), new AzureKeyCredential(mockConfig.Object["AIServicesKey"]));
        mockImageAnalysisClient.Setup(client => client.ReadAsync(It.IsAny<string>(), It.IsAny<ReadOptions>())).Returns(Task.FromResult(new ReadResult()));

        IConfigurationRoot configuration = mockConfig.Object;
        Program.GetTextRead = (imageFile, client) => { };

        // Act
        Console.SetIn(new StringReader("1"));
        Program.Main(new string[] { });

        // Assert
        mockImageAnalysisClient.Verify(client => client.ReadAsync("images/Lincoln.jpg", null), Times.Once);
    }

    [Test]
    public void Main_Handwriting_CallsCorrectly()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://example.com");
        mockConfig.SetupGet(c => c["AIServicesKey"]).Returns("key");

        var mockImageAnalysisClient = new Mock<ImageAnalysisClient>(new Uri(mockConfig.Object["AIServicesEndpoint"]), new AzureKeyCredential(mockConfig.Object["AIServicesKey"]));
        mockImageAnalysisClient.Setup(client => client.ReadAsync(It.IsAny<string>(), It.IsAny<ReadOptions>())).Returns(Task.FromResult(new ReadResult()));

        IConfigurationRoot configuration = mockConfig.Object;
        Program.GetTextRead = (imageFile, client) => { };

        // Act
        Console.SetIn(new StringReader("2"));
        Program.Main(new string[] { });

        // Assert
        mockImageAnalysisClient.Verify(client => client.ReadAsync("images/Note.jpg", null), Times.Once);
    }

    [Test]
    public void Main_InvalidInput_DoesNotCallClient()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(c => c["AIServicesEndpoint"]).Returns("https://example.com");
        mockConfig.SetupGet(c => c["AIServicesKey"]).Returns("key");

        var mockImageAnalysisClient = new Mock<ImageAnalysisClient>(new Uri(mockConfig.Object["AIServicesEndpoint"]), new AzureKeyCredential(mockConfig.Object["AIServicesKey"]));
        mockImageAnalysisClient.Setup(client => client.ReadAsync(It.IsAny<string>(), It.IsAny<ReadOptions>())).Returns(Task.FromResult(new ReadResult()));

        IConfigurationRoot configuration = mockConfig.Object;
        Program.GetTextRead = (imageFile, client) => { };

        // Act
        Console.SetIn(new StringReader("3"));
        Program.Main(new string[] { });

        // Assert
        mockImageAnalysisClient.VerifyNoOtherCalls();
    }
}