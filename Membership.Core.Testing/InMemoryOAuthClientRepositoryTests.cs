using Membership.Core.Data;
using System;
using Xunit;

namespace Membership.Core.Testing
{
    public class InMemoryOAuthClientRepositoryTests
    {
        [Fact]
        void Add_NoClientId_ThrowsArgumentException()
        {
            var repository = new InMemoryOAuthClientRepository();
            Assert.Throws<ArgumentException>(delegate
            {
                repository.Add(new OAuthClientEntry
                {
                    Name = "Foo",
                    Secret = new SHA256HashedString("Bar")
                });
            });
        }

        [Fact]
        void Add_DuplicatedClientId_ThrowsInvalidOperationException()
        {
            var repository = new InMemoryOAuthClientRepository();
            Assert.Throws<InvalidOperationException>(delegate
            {
                repository.Add(new OAuthClientEntry { Id = "1" });
                repository.Add(new OAuthClientEntry { Id = "1" });
            });
        }

        [Fact]
        void Find_PreviouslyAddedClient_Success()
        {
            var repository = new InMemoryOAuthClientRepository();
            repository.Add(new OAuthClientEntry { Id = "MyId", Name = "MyName", Secret = new SHA256HashedString("MySecret")});
            var client = repository.Find("MyId");

            Assert.Equal("MyId", client.Id);
            Assert.Equal("MyName", client.Name);
            Assert.Equal(new SHA256HashedString("MySecret"), client.Secret);
        }
    }
}
