using Xunit;

namespace Membership.Core.Testing
{
    public class SHA256HashedStringTests
    {
        [Fact]
        public void Hash_KnownHashValues_Match()
        {
            Assert.Equal(
                // Checked against this website https://passwordsgenerator.net/sha256-hash-generator/
                "E2FC8CFF37D175668B4877367DF3B0D1234705E07C6AE0BCB75041CDCEAC94E7",
                new SHA256HashedString("helloWorld").ToString()
                );
        }

        [Fact]
        public void Equals_SameString_ReturnsTrue()
        {
            Assert.True(new SHA256HashedString("helloWorld").Equals(new SHA256HashedString("helloWorld")));
        }

        [Fact]
        public void Equals_DifferentString_ReturnsFalse()
        {
            Assert.False(new SHA256HashedString("helloWorld").Equals(new SHA256HashedString("helloWORLD")));
            Assert.False(new SHA256HashedString("helloWorld").Equals(null));
        }

        [Fact]
        public void EqualOperator_SameString_ReturnsTrue()
        {
            Assert.True(new SHA256HashedString("helloWorld") 
                == new SHA256HashedString("helloWorld"));

            SHA256HashedString x = null;
            SHA256HashedString y = null;

            Assert.True(x == y);
        }

        [Fact]
        public void EqualOperator_DifferentString_ReturnsTrue()
        {
            Assert.False(new SHA256HashedString("helloWorld")
                == new SHA256HashedString("helloWORLD"));

            SHA256HashedString x = null;
            Assert.False(x == new SHA256HashedString("hello"));
        }
    }
}
