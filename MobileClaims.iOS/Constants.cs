using System;
using UIKit;
using Foundation;

namespace MobileClaims.iOS
{
    public static partial class Constants
    {
        public static string TEMPLATE_NAME = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone ? "TemplatePhone" : "Template";

        //public static float DEVICE_WIDTH = (float)UIScreen.MainScreen.Bounds.Width;
        //public static float DEVICE_HEIGHT = (float)UIScreen.MainScreen.Bounds.Height;
        public static float STATUS_BAR_HEIGHT = 20; //Status bar height peculiar in landscape. TODO: Figure a better way to get status bar height

        public static float ELIGIBILITY_TABLE_FONT_SIZE = 10f;
        public static float SMALL_FONT_SIZE = 12f;
        public static float MAIN_NAV_FONT_SIZE = Constants.IsPhone() ? 10.0f : 14f;
        public static float TAB_BAR_HEIGHT = 15f;
        public static float TAB_BAR_BUTTON_HEIGHT = 7f;
        public static float TAB_BAR_BUTTON_WIDTH = 10f;
        public static float FIELD_BORDER_SIZE = 1.0f;

        public static float STATUS_BAR_OFFSET = Constants.IS_OS_7_OR_LATER() ? 20 : 0;
        public static float STATUS_BAR_CARD_OFFSET = Constants.IS_OS_7_OR_LATER() ? 15 : -3;
        public static float PHONE_STATUS_CARD_PAGING_OFFSET = Constants.IS_OS_7_OR_LATER() ? 12 : -6;

        //HCSA Claim Type
        public static float HEADING_HCSA_FONT_SIZE = 13.0f;
        public static float HEADING_CLAIMTYPES = 24.0f;

        //green button
        public static float GREEN_BUTTON_FONT_SIZE = 24;

        public static float DROPDOWN_TOP_PADDING = Constants.IsPhone() ? 12 : 20;
        public static float DROPDOWN_SIDE_PADDING = Constants.IsPhone() ? 20 : 20;
        public static float DROPDOWN_HEIGHT = Constants.IsPhone() ? 48 : 20;
        public static float DROPDOWN_LABEL_PADDING = Constants.IsPhone() ? 12 : 12;
        public static float DROPDOWN_FONT_SIZE = Constants.IsPhone() ? 18.0f : 18.0f;
        public static float TEXTVIEW_FONT_SIZE = Constants.IsPhone() ? 13.0f : 14.0f;
        public static float FONT_SIZE_13_15 = Constants.IsPhone() ? 13.0f : 15.0f;

        public static float LABEL_SIDE_PADDING = Constants.IsPhone() ? 20 : 25;
        public static float TNC_SIDE_PADDING = Constants.IsPhone() ? 20 : 60;

        public static float LABEL_TOP_PADDING = Constants.IsPhone() ? 20 : 25;
        public static float DROPDOW_GREEN_BUTTON_HEIGHT = Constants.IsPhone() ? 60 : 60;

        public static float GREEN_BUTTON_HEIGHT = Constants.IsPhone() ? 50 : 60;
        public static float GREEN_BUTTON_WIDTH = Constants.IsPhone() ? 280 : 280;

        public static float TEXT_FIELD_HEIGHT = Constants.IsPhone() ? 30 : 40;

        public static float LABEL_HEIGHT = Constants.IsPhone() ? 18 : 25;

        //HSCA ClaimDetailsHCSAView
        public static float HEADING_SIZE = Constants.IsPhone() ? 22.0f : 24.0f;
        public static float HEADING_LABEL_HEIGHT = Constants.IsPhone() ? 23 : 30;

        public static float PARTICIPANT_LIST_BUTTON_HEIGHT = 40.0f;
        public static float LIST_BUTTON_HEIGHT = 50.0f;
        public static float BUTTON_SIDE_PADDING = Constants.IsPhone() ? 20 : 25;
        public static float GREEN_BUTTON_SIDE_PADDING = Constants.IsPhone() ? 20 : 190;

        public static float DRUG_BUTTON_HEIGHT = Constants.IsPhone() ? 50 : 65;
        public static float BUTTON_TEXT_PADDING = 15.0f;

        public static float CONTENT_SIDE_PADDING = Constants.IsPhone() ? 20 : 25;

