using Membership.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Membership.Core.Testing
{
    public class InMemoryOAuthTokenRepositoryTests
    {
        [Fact]
        public void AddToken_ValidToken_BecomesAvailableInFindToken()
        {
            var repo = new InMemoryOAuthTokenRepository();
            var newToken = new OAuthTokenEntry
            {
                Token = "foo",
                Expires = 1,
                UserId = Guid.NewGuid()
            };
            repo.AddToken(newToken);

            var addedToken = repo.FindToken("foo");

            Assert.Equal(addedToken.UserId, newToken.UserId);
            Assert.Equal(addedToken.Expires, newToken.Expires);
            Assert.Equal(addedToken.Token, newToken.Token);
        }

        [Fact]
        public void AddToken_PreExistingTokenSameId_ThrowsInvalidOperationException()
        {
            var repo = new InMemoryOAuthTokenRepository();
            var newToken = new OAuthTokenEntry
            {
                Token = "foo",
                Expires = 1,
                UserId = Guid.NewGuid()
            };
            repo.AddToken(newToken);

            Assert.Throws<InvalidOperationException>(delegate
            {
                repo.AddToken(new OAuthTokenEntry
                {
                    Token = "foo",
                    Expires = 2,
                    UserId = Guid.NewGuid()
                });
            });
        }
    }
}
