using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Membership.Core.Authentication
{
    /// <summary>
    /// Bearer token authentication handler,
    /// used to authenticate the member in which the tenant applications
    /// are making request on their behalf.
    /// </summary>
    sealed class MemberAuthenticationHandler
        : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        readonly IMemberAuthenticationService _memberAuthenticationService;

        public MemberAuthenticationHandler(
            IMemberAuthenticationService memberAuthenticationService,
            IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
            _memberAuthenticationService = memberAuthenticationService 
                ?? throw new ArgumentNullException(nameof(memberAuthenticationService));
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return Task.FromResult(HandleAuthenticateAsyncImpl(
                Logger,
                _memberAuthenticationService, 
                Request.Headers,
                Scheme.Name));
        }

        internal static AuthenticateResult HandleAuthenticateAsyncImpl(
            ILogger logger,
            IMemberAuthenticationService memberAuthenticationService, 
            IHeaderDictionary headers,
            string scheme)
        {
            try
            {
                if (memberAuthenticationService.TryAuthenticate(
                    headers, out string errorMessage, out string clientId, out Guid userId))
                {
                    return AuthenticateResult.Success(
                        new AuthenticationTicket(
                            new ClaimsPrincipal(
                                new ClaimsIdentity(
                                    new Claim[] {
                                    new Claim(CustomClaimTypes.OAuthClientId, clientId),
                                    new Claim(CustomClaimTypes.OAuthUserId, userId.ToString()),
                                    },
                                scheme)),
                            scheme));
                }

                return AuthenticateResult.Fail(errorMessage);
            }
            catch(Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToStringWithInnerExceptions());
                return AuthenticateResult.Fail(Text.Authentication_BasicAuthFatalErrorMessage);
            }
        }
    }
}