using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Membership.Api.Messages
{
    /// <summary>
    /// OAuth parameters from clients (tenant applications) to request
    /// token from us.
    /// 
    /// Notes
    /// We don't include ClientID, ClientSecret parameter
    /// here as they are handled speratedly by HTTP basic
    /// authentication.
    /// </summary>
    public sealed class ApiTokenRequestMessage
    {
        /// <summary>
        /// Grant type, must be 'password' at the moment because our OAuth server only support that.
        /// </summary>
        [FromQuery(Name = "grant_type")]
        [Required]
        public string GrantType
        { get; set; }

        /// <summary>
        /// The user's username, tenant application will have to display
        /// a login form to obtain username/password from their users
        /// and pass these to us.
        /// </summary>
        [FromQuery(Name = "username")]
        [Required]
        public string Username
        { get; set; }

        /// <summary>
        /// The user's password.
        /// </summary>
        [FromQuery(Name = "password")]
        [Required]
        public string Password
        { get; set; }
    }
}
