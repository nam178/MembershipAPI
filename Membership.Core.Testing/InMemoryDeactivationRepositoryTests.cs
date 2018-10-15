using Membership.Core.Data;
using System;
using Xunit;

namespace Membership.Core.Testing
{
    public class InMemoryDeactivationRepositoryTests
    {
        [Fact]
        public void Add_ValidEntry_ExistsReturnsTrue()
        {
            var repo = new InMemoryDeactivationRepository();
            var userId = Guid.NewGuid();

            repo.Add(new DeactivationEntry
            {
                UserId = userId,
                OAuthClientId = "clientA"
            });

            Assert.True(repo.Exists(new DeactivationEntry
            {
                UserId = userId,
                OAuthClientId = "clientA"
            }));
        }

        [Fact]
        public void Remove_ExistingEntry_ExistsReturnFalse()
        {
            var repo = new InMemoryDeactivationRepository();
            var userId = Guid.NewGuid();

            repo.Add(new DeactivationEntry
            {
                UserId = userId,
                OAuthClientId = "clientA"
            });
            repo.Remove(new DeactivationEntry
            {
                UserId = userId,
                OAuthClientId = "clientA"
            });
            Assert.False(repo.Exists(new DeactivationEntry
            {
                UserId = userId,
                OAuthClientId = "clientA"
            }));
        }
    }
}
