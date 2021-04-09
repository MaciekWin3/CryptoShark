using System;
using Xunit;
using Bunit;
using CryptoShark.BlazorServer.Shared;

namespace CryptoShark.Tests
{
    public class BunitTests : TestContext
    {
        [Fact]
        public void Test1()
        {
            // Act
            var cut = RenderComponent<TestComponent>();

            // Assert
            cut.MarkupMatches("<h3>TestComponent</h3>");
        }

        [Fact]
        public void ApiCallSuccess()
        { 
        }
    }
}
