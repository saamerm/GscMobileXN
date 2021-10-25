using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MobileClaims.Core.Extensions;

namespace MobileClaims.Core.ViewModels
{
    public class EligibilityResultsViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimService _claimservice;
        private readonly IEligibilityService _eligibilityservice;
        private readonly IParticipantService _participantsvc;
        private readonly MvxSubscriptionToken _eligibilitycheckcomplete;
        private MvxSubscriptionToken _gettextalterationphone;
        private MvxSubscriptionToken _gettextalterationphoneerror;
        private MvxSubscriptionToken _notextalterationphone;
        private bool _isVisionEnhancementApplicable;
        public string EligibilityResultsPCAndPGText => Resource.EligibilityResultsPCAndPGText;

        public bool IsVisionEnhancementApplicable
        {
            get => _isVisionEnhancementApplicable;
            set => SetProperty(ref _isVisionEnhancementApplicable, value);
        }

        private MvxSubscriptionToken _eligibilityResultCloseToken;//added by vivian on Junly 3 2014 

        public EligibilityResultsViewModel(IMvxMessenger messenger, IClaimService claimservice, IEligibilityService eligibilityservice, IParticipantService participantService)
        {
            _messenger = messenger;
            _claimservice = claimservice;
            _eligibilityservice = eligibilityservice;
            _participantsvc = participantService;

#if CCQ || FPPM
            var isMinorFromQuebecProvice = _participantsvc.SelectedParticipant.IsOrUnderAgeOf18()
              && _participantsvc.SelectedParticipant.IsResidentOfQuebecProvince();
            if (isMinorFromQuebecProvice &&
                _eligibilityservice.SelectedEligibilityCheckType != null &&
                _eligibilityservice.SelectedEligibilityCheckType.ID != null &&
                _eligibilityservice.SelectedEligibilityCheckType.ID.IsVisionEnhancement())
            {
                IsVisionEnhancementApplicable = true;
            }
            else
            {
                IsVisionEnhancementApplicable = false;
            }
#else
            IsVisionEnhancementApplicable = false;
#endif

            PopulateEligibilityCheckResults();

            _eligibilitycheckcomplete = _messenger.Subscribe<EligibilityCheckSubmissionComplete>(message =>
            {
                PopulateEligibilityCheckResults();
            });

            _eligibilityResultCloseToken = _messenger.Subscribe<ClearEligibilityResultsViewRequested>(messege =>
              {
                  _messenger.Unsubscribe<ClearEligibilityResultsViewRequested>(_eligibilityResultCloseToken);
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
            _gettextalterationphone = _messenger.Subscribe<GetTextAlterationPhoneComplete>(message =>
            {
                PhoneNumber = _claimservice.PhoneText;
                PhoneBusy = false;
            });

            _gettextalterationphoneerror = _messenger.Subscribe<GetTextAlterationPhoneError>(message =>
            {
                RaiseError(new EventArgs());
                PhoneBusy = false;
            });

            _notextalterationphone = _messenger.Subscribe<NoTextAlterationPhoneFound>(message =>
            {
                NoPhoneAlteration = true;
                PhoneBusy = false;
            });
        }

        public event EventHandler OnError;
        protected virtual void RaiseError(EventArgs e)
        {
            if (OnError != null)
            {
                OnError(this, e);
            }
        }

        private bool _noPhoneAlteration;
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

        private bool _phoneBusy;
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
                RaisePropertyChanged(() => EligibilityCheckResults);
            }
        }

        private bool _showPlanLimitations;
        public bool ShowPlanLimitations
        {
            get => _showPlanLimitations;
            set
            {
                _showPlanLimitations = value;
                RaisePropertyChanged(() => ShowPlanLimitations);
            }
        }

        public string _eligibilityResultNotes;
        public string EligibilityResultNotes
        {
            get => _eligibilityResultNotes;
            set
            {
                _eligibilityResultNotes = value;
                RaisePropertyChanged(() => EligibilityResultNotes);
            }
        }

        public ICommand SubmitAnotherEligibilityCheckCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Unsubscribe();
                    CloseViews(); //added by vivian on July 3 2014
                    //Close(this);
                    ShowViewModel<EligibilityCheckTypesViewModel>();
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

        private void PopulateEligibilityCheckResults()
        {
            EligibilityCheckResults = _eligibilityservice.EligibilityCheckResults;

            if (EligibilityCheckResults != null && EligibilityCheckResults.Result != null
                && EligibilityCheckResults.Result.PlanLimitations != null && EligibilityCheckResults.Result.PlanLimitations.Count > 0)
            {
                ShowPlanLimitations = true;
            }
        }

        private void CloseViews() //added by vivian on July 3 2014
        {
            _messenger.Publish(new ClearEligibilityCheckTypesRequested(this));

            _messenger.Publish(new ClearEligibilityParticipantsRequest(this));

            _messenger.Publish(new ClearEligibilityCheckDRERequest(this));
            _messenger.Publish(new ClearEligibilityCheckCFORequest(this));

            _messenger.Publish(new ClearEligibilityCheckCPRequest(this));
            _messenger.Publish(new ClearEligibilityCheckEyeRequest(this));
            _messenger.Publish(new ClearEligibilityCheckMassageRequest(this));

            _messenger.Publish(new ClearEligibilityResultsViewRequested(this));
            _messenger.Publish(new ClearEligibilityBenefitInquiryRequest(this));
        }
    }
}