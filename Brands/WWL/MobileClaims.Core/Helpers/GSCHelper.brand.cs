namespace MobileClaims.Core
{
    public partial class GSCHelper
    {
        public const string XEROX_AUTH_BASE_URL = null;
        public const string XEROX_AUTH_BASE_URL_SUB = null;
#if QA2
        public const string GSC_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "oauth_qa2/accesstoken";
#elif QA6
        public const string GSC_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "oauth_qa6/accesstoken";
#elif QA17
        public const string GSC_AUTH_BASE_URL = "https://auth-uat.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "oauth/accesstoken";
#elif PROD || PROD1 || PROD2
        public const string GSC_AUTH_BASE_URL = "https://auth.gscservices.ca/";  
        public const string GSC_AUTH_BASE_URL_SUB = "oauth/accesstoken";            
#else // QA4 default
        public const string GSC_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "oauth_qa4/accesstoken";
#endif
	}
}