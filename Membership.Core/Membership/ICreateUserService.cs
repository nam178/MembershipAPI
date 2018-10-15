using Membership.Core.Data;

namespace Membership.Core.Membership
{
    /// <summary>
    /// A service to create new membership account.
    /// </summary>
    public interface ICreateUserService
    {
        /// <summary>
        /// Try create user and return true if success,
        /// otherwise, false and output an error message
        /// </summary>
        bool TryCreateUser(
            CreateUserRequest request,
            out UserEntry user,
            out string errorMessage
            );
    }
}
