using Xunit;

namespace Membership.Core.Testing
{
    public class RandomStringTests
    {
        [Fact]
        public void Generate_AlphaNumericCharSet_NoDups()
        {
            Assert.NotEqual(
                RandomString.Generate(32, RandomString.AlphaNumericCharSet, 1),
                RandomString.Generate(32, RandomString.AlphaNumericCharSet, 2));
        }

        [Fact]
        public void Generate_AlphaNumericCharSet_CorrectLength()
        {
            Assert.Equal(
                32,
                RandomString.Generate(32, RandomString.AlphaNumericCharSet, 1).Length);
        }
    }
}
