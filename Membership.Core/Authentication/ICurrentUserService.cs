using Membership.Core.Data;

namespace Membership.Core.Authentication
{
    /// <summary>
    /// Service that allows you to query current member and client 
    /// for the current HTTP request.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// The authenticated user (the member)
        /// </summary>
        ///<exception cref="System.InvalidOperationException"> Accessing this property when the user is not authenticated.</exception>
        UserEntry AuthenticatedUser
        { get; }

        /// <summary>
        /// The autheticated client (the tenant application)
        /// </summary>
        ///<exception cref="System.InvalidOperationException">Accessing this property when the user is not authenticated.</exception>
        string AuthenticatedClientId
        { get; }
    }
}