        public static float CLAIMS_TOP_PADDING = Constants.IsPhone() ? 20 : 25;
        public static float DRUG_LOOKUP_TOP_PADDING = 20;
        public static float DRUG_LOOKUP_SIDE_PADDING = 10;
        public static float DRUG_LOOKUP_PADDED_LABEL_HEIGHT = 35;
        public static float DRUG_LOOKUP_LABEL_HEIGHT = 25;
        public static float DRUG_LOOKUP_FIELD_HEIGHT = 45;
        public static float DRUG_LOOKUP_LEFT_TEXT_INSET = 25;
        public static float DRUG_LOOKUP_FIELD_HEIGHT_SMALL = 35;
        public static float DRUG_LOOKUP_BUTTON_PADDING = Constants.IsPhone() ? 20 : 30;

        public static float DashboardNotificationHeight = Constants.IsPhone() ? 44.0f : 36.0f;
        public static float DASHBOARD_WELCOME_FONT_SIZE = Constants.IsPhone() ? 26.0f : 60.0f;
        public static float DASHBOARD_PARTICIPANT_NAME_LABEL_SIZE = Constants.IsPhone() ? 22.0f : 45.0f;
        public static float DASHBOARD_SECTION_TITLE_FONT_SIZE = Constants.IsPhone() ? 20.0f : 30.0f;
        public static float DASHBOARD_LARGE_BUTTON_FONT_SIZE = 22.0f;
        public static float DASHBOARD_MEDIUM_BUTTON_FONT_SIZE = 18.0f;
        public static float DASHBOARD_SMALL_BUTTON_FONT_SIZE = 14.0f;

        public static float BOTTOM_NAVIGATION_BAR_FONT_SIZE = Constants.IsPhone() ? 11.0f : 14.0f;

        public static float WELCOME_HEADING_FONT_SIZE = Constants.IsPhone() ? 32.0f : 64.0f;
        public static float LG_HEADING_FONT_SIZE = Constants.IsPhone() ? 22.0f : 22.0f;
        public static float PARTICPAINT_RESULTS_FONT_SIZE = Constants.IsPhone() ? 20.0f : 20.0f;
        public static float HEADING_FONT_SIZE = Constants.IsPhone() ? 14.0f : 14.0f;
        public static float HEADING_FONT_SIZE2 = Constants.IsPhone() ? 14.5f : 14.5f;
        public static float SUB_HEADING_FONT_SIZE = Constants.IsPhone() ? 12.0f : 12.0f;
        public static float NAV_BAR_FONT_SIZE = Constants.IsPhone() ? 16.0f : 20.0f;
        public static float NAV_BAR_BUTTON_SIZE = Constants.IsPhone() ? 12.0f : 14.0f;

        public static float DETAILS_FONT_SIZE = Constants.IsPhone() ? 12.0f : 20.0f;
        public static float LOGIN_REGISTER_FONT_SIZE = Constants.IsPhone() ? 9.0f : 18.0f;
        public static float SMALL_DETAILS_FONT_SIZE = Constants.IsPhone() ? 9.0f : 18.0f;
        public static float BUTTON_FONT_SIZE = Constants.IsPhone() ? 18.0f : 18.0f;
        public static float TEXT_FIELD_LABEL_SIZE = Constants.IsPhone() ? 12.0f : 18.0f;
        public static float GS_BUTTON_FONT_SIZE = Constants.IsPhone() ? 14.0f : 24.0f;
        public static float LIST_ITEM_FONT_SIZE = Constants.IsPhone() ? 14.0f : 18.0f;
        public static float SELECTION_ITEM_FONT_SIZE = Constants.IsPhone() ? 14.0f : 18.0f;
        public static float LANDING_BUTTON_FONT_SIZE = Constants.IsPhone() ? 24.0f : 46.0f;
        public static float GS_SELECTION_BUTTON = Constants.IsPhone() ? 12.0f : 12.0f;
        public static float GS_SELECTION_BUTTON_SMALL = Constants.IsPhone() ? 12.0f : 14.0f;
        public static float TEXT_FIELD_FONT_SIZE = Constants.IsPhone() ? 14.0f : 20.0f;
        public static float TERMS_TEXT_FONT_SIZE = Constants.IsPhone() ? 12.0f : 14.0f;


        public static float SIZE_10 = 10.0f;
        public static float SIZE_30 = 30.0f;

