using System;
using System.Collections.Generic;
using System.Text;

namespace Membership.Core.Data
{
    /// <summary>
    /// Represents a client database, where we keep entries about tenant applications
    /// </summary>
    interface IOAuthClientRepository
    {
        /// <summary>
        /// Find an OAuth client by its unique ID
        /// </summary>
        /// <param name="id">The client's unique id</param>
        /// <returns>The client or NULL of not found</returns>
        OAuthClientEntry Find(string id);
    }
}
