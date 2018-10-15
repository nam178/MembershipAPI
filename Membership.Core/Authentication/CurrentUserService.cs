using System;
using System.Linq;
using Membership.Core.Data;
using Microsoft.AspNetCore.Http;

namespace Membership.Core.Authentication
{
    /// <summary>
    /// Implementation for ICurrentUserService that uses Aspnet core's 
    /// authentication system
    /// </summary>
    /// <remarks>Not thread safe. Do not make singleton.</remarks>
    sealed class CurrentUserService : ICurrentUserService
    {
        readonly Lazy<UserEntry> _currentUser;
        readonly Lazy<string> _currentClientId;

        public UserEntry AuthenticatedUser
        { get { return _currentUser.Value; } }

        public string AuthenticatedClientId
        { get { return _currentClientId.Value; } }

        public CurrentUserService(
            IHttpContextAccessor httpContext, 
            IUserRepository users)
        {
            if (users == null)
                throw new ArgumentNullException(nameof(users));

            // Declare a lazy loader for CurrentUser
            _currentUser = new Lazy<UserEntry>(delegate
            {
                return GetUser(httpContext, users);
            }, false);

            // Declare a lazy loader for CurrentClientId
            _currentClientId = new Lazy<string>(delegate
            {
                return GetOAuthClientId(httpContext);
            }, false);
        }

        static string GetOAuthClientId(IHttpContextAccessor httpContext)
        {
            var oauthClientIdClaimType = httpContext.HttpContext.User.FindFirst(CustomClaimTypes.OAuthClientId);
            if (oauthClientIdClaimType == null)
                throw new InvalidOperationException();
            return oauthClientIdClaimType.Value;
        }

        static UserEntry GetUser(IHttpContextAccessor httpContext, IUserRepository users)
        {
            var user = httpContext.HttpContext.User;

            var oauthUserIdClaim = user.FindFirst(CustomClaimTypes.OAuthUserId);
            if (null == oauthUserIdClaim)
                throw new InvalidOperationException();

            return users.FindUserByUserId(Guid.Parse(oauthUserIdClaim.Value));
        }
    }
}
