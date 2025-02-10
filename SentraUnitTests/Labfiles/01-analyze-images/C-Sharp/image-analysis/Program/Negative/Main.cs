using System;
using System.IO;
using System.Threading.Tasks;
using Azure.AI.Vision.ImageAnalysis;
using Azure.Core.TestFramework;
using NUnit.Framework;

public class ProgramTests : RecordedTestBase<NoOpHttpClientFactory>
{
    [Test]
    public async Task ShouldThrowExceptionForInvalidEndpoint()
    {
        // Arrange
        var client = new ImageAnalysisClient(
            new Uri("invalid-endpoint"),
            new AzureKeyCredential("key"));

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => BackgroundForeground("image.jpg", "endpoint", "key"));
    }

    [Test]
    public async Task ShouldThrowExceptionForEmptyKey()
    {
        // Arrange
        var client = new ImageAnalysisClient(
            new Uri("endpoint"),
            new AzureKeyCredential(string.Empty));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => BackgroundForeground("image.jpg", "endpoint", string.Empty));
    }

    [Test]
    public void ShouldThrowExceptionForNonExistentImageFile()
    {
        // Arrange
        var client = new ImageAnalysisClient(
            new Uri("endpoint"),
            new AzureKeyCredential("key"));

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => AnalyzeImage("non-existent-file.jpg", client));
    }
}

public static class Extensions
{
    public static void AnalyzeImage(this ImageAnalysisClient client, string imageFile, ImageAnalysisClient _)
    {
        // Simulate image analysis
    }

    public static async Task BackgroundForeground(this string imageFile, string endpoint, string key)
    {
        // Simulate background/foreground operation
        await Task.Delay(1);
    }
}