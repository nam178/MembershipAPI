using System;

namespace Membership.Core.Authentication
{
    /// <summary>
    /// Service to active, deactivate and check activation status for users
    /// </summary>
    public interface IDeactivationService : IDeactivationServiceReadOnly
    {
        /// <summary>
        /// Activate or de-activate specified user
        /// </summary>
        /// <returns>True on success, or false on failure</returns>
        bool TrySetActivationStatus(
            Guid userId, 
            bool activate,
            out string err);
    }
}
