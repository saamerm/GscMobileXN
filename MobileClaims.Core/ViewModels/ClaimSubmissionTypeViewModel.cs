using System;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.HCSA;
using MobileClaims.Core.ViewModels.HCSA;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimSubmissionTypeViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        public readonly IClaimService _claimservice;
        private readonly IParticipantService _participantservice;
        private readonly IRehydrationService _rehydrationSvc;
        private readonly IProviderLookupService _providerservice;
        private readonly IHCSAClaimService _hcsaservice;
        private readonly IDeviceService _deviceService;
        private readonly MvxSubscriptionToken _claimtypesretrieved;
        private readonly MvxSubscriptionToken _participantretrieved;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private readonly MvxSubscriptionToken _noclaimtypesfound;
        private bool webViewShown;

        public ClaimSubmissionTypeViewModel(IMvxMessenger messenger,
                                            IClaimService claimservice,
                                            IParticipantService participantservice,
                                            IRehydrationService rehydrationsvc,
                                            IProviderLookupService providerservice,
                                            IHCSAClaimService hcsaservice, IDeviceService deviceService)
        {
            _claimservice = claimservice;
            _messenger = messenger;
            _participantservice = participantservice;
            _rehydrationSvc = rehydrationsvc;
            _providerservice = providerservice;
            _hcsaservice = hcsaservice;
            _deviceService = deviceService;

            if (_claimservice.SelectedClaimSubmissionType != null)
            {
                if (_claimservice.SelectedClaimSubmissionType.ID != "HC")
                {
                    _hcsaservice.HaveClaimDetailsAlreadyBeenInitialized = false;
                }
                SelectedClaimSubmissionType = _claimservice.SelectedClaimSubmissionType;
            }
            if (_claimservice.ClaimSubmissionTypes != null && _claimservice.ClaimSubmissionTypes.Count() > 0)
            {
                ClaimSubmissionTypes = _claimservice.ClaimSubmissionTypes;
            }

            _claimtypesretrieved = _messenger.Subscribe<GetClaimSubmissionTypesComplete>(message =>
            {
                Busy = false;
                ClaimSubmissionTypes = _claimservice.ClaimSubmissionTypes;
            });

            _participantretrieved = _messenger.Subscribe<RetrievedPlanMemberMessage>(message =>
            {
                Busy = true;
                VisionRequiresPrescription = _participantservice.PlanMember.PlanConditions.VisionRequiresPrescription;
                _claimservice.GetClaimSubmissionTypes(_participantservice.PlanMember.PlanMemberID);
            });

            _noclaimtypesfound = _messenger.Subscribe<NoClaimSubmissionTypesFound>(message =>
            {
                Busy = false;
                claimservice.ClearClaimDetails();
                rehydrationsvc.ClearData();
                NoAccessToOnlineClaimSubmission = true;
            });

            if (_participantservice.PlanMember != null && !string.IsNullOrEmpty(_participantservice.PlanMember.PlanMemberID) && (_claimservice.ClaimSubmissionTypes == null || _claimservice.ClaimSubmissionTypes.Count() == 0))
            {
                Busy = true;
                VisionRequiresPrescription = _participantservice.PlanMember.PlanConditions.VisionRequiresPrescription;
                _claimservice.GetClaimSubmissionTypes(_participantservice.PlanMember.PlanMemberID);
            }

            _shouldcloseself = _messenger.Subscribe<ClearClaimSubmissionTypeViewRequested>(message =>
            {
                _messenger.Unsubscribe<ClearClaimSubmissionTypeViewRequested>(_shouldcloseself);
                Close(this);
            });
            RaiseAllPropertiesChanged();
        }

        public EventHandler FragmentActivityBuilt;

        public override async Task Initialize()
        {
            await base.Initialize();
            if (_rehydrationSvc.Rehydrating && _deviceService.CurrentDevice == GSCHelper.OS.Droid)
            {
                Busy = true;
                FragmentActivityBuilt += OnFragmentActivityBuilt;
            }
        }

        private async void OnFragmentActivityBuilt(object sender, EventArgs e)
        {
            // for better user experience
            await Task.Delay(500);
            Busy = false;
            FragmentActivityBuilt -= OnFragmentActivityBuilt;
        }

        private ClaimSubmissionType _selectedClaimSubmissionType;
        public ClaimSubmissionType SelectedClaimSubmissionType
        {
            get => _selectedClaimSubmissionType;
            set => SetProperty(ref _selectedClaimSubmissionType, value);
        }

        private List<ClaimSubmissionType> _claimSubmissionTypes;
        public List<ClaimSubmissionType> ClaimSubmissionTypes
        {
            get
            {
                return _claimSubmissionTypes != null ? _claimSubmissionTypes.OrderBy(cst => cst.Name).ToList() : _claimSubmissionTypes;
            }
            set
            {
                _claimSubmissionTypes = value;
                RaisePropertyChanged(() => ClaimSubmissionTypes);
            }
        }

        private bool _noAccessToOnlineClaimSubmission;
        public bool NoAccessToOnlineClaimSubmission
        {
            get => _noAccessToOnlineClaimSubmission;
            set
            {
                _noAccessToOnlineClaimSubmission = value;
                RaisePropertyChanged(() => NoAccessToOnlineClaimSubmission);
            }
        }

        private bool _visionRequiresPrescription;
        public bool VisionRequiresPrescription
        {
            get => _visionRequiresPrescription;
            set
            {
                _visionRequiresPrescription = value;
                RaisePropertyChanged(() => VisionRequiresPrescription);
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

        private void PublishMessages()
        {
            _messenger.Publish(new ClearClaimTreatmentDetailsViewRequested(this));
            _messenger.Publish(new ClearClaimTermsAndConditionsViewRequested(this));
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
            _messenger.Publish(new ClearClaimTypeViewRequested(this));

        }

        public ICommand ClaimSubmissionTypeSelectedCommand
        {
            get
            {
                return new MvxCommand<ClaimSubmissionType>(selectedClaimSubmissionType =>
                {
                    Unsubscribe();
                    PublishMessages();
                    if (!_rehydrationSvc.Rehydrating)
                    {
                        _claimservice.ClearOldClaimData();
                        _providerservice.ClearOldClaimData();
                        _participantservice.ClearOldClaimData();
                        _rehydrationSvc.ClearFromStartingPoint(GetType());

                        _claimservice.Claim = new Claim();
                    }
                    ShowAppropriateViewModel(selectedClaimSubmissionType);
                });
            }
        }

        private void ShowAppropriateViewModel(ClaimSubmissionType selectedClaimSubmissionType)
        {
            if (selectedClaimSubmissionType.ID == "DRUG")
            {
                _claimservice.Claim.Type = selectedClaimSubmissionType;
                _claimservice.SelectedClaimSubmissionType = selectedClaimSubmissionType;
                _claimservice.Claim.ClaimSubmissionTypes = ClaimSubmissionTypes;
                _claimservice.PersistClaim();
                SelectedClaimSubmissionType = selectedClaimSubmissionType;
                _claimservice.IsHCSAClaim = false;
                ShowViewModel<ClaimParticipantsViewModel>();
            }
            else if (selectedClaimSubmissionType.ID == "HC")
            {
                _hcsaservice.Claim = new Entities.HCSA.Claim();
                _hcsaservice.SelectedClaimType = null;
                _hcsaservice.SelectedExpenseType = null;
                _hcsaservice.PersistClaim();
                _hcsaservice.HaveClaimDetailsAlreadyBeenInitialized = false;
                _claimservice.IsHCSAClaim = true;
                _claimservice.SelectedClaimSubmissionType = selectedClaimSubmissionType;
                _claimservice.PersistClaim();
                ShowViewModel<ClaimTypeViewModel>();
            }
            else
            {
                _claimservice.Claim.Type = selectedClaimSubmissionType;
                _claimservice.SelectedClaimSubmissionType = selectedClaimSubmissionType;
                _claimservice.Claim.ClaimSubmissionTypes = ClaimSubmissionTypes;
                _claimservice.PersistClaim();
                _claimservice.IsHCSAClaim = false;
                ShowViewModel<ClaimServiceProvidersViewModel>();
            }
        }

        public ICommand ShowChooseClaimOrHistoryCommand
        {
            get
            {
                return new MvxCommand(() =>
                    {
                        Unsubscribe();
                        ShowViewModel<ChooseClaimOrHistoryViewModel>();
                    });
            }
        }

        public ICommand NavigateToViewModelCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Unsubscribe();
                    PublishMessages();
                    ShowViewModel<ClaimServiceProvidersViewModel>();
                });
            }
        }

        private void Unsubscribe()
        {
            _messenger.Unsubscribe<GetClaimSubmissionTypesComplete>(_claimtypesretrieved);
            _messenger.Unsubscribe<RetrievedPlanMemberMessage>(_participantretrieved);
            _messenger.Unsubscribe<NoClaimSubmissionTypesFound>(_noclaimtypesfound);
        }
    }
}