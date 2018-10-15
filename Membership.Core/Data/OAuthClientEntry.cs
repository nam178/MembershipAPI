namespace Membership.Core.Data
{
    /// <summary>
    /// Represenst an OAuth client (the other tenant application)
    /// as stored in our database
    /// </summary>
    sealed class OAuthClientEntry
    {
        /// <summary>
        /// Unqiue ID of this client, required for authentication
        /// </summary>
        public string Id
        { get; set; }

        /// <summary>
        /// This client's name, for example 'Application A'
        /// </summary>
        public string Name
        { get; set; }

        /// <summary>
        /// The password that this client keeps, for authentication with us.
        /// </summary>
        public SHA256HashedString Secret
        { get; set; }

        public OAuthClientEntry Clone()
        {
            return new OAuthClientEntry
            {
                Id = Id,
                Name = Name,
                Secret= Secret
            };
        }

        public override string ToString() => $"[{GetType().Name} {Name}]";
    }
}