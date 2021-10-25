using MobileClaims.Core.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Linq;

namespace MobileClaims.Core.Entities.ClaimsHistory
{
    public class ClaimState : MvxNotifyPropertyChanged
    {
        [JsonProperty("claimFormId")]
        public long ClaimFormID { get; set; }

        [JsonProperty("claimFormRevisionNumber")]
        public long ClaimFormRevisionNumber { get; set; }

        [JsonProperty("claimId")]
        public long ClaimID { get; set; }

        [JsonProperty("claimRevisionNumber")]
        public long ClaimRevisionNumber { get; set; }

        [JsonProperty("claimDetailId")]
        public long ClaimDetailID { get; set; }

        [JsonProperty("planMemberId")]
        public long PlanMemberID { get; set; }

        [JsonProperty("participantNumber")]
        public string ParticipantNumber { get; set; }

        [JsonProperty("serviceDate")]
        public DateTime ServiceDate { get; set; }

        [JsonProperty("benefitId")]
        public string BenefitID { get; set; }

        [JsonProperty("serviceDescription")]
        public string ServiceDescription { get; set; }

        [JsonProperty("claimedAmount")]
        public double ClaimedAmount { get; set; }

        [JsonProperty("otherPaidAmount")]
        public double OtherPaidAmount { get; set; }

        [JsonProperty("copayAmount")]
        public double? CopayAmount { get; set; }

        [JsonProperty("paidAmount")]
        public double? PaidAmount { get; set; }

        [JsonProperty("quantity")]
        public double? Quantity { get; set; }

        [JsonProperty("payeeId")]
        public string PayeeID { get; set; }

        [JsonIgnore]
        public ClaimHistoryPayee Payee { get; set; }

        [JsonProperty("processedDate")]
        public DateTime? ProcessedDate { get; set; }

        [JsonIgnore]
        public string ProcessedDateAsString
        {
            get
            {
                if (ProcessedDate == null || ProcessedDate == DateTime.MinValue)
                {
                    return Resource.InProcess;
                }
                else
                {
                    DateTime dt = DateTime.Parse(ProcessedDate.ToString());
                    IDeviceService deviceservice = Mvx.IoCProvider.Resolve<IDeviceService>();
                    if (deviceservice.CurrentDevice == GSCHelper.OS.Droid)
                        return dt.ToString(GSCHelper.CLAIMS_HISTORY_SEARCH_DATE_FORMAT_ANDROID);
                    else if (deviceservice.CurrentDevice == GSCHelper.OS.iOS)
                        return dt.ToString(GSCHelper.CLAIMS_HISTORY_SEARCH_DATE_FORMAT_IOS);
                    else
                        return dt.ToString();
                }
            }
        }

        [JsonProperty("isStricken")]
        public bool IsStricken { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("eobMessages")]
        public List<ClaimEOBMessageGSC> EOBMessages { get; set; }

        [JsonProperty("payment")]
        public ClaimPayment Payment { get; set; }

        [JsonProperty("isCOP")]
        public bool RequiresConfirmationOfPayment { get; set; }

        [JsonProperty("claimActionStatus")]
        public ClaimActionState ClaimActionStatus { get; set; }

        [JsonIgnore]
        public ClaimHistoryType ClaimHistoryType { get; set; }

        public string ServiceDateLabel => Resource.ServiceDate;

        public string ClaimFormNumberLabel => Resource.ClaimFormNumber;

        public string ServiceDescriptionLabel => Resource.ServiceDescription;

        public string ClaimedAmountLabel => Resource.ClaimedAmount;

        public string OtherPaidAmountLabel => Resource.OtherPaidAmount;

        public string PaidAmountLabel => Resource.PaidAmount;

        public string CopayLabel => Resource.CopayDeductible;

        public string PaymentDateLabel
        {
            get
            {
                if (ClaimHistoryType.ID == GSCHelper.ClaimsHistoryTypePDTID || ClaimHistoryType.ID == GSCHelper.ClaimsHistoryTypeMIID)
                    return Resource.ProcessedDate;
                else
                    return Resource.PaymentDateNoColon;
            }
        }

        public string PaidToLabel => Resource.PaidTo;

        public string EOBLabel => Resource.ExplanationOfBenefits;

        public string QuantityLabel => Resource.Quantity;

        public string ClaimStatusLabel => Resource.ClaimStatus;

        public bool IsProcessedDateVisible => true;

        public bool IsStatusVisible => ClaimHistoryType.ID == GSCHelper.ClaimsHistoryTypePDTID || ClaimHistoryType.ID == GSCHelper.ClaimsHistoryTypeMIID;

        public bool IsQuantityVisible => BenefitID == GSCHelper.ClaimHistoryBenefitIDForDrug;

        public bool IsServiceDateVisible => ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypePDTID && ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypeMIID;

        public bool IsClaimIDVisible => ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypePDTID && ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypeMIID;

        public bool IsServiceDescriptionVisible => ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypePDTID && ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypeMIID;

        public bool IsClaimedAmountVisible => ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypePDTID && ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypeMIID;

        public bool IsOtherPaidAmountVisible => ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypePDTID && ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypeMIID;

        public bool IsPaidAmountVisible => ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypePDTID && ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypeMIID;

        public bool IsCopayAmountVisible => ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypePDTID && ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypeMIID;

        public bool IsPaidToVisible => ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypePDTID && ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypeMIID;

        public bool IsEOBMessagesVisible => ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypePDTID && ClaimHistoryType.ID != GSCHelper.ClaimsHistoryTypeMIID;

        public Action ExecuteOpenConfirmationOfPayment { get; set; }

        public string UploadDocumentsLabel => Resource.GenericUploadDocuments;

        public IMvxCommand UploadDocumentsCommand => new MvxCommand(UploadDocuments);

        private void UploadDocuments()
        {
            ExecuteOpenConfirmationOfPayment.Invoke();
        }

        private bool _isPaymentButtonVisible;

        public bool IsPaymentButtonVisible
        {
            get => _isPaymentButtonVisible;
            set
            {
                _isPaymentButtonVisible = value;
                RaisePropertyChanged(nameof(IsPaymentButtonVisible));
            }
        }

        public Action ExecuteOpenPaymentInfo { get; set; }

        public string PaymentInformationLabel => Resource.PaymentInformation;

        public IMvxCommand ShowPaymentInfoCommand => new MvxCommand(ShowPaymentInfo);

        private void ShowPaymentInfo()
        {
            ExecuteOpenPaymentInfo.Invoke();
        }
        public string EOBMessage
        {
            get
            {
                return string.Join(Environment.NewLine, EOBMessages.Select(x => x.Message));
            }
            
        }
    }
}