using Membership.Core.Data;
using System;

namespace Membership.Core.Authentication
{
    public static class UserEntryExtensions
    {
        /// <summary>
        /// Helper method to check if the supplied password 
        /// is the correct password for the provided user.
        /// </summary>
        public static bool IsPassword(
            this UserEntry user, 
            string plainPassword)
        {
            if (plainPassword == null)
                throw new ArgumentNullException(nameof(plainPassword));

            return user.Password == new SHA256HashedString(plainPassword, user.PasswordSalt);
        }

        /// <summary>
        /// Helper method to update password for the supplied user.
        /// </summary>
        public static void SetPassword(
            this UserEntry user,
            string newPassword)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (newPassword == null)
                throw new ArgumentNullException(nameof(newPassword));
            user.PasswordSalt = RandomString.Generate(3);
            user.Password = new SHA256HashedString(
                newPassword, user.PasswordSalt);
        }
    }
}
