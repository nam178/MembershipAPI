using System;

namespace Membership.Api.Messages
{
    public class ApiCreateMemberResponseMessage
    {
        public bool Success
        { get; set; }

        public string ErrorMessage
        { get; set; }

        public Guid CreatedUserId
        { get; set; }
    }
}
