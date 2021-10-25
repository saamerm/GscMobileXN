namespace MobileClaims.Core
{
    public partial class GSCHelper
    {
        public const string XEROX_AUTH_BASE_URL = null;
        public const string XEROX_AUTH_BASE_URL_SUB = null;
#if QA2
        public const string GSC_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "idp-ccq/accesstoken";
#elif QA6
        public const string GSC_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "idp-ccq/accesstoken";
#elif PROD || PROD1 || PROD2
        public const string GSC_AUTH_BASE_URL = "https://sel.ccq.org/";  
        public const string GSC_AUTH_BASE_URL_SUB = "ServWeb/GestionSel/ZJWEB027/oauth/token";   
#elif QA4x
        public const string GSC_AUTH_BASE_URL = "https://sel01prep01.ccq.org/";
        public const string GSC_AUTH_BASE_URL_SUB = "ServWeb/GestionSel/ZJWEB027/oauth/token";   
#elif QA17
        public const string GSC_AUTH_BASE_URL = "https://auth-uat.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "idp-ccq/accesstoken";      
#else // QA4 default
        public const string GSC_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "idp-ccq/accesstoken";
#endif
	}
}