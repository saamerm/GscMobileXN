using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class EligibilityCheckDREViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimService _claimservice;
        private readonly IEligibilityService _eligibilityservice;
        private readonly MvxSubscriptionToken _eligibilitycheckcomplete;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private MvxSubscriptionToken _gettextalterationphone;
        private MvxSubscriptionToken _gettextalterationphoneerror;
        private MvxSubscriptionToken _notextalterationphone;

        public EligibilityCheckDREViewModel(IMvxMessenger messenger, IClaimService claimservice, IEligibilityService eligibilityservice)
        {
            _messenger = messenger;
            _claimservice = claimservice;
            _eligibilityservice = eligibilityservice;

            EligibilityCheckResults = _eligibilityservice.EligibilityCheckResults;
            SelectedParticipants = new ObservableCollection<ParticipantEligibilityResult>();

            _eligibilitycheckcomplete = _messenger.Subscribe<EligibilityCheckSubmissionComplete>((message) =>
            {
                EligibilityCheckResults = _eligibilityservice.EligibilityCheckResults;
                SelectedParticipants = new ObservableCollection<ParticipantEligibilityResult>();
            });

            _shouldcloseself = _messenger.Subscribe<ClearEligibilityCheckDRERequest>((message) =>
            {
                _messenger.Unsubscribe<ClearEligibilityCheckDRERequest>(_shouldcloseself);
                Close(this);
            });

            if (_claimservice.PhoneTextAlterations == null)
            {
                PhoneBusy = true;

                _claimservice.GetPhoneNumber(_loginservice.CurrentPlanMemberID);
            }
            else
            {
                PhoneNumber = _claimservice.PhoneText;
                PhoneBusy = false;
            }
            _gettextalterationphone = _messenger.Subscribe<GetTextAlterationPhoneComplete>((message) =>
            {
                PhoneNumber = _claimservice.PhoneText;
                PhoneBusy = false;
            });

            _gettextalterationphoneerror = _messenger.Subscribe<GetTextAlterationPhoneError>((message) =>
            {
                RaiseError(new EventArgs());
                PhoneBusy = false;
            });

            _notextalterationphone = _messenger.Subscribe<NoTextAlterationPhoneFound>((message) =>
            {
                NoPhoneAlteration = true;
                PhoneBusy = false;
            });
        }

        public event EventHandler OnError;
        protected virtual void RaiseError(EventArgs e)
        {
            if (this.OnError != null)
            {
                OnError(this, e);
            }
        }

        private bool _noPhoneAlteration = false;
        public bool NoPhoneAlteration
        {
            get => _noPhoneAlteration;
            set
            {
                _noPhoneAlteration = value;
                RaisePropertyChanged(() => NoPhoneAlteration);
            }
        }
        private TextAlteration _phoneNumber;
        public TextAlteration PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                RaisePropertyChanged(() => PhoneNumber);
            }
        }

        private bool _phoneBusy = false;
        public bool PhoneBusy
        {
            get => _phoneBusy;
            set
            {
                if (_phoneBusy != value)
                {
                    _phoneBusy = value;
                    RaisePropertyChanged(() => PhoneBusy);
                }
            }
        }
        public EligibilityCheckType EligibilityCheckType => _eligibilityservice.SelectedEligibilityCheckType;

        private EligibilityCheck _eligibilityCheckResults;
        public EligibilityCheck EligibilityCheckResults
        {
            get => _eligibilityCheckResults;
            set
            {
                _eligibilityCheckResults = value;
                SetContactCustomerServiceVisibility();
                RaisePropertyChanged(() => EligibilityCheckResults);
            }
        }

        private ObservableCollection<ParticipantEligibilityResult> _selectedParticipants;
        public ObservableCollection<ParticipantEligibilityResult> SelectedParticipants
        {
            get => _selectedParticipants;
            set
            {
                _selectedParticipants = value;
                RaisePropertyChanged(() => SelectedParticipants);
            }
        }

        private bool _isContactCustomerServiceVisible;
        public bool IsContactCustomerServiceVisible
        {
            get => _isContactCustomerServiceVisible;
            set
            {
                _isContactCustomerServiceVisible = value;
                RaisePropertyChanged(() => IsContactCustomerServiceVisible);
            }
        }

        private string _changeFPPMNumberResults;
        public string ChangeFPPMNumberResults
        {
            get => _changeFPPMNumberResults;
            set
            {
                _changeFPPMNumberResults = value;
                RaisePropertyChanged(() => ChangeFPPMNumberResults);
            }
        }

        private string _changeFPPMNumberBenefitLabel;
        public string ChangeFPPMNumberBenefitLabel
        {
            get => _changeFPPMNumberBenefitLabel;
            set
            {
                _changeFPPMNumberBenefitLabel = value;
                RaisePropertyChanged(() => ChangeFPPMNumberBenefitLabel);
            }
        }

        public ICommand SelectParticipantCommand
        {
            get
            {
                return new MvxCommand<ParticipantEligibilityResult>((selectedParticipant) =>
                {
                    if (SelectedParticipants.Contains(selectedParticipant))
                        SelectedParticipants.Remove(selectedParticipant);
                    else
                        SelectedParticipants.Add(selectedParticipant);
                });
            }
        }

        public ICommand NavigateToBenefitInquiryCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _eligibilityservice.SelectedParticipantsForBenefitInquiry = SelectedParticipants.ToList<ParticipantEligibilityResult>();
                    Unsubscribe();

                    _messenger.Publish<Messages.ClearEligibilityBenefitInquiryRequest>(new MobileClaims.Core.Messages.ClearEligibilityBenefitInquiryRequest(this));   
                    this.ShowViewModel<EligibilityBenefitInquiryViewModel>();
                });
            }
        }

        public ICommand SubmitAnotherEligibilityCheckCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Unsubscribe();
                    CloseViews();

                    this.ShowViewModel<EligibilityCheckTypesViewModel>();
                });
            }
        }
        private void Unsubscribe()
        {
            _messenger.Unsubscribe<EligibilityCheckSubmissionComplete>(_eligibilitycheckcomplete);
            _messenger.Unsubscribe<GetTextAlterationPhoneComplete>(_gettextalterationphone);
            _messenger.Unsubscribe<GetTextAlterationPhoneError>(_gettextalterationphoneerror);
            _messenger.Unsubscribe<NoTextAlterationPhoneFound>(_notextalterationphone);
        }

        private void SetContactCustomerServiceVisibility()
        {
            if (EligibilityCheckResults == null)
                IsContactCustomerServiceVisible = false;
            else if (EligibilityCheckResults.Result == null)
                IsContactCustomerServiceVisible = false;
            else if (EligibilityCheckResults.Result.ParticipantEligibilityResults == null || EligibilityCheckResults.Result.ParticipantEligibilityResults.Count == 0)
                IsContactCustomerServiceVisible = false;
            else
            {
                bool vis = false;
                foreach (ParticipantEligibilityResult per in EligibilityCheckResults.Result.ParticipantEligibilityResults)
                {
                    DateTime rreed, rsed, rped;
                    DateTime.TryParse(per.RawRecallExamEligibilityDate, out rreed);
                    DateTime.TryParse(per.RawScalingEligibilityDate, out rsed);
                    DateTime.TryParse(per.RawPolishingEligibilityDate, out rped);
                    if (string.IsNullOrEmpty(per.RawRecallExamEligibilityDate) || rreed == DateTime.MinValue ||
                        string.IsNullOrEmpty(per.RawScalingEligibilityDate) || rsed == DateTime.MinValue ||
                        string.IsNullOrEmpty(per.RawPolishingEligibilityDate) || rped == DateTime.MinValue)
                    {
                        vis = true;
                        break;
                    }
                }
                IsContactCustomerServiceVisible = vis;
            }
        }

        private void CloseViews()
        {
            _messenger.Publish<ClearEligibilityCheckTypesRequested>(new ClearEligibilityCheckTypesRequested(this));
            _messenger.Publish<Messages.ClearEligibilityParticipantsRequest>(new MobileClaims.Core.Messages.ClearEligibilityParticipantsRequest(this));
            _messenger.Publish<Messages.ClearEligibilityCheckDRERequest>(new MobileClaims.Core.Messages.ClearEligibilityCheckDRERequest(this));
            _messenger.Publish<Messages.ClearEligibilityCheckCFORequest>(new MobileClaims.Core.Messages.ClearEligibilityCheckCFORequest(this));
            _messenger.Publish<Messages.ClearEligibilityCheckCPRequest>(new MobileClaims.Core.Messages.ClearEligibilityCheckCPRequest(this));
            _messenger.Publish<Messages.ClearEligibilityCheckEyeRequest>(new MobileClaims.Core.Messages.ClearEligibilityCheckEyeRequest(this));
            _messenger.Publish<Messages.ClearEligibilityCheckMassageRequest>(new MobileClaims.Core.Messages.ClearEligibilityCheckMassageRequest(this));
            _messenger.Publish<Messages.ClearEligibilityResultsViewRequested>(new ClearEligibilityResultsViewRequested(this));
            _messenger.Publish<Messages.ClearEligibilityBenefitInquiryRequest>(new MobileClaims.Core.Messages.ClearEligibilityBenefitInquiryRequest(this));
        }

        private IMvxCommand _backBtnClickCommandDroid;
        public new IMvxCommand BackBtnClickCommandDroid
        {
            get
            {
                return _backBtnClickCommandDroid ?? (_backBtnClickCommandDroid = new MvxAsyncCommand(
                    async () =>
                    {
                        Close(this);
                    }));
            }
        }
    }
}