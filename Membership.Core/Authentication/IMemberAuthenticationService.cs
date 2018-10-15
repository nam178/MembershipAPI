using Microsoft.AspNetCore.Http;
using System;

namespace Membership.Core.Authentication
{
    /// <summary>
    /// A service to authenticate member (users in the user repository)
    /// using Authorization header + bearer token
    /// </summary>
    interface IMemberAuthenticationService
    {
        /// <summary>
        /// Authenticate the HTTP request with provided header and returns True on success
        /// </summary>
        bool TryAuthenticate(
            IHeaderDictionary requestHeader,
            out string errorMessage,
            out string clientId,
            out Guid userId
            );
    }
}