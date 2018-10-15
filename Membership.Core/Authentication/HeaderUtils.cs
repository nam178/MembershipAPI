using System.Text.RegularExpressions;

namespace Membership.Core.Authentication
{
    public static class HeaderUtils
    {
        static Regex _basicAuthHeaderPattern = new Regex(@"Basic\s+(.+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static Regex _bearerAuthHeaderPattern = new Regex(@"Bearer\s+(.+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Helper method to get the authorization string part
        /// from the header, i.e. 'Basic aGVsbG86d29ybGQ=' > 'aGVsbG86d29ybGQ='
        /// </summary>
        public static bool TryGetBasicAuthStr(
            string authHeader,
            out string authenticationString)
        {
            return TryGetAuthStr(authHeader, _basicAuthHeaderPattern, out authenticationString);
        }

        /// <summary>
        /// Helper method to get the authorization string part
        /// from the Athorization: Bearer header, i.e. 'Bearer xzy' > 'xyz'
        /// </summary>
        public static bool TryGetBearerAuthString(
            string authHeader,
            out string authenticationString)
        {
            return TryGetAuthStr(authHeader, _bearerAuthHeaderPattern, out authenticationString);
        }

        static bool TryGetAuthStr(
            string authHeader,
            Regex pattern,
            out string authenticationString)
        {
            var matches = pattern.Match(authHeader);
            if (!matches.Success)
            {
                authenticationString = null;
                return false;
            }

            authenticationString = matches.Groups[1].Value;
            return true;
        }
    }
}
