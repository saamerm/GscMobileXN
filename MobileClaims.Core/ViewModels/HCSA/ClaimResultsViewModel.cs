using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using MobileClaims.Core.Constants;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Entities.HCSA;
using MobileClaims.Core.Extensions;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.HCSA;
using MobileClaims.Core.Util;
using MobileClaims.Core.ViewModelParameters;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using Newtonsoft.Json;
using Claim = MobileClaims.Core.Entities.HCSA.Claim;

namespace MobileClaims.Core.ViewModels.HCSA
{
    public class ClaimResultsViewModel : HCSAViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IHCSAClaimService _claimservice;
        private readonly IClaimService _standardclaimservice;
        private readonly IParticipantService _participantservice;
        private readonly IUserDialogs _userDialogs;
        private readonly IMenuService _menuService;
        private readonly MvxSubscriptionToken _shouldcloseself;

        public ClaimResultsViewModel(IMvxMessenger messenger, IHCSAClaimService claimservice,
            IClaimService standardclaimservice, IParticipantService participantservice, IUserDialogs userDialogs, IMenuService menuService)
        {
            _messenger = messenger;
            _claimservice = claimservice;
            _standardclaimservice = standardclaimservice;
            _participantservice = participantservice;
            _userDialogs = userDialogs;
            _menuService = menuService;

            _shouldcloseself = _messenger.Subscribe<ClearClaimResultsViewRequested>(message =>
            {
                _messenger.Unsubscribe<ClearClaimResultsViewRequested>(_shouldcloseself);
                Close(this);
            });

            Claim = _claimservice.Claim;
            Claim.ClaimType = _claimservice.SelectedClaimType;
            if (Claim != null)
            {
                if (Claim.Results != null)
                {
                    foreach (ClaimResultGSC CR in Claim.Results)
                    {
                        CR.ReferralType = ReferralType;
                    }
                }
            }
            Claim.ExpenseType = _claimservice.SelectedExpenseType;
            SetResultsValues();

            _standardclaimservice.ClearClaimDetails();
        }

        MvxCommand _SubmitAnotherClaimCommand;
        public ICommand SubmitAnotherClaimCommand
        {
            get
            {
                _SubmitAnotherClaimCommand = _SubmitAnotherClaimCommand ?? new MvxCommand(() =>
                                                              {
                                                                  _claimservice.Claim = new Claim();
                                                                  _standardclaimservice.ClearClaimDetails();
                                                                  _standardclaimservice.IsHCSAClaim = false;
                                                                  _participantservice.SelectedParticipant = null;
                                                                  var _dataservice = Mvx.IoCProvider.Resolve<IDataService>();
                                                                  _dataservice.PersistHCSAClaim(_claimservice.Claim);
                                                                  _claimservice.SelectedClaimType = null;
                                                                  _claimservice.SelectedExpenseType = null;
                                                                  PublishMessages();
                                                                  ShowViewModel<ClaimSubmissionTypeViewModel>();
                                                              }
                                                         );
                return _SubmitAnotherClaimCommand;
            }
        }
        
        public bool ShowUploadDocuments => Claim?.Results?.Any(x => x.ClaimResultDetails?.Any(y => y.RequiresConfirmationOfPayment) ?? false) ?? false;

        public string RequiredAuditLabel => Resource.AuditListNotes;

        private bool _isSelectedForAudit;
        public bool IsSelectedForAudit
        {
            get
            {
                return _isSelectedForAudit;
            }
            set
            {
                _isSelectedForAudit = value;
                RaisePropertyChanged(() => IsSelectedForAudit);
            }
        }

        private Claim _claim;
        public Claim Claim
        {
            get => _claim;
            set
            {
                SetProperty(ref _claim, value);
                SetPropertyBasedOnResults();
                RaisePropertyChanged(() => ShowUploadDocuments);
            }
        }

