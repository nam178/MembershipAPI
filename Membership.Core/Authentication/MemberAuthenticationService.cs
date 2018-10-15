using System;
using Membership.Core.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Membership.Core.Authentication
{
    /// <summary>
    /// Implementation for IMemberAuthenticationService,
    /// uses token and user database.
    /// </summary>
    sealed class MemberAuthenticationService : IMemberAuthenticationService
    {
        readonly IOAuthTokenRepository _tokenRepository;
        readonly IUserRepository _userRepository;
        readonly ISystemClock _clock;

        public MemberAuthenticationService(
            IOAuthTokenRepository tokenRepository,
            IUserRepository userRepository,
            ISystemClock clock)
        {
            _tokenRepository = tokenRepository 
                ?? throw new ArgumentNullException(nameof(tokenRepository));
            _userRepository = userRepository 
                ?? throw new ArgumentNullException(nameof(userRepository));
            _clock = clock 
                ?? throw new ArgumentNullException(nameof(clock));
        }

        public bool TryAuthenticate(
            IHeaderDictionary requestHeader, 
            out string errorMessage, 
            out string clientId, 
            out Guid userId)
        {
            if (requestHeader == null)
                throw new ArgumentNullException(nameof(requestHeader));

            errorMessage = null;
            clientId = null;
            userId = default(Guid);
            
            // First get the token
            if(false == TryGetToken(requestHeader, out errorMessage, out OAuthTokenEntry tokenEntry))
                return false;

            // Then get the user, to look up its client_id
            var user = _userRepository.FindUserByUserId(tokenEntry.UserId);
            if(null == user)
            {
                errorMessage = Text.OAuthPasswordGrantService_CantFindUserAssociatedWithToken;
                return false;
            }

            // Authentication successed
            userId = tokenEntry.UserId;
            clientId = tokenEntry.OAuthClientId;
            return true;
        }

        bool TryGetToken(
            IHeaderDictionary requestHeader, 
            out string errorMessage, 
            out OAuthTokenEntry tokenEntry)
        {
            tokenEntry = null;
            if (false == requestHeader.ContainsKey(HttpHeaderKey.Authorization))
            {
                errorMessage = Text.MemberAuthenticationService_MissingAuthHeader;
                return false;
            }

            var authStr = requestHeader[HttpHeaderKey.Authorization];
            if (false == HeaderUtils.TryGetBearerAuthString(authStr, out string token))
            {
                errorMessage = Text.MemberAuthenticationService_AuthHeaderInvalidFormat;
                return false;
            }

            tokenEntry = _tokenRepository.FindToken(token);
            if (null == tokenEntry)
            {
                errorMessage = Text.MemberAuthenticationService_TokenNotFound;
                return false;
            }

            var now = UnixTimestamp.FromDateTime(_clock.UtcNow.UtcDateTime);
            if (tokenEntry.Expires < now)
            {
                errorMessage = Text.MemberAuthenticationService_TokenNotFound;
                return false;
            }

            errorMessage = null;
            return true;
        }
    }
}
