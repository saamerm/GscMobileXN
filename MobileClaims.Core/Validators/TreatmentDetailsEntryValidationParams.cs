namespace MobileClaims.Core.Validators
{
    public static class TreatmentDetailsEntryValidationParams
    {
        // Due to android edittext behaviour, these limits are also set in android layouts
        public const int PROCEDURE_CODE_LENGETH = 5;
        public const int TOOTH_CODE_LENGETH = 2;
        public const int TOOTH_SURFACE_LENGETH = 5;
        public const double DENTIST_FEE_MAX = 9999.99;
        public const int TREATMENT_DATE_ALLOWED_WITHHIN_MONTHS = -24;
        public static int REQUIRED_TOOTH_SURFACE_COUNT = 5;

        internal const string PROCEDURE_CODE_REGEX = @"^[0-9]{5}$";
        internal const string TOOTH_CODE_REGEX = @"^(([1-8])|([0-4][1-8])|([5-9][1-5])|([9][6-9]))?$";
        internal const string TOOTH_SURFACE_REGEX = @"^([MOIDVBFL]|[moidvbfl]){0,5}$";
    }
}
