using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

[TestFixture]
public class ProgramTests
{
    [Test]
    public void Main_ThrowsException_WhenConfigurationIsMissing()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["AIServicesEndpoint"]).Returns((string)null);
        mockConfig.Setup(c => c["AIServicesKey"]).Returns((string)null);

        IConfigurationBuilder builder = new ConfigurationBuilder();
        builder.AddInMemoryCollection(mockConfig.Object.GetSection("").GetChildren());
        IConfigurationRoot configuration = builder.Build();

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => Program.Main(new string[] { }));
    }

    [Test]
    public async Task Main_ThrowsException_WhenHttpClientRequestFails()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["AIServicesEndpoint"]).Returns("https://example.com");
        mockConfig.Setup(c => c["AIServicesKey"]).Returns("key");

        IConfigurationBuilder builder = new ConfigurationBuilder();
        builder.AddInMemoryCollection(mockConfig.Object.GetSection("").GetChildren());
        IConfigurationRoot configuration = builder.Build();

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClient.Setup(h => h.GetAsync(It.IsAny<string>())).ThrowsAsync(new HttpRequestException());

        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

        using (var stream = File.OpenRead("images/street.jpg"))
        {
            var mockStreamContent = new Mock<HttpContent>();
            mockStreamContent.Setup(s => s.ReadAsStreamAsync()).ReturnsAsync(stream);

            mockHttpClient.Setup(h => h.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(
                new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = mockStreamContent.Object
                });
        }

        var program = new Program();
        program.ConfigureServices = services =>
        {
            services.AddSingleton<IHttpClientFactory>(mockHttpClientFactory.Object);
        };

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => program.Main(new string[] { }));
    }

    [Test]
    public async Task Main_ThrowsException_WhenImageAnalysisFails()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["AIServicesEndpoint"]).Returns("https://example.com");
        mockConfig.Setup(c => c["AIServicesKey"]).Returns("key");

        IConfigurationBuilder builder = new ConfigurationBuilder();
        builder.AddInMemoryCollection(mockConfig.Object.GetSection("").GetChildren());
        IConfigurationRoot configuration = builder.Build();

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClient.Setup(h => h.GetAsync(It.IsAny<string>())).ThrowsAsync(new HttpRequestException());

        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

        using (var stream = File.OpenRead("images/street.jpg"))
        {
            var mockStreamContent = new Mock<HttpContent>();
            mockStreamContent.Setup(s => s.ReadAsStreamAsync()).ReturnsAsync(stream);

            mockHttpClient.Setup(h => h.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(
                new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = mockStreamContent.Object
                });
        }

        var mockImageAnalysisClient = new Mock<ImageAnalysisClient>(new Uri("https://example.com"), new AzureKeyCredential("key"));
        mockImageAnalysisClient.Setup(client => client.AnalyzeImageAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Analyze failed"));

        var program = new Program();
        program.ConfigureServices = services =>
        {
            services.AddSingleton<IHttpClientFactory>(mockHttpClientFactory.Object);
            services.AddSingleton<ImageAnalysisClient>(mockImageAnalysisClient.Object);
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => program.Main(new string[] { }));
    }
}