namespace MobileClaims.Core.Entities
{
    public static class UploadDocumentProcessType
    {
        public const string COP = "COP";

        public const string Audit = "Audit";

        public const string RecentClaims = "recentClaims";

        public const string NewClaimSubmission = "claimUpload";
    }

    public class UploadDocumentsFormData
    {
        public long ClaimFormId { get; set; }
        public string ParticipantNumber { get; set; }
    }
}