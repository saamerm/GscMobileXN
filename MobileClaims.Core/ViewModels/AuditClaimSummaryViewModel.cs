using Acr.UserDialogs;
using MobileClaims.Core.Constants;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Extensions;
using MobileClaims.Core.Models;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Services;
using MobileClaims.Core.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using MobileClaims.Core.ViewModelParameters;

namespace MobileClaims.Core.ViewModels
{
    public class AuditClaimSummaryViewModel : ViewModelBase<AuditClaimSummaryViewModelParameters>, IClaimSummaryProperties
    {
        private readonly IClaimExService _claimService;
        private readonly IUserDialogs _userDialog;

        private string _userName;
        private AuditClaimSummaryHeader _auditClaimSummaryHeader;
        private AuditClaimSummaryFooter _auditClaimSummaryFooter;
        private ObservableCollection<ClaimFormClaimSummary> _claims;
        private DashboardRecentClaim _recentClaim;

        public string TwentyFourMb => Resource.TwentyFourMb;
        public string UploadDocuments => Resource.GenericUploadDocuments;
        public string CombinedSizeOfFilesMustBe => Resource.CombinedSizeOfFilesMustBe;
        public string SubmitAdditionalInformationLabel => Resource.ClaimHistoryDetailRequriesCopTrue;
        public string ClaimDetailsSectionTitle => Resource.ClaimDetails.ToUpperInvariant();

        public string Title { get; private set; }
        public bool IsUploadSectionVisible { get; private set; }
        public string UploadButtonText { get; private set; }

        public event EventHandler ClaimsFetched;
        public MvxCommand OpenUploadDocumentsCommand { get; }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public AuditClaimSummaryHeader AuditClaimSummaryHeader
        {
            get => _auditClaimSummaryHeader;
            set => SetProperty(ref _auditClaimSummaryHeader, value);
        }

        public AuditClaimSummaryFooter AuditClaimSummaryFooter
        {
            get => _auditClaimSummaryFooter;
            set => SetProperty(ref _auditClaimSummaryFooter, value);
        }

        public ObservableCollection<ClaimFormClaimSummary> Claims
        {
            get => _claims;
            set => SetProperty(ref _claims, value);
        }

        public AuditClaimSummaryViewModel(IClaimExService claimService, IUserDialogs userDialogs)
        {
            _claimService = claimService;
            _userDialog = userDialogs;

            Claims = new ObservableCollection<ClaimFormClaimSummary>();
            OpenUploadDocumentsCommand = new MvxCommand(OpenUploadDocuments);
        }

        //public override async void Start()
        public override async Task Initialize()
        {
            _userDialog.ShowLoading(Resource.Loading);
            await base.Initialize();

            try
            {
                await GetClaimResult();
            }
            catch (ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await LogoutWithAlertMessageAsync();
            }
            catch (Exception ex)
            {
                _userDialog.HideLoading();

                // TODO: Fix following error
                // _userDialog. ShowError(Resource.GenericErrorDialogMessage);
            }
            finally
            {
                _userDialog.HideLoading();
            }
        }

        private async Task GetClaimResult()
        {
            var claimResult = await _claimService.GetClaimResult(_recentClaim.ClaimFormId);
            if (claimResult != null)
            {
                UserName = claimResult.ParticipantFullName;
                _recentClaim = DeriveRecentClaimInfo(claimResult);

                foreach (var claimResultDetail in claimResult.ClaimResultDetails.Select((item, index) => new { item, index }))
                {
                    var claimFormClaimSummary = new ClaimFormClaimSummary
                    {
                        CountValue = string.Format(Resource.ClaimCountOutOf, claimResultDetail.index + 1, claimResult.ClaimResultDetails.Count),

                        ServiceDateValue = claimResultDetail.item.ServiceDate.ToString("d"),
                        ServiceDescriptionValue = claimResultDetail.item.ServiceDescription,

                        ClaimedAmount = claimResultDetail.item.ClaimedAmountLabel,
                        ClaimedAmountValue = claimResultDetail.item.ClaimedAmount.AddDolarSignBasedOnCulture(),

                        // OtherPaidAmount, PaidAmout, and Copay are not available for Audit Claim Summary
                        // However, the mockups for intended not just for Audit but also for COP and non-action claims, these fields are added in ViewModel
                        // in iOS these are already implemented in the view, but they are hidden at runtime if values are string.Empty
                        OtherPaidAmount = string.Empty,
                        OtherPaidAmountValue = string.Empty,

                        PaidAmount = string.Empty,
                        PaidAmountValue = string.Empty,

                        Copay = string.Empty,
                        CopayValue = string.Empty,

                        ClaimStatus = string.Empty,
                        ClaimStatusValue = string.Empty,

                        ClaimActionState = claimResult.ClaimActionStatus
                    };

                    Claims.Add(claimFormClaimSummary);
                }

                AuditClaimSummaryHeader = new AuditClaimSummaryHeader
                {
                    SubmissionDateLabel = claimResult.SubmissionDateLabel,
                    GscNumber = claimResult.PlanMemberDisplayID,
                    ClaimFormNumber = claimResult.ClaimFormID.ToString(),
                    SubmissionDate = claimResult.SubmissionDate.ToString("d")
                };

                AuditClaimSummaryFooter = new AuditClaimSummaryFooter
                {
                    ClaimStatusValue = claimResult.ClaimActionStatus == ClaimActionState.Audit
                            ? Resource.AuditClaimStatus
                            : "Action Required",
                    AuditDueDate = claimResult.AuditDueDate.ToString("d"),
                    AuditInformation = string.Join(Environment.NewLine + Environment.NewLine,
                        claimResult.ActionRequiredEobMessages?.Select(x => x.Message).Distinct().ToList() ?? new List<string>())
                };

                ClaimsFetched?.Invoke(this, EventArgs.Empty);
            }
        }

        private DashboardRecentClaim DeriveRecentClaimInfo(ClaimResultResponse claimResult)
        {
            var minDate = claimResult.ClaimResultDetails.Select(x => x.ServiceDate).Min();
            var maxDate = claimResult.ClaimResultDetails.Select(x => x.ServiceDate).Max();

            return new DashboardRecentClaim
            {
                ClaimFormId = claimResult.ClaimFormID.ToString(),
                ServiceDescription = claimResult.BenefitTypeDescr,
                ServiceDate = minDate.ToString("d") == maxDate.ToString("d")
                    ? minDate.ToString("d")
                    : $"{minDate.ToString("d")} - {maxDate.ToString("d")}",
                ClaimedAmount = claimResult.ClaimedAmountTotal.AddDolarSignBasedOnCulture(),
                ActionRequired = true,
                ClaimActionState = ClaimActionState.Audit
            };
        }

        private void OpenUploadDocuments()
        {
            var topCardViewData = new TopCardViewData
            {
                ClaimForm = _recentClaim.ClaimFormId,
                ClaimedAmount = _recentClaim.ClaimedAmount,
                ServiceDescription = _recentClaim.ServiceDescription,
                ServiceDate = _recentClaim.ServiceDate,
                UserName = UserName,
                // Need this in claim result response.
                // ParticipantNumber = _recentClaim.ParticipantNumber,
                EobMessages = AuditClaimSummaryFooter.AuditInformation,
                ClaimActionState = _recentClaim.ClaimActionState
            };

            var copPlanMemberData = new UploadDocumentsFormData
            {
                ClaimFormId = long.Parse(topCardViewData.ClaimForm),
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