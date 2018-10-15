using Membership.Core.Authentication;
using Membership.Core.Data;
using Moq;
using System;
using Xunit;

namespace Membership.Core.Testing
{
    public class DeactivationServiceTests
    {
        [Theory]
        [InlineData("app1", false)]
        [InlineData("app2", true)]
        public void IsUserDeactivated_MultipleApplications_OnlyDeactivatedOnOne(string currentAppId, bool expectDeactivated)
        {
            MockUp(
                out DeactivationService service,
                out Guid userId, 
                out Mock<ICurrentUserService> mockCurrentUserService, 
                out Mock<IDeactivationRepository> mockDeactivationRepository, 
                out _);
            mockCurrentUserService
                .Setup(x => x.AuthenticatedClientId)
                .Returns(currentAppId);
            mockDeactivationRepository
                .Setup(x => x.Exists(It.Is<DeactivationEntry>(e => e.OAuthClientId == "app2")))
                .Returns(true);

            Assert.Equal(expectDeactivated, service.IsUserDeactivated(userId));
        }

        static void MockUp(
            out DeactivationService service,
            out Guid userId,
            out Mock<ICurrentUserService> mockCurrentUserService,
            out Mock<IDeactivationRepository> mockDeactivationRepository,
            out Mock<IUserRepository> mockUsers
            )
        {
            var t = Guid.NewGuid();
            userId = t;
            mockCurrentUserService = new Mock<ICurrentUserService>();
            mockDeactivationRepository = new Mock<IDeactivationRepository>();
            mockUsers = new Mock<IUserRepository>();
            mockUsers.Setup(x => x.FindUserByUserId(t)).Returns(new UserEntry { Id = t });
            service = new DeactivationService(
               mockCurrentUserService.Object,
               mockDeactivationRepository.Object,
               mockUsers.Object
               );
        }

        [Theory]
        [InlineData(true, true, "currentAppId", true)]
        [InlineData(true, true, "notCurrentAppId", false)]
        [InlineData(true, false, "notCurrentAppId", false)]
        [InlineData(true, false, "currentAppId", false)]
        [InlineData(false, true, "currentAppId", true)]
        [InlineData(false, true, "notCurrentAppId", false)]
        [InlineData(false, false, "notCurrentAppId", false)]
        [InlineData(false, false, "currentAppId", false)]
        public void TrySetActivationStatus_ActiviteDeactiveUserNonAdmin_ReturnsFalseForNonCurrentSystemAdmin(
            bool activating, 
            bool currentUserIdAdmin, 
            string currentUserApplicationId,
            bool expectedReturnValue)
        {
            MockUp(
                out DeactivationService service,
                out Guid userId, 
                out Mock<ICurrentUserService> mockCurrentUserService, 
                out _,
                out _);

            mockCurrentUserService
                .Setup(x => x.AuthenticatedClientId)
                .Returns("currentAppId");
            mockCurrentUserService
                .Setup(x => x.AuthenticatedUser)
                .Returns(new UserEntry
                {
                    IsAdmin = currentUserIdAdmin,
                    ClientId = currentUserApplicationId
                });

            Assert.Equal(expectedReturnValue, service.TrySetActivationStatus(userId, activating, out _));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TrySetActivationStatus_ActionPerformed_ChangesWrittenForCurrentApplication(bool isActivating)
        {
            MockUp(
               out DeactivationService service,
               out Guid userId,
               out Mock<ICurrentUserService> mockCurrentUserService,
               out Mock<IDeactivationRepository> mockDeactivationRepository, 
               out _);

            mockCurrentUserService
                .Setup(x => x.AuthenticatedClientId)
                .Returns("currentAppId");
            mockCurrentUserService
                .Setup(x => x.AuthenticatedUser)
                .Returns(new UserEntry
                {
                    IsAdmin = true,
                    ClientId = "currentAppId"
                });
            if(isActivating)
            {
                mockDeactivationRepository
                    .Setup(x => x.Exists(It.IsAny<DeactivationEntry>()))
                    .Returns(true);
            }
            service.TrySetActivationStatus(userId, isActivating, out _);

            if(isActivating)
                mockDeactivationRepository
                    .Verify(x => x.Remove(It.Is<DeactivationEntry>(e => e.OAuthClientId == "currentAppId")));
            else
                mockDeactivationRepository
                    .Verify(x => x.Add(It.Is<DeactivationEntry>(e => e.OAuthClientId == "currentAppId")));
        }
    }
}
