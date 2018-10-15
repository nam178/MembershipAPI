using System;

namespace Membership.Core.Data
{
    /// <summary>
    /// A deactivation record as stored in database,
    /// indicates that an user has been de-activated for an application.
    /// </summary>
    public class DeactivationEntry
    {
        /// <summary>
        /// The user in which was de-activated
        /// </summary>
        public Guid UserId
        { get; set; }

        /// <summary>
        /// The application (OAth client) in which this user is de-activated
        /// </summary>
        public string OAuthClientId
        { get; set; }
    }
}
