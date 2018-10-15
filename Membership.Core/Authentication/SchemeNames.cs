namespace Membership.Core.Authentication
{
    /// <summary>
    /// For our OAuth authentication, we have 2 authentication schemes:
    /// </summary>
    public static class SchemeName
    {
        /// <summary>
        /// The 'TenantApps' scheme, using HTTP Basic authentication 
        /// to authenticatate the 3rd party tenant applications.
        /// </summary>
        public const string TenantApps = "TenantApps";

        /// <summary>
        /// The 'Member' scheme, using Bearer token to authenticate the user
        /// in which the 3rd party applications are making requests in their behalf.
        /// </summary>
        public const string Member = "Member";
    }
}
