using Acr.UserDialogs;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Extensions;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Services;
using MobileClaims.Core.Util;
using MobileClaims.Core.ViewModelParameters;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.StoreReview;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimSubmissionResultViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimService _claimservice;
        private readonly IUserDialogs _userDialogs;
        private readonly IMenuService _menuService;
        private readonly IReviewService _reviewService;
        private readonly MvxSubscriptionToken _claimsuccessful;
        private readonly MvxSubscriptionToken _shouldcloseself;

        private string _spouseTitleText;
        private bool _isSpouseTitleTextVisible;

        private ClaimResultGSC _claimSubmissionResult;
        private ObservableCollection<ClaimResultDetailQAndAList> _claimSubmissionResultDetails;
        private ObservableCollection<ClaimDetailGSC> _claimSubmissionDetails;
        private ClaimResultDetailQAndAList _planLimitation;
        private ClaimResultDetailQAndAList _totals;

        public event EventHandler ClaimResultFetched;

        public string UploadDocuments => Resource.GenericUploadDocuments;
        public string Title => Resource.claimResult_title;
        public string PlanInformationTitle => Resource.PlanInformation;
        public string GscIdNumberLabel => string.Format(Resource.GscIdNumber, BrandResource.BrandAcronym);
        public string ParticipantNameLabel => Resource.claimConfirm_participant;
        public string SubmissionDateLabel => Resource.SubmissionDateLabel;
        public string ClaimDetailTitle => Resource.ClaimDetails.ToUpperInvariant();
        public string TotalsTitle => Resource.Total.ToUpperInvariant();
        public string PlanLimitationTitle => Resource.PlanLimitations;
        public string SubmitAnotherClaim => Resource.claimResult_button.ToUpperInvariant();
        public bool IsPlanLimitationVisible => ClaimSubmissionResult?.IsPlanLimitationVisible ?? false;
        public int ResultTypeId => ClaimSubmissionResult.ResultTypeID;

        public bool IsSpouseTitleTextVisible
        {
            get => _isSpouseTitleTextVisible;
            set => SetProperty(ref _isSpouseTitleTextVisible, value);
        }

        public string SpouseTitleText
        {
            get => _spouseTitleText;
            set => SetProperty(ref _spouseTitleText, value);
        }

        public ClaimResultGSC ClaimSubmissionResult
        {
            get => _claimSubmissionResult;
            set => SetProperty(ref _claimSubmissionResult, value);
        }

        public ObservableCollection<ClaimResultDetailQAndAList> ClaimSubmissionResultDetails
        {
            get => _claimSubmissionResultDetails;
            set => SetProperty(ref _claimSubmissionResultDetails, value);
        }

        public ObservableCollection<ClaimDetailGSC> ClaimSubmissionDetails
        {
            get => _claimSubmissionDetails;
            set => SetProperty(ref _claimSubmissionDetails, value);
        }

        public ClaimResultDetailQAndAList PlanLimitation
        {
            get => _planLimitation;
            set => _planLimitation = value;
        }

        public ClaimResultDetailQAndAList Totals
        {
            get => _totals;
            set => SetProperty(ref _totals, value);
        }

        public ClaimSubmissionResultViewModel(IMvxMessenger messenger,
            IClaimService claimservice,
            IReviewService reviewService,
            IUserDialogs userDialogs,
            IMenuService menuService)
        {
            _messenger = messenger;
            _claimservice = claimservice;
            _userDialogs = userDialogs;
            _menuService = menuService;
            _reviewService = reviewService;

            Totals = new ClaimResultDetailQAndAList();
            PlanLimitation = new ClaimResultDetailQAndAList();
            ClaimSubmissionResultDetails = new ObservableCollection<ClaimResultDetailQAndAList>();

            Claim = _claimservice.ClaimResults;
            ShowUploadDocuments = Claim?.Results?.Any(x => x.ClaimResultDetails?.Any(y => y.RequiresConfirmationOfPayment) ?? false) ?? false;

            PopulateClaimResultAndDetails();
            SelectedProviderType = _claimservice.Claim.Provider.ProviderTypeCode;
            ClaimResultFetched?.Invoke(this, EventArgs.Empty);

            foreach (var claimDetailGsc in Claim.Results)
            {
                claimDetailGsc.ExecutePassDataToParent = OpenConfirmationOfPayment;
            }

            _claimsuccessful = _messenger.Subscribe<ClaimSubmissionComplete>((message) =>
            {
                _messenger.Unsubscribe<ClaimSubmissionComplete>(_claimsuccessful);

                Claim = _claimservice.ClaimResults;
                ShowUploadDocuments = Claim?.Results?.Any(x => x.ClaimResultDetails?.Any(y => y.RequiresConfirmationOfPayment) ?? false) ?? false;

                PopulateClaimResultAndDetails();
                ClaimResultFetched?.Invoke(this, EventArgs.Empty);
            });

            _shouldcloseself = _messenger.Subscribe<ClearClaimSubmissionResultViewRequested>((message) =>
            {
                _messenger.Unsubscribe<ClearClaimSubmissionResultViewRequested>(_shouldcloseself);
                Close(this);
            });

            _claimservice.ClearClaimDetails();
        }

        public async override void Start()
        {
            base.Start();
            await Task.Delay(500);
            _messenger.Publish(new ClaimSubmissionComplete(this));
            await _reviewService.RequestReview();
        }

        private void PopulateClaimResultAndDetails()
        {
            if (Claim != null && Claim.Results != null && Claim.Results.Any())
            {
                ClaimSubmissionResult = Claim.Results.FirstOrDefault();

                CreateClaimResultDetailsList();
                CreatePlanLimitation();
                CreateTotalList();

                switch (ClaimSubmissionResult.ResultTypeID)
                {
                    case 1:
                        SpouseTitleText = "CLAIM SUBMISSION RESULTS -YOUR SPOUSE'S ACCOUNT";
                        IsSpouseTitleTextVisible = true;
                        break;
                    case 2:
                        SpouseTitleText = "CLAIM SUBMISSION RESULTS " + "-"
                            + ClaimSubmissionResult.SpendingAccountModelName?.ToUpper()
                            ?? ClaimSubmissionResult.AwaitingPaymentNote;
                        IsSpouseTitleTextVisible = true;
                        break;
                    default:
                        SpouseTitleText = string.Empty;
                        IsSpouseTitleTextVisible = false;
                        break;
                }
            }
        }

        private void CreateClaimResultDetailsList()
        {
            var isClaimDetailsVisible = ClaimSubmissionResult.ClaimResultDetails != null && ClaimSubmissionResult.ClaimResultDetails.Any();
            var wasDentalClaim = string.Equals(Claim.ClaimSubmissionTypeID, "DENTAL", StringComparison.OrdinalIgnoreCase);
            var list = new ObservableCollection<ClaimResultDetailQAndAList>();

            foreach (var claimResultDetail in ClaimSubmissionResult.ClaimResultDetails.Where(crd => crd != null))
            {
                var qnaList = new ClaimResultDetailQAndAList();

                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.FormNumber,
                    Answer = claimResultDetail.ClaimFormID.ToString()
                });

                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = claimResultDetail.HasHCSADetails ? Resource.DateOfExpense
                    : wasDentalClaim
                    ? Resource.DentalServiceDate : Resource.ServiceDate,
                    Answer = claimResultDetail.ServiceDate.ToString("d")
                });

                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = claimResultDetail.HasHCSADetails ? Resource.TypeOfExpenseNoColon
                    : wasDentalClaim
                    ? Resource.TreatmentDetailsProcedureCode : Resource.ServiceDescription,
                    Answer = claimResultDetail.ServiceDescription
                });

                if (wasDentalClaim)
                {
                    qnaList.Add(new ClaimQuestionAnswerPair()
                    {
                        Question = Resource.TreatmentDetailsToothCode,
                        Answer = claimResultDetail.ToothCode
                    });
                }

                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.ClaimedAmount,
                    Answer = claimResultDetail.ClaimedAmount.AddDolarSignBasedOnCulture()
                });

                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.OtherPaidAmount,
                    Answer = claimResultDetail.OtherPaidAmount.AddDolarSignBasedOnCulture()
                });

                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.DeductibleAmount,
                    Answer = claimResultDetail.DeductibleAmount.AddDolarSignBasedOnCulture()
                });

                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.CopayAmount,
                    Answer = claimResultDetail.CopayAmount.AddDolarSignBasedOnCulture()
                });

                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.PaidAmount,
                    Answer = claimResultDetail.PaidAmount.AddDolarSignBasedOnCulture()
                });

                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.ClaimStatus,
                    Answer = claimResultDetail.ClaimStatus
                });

                var eobMessages = claimResultDetail.EOBMessages?.Select(x => x.Message).Where(x => !string.IsNullOrWhiteSpace(x));
                qnaList.EOB = eobMessages?.ToList() ?? new List<string>();
                list.Add(qnaList);
            }
            ClaimSubmissionResultDetails = list;
        }

        private void CreateTotalList()
        {
            if (ClaimSubmissionResult != null)
            {
                var qnaList = new ClaimResultDetailQAndAList();
                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.ClaimedAmount,
                    Answer =  ClaimSubmissionResult.ClaimedAmountTotal.AddDolarSignBasedOnCulture()
                });

                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.OtherPaidAmount,
                    Answer =  ClaimSubmissionResult.OtherPaidAmountTotal.AddDolarSignBasedOnCulture()
                });

                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.PaidAmount,
                    Answer = ClaimSubmissionResult.PaidAmountTotal.AddDolarSignBasedOnCulture()
                });

                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.DeductibleAmount,
                    Answer =  ClaimSubmissionResult.DeductibleAmountTotal.AddDolarSignBasedOnCulture()
                });

                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.CopayAmount,
                    Answer =  ClaimSubmissionResult.CopayAmountTotal.AddDolarSignBasedOnCulture()
                });

                var messageList = new List<string>();
                if (ClaimSubmissionResult.ClaimResultDetails != null)
                {
                    var awaitinPayments = ClaimSubmissionResult.ClaimResultDetails.Where(x => string.Equals(x.ClaimStatus, "Awaiting payment", StringComparison.OrdinalIgnoreCase));
                    if (awaitinPayments != null && awaitinPayments.Any())
                    {
                        switch (ClaimSubmissionResult.ResultTypeID)
                        {
                            case 1:
                                messageList.Add(Resource.GenericAwaitingPaymentNote);
                                break;
                            case 2:
                                messageList.Add(Resource.AwaitingPaymentNote);
                                break;
                            default:
                                messageList.Add(Resource.GenericAwaitingPaymentNote);
                                break;
                        }
                    }
                }

                qnaList.EOB = messageList;
                Totals = qnaList;
            }
        }

        private void CreatePlanLimitation()
        {
            if (ClaimSubmissionResult.IsPlanLimitationVisible)
            {
                var planLimitation = ClaimSubmissionResult.PlanLimitations.FirstOrDefault();

                var qnaList = new ClaimResultDetailQAndAList();
                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = planLimitation.BenefitDescription,
                    Answer = planLimitation.LimitationDescription
                });

                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.PlanLimitationParticipantType,
                    Answer = planLimitation.AppliesTo
                });

                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.Start,
                    Answer = planLimitation.AccumStartDate.ToString("d")
                });

                if (planLimitation.AccumAmountUsed > 0)
                {
                    qnaList.Add(new ClaimQuestionAnswerPair()
                    {
                        Question = Resource.PlanLimitationAmountUsedToDate,
                        Answer =  planLimitation.AccumAmountUsed.AddDolarSignBasedOnCulture()
                    });
                }

                if (planLimitation.AccumUnitsUsed > 0)
                {
                    qnaList.Add(new ClaimQuestionAnswerPair()
                    {
                        Question = Resource.PlanLimitationOccurrencesToDate,
                        Answer =  planLimitation.AccumUnitsUsed.ToString()
                    });
                }

                PlanLimitation = qnaList;
            }
        }

        private bool _showUploadDocuments;
        public bool ShowUploadDocuments
        {
            get => _showUploadDocuments;
            set => SetProperty(ref _showUploadDocuments, value);
        }

        private ClaimGSC _claim;
        public ClaimGSC Claim
        {
            get => _claim;
            set
            {
                _claim = value;
                if (_claim != null)
                {
                    if (_claim.Results != null && _claim.Results.Count > 0)
                    {
                        foreach (ClaimResultGSC cr in _claim.Results)
                        {
                            if (cr.ClaimResultDetails != null && cr.ClaimResultDetails.Count > 0)
                            {
                                foreach (ClaimResultDetailGSC crdg in cr.ClaimResultDetails)
                                {
                                    if (crdg.EOBMessages != null && crdg.EOBMessages.Count > 0)
                                    {
                                        List<int> emptyEobIndexes = new List<int>();
                                        for (int i = 0; i < crdg.EOBMessages.Count; i++)
                                        {
                                            if (string.IsNullOrEmpty(crdg.EOBMessages[i].Message))
                                            {
                                                emptyEobIndexes.Add(i);
                                            }
                                        }
                                        if (emptyEobIndexes.Count > 0)
                                        {
                                            foreach (int j in emptyEobIndexes)
                                            {
                                                crdg.EOBMessages.RemoveAt(j);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                SetPropertiesBasedOnResults();
                RaisePropertyChanged(() => Claim);
                RaisePropertyChanged(() => ShowUploadDocuments);
            }
        }

        public string RequiredAuditLabel => Resource.AuditListNotes;

        private string _selectedProviderType;
        public string SelectedProviderType
        {
            get => _selectedProviderType;
            set
            {
                _selectedProviderType = value;
                RaisePropertyChanged(() => SelectedProviderType);
            }
        }

        private bool _isSelectedForAudit;
        public bool IsSelectedForAudit
        {
            get => _isSelectedForAudit;
            set
            {
                _isSelectedForAudit = value;
                RaisePropertyChanged(() => IsSelectedForAudit);
            }
        }

        private string _formReferenceNumber;
        public string FormReferenceNumber
        {
            get => _formReferenceNumber;
            set
            {
                _formReferenceNumber = value;
                RaisePropertyChanged(() => FormReferenceNumber);
            }
        }

        // validation status code 6
        private bool _secondaryAccountDisabled;
        public bool SecondaryAccountDisabled
        {
            get => _secondaryAccountDisabled;
            set
            {
                _secondaryAccountDisabled = value;
                RaisePropertyChanged(() => SecondaryAccountDisabled);
            }
        }

        // validation status code 7
        private bool _secondaryAccountNotRegistered;
        public bool SecondaryAccountNotRegistered
        {
            get => _secondaryAccountNotRegistered;
            set
            {
                _secondaryAccountNotRegistered = value;
                RaisePropertyChanged(() => SecondaryAccountNotRegistered);
            }
        }

        // validation status code 8
        private bool _secondaryAccountHasntAcceptedAgreement;

        public bool SecondaryAccountHasntAcceptedAgreement
        {
            get => _secondaryAccountHasntAcceptedAgreement;
            set
            {
                _secondaryAccountHasntAcceptedAgreement = value;
                RaisePropertyChanged(() => SecondaryAccountHasntAcceptedAgreement);
            }
        }

        public string HCSAQuestion => Resource.HCSAQuestion;

        // TODO: If not used in Android, remove following property
        public string AwaitingPaymentNote => Resource.AwaitingPaymentNote;

        private void PublishMessages()
        {
            _messenger.Publish(new ClearClaimTreatmentDetailsViewRequested(this));
            _messenger.Publish(new ClearClaimSubmissionTypeViewRequested(this));
            _messenger.Publish(new ClearClaimSubmitTermsAndConditionsViewRequested(this));
            _messenger.Publish(new ClearClaimServiceProviderViewRequested(this));
            _messenger.Publish(new ClearClaimServiceProviderSearchViewRequested(this));
            _messenger.Publish(new ClearClaimServiceProviderSearchResultsViewRequested(this));
            _messenger.Publish(new ClearClaimServiceProviderEntryViewRequested(this));
            _messenger.Publish(new ClearClaimParticipantsViewRequested(this));
            _messenger.Publish(new ClearClaimDetailsViewRequested(this));
            _messenger.Publish(new ClearClaimTreatmentDetailsListViewRequested(this));
            _messenger.Publish(new ClearClaimSubmitTermsAndConditionsViewRequested(this));
            _messenger.Publish(new ClearClaimSubmissionConfirmationViewRequested(this));
            _messenger.Publish(new ClearClaimSubmissionResultViewRequested(this));
        }

        private void PublishMessagesWinRT()
        {
            _messenger.Publish(new ClearClaimTreatmentDetailsViewRequested(this));
            _messenger.Publish(new ClearClaimSubmissionTypeViewRequested(this));
            _messenger.Publish(new ClearClaimSubmitTermsAndConditionsViewRequested(this));
            _messenger.Publish(new ClearClaimServiceProviderViewRequested(this));
            _messenger.Publish(new ClearClaimServiceProviderSearchViewRequested(this));
            _messenger.Publish(new ClearClaimServiceProviderSearchResultsViewRequested(this));
            _messenger.Publish(new ClearClaimServiceProviderEntryViewRequested(this));
            _messenger.Publish(new ClearClaimDetailsViewRequested(this));
            _messenger.Publish(new ClearClaimTreatmentDetailsListViewRequested(this));
            _messenger.Publish(new ClearClaimSubmitTermsAndConditionsViewRequested(this));
            _messenger.Publish(new ClearClaimSubmissionConfirmationViewRequested(this));
            _messenger.Publish(new ClearClaimSubmissionResultViewRequested(this));
        }

        private void PublishMessagesDroid()
        {
            _messenger.Publish(new ClearClaimTreatmentDetailsViewRequested(this));
            //			_messenger.Publish<Messages.ClearClaimSubmissionTypeViewRequested>(new MobileClaims.Core.Messages.ClearClaimSubmissionTypeViewRequested(this));
            _messenger.Publish(new ClearClaimSubmitTermsAndConditionsViewRequested(this));
            _messenger.Publish(new ClearClaimServiceProviderViewRequested(this));
            _messenger.Publish(new ClearClaimServiceProviderSearchViewRequested(this));
            _messenger.Publish(new ClearClaimServiceProviderSearchResultsViewRequested(this));
            _messenger.Publish(new ClearClaimServiceProviderEntryViewRequested(this));
            _messenger.Publish(new ClearClaimDetailsViewRequested(this));
            _messenger.Publish(new ClearClaimTreatmentDetailsListViewRequested(this));
            _messenger.Publish(new ClearClaimSubmitTermsAndConditionsViewRequested(this));
            _messenger.Publish(new ClearClaimSubmissionConfirmationViewRequested(this));
            _messenger.Publish(new ClearClaimSubmissionResultViewRequested(this));
        }

        public ICommand SubmitAnotherClaimCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    PublishMessages();
                    var claimService = Mvx.IoCProvider.Resolve<IClaimService>();
                    claimService.ClearClaimDetails();
                    ShowViewModel<ClaimSubmissionTypeViewModel>();
                });
            }
        }

        public ICommand SubmitAnotherClaimDroidCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    PublishMessagesDroid();
                    Close(this);
                    ShowViewModel<ClaimSubmissionTypeViewModel>();
                });
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

            TopCardViewData topCardViewData = new TopCardViewData();
            if (ShowUploadDocuments)
            {
                topCardViewData = await GetTopCardViewData(claimSubmissionResultSummary);
            }

            if (IsSelectedForAudit)
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

        private void SetPropertiesBasedOnResults()
        {
            SecondaryAccountDisabled = (Claim.ValidationStatusCode == 6);
            SecondaryAccountNotRegistered = (Claim.ValidationStatusCode == 7);
            SecondaryAccountHasntAcceptedAgreement = (Claim.ValidationStatusCode == 8);

            foreach (ClaimResultGSC cr in Claim.Results)
            {
                if (cr.IsSelectedForAudit)
                {
                    IsSelectedForAudit = true;
                    FormReferenceNumber = cr.ClaimFormID.ToString();
                }
            }

            if (IsSelectedForAudit)
            {
                _menuService.GetMenuAsync(_loginservice.CurrentPlanMemberID);
            }
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
            topCardViewData.ClaimActionState = ClaimActionState.Cop;
            return topCardViewData;
        }

        private async Task<ClaimSummary> GetCopClaimSummaryAsync(string claimFormId)
        {
            IEnumerable<ClaimSummary> claimSummary;
            try
            {
                claimSummary = await _claimservice.GetCOPClaimSummaryAsync(claimFormId);
                if (claimSummary == null)
                {
                    throw new NullResponseException();
                }
            }
            catch (NullResponseException)
            {
                await _userDialogs.AlertAsync(Resource.ServerError_Description, Resource.ServerError_Title);
                return null;
            }
            return claimSummary.FirstOrDefault();
        }
    }
}
