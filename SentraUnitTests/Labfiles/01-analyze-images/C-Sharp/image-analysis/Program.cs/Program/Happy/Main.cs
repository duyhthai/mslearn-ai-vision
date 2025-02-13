```csharp
using System;
using System.IO;
using System.Threading.Tasks;
using Azure.AI.Vision.ImageAnalysis;
using Azure.Core.TestFramework;
using NUnit.Framework;

namespace ImageProcessingTests
{
    public class ProgramTests : RecordedTestBase<ImageProcessingTestEnvironment>
    {
        [Test]
        public async Task Main_HappyPath()
        {
            // Arrange
            var mockClient = new MockImageAnalysisClient();
            var mockConfig = new Mock<IConfiguration>
            {
                { "AIServicesEndpoint", "https://mockendpoint.com" },
                { "AIServicesKey", "mockkey" }
            };

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            var program = new Program();

            // Act
            await program.Main(new string[] { });

            // Assert
            mockClient.VerifyAnalyzeImageCalled("images/street.jpg");
            mockClient.VerifyBackgroundForegroundCalled("images/street.jpg", "https://mockendpoint.com", "mockkey");
        }

        private class MockImageAnalysisClient : ImageAnalysisClient
        {
            public MockImageAnalysisClient() : base(new Uri("https://dummy"), new AzureKeyCredential("dummy"))
            {
            }

            public void VerifyAnalyzeImageCalled(string imageFile)
            {
                // Implement verification logic here
            }

            public void VerifyBackgroundForegroundCalled(string imageFile, string endpoint, string key)
            {
                // Implement verification logic here
            }
        }

        private class Mock<T> : T where T : class
        {
            public Dictionary<string, object> Values { get; } = new Dictionary<string, object>();

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                return base.ToString();
            }

            protected override T CreateInstance(Type type)
            {
                return null;
            }

            protected override T CreateInstance(Type type, Type[] types, object[] arguments)
            {
                return null;
            }

            protected override T CreateInstance(Type type, object[] arguments)
            {
                return null;
            }

            protected override T CreateInstance(Type type, BindingFlags bindingAttr, Binder binder, object[] parameters, CultureInfo culture)
            {
                return null;
            }

            protected override T CreateInstance(Type type, Type[] types, object[] values, ParameterModifier[] modifiers, CultureInfo culture)
            {
                return null;
            }

            protected override T CreateInstance(Type type, Type[] types, object[] values, ParameterModifier[] modifiers, CultureInfo culture, bool ignoreCase)
            {
                return null;
            }

            protected override T CreateInstance(Type type, Type[] types, object[] values, ParameterModifier[] modifiers, CultureInfo culture, bool ignoreCase, bool throwOnBindFailure)
            {
                return null;
            }

            protected override T CreateInstance(Type type, Type[] types, object[] values, ParameterModifier[] modifiers, CultureInfo culture, bool ignoreCase, bool throwOnBindFailure, bool checkAccess)
            {
                return null;
            }

            protected override T CreateInstance(Type type, Type[] types, object[] values, ParameterModifier[] modifiers, CultureInfo culture, bool ignoreCase, bool throwOnBindFailure, bool checkAccess, bool allowNonPublicConstructors)
            {
                return null;
            }

            protected override T CreateInstance(Type type, Type[] types, object[] values, ParameterModifier[] modifiers, CultureInfo culture, bool ignoreCase, bool throwOnBindFailure, bool checkAccess, bool allowNonPublicConstructors, bool useDefaultCtor)
            {
                return null;
            }

            protected override T CreateInstance(Type type, Type[] types, object[] values, ParameterModifier[] modifiers, CultureInfo culture, bool ignoreCase, bool throwOnBindFailure, bool checkAccess, bool allowNonPublicConstructors, bool useDefaultCtor, bool allowUnboundConstructors)
            {
                return null;
            }

            protected override T CreateInstance(Type type, Type[] types, object[] values, ParameterModifier[] modifiers, CultureInfo culture, bool ignoreCase, bool throwOnBindFailure, bool checkAccess, bool allowNonPublicConstructors, bool useDefaultCtor, bool allowUnboundConstructors, bool allowUnboundParameters)
            {
                return null;
            }

            protected override T CreateInstance(Type type, Type[] types, object[] values, ParameterModifier[] modifiers, CultureInfo culture, bool ignoreCase, bool throwOnBindFailure, bool checkAccess, bool allowNonPublicConstructors, bool useDefaultCtor, bool allowUnboundConstructors, bool allowUnboundParameters, bool allowUnboundTypes)
            {
                return null;
            }

            protected override T CreateInstance(Type type, Type[] types, object[] values, ParameterModifier[] modifiers, CultureInfo culture, bool ignoreCase, bool throwOnBindFailure, bool checkAccess, bool allowNonPublicConstructors, bool useDefaultCtor, bool