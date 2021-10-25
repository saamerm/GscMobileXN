namespace MobileClaims.Core.Entities
{
    public class VersionInfo
    {
        public string Versioncode { get; private set; }

        public string VersionName { get; private set; }

        public VersionInfo(string versionCode, string versionName)
        {
            Versioncode = versionCode;
            VersionName = versionName;
        }
    }
}