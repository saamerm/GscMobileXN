using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class C4LOAuth
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("rs_uri")]
        public string RS_URI { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_description")]
        private string _errorDescription;
        public string ErrorDescription
        { 
            get
            {
                return _errorDescription;
            }
            set
            {
                _errorDescription = value;
                if (!string.IsNullOrEmpty(_errorDescription))
                    SetErrorCodeAndMessage();
            }
        }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
        
        public C4LOAuth()
        {
        }

        public C4LOAuth(string errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public void SetErrorCodeAndMessage()
        {
            ErrorCode = string.Empty;
            ErrorMessage = string.Empty;

            if (!string.IsNullOrEmpty(this.ErrorDescription))
            {
                string[] arrError = this.ErrorDescription.Split(':');
                if (arrError.Length == 2)
                {
                    ErrorCode = arrError[0];
                    ErrorMessage = arrError[1];
                }
            }
        }
    }
}