using Membership.Core.Authentication;
using Membership.Core.Data;
using System;

namespace Membership.Core.Membership
{
    /// <summary>
    /// Implementation for CreateUserService
    /// </summary>
    sealed class CreateUserService : ICreateUserService
    {
        readonly IUserRepository _users;
        readonly IPasswordPolicy _passwordPolicy;
        readonly ICurrentUserService _currentUser;

        public CreateUserService(
            IUserRepository users, 
            IPasswordPolicy passwordPolicy,
            ICurrentUserService currentUser)
        {
            _users = users 
                ?? throw new ArgumentNullException(nameof(users));
            _passwordPolicy = passwordPolicy 
                ?? throw new ArgumentNullException(nameof(passwordPolicy));
            _currentUser = currentUser 
                ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public bool TryCreateUser(
            CreateUserRequest request,
            out UserEntry user,
            out string errorMessage)
        {
            user = null;
            errorMessage = null;

            // Validate username
            if (!EnsureUniqueUsername(request.Username, out errorMessage))
                return false;

            // Validate password
            if (false == _passwordPolicy.IsValid(request.PlainPassword, out errorMessage))
                return false;

            // Add user
            user = new UserEntry
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                ClientId = _currentUser.AuthenticatedClientId,
                IsAdmin = request.IsAdmin,
                Email = request.Email,
                Username = request.Username
            };
            user.SetPassword(request.PlainPassword);
            _users.AddUser(user);
            return true;
        }

        bool EnsureUniqueUsername(string username, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty");

            if (_users.FindUserByName(username) != null)
            {
                errorMessage = Text.CreateUserService_UserExists;
                return false;
            }

            errorMessage = null;
            return true;
        }
    }
}
