using System;
using System.Collections.Generic;

namespace Membership.Core.Data
{
    /// <summary>
    /// In-memory repository to simulate a database,
    /// storing users 
    /// </summary>
    /// <remarks>This implementation is thread safe</remarks>
    public sealed class InMemoryUserRepository
        : IUserRepository
    {
        readonly List<UserEntry> _users = new List<UserEntry>();
        readonly Dictionary<Guid, UserEntry> _indexById = new Dictionary<Guid, UserEntry>();        // Simulate database indexes
        readonly Dictionary<string, UserEntry> _indexByName = new Dictionary<string, UserEntry>();  // Simulate database indexes
        readonly object _sync = new object();

        public UserEntry FindUserByUserId(Guid userId)
        {
            lock (_sync)
            {
                if (_indexById.ContainsKey(userId))
                {
                    return _indexById[userId].Clone();
                }
            }
            return null;
        }

        public UserEntry FindUserByName(string username)
        {
            lock(_sync)
            {
                if(_indexByName.ContainsKey(username))
                {
                    return _indexByName[username].Clone();
                }
            }
            return null;
        }

        public UserEntry AddUser(UserEntry user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // Clone the user to simulate that the data
            // is stored as a copy in disk.
            var clonedUser = user.Clone();

            lock(_sync)
            {
                if (_indexById.ContainsKey(clonedUser.Id))
                    throw new InvalidOperationException(
                        $"User exists with id {clonedUser.Id}"
                        );
                if(_indexByName.ContainsKey(clonedUser.Username))
                    throw new InvalidOperationException(
                        $"User exists with name {clonedUser.Username}"
                        );
                _users.Add(clonedUser);
                UserAdded(clonedUser);
            }

            return user;
        }

        public void Update(UserEntry user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            lock (_sync)
            {
                // Find this user
                if (false == _indexById.ContainsKey(user.Id))
                    throw new InvalidOperationException($"User not found by id {user.Id}");
                var storedUser = _indexById[user.Id];

                // If 'Username' field changes,
                // Remove the username index
                if (!string.Equals(user.Username, storedUser.Username, StringComparison.InvariantCulture))
                {
                    _indexByName.Remove(storedUser.Username);
                    _indexByName[user.Username] = storedUser;
                }

                // Push changes
                CopyProperties(fromUser: user, toUser: storedUser);
            }
        }

        void UserAdded(UserEntry clonedUser)
        {
            _indexById[clonedUser.Id] = clonedUser;
            _indexByName[clonedUser.Username] = clonedUser;
        }

        static void CopyProperties(UserEntry fromUser, UserEntry toUser)
        {
            toUser.Username = fromUser.Username;
            toUser.Password = fromUser.Password;
            toUser.PasswordSalt = fromUser.PasswordSalt;
            toUser.IsAdmin = fromUser.IsAdmin;
            toUser.ClientId = fromUser.ClientId;
            toUser.Email = fromUser.Email;
            toUser.FirstName = fromUser.FirstName;
            toUser.LastName = fromUser.LastName;
        }
    }
}
