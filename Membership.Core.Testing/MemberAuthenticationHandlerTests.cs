using Membership.Core.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace Membership.Core.Testing
{
    public class MemberAuthenticationHandlerTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void BackendSuccessOrFailure_ReturnsSucessOrFailureCorrespondingly(
            bool backendSuccess, 
            bool expectAuthSuccess)
        {
            var mockLogger = new Mock<ILogger>();
            string error, clientId = "myId";
            Guid userId = Guid.NewGuid();
            var mockAuthService = new Mock<IMemberAuthenticationService>();
            if (backendSuccess)
                mockAuthService
                    .Setup(inst => inst.TryAuthenticate(It.IsAny<IHeaderDictionary>(), out error, out clientId, out userId))
                    .Returns(true);
            else
                mockAuthService
                    .Setup(inst => inst.TryAuthenticate(It.IsAny<IHeaderDictionary>(), out error, out clientId, out userId))
                    .Returns(false);

            var result = MemberAuthenticationHandler.HandleAuthenticateAsyncImpl(
                    mockLogger.Object,
                    mockAuthService.Object,
                    new HeaderDictionary(),
                    "MyScheme");
            if (expectAuthSuccess)
            {
                Assert.True(result.Succeeded);
                Assert.Contains(
                    result.Principal.Claims,
                    c => c.Type == CustomClaimTypes.OAuthClientId && c.Value == "myId");
                Assert.Contains(
                    result.Principal.Claims,
                    c => c.Type == CustomClaimTypes.OAuthUserId && c.Value == userId.ToString());
            }
            else
                Assert.False(result.Succeeded);
        }

        [Fact]
        public void HandleAuthenticateAsyncImpl_BackendThrowsException_AuthFailure()
        {
            var mockLogger = new Mock<ILogger>();
            string error, clientId = "myId";
            Guid userId;
            var mockAuthService = new Mock<IMemberAuthenticationService>();
            mockAuthService
                .Setup(inst => inst.TryAuthenticate(It.IsAny<IHeaderDictionary>(), out error, out clientId, out userId))
                .Callback(delegate
                {
                    throw new System.Exception("unhandled exception");
                });
            var result = MemberAuthenticationHandler.HandleAuthenticateAsyncImpl(
                    mockLogger.Object,
                    mockAuthService.Object,
                    new HeaderDictionary(),
                    "MyScheme");
            Assert.False(result.Succeeded);
        }
    }
}
