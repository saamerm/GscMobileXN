using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using FluentValidation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.Validators;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimTreatmentDetailsListViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IParticipantService _participantservice;
        private readonly IClaimService _claimservice;
        private readonly IUserDialogs _userDialog;
        private readonly MvxSubscriptionToken _reloadself;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private bool _hasNavigatedToFirstEntry;
        private bool _missingTreatmentDetails;
        private int _selectedTreatmentPosition = -1;
        private ObservableCollection<TreatmentDetail> _treatmentDetails;

        public event EventHandler OnInvalidClaim;
        public event EventHandler OnTriggerRemoveWindowPopup;

        public Participant Participant
        {
            get
            {
                if (_claimservice != null && _claimservice.Claim != null)
                {
                    return _claimservice.Claim.Participant;

                }
                else
                {
                    return null;
                }
            }
        }

        public ClaimSubmissionType ClaimSubmissionType
        {
            get
            {
                return _claimservice.SelectedClaimSubmissionType;
            }
        }

        //Used to display message once we get 5 items, which is the maximum
        private bool _fiveTreatmentDetails;
        public bool FiveTreatmentDetails
        {
            get
            {
                _fiveTreatmentDetails = TreatmentDetails != null && TreatmentDetails.Count > 0 && _claimservice != null && _claimservice.Claim != null
                    ? TreatmentDetails.Count == _claimservice.Claim.MaximumAllowedTreatmentDetails
                    : false;
                return _fiveTreatmentDetails;
            }
            set
            {
                SetProperty(ref _fiveTreatmentDetails, value);
                RaisePropertyChanged(() => FiveTreatmentDetails);
            }
        }

        //Used to keep track of navigation flow
        //For WP implementation, on view load, I navigate to Entry view and update this to be true 
        //so that on subsequent view loads, it does not trigger Entry view navigate again
        public bool HasNavigatedToFirstEntry
        {
            get => _hasNavigatedToFirstEntry;
            set => SetProperty(ref _hasNavigatedToFirstEntry, value);
        }

        public bool MissingTreatmentDetails
        {
            get => _missingTreatmentDetails;
            set => SetProperty(ref _missingTreatmentDetails, value);
        }

        public int SelectedTreatmentPosition
        {
            get => _selectedTreatmentPosition;
            set => SetProperty(ref _selectedTreatmentPosition, value);
        }

        public ObservableCollection<TreatmentDetail> TreatmentDetails
        {
            get => _treatmentDetails;
            set => SetProperty(ref _treatmentDetails, value);
        }

        public MvxCommand<TreatmentDetail> SelectTreatmentDetailCommand { get; }

        public IMvxCommand AddTreatmentDetailCommand { get; }

        public override ICommand RemoveCommand { get; }

        public IMvxCommand RemoveDroidCommand { get; }

        public MvxCommand<TreatmentDetail> TriggerRemoveWindow { get; }

        public IMvxCommand SubmitClaimCommand { get; }

        public IMvxCommand SubmitClaimCommandWindows { get; }
        
        public string MaxTreatmentDetailsLabel
        {
            get
            {
                switch (ClaimSubmissionType.ID)
                {
                    case "DENTAL":
                        return Resource.MoreThanTenClaims;
                    default:
                        return Resource.MoreThanFiveClaims;
                }
            }
        }

        public ClaimTreatmentDetailsListViewModel(IMvxMessenger messenger,
            IParticipantService participantservice,
            IClaimService claimservice,
            IUserDialogs userDialogs)
        {
            _messenger = messenger;
            _participantservice = participantservice;
            _claimservice = claimservice;
            _userDialog = userDialogs;

            var rehydrationService = Mvx.IoCProvider.Resolve<IRehydrationService>();
            if (!rehydrationService.Rehydrating)
            {
                _claimservice.IsTreatmentDetailsListInNavStack = true;
            }
            else
            {
                rehydrationService.Rehydrating = false;
            }

            TreatmentDetails = _claimservice.Claim.TreatmentDetails;
       
            SelectTreatmentDetailCommand = new MvxCommand<TreatmentDetail>(ExecuteSelectTreatmentDetailCommand);
            AddTreatmentDetailCommand = new MvxCommand(ExecuteAddTreatmentDetailCommand);
            RemoveCommand = new MvxCommand<int>(ExecuteRemoveCommand);
            RemoveDroidCommand = new MvxCommand(ExecuteRemoveDroidCommand);
            TriggerRemoveWindow = new MvxCommand<TreatmentDetail>(ExecuteTriggerRemoveCommand);
            SubmitClaimCommand = new MvxCommand(ExecuteSubmitClaimCommand);
            SubmitClaimCommandWindows = new MvxCommand(ExecuteSubmitClaimCommandWindows, CanExecuteSubmitClaimCommandWindows);

            _shouldcloseself = _messenger.Subscribe<ClearClaimTreatmentDetailsListViewRequested>((message) =>
            {
                if (rehydrationService.Rehydrating)
                {
                    return;
                }
                _messenger.Unsubscribe<ClearClaimTreatmentDetailsListViewRequested>(_shouldcloseself);
                _claimservice.IsTreatmentDetailsListInNavStack = false;
                Close(this);
            });

            _messenger.Publish<OnTreatmentDetailsListViewModelMessage>(new OnTreatmentDetailsListViewModelMessage(this));
        }

        public override void Start()
        {
            TreatmentDetails = _claimservice.Claim.TreatmentDetails;
            if (TreatmentDetails == null || TreatmentDetails.Count == 0)
            {
                var rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
                if (rehydrationservice.BusinessProcess.Contains(typeof(ClaimSubmissionConfirmationViewModel)))
                {
                    rehydrationservice.BusinessProcess.Remove(typeof(ClaimSubmissionConfirmationViewModel));
                    rehydrationservice.Save();
                }
            }
        }

        protected virtual void RaiseInvalidClaim(EventArgs e)
        {
            if (this.OnInvalidClaim != null)
            {
                OnInvalidClaim(this, e);
            }
        }

        protected virtual void RaiseOnTriggerRemoveWindowPopup(EventArgs e)
        {
            if (this.OnTriggerRemoveWindowPopup != null)
            {
                OnTriggerRemoveWindowPopup(this, e);
            }
        }

        private void ExecuteSelectTreatmentDetailCommand(TreatmentDetail selectedTreatmentDetail)
        {
            _claimservice.SelectedTreatmentDetailID = selectedTreatmentDetail.ID;
            ShowAppropriateViewModel(false);
        }

        private void ExecuteAddTreatmentDetailCommand()
        {
            if (TreatmentDetails.Count == _claimservice.Claim.MaximumAllowedTreatmentDetails)
            {
                _userDialog.Alert(_claimservice.Claim.MaximumAllowedTreatmentDetails == 10
                    ? Resource.MoreThanTenClaims
                    : Resource.MoreThanFiveClaims, null, Resource.ok);
                return;
            }

            _claimservice.SelectedTreatmentDetailID = Guid.Empty;
            ShowAppropriateViewModel(true);
        }

        private void ExecuteRemoveCommand(int index)
        {
            TreatmentDetails.RemoveAt(index);

            

            if (TreatmentDetails.Count == 0)
            {
                var rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
                if (rehydrationservice.BusinessProcess.Contains(typeof(ClaimSubmissionConfirmationViewModel)))
                {
                    rehydrationservice.BusinessProcess.Remove(typeof(ClaimSubmissionConfirmationViewModel));
                    rehydrationservice.Save();
                }
            }
        }

        private void ExecuteRemoveDroidCommand()
        {
            if (SelectedTreatmentPosition > -1)
            {
                TreatmentDetails.RemoveAt(SelectedTreatmentPosition);
            }
        }

        private void ExecuteTriggerRemoveCommand(TreatmentDetail selectedTreatmentDetail)
        {
            SelectedTreatmentPosition = TreatmentDetails.IndexOf(selectedTreatmentDetail);

            if (SelectedTreatmentPosition > -1)
            {
                TreatmentDetails.RemoveAt(SelectedTreatmentPosition);
            }

            FiveTreatmentDetails = TreatmentDetails != null && TreatmentDetails.Count > 0 && _claimservice != null && _claimservice.Claim != null
                    ? TreatmentDetails.Count == _claimservice.Claim.MaximumAllowedTreatmentDetails
                    : false;

            if (TreatmentDetails.Count == 0)
            {
                _claimservice.SelectedTreatmentDetailID = Guid.Empty;

                var rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
                if (rehydrationservice.BusinessProcess.Contains(typeof(ClaimSubmissionConfirmationViewModel)))
                {
                    rehydrationservice.BusinessProcess.Remove(typeof(ClaimSubmissionConfirmationViewModel));
                    rehydrationservice.Save();
                }

                ShowAppropriateViewModel(true);
            }
        }

        private void ExecuteSubmitClaimCommand()
        {
            var validator = new TreatmentDetailListValidator();
            MissingTreatmentDetails = !validator.Validate<ClaimTreatmentDetailsListViewModel>(this, "TreatmentDetails").IsValid;
            List<bool> errors = new List<bool>();
            errors.Add(MissingTreatmentDetails);
            if (errors.Any(b => b == true))
            {
                RaiseInvalidClaim(new EventArgs());
                return;
            }
            PublishMessages();

            this.ShowViewModel<ClaimSubmitTermsAndConditionsViewModel>();
        }

        private bool CanExecuteSubmitClaimCommandWindows()
        {
            return TreatmentDetails != null && TreatmentDetails.Count > 0;
        }

        private void ExecuteSubmitClaimCommandWindows()
        {
            PublishMessages();
            this.ShowViewModel<ClaimSubmitTermsAndConditionsViewModel>();
        }


        private void ShowAppropriateViewModel(bool addNew)
        {
            PublishMessages();
            switch (ClaimSubmissionType.ID)
            {
                case "ACUPUNCTURE":
                case "CHIROPODY":
                case "CHIRO":
                case "PHYSIO":
                case "PODIATRY":
                    this.ShowViewModel<ClaimTreatmentDetailsEntry1ViewModel>();
                    break;
                case "PSYCHOLOGY":
                case "MASSAGE":
                case "NATUROPATHY":
                case "SPEECH":
                    this.ShowViewModel<ClaimTreatmentDetailsEntry2ViewModel>();
                    break;
                case "MI":
                    this.ShowViewModel<ClaimTreatmentDetailsEntryMIViewModel>();
                    break;
                case "ORTHODONTIC":
                    this.ShowViewModel<ClaimTreatmentDetailsEntryOMFViewModel>();
                    break;
                case "CONTACTS":
                    this.ShowViewModel<ClaimTreatmentDetailsEntryPCViewModel>();
                    break;
                case "GLASSES":
                    this.ShowViewModel<ClaimTreatmentDetailsEntryPGViewModel>();
                    break;
                case "EYEEXAM":
                    this.ShowViewModel<ClaimTreatmentDetailsEntryREEViewModel>();
                    break;
                case "DENTAL":
                    ShowViewModel<ClaimTreatmentDetailsEntryDentalViewModel>();
                    break;
            }
        }

        private void PublishMessages()
        {
            _messenger.Publish<Messages.ClearClaimTreatmentDetailsViewRequested>(new MobileClaims.Core.Messages.ClearClaimTreatmentDetailsViewRequested(this));
            _messenger.Publish<Messages.ClearClaimSubmitTermsAndConditionsViewRequested>(new MobileClaims.Core.Messages.ClearClaimSubmitTermsAndConditionsViewRequested(this));
            _messenger.Publish<Messages.ClearClaimSubmissionConfirmationViewRequested>(new MobileClaims.Core.Messages.ClearClaimSubmissionConfirmationViewRequested(this));
            _messenger.Publish<Messages.ClearClaimSubmissionResultViewRequested>(new MobileClaims.Core.Messages.ClearClaimSubmissionResultViewRequested(this));
        }
    }
}