using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Membership.Core.Authentication
{
    /// <summary>
    /// Basic authentication handler for ASP.NET core,
    /// used to authenticate 3rd party applications using
    /// Client ID and Client Secret as per OAuth specs.
    /// </summary>
    sealed class TenantAppsAuthenticationHandler 
        : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        readonly ITenantAppsAuthenticationService _authenticationService;

        public TenantAppsAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ITenantAppsAuthenticationService authenticationService)
            : base(options, logger, encoder, clock)
        {
            _authenticationService = authenticationService
                ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return HandleAuthenticateAsyncImpl(
                Logger,
                Request.Headers,
                _authenticationService,
                Scheme.Name
                );
        }

        static internal Task<AuthenticateResult> HandleAuthenticateAsyncImpl(
            ILogger logger,
            IHeaderDictionary headers,
            ITenantAppsAuthenticationService authService,
            string scheme)
        {
            try
            {
                // Failed to authentcate?
                if (false == authService.TryAuthenticate(headers,
                        out string errorMessage,
                        out string clientId))
                {
                    return Task.FromResult(AuthenticateResult.Fail(errorMessage));
                }

                // Authentication success?
                return Task.FromResult(AuthenticateResult.Success(
                    new AuthenticationTicket(
                        new ClaimsPrincipal(new ClaimsIdentity(
                            new Claim[] { new Claim(CustomClaimTypes.OAuthClientId, clientId) },
                            scheme
                            )),
                        scheme)));
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToStringWithInnerExceptions());
                return Task.FromResult(AuthenticateResult.Fail(Text.Authentication_BasicAuthFatalErrorMessage));
            }
        }
    }
}
