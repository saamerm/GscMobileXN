using MobileClaims.Core.Entities;

namespace MobileClaims.Core.Services.Responses
{
    public class AuthRefreshResponse
    {
        public bool IsSuccessful { get; private set; }

        public AuthInfo AuthInfo { get; private set; }

        internal AuthRefreshResponse(bool isSuccessful, AuthInfo authInfo)
        {
            IsSuccessful = isSuccessful;
            AuthInfo = authInfo;
        }
    }
}