using Acr.UserDialogs;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.HCSA;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.File;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels.HCSA
{
    public class ClaimParticipantViewModel : HCSAViewModelBase
    {
        #region Member Variables
        private IMvxMessenger _messenger;
        private IMvxFileStore _filesystem;
        private IParticipantService _participantservice;
        private IHCSAClaimService _claimservice;
        ILoggerService _logger;
        #endregion

        #region CIRS
        public ClaimParticipantViewModel(IMvxMessenger messenger,
                                         IMvxFileStore filesystem,
                                         IParticipantService participantservice,
                                         IHCSAClaimService claimservice,
                                         ILoggerService logger)
        {
            _messenger = messenger;
            _filesystem = filesystem;
            _participantservice = participantservice;
            _claimservice = claimservice;
            _logger = logger;
        }

        public override void Start()
        {
            base.Start();
            if (_participantservice.PlanMember != null)
            {
                Participants = new ObservableCollection<Participant>();
                Participants.Add(_participantservice.PlanMember as Participant);
                foreach (Participant p in _participantservice.PlanMember.Dependents)
                {
                    Participants.Add(p);
                }
                if (_participantservice.SelectedHCSAParticipant != null)
                {
                    var qry = from Participant p in Participants
                              where p.PlanMemberID.Equals(_participantservice.SelectedHCSAParticipant.PlanMemberID)
                              select p;
                    var match = qry.FirstOrDefault();
                    if (match != null)
                    {
                        match.Selected = true;
                    }
                }
            }
        }
        #endregion

        #region Properties

        #region Participants
        private ObservableCollection<Participant> _participants;
        public ObservableCollection<Participant> Participants
        {
            get
            {
                return _participants;
            }
            set
            {
                _participants = value;
                RaisePropertyChanged(() => Participants);
            }
        }

        #region Labels
        public string SelectPlanParticipantLabel
        {
            get
            {
                return Resource.SelectThePlanParticipant;
            }
        }
        #endregion

        #endregion

        #endregion

        #region Commands
        #region SelectParticipantAndNavigateCommand
        MvxCommand<Participant> _selectparticipantandnavigatecommand;
        public ICommand SelectParticipantAndNavigateCommand
        {
            get
            {
                _selectparticipantandnavigatecommand = _selectparticipantandnavigatecommand ?? new MvxCommand<Participant>(async(participant) =>
                                                              {
                                                                  //logic goes here
                                                                  if(participant != _participantservice.SelectedHCSAParticipant && _participantservice.SelectedHCSAParticipant !=null)
                                                                  {
                                                                      ConfirmConfig config = new ConfirmConfig();
                                                                      config.CancelText = Resource.cancelCaps;
                                                                      config.OkText = Resource.ok;
                                                                      config.Message = string.Format("{0}\n{1}", Resource.HCSAPlanParticipantChangedMessage, Resource.HCSAPlanParticipantChangedMessage2);
                                                                      config.Title = Resource.HCSAPlanParticipantChangedTitle;
                                                                      var confirm = await UserDialogs.Instance.ConfirmAsync(config);
                                                                      if(!confirm)
                                                                      {
                                                                          return;
                                                                      }
                                                                  }
                                                                  Unsubscribe();
                                                                  PublishMessages();
                                                                  participant.Selected = true;
                                                                  _participantservice.SelectedHCSAParticipant = participant;
                                                                  ShowViewModel<ClaimDetailsHCSAViewModel>();
                                                              },
                                                          (participant) =>
                                                              {
                                                                  return participant != null;
                                                              }
                                                         );
                return _selectparticipantandnavigatecommand;
            }
        }
        #endregion


        #endregion

        #region Private Methods

        #endregion
    }
}
