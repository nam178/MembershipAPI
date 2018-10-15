using System;
using System.Collections.Concurrent;

namespace Membership.Core.Data
{
    /// <summary>
    /// Implementation of IOAuthClientRepository that stores data in memory
    /// </summary>
    /// <remarks>This implementation is thread safe</remarks>
    sealed class InMemoryOAuthClientRepository
        : IOAuthClientRepository
    {
        readonly ConcurrentDictionary<string, OAuthClientEntry> _data 
               = new ConcurrentDictionary<string, OAuthClientEntry>();

        /// <summary>
        /// Find a client by its unique id
        /// </summary>
        /// <returns>The client or NULL if not found</returns>
        public OAuthClientEntry Find(string id)
        {
            if (_data.TryGetValue(id, out OAuthClientEntry result))
                return result.Clone();
            return null;
        }

        /// <summary>
        /// Add a client into this repository
        /// </summary>
        /// <exception cref="InvalidOperationException">When adding fails</exception>
        public void Add(OAuthClientEntry client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            // We'll clone this client to store it,
            // to prevent others from updating the client
            // without our knowledge.
            var clone = client.Clone();
            if (string.IsNullOrWhiteSpace(clone.Id))
                throw new ArgumentException("Client must have an ID");
            if (false == _data.TryAdd(clone.Id, clone))
                throw new InvalidOperationException($"Client with id {client.Id} already exist");
        }
    }
}
