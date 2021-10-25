using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MobileClaims.Core.ViewModelParameters;

namespace MobileClaims.Core.ViewModels
{
    public class DrugLookupByNameSearchResultsViewModel : ViewModelBase<DrugLookupByNameSearchResultsViewModelParameters>
    {
        private readonly IMvxMessenger _messenger;
        private readonly IParticipantService _participantservice;
        private readonly IDrugLookupService _drugservice;
        private MvxSubscriptionToken _searchcomplete;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private object _sync = new object();

        public DrugLookupByNameSearchResultsViewModel(IMvxMessenger messenger, IParticipantService participantservice, IDrugLookupService drugservice)
        {
            _messenger = messenger;
            _participantservice = participantservice;
            _drugservice = drugservice;

            _searchcomplete = _messenger.Subscribe<SearchDrugByNameComplete>((message) =>
            {
                Busy = false;
                SearchResults = _drugservice.SearchResult;
            });

            _shouldcloseself = _messenger.Subscribe<ClearDrugSearchByNameResultsRequested>((message) =>
            {
                _messenger.Unsubscribe<SearchDrugByNameComplete>(_searchcomplete);
                _messenger.Unsubscribe<ClearDrugSearchByNameResultsRequested>(_shouldcloseself);
                Close(this);
            });
        }

        private Participant _participant;
        public Participant Participant
        {
            get { return _participant; }
            set
            {
                if (_participant != value)
                {
                    _participant = value;
                    RaisePropertyChanged(() => Participant);
                };
            }
        }
        
        private List<DrugInfo> _searchresults;
        public List<DrugInfo> SearchResults
        {
            get
            {
                return _searchresults;
            }
            set
            {
                _searchresults = value;
                RaisePropertyChanged(() => SearchResults);
            }
        }

        private DrugInfo _selecteddrug;
        public DrugInfo SelectedDrug
        {
            get { return _selecteddrug; }
            set
            {
                if (_selecteddrug != value)
                {
                    _selecteddrug = value;
                    RaisePropertyChanged(() => SelectedDrug);
                };
            }
        }

        private bool _busy = false;
        public bool Busy
        {
            get
            {
                return _busy;
            }
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

        public ICommand ShowMyAccountCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Unsubscribe();
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
                    Unsubscribe();
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
                    Unsubscribe();
                    _messenger.Publish<RequestNavToDrugLookup>(new RequestNavToDrugLookup(this));
                    ShowViewModel<ViewModels.DrugLookupModelSelectionViewModel>();
                });
            }
        }

        public ICommand SelectWithoutNavigationCommand
        {
            get
            {
                return new MvxCommand<DrugInfo>((drug) =>
                {
                    this.SelectedDrug = drug;
                }, 
                (drug) =>
                {
                    return drug !=null;
                });
            }
        }

        public ICommand SelectAndNavigateCommand
        {
            get
            {
                return new MvxCommand<DrugInfo>((drug) =>
                {
                    Unsubscribe();
                    _messenger.Publish<ClearDrugSearchResultsRequested>(new ClearDrugSearchResultsRequested(this));
                    ShowViewModel<DrugLookupResultsViewModel, DrugLookupResultsViewModelParameters>(new DrugLookupResultsViewModelParameters(drug.DIN, Participant.PlanMemberID));
                },
                (drug) =>
                {
                    return drug != null;
                });
            }
        }

        private void Unsubscribe()
        {
            _messenger.Unsubscribe<SearchDrugByNameComplete>(_searchcomplete);
        }

        public override void Prepare(DrugLookupByNameSearchResultsViewModelParameters parameter)
        {
            if (!string.IsNullOrEmpty(parameter.PlanMemberID))
            {
                if (_participantservice.PlanMember.PlanMemberID == parameter.PlanMemberID)
                {
                    this.Participant = _participantservice.PlanMember as Participant;
                }
                else
                {
                    if (_participantservice.PlanMember.Dependents.Where(p => p.PlanMemberID == parameter.PlanMemberID).FirstOrDefault() != null)
                    {
                        this.Participant = _participantservice.PlanMember.Dependents.Where(p => p.PlanMemberID == parameter.PlanMemberID).FirstOrDefault();
                    }
                }
            }

            SearchResults = _drugservice.SearchResult;
        }
    }
}
