namespace MobileClaims.Core.Services.FacadeEntities
{
    public class RefreshRequest
    {
        public string grant_type { get; set; }
        public string refresh_token { get; set; }
        public RefreshRequest(string token)
        {
            refresh_token = token;
            grant_type = "refresh_token";
        }
        public override string ToString()
        {
            return $"grant_type={grant_type}&refresh_token={refresh_token}";
        }
    }
}
