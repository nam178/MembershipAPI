using Membership.Core.Data;
using Microsoft.AspNetCore.Authentication;
using System;

namespace Membership.Core.Authentication
{
    /// <summary>
    /// Implementation of IOAuthPasswordGrantService,
    /// this instance won't 
    /// </summary>
    sealed class OAuthPasswordGrantService
        : IOAuthPasswordGrantService
    {
        readonly ISystemClock _clock;
        readonly IUserRepository _userRepository;
        readonly IOAuthTokenRepository _tokenRepository;
        readonly IDeactivationServiceReadOnly _deactivationService;
        readonly ICurrentUserService _currentUser;
        const int TokenLength = 40;
        const int TokenTTL = 3600;

        public OAuthPasswordGrantService(
            ISystemClock clock,
            IUserRepository userRepository,
            IOAuthTokenRepository tokenRepository,
            IDeactivationServiceReadOnly deactivationService,
            ICurrentUserService currentUser)
        {
            _clock = clock;
            _userRepository = userRepository 
                ?? throw new ArgumentNullException(nameof(userRepository));
            _tokenRepository = tokenRepository 
                ?? throw new ArgumentNullException(nameof(tokenRepository));
            _deactivationService = deactivationService 
                ?? throw new ArgumentNullException(nameof(deactivationService));
            _currentUser = currentUser
                ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public bool TryGrant(
            OAuthPasswordGrantRequest request, 
            out string errorMessage, 
            out string bearerToken, 
            out int bearerTokenExpires)
        {
            Validate(request);

            // Defaults for out
            errorMessage = null;
            bearerTokenExpires = default(int);
            bearerToken = null;

            // Authenticate the user first
            if(false == TryAuthenticateUser(request, out UserEntry user, out errorMessage))
                return false;

            // Grant
            GrantToken(out bearerToken, out bearerTokenExpires, user);
            return true;
        }

        bool TryAuthenticateUser(
            OAuthPasswordGrantRequest grantRequest,
            out UserEntry user,
            out string errorMessage)
        {
            // First find the user
            user = _userRepository.FindUserByName(grantRequest.Username);
            if (null == user)
            {
                errorMessage = string.Format(
                    Text.OAuthPasswordGrantService_NoUserWithProvidedNameErrorMessage,
                    grantRequest.Username
                    );
                return false;
            }

            // Ensure this account is active
            if(_deactivationService.IsUserDeactivated(user.Id))
            {
                errorMessage = Text.OAuthPasswordGrantService_UserDeactivated;
                return false;
            }

            // Check password
            if (!user.IsPassword(grantRequest.Password))
            {
                errorMessage = string.Format(
                   Text.OAuthPasswordGrantService_IncorrectPasswordErrorMessage,
                   grantRequest.Username
                   );
                return false;
            }

            errorMessage = null;
            return true;
        }

        void GrantToken(out string bearerToken, out int bearerTokenExpires, UserEntry user)
        {
            bearerToken = RandomString.Generate(TokenLength);
            bearerTokenExpires = UnixTimestamp.FromDateTime(_clock.UtcNow.UtcDateTime) + TokenTTL;
            _tokenRepository.AddToken(new OAuthTokenEntry
            {
                UserId = user.Id,
                Expires = bearerTokenExpires,
                Token = bearerToken,
                OAuthClientId = _currentUser.AuthenticatedClientId
            });
        }

        static void Validate(OAuthPasswordGrantRequest context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (context.Username == null
                || context.Password == null)
            {
                throw new ArgumentException();
            }
        }
    }
}
