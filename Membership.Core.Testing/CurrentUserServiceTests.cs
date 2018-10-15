using Membership.Core.Authentication;
using Membership.Core.Data;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Security.Claims;
using Xunit;

namespace Membership.Core.Testing
{
    public class CurrentUserServiceTests
    {
        [Fact]
        public void GetCurrentClientId_UserNotRegisterdInThisApplication_ReturnsClientIdFromRequestingApp()
        {
            var service = MockUp(authenticated: true);
            Assert.Equal("authenticatedAppId", service.AuthenticatedClientId);
        }

        [Fact]
        public void GetCurrentUser_Authenticated_ReturnsCorrectUsr()
        {
            var service = MockUp(authenticated: true);
            Assert.Equal("a2403474-cede-49c3-9e1e-45a9b0efa429", service.AuthenticatedUser.Id.ToString());
        }

        [Fact]
        public void AccessProperties_WhenNotAuthenticated_ThrowsInvalidOperation()
        {
            var service = MockUp(authenticated: false);
            Assert.Throws<InvalidOperationException>(() => service.AuthenticatedUser);
            Assert.Throws<InvalidOperationException>(() => service.AuthenticatedClientId);
        }

        static CurrentUserService MockUp(bool authenticated)
        {
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext
                .Setup(x => x.User)
                .Returns(delegate
                {
                    Claim[] claims;
                    if (authenticated)
                        claims = new Claim[] {
                            new Claim(CustomClaimTypes.OAuthUserId, "a2403474-cede-49c3-9e1e-45a9b0efa429"),
                            new Claim(CustomClaimTypes.OAuthClientId, "authenticatedAppId")
                        };
                    else
                        claims = new Claim[0];

                    return new ClaimsPrincipal(
                        new ClaimsIdentity[] {
                            new ClaimsIdentity(claims)
                        });
                });

            var mockHttpContextAccesor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccesor
                .Setup(x => x.HttpContext)
                .Returns(mockHttpContext.Object);

            var mockUsers = new Mock<IUserRepository>();
            mockUsers
                .Setup(x => x.FindUserByUserId(Guid.Parse("a2403474-cede-49c3-9e1e-45a9b0efa429")))
                .Returns(new UserEntry
                {
                    Id = Guid.Parse("a2403474-cede-49c3-9e1e-45a9b0efa429"),
                    ClientId = "registeredAppId"
                });

            var service = new CurrentUserService(
                mockHttpContextAccesor.Object,
                mockUsers.Object
                );
            return service;
        }
    }
}
