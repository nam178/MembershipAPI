using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Membership.Core.Data
{
    /// <summary>
    /// Token database, implemented in-memory.
    /// </summary>
    /// <remarks>This implementation is thread safe</remarks>
    sealed class InMemoryOAuthTokenRepository
        : IOAuthTokenRepository
    {
        readonly ConcurrentDictionary<string, OAuthTokenEntry> _data = new ConcurrentDictionary<string, OAuthTokenEntry>();
        
        // TODO: auto delete old tokens?

        public OAuthTokenEntry FindToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Supplied token is NULL or empty", nameof(token));

            if(_data.TryGetValue(token, out OAuthTokenEntry value))
            {
                return value.Clone();
            }

            return null;
        }

        public void AddToken(OAuthTokenEntry token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));
            
            // Copy the token, to simulate that the data is written
            // into database as a copy.
            var clone = token.Clone();

            // Add it
            if(false == _data.TryAdd(clone.Token, clone))
            {
                throw new InvalidOperationException(
                       $"Token with the same id {clone.Token} has already been adeded"
                        );
            }
        }
    }
}
