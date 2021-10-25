using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.ClaimsHistory;
using MobileClaims.Core.ViewModels.HCSA;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels.ClaimsHistory
{
    public class ClaimsHistoryParticipantsViewModel : HCSAViewModelBase
    {
        private readonly IParticipantService _participantsvc;
        private readonly IClaimsHistoryService _claimshistoryservice;
        private readonly IMvxMessenger _messenger;

        public ClaimsHistoryParticipantsViewModel(IParticipantService participantservice, IClaimsHistoryService claimshistoryservice, IMvxMessenger messenger)
        {
            _participantsvc = participantservice;
            _claimshistoryservice = claimshistoryservice;
            _messenger = messenger;
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            
            MvxSubscriptionToken _participantschanged = null;
            _participantschanged = _messenger.Subscribe<RetrievedPlanMemberMessage>((message) =>
            {
                Busy = false;
                _messenger.Unsubscribe<RetrievedPlanMemberMessage>(_participantschanged);
                PopulatePlanMemberAndParticipants(true);
            });

            if (_participantsvc.PlanMember == null)
            {
                try
                {
                    Busy = true;
                    _participantsvc.GetParticipant(_loginservice.CurrentPlanMemberID);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            else
            {
                PopulatePlanMemberAndParticipants(true);
            }

            SetSelectedParticipant();
        }

        private PlanMember _planmember;
        public PlanMember PlanMember
        {
            get => _planmember;
            set
            {
                _planmember = value;
                RaisePropertyChanged(() => PlanMember);
            }
        }

        private ObservableCollection<Participant> _participants = new ObservableCollection<Participant>();
        public ObservableCollection<Participant> Participants
        {
            get => _participants;
            set
            {
                _participants = value;
                RaisePropertyChanged(() => Participants);
            }
        }

        private Participant _selectedParticipant;
        public Participant SelectedParticipant
        {
            get => _selectedParticipant;
            set
            {
                if (value != _selectedParticipant && value != null)
                {
                    _selectedParticipant = value;
                    RaisePropertyChanged(() => SelectedParticipant);
                }
            }
        }

        private bool _busy = false;
        public bool Busy
        {
            get => _busy;
            set
            {
                if (_busy != value)
                {
                    _busy = value;
                    _messenger.Publish<BusyIndicator>(new BusyIndicator(this)
                    {
                        Busy = _busy
                    });
                    RaisePropertyChanged(() => Busy);
                }
            }
        }

        public string ParticipantLabel => Resource.PlanMemberNoColon;

        public string SelectThePlanParticipantLabel => Resource.SelectThePlanParticipant;

        public string DoneLabel => Resource.Done;

        public ICommand ParticipantSelectedCommand
        {
            get
            {
                return new MvxCommand<Participant>((selectedParticipant) =>
                {
                    this.SelectedParticipant = selectedParticipant;
                },
                (selectedParticipant) =>
                {
                    return true;
                });
            }
        }

        MvxCommand _doneCommand;
        public ICommand DoneCommand
        {
            get
            {
                _doneCommand = _doneCommand ?? new MvxCommand(() =>
                {
                    _claimshistoryservice.SelectedParticipant = _selectedParticipant;
                    RaiseParticipantSelectionComplete(new EventArgs());
                    Close(this);
                },
                () =>
                {
                    return true;
                });
                return _doneCommand;
            }
        }

        public event EventHandler ParticipantSelectionComplete;
        protected virtual void RaiseParticipantSelectionComplete(EventArgs e)
        {
            if (this.ParticipantSelectionComplete != null)
            {
                ParticipantSelectionComplete(this, e);
            }
        }

        private void PopulatePlanMemberAndParticipants(bool setDefaultParticipant)
        {
            this.PlanMember = _participantsvc.PlanMember;
            ObservableCollection<Participant> participants = new ObservableCollection<Participant>();
            participants.Add((Participant)PlanMember);
            foreach (Participant p in PlanMember.Dependents)
            {
                participants.Add(p);
            }
            this.Participants = participants;

            if (_claimshistoryservice.SelectedParticipant != null)
            {
                SelectedParticipant = _claimshistoryservice.SelectedParticipant;
            }
            else
            {
                if (setDefaultParticipant)
                    SelectedParticipant = participants.First();
            }

            SetSelectedParticipant();
        }

        private void SetSelectedParticipant()
        {
            if (_claimshistoryservice.SelectedParticipant != null && Participants != null && Participants.Count > 0)
            {
                SelectedParticipant = Participants.FirstOrDefault(p => p.PlanMemberID == _claimshistoryservice.SelectedParticipant.PlanMemberID);
            }
        }
    }
}