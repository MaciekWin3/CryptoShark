using Xunit;
using Bunit;
using CryptoShark.BlazorServer.Shared;

namespace Bunit.Docs.Samples
{
    public class HelloWorldImplicitContextTest : TestContext
    {
        [Fact]
        public void HelloWorldComponentRendersCorrectly()
        {
            // Act
            var cut = RenderComponent<TestComponent>();

            // Assert
            cut.MarkupMatches("<h1>Hello world from Blazor</h1>");
        }
    }
}