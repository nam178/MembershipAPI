using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Membership.Api.Messages
{
    /// <summary>
    /// OAuth response message to the clients when authentication success.
    /// </summary>
    public class ApiTokenResponseMessage
    {
        [JsonProperty("access_token")]
        public string AccessToken
        { get; set; }

        [JsonProperty("token_type")]
        public string TokenType
        { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn
        { get; set; }
    }
}
