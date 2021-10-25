using MobileClaims.Core.Entities.HCSA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.Entities
{
    public class ClaimResultGSC : IMvxViewModel
    {
        [JsonProperty("resultTypeId")]
        public int ResultTypeID { get; set; }

        [JsonProperty("claimFormId")]
        public long ClaimFormID { get; set; }

        [JsonProperty("benefitType_Descr")]
        public string BenefitTypeDescr { get; set; }

        [JsonProperty("planMemberDisplayId")]
        public string PlanMemberDisplayID { get; set; }

        [JsonProperty("participantFullName")]
        public string ParticipantFullName { get; set; }

        [JsonProperty("submissionDate")]
        public DateTime SubmissionDate { get; set; }

        [JsonProperty("claimResultDetails")]
        public List<ClaimResultDetailGSC> ClaimResultDetails { get; set; }

        [JsonProperty("marketingMessage")]
        public string MarketingMessage { get; set; }

        [JsonProperty("awaitingPaymentMessage")]
        public string AwaitingPaymentMessage { get; set; }

        [JsonProperty("planLimitations")]
        public List<ClaimPlanLimitationGSC> PlanLimitations { get; set; }

        [JsonProperty("isSelectedForAudit")]
        public bool IsSelectedForAudit { get; set; }

        [JsonProperty("spendingAccountModelName")]
        public string SpendingAccountModelName { get; set; }

        [JsonIgnore]
        public double ClaimedAmountTotal
        {
            get
            {
                double total = 0.0;
                foreach (ClaimResultDetailGSC crd in ClaimResultDetails)
                {
                    total += crd.ClaimedAmount;
                }
                return total;
            }
        }

        [JsonIgnore]
        public double OtherPaidAmountTotal
        {
            get
            {
                double total = 0.0;
                foreach (ClaimResultDetailGSC crd in ClaimResultDetails)
                {
                    total += crd.OtherPaidAmount;
                }
                return total;
            }
        }

        [JsonIgnore]
        public double DeductibleAmountTotal
        {
            get
            {
                double total = 0.0;
                foreach (ClaimResultDetailGSC crd in ClaimResultDetails)
                {
                    total += crd.DeductibleAmount;
                }
                return total;
            }
        }

        [JsonIgnore]
        public double CopayAmountTotal
        {
            get
            {
                double total = 0.0;
                foreach (ClaimResultDetailGSC crd in ClaimResultDetails)
                {
                    total += crd.CopayAmount;
                }
                return total;
            }
        }

        [JsonIgnore]
        public double PaidAmountTotal
        {
            get
            {
                double total = 0.0;
                foreach (ClaimResultDetailGSC crd in ClaimResultDetails)
                {
                    total += crd.PaidAmount;
                }
                return total;
            }
        }

        public void ViewCreated()
        {
            // throw new NotImplementedException();
        }

        public void ViewAppearing()
        {
            // throw new NotImplementedException();
        }

        public void ViewAppeared()
        {
            // throw new NotImplementedException();
        }

        public void ViewDisappearing()
        {
            // throw new NotImplementedException();
        }

        public void ViewDisappeared()
        {
            // throw new NotImplementedException();
        }

        public void ViewDestroy(bool viewFinishing = true)
        {
            // throw new NotImplementedException();
        }

        public void Init(IMvxBundle parameters)
        {
        }

        public void ReloadState(IMvxBundle state)
        {
        }

        public string RequestedBy { get; set; }

        public void SaveState(IMvxBundle state)
        {
        }

        public void Prepare()
        {
            //throw new NotImplementedException();
        }

        public Task Initialize()
        {
            return Task.Delay(1);
            //throw new NotImplementedException();
        }

        public MvxNotifyTask InitializeTask { get; set; }

        public void Start()
        {
        }

        [JsonIgnore]
        public bool IsPlanLimitationVisible
        {
            get
            {
                if (PlanLimitations != null)
                {
                    return (PlanLimitations.Count > 0);
                }
                else
                {
                    return false;
                }
            }
        }


        [JsonIgnore]
        public HCSAReferralType ReferralType { get; set; }

        [JsonIgnore]
        public bool IsReferrerVisible => ReferralType != null;


        [JsonIgnore]
        public string ParticipantLabel => Resource.claimConfirm_participant;

        [JsonIgnore]
        public string IDNumberLabel => BrandResource.claimConfirm_id_number;

        [JsonIgnore]
        public string RefferedLabel => Resource.claimConfirm_reffered;

        [JsonIgnore]
        public string ClaimedAmountLabel => Resource.ClaimedAmount;

        [JsonIgnore]
        public string PaidAmountLabel => Resource.PaidAmount;

        [JsonIgnore]
        public string OtherPaidLabel => Resource.OtherPaidAmount;

        [JsonIgnore]
        public string DateOfExpenseLabel => Resource.DateOfExpense;

        [JsonIgnore]
        public string GovernmentPlanLabel => Resource.OtherPaidAmount;

        [JsonIgnore]
        public string TypeExpenseLabel => Resource.claimConfirm_type_expense;

        [JsonIgnore]
        public string ClaimStatusLabel => Resource.ClaimStatus;

        [JsonIgnore]
        public string AwaitingPaymentLabel => Resource.AwaitingPayment;

        // TODO: Remove this property if not used in Android
        public string AwaitingPaymentNote => Resource.AwaitingPaymentNote;

        public string FormNumberLabel => Resource.ClaimFormNumber;

        public string SubmissionDateLabel => Resource.SubmissionDateLabel;

        public string UploadDocumentsLabel => Resource.UploadDocuments;

        public bool ShowUploadDocuments => ClaimResultDetails.Any(x => x.RequiresConfirmationOfPayment);

        public Action ExecutePassDataToParent { get; set; }

        public IMvxCommand UploadDocumentsCommand => new MvxCommand(UploadDocuments);

        private void UploadDocuments()
        {
            ExecutePassDataToParent.Invoke();
        }
    }
}
