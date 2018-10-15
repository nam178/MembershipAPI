using Membership.Core.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Membership.Core.Authentication
{
    /// <summary>
    /// Implementation for ITenantAppsAuthenticationService that uses
    /// a database with stored secret to check against the client's supplied secret.
    /// </summary>
    sealed class TenantAppsAuthenticationService
        : ITenantAppsAuthenticationService
    {
        readonly IOAuthClientRepository _oAuthClientRepository;

        public TenantAppsAuthenticationService(IOAuthClientRepository oAuthClientRepository)
        {
            _oAuthClientRepository = oAuthClientRepository 
                ?? throw new ArgumentNullException(nameof(oAuthClientRepository));
        }

        public bool TryAuthenticate(
            IHeaderDictionary requestHeader,
            out string errorMessage,
            out string clientId)
        {
            if (requestHeader == null)
                throw new System.ArgumentNullException(nameof(requestHeader));
            errorMessage = null;
            clientId = null;

            // Find client_id, client_secret from Authentication header
            if(false == TryParseHeader(requestHeader, 
                out errorMessage, out clientId, out string clientSecret))
            {
                return false;
            }

            // Find the client
            var client = _oAuthClientRepository.Find(clientId);
            if (null == client)
            {
                errorMessage = Text.Authentication_BasicAuthNoClientIdErrorMessage;
                return false;
            }

            // Check its secret
            if (client.Secret != new SHA256HashedString(clientSecret))
            {
                errorMessage = Text.Authentication_BasicAuthClientPasswordNotMatchErrorMessage;
                return false;
            }

            // Success!
            return true;
        }

        /// <summary>
        /// Parse Client ID, Client Secret from provided HTTP headers
        /// </summary>
        static bool TryParseHeader(
            IHeaderDictionary requestHeader, 
            out string errorMessage, 
            out string clientId,
            out string clientSecret)
        {
            clientId = null;
            errorMessage = null;
            clientSecret = null;

            if (false == requestHeader.ContainsKey(
                HttpHeaderKey.Authorization))
            {
                errorMessage = Text.Authentication_BasicAuthMissingAuthHeaderErrorMessage;
                return false;
            }

            if (false == HeaderUtils.TryGetBasicAuthStr(
                requestHeader[HttpHeaderKey.Authorization], 
                out string authStrBinary))
            {
                errorMessage = Text.Authentication_BasicAuthInvalidAuthHeaderErrorMessage;
                return false;
            }

            var authStr = Encoding.ASCII.GetString(Convert.FromBase64String(authStrBinary));
            if (false == TrySplit(authStr, out clientId, out clientSecret))
            {
                errorMessage = Text.Authentication_BasicAuthInvalidAuthHeaderErrorMessage;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Helper method that spltis username:password into username, password
        /// </summary>
        internal static bool TrySplit(
            string inputString,
            out string left,
            out string right)
        {
            var seperatorIndex = inputString.IndexOf(':');
            if (seperatorIndex <= 0
                || seperatorIndex == (inputString.Length - 1))
            {
                left = null;
                right = null;
                return false;
            }

            left = inputString.Substring(0, seperatorIndex);
            right = inputString.Substring(seperatorIndex + 1);
            return true;
        }
    }
}
