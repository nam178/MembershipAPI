using Membership.Core.Authentication;
using Membership.Core.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using Xunit;

namespace Membership.Core.Testing
{
    public class MemberAuthenticationServiceTests
    {
        [Theory]
        [InlineData("expiredToken")]
        [InlineData("non-existing-token")]
        public void TryAuthenticate_NotValidToken_ReturnsFalse(string token)
        {
            MockUp(
                out Mock<ISystemClock> clock, 
                out Mock<IOAuthTokenRepository> tokens, 
                out Mock<IUserRepository> mockUsers
                );

            var service = new MemberAuthenticationService(
                tokens.Object,
                mockUsers.Object,
                clock.Object
                );
            Assert.False(service.TryAuthenticate(
                new HeaderDictionary { { HttpHeaderKey.Authorization, "Bearer " + token } },
                out string erri,
                out string clientId,
                out Guid userId
                ));
        }

        [Fact]
        public void TryAuthenticate_ValidToken_ReturnsTrue()
        {
            MockUp(
              out Mock<ISystemClock> clock,
              out Mock<IOAuthTokenRepository> tokens,
              out Mock<IUserRepository> mockUsers
              );

            var service = new MemberAuthenticationService(
                tokens.Object,
                mockUsers.Object,
                clock.Object
                );
            Assert.True(service.TryAuthenticate(
                new HeaderDictionary { { HttpHeaderKey.Authorization, "Bearer validToken" } },
                out string erri,
                out string clientId,
                out Guid userId
                ));

            Assert.Equal(Guid.Parse("bcab4e28-81d8-4911-b52c-259230a591db"), userId);
            Assert.Equal("requestingAppId", clientId);
        }

        static void MockUp(out Mock<ISystemClock> clock, out Mock<IOAuthTokenRepository> tokens, out Mock<IUserRepository> mockUsers)
        {
            var now = new DateTime(2018, 06, 03, 12, 13, 14, DateTimeKind.Utc);
            clock = new Mock<ISystemClock>();
            clock.Setup(x => x.UtcNow).Returns(new DateTimeOffset(now));

            tokens = new Mock<IOAuthTokenRepository>();
            tokens
                .Setup(inst => inst.FindToken("expiredToken"))
                .Returns(new OAuthTokenEntry
                {
                    Token = "expiredToken",
                    Expires = UnixTimestamp.FromDateTime(now.Subtract(TimeSpan.FromDays(30))),
                    UserId = Guid.Parse("bcab4e28-81d8-4911-b52c-259230a591db"),
                    OAuthClientId = "requestingAppId"
                });
            tokens
               .Setup(inst => inst.FindToken("validToken"))
               .Returns(new OAuthTokenEntry
               {
                   Token = "validToken",
                   Expires = UnixTimestamp.FromDateTime(now.Add(TimeSpan.FromMinutes(5))),
                   UserId = Guid.Parse("bcab4e28-81d8-4911-b52c-259230a591db"),
                   OAuthClientId = "requestingAppId"
               });

            mockUsers = new Mock<IUserRepository>();
            mockUsers
                .Setup(x => x.FindUserByUserId(Guid.Parse("bcab4e28-81d8-4911-b52c-259230a591db")))
                .Returns(new UserEntry
                {
                    Id = Guid.Parse("bcab4e28-81d8-4911-b52c-259230a591db"),
                    ClientId = "registeredAppId"
                });
        }
    }
}
