using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System;
using System.Windows.Input;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class EligibilityParticipantsViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IParticipantService _participantsvc;
        private readonly IEligibilityService _eligibilityservice;
        private readonly MvxSubscriptionToken _participantselected;
        private MvxSubscriptionToken _shouldcloseself;

        public EligibilityParticipantsViewModel(IMvxMessenger messenger, IParticipantService participantservice, IEligibilityService eligibilityservice)
        {
            _participantsvc = participantservice;
            _messenger = messenger;
            _eligibilityservice = eligibilityservice;

            ParticipantsViewModel = new ParticipantsViewModel(participantservice, messenger, null, false);

            _participantselected = _messenger.Subscribe<ParticipantSelected>((message) =>
            {
                SelectedParticipant = _participantsvc.SelectedParticipant;
            });
            _shouldcloseself = _messenger.Subscribe<ClearEligibilityParticipantsRequest>((message) =>
            {
                _messenger.Unsubscribe<ClearEligibilityParticipantsRequest>(_shouldcloseself);
                Close(this);
            });
        }

        public EligibilityCheckType EligibilityCheckType
        {
            get
            {
                return _eligibilityservice.SelectedEligibilityCheckType;
            }
        }

        private ParticipantsViewModel _participantsViewModel;
        public ParticipantsViewModel ParticipantsViewModel
        {
            get
            {
                return _participantsViewModel;
            }
            set
            {
                _participantsViewModel = value;
                RaisePropertyChanged(() => ParticipantsViewModel);
            }
        }

        private Participant _selectedParticipant;
        public Participant SelectedParticipant
        {
            get
            {
                return _selectedParticipant;
            }
            set
            {
                _selectedParticipant = value;
				if (_selectedParticipant != null) {
					string[] splitPlanMemberID = _selectedParticipant.PlanMemberID.Split ('-');
					_eligibilityservice.EligibilityCheck.PlanMemberID = long.Parse (splitPlanMemberID [0]);
					_eligibilityservice.EligibilityCheck.ParticipantNumber = splitPlanMemberID [1];
					ParticipantsViewModel.SelectedParticipant = _selectedParticipant;
                    _eligibilityservice.EligibilitySelectedParticipant = _selectedParticipant;
				}
                RaisePropertyChanged(() => SelectedParticipant);
            }
        }

		private Participant _requestedParticipant;
		public Participant RequestedParticipant
		{
			get
			{
				return _requestedParticipant;
			}
			set
			{
				_requestedParticipant = value;
				RaisePropertyChanged(() => SelectedParticipant);
			}
		}

        public event EventHandler ChangeParticipantRequest;
		protected virtual void OnChangeParticipantRequest(EventArgs e)
		{
			if (ChangeParticipantRequest != null)
			{
				ChangeParticipantRequest(this, e);
			}
		}

        public ICommand NavigateToClaimDetailsCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Unsubscribe();
                    SelectedParticipant = _participantsvc.SelectedParticipant;
                    ShowAppropriateViewModel();
                });
            }
        }

		public ICommand NavigateToClaimDetailsDroidCommand
		{
			get
			{
				return new MvxCommand(() =>
					{
						Unsubscribe();
//						SelectedParticipant = _participantsvc.SelectedParticipant;
						ShowAppropriateViewModel();
					});
			}
		}
		public ICommand RequestChangeParticipantCommand
		{
			get
			{
				return new MvxCommand<Participant>((participant) =>
					{
						RequestedParticipant = participant;
					    var rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
						rehydrationservice.Rehydrating=false;
					    if(SelectedParticipant==null)
						{
							ChangeParticipantCommand.Execute(participant);
							return;
						}
						if(SelectedParticipant != null && participant.PlanMemberID != SelectedParticipant.PlanMemberID) {
							_messenger.Publish(new EligibilityParticipantChangeRequested(this));
							OnChangeParticipantRequest(new EventArgs());
						} else {
							ChangeParticipantCommand.Execute(participant);
						}
					});
			}
		}
		public ICommand ChangeParticipantCommand
		{
			get
			{
				return new MvxCommand<Participant>((participant) =>
					{
						if(participant != SelectedParticipant)
						{
							SelectedParticipant = participant;
						}
						NavigateToClaimDetailsCommand.Execute(null);
					});
			}
		}

        private void Unsubscribe()
        {
            _messenger.Unsubscribe<ParticipantSelected>(_participantselected);
        }

        private void ShowAppropriateViewModel()
        {
            switch (EligibilityCheckType.ID)
            {
                case "CHIRO":
                case "PHYSIO":
                    CloseViews();                  
                    ShowViewModel<EligibilityCheckCPViewModel>();
                    break;
                case "MASSAGE":
                    CloseViews();
                    ShowViewModel<EligibilityCheckMassageViewModel>();
                    break;
                case "GLASSES":
                case "CONTACTS":
                case "EYEEXAM":

                    CloseViews();
                    ShowViewModel<EligibilityCheckEyeViewModel>();
                    break;
            }
        }

        private void CloseViews()
        {
            _messenger.Publish(new ClearEligibilityCheckCPRequest(this));
            _messenger.Publish(new ClearEligibilityCheckEyeRequest(this));
            _messenger.Publish(new ClearEligibilityCheckMassageRequest(this));
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
