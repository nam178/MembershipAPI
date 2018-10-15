namespace Membership.Core.Data
{
    /// <summary>
    /// A token database
    /// </summary>
    interface IOAuthTokenRepository
    {
        /// <summary>
        /// Find the token and return it
        /// </summary>
        /// <returns>The token or NULL when not found</returns>
        OAuthTokenEntry FindToken(string token);

        /// <summary>
        /// Add a new entry into the token database
        /// </summary>
        void AddToken(OAuthTokenEntry token);
    }
}
