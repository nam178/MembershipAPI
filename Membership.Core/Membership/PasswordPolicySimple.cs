using System;

namespace Membership.Core.Membership
{
    public class PasswordPolicySimple : IPasswordPolicy
    {
        public bool IsValid(string password, out string errorMessage)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (password.Length < 6)
            {
                errorMessage = "Password is too short";
                return false;
            }

            errorMessage = null;
            return true;
        }
    }
}