        public static string NUNITO_SEMIBOLD = "NunitoSans-SemiBold"; //replaced of Avenir-Roman(153 Reference)
        public static string NUNITO_REGULAR = "NunitoSans-Regular";  //replaced of AvenirLTStd-Book(108 referece)
        public static string NUNITO_BOLD = "NunitoSans-Black";   //replaced of AvenirLTStd-Black(288 reference)
        public static string NUNITO_HEAVY = "NunitoSans-Bold";  //replaced of AvenirLTStd-Heavy(138 reference)
        public static string NUNITO_BLACK = "NunitoSans-Black"; //SYSTEM replace of Avenir-Black (35 reference)
        public static string NUNITO_MEDIUM = "NunitoSans-Bold"; //SYSTEM  replace of Avenir-Black (4 reference)

        public static string LEAGUE_GOTHIC = "LeagueGothic";// long font

        public static string HTML_FONT = Constants.NUNITO_SEMIBOLD;
        public static string HTML_HEADING_FONT = "LeagueGothic";

        public static float DIVIDER_HEIGHT = 2;

        public static float STACKED_MENU_LABEL_HEIGHT = 10;
        public static float STACKED_MENU_LABEL_PADDING = 3;
        public static float STACKED_MENU_LABEL_INSET = 10;
        public static float STACKED_MENU_TOTAL_HEIGHT = STACKED_MENU_LABEL_INSET * 2 + STACKED_MENU_LABEL_HEIGHT * 3 + STACKED_MENU_LABEL_PADDING * 2;

        public static float TEXT_FIELD_BOLD_SIZE = 20.0F;
        public static float TEXT_FIELD_HEADING_SIZE = 19.0F;
        public static float TEXT_FIELD_SUB_HEADING_SIZE = 16.0F;
        public static float TEXT_FIELD_LIST_ITEM_SIZE = 14.0F;

        public static float ENTRY_HEIGHT = Constants.IsPhone() ? 40 : 50;


        public static float TOTAL_RIGHT_SIDE_PADDING = 25.0F;
        public static float TOTAL_RIGHT_SIDE_DEEP_PADDING = 75.0F;

        public static float BALANCES_SIDE_PADDING = 10;
        public static float BALANCES_ITEM_SIDE_PADDING = 20;
        public static float TERMS_SIDE_PADDING = Constants.IsPhone() ? 10 : 20;

        public static float CORNER_RADIUS = 6.0f;

        public static float NAV_BUTTON_SIZE_IPAD = 80;
        public static float NAV_BUTTON_WIDTH_IPAD = 100;
        public static float NAV_BUTTON_SIZE_IPHONE = 60;
        public static float NAV_BUTTON_SIZE_IPHONEX = 75;
        public static float NAV_BUTTON_SIZE_IPHONE_WIDTH = 60;
        public static float NAV_BUTTON_PADDING_IPAD = 50;
        public static float NAV_BUTTON_PADDING_IPHONE = 20;
        public static float NAV_HEIGHT = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;

        public static float LANDING_BUTTON_HEIGHT_IPHONE = 60;
        public static float LANDING_BUTTON_HEIGHT_IPAD = 90;
        public static float LANDING_BUTTON_HEIGHT = Constants.IsPhone() ? Constants.LANDING_BUTTON_HEIGHT_IPHONE : Constants.LANDING_BUTTON_HEIGHT_IPAD;
        public static float LANDING_BUTTON_LOGO_AREA_WIDTH = Constants.IsPhone() ? 60 : 100;
        public static float LANDING_BUTTON_LABEL_X = Constants.IsPhone() ? 85 : 150;

        public static string ICON_PATH = Constants.IsPhone() ? "Icons/Phone/" : "Icons/";
        public static string MISC_PATH = Constants.IsPhone() ? "Misc/Phone/" : "Misc/";
        public static string MISC_PATH_IPad = "Misc/";

        public static float IOS_7_TOP_PADDING = 85.0f;
        public static float IOS_6_TOP_PADDING = 25.0f;
        public static float TOP_PADDING = IS_OS_7_OR_LATER() ? IOS_7_TOP_PADDING : IOS_6_TOP_PADDING;

        public static float TOGGLE_ANIMATION_DURATION = 0.5f;

        public static float CLAIMS_DETAILS_ITEM_V_PADDING = 15;
        public static float CLAIMS_DETAILS_ITEM_LEFT_PADDING = 10;
        public static float CLAIMS_DETAILS_SUB_ITEM_PADDING = 10;
        public static float CLAIMS_DETAILS_FIELD_HEIGHT = 40;
        public static float CLAIMS_DETAILS_COMPONENT_PADDING = 10;

        public static string MIN_DATE = "1980-01-01";

