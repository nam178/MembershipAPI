namespace Membership.Core.Membership
{
    /// <summary>
    /// This service allows the current authenticated user to update its password.
    /// </summary>
    public interface IUpdatePasswordService
    {
        /// <summary>
        /// Update current user's password, 
        /// returns true on success or false on failure
        /// </summary>
        /// <param name="oldPassword">The user's current password</param>
        /// <param name="newPassword">New password</param>
        /// <param name="errorMessage">The error message if the update fails</param>
        bool TryUpdatePassword(
            string oldPassword,
            string newPassword,
            out string errorMessage
            );
    }
}
