using Membership.Core.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Membership.Core.Testing
{
    public class TenantAppsAuthenticationHandlerTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async void HandleAuthenticateAsync_BackendSuccessOrFailure_ReturnsSucessOrFailureCorrespondingly(
            bool backendSuccess,
            bool expectAuthSuccess)
        {
            var mockLogger = new Mock<ILogger>();

            string error, clientId = "myId";
            var mockAuthService = new Mock<ITenantAppsAuthenticationService>();
            if(backendSuccess)
                mockAuthService
                    .Setup(inst => inst.TryAuthenticate(It.IsAny<IHeaderDictionary>(), out error, out clientId))
                    .Returns(true);
            else
                mockAuthService
                    .Setup(inst => inst.TryAuthenticate(It.IsAny<IHeaderDictionary>(), out error, out clientId))
                    .Returns(false);
            
            var result = await TenantAppsAuthenticationHandler.HandleAuthenticateAsyncImpl(
                    mockLogger.Object,
                    new HeaderDictionary(),
                    mockAuthService.Object,
                    "MyScheme");
            if(expectAuthSuccess)
            {
                Assert.True(result.Succeeded);
                Assert.Contains(
                    result.Principal.Claims, 
                    c => c.Type == CustomClaimTypes.OAuthClientId && c.Value == "myId");
            }
            else
                Assert.False(result.Succeeded);
        }

        [Fact]
        public async void HandleAuthenticateAsync_BackendThrowsException_ReturnsFalse()
        {
            var mockLogger = new Mock<ILogger>();
            string error, clientId = "myId";
            var mockAuthService = new Mock<ITenantAppsAuthenticationService>();
            mockAuthService
                .Setup(inst => inst.TryAuthenticate(It.IsAny<IHeaderDictionary>(), out error, out clientId))
                .Callback(delegate
                {
                    throw new System.Exception("unhandled exception");
                });
            var result = await TenantAppsAuthenticationHandler.HandleAuthenticateAsyncImpl(
                    mockLogger.Object,
                    new HeaderDictionary(),
                    mockAuthService.Object,
                    "MyScheme");
            Assert.False(result.Succeeded);
        }
    }
}
