using Membership.Core.Authentication;
using Membership.Core.Data;
using Membership.Core.Membership;
using Moq;
using System;
using Xunit;

namespace Membership.Core.Testing
{
    public class CreateUserServiceTests
    {
        [Fact]
        public void TryCreateUser_NotAuthenticated_ThrowsException()
        {
            var mockUsers = new Mock<IUserRepository>();
            var mockCurrentUsers = new Mock<ICurrentUserService>();
            mockCurrentUsers.Setup(x => x.AuthenticatedClientId).Throws<InvalidOperationException>();

            var service = new CreateUserService(
                mockUsers.Object,
                new PasswordPolicySimple(),
                mockCurrentUsers.Object
                );

            Assert.Throws<InvalidOperationException>(delegate
            {
                service.TryCreateUser(new CreateUserRequest {
                    FirstName = "foo", LastName = "bar",
                    Email = "foo@gmail.com",
                    PlainPassword = "foobar1",
                    Username = "foobar"
                }, out _, out _);
            });
        }

        [Fact]
        public void TryCreateUser_ValidUser_ReturnsTrue()
        {
            var mockUsers = new Mock<IUserRepository>();
            var mockCurrentUsers = new Mock<ICurrentUserService>();
            mockCurrentUsers
                .Setup(x => x.AuthenticatedClientId)
                .Returns("foobar");

            var service = new CreateUserService(
                mockUsers.Object,
                new PasswordPolicySimple(),
                mockCurrentUsers.Object
                );

            Assert.True(service.TryCreateUser(new CreateUserRequest
            {
                FirstName = "foo",
                LastName = "bar",
                Email = "foo@gmail.com",
                PlainPassword = "foobar1",
                Username = "foobar",
                IsAdmin = true
            }, out _, out _));

            mockUsers.Verify(
                x => x.AddUser(It.Is<UserEntry>(
                    u => u.FirstName == "foo" 
                        && u.LastName == "bar" 
                        && u.Email == "foo@gmail.com"
                        && u.Username == "foobar"
                        && u.Password == new SHA256HashedString("foobar1", u.PasswordSalt)
                        && u.IsAdmin == true
                        )),
                    Times.Once
                );
        }
    }
}
