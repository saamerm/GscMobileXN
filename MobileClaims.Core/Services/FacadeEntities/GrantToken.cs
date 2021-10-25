namespace MobileClaims.Core.Services.FacadeEntities
{
    public class GrantToken
    {
        public string grant_type { get; set; }
        public string token { get; set; }
        public string rs { get; set; }
        public GrantToken(string Token, string RS)
        {
            token = Token;
            rs = RS;
            grant_type = "gsc_token";
        }
    }
}
