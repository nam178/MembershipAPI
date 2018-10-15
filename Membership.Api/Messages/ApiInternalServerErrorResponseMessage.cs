using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Membership.Api.Messages
{
    public sealed class ApiInternalServerErrorResponseMessage
    {
        public bool Success
        { get; set; }

        public string ErrorMessage
        { get; set; }
    }
}
