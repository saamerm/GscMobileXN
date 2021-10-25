using System;
using System.Collections.Generic;
using System.Linq;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.HCSA;
using MobileClaims.Core.ViewModels.HCSA;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimParticipantsViewModel : ViewModelBase
    {
        private readonly IParticipantService _participantService;
        private readonly IMvxMessenger _messengerService;
        private readonly IClaimService _claimService;
        private readonly IHCSAClaimService _hcsaClaimService;
        private readonly IRehydrationService _rehydrationService;

        private readonly MvxSubscriptionToken _participantSelected;
        private readonly MvxSubscriptionToken _shouldCloseSelf;

        private bool processingParticipant;
        private bool HaveShownClaimDetailsHCSAVM = false;
        private bool _ishcsaclaim;
        private bool _isVisionEnhancementApplicable;

        private Participant _selectedParticipant;
        private Participant _requestedParticipant;

        private ParticipantsViewModel _participantsViewModel;

        public event EventHandler ChangeParticipantRequest;
        public string Title => Resource.ClaimPlanParticipants;
        public string Important => Resource.Important;
        public string VisionEnhancementsMessage => BrandResource.VisionEnhancementsMessage;
        public string ProgramName => BrandResource.VisionEnhancementsProgramName;
        public string ProgramAdditionalInfo => BrandResource.VisionEnhancementsProgramAdditionalInfo;

        public bool IsHCSAClaim
        {
            get => _ishcsaclaim;
            set => SetProperty(ref _ishcsaclaim, value);
        }

        public bool IsVisionEnhancementApplicable
        {
            get => _isVisionEnhancementApplicable;
            set => SetProperty(ref _isVisionEnhancementApplicable, value);
        }

        public ParticipantsViewModel ParticipantsViewModel
        {
            get => _participantsViewModel;
            set => SetProperty(ref _participantsViewModel, value);
        }

        public ServiceProvider ServiceProvider => _claimService.Claim != null ? _claimService.Claim.Provider : null;

        public ClaimSubmissionType ClaimSubmissionType
        {
            get
            {
                if (!_claimService.IsHCSAClaim)
                {
                    return _claimService.SelectedClaimSubmissionType;
                }
                else
                {
                    var qry = from ClaimSubmissionType cst in _claimService.ClaimSubmissionTypes
                              where cst.ID == "HC"
                              select cst;
                    return qry.FirstOrDefault();
                }
            }
        }

        public Participant SelectedParticipant
        {
            get => _selectedParticipant;
            set
            {
                if (!processingParticipant)
                {
                    processingParticipant = true;
                    if (_claimService.Claim != null || _claimService.IsHCSAClaim)
                    {
                        _selectedParticipant = value;
                        if (!_claimService.IsHCSAClaim)
                        {
                            _claimService.Claim.Participant = value;
                        }
                        else
                        {
                            _hcsaClaimService.Claim.ParticipantNumber = value.PlanMemberID;
                            _hcsaClaimService.PersistClaim();
                        }
                        ParticipantsViewModel.SelectedParticipant = _selectedParticipant;
                        _messengerService.Publish(new ClaimParticipantSelected(this, value));
                        RaisePropertyChanged(() => SelectedParticipant);

                        if (_claimService.IsHCSAClaim && value != null)
                        {
                            _hcsaClaimService.Claim.ParticipantNumber = value.PlanMemberID;
                        }
                        processingParticipant = false;
                    }
                }
            }
        }

        public Participant RequestedParticipant
        {
            get => _requestedParticipant;
            set
            {
                SetProperty(ref _requestedParticipant, value);
                RaisePropertyChanged(() => SelectedParticipant);
            }
        }

        public MvxCommand<Participant> RequestChangeParticipantCommand { get; }
        public MvxCommand<Participant> ChangeParticipantCommand { get; }
        public MvxCommand<Participant> ChangeParticipantWithoutNavigationCommand { get; }
        public IMvxCommand NavigateToClaimDetailsCommand { get; }

        public ClaimParticipantsViewModel(IParticipantService participantservice,
            IMvxMessenger messenger,
            IClaimService claimservice,
            IHCSAClaimService hcsaClaimService,
            IRehydrationService rehydrationService)
        {
            _participantService = participantservice;
            _messengerService = messenger;
            _claimService = claimservice;
            _hcsaClaimService = hcsaClaimService;
            _rehydrationService = rehydrationService;

            RequestChangeParticipantCommand = new MvxCommand<Participant>(ExecuteRequestChangeParticipantCommand);
            ChangeParticipantCommand = new MvxCommand<Participant>(ExecuteChangeParticipantCommand);
            ChangeParticipantWithoutNavigationCommand = new MvxCommand<Participant>(ExecuteChangeParticipantWithoutNavigationCommand);
            NavigateToClaimDetailsCommand = new MvxCommand(ExecuteNavigationToClaimDetailsCommand);

            // why do we have the following code? i'm so confused.
            ParticipantsViewModel = new ParticipantsViewModel(participantservice, messenger, ClaimSubmissionType, false);

#if CCQ || FPPM
            if (ParticipantsViewModel.OtherParticipants.Any() &&
                (string.Equals(ClaimSubmissionType.ID, "GLASSES", StringComparison.OrdinalIgnoreCase)
                || string.Equals(ClaimSubmissionType.ID, "CONTACTS", StringComparison.OrdinalIgnoreCase)))
            {
                IsVisionEnhancementApplicable = true;
            }
#else
            IsVisionEnhancementApplicable = false;
#endif

            _participantSelected = _messengerService.Subscribe<ParticipantSelected>((message) =>
            {
                if (_rehydrationService.Rehydrating)
                {
                    _selectedParticipant = _participantService.SelectedParticipant;
                    var deviceservice = Mvx.IoCProvider.Resolve<IDeviceService>();
                    if (deviceservice.CurrentDevice != GSCHelper.OS.Windows
                        && deviceservice.CurrentDevice != GSCHelper.OS.WP)
                    {
                        RaisePropertyChanged(() => SelectedParticipant);
                    }
                }
                else
                {
                    SelectedParticipant = _participantService.SelectedParticipant;
                }
            });

            _shouldCloseSelf = _messengerService.Subscribe<ClearClaimParticipantsViewRequested>((message) =>
            {
                _messengerService.Unsubscribe<ClearClaimParticipantsViewRequested>(_shouldCloseSelf);
                _messengerService.Unsubscribe<ParticipantSelected>(_participantSelected);
                Close(this);
            });

            if (_claimService.Claim != null && _claimService.Claim.Participant != null)
            {
                if (_rehydrationService.Rehydrating)
                {
                    _selectedParticipant = _claimService.Claim != null ? _claimService.Claim.Participant : null;
                }
                else
                {
                    _selectedParticipant = _claimService.Claim != null ? _claimService.Claim.Participant : null;
                }
            }

            IsHCSAClaim = _claimService.IsHCSAClaim;
            if (IsHCSAClaim)
            {
                if (_rehydrationService.Rehydrating)
                {
                    foreach (Participant p in ParticipantsViewModel.Participants)
                    {
                        try
                        {
                            var test = _hcsaClaimService.Claim.ParticipantNumber.Contains("-") ? _hcsaClaimService.Claim.ParticipantNumber : string.Format("{0}-{1}", _hcsaClaimService.Claim.PlanMemberID.ToString(), _hcsaClaimService.Claim.ParticipantNumber);
                            if (p.PlanMemberID == test)
                            {
                                _selectedParticipant = p;
                                processingParticipant = true;
                                RaisePropertyChanged(() => SelectedParticipant);
                                processingParticipant = false;
                            }
                        }
                        catch
                        {
                            processingParticipant = false;
                        }
                    }
                    //SelectedParticipant = ParticipantsViewModel.Participants.Where(p => p.PlanMemberID == string.Format("{0}-{1}",_hcsaservice.Claim.PlanMemberID.ToString(),_hcsaservice.Claim.ParticipantNumber)).FirstOrDefault();
                    ParticipantsViewModel.SelectedParticipant = _selectedParticipant;

                }
                else
                {
                    _selectedParticipant = ParticipantsViewModel.Participants.Where(p => p.PlanMemberID == _hcsaClaimService.Claim.ParticipantNumber).FirstOrDefault();
                    ParticipantsViewModel.SelectedParticipant = _selectedParticipant;
                }
            }
        }

        protected virtual void OnChangeParticipantRequest(EventArgs e)
        {
            ChangeParticipantRequest?.Invoke(this, e);
        }

        private void ExecuteRequestChangeParticipantCommand(Participant participant)
        {
            RequestedParticipant = participant;
            _rehydrationService.Rehydrating = false;
            HaveShownClaimDetailsHCSAVM = false;

            if (SelectedParticipant == null)
            {
                ChangeParticipantCommand.Execute(participant);
                return;
            }

            if (SelectedParticipant != null && participant.PlanMemberID != SelectedParticipant.PlanMemberID)
            {
                _messengerService.Publish(new ClaimParticipantChangeRequested(this));
                OnChangeParticipantRequest(new EventArgs());
            }
            else
            {
                ChangeParticipantCommand.Execute(participant);
            }

            var isParticipantUnder19Clicked = false;

            var selectedFromOtherList = ParticipantsViewModel.OtherParticipants.Where(p => p.PlanMemberID == participant.PlanMemberID).FirstOrDefault();

            if (selectedFromOtherList != null)
            {
                isParticipantUnder19Clicked = true;
            }
            selectOnlyOneItemAtATime(isParticipantUnder19Clicked);
        }
        public delegate void SelectOnlyOneItemAtATimeEvent(object sender, EventArgs args, bool type);

        public event SelectOnlyOneItemAtATimeEvent SelectOnlyOneItemAtATimeClick;


        protected virtual void selectOnlyOneItemAtATime(bool type)
        {
            SelectOnlyOneItemAtATimeClick?.Invoke(this, EventArgs.Empty, type);
        }

        private void ExecuteChangeParticipantCommand(Participant participant)
        {
            if (participant != SelectedParticipant)
            {
                SelectedParticipant = participant;
            }
            NavigateToClaimDetailsCommand.Execute(null);
        }

        private void ExecuteChangeParticipantWithoutNavigationCommand(Participant participant)
        {
            SelectedParticipant = participant;
        }

        private void ExecuteNavigationToClaimDetailsCommand()
        {
            PublishMessages();
            if (!_claimService.IsHCSAClaim)
            {
                ShowViewModel<ClaimDetailsViewModel>();
            }
            else
            {
                if (!HaveShownClaimDetailsHCSAVM &&
                    _hcsaClaimService.Claim != null &&
                    _hcsaClaimService.Claim.Details != null &&
                    _hcsaClaimService.Claim.Details.Count == 0)
                {
                    ShowViewModel<ClaimDetailsHCSAViewModel>();
                    HaveShownClaimDetailsHCSAVM = true;
                }
                else
                {
                    ShowViewModel<ClaimReviewAndEditViewModel>();
                }
            }
        }

        private void PublishMessages()
        {
            var rehydration = Mvx.IoCProvider.Resolve<IRehydrationService>();
            if (rehydration.Rehydrating && rehydration.HackingRehydration)
            {
                rehydration.Rehydrating = false;
                rehydration.HackingRehydration = false;
                return;
            }
            _messengerService.Publish(new ClearClaimTreatmentDetailsViewRequested(this));
            _messengerService.Publish(new ClearClaimTreatmentDetailsListViewRequested(this));
            _messengerService.Publish(new ClearClaimSubmitTermsAndConditionsViewRequested(this));
            _messengerService.Publish(new ClearClaimSubmissionConfirmationViewRequested(this));
            _messengerService.Publish(new ClearClaimSubmissionResultViewRequested(this));
        }
    }
}