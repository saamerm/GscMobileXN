using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class DrugLookupSelectParticipantViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IParticipantService _participantservice;
        private readonly IDrugLookupService _drugservice;
        private object _sync = new object();

        public DrugLookupSelectParticipantViewModel(IMvxMessenger messenger, IParticipantService participantservice, IDrugLookupService drugservice)
        {
            _messenger = messenger;
            _participantservice = participantservice;
            _drugservice = drugservice;

            PlanMember = _participantservice.PlanMember;
        }

        public void Init(NavigationHelper navHelper)
        {
            Drug = navHelper.Drug;
        }

        public class NavigationHelper
        {
            public DrugInfo Drug
            {
                get;
                set;
            }
            public NavigationHelper()
            {

            }
            public NavigationHelper(DrugInfo drug)
            {
                Drug = drug;
            }
        }

        private PlanMember _planmember;
        public PlanMember PlanMember
        {
            get { return _planmember; }
            set
            {
                if (_planmember != value)
                {
                    _planmember = value;
                    RaisePropertyChanged(() => PlanMember);
                };
            }
        }
        
        private DrugInfo _drug;
        public DrugInfo Drug
        {
            get { return _drug; }
            set
            {
                if (_drug != value)
                {
                    _drug = value;
                    RaisePropertyChanged(() => Drug);
                };
            }
        }

        private Participant _selectedparticipant;
        public Participant SelectedParticipant
        {
            get { return _selectedparticipant; }
            set
            {
                if (_selectedparticipant != value)
                {
                    _selectedparticipant = (Participant)value;
                    RaisePropertyChanged(() => SelectedParticipant);
                };
            }
        }

        public ICommand ShowMyAccountCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _messenger.Publish<RequestNavToCard>(new RequestNavToCard(this));
                    ShowViewModel<CardViewModel>();
                });
            }
        }
        public ICommand ShowBalancesCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _messenger.Publish<RequestNavToSpendingAccounts>(new RequestNavToSpendingAccounts(this));
                    ShowViewModel<ChooseSpendingAccountTypeViewModel>();
                });
            }
        }
       
        public ICommand ShowDrugLookupCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _messenger.Publish<RequestNavToDrugLookup>(new RequestNavToDrugLookup(this));
                    ShowViewModel<ViewModels.DrugLookupModelSelectionViewModel>();
                });
            }
        }

        public ICommand SelectWithoutNavigationCommand
        {
            get
            {
                return new MvxCommand<Participant>((participant) =>
                    {
                        SelectedParticipant = participant;
                    },
                    (participant) =>
                    {
                        return participant != null;
                    });
            }
        }
    }
}
