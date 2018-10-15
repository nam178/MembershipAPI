using System;

namespace Membership.Core.Data
{
    /// <summary>
    /// Represents an user (a member in our membership API) 
    /// as stored in our user database
    /// </summary>
    public sealed class UserEntry
    {
        public Guid Id
        { get; set; }

        public string Email
        { get; set; }

        public string FirstName
        { get; set; }

        public string LastName
        { get; set; }

        public string Username
        { get; set; }

        public SHA256HashedString Password
        { get; set; }

        public string PasswordSalt
        { get; set; }

        /// <summary>
        /// The client (tenant application) which this member belongs to.
        /// </summary>
        public string ClientId
        { get; set; }

        /// <summary>
        /// Whenever this user is the admin of its application
        /// </summary>
        public bool IsAdmin
        { get; set; }

        public UserEntry Clone()
        {
            return new UserEntry
            {
                Id = Id,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Password = Password,
                PasswordSalt = PasswordSalt,
                Username = Username,
                ClientId = ClientId,
                IsAdmin = IsAdmin
            };
        }
    }
}