        public static float DEFAULT_BUTTON_WIDTH = Constants.IsPhone() ? 270 : 400;
        public static float DEFAULT_BUTTON_HEIGHT = Constants.IsPhone() ? 40 : 50;

        //single selection table view
        public static float SELECTION_MENU_ITEM_FONT_SIZE = Constants.IsPhone() ? 12.0f : 14f;

        public static float SINGLE_SELECTION_TOP_TABLE_PADDING = 30.0f;

        public static float SINGLE_SELECTION_CELL_HEIGHT = 55.0f;

        public static float SINGLE_SELECTION_VERTICAL_CELL_PADDING = 10.0f;
        public static float SINGLE_SELECTION_LABEL_FONT_SIZE = Constants.SMALL_FONT_SIZE;

        //single selection accented title three subtitle table view
        public static float SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE = Constants.SMALL_FONT_SIZE - 2;
        public static float SINGLE_SELECTION_ACCENTED_TITLE_TITLE_PADDING_LEFT = 25.0f;
        public static float TREATMENT_DETAILS_LIST_ITEM_PADDING = 15.0f;
        public static float SINGLE_SELECTION_ACCENTED_TITLE_SUBTITLE_PADDING_LEFT = 40.0f;
        public static float SINGLE_SELECTION_ACCENTED_TITLE_INNER_MARGIN = 5.0f;
        public static float SINGLE_SELECTION_ACCENTED_TITLE_CELL_HEIGHT = SINGLE_SELECTION_ACCENTED_TITLE_INNER_MARGIN * 7 + SINGLE_SELECTION_LABEL_FONT_SIZE + SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE * 3 + 10;
        public static float SINGLE_SELECTION_NO_TREATMENT_TITLE_CELL_HEIGHT = SINGLE_SELECTION_ACCENTED_TITLE_INNER_MARGIN * 6 + SINGLE_SELECTION_LABEL_FONT_SIZE + SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE * 2 + 10;
        public static float SINGLE_SELECTION_ACCENTED_TITLE_CELL_CONTENTS_HEIGHT = SINGLE_SELECTION_ACCENTED_TITLE_INNER_MARGIN * 3 + SINGLE_SELECTION_LABEL_FONT_SIZE + SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE * 3;

        public static float SINGLE_SELECTION_LOCATED_PROVIDERS_INNER_MARGIN = 2.0f;
        public static float SINGLE_SELECTION_LOCATED_PROVIDERS_CELL_HEIGHT = SINGLE_SELECTION_ACCENTED_TITLE_INNER_MARGIN * 7 + SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE * 3 + 10;
        public static float SINGLE_SELECTION_LOCATED_PROVIDERS_CELL_CONTENTS_HEIGHT = SINGLE_SELECTION_LOCATED_PROVIDERS_INNER_MARGIN * 2 + SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE * 3 + 15;

        public static float CLAIM_RESULTS_INITIAL_HEIGHT = Constants.IS_OS_7_OR_LATER() ? 435 : 450;
        public static float CLAIM_RESULTS_DETAILS_HEIGHT = Constants.IS_OS_7_OR_LATER() ? 261 : 276;
        public static float CLAIM_RESULTS_LIMITATIONS_HEIGHT = Constants.IS_OS_7_OR_LATER() ? 153 : 169;
        public static float ELIGIBILITY_RESULTS_LIMITATIONS_HEIGHT = Constants.IS_OS_7_OR_LATER() ? 193 : 209;
        public static float CLAIM_CONFIRMATION_DETAILS_LT_HEIGHT = Constants.IS_OS_7_OR_LATER() ? 125 : 145;
        public static float CLAIM_CONFIRMATION_DETAILS_HEIGHT = Constants.IS_OS_7_OR_LATER() ? 103 : 119;
        public static float CLAIM_CONFIRMATION_DETAILS_NT_HEIGHT = Constants.IS_OS_7_OR_LATER() ? 81 : 93;
        public static float CLAIM_CONFIRMATION_DETAILS_TOP_PADDING = 15;
        public static float CLAIM_CONFIRMATION_LABEL_HEIGHT = Constants.IS_OS_7_OR_LATER() ? 22 : 26;
        public static float CLAIMS_RESULTS_IPAD_HEIGHT = 50;
        public static float CLAIMS_RESULTS_IPAD_PADDING = 10;

