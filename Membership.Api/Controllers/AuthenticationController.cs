using Membership.Api.Messages;
using Membership.Core.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Membership.Api.Controllers
{
    /// <summary>
    /// API endpoint to authenticate user, by exchanging username 
    /// password pair for an authentication token.
    /// 
    /// The clients (Tenant applications) must be athenticated to 
    /// use this endpoint (SchemeName.TenantApps)
    /// </summary>
    [ApiController]
    [Authorize(AuthenticationSchemes = SchemeName.TenantApps)]
    public class AuthenticationController : ControllerBase
    {
        readonly IOAuthPasswordGrantService _grantService;

        public AuthenticationController(
            IOAuthPasswordGrantService grantService,
            ICurrentUserService currentUserService)
        {
            _grantService = grantService 
                ?? throw new ArgumentNullException(nameof(grantService));
        }

        // GET: /api/auth
        [HttpGet]
        [Route("/api/auth")]
        public ActionResult<ApiTokenResponseMessage> Get([FromQuery] ApiTokenRequestMessage request)
        {
            // We only support the 'password' grant type at the moment.
            if (false == string.Equals(
                request.GrantType, 
                "password", 
                StringComparison.InvariantCultureIgnoreCase))
            {
                return new StatusCodeResult(501);
            }

            // Grant
            var grantRequest = new OAuthPasswordGrantRequest
            {
                Password = request.Password,
                Username = request.Username
            };
            if (false == _grantService.TryGrant(
                grantRequest, out string err, out string token, out int expires))
            {
                return BadRequest(err);
            }

            // Grant success,
            // Return the token to the client
            return new ApiTokenResponseMessage
            {
                AccessToken = token,
                ExpiresIn = expires,
                TokenType = "Bearer" 
            };
        }
    }
}
