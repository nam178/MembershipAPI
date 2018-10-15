// I ran into problem as described here
// https://github.com/aspnet/Mvc/issues/7710
// So won't beable to add any integration test for this homework.

#if false
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace Membership.Core.Testing
{
    public class AuthenticationControllerIntegrationTest
        : IClassFixture<WebApplicationFactory<Api.Startup>>
    {
        readonly WebApplicationFactory<Api.Startup> _factory;

        public AuthenticationControllerIntegrationTest(WebApplicationFactory<Api.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("esiStOMporitandRogYRaTRacstigHAnTERStATH", "wrongPassword")]
        [InlineData("wrongId", "$lGh6QlOIPRgd87DWeOUo5Y!H!%GOPsI0YdFxDN1")]
        public async void Get_IncorrectClientIdOrSecret_ReturnsUnauthorized(string clientId, string secret)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/token");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
#endif