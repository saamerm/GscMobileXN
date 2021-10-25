using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MobileClaims.Core.Constants;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Extensions;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Services;
using MobileClaims.Core.Util;
using MobileClaims.Core.ViewModelParameters;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Newtonsoft.Json;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimSummaryViewModel : ViewModelBase<AuditClaimSummaryViewModelParameters>, IClaimSummaryProperties
    {
        private readonly IClaimService _claimService;
        private readonly IUserDialogs _userDialog;

        private ClaimSummaryData _claimSummary;
        private DashboardRecentClaim _recentClaim;

        public string Title { get; private set; }
        public bool IsUploadSectionVisible { get; private set; }
        public string UploadButtonText { get; private set; }

        public string ServiceDate => Resource.ServiceDate;
        public string ClaimForm => Resource.ClaimFormNumber;
        public string ServiceDescription => Resource.ServiceDescription;
        public string ClaimedAmount => Resource.ClaimedAmount;
        public string OtherPaidAmount => Resource.OtherPaidAmount;
        public string PaidAmount => Resource.PaidAmount;
        public string CoPay => Resource.CopayDeductible;
        public string PaymentDate => Resource.PaymentDate;
        public string PaidTo => Resource.PaidTo;
        public string Eob => Resource.ExplanationOfBenefits;

        public string TwentyFourMb => Resource.TwentyFourMb;
        public string DocumentsToUpload => Resource.DocumentsToUpload;
        public string ExplanationOfBenefitsLabel => Resource.ExplanationOfBenefits;
        public string CombinedSizeOfFilesMustBe => Resource.CombinedSizeOfFilesMustBe;
        public string SubmitAdditionalInformationLabel => Resource.ClaimHistoryDetailRequriesCopTrue;

        public IMvxCommand OpenUploadDocumentsCommand { get; }

        public ClaimSummaryData ClaimSummary
        {
            get => _claimSummary;
            private set => SetProperty(ref _claimSummary, value);
        }

        public ClaimSummaryViewModel(IClaimService claimService, IUserDialogs userDialogs)
        {
            _claimService = claimService;
            _userDialog = userDialogs;

            OpenUploadDocumentsCommand = new MvxCommand(OpenUploadDocuments);
        }

        public override async Task Initialize()
        {
            _userDialog.ShowLoading(Resource.Loading);
            await base.Initialize();
            try
            {
                var claimSummary =
                    await _claimService.GetCOPClaimSummaryAsync(
                        $"{_recentClaim.ClaimFormId}-{_recentClaim.ClaimDetailId}");
                var claim = claimSummary.FirstOrDefault();

                if (claim == null)
                {
                    throw new NullResponseException();
                }

                var claimPaymentDate = claim.PaymentDate.Date == DateTime.MinValue
                        ? null
                        : claim.PaymentDate.Date.ToString("d");
                            
                ClaimSummary = new ClaimSummaryData
                {
                    ParticipantNumber = claim.ParticipantNumber,
                    UserName = claim.ParticipantName,
                    ServiceDate = claim.MinServeDate.Date.ToString("d") == claim.MaxServeDate.Date.ToString("d")
                        ? claim.MinServeDate.Date.ToString("d")
                        : $"{claim.MinServeDate.Date:d} - {claim.MaxServeDate.Date:d}",
                    ClaimForm = claim.ClaimFormID.ToString(),
                    ServiceDescription = claim.BenefitTypeDescr,
                    ClaimedAmount = claim.TotalCdRendAmt?.AddDolarSignBasedOnCulture(),
                    OtherPaidAmount = claim.OtherPaidAmt?.AddDolarSignBasedOnCulture(),
                    PaidAmount = claim.TotalPaidAmt?.AddDolarSignBasedOnCulture(),
                    Copay = claim.TotalCoPayAmt?.AddDolarSignBasedOnCulture(),
                    PaymentDate = claim.Status.Equals("in process", StringComparison.OrdinalIgnoreCase)
                        ? claim.Status
                        : claimPaymentDate,
                    PayTo = claim.PayeeType,
                    EobMessages = string.Join(Environment.NewLine + Environment.NewLine,
                        claim.EOBMessages?.Select(x => x.Message).Distinct().ToList() ?? new List<string>()),
                    IsCOP = claim.IsCop,
                    IsSelectedForAudit = claim.IsSelectedForAudit
                };
            }
            catch (Exception)
            {
                _userDialog.HideLoading();

                // TODO: Fix following error
                // _userDialog.ShowError(Resource.GenericErrorDialogMessage);               
            }
            finally
            {
                _userDialog.HideLoading();
            }
        }

        private void OpenUploadDocuments()
        {
            var topCardViewData = new TopCardViewData
            {
                ClaimForm = ClaimSummary.ClaimForm,
                ClaimedAmount = ClaimSummary.ClaimedAmount,
                ServiceDescription = ClaimSummary.ServiceDescription,
                ServiceDate = ClaimSummary.ServiceDate,
                UserName = ClaimSummary.UserName,
                ParticipantNumber = ClaimSummary.ParticipantNumber,
                EobMessages = ClaimSummary.EobMessages,
                ClaimActionState = _recentClaim.ClaimActionState
            };

            var copPlanMemberData = new UploadDocumentsFormData
            {
                ClaimFormId = long.Parse(ClaimSummary.ClaimForm),
                ParticipantNumber = topCardViewData.ParticipantNumber
            };

            IClaimUploadProperties uploadable = (IClaimUploadProperties)UploadFactory.Create(_recentClaim.ClaimActionState, nameof(ConfirmationOfPaymentUploadViewModel));

            ShowViewModel<ConfirmationOfPaymentUploadViewModel, IViewModelParameters>(new ConfirmationOfPaymentUploadViewModelParameters(topCardViewData, copPlanMemberData, uploadable));
        }

        public override void Prepare(AuditClaimSummaryViewModelParameters parameter)
        {
            _recentClaim = parameter.RecentClaim;
            Title = parameter.ClaimSummary.Title;
            IsUploadSectionVisible = parameter.ClaimSummary.IsUploadSectionVisible;
            UploadButtonText = parameter.ClaimSummary.UploadButtonText;
        }
    }
}