        public static string ClaimHistoryCountTableViewCell = "ClaimHistoryCountTableViewCell";
        public static float ClaimHistoryCountTableViewCellHeight = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) ? ((System.Globalization.CultureInfo.CurrentUICulture.Name.ToString().Contains("fr") || System.Globalization.CultureInfo.CurrentUICulture.Name.ToString().Contains("Fr")) ? 120f : 80f) : ((System.Globalization.CultureInfo.CurrentUICulture.Name.ToString().Contains("fr") || System.Globalization.CultureInfo.CurrentUICulture.Name.ToString().Contains("Fr")) ? 180f : 80f);
        public static string IPadNumericKeyBoardDot = (System.Globalization.CultureInfo.CurrentUICulture.Name.ToString().Contains("fr") || System.Globalization.CultureInfo.CurrentUICulture.Name.ToString().Contains("Fr")) ? "," : ".";
        public static float RESULTSDETAILSTABLEVIEWCELLHEIGHT = (Constants.IsPhone()) ? 255f : 230f;
        public static int EOBMESSAGELIMATELENGTH_PORTRAIT = Constants.IsPhone() ? 22 : 60;
        public static int EOBMESSAGELIMATELENGTH_LANDSCAPE = Constants.IsPhone() ? 42 : 80;
        public static float EOBMESSAGE_LINEHEIGHT = 22f;
        public static int CLAIMHISTORY_LINEOFBUNESSLIMATE = Constants.IsPhone() ? 24 : 24;
        public static int HCSAEXPENSETYPE_LIMATELENGTH_PORTRAIT = Constants.IsPhone() ? 22 : 60;
        public static int HCSAEXPENSETYPE_LIMATELENGTH_LANDSCAPE = Constants.IsPhone() ? 42 : 80;
        public static int CLAIMHISTORY_PARTICIPANTLIMATE = Constants.IsPhone() ? 23 : 23;
        public static int CLAIMHISTORY_PAYMENT_IDNUMBERFIELD_LIMATELENGTHLANDSCAPE = Constants.IsPhone() ? 22 : 31;
        public static int CLAIMHISTORY_PAYMENT_IDNUMBERFIELD_LIMATELENGTHPORTRAIT = Constants.IsPhone() ? 18 : 26;
        public static float HCSA_CellBorder = 5f;
        public static nfloat iPhoneXHeight = 2436;
        public static nfloat iPhoneXWidth = 1125;

        public static nfloat DocumentListCellHeight = 55;
        public static nfloat TopCardBorderWidth = 0.5f;
        public static nfloat CellBorderWith = 2.0f;
        public static nfloat ActiveClaimCellHeight = 108.0f;
        public static nfloat ButtonBorderWidth = 1.0f;

        private static nfloat RecentClaimsCollectionViewCellSpacingOniPhone = 8.0f;
        private static nfloat RecentClaimsCollectionViewCellSpacingOniPad = 16.0f;

        public static nfloat RecentClaimsWhenActionRequiredCellHeight = 80.0f;
        public static nfloat RecentClaimsCellHeightOniPhone = 60.0f;
        public static nfloat RecentClaimsCellHeightOniPad = 90.0f;
        public static nfloat AuditClaimsCellHeight = 125.0f;

        public static float ClaimDetailsSectionHeaderFontSize = Constants.IsPhone() ? 18.0f : 24.0f;
        public static float ClaimDetaulsHealthProviderDetailsFontSize = Constants.IsPhone() ? 12.0f : 16.0f;
        public static float ClaimDetailsQuestionFontSize = Constants.IsPhone() ? 14.0f : 16.0f;
        public static float ClaimDetailsSubquestionFotSize = Constants.IsPhone() ? 12.0f : 14.0f;

        public static float ClaimSummarySectionHeaderFontSize = Constants.IsPhone() ? 18.0f : 24.0f;
        public static float ClaimSummaryParticipantLabelFontSize = Constants.IsPhone() ? 24.0f : 30.0f;
        public static float ClaimSummaryClaimCounterFontSize = Constants.IsPhone() ? 10.0f : 18.0f;
        public static float ClaimSummaryFooterFontSize = Constants.IsPhone() ? 12.0f : 18.0f;
        public static float ClaimSummaryInfoLabelFontSize = Constants.IsPhone() ? 12.0f : 18.0f;
        public static float ClaimSummaryInfoValueFontSize = Constants.IsPhone() ? 12.0f : 16.0f;

