namespace MobileClaims.Core
{
    public partial class GSCHelper
    {
        public const string XEROX_AUTH_BASE_URL = null;
        public const string XEROX_AUTH_BASE_URL_SUB = null;
#if QA2
        public const string GSC_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "idp-fppm/accesstoken";
#elif QA6
        public const string GSC_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "idp-fppm/accesstoken";        
#elif QA17
        public const string GSC_AUTH_BASE_URL = "https://auth-uat.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "idp-fppm/accesstoken";            
#elif PROD || PROD1 || PROD2
        public const string GSC_AUTH_BASE_URL = "https://idpsso.fppm.qc.ca:8443/"; 
        public const string GSC_AUTH_BASE_URL_SUB = "oauth-bridge/oauth/accesstoken";
#else // QA4 default        
        public const string GSC_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "idp-fppm/accesstoken";        
#endif
	}
}