namespace MobileClaims.Core.Services.FacadeEntities
{
    public class RefreshTokenRequest
    {
        public string grant_type { get; set; }
        public string refresh_token { get; set; }
        public RefreshTokenRequest()
        {
            grant_type = "refresh_token";
        }
    }
}
