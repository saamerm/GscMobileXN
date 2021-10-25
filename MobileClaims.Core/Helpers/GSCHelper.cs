using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MobileClaims.Core
{
	public partial class GSCHelper
	{
		// Service URLs        
		public const string IPSTACK_GEOCODING_BASE_URL = "http://api.ipstack.com/";

		// This value is populated from a build script. DO NOT RENAME/MOVE!
		public const string AppCenterSecret = "";

#if QA2
        public const string GSC_SERVICE_BASE_URL = "https://api-tst.gs-newtech.ca/";
        public const string GSC_SERVICE_BASE_URL_SUB = "planmemberservices_qa2";
		public const string GSC_APP_UPGRADE_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_APP_UPGRADE_BASE_URL_SUB = "oauth_qa2/client/validation";
        public const string C4L_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string C4L_AUTH_BASE_URL_SUB = "oauth_qa2/accesstoken";    
#elif QA6
        public const string GSC_SERVICE_BASE_URL = "https://api-tst.gs-newtech.ca/";
        public const string GSC_SERVICE_BASE_URL_SUB = "planmemberservices_qa6";
		public const string GSC_APP_UPGRADE_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_APP_UPGRADE_BASE_URL_SUB = "oauth_qa6/client/validation";
        public const string C4L_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string C4L_AUTH_BASE_URL_SUB = "oauth_qa6/accesstoken"; 
#elif SPSQA3
        public const string GSC_SERVICE_BASE_URL = "https://api-tst.gs-newtech.ca/";
        public const string GSC_SERVICE_BASE_URL_SUB = "planmemberservices_spsqa3";
		public const string GSC_APP_UPGRADE_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_APP_UPGRADE_BASE_URL_SUB = "oauth_spsqa3/client/validation";
        public const string C4L_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string C4L_AUTH_BASE_URL_SUB = "oauth_spsqa3/accesstoken"; 		
#elif SPSQA4
        public const string GSC_SERVICE_BASE_URL = "https://api-tst.gs-newtech.ca/";
        public const string GSC_SERVICE_BASE_URL_SUB = "planmemberservices_spsqa4";
		public const string GSC_APP_UPGRADE_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string GSC_APP_UPGRADE_BASE_URL_SUB = "oauth_spsqa4/client/validation";
        public const string C4L_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string C4L_AUTH_BASE_URL_SUB = "oauth_spsqa4/accesstoken"; 
#elif QA13
        public const string GSC_SERVICE_BASE_URL = "https://api-tst.gs-newtech.ca/";
        public const string GSC_SERVICE_BASE_URL_SUB = "planmemberservices";
	    public const string GSC_APP_UPGRADE_BASE_URL = "https://auth-tst.gs-newtech.ca";
        public const string GSC_APP_UPGRADE_BASE_URL_SUB = "oauth/client/validation";
        public const string C4L_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca";
        public const string C4L_AUTH_BASE_URL_SUB = "oauth/accesstoken";
#elif QA17
        public const string GSC_SERVICE_BASE_URL = "https://api-uat.gs-newtech.ca/";
        public const string GSC_SERVICE_BASE_URL_SUB = "planmemberservices";
	    public const string GSC_APP_UPGRADE_BASE_URL = "https://auth-uat.gs-newtech.ca/";
        public const string GSC_APP_UPGRADE_BASE_URL_SUB = "oauth/client/validation";
        public const string C4L_AUTH_BASE_URL = "https://auth-uat.gs-newtech.ca/";
        public const string C4L_AUTH_BASE_URL_SUB = "oauth/accesstoken";
#elif PROD
        public const string GSC_SERVICE_BASE_URL = "https://api.gscservices.ca/";
        public const string GSC_SERVICE_BASE_URL_SUB = "planmemberservices";
        public const string GSC_APP_UPGRADE_BASE_URL = "https://auth.gscservices.ca/";
        public const string GSC_APP_UPGRADE_BASE_URL_SUB = "oauth/client/validation";
        public const string C4L_AUTH_BASE_URL = "https://auth.gscservices.ca/";
        public const string C4L_AUTH_BASE_URL_SUB = "oauth/accesstoken";      
#elif PROD1
        public const string GSC_SERVICE_BASE_URL = "https://api.gscservices.ca/";
        public const string GSC_SERVICE_BASE_URL_SUB = "planmemberservices-v1";
        public const string GSC_APP_UPGRADE_BASE_URL = "https://auth.gscservices.ca/";
        public const string GSC_APP_UPGRADE_BASE_URL_SUB = "oauth/client/validation";
        public const string C4L_AUTH_BASE_URL = "https://auth.gscservices.ca/";
        public const string C4L_AUTH_BASE_URL_SUB = "oauth/accesstoken";      
#elif PROD2
        public const string GSC_SERVICE_BASE_URL = "https://api.gscservices.ca/";
        public const string GSC_SERVICE_BASE_URL_SUB = "planmemberservices-v2";
        public const string GSC_APP_UPGRADE_BASE_URL = "https://auth.gscservices.ca/";
        public const string GSC_APP_UPGRADE_BASE_URL_SUB = "oauth/client/validation";
        public const string C4L_AUTH_BASE_URL = "https://auth.gscservices.ca/";
        public const string C4L_AUTH_BASE_URL_SUB = "oauth/accesstoken";       
#else // QA4 default
		public const string GSC_SERVICE_BASE_URL = "https://api-tst.gs-newtech.ca/";
        public const string GSC_SERVICE_BASE_URL_SUB = "planmemberservices_qa4";		
        public const string GSC_APP_UPGRADE_BASE_URL = "https://auth-tst.gs-newtech.ca/";
		public const string GSC_APP_UPGRADE_BASE_URL_SUB = "oauth_qa4/client/validation";
		public const string C4L_AUTH_BASE_URL = "https://auth-tst.gs-newtech.ca/";
        public const string C4L_AUTH_BASE_URL_SUB = "oauth_qa4/accesstoken";
#endif

        // Current Language
        public static string CURRENT_LANGUAGE = "en-CA";

		// Date formats
		public const string GSC_DATE_FORMAT = "yyyy/MM/dd";
		public const string YTD_DATE_FORMAT = "MMMM dd, yyyy";
		public const string DATE_OF_INQUIRY_FORMAT = "MMM dd, yyyy hh:mm tt";
		public const string CLAIMS_HISTORY_SEARCH_DATE_FORMAT_IOS = "yyyy-MM-dd";
		public const string CLAIMS_HISTORY_SEARCH_DATE_FORMAT_ANDROID = "MMM dd, yyyy";

		// Money regex
		public const string MONEY_REGEX = @"^\d+[.,]?\d{0,2}$";

		public static int SecondsToDelaySplashPage = 10;

		public const string orientation_PortraitUp = "PortraitUp";
		public const string orientation_PortraitDown = "PortraitDown";
		public const string orientation_LandscapeLeft = "LandscapeLeft";
		public const string orientation_LandscapeRight = "LandscapeRight";

		//change for life agreement link to GSC term and condition 
		public const string LinkToPrivacy = "LinkPrivacy";
		public const string LinkToLegal = "LinkLegal";
		public const string LinkToSecurity = "LinkSecurity";

		// Claims History
		public const string DefaultClaimsHistoryTypeID = "CL";
		public const string ClaimsHistoryTypePDTID = "PD";
		public const string ClaimsHistoryTypeMIID = "MI";
		public const string ClaimHistoryPayeeAllID = "AL";
		public const string DefaultClaimHistoryBenefitID = "100000";
		public const string ClaimHistoryDentalPredeterminationID = "PD";
		public const string ClaimHistoryMedicalItemID = "MI";
		public const string ClaimHistoryBenefitIDForAll = "100000";
		public const string ClaimHistoryBenefitIDForDental = "100005";
		public const string ClaimHistoryBenefitIDForDrug = "100006";
		public const string ClaimHistoryBenefitIDForEHS = "100007";
		public const string ClaimHistoryBenefitIDForHCSA = "110048";
		public const string ClaimHistoryBenefitIDForNonHealth = "100004";
		public static DateTime StartDateOfCurrentYear = new DateTime(DateTime.Now.Year, 1, 1);
		public const string DisplayByYearToDateKey = "YTD";
		public const string DisplayByDateRangeKey = "DR";
		public const string DisplayByYearKey = "YR";
		public const int ClaimHistoryDefaultEarliestYear = 2008;
		public const string EFTPaymentMethodCode = "EF";
		public const string ChequePaymentMethodCode = "CH";
		public const string CANCurrencyCode = "CAN";
		public const string USCurrencyCode = "USA";

		// Amounts
		public const double MaxAmountPaidByPPorGP = 999.99;

		public enum OS
		{
			Droid,
			iOS,
			Windows,
			WP
		}

		public static double GetDollarAmount(string moneyString)
		{
			double amt = 0.0;
			if (!string.IsNullOrEmpty(moneyString))
			{
				moneyString = moneyString.Replace(',', '.');
				double.TryParse(moneyString, NumberStyles.Currency, CultureInfo.InvariantCulture, out amt);
			}
			return amt;
		}

		public static bool IsXeroxUser(string username)
		{
			if (string.IsNullOrEmpty(username))
				return false;

			string _condition = @"^[a-zA-Z0-9]{3,4}-.*";

			return Regex.Match(username, _condition).Success;
		}

		public static bool IsXeroxSupported(string loggerFragment, string languageFragment, string dataFragment)
		{
			return 
				!string.IsNullOrEmpty(XEROX_AUTH_BASE_URL) && 
				!string.IsNullOrEmpty(XEROX_AUTH_BASE_URL_SUB) &&
				!string.IsNullOrEmpty(loggerFragment) && 
				!string.IsNullOrEmpty(languageFragment) && 
				!string.IsNullOrEmpty(dataFragment);
		}

		public static string GetParticipantNumberFromPlanMemberID(string planMemberID)
		{
			string trimmed = planMemberID;
			if (planMemberID.IndexOf('-') > -1)
			{
				trimmed = planMemberID.Substring(planMemberID.IndexOf('-') + 1, 2);
			}
			return trimmed;
		}

		public static string TrimParticipantNumberFromPlanMemberID(string planMemberID)
		{
			string trimmed = planMemberID;
			if (planMemberID.IndexOf('-') > -1)
			{
				trimmed = planMemberID.Substring(0, planMemberID.IndexOf('-'));
			}
			return trimmed;
		}

		public static string GetLocalizedString(string name)
		{
			return Resource.ResourceManager.GetString(name);
		}

		//#if QA_BUILD
		//       public const string GSC_SERVICE_BASE_URL = "https://mobileapps.gs-newtech.ca/";
		//       public const string GSC_SERVICE_BASE_URL_SUB = "planmemberservices";
		//		 public const string GSC_APP_UPGRADE_BASE_URL = "https://auth.gs-newtech.ca/";
		//       public const string C4L_AUTH_BASE_URL = "https://auth.gs-newtech.ca/";  
		//       public const string C4L_AUTH_BASE_URL_SUB = "oauth/accesstoken";       
		//       public const string XEROX_AUTH_BASE_URL = "https://services.buckconsultants.ca/";
		//       public const string XEROX_AUTH_BASE_URL_SUB = "uat81_apps/gscmobile/xerox_authorization.ashx";
		//#if ENCON
		//        public const bool IsGSSCBuild = false;
		//        public const string GSC_AUTH_BASE_URL = "https://auth.gs-newtech.ca/";
		//        public const string GSC_AUTH_BASE_URL_SUB = "idp-encon/accesstoken";        
		//#elif CLAC
		//        public const bool IsGSSCBuild = false;
		//        public const string GSC_AUTH_BASE_URL = "https://testwww.clac.ca/";
		//        public const string GSC_AUTH_BASE_URL_SUB = "desktopmodules/clac.oauth.gsc/api/oauth/authorize";           
		//#elif FPPM
		//        public const bool IsGSSCBuild = false;       
		//        public const string GSC_AUTH_BASE_URL = "https://auth.gs-newtech.ca/";
		//        public const string GSC_AUTH_BASE_URL_SUB = "idp-fppm/accesstoken";        
		//#elif WestJet
		//        public const bool IsGSSCBuild = false;
		//        public const string GSC_AUTH_BASE_URL = "https://services.buckconsultants.ca/";
		//        public const string GSC_AUTH_BASE_URL_SUB = "uat81_apps/gscmobile/westjet_authorization.ashx";        
		//#else
		//        public const bool IsGSSCBuild = true;
		//        public const string GSC_AUTH_BASE_URL = "https://auth.gs-newtech.ca/";
		//        public const string GSC_AUTH_BASE_URL_SUB = "oauth/accesstoken";         
		//#endif
	}
}
