namespace Membership.Core.Membership
{
    /// <summary>
    /// Represents a request to create new membership account,
    /// containing required information.
    /// </summary>
    public sealed class CreateUserRequest
    {
        public string Username
        { get; set; }

        public string FirstName
        { get; set; }

        public string LastName
        { get; set; }

        public string Email
        { get; set; }

        public string PlainPassword
        { get; set; }

        public bool IsAdmin
        { get; set; }
    }
}