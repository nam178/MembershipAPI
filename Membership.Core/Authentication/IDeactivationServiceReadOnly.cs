using System;

namespace Membership.Core.Authentication
{
    /// <summary>
    /// Service to check activation status for users
    /// </summary>
    public interface IDeactivationServiceReadOnly
    {
        /// <summary>
        /// Is the provided user is deactivated for the current authenticated tenant application?
        /// </summary>
        bool IsUserDeactivated(Guid userId);
    }
}