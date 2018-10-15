using Membership.Core.Data;
using System;

namespace Membership.Core.Authentication
{
    /// <summary>
    /// Implementation for IDeactivationService,
    /// uses authenticated client from current HTTP request and check against
    /// records from IDeactivationRepository
    /// </summary>
    public sealed class DeactivationService : IDeactivationService
    {
        readonly ICurrentUserService _currentUserService;
        readonly IDeactivationRepository _deactivations;
        readonly IUserRepository _users;

        public DeactivationService(
            ICurrentUserService currentUserService,
            IDeactivationRepository deactivations,
            IUserRepository users)
        {
            _currentUserService = currentUserService 
                ?? throw new ArgumentNullException(nameof(currentUserService));
            _deactivations = deactivations 
                ?? throw new ArgumentNullException(nameof(deactivations));
            _users = users 
                ?? throw new ArgumentNullException(nameof(users));
        }

        public bool IsUserDeactivated(Guid userId)
        {
            return _deactivations.Exists(new DeactivationEntry
            {
                OAuthClientId = _currentUserService.AuthenticatedClientId,
                UserId = userId 
            });
        }

        public bool TrySetActivationStatus(Guid userId, bool activate, out string err)
        {
            if (false == ValidateRequest(userId, out err))
                return false;
            
            var entry = new DeactivationEntry {
                UserId = userId,
                OAuthClientId = _currentUserService.AuthenticatedClientId
            };

            // Activating?
            // Ensure there is no deactivation entry for this user+application exists
            if (_deactivations.Exists(entry) && activate)
            {
                _deactivations.Remove(entry);
            }
            // De-activating?
            // Ensure there is a deactivation entry for this user+application
            else if (!_deactivations.Exists(entry) && !activate) {
                _deactivations.Add(entry);
            }

            err = null;
            return true;
        }

        bool ValidateRequest(Guid userId, out string err)
        {
            // Make sure the targeting user exists
            if (null == _users.FindUserByUserId(userId))
            {
                err = string.Format(Text.DeactivationService_FailedToFindUserById, userId);
                return false;
            }

            // Make sure only administrators OF CURRENT SYSTEM can perform this action
            if (false == _currentUserService.AuthenticatedUser.IsAdmin
                || _currentUserService.AuthenticatedUser.ClientId != _currentUserService.AuthenticatedClientId)
            {
                err = Text.DeactivationService_OnlyAdminCanDeactivate;
                return false;
            }

            err = null;
            return true;
        }
    }
}
