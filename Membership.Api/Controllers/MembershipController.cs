using Membership.Api.Messages;
using Membership.Core.Authentication;
using Membership.Core.Data;
using Membership.Core.Membership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Membership.Api.Controllers
{
    /// <summary>
    /// Various API endpoints for membership management.
    /// </summary>
    [ApiController]
    public class MembershipController : ControllerBase
    {
        readonly ICreateUserService _createUserService;
        readonly IUpdatePasswordService _updatePasswordService;
        readonly IDeactivationService _deactivationService;

        public MembershipController(
            ICreateUserService createUserService,
            IUpdatePasswordService updatePasswordService,
            IDeactivationService deactivationService)
        {
            _createUserService = createUserService 
                ?? throw new System.ArgumentNullException(nameof(createUserService));
            _updatePasswordService = updatePasswordService 
                ?? throw new System.ArgumentNullException(nameof(updatePasswordService));
            _deactivationService = deactivationService 
                ?? throw new System.ArgumentNullException(nameof(deactivationService));
        }

        /// <summary>
        /// POST: api/membership
        /// 
        /// Allows tenant application to create new membership
        /// 
        /// The clients (Tenant applications) must be athenticated to 
        /// use this endpoint (SchemeName.TenantApps)
        /// </summary>
        [Authorize(AuthenticationSchemes = SchemeName.TenantApps)]
        [HttpPost()]
        [Route("api/register")]
        public ActionResult<ApiCreateMemberResponseMessage> CreateUser([FromBody] ApiCreateMemberRequestMessage request)
        {
            if(false == _createUserService.TryCreateUser(
                request,
                out UserEntry user,
                out string err))
            {
                return BadRequest(new ApiCreateMemberResponseMessage
                {
                    Success = false,
                    ErrorMessage = err
                });
            }

            // Success!
            return new ApiCreateMemberResponseMessage
            {
                Success = true,
                CreatedUserId = user.Id
            };
        }

        /// <summary>
        /// PUT /api/password
        /// 
        /// Endpoint to update current user's password 
        /// (User must be authenticated with token, SchemeName.Member)
        /// </summary>
        [Authorize(AuthenticationSchemes = SchemeName.Member)]
        [Route("api/password")]
        [HttpPut]
        public ActionResult<ApiGenericResponseMessage> UpdatePassword([FromBody] ApiUpdatePasswordRequestMessage request)
        {
            if(false == _updatePasswordService.TryUpdatePassword(
                request.OldPassword, 
                request.NewPassword, 
                out string err))
            {
                return BadRequest(new ApiGenericResponseMessage
                {
                    Success = false,
                    ErrorMessage = err
                });
            }

            return new ApiGenericResponseMessage
            { Success = true };
        }

        /// <summary>
        /// PUT /api/activate
        /// 
        /// Endpoint to activate an user, used by an application's admin.
        /// (User must be authenticated with token, SchemeName.Member)
        /// </summary>
        [Authorize(AuthenticationSchemes = SchemeName.Member)]
        [Route("api/activate")]
        [HttpPut]
        public ActionResult<ApiGenericResponseMessage> ActivateUser([FromQuery] Guid userId)
        {
            return ActivateUserImpl(userId, activating: true);
        }

        /// <summary>
        /// PUT /api/deactivate
        /// 
        /// Endpoint to deactivate an user, used by an application's admin.
        /// (User must be authenticated with token, SchemeName.Member)
        /// </summary>
        [Authorize(AuthenticationSchemes = SchemeName.Member)]
        [Route("api/deactivate")]
        [HttpPut]
        public ActionResult<ApiGenericResponseMessage> DeactivateUser([FromQuery] Guid userId)
        {
            return ActivateUserImpl(userId, activating: false);
        }

        ActionResult<ApiGenericResponseMessage> ActivateUserImpl(Guid userId, bool activating)
        {
            if (false == _deactivationService.TrySetActivationStatus(
                            userId,
                            activating,
                            out string err))
            {
                return BadRequest(new ApiGenericResponseMessage
                {
                    Success = false,
                    ErrorMessage = err
                });
            }

            return new ApiGenericResponseMessage
            { Success = true };
        }
    }
}
