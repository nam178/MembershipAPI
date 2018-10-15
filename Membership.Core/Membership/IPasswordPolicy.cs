namespace Membership.Core.Membership
{
    public interface IPasswordPolicy
    {
        bool IsValid(string password, out string errorMessage);
    }
}