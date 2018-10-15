using Membership.Core.Authentication;
using Membership.Core.Data;
using System;

namespace Membership.Core.Membership
{
    /// <summary>
    /// Implementation for IUpdatePasswordService that uses the user
    /// in current HTTP context.
    /// </summary>
    sealed class UpdatePasswordService : IUpdatePasswordService
    {
        readonly ICurrentUserService _currentUserService;
        readonly IUserRepository _users;
        readonly IPasswordPolicy _passwordPolicy;

        public UpdatePasswordService(
            ICurrentUserService currentUserService,
            IUserRepository users,
            IPasswordPolicy passwordPolicy)
        {
            _currentUserService = currentUserService 
                ?? throw new ArgumentNullException(nameof(currentUserService));
            _users = users 
                ?? throw new ArgumentNullException(nameof(users));
            _passwordPolicy = passwordPolicy 
                ?? throw new ArgumentNullException(nameof(passwordPolicy));
        }

        public bool TryUpdatePassword(
            string oldPassword, 
            string newPassword, 
            out string errorMessage)
        {
            if (oldPassword == null)
                throw new ArgumentNullException(nameof(oldPassword));
            if (newPassword == null)
                throw new ArgumentNullException(nameof(newPassword));

            // Ensure current password is correct
            if (!_currentUserService.AuthenticatedUser.IsPassword(oldPassword))
            {
                errorMessage = Text.UpdatePasswordService_IncorrectCurrentPassword;
                return false;
            }

            // Ensure the new password is valid.
            if (!_passwordPolicy.IsValid(newPassword, out errorMessage))
                return false;

            // Change password and save
            _currentUserService.AuthenticatedUser.SetPassword(newPassword);
            _users.Update(_currentUserService.AuthenticatedUser);
            return true;
        }
    }
}