        private void SetPropertyBasedOnResults()
        {
            foreach (ClaimResultGSC cr in Claim.Results)
            {
                if (cr.IsSelectedForAudit)
                {
                    IsSelectedForAudit = true;
                }
            }

            if (IsSelectedForAudit)
            {
                UploadDocuments = Resource.GenericUploadDocuments;
                _menuService.GetMenuAsync(_loginservice.CurrentPlanMemberID);
            }
        }

        public string ExpenseType
        {
            get
            {
                if (Claim.ExpenseType != null)
                {
                    return Claim.ExpenseType.Name;
                }

                return Claim.ClaimType.Name;
            }
        }

        private List<HCSAReferralType> MedicalProfessionalTypes = JsonConvert.DeserializeObject<List<HCSAReferralType>>(Resource.HCSAReferralTypes);

        public HCSAReferralType ReferralType
        {
            get
            {
                return MedicalProfessionalTypes.Where(mpt => mpt.Code == Claim.MedicalProfessionalID).FirstOrDefault();
            }
        }

        public bool IsReferrerVisible => ReferralType != null;

        public string ClaimedAmountLabel => Resource.ClaimedAmount;
        public string OtherPaidAmountLabel => Resource.OtherPaidAmount;
        public string TypeExpenseLabel => Resource.claimConfirm_type_expense;
        public string TotalLabel => Resource.claimConfirm_total;
        public string TitleLabel => Resource.claimResult_title;
        public string RefferedLabel => Resource.claimConfirm_reffered;
        public string PlanInfoLabel => Resource.claimConfirm_plan_info;
        public string ParticipantLabel => Resource.claimConfirm_participant;
        public string IDNumberLabel => BrandResource.claimConfirm_id_number;
        public string GovernmentPlanLabel => Resource.claimConfirm_government;
        public string DetailsLabel => Resource.claimConfirm_detais;
        public string DescriptionLabel => Resource.claimResult_description;
        public string SubmitButtonLabel => Resource.claimResult_button;
        public string DateOfExpenseLabel => Resource.DateOfExpense;
        public string ErrorMessageLabel => Resource.error_message;
        public string AwaitingPaymentNote => Resource.AwaitingPaymentNote;
        public string SubmissionDateLabel => Resource.SubmissionDateLabel;

        protected override void PublishMessages()
        {
            base.PublishMessages();
            _messenger.Publish(new ClearClaimTypeViewRequested(this));
            _messenger.Publish(new ClearClaimParticipantsViewRequested(this));
            _messenger.Publish(new ClearClaimDetailsViewRequested(this)); //closes ReviewAndEdit
            _messenger.Publish(new ClearClaimSubmitTermsAndConditionsViewRequested(this));
            _messenger.Publish(new ClearClaimConfirmationHCSAViewRequested(this));
            _messenger.Publish(new ClearClaimResultsViewRequested(this));
        }

        private void SetResultsValues()
        {
            if (Claim.Results == null || Claim.Results.Count == 0)
                return;

            if (Claim.Results[0].ClaimResultDetails == null || Claim.Results[0].ClaimResultDetails.Count == 0)
                return;

            foreach (ClaimDetail cd in Claim.Details)
            {
                ClaimResultDetailGSC result = Claim.Results[0].ClaimResultDetails.FirstOrDefault(crd => (crd.ServiceDate == cd.ExpenseDate.Value && crd.ClaimedAmount == cd.ClaimAmount));
                if (result != null)
                {
                    cd.PaidAmount = result.PaidAmount;
                    cd.ClaimStatus = result.ClaimStatus;
                    cd.ClaimFormID = Claim.Results[0].ClaimFormID;
                    cd.EOBMessages = result.EOBMessages;
                }
            }
        }

        public string PaidAmountLabel => Resource.PaidAmount;

        private string _uploadDocuments = Resource.UploadDocuments;
        public string UploadDocuments
        {
            get => _uploadDocuments;
            set
            {
                SetProperty(ref _uploadDocuments, value);
            }
        }

        public MvxCommand OpenConfirmationOfPaymentCommand => new MvxCommand(OpenConfirmationOfPayment);

