using MobileClaims.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileClaims.Core.Services
{
    public interface IEligibilityService
    {
        EligibilityCheck EligibilityCheck { get; set; }
        EligibilityCheckType SelectedEligibilityCheckType { get; set; }
        List<EligibilityCheckType> EligibilityCheckTypes { get; }
        List<EligibilityProvince> EligibilityProvinces { get; }
        List<EligibilityBenefit> EligibilityBenefits { get; }
        List<EligibilityOption> EligibilityOptions { get; }
        EligibilityCheck EligibilityCheckResults { get; }
        EligibilityInquiry EligibilityInquiryResults { get; }
        Participant EligibilitySelectedParticipant { get; set; }
        List<ParticipantEligibilityResult> SelectedParticipantsForBenefitInquiry { get; set; }

        Task GetEligibilityCheckTypes(string planMemberID);
        Task GetEligibilityProvinces(string eligibilityCheckTypeID);
        Task GetEligibilityBenefits(string eligibilityCheckTypeID);
        Task GetEligibilityOptions(string optionType);
        Task CheckEligibility();
        Task EligibilityInquiry(EligibilityInquiry ei);
    }
}
