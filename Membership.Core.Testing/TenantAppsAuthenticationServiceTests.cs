using Membership.Core.Authentication;
using Membership.Core.Data;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using Xunit;

namespace Membership.Core.Testing
{
    public class TenantAppsAuthenticationServiceTests
    {
        [Fact]
        void TryAuthenticate_NoAuthenticationHeader_ReturnsFalseWithErrorMessage()
        {
            var service = new TenantAppsAuthenticationService(
                Mock.Of<IOAuthClientRepository>());

            var headerDictionary = new HeaderDictionary();

            Assert.False(service.TryAuthenticate(headerDictionary, out string errorMessage, out _));
            Assert.NotNull(errorMessage);
        }

        [Theory]
        [InlineData("password", true)]
        [InlineData("password1", false)]
        [InlineData(" password", false)]
        [InlineData("password ", false)]
        void TryAuthenticate_ValidAuthentication_OnlyReturnsTrueWhenCorrectPasswordAndUsername(
            string clientProvidedPassword,
            bool expectedReturnValue)
        {
            var mockRepo = new Mock<IOAuthClientRepository>();
            mockRepo
                .Setup(inst => inst.Find("admin"))
                .Returns(new OAuthClientEntry
                {
                    Id = "admin",
                    Name = "Tenant Application",
                    Secret = new SHA256HashedString("password")
                });
            var headerDictionary = new HeaderDictionary
            {
                {
                    "authorization",
                    "Basic " + Convert.ToBase64String(
                        System.Text.Encoding.UTF8.GetBytes($"admin:{clientProvidedPassword}"))
                }
            };
            var service = new TenantAppsAuthenticationService(mockRepo.Object);
            Assert.True(expectedReturnValue == 
                service.TryAuthenticate(headerDictionary, out string errorMessage, out _));
        }

        [Theory]
        [InlineData(":hello")]
        [InlineData("hello")]
        [InlineData("hello:")]
        void TrySplit_InvalidColonPlacement_ReturnsFalse(string inputString)
        {
            Assert.False(TenantAppsAuthenticationService.TrySplit(inputString, out _, out _));
        }

        [Theory]
        [InlineData("a:b", "a", "b")]
        [InlineData("myadmin:myPassword", "myadmin", "myPassword")]
        void TrySplit_ValidColonPlacement_ReturnsTrueWithCorrectOutput(
            string inputString, 
            string expectedUsername, 
            string expectedPassword)
        {
            Assert.True(TenantAppsAuthenticationService.TrySplit(
                inputString, 
                out string actualUsername, 
                out string actualPassword));
            Assert.Equal(actualUsername, expectedUsername);
            Assert.Equal(actualPassword, expectedPassword);
        }
    }
}
