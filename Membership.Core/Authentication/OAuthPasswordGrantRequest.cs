namespace Membership.Core.Authentication
{
    /// <summary>
    /// Required data for an OAuth password grant
    /// </summary>
    public class OAuthPasswordGrantRequest
    {
        /// <summary>
        /// The member's username, collected by the client application and forwarding to us
        /// </summary>
        public string Username
        { get; set; }

        /// <summary>
        /// The member's password, collected by the client application and forwarding to us.
        /// </summary>
        public string Password
        { get; set; }
    }
}