        private async void OpenConfirmationOfPayment()
        {
            var claimSubmissionResultSummary = Claim.Results?.FirstOrDefault();

            if (claimSubmissionResultSummary == null)
            {
                await _userDialogs.AlertAsync(Resource.ServerError_Description, Resource.ServerError_Title);
                return;
            }

            var topCardViewData = new TopCardViewData();
            if (ShowUploadDocuments)
            {
                topCardViewData = await GetTopCardViewData(claimSubmissionResultSummary);
            }
            else
            {
                topCardViewData = GetTopCardViewDataForAuditUpload(claimSubmissionResultSummary);
            }                

            var coPPlanMemberData = new UploadDocumentsFormData
            {
                ClaimFormId = claimSubmissionResultSummary.ClaimFormID,
                ParticipantNumber = topCardViewData.ParticipantNumber
            };
            IClaimUploadProperties uploadable = (IClaimUploadProperties)UploadFactory.Create(topCardViewData.ClaimActionState, nameof(ConfirmationOfPaymentUploadViewModel));

            await ShowViewModel<ConfirmationOfPaymentUploadViewModel, IViewModelParameters>(new ConfirmationOfPaymentUploadViewModelParameters(topCardViewData, coPPlanMemberData, uploadable));
        }

        private TopCardViewData GetTopCardViewDataForAuditUpload(ClaimResultGSC submitedClaim)
        {
            var minDate = submitedClaim.ClaimResultDetails.Select(x => x.ServiceDate).Min();
            var maxDate = submitedClaim.ClaimResultDetails.Select(x => x.ServiceDate).Max();

            return new TopCardViewData()
            {
                ClaimForm = submitedClaim.ClaimFormID.ToString(),
                ClaimedAmount = submitedClaim.ClaimedAmountTotal.AddDolarSignBasedOnCulture(),
                ServiceDescription = submitedClaim.BenefitTypeDescr,
                ServiceDate = minDate.ToString("d") == maxDate.ToString("d")
                    ? minDate.ToString("d")
                    : $"{minDate.ToString("d")} - {maxDate.ToString("d")}",

                UserName = submitedClaim.ParticipantFullName,
                ClaimActionState = ClaimActionState.Audit,
            };
        }

        private async Task<TopCardViewData> GetTopCardViewData(ClaimResultGSC submitedClaim)
        {
            var topCardViewData = new TopCardViewData();
            var claimFormId = submitedClaim.ClaimFormID.ToString();

            var claim = await GetCopClaimSummaryAsync(claimFormId);

            topCardViewData.ClaimForm = claim.ClaimFormID.ToString();
            topCardViewData.ClaimedAmount = claim.TotalCdRendAmt?.AddDolarSignBasedOnCulture();
            topCardViewData.ServiceDescription = claim.BenefitTypeDescr;
            topCardViewData.ServiceDate = claim.MinServeDate.Date.ToString("d") == claim.MaxServeDate.Date.ToString("d")
                ? claim.MinServeDate.Date.ToString("d")
                : $"{claim.MinServeDate.Date:d} - {claim.MaxServeDate.Date:d}";
            topCardViewData.UserName = claim.ParticipantName;
            topCardViewData.ParticipantNumber = claim.ParticipantNumber;
            topCardViewData.EobMessages = string.Join(Environment.NewLine + Environment.NewLine,
                claim.EOBMessages.Select(x => x.Message).Distinct().ToList());

            return topCardViewData;
        }

        private async Task<ClaimSummary> GetCopClaimSummaryAsync(string claimFormId)
        {
            IEnumerable<ClaimSummary> claimSummary;
            try
            {
                claimSummary = await _standardclaimservice.GetCOPClaimSummaryAsync(claimFormId);
                if (claimSummary == null)
                {
                    throw new NullResponseException();
                }
            }
            catch (NullResponseException e)
            {
                await _userDialogs.AlertAsync(Resource.ServerError_Description, Resource.ServerError_Title);
                return null;
            }
            return claimSummary.FirstOrDefault();
        }
    }
}
