namespace MobileClaims.Core.Services.FacadeEntities
{
    public class C4LRequest
    {
        public string grant_type { get; private set; }
        public string token { get; set; }
        public string rs { get; private set; }
        public C4LRequest()
        {
            grant_type = "gsc_token";
            rs = "C4L";
        }

        public override string ToString()
        {
            return $"grant_type={grant_type}&rs={rs}&token={token}";
        }
    }
}
