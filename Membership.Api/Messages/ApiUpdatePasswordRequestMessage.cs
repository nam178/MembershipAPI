using System.ComponentModel.DataAnnotations;

namespace Membership.Api.Messages
{
    public class ApiUpdatePasswordRequestMessage
    {
        [Required]
        public string OldPassword
        { get; set; }

        [Required]
        public string NewPassword
        { get; set; }
    }
}
