namespace MobileClaims.Core
{
    public partial class GSCHelper
    {
        public const string XEROX_AUTH_BASE_URL = null;
        public const string XEROX_AUTH_BASE_URL_SUB = null;
#if QA2
        //public const string GSC_AUTH_BASE_URL = "https://sts.encon.ca/";
        //public const string GSC_AUTH_BASE_URL_SUB = "authtest/accesstoken";
        public const string GSC_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "idp-encon/accesstoken";
#elif QA6
        // public const string GSC_AUTH_BASE_URL = "https://sts.encon.ca/";
        // public const string GSC_AUTH_BASE_URL_SUB = "authtest/accesstoken";
        public const string GSC_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "idp-encon/accesstoken";
#elif PROD || PROD1 || PROD2
        public const string GSC_AUTH_BASE_URL = "https://sts.encon.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "auth/accesstoken";
#else // QA4 default
        // public const string GSC_AUTH_BASE_URL = "https://sts.encon.ca/";
        // public const string GSC_AUTH_BASE_URL_SUB = "authtest/accesstoken";
        public const string GSC_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "idp-encon/accesstoken";
#endif
	}
}