namespace MobileClaims.Core.Services.FacadeEntities
{
    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
        public string grant_type { get; set; }
        public User()
        {
            grant_type = "password";
        }

        public override string ToString()
        {
            return $"grant_type={grant_type}&username={username}&password={password}";
        }
    }
}
