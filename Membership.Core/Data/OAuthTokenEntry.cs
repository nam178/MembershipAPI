using System;

namespace Membership.Core.Data
{
    /// <summary>
    /// The token entry as it stored in database
    /// </summary>
    sealed class OAuthTokenEntry
    {
        /// <summary>
        /// The bearer token
        /// </summary>
        public string Token
        { get; set; }

        /// <summary>
        /// The member/user whose this token represents
        /// </summary>
        public Guid UserId
        { get; set; }

        /// <summary>
        /// The unix timestamp when this token expires
        /// </summary>
        public int Expires
        { get; set; }

        /// <summary>
        /// The application which is token is issued for
        /// (i.e. the user can't use this token to perform actions related other other applications,
        /// such as disabling other applications' users)
        /// </summary>
        public string OAuthClientId
        { get; set; }

        public OAuthTokenEntry Clone()
        {
            return new OAuthTokenEntry
            {
                Token = Token,
                UserId = UserId,
                Expires = Expires,
                OAuthClientId = OAuthClientId
            };
        }

        public override string ToString()
        {
            return $"Token={Token}, Expires={Expires}";
        }
    }
}