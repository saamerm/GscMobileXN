using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class EligibilityCheckTypesViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IParticipantService _participantservice;
        private readonly IEligibilityService _eligibilityservice;
        private readonly MvxSubscriptionToken _participantretrieved;
        private readonly MvxSubscriptionToken _checktypesretrieved;
        private MvxSubscriptionToken _eligibilitycheckcomplete;
        private MvxSubscriptionToken _eligibilityCheckFailure;
        private MvxSubscriptionToken _noeligibilitychecktypesfound;
        private MvxSubscriptionToken _shouldcloseself;//added by vivian on July 7 2014

        public string MyBenefitsTitle => Resource.MyBenefits;

        public EligibilityCheckTypesViewModel(IMvxMessenger messenger, IParticipantService participantservice, IEligibilityService eligibilityservice, ILoginService loginservice)
        {
            _messenger = messenger;
            _participantservice = participantservice;
            _eligibilityservice = eligibilityservice;
            _loginservice = loginservice;

            _checktypesretrieved = _messenger.Subscribe<GetEligibilityCheckTypesComplete>(message =>
            {
                Busy = false;
                EligibilityCheckTypes = _eligibilityservice.EligibilityCheckTypes;
            });

            _noeligibilitychecktypesfound = _messenger.Subscribe<NoEligibilityCheckTypesFound>(message =>
            {
                Busy = false;
                NoAccessToEligibilityChecks = true;
            });

            _participantretrieved = _messenger.Subscribe<RetrievedPlanMemberMessage>(message =>
            {
                Busy = true;
                _eligibilityservice.GetEligibilityCheckTypes(_participantservice.PlanMember.PlanMemberID);
            });

            if (_participantservice.PlanMember != null && !string.IsNullOrEmpty(_participantservice.PlanMember.PlanMemberID))
            {
				Busy = true;
                _eligibilityservice.GetEligibilityCheckTypes(_participantservice.PlanMember.PlanMemberID);
            }

            //added by vivian on July 7 2014
            _shouldcloseself = _messenger.Subscribe<ClearEligibilityCheckTypesRequested>(message =>
            {
                _messenger.Unsubscribe<GetEligibilityCheckTypesComplete>(_checktypesretrieved);
                _messenger.Unsubscribe<RetrievedPlanMemberMessage>(_participantretrieved);
                _messenger.Unsubscribe<NoEligibilityCheckTypesFound>(_noeligibilitychecktypesfound);
                _messenger.Unsubscribe<ClearEligibilityCheckTypesRequested>(_shouldcloseself);
                Close(this);
            });
            //ended of adding by vivian on July 7 2014


        }

        private EligibilityCheckType _selectedEligibilityCheckType;
        public EligibilityCheckType SelectedEligibilityCheckType
        {
            get => _selectedEligibilityCheckType;
            set
            {
                _selectedEligibilityCheckType = value;
                _eligibilityservice.SelectedEligibilityCheckType = _selectedEligibilityCheckType;
                RaisePropertyChanged(() => SelectedEligibilityCheckType);
            }
        }

        private List<EligibilityCheckType> _eligibilityCheckTypes;
        public List<EligibilityCheckType> EligibilityCheckTypes
        {
            get => _eligibilityCheckTypes;
            set
            {
                _eligibilityCheckTypes = value;
                RaisePropertyChanged(() => EligibilityCheckTypes);
            }
        }

        private bool _busy;
        public bool Busy
        {
            get => _busy;
            set
            {
                if (_busy != value)
                {
                    _busy = value;
                    _messenger.Publish(new BusyIndicator(this)
                    {
                        Busy = _busy
                    });
                    RaisePropertyChanged(() => Busy);
                }
            }
        }

        private bool _noAccessToEligibilityChecks;
        public bool NoAccessToEligibilityChecks
        {
            get => _noAccessToEligibilityChecks;
            set
            {
                _noAccessToEligibilityChecks = value;
                RaisePropertyChanged(() => NoAccessToEligibilityChecks);
            }
        }

        public ICommand EligibilityCheckTypeSelectedCommand
        {
            get
            {
                return new MvxCommand<EligibilityCheckType>(selectedEligibilityCheckType =>
                {
                    SelectedEligibilityCheckType = selectedEligibilityCheckType;

                    _eligibilityservice.EligibilitySelectedParticipant = null; // reset to null to avoid confusion with old checks
                    _eligibilityservice.EligibilityCheck = new EligibilityCheck();
                    _eligibilityservice.EligibilityCheck.EligibilityCheckType = _eligibilityservice.SelectedEligibilityCheckType;
                    _eligibilityservice.EligibilityCheck.EligibilityCheckTypeID = _eligibilityservice.SelectedEligibilityCheckType?.ID;
                    string[] planMemberIDAndParticipantNumber = _loginservice.CurrentPlanMemberID.Split('-');
                    _eligibilityservice.EligibilityCheck.PlanMemberID = long.Parse(planMemberIDAndParticipantNumber[0]);
                    _eligibilityservice.EligibilityCheck.ParticipantNumber = planMemberIDAndParticipantNumber[1];

                    ShowAppropriateViewModel();
                });
            }
        }

        public ICommand NavigateToViewModelCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowAppropriateViewModel();
                });
            }
        }

        private void ShowAppropriateViewModel()
        {
            switch(SelectedEligibilityCheckType?.ID)
            {
                case "RECALLEXAM":
                    {
                        Busy = true;
                        _eligibilitycheckcomplete = _messenger.Subscribe<EligibilityCheckSubmissionComplete>(message2 =>
                        {
                            Busy = false;
                            _messenger.Unsubscribe<EligibilityCheckSubmissionComplete>(_eligibilitycheckcomplete);
                            _messenger.Unsubscribe<EligibilityCheckSubmissionError>(_eligibilityCheckFailure);

                            CloseViews();

                            ShowViewModel<EligibilityCheckDREViewModel>();
                        });
                        _eligibilityCheckFailure = _messenger.Subscribe<EligibilityCheckSubmissionError>(message =>
                        {
                            Busy = false;
                            _messenger.Unsubscribe<EligibilityCheckSubmissionComplete>(_eligibilitycheckcomplete);
                            _messenger.Unsubscribe<EligibilityCheckSubmissionError>(_eligibilityCheckFailure);
                            Dialogs.Alert(Resource.GenericErrorDialogMessage);
                        });
                        _eligibilityservice.CheckEligibility();
                        break;
                    }
                case "ORTHOTICS":
                    {
                        Busy = true;
                        _eligibilitycheckcomplete = _messenger.Subscribe<EligibilityCheckSubmissionComplete>(message2 =>
                        {
                            Busy = false;
                            _messenger.Unsubscribe<EligibilityCheckSubmissionComplete>(_eligibilitycheckcomplete);
                            _messenger.Unsubscribe<EligibilityCheckSubmissionError>(_eligibilityCheckFailure);

                            CloseViews();

                            ShowViewModel<EligibilityCheckCFOViewModel>();
                        });
                        _eligibilityCheckFailure = _messenger.Subscribe<EligibilityCheckSubmissionError>(message =>
                        {
                            Busy = false;
                            _messenger.Unsubscribe<EligibilityCheckSubmissionComplete>(_eligibilitycheckcomplete);
                            _messenger.Unsubscribe<EligibilityCheckSubmissionError>(_eligibilityCheckFailure);
                            Dialogs.Alert(Resource.GenericErrorDialogMessage);
                        });

                        _eligibilityservice.CheckEligibility();
                        break;
                    }
                case "CHIRO":
                case "PHYSIO":
                case "MASSAGE":
                case "GLASSES":
                case "CONTACTS":
                case "EYEEXAM":
                    CloseViews();
                    ShowViewModel<EligibilityParticipantsViewModel>();
                    break;
            }
        }

        private void CloseViews() //added by vivian on June 16 2014
        {
            _messenger.Publish(new ClearEligibilityCheckDRERequest(this));
            _messenger.Publish(new ClearEligibilityCheckCFORequest(this));
            _messenger.Publish(new ClearEligibilityParticipantsRequest(this));
            _messenger.Publish(new ClearEligibilityBenefitInquiryRequest(this));   
            _messenger.Publish(new ClearEligibilityCheckCPRequest(this));
            _messenger.Publish(new ClearEligibilityCheckEyeRequest(this));
            _messenger.Publish(new ClearEligibilityCheckMassageRequest(this));
            _messenger.Publish(new ClearEligibilityResultsViewRequested(this));
          //    _messenger.Publish<ClearEligibilityCheckTypesRequested >(new ClearEligibilityCheckTypesRequested(this));
        }
    }
}