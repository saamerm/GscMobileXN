namespace MobileClaims.Core
{
    public partial class GSCHelper
    {
        public const string XEROX_AUTH_BASE_URL = null;
        public const string XEROX_AUTH_BASE_URL_SUB = null;
#if QA2
        public const string GSC_AUTH_BASE_URL = "https://services.buckconsultants.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "uat81_apps/gscmobile/westjet_authorization.ashx";
#elif QA6
        // public const string GSC_AUTH_BASE_URL = "https://services.buckconsultants.ca/";
        // public const string GSC_AUTH_BASE_URL_SUB = "uat81_apps/gscmobile/westjet_authorization.ashx";
        public const string GSC_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "idp-westjet/accesstoken";
#elif PROD || PROD1 || PROD2
        public const string GSC_AUTH_BASE_URL = "https://secure.myflexadvantage.ca/";  
        public const string GSC_AUTH_BASE_URL_SUB = "gscmobile/westjet_authorization.ashx";
#else // QA4 default
        // public const string GSC_AUTH_BASE_URL = "https://services.buckconsultants.ca/";
        // public const string GSC_AUTH_BASE_URL_SUB = "uat81_apps/gscmobile/westjet_authorization.ashx";
        public const string GSC_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_AUTH_BASE_URL_SUB = "idp-westjet/accesstoken";
#endif
	}
}