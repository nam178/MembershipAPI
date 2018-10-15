using System;
using System.Collections.Generic;

namespace Membership.Core.Data
{
    sealed class InMemoryDeactivationRepository
        : IDeactivationRepository
    {
        // Simulate a key value database,
        // The keys are user ids, those who de-activated.
        // The values are set of applications which the users were deactivated.
        readonly Dictionary<Guid, HashSet<string>> _index = new Dictionary<Guid, HashSet<string>>();

        public void Add(DeactivationEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));
            var clientId = ValidateClientId(entry);

            lock (_index)
            {
                if (false == _index.ContainsKey(entry.UserId))
                    _index[entry.UserId] = new HashSet<string>();

                if (false == _index[entry.UserId].Contains(clientId))
                    _index[entry.UserId].Add(clientId);
            }
        }

        public bool Exists(DeactivationEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            var clientId = ValidateClientId(entry);

            lock (_index)
            {
                return _index.ContainsKey(entry.UserId)
                    && _index[entry.UserId].Contains(clientId);
            }
        }

        public void Remove(DeactivationEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            var clientId = ValidateClientId(entry);

            lock (_index)
            {
                if(_index.ContainsKey(entry.UserId))
                {
                    if(_index[entry.UserId].Contains(clientId))
                        _index[entry.UserId].Remove(clientId);

                    // Last entry removed? clean up
                    if (_index[entry.UserId].Count == 0)
                        _index.Remove(entry.UserId);
                }
            }
        }

        static string ValidateClientId(DeactivationEntry entry)
        {
            string clientId = entry.OAuthClientId;
            if (clientId == null)
                throw new ArgumentException(nameof(entry.OAuthClientId) + " cannot be null");
            return clientId;
        }
    }
}
