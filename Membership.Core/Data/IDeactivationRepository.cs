namespace Membership.Core.Data
{
    /// <summary>
    /// The deactivation database,
    /// We add DeactivationEntry entries here to indicate deactivated users.
    /// </summary>
    public interface IDeactivationRepository
    {
        /// <summary>
        /// Check if we have an entry as provided.
        /// </summary>
        bool Exists(DeactivationEntry entry);

        /// <summary>
        /// Register an user as being de-activated for an application.
        /// </summary>
        void Add(DeactivationEntry entry);

        /// <summary>
        /// De-register an user from being deactivated.
        /// </summary>
        void Remove(DeactivationEntry entry);
    }
}
