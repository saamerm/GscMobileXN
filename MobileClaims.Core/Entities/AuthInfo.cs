using Newtonsoft.Json;
using System;

namespace MobileClaims.Core.Entities
{
    public class AuthInfo
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        private int _expiresIn;
        [JsonProperty("expires_in")]
        public int ExpiresIn
        {
            get => _expiresIn;
            set
            {
                _expiresIn = value;
                ExpiresAt = DateTime.Now.AddSeconds(_expiresIn);
            }
        }

        public DateTime ExpiresAt { get; private set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
