using MobileClaims.Core.Entities;
using MobileClaims.Core.Services.FacadeEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileClaims.Core.Services
{
    public interface IClaimService
    {
        List<ClaimSubmissionType> ClaimSubmissionTypes { get; }
        ClaimSubmissionType SelectedClaimSubmissionType { get; set; }
        List<ClaimSubmissionBenefit> ClaimSubmissionBenefits { get; }
        List<ClaimOption> ClaimOptions { get; }
        List<ClaimOption> TypesOfMedicalProfessional { get; }
        List<LensType> LensTypes { get; }
        List<ClaimOption> VisionSpheres { get; }
        List<ClaimOption> VisionCylinders { get; }
        List<ClaimOption> VisionAxes { get; }
        List<ClaimOption> VisionPrisms { get; }
        List<ClaimOption> VisionBifocals { get; }
        Claim Claim { get; set; }
        ClaimGSC ClaimResults { get; }
        Guid SelectedTreatmentDetailID { get; set; }
        List<ClaimAudit> ClaimAudits { get; set; }//added set by vivian for landing page show selected
        ClaimResultResponse ClaimResult { get; }
        string HCSAName { get; }
        List<TextAlteration> ClaimDisclaimerTextAlterations { get; }
        List<TextAlteration> ClaimAgreementTextAlterations { get; }
        List<TextAlteration> PhoneTextAlterations { get; }
        TextAlteration PhoneText { get; }
        Task GetPhoneNumber(string planMemberID);
        Task GetClaimAgreement(string planMemberID);

        Task<TextAlteration_1Response> GetClaimAgreementTextAsync(string planMemberId);

        Task<TextAlterationDateResponse> GetClaimAgreementDateAsync(string planMemberId);

        Task GetClaimDisclaimer(string planMemberID);
		bool NoPhoneNumber { get; set; }
		bool PhoneError { get; set; }
		bool ClaimError { get; set; }
		bool ClaimDisclaimerError { get; set; }

        bool IsTreatmentDetailsListInNavStack { get; set; }
        bool IsHCSAClaim { get; set; }
        Task GetClaimSubmissionTypes(string planMemberID);
        Task GetClaimSubmissionBenefits(string claimSubmissionTypeID);
        Task GetClaimOptions(string optionType);
        Task GetTypesOfMedicalProfessional();
        Task GetLensTypes(string planMemberID);
        Task GetVisionSpheres();
        Task GetVisionCylinders();
        Task GetVisionAxes();
        Task GetVisionPrisms();
        Task GetVisionBifocals();
        Task SubmitClaimAsync(string comments, string uploadDocumentProcessTypem, IEnumerable<DocumentInfo> attachments);
        Task SubmitClaim();
        Task<bool> UploadDocumentsAsync(string comment, string claimId, string uploadDocumentProcessType, IEnumerable<DocumentInfo> attachments, string participantNumber = null);
        Task<IEnumerable<ClaimCOP>> GetCOPClaimsAsync();
        Task<IEnumerable<ClaimSummary>> GetCOPClaimSummaryAsync(string claimFormId);
        Task GetClaimAudits();
        Task<IList<ClaimAudit>> GetAuditClaimsAsync();
        Task<ClaimResultResponse> GetClaimResultByID(string claimFormId);
        void PersistClaim();
        void ClearClaimDetails();
        void ClearOldClaimData();
    }
}
