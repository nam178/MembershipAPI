using System;

namespace Membership.Core.Data
{
    public interface IUserRepository
    {
        /// <summary>
        /// Find an user with given name
        /// </summary>
        /// <returns>The user if found, otherwise, NULL</returns>
        UserEntry FindUserByName(string name);

        /// <summary>
        /// Find an user with given id
        /// </summary>
        /// <returns>The user if found, otherwise, NULL</returns>
        UserEntry FindUserByUserId(Guid userId);

        /// <summary>
        /// Add user
        /// </summary>
        UserEntry AddUser(UserEntry user);

        /// <summary>
        /// Update specified user
        /// </summary>
        void Update(UserEntry user);
    }
}