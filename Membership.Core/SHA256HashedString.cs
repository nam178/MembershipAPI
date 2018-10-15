using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Membership.Core
{
    /// <summary>
    /// Util to create HASH256 hash as as tring
    /// </summary>
    public sealed class SHA256HashedString : IEquatable<SHA256HashedString>
    {
        readonly string _value;

        const string DefaultSalt = "z2,i";

        /// <summary>
        /// Create a SHA256 hash from original string with default salt
        /// </summary>
        public SHA256HashedString(string orignal)
            : this(orignal, DefaultSalt)
        {
            
        }

        /// <summary>
        /// Create a SHA256 hash from original string with salt
        /// </summary>
        public SHA256HashedString(string original, string salt)
        {
            var hashAlgorithm = SHA256Managed.Create();
            if (salt != null)
                original += salt;
            var hash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(original));
            var result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            _value = result.ToString();
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(obj, null) == false
                && (obj is SHA256HashedString)
                && string.Equals(((SHA256HashedString)obj)._value, _value, StringComparison.InvariantCulture);
        }

        public bool Equals(SHA256HashedString other)
        {
            return ReferenceEquals(other, null) == false
                   && string.Equals(other._value, _value, StringComparison.InvariantCulture);
        }

        public override int GetHashCode()
        {
            return -1939223833 + EqualityComparer<string>.Default.GetHashCode(_value);
        }

        public static bool operator==(SHA256HashedString x, SHA256HashedString y)
        {
            if (ReferenceEquals(x, null) && ReferenceEquals(y, null))
                return true;
            else if (!ReferenceEquals(x, null) && !ReferenceEquals(y, null))
                return x._value == y._value;
            return 
                false;
        }

        public static bool operator !=(SHA256HashedString x, SHA256HashedString y)
        {
            return false == (x == y);
        }

        public override string ToString() => _value;
    }
}
