using System;

namespace Membership.Core
{
    static class UnixTimestamp
    {
        public static int FromDateTime(DateTime dateTime)
        {
            return (int)(dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)))
                    .TotalSeconds;
        }
    }
}
