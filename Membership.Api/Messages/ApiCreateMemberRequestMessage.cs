using Membership.Core.Membership;
using System.ComponentModel.DataAnnotations;

namespace Membership.Api.Messages
{
    public class ApiCreateMemberRequestMessage
    {
        [Required]
        public string Username
        { get; set; }

        [Required]
        public string FirstName
        { get; set; }

        [Required]
        public string LastName
        { get; set; }

        [EmailAddress]
        public string Email
        { get; set; }

        [Required]
        public string PlainPassword
        { get; set; }

        [Required]
        public bool IsAdmin
        { get; set; }

        public static implicit operator CreateUserRequest (ApiCreateMemberRequestMessage x)
        {
            return new CreateUserRequest
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PlainPassword = x.PlainPassword,
                Username = x.Username,
                IsAdmin = x.IsAdmin
            };
        }
    }
}
