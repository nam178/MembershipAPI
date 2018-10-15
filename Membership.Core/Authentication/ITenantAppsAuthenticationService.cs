using Microsoft.AspNetCore.Http;

namespace Membership.Core.Authentication
{
    /// <summary>
    /// A service to authenticate tenant applications using HTTP header field.
    /// </summary>
    interface ITenantAppsAuthenticationService
    {
        /// <summary>
        /// Authenticate using provided HTTP headers
        /// </summary>
        /// <param name="errorMessage">If authentication fails, the error message, or NULL when success</param>
        /// <param name="errorMessage">If authentication successes, the authenticated client ID</param>
        /// <returns>True if authenticated</returns>
        bool TryAuthenticate(
            IHeaderDictionary requestHeader,
            out string errorMessage,
            out string clientId);
    }
}