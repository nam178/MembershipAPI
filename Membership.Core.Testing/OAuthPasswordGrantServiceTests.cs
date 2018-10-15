using Membership.Core.Authentication;
using Membership.Core.Data;
using Microsoft.AspNetCore.Authentication;
using Moq;
using System;
using Xunit;

namespace Membership.Core.Testing
{
    public class OAuthPasswordGrantServiceTests
    {
        [Theory]
        [InlineData("wrongUsername", "correctPassword")]
        [InlineData("correctUsername", "wrongPassword")]
        public void TryGrant_InvalidusernameOrPassword_ReturnsFalseWithError(
            string username, 
            string password)
        {
            var mockUsers = MockUsers(isActiveUser: true);
            var mockOAuthTokens = new Mock<IOAuthTokenRepository>();
            var mockDeactivationService = new Mock<IDeactivationService>();
            var mockCurrentUserService = new Mock<ICurrentUserService>();
            mockCurrentUserService.Setup(x => x.AuthenticatedClientId).Returns("myAppId");
            var service = new OAuthPasswordGrantService(
                Mock.Of<ISystemClock>(),
                mockUsers.Object,
                mockOAuthTokens.Object,
                mockDeactivationService.Object,
                mockCurrentUserService.Object
                );
            Assert.False(service.TryGrant(new OAuthPasswordGrantRequest
            {
                Username = username,
                Password = password
            }, out string err, out _, out _));
            Assert.NotNull(err);
            // No token must be created on failure
            mockOAuthTokens
                .Verify(i => i.AddToken(It.IsAny<OAuthTokenEntry>()), Times.Never);
        }

        [Fact]
        public void TryGrant_CorrectUsernamePasswordButInactive_ReturnsFalse()
        {
            var mockUsers = MockUsers(isActiveUser: false);
            var mockOAuthTokens = new Mock<IOAuthTokenRepository>();
            var mockDeactivationService = new Mock<IDeactivationService>();
            var mockCurrentUserService = new Mock<ICurrentUserService>();
            mockCurrentUserService.Setup(x => x.AuthenticatedClientId).Returns("myAppId");
            mockDeactivationService
                .Setup(x => x.IsUserDeactivated(It.IsAny<Guid>()))
                .Returns(true);
            var service = new OAuthPasswordGrantService(
                new Mock<ISystemClock>().Object,
                mockUsers.Object,
                mockOAuthTokens.Object,
                mockDeactivationService.Object,
                mockCurrentUserService.Object
                );
            Assert.False(service.TryGrant(new OAuthPasswordGrantRequest
            {
                Username = "correctUsername",
                Password = "correctPassword"
            }, out string err, out string token, out int expires));
            Assert.NotNull(err);
        }

        [Fact]
        public void TryGrant_CorrectUsernamePassword_ReturnsTrueWithTokenCreated()
        {
            var now = new DateTime(2018, 6, 3, 23, 18, 20, DateTimeKind.Utc);
            var clock = new Mock<ISystemClock>();
            clock.Setup(inst => inst.UtcNow).Returns(new DateTimeOffset(now));

            var mockDeactivationService = new Mock<IDeactivationService>();
            var mockUsers = MockUsers(isActiveUser: true);
            var mockOAuthTokens = new Mock<IOAuthTokenRepository>();
            var mockCurrentUserService = new Mock<ICurrentUserService>();
            mockCurrentUserService.Setup(x => x.AuthenticatedClientId).Returns("myAppId");
            var service = new OAuthPasswordGrantService(
                clock.Object,
                mockUsers.Object,
                mockOAuthTokens.Object,
                mockDeactivationService.Object,
                mockCurrentUserService.Object
                );
            Assert.True(service.TryGrant(new OAuthPasswordGrantRequest
            {
                Username = "correctUsername",
                Password = "correctPassword"
            }, out string err, out string token, out int expires));

            // A token must be created
            mockOAuthTokens
                .Verify(
                    i => i.AddToken(It.Is<OAuthTokenEntry>(t => t.Token == token 
                        && t.OAuthClientId == "myAppId"
                        && t.Expires > UnixTimestamp.FromDateTime(now))),
                    Times.Once
                    );
        }

        static Mock<IUserRepository> MockUsers(bool isActiveUser)
        {
            var mockUsers = new Mock<IUserRepository>();
            mockUsers
                .Setup(i => i.FindUserByName("correctUsername"))
                .Returns(new UserEntry
                {
                    ClientId = "correctClientId",
                    Username = "correctUsername",
                    Password = new SHA256HashedString("correctPassword", "sal"),
                    PasswordSalt = "sal"
                });
            return mockUsers;
        }
    }
}
