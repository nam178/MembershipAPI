using Membership.Core.Data;
using System;
using Xunit;

namespace Membership.Core.Testing
{
    public class InMemoryUserRepositoryTests
    {
        [Theory]
        [InlineData("61a09e21-fdc5-40f0-b9cf-db86c4b661fa", "bar")]
        [InlineData("bc23d2e6-f4c4-48d1-aaf3-f404f65cfcd3", "foo")]
        public void AddUser_DuplicatedIdOrName_ThrowsInvalidOperationException(string id, string name)
        {
            var repository = new InMemoryUserRepository();

            // Pre-existing users
            repository.AddUser(new UserEntry
            {
                Id = Guid.Parse("61a09e21-fdc5-40f0-b9cf-db86c4b661fa"),
                Username = "foo"
            });

            Assert.Throws<InvalidOperationException>(delegate
            {
                repository.AddUser(new UserEntry
                {
                    Id = Guid.Parse(id),
                    Username = name
                });
            });
        }

        [Fact]
        public void AddUser_ValidUser_CanFindItWithFindUser()
        {
            var repository = new InMemoryUserRepository();

            // Add a valid user
            repository.AddUser(new UserEntry
            {
                Id = Guid.Parse("61a09e21-fdc5-40f0-b9cf-db86c4b661fa"),
                Username = "foo",
                ClientId = "client-id-here",
                Email = "foo@gmail.com",
                FirstName = "first",
                LastName = "last",
                Password = new SHA256HashedString("pass", "salt"),
                PasswordSalt = "salt"
            });

            VerifyUser(repository.FindUserByName("foo"));
            VerifyUser(repository.FindUserByUserId(Guid.Parse("61a09e21-fdc5-40f0-b9cf-db86c4b661fa")));
        }

        [Fact]
        public void Update_UserNotFound_ThrowsInvalidOperation()
        {
            var repository = new InMemoryUserRepository();

            Assert.Throws<InvalidOperationException>(delegate
            {
                repository.Update(new UserEntry
                {
                    Id = Guid.NewGuid(),
                    Username = "foo"
                });
            });
        }

        [Fact]
        public void Update_UserFound_ChangesSaved()
        {
            var userId = Guid.NewGuid();
            var repository = new InMemoryUserRepository();
            repository.AddUser(new UserEntry
            {
                Id = userId,
                Username = "bar"
            });
            
            repository.Update(new UserEntry
            {
                Id = userId,
                Username = "foo",
                Password = new SHA256HashedString("pas", "sal"),
                PasswordSalt = "sal"
            });

            var savedUser = repository.FindUserByName("foo");
            Assert.Equal("foo", savedUser.Username);
            Assert.Equal(new SHA256HashedString("pas", "sal"), savedUser.Password);
            Assert.Equal("sal", savedUser.PasswordSalt);
        }

        static void VerifyUser(UserEntry user)
        {
            Assert.Equal(Guid.Parse("61a09e21-fdc5-40f0-b9cf-db86c4b661fa"), user.Id);
            Assert.Equal("foo", user.Username);
            Assert.Equal("client-id-here", user.ClientId);
            Assert.Equal("foo@gmail.com", user.Email);
            Assert.Equal("first", user.FirstName);
            Assert.Equal("last", user.LastName);
            Assert.Equal(new SHA256HashedString("pass", "salt"), user.Password);
            Assert.Equal("salt", user.PasswordSalt);
        }
    }
}
