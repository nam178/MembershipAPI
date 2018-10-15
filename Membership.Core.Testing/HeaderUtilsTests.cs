using Membership.Core.Authentication;
using Xunit;

namespace Membership.Core.Testing
{
    public class HeaderUtilsTests
    {
        [Fact]
        public void TryGetBasicAuthStr_ValidInput_ReturnsTrue()
        {
            Assert.True(HeaderUtils.TryGetBasicAuthStr(
                "Basic aGVsbG86d29ybGQ=", out string t));
            Assert.Equal("aGVsbG86d29ybGQ=", t);
        }

        [Fact]
        public void TryGetBearerAuthStr_ValidInput_ReturnsTrue()
        {
            Assert.True(HeaderUtils.TryGetBearerAuthString(
                "Bearer xyz\\|123", out string t));
            Assert.Equal("xyz\\|123", t);
        }
    }
}
