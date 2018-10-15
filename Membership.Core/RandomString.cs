using System;
using System.Text;

namespace Membership.Core
{
    /// <summary>
    /// Helper class to generate random string
    /// </summary>
    public static class RandomString
    {
        public const string AlphaNumericCharSet 
            = "abcdefghijklmnopqrstuvwxyzABCDEGGHIJKLMNOPQRSTUVWXYZ0123456789";

        /// <summary>
        /// Generate a random string
        /// </summary>
        public static string Generate(int length, string charSet = AlphaNumericCharSet, int? seed = null)
        {
            if (length <= 0)
                throw new ArgumentException(nameof(length));
            if (charSet == null)
                throw new ArgumentNullException(nameof(charSet));
            if (charSet.Length == 0)
                throw new ArgumentException(nameof(charSet));
            if (length >= 1024)
                throw new ArgumentOutOfRangeException(nameof(length));

            var random = seed == null ? new Random() : new Random((int)seed);
            var stringBuilder = new StringBuilder();

            for(var i = 0; i < length; i++)
            {
                stringBuilder.Append(charSet[random.Next(0, length)]);
            }

            return stringBuilder.ToString();
        }
    }
}
