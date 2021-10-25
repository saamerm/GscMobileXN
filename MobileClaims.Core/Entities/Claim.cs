using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MobileClaims.Core.Entities
{
    public class Claim
    {
        public int ID { get; set; }
        public ClaimSubmissionType Type { get; set; }
        public ServiceProvider Provider { get; set; }
        public Participant Participant { get; set; }

        public bool CoverageUnderAnotherBenefitsPlan { get; set; }
        public bool IsOtherCoverageWithGSC { get; set; }

        private bool _hasClaimBeenSubmittedToOtherBenefitPlan;
        public bool HasClaimBeenSubmittedToOtherBenefitPlan
        {
            get => _hasClaimBeenSubmittedToOtherBenefitPlan;
            set
            {
                _hasClaimBeenSubmittedToOtherBenefitPlan = value;
                UpdateTreatmentDetails();
            }
        }

        public bool PayAnyUnpaidBalanceThroughOtherGSCPlan { get; set; }
        public string UnmodifiedOtherGSCNumber { get; set; }
        public string OtherGSCNumber { get; set; }
        public string OtherGSCParticipantNumber { get; set; }
        public bool PayUnderHCSA { get; set; }
        public bool IsClaimDueToAccident { get; set; }
        public bool IsTreatmentDueToAMotorVehicleAccident { get; set; }
        public DateTime DateOfMotorVehicleAccident { get; set; }
        public bool IsTreatmentDueToAWorkRelatedInjury { get; set; }
        public DateTime DateOfWorkRelatedInjury { get; set; }
        public int WorkRelatedInjuryCaseNumber { get; set; }
        public bool IsMedicalItemForSportsOnly { get; set; }
        public bool HasReferralBeenPreviouslySubmitted { get; set; }
        public DateTime DateOfReferral { get; set; }
        public ClaimOption TypeOfMedicalProfessional { get; set; }
        public bool IsGSTHSTIncluded { get; set; }
        public bool IsOtherTypeOfAccident { get; set; }

        public bool IsOtherTypeOfAccidentVisible
        {
            get
            {
                if (Type != null)
                {
                    return string.Equals(Type.ID, "DENTAL", StringComparison.OrdinalIgnoreCase);
                }

                return false;
            }
        }

        public int MaximumAllowedTreatmentDetails
        {
            get
            {
                if (Type != null)
                {
                    return string.Equals(Type.ID, "DENTAL", StringComparison.OrdinalIgnoreCase) ? 10 : 5;
                }

                return 5;
            }
        }

        public ObservableCollection<TreatmentDetail> TreatmentDetails { get; set; }

        public bool IsIsOtherCoverageWithGSCVisible => CoverageUnderAnotherBenefitsPlan; //Q2

        public bool IsHasClaimBeenSubmittedToOtherBenefitPlanVisible => CoverageUnderAnotherBenefitsPlan; //Q3

        public bool IsPayAnyUnpaidBalanceThroughOtherGSCPlanVisible => CoverageUnderAnotherBenefitsPlan && IsOtherCoverageWithGSC && !HasClaimBeenSubmittedToOtherBenefitPlan; //Q4

        public bool IsOtherGSCNumberVisible => IsPayAnyUnpaidBalanceThroughOtherGSCPlanVisible && PayAnyUnpaidBalanceThroughOtherGSCPlan; //Q5

        private bool _isPayUnderHCSAVisible;
        public bool IsPayUnderHCSAVisible
        {
            get => _isPayUnderHCSAVisible;
            set => _isPayUnderHCSAVisible = value;
        }

        public bool IsDateOfMotorVehicleAccidentVisible => IsTreatmentDueToAMotorVehicleAccident;

        public bool IsDateOfWorkRelatedInjuryVisible => IsTreatmentDueToAWorkRelatedInjury;

        public bool IsWorkRelatedInjuryCaseNumberVisible => IsTreatmentDueToAWorkRelatedInjury;

        public bool IsIsMedicalItemForSportsOnlyVisible
        {
            get
            {
                if (Type != null)
                    return Type.ID == "MI";
                else
                    return false;
            }
        }

        public bool IsHasReferralBeenPreviouslySubmittedVisible //Q13
        {
            get
            {
                if (Type == null) return false;
                switch (Type.ID)
                {
                    case "DRUG":
                    case "DENTAL":
                        return false;
                    default:
                        return (Type.ID != "MI" && Type.ID != "ORTHODONTIC");
                }
            }
        }
        //Used on confirmation page only
        public bool IsDateOfReferralVisible
        {
            get
            {
                if (Type != null && Type.ID != "MI" && Type.ID != "ORTHODONTIC" && HasReferralBeenPreviouslySubmitted && DateOfReferral != DateTime.MinValue)
                    return true;
                else
                    return false;
            }
        }

        public bool IsTypeOfMedicalProfessionalVisible
        {
            get
            {
                if (Type != null && Type.ID != "MI" && Type.ID != "ORTHODONTIC" && HasReferralBeenPreviouslySubmitted && TypeOfMedicalProfessional != null)
                    return true;
                else
                    return false;
            }
        }

        public bool IsIsGSTHSTIncludedVisible
        {
            get
            {
                if (Type != null && (Type.ID == "ORTHODONTIC" || Type.ID == "MASSAGE"))
                    return true;
                else
                    return false;
            }
        }

        public bool IsAlternateCarrierPaymentVisible => this.HasClaimBeenSubmittedToOtherBenefitPlan;

        public bool IsTypeOfTreatmentVisible
        {
            get
            {
                if (Type != null)
                {
                    switch (Type.ID)
                    {
                        case "ACUPUNCTURE":
                        case "CHIROPODY":
                        case "CHIRO":
                        case "PHYSIO":
                        case "PODIATRY":
                        case "PSYCHOLOGY":
                        case "MASSAGE":
                        case "NATUROPATHY":
                        case "SPEECH":
                            return true;
                        default:
                            return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsTreatementDateVisible
        {
            get
            {
                if (Type != null)
                {
                    switch (Type.ID)
                    {
                        case "ACUPUNCTURE":
                        case "CHIROPODY":
                        case "CHIRO":
                        case "PHYSIO":
                        case "PODIATRY":
                        case "PSYCHOLOGY":
                        case "MASSAGE":
                        case "NATUROPATHY":
                        case "SPEECH":
                            return true;
                        default:
                            return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsTreatmentAmountVisible
        {
            get
            {
                if (Type != null)
                {
                    switch (Type.ID)
                    {
                        case "ACUPUNCTURE":
                        case "CHIROPODY":
                        case "CHIRO":
                        case "PHYSIO":
                        case "PODIATRY":
                        case "PSYCHOLOGY":
                        case "MASSAGE":
                        case "NATUROPATHY":
                        case "SPEECH":
                        case "CONTACTS":
                        case "EYEEXAM":
                            return true;
                        default:
                            return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsTotalAmountChargedForMIVisible
        {
            get
            {
                if (Type != null && Type.ID == "MI")
                    return true;
                else
                    return false;
            }
        }

        public bool IsTreatmentDurationVisible
        {
            get
            {
                if (Type != null)
                {
                    switch (Type.ID)
                    {
                        case "PSYCHOLOGY":
                        case "MASSAGE":
                        case "NATUROPATHY":
                        case "SPEECH":
                            return true;
                        default:
                            return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsItemDescriptionVisible
        {
            get
            {
                if (Type != null && Type.ID == "MI")
                    return true;
                else
                    return false;
            }
        }

        public bool IsPickupDateVisible
        {
            get
            {
                if (Type != null && Type.ID == "MI")
                    return true;
                else
                    return false;
            }
        }

        public bool IsQuantityVisible
        {
            get
            {
                if (Type != null && Type.ID == "MI")
                    return true;
                else
                    return false;
            }
        }

        public bool IsGSTHSTIncludedInTotalVisible
        {
            get
            {
                if (Type != null && Type.ID == "MI")
                    return true;
                else
                    return false;
            }
        }

        public bool IsPSTIncludedInTotalVisible
        {
            get
            {
                if (Type != null && Type.ID == "MI")
                    return true;
                else
                    return false;
            }
        }

        public bool IsHasReferralBeenPreviouslySubmittedVisibleForTreatment
        {
            get
            {
                if (Type != null && Type.ID == "MI")
                    return true;
                else
                    return false;
            }
        }
        //Used on confirmation page only
        public bool IsDateOfReferralVisibleForTreatment
        {
            get
            {
                var qry = from TreatmentDetail td in TreatmentDetails
                          where td.DateOfReferral != DateTime.MinValue
                          select td;
                if (Type != null && Type.ID == "MI" && !HasReferralBeenPreviouslySubmitted && qry.Any())
                    return true;
                else
                    return false;
            }
        }

        public bool IsTypeOfMedicalProfessionalVisibleForTreatment
        {
            get
            {
                var qry = from TreatmentDetail td in TreatmentDetails
                          where td.TypeOfMedicalProfessional != null
                          select td;
                if (Type != null && Type.ID == "MI" && !HasReferralBeenPreviouslySubmitted && qry.Any())
                    return true;
                else
                    return false;
            }
        }

        public bool IsDateOfMonthlyTreatmentVisible
        {
            get
            {
                if (Type != null && Type.ID == "ORTHODONTIC")
                    return true;
                else
                    return false;
            }
        }

        public bool IsOrthodonticMonthlyFeeVisible
        {
            get
            {
                if (Type != null && Type.ID == "ORTHODONTIC")
                    return true;
                else
                    return false;
            }
        }

        public bool IsDateOfPurchaseVisible
        {
            get
            {
                if (Type != null && (Type.ID == "CONTACTS" || Type.ID == "GLASSES"))
                    return true;
                else
                    return false;
            }
        }

        public bool IsTypeOfEyewearVisible
        {
            get
            {
                if (Type != null && (Type.ID == "CONTACTS" || Type.ID == "GLASSES"))
                    return true;
                else
                    return false;
            }
        }

        public bool IsTypeOfLensVisible
        {
            get
            {
                if (Type != null && Type.ID == "GLASSES")
                    return true;
                else
                    return false;
            }
        }

        public bool IsDateOfExaminationVisible
        {
            get
            {
                if (Type != null && Type.ID == "EYEEXAM")
                    return true;
                else
                    return false;
            }
        }

        public bool IsPrescriptionDetailsVisible
        {
            get
            {
                if (Type != null && (Type.ID == "GLASSES" || Type.ID == "CONTACTS"))
                {
                    var qry = from TreatmentDetail td in TreatmentDetails
                              where td.IsPrescriptionDetailsVisible
                              select td;
                    if (qry.Any())
                        return true;
                }

                return false;
            }
        }

        public bool IsAcknowledgeEyewearIsCorrectiveVisible
        {
            get
            {
                if (Type != null && (Type.ID == "GLASSES" || Type.ID == "CONTACTS"))
                {
                    var qry = from TreatmentDetail td in TreatmentDetails
                              where td.IsAcknowledgeEyewearIsCorrectiveVisible
                              select td;
                    if (qry.Any())
                        return true;
                }

                return false;
            }
        }

        public bool IsTotalAmountChargedVisible
        {
            get
            {
                if (Type != null && Type.ID == "GLASSES")
                {
                    var qry = from TreatmentDetail td in TreatmentDetails
                              where td.IsTotalAmountChargedVisible
                              select td;
                    if (qry.Any())
                        return true;
                }

                return false;
            }
        }

        public bool IsFrameAmountVisible
        {
            get
            {
                if (Type != null && Type.ID == "GLASSES")
                {
                    var qry = from TreatmentDetail td in TreatmentDetails
                              where td.IsFrameAmountVisible
                              select td;
                    if (qry.Any())
                        return true;
                }

                return false;
            }
        }

        public bool IsEyeglassLensesAmountVisible
        {
            get
            {
                if (Type != null && Type.ID == "GLASSES")
                {
                    var qry = from TreatmentDetail td in TreatmentDetails
                              where td.IsEyeglassLensesAmountVisible
                              select td;
                    if (qry.Any())
                        return true;
                }

                return false;
            }
        }

        public bool IsFeeAmountVisible
        {
            get
            {
                if (Type != null && Type.ID == "GLASSES")
                {
                    var qry = from TreatmentDetail td in TreatmentDetails
                              where td.IsFeeAmountVisible
                              select td;
                    if (qry.Any())
                        return true;
                }

                return false;
            }
        }

        public bool IsRightSphereVisible
        {
            get
            {
                if (Type != null && (Type.ID == "GLASSES" || Type.ID == "CONTACTS"))
                {
                    var qry = from TreatmentDetail td in TreatmentDetails
                              where td.IsRightSphereEnabled
                              select td;
                    if (qry.Any())
                        return true;
                }

                return false;
            }
        }

        public bool IsRightCylinderVisible
        {
            get
            {
                if (Type != null && (Type.ID == "GLASSES" || Type.ID == "CONTACTS"))
                {
                    var qry = from TreatmentDetail td in TreatmentDetails
                              where td.IsRightCylinderEnabled
                              select td;
                    if (qry.Any())
                        return true;
                }

                return false;
            }
        }

        public bool IsRightAxisVisible
        {
            get
            {
                if (Type != null && (Type.ID == "GLASSES" || Type.ID == "CONTACTS"))
                {
                    var qry = from TreatmentDetail td in TreatmentDetails
                              where td.IsRightAxisEnabled
                              select td;
                    if (qry.Any())
                        return true;
                }

                return false;
            }
        }

        public bool IsRightPrismVisible
        {
            get
            {
                if (Type != null && (Type.ID == "GLASSES" || Type.ID == "CONTACTS"))
                {
                    var qry = from TreatmentDetail td in TreatmentDetails
                              where td.IsRightPrismEnabled
                              select td;
                    if (qry.Any())
                        return true;
                }

                return false;
            }
        }

        public bool IsRightBifocalVisible
        {
            get
            {
                if (Type != null && (Type.ID == "GLASSES" || Type.ID == "CONTACTS"))
                {
                    var qry = from TreatmentDetail td in TreatmentDetails
                              where td.IsRightBifocalEnabled
                              select td;
                    if (qry.Any())
                        return true;
                }

                return false;
            }
        }

        public bool IsLeftSphereVisible
        {
            get
            {
                if (Type != null && (Type.ID == "GLASSES" || Type.ID == "CONTACTS"))
                {
                    var qry = from TreatmentDetail td in TreatmentDetails
                              where td.IsLeftSphereEnabled
                              select td;
                    if (qry.Any())
                        return true;
                }

                return false;
            }
        }

        public bool IsLeftCylinderVisible
        {
            get
            {
                if (Type != null && (Type.ID == "GLASSES" || Type.ID == "CONTACTS"))
                {
                    var qry = from TreatmentDetail td in TreatmentDetails
                              where td.IsLeftCylinderEnabled
                              select td;
                    if (qry.Any())
                        return true;
                }

                return false;
            }
        }

        public bool IsLeftAxisVisible
        {
            get
            {
                if (Type != null && (Type.ID == "GLASSES" || Type.ID == "CONTACTS"))
                {
                    var qry = from TreatmentDetail td in TreatmentDetails
                              where td.IsLeftAxisEnabled
                              select td;
                    if (qry.Any())
                        return true;
                }

                return false;
            }
        }

        public bool IsLeftPrismVisible
        {
            get
            {
                if (Type != null && (Type.ID == "GLASSES" || Type.ID == "CONTACTS"))
                {
                    var qry = from TreatmentDetail td in TreatmentDetails
                              where td.IsLeftPrismEnabled
                              select td;
                    if (qry.Any())
                        return true;
                }

                return false;
            }
        }

        public bool IsLeftBifocalVisible
        {
            get
            {
                if (Type != null && (Type.ID == "GLASSES" || Type.ID == "CONTACTS"))
                {
                    var qry = from TreatmentDetail td in TreatmentDetails
                              where td.IsLeftBifocalEnabled
                              select td;
                    if (qry.Any())
                        return true;
                }

                return false;
            }
        }

        public bool IsAdditionalInformationVisible
        {
            get
            {
                if (Type != null && Type.ID == "DRUG")
                    return true;
                else
                    return false;
            }
        }

        public bool IsAttachmentsVisible
        {
            get
            {
                if (Type != null && Type.ID == "DRUG")
                    return true;
                else
                    return false;
            }
        }
        public bool IsHealthProviderVisible
        {
            get
            {
                if (Type != null && Type.ID != "DRUG")
                    return true;
                else
                    return false;
            }
        }
        public bool IsTreatmentDetailsVisible
        {
            get
            {
                if (Type != null && Type.ID != "DRUG")
                    return true;
                else
                    return false;
            }
        }

        public List<ClaimSubmissionType> ClaimSubmissionTypes { get; set; }
        public List<ServiceProvider> PreviousServiceProviders { get; set; }
        public string ServiceProviderSearchInitial { get; set; }
        public string ServiceProviderSearchLastName { get; set; }
        public string ServiceProviderSearchPhoneNumber { get; set; }
        public List<ServiceProvider> ServiceProviderSearchResults { get; set; }
        public string ServiceProviderEntryName { get; set; }
        public string ServiceProviderEntryAddress1 { get; set; }
        public string ServiceProviderEntryAddress2 { get; set; }
        public string ServiceProviderEntryCity { get; set; }
        public ServiceProviderProvince ServiceProviderEntryProvince { get; set; }
        public string ServiceProviderEntryPostalCode { get; set; }
        public string ServiceProviderEntryPhone { get; set; }
        public string ServiceProviderEntryRegistrationNumber { get; set; }
        public List<ClaimOption> TypesOfMedicalProfessional { get; set; }
        public bool TermsAndConditionsAccepted { get; set; }

        public Claim()
        {
            TreatmentDetails = new ObservableCollection<TreatmentDetail>();
            OtherGSCNumber = "";

            DateOfMotorVehicleAccident = DateTime.Now;
            DateOfWorkRelatedInjury = DateTime.Now;
            DateOfReferral = DateTime.MinValue;
        }

        public TreatmentDetail GetTreatmentDetailByID(Guid id)
        {
            foreach (TreatmentDetail td in TreatmentDetails)
            {
                if (td.ID == id)
                {
                    return td;
                }
            }
            return null; //not found
        }

        private void UpdateTreatmentDetails()
        {
            foreach (TreatmentDetail td in this.TreatmentDetails)
            {
                td.IsAlternateCarrierPaymentVisible = this.HasClaimBeenSubmittedToOtherBenefitPlan;
            }
        }

        public string HCSAQuestion => Resource.HCSAQuestion;
    }
}
