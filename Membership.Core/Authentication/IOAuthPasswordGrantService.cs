namespace Membership.Core.Authentication
{
    /// <summary>
    /// Service to exchange an user using username and password pair
    /// for an authentication token.
    /// 
    /// (Conforming OAuth standard)
    /// </summary>
    public interface IOAuthPasswordGrantService
    {
        /// <summary>
        /// Grant access for user, outputting a bearer token if success,
        /// otherwise, output the error message
        /// </summary>
        /// <param name="request">Required data for this OAuth grant type.</param>
        bool TryGrant(
            OAuthPasswordGrantRequest request,
            out string errorMessage,
            out string bearerToken, 
            out int bearerTokenExpires
            );
    }
}