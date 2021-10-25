namespace MobileClaims.Core
{
    public partial class GSCHelper
    {
        public const string XEROX_AUTH_BASE_URL = null;
        public const string XEROX_AUTH_BASE_URL_SUB = null;
#if QA2
        public const string GSC_AUTH_BASE_URL = "https://testwww.clac.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "desktopmodules/clac.oauth.gsc/api/oauth/authorize";
#elif QA6
        public const string GSC_AUTH_BASE_URL = "https://testwww.clac.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "desktopmodules/clac.oauth.gsc/api/oauth/authorize";
       // public const string GSC_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
      //  public const string GSC_AUTH_BASE_URL_SUB = "idp-clac/accesstoken";
#elif PROD || PROD1 || PROD2
        public const string GSC_AUTH_BASE_URL = "https://clac.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "desktopmodules/clac.oauth.gsc/api/oauth/authorize";
#else // QA4 default
        // public const string GSC_AUTH_BASE_URL = "https://testwww.clac.ca/";
        // public const string GSC_AUTH_BASE_URL_SUB = "desktopmodules/clac.oauth.gsc/api/oauth/authorize";
        public const string GSC_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "idp-clac/accesstoken";
#endif
	}
}