using Membership.Core.Authentication;
using Membership.Core.Data;
using Membership.Core.Membership;
using Moq;
using Xunit;

namespace Membership.Core.Testing
{
    public class UpdatePasswordServiceTests
    {
        [Fact]
        public void TryUpdatePassword_IncorrectOldPassword_ReturnsFalse()
        {
            MockUp(out UpdatePasswordService service, out _, out _);

            Assert.False(service.TryUpdatePassword("old-wrong", "new", out string err));
        }

        [Fact]
        public void TryUpdatePassword_CorrectOldButInvalidNewPassword_ReturnsFalse()
        {
            MockUp(out UpdatePasswordService service, out _, out _);

            Assert.False(service.TryUpdatePassword("old", "new", out _));
        }

        [Fact]
        public void TryUpdatePassword_ValidOldAndNewPass_ReturnsTrueWithUserUpdated()
        {
            MockUp(
              out UpdatePasswordService service,
              out Mock<IPasswordPolicy> mockPasswordPolicy,
              out Mock<IUserRepository> mockUsers);

            string err;
            mockPasswordPolicy
                .Setup(x => x.IsValid(It.IsAny<string>(), out err))
                .Returns(true);

            Assert.True(service.TryUpdatePassword("old", "new", out _));

            mockUsers.Verify(x => x.Update(It.Is<UserEntry>(
                u => u.Password == new SHA256HashedString("new", u.PasswordSalt))));
        }

        static void MockUp(
            out UpdatePasswordService service,
            out Mock<IPasswordPolicy> mockPasswordPolicy,
            out Mock<IUserRepository> mockUsers)
        {
            var user = new UserEntry
            {
                Password = new SHA256HashedString("old", "sal"),
                PasswordSalt = "sal"
            };
            var currentUserService = new Mock<ICurrentUserService>();
            currentUserService.Setup(inst => inst.AuthenticatedUser).Returns(user);
            mockUsers = new Mock<IUserRepository>();
            mockPasswordPolicy = new Mock<IPasswordPolicy>();
            
            service = new UpdatePasswordService(
                currentUserService.Object,
                mockUsers.Object,
                mockPasswordPolicy.Object
                );
        }
    }
}
