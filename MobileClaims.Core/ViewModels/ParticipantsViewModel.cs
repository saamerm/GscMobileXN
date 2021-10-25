using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class ParticipantsViewModel : ViewModelBase
    {
        private readonly IParticipantService _participantsvc;
        private readonly IMvxMessenger _messenger;
        private readonly MvxSubscriptionToken _participantschanged;

        //Hack - set this in the constructor -> it correlates to setDefaultParticipant parameter in the constructor.  If it's true then we have *not* been created by the Claims process.  
        private bool _defaultparticipantsetinconstructor;

        private bool _busy;
        private PlanMember _planmember;
        private Participant _selectedParticipant;
        private ClaimSubmissionType _selectedClaimSubmissionType;

        private List<Participant> _participants = new List<Participant>();
        private List<Participant> _otherParticipants = new List<Participant>();

        public ParticipantsViewModel(IParticipantService participantservice,
            IMvxMessenger messenger,
            ClaimSubmissionType selectedClaimSubmissionType,
            bool setDefaultParticipant = true)
        {
            _participantsvc = participantservice;
            _messenger = messenger;
            _defaultparticipantsetinconstructor = setDefaultParticipant;
            _selectedClaimSubmissionType = selectedClaimSubmissionType ?? new ClaimSubmissionType();

            _participantschanged = _messenger.Subscribe<RetrievedPlanMemberMessage>((message) =>
            {
                Busy = false;
                _messenger.Unsubscribe<RetrievedPlanMemberMessage>(_participantschanged);
                PopulatePlanMemberAndParticipants(setDefaultParticipant);
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
                PopulatePlanMemberAndParticipants(setDefaultParticipant);
            }
        }

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

        public PlanMember PlanMember
        {
            get => _planmember;
            set
            {
                _planmember = value;
                RaisePropertyChanged(() => PlanMember);
            }
        }

        public Participant SelectedParticipant
        {
            get => _defaultparticipantsetinconstructor ? _participantsvc.SelectedDrugParticipant : _participantsvc.SelectedParticipant;
            set
            {
                if (value != _selectedParticipant && value != null)
                {
                    _selectedParticipant = value;

                    if (!_defaultparticipantsetinconstructor)
                    {
                        _participantsvc.SelectedParticipant = value;
                        _messenger.Publish<ParticipantSelected>(new ParticipantSelected(this, value));
                    }
                    else
                    {
                        _participantsvc.SelectedDrugParticipant = value;
                        _messenger.Publish<DrugParticipantSelected>(new DrugParticipantSelected(this, value));
                        _messenger.Publish<ClearDrugSearchByNameResultsRequested>(new ClearDrugSearchByNameResultsRequested(this));
                        _messenger.Publish<ClearDrugSearchResultsRequested>(new ClearDrugSearchResultsRequested(this));
                    }
                    RaisePropertyChanged(() => SelectedParticipant);
                }
            }
        }

        public List<Participant> Participants
        {
            get => _participants;
            set
            {
                _participants = value;
                RaisePropertyChanged(() => Participants);
                RaisePropertyChanged(() => ParticipantsActive);
            }
        }

        public List<Participant> OtherParticipants
        {
            get => _otherParticipants;
            set
            {
                _otherParticipants = value;
                RaisePropertyChanged(() => OtherParticipants);
            }
        }

        public List<Participant> ParticipantsActive
        {
            get
            {
                return _participants.Where(p => string.Equals(p.Status, "ACTIVE", StringComparison.OrdinalIgnoreCase)
                                             || (!string.IsNullOrWhiteSpace(p.FirstName) &&
                                                string.Equals(p.FirstName, PlanMember.FirstName, StringComparison.OrdinalIgnoreCase))).ToList();
            }
        }

        private void PopulatePlanMemberAndParticipants(bool setDefaultParticipant)
        {
            this.PlanMember = _participantsvc.PlanMember;

            List<Participant> participants = new List<Participant>();

            participants.Add(PlanMember);

            foreach (Participant p in PlanMember.Dependents)
            {
                participants.Add(p);
            }

            this.Participants = participants;

            if (setDefaultParticipant)
            {
                SelectedParticipant = participants.First();
            }
           
#if CCQ || FPPM
            if (string.Equals(_selectedClaimSubmissionType.ID, "GLASSES", StringComparison.OrdinalIgnoreCase)
                || string.Equals(_selectedClaimSubmissionType.ID, "CONTACTS", StringComparison.OrdinalIgnoreCase))
            {
                var otherParticipants = Participants.Where(x => x.IsResidentOfQuebecProvince()
                            && x.IsOrUnderAgeOf18());
                OtherParticipants = otherParticipants.ToList();
                Participants = participants.Except(otherParticipants).ToList();
            }
#else
       
            OtherParticipants = new List<Participant>();
#endif
        }
    }
}