        public static float ClaimDocumentsUploadSectionHeaderFontSize = Constants.IsPhone() ? 18.0f : 24.0f;
        public static float ClaimDocumentsUploadFootNoteFontSize = Constants.IsPhone() ? 12.0f : 16.0f;
        public static float ClaimDocumentsUploadAnotherDocumentButtonFontSize = 14.0f;

        public static float ClaimSubmitConfirmationAdditionalInfoFontSize = Constants.IsPhone() ? 12.0f : 14.0f;
        public static float ClaimSubmitConfirmationFooterNoteFontSize = Constants.IsPhone() ? 10.0f : 13.0f;
        public static float ClaimSubmitTreatmentDetailsCollectionViewLineSpacing = Constants.IsPhone() ? 10.0f : 18.0f;
        public static float ClaimSubmitTreatmentDetailsSectionTopInset = Constants.IsPhone() ? 16.0f : 26.0f;
        public static float ClaimSubmitTreatmentDetailsSectionBottomInset = 16.0f;

        public static float DisclaimerTitleFontSize = 23.0f;
        public static float DisclaimerContentFontSize = Constants.IsPhone() ? 14.0f : 18.0f;
        public static float DisclaimerContentLineSpacing = Constants.IsPhone() ? 1.36f : 1.39f;

        public static float AuditListInstructionFontSize = Constants.IsPhone() ? 14.0f : 18.0f;
        public static float AuditListNotesFontSize = Constants.IsPhone() ? 12.0f : 16.0f;
        public static float AuditListInstructionLineSpacing = Constants.IsPhone() ? 1.36f : 1.39f;
        public static float AuditListNotesLineSpacing = Constants.IsPhone() ? 1.33f : 1.38f;
        public static float AuditListSectionHeaderFontSize = Constants.IsPhone() ? 20.0f : 30.0f;

        public static float AuditClaimCellActionRequiredFontSize = Constants.IsPhone() ? 12.0f : 14.0f;
        public static float AuditClaimCellGeneralFontSize = 12.0f;
        public static float AuditClaimCellDescriptionFontSize = Constants.IsPhone() ? 16.0f : 18.0f;

        public static float UploadCompletedNoteFontSize = Constants.IsPhone() ? 12.0f : 16.0f;
        public static float UploadCompletedNoteLineSpacing = Constants.IsPhone() ? 1.33f : 1.38f;

        public static float TCCheckBoxFontSize = Constants.IsPhone() ? 12.0f : 18.0f;
        public static string TCLogoHeight = Constants.IsPhone() ? TCLogoHeightOniPhone : TCLogoHeightOniPad;
        public static string TCLogoHeightOniPhone = "50px";
        public static string TCLogoHeightOniPad = "100px";

        public static nfloat RecentClaimsCollectionViewCellSpacing = IsPhone() ?
            RecentClaimsCollectionViewCellSpacingOniPhone : RecentClaimsCollectionViewCellSpacingOniPad;

        public static nfloat OtherParticipantsNotesLineSpacing = 1.24f;
        public static nfloat OtherParticipantsNotesFontSize = IsPhone() ? 13.0f : 17.0f;

        public static nfloat EligibilityResultVisionEnhancementFontSize = Constants.IsPhone() ? 12f : 14f;
        public static nfloat EligibilityResultVisionEnhancementFontLineSpace = Constants.IsPhone() ? 1.33f : 1.38f;

        public static bool IsPhone()
        {
            return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;
        }

        public static bool IsiPhoneXDevice()
        {
            bool iPhoneXDevice = false;

            if (UIScreen.MainScreen.NativeBounds.Height == iPhoneXHeight && UIScreen.MainScreen.NativeBounds.Width == iPhoneXWidth)
            {
                iPhoneXDevice = true;
            }

            return iPhoneXDevice;
        }

        public static float Bottom;
        public static void SetSafe(float bottom)
        {
            Bottom = bottom;
        }

        public static bool IS_OS_7_OR_LATER()
        {
            return UIDevice.CurrentDevice.CheckSystemVersion(7, 0);
        }

        public static bool IS_OS_VERSION_OR_LATER(int max, int min)
        {
            return UIDevice.CurrentDevice.CheckSystemVersion(max, min);
        }

        //returns localization values
        public static string tr(this string translate)
        {
            return NSBundle.MainBundle.GetLocalizedString(translate, "", "");
        }

        public static string FormatWithBrandKeywords(this string translate, params string[] brandResource)
        {
            return string.Format(NSBundle.MainBundle.GetLocalizedString(translate, "", ""), brandResource);
        }
    }
}

