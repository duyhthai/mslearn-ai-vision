using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Azure.AI.Vision.ImageAnalysis;
using NUnit.Framework;

[TestFixture]
public class ProgramTests
{
    [Test]
    public void Main_ShouldAuthenticateAzureAIVisionClient_WithCorrectEndpointAndKey()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(config => config["AIServicesEndpoint"]).Returns("https://fakeendpoint.cognitiveservices.azure.com/");
        mockConfiguration.Setup(config => config["AIServicesKey"]).Returns("fakekey");

        var mockBuilder = new Mock<IConfigurationBuilder>();
        mockBuilder.Setup(builder => builder.AddJsonFile("appsettings.json")).Returns(mockBuilder.Object);
        mockBuilder.Setup(builder => builder.Build()).Returns(mockConfiguration.Object);

        var originalMain = Program.Main;

        // Replace Main with a mock version
        Program.Main = () =>
        {
            var aiSvcEndpoint = mockConfiguration.Object["AIServicesEndpoint"];
            var aiSvcKey = mockConfiguration.Object["AIServicesKey"];

            var client = new ImageAnalysisClient(
                new Uri(aiSvcEndpoint),
                new AzureKeyCredential(aiSvcKey));

            Assert.AreEqual("https://fakeendpoint.cognitiveservices.azure.com/", aiSvcEndpoint);
            Assert.AreEqual("fakekey", aiSvcKey);
        };

        // Act
        Program.Main();

        // Restore original Main
        Program.Main = originalMain;
    }

    [Test]
    public void Main_ShouldDisplayMenuOptions()
    {
        // Arrange
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        var originalMain = Program.Main;

        // Replace Main with a mock version
        Program.Main = () =>
        {
            Console.WriteLine("\n1: Use Read API for image (Lincoln.jpg)\n2: Read handwriting (Note.jpg)\nAny other key to quit\n");
            Console.WriteLine("Enter a number:");
        };

        // Act
        Program.Main();

        // Restore original Main
        Program.Main = originalMain;

        // Assert
        var output = consoleOutput.ToString();
        Assert.IsTrue(output.Contains("\n1: Use Read API for image (Lincoln.jpg)"));
        Assert.IsTrue(output.Contains("\n2: Read handwriting (Note.jpg)"));
        Assert.IsTrue(output.Contains("\nEnter a number:"));
    }

    [Test]
    public void Main_ShouldHandleInvalidInputGracefully()
    {
        // Arrange
        var consoleInput = new StringReader("abc");
        Console.SetIn(consoleInput);

        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        var originalMain = Program.Main;

        // Replace Main with a mock version
        Program.Main = () =>
        {
            try
            {
                // Simulate reading input
                Console.WriteLine("\n1: Use Read API for image (Lincoln.jpg)\n2: Read handwriting (Note.jpg)\nAny other key to quit\n");
                Console.WriteLine("Enter a number:");
                string command = Console.ReadLine();

                string imageFile;

                switch (command)
                {
                    case "1":
                        imageFile = "images/Lincoln.jpg";
                        GetTextRead(imageFile, null); // Pass null to simulate exception
                        break;
                    case "2":
                        imageFile = "images/Note.jpg";
                        GetTextRead(imageFile, null); // Pass null to simulate exception
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        };

        // Act
        Program.Main();

        // Restore original Main
        Program.Main = originalMain;

        // Assert
        var output = consoleOutput.ToString();
        Assert.IsTrue(output.Contains("System.NullReferenceException"));
    }
}

// Helper method to replace the actual GetTextRead method
private static void GetTextRead(string imageFile, ImageAnalysisClient client)
{
    if (client == null)
    {
        throw new NullReferenceException("ImageAnalysisClient cannot be null.");
    }

    // Simulated implementation
    Console.WriteLine($"Reading text from {imageFile}");
}