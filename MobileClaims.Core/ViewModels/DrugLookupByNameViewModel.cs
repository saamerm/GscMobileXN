using FluentValidation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MobileClaims.Core.Validators;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MobileClaims.Core.ViewModelParameters;

namespace MobileClaims.Core.ViewModels
{
    public class DrugLookupByNameViewModel : ViewModelBase
    {
        private readonly IParticipantService _participantsvc;
        private readonly IMvxMessenger _messenger;
        private readonly IDrugLookupService _lookupservice;
        private MvxSubscriptionToken _searchbynamecomplete;
        private MvxSubscriptionToken _searcherror;
        private readonly MvxSubscriptionToken _navigatingtobydin;
        private readonly MvxSubscriptionToken _participantchanged;
        private readonly MvxSubscriptionToken _participantselected;
        private bool _navigateaftersearch;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private object _sync = new object();

        public DrugLookupByNameViewModel(IParticipantService participantsvc, IMvxMessenger messenger,
            IDrugLookupService lookupservice)
        {
            _participantsvc = participantsvc;
            _messenger = messenger;
            _lookupservice = lookupservice;

            if (_participantsvc.PlanMember == null)
            {
                Busy = true;
                _participantsvc.GetParticipant(_loginservice.CurrentPlanMemberID);
            }
            else
            {
                PlanMember = _participantsvc.PlanMember;


                var participants = new List<Participant>();
                if (string.Equals(PlanMember.Status, "ACTIVE", StringComparison.OrdinalIgnoreCase))
                {
                    participants.Add(PlanMember);
                }

                foreach (var p in PlanMember.Dependents.Where(status =>
                    status.Status.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase)))
                {
                    participants.Add(p);
                }

                Participants = participants;
            }

            _participantchanged = _messenger.Subscribe<RetrievedPlanMemberMessage>(message =>
            {
                if (_participantsvc.PlanMember != null)
                {
                    Busy = false;
                    PlanMember = _participantsvc.PlanMember;
                    var participants = new List<Participant>();
                    participants.Add(PlanMember);
                    foreach (var p in PlanMember.Dependents)
                    {
                        participants.Add(p);
                    }

                    Participants = participants;
                }
                else
                {
                    _messenger.Unsubscribe<RetrievedPlanMemberMessage>(_participantchanged);
                    ShowViewModel<LoginViewModel>();
                }
            });

            _navigatingtobydin = _messenger.Subscribe<RequestNavToSearchDrugByDIN>(message =>
            {
                _messenger.Unsubscribe<RequestNavToSearchDrugByDIN>(_navigatingtobydin);
                Close(this);
            });

            SelectedParticipant = _participantsvc.SelectedDrugParticipant;

            _participantselected = _messenger.Subscribe<DrugParticipantSelected>(message =>
            {
                SelectedParticipant = message.SelectedParticipant;
            });

            _shouldcloseself = _messenger.Subscribe<ClearSearchByDrugNameRequested>(message =>
            {
                _messenger.Unsubscribe<RetrievedPlanMemberMessage>(_participantchanged);
                _messenger.Unsubscribe<RequestNavToSearchDrugByDIN>(_navigatingtobydin);
                _messenger.Unsubscribe<ClearSearchByDrugNameRequested>(_shouldcloseself);
                Close(this);
            });
        }

        private bool _searching;

        public bool Searching
        {
            get { return _searching; }
            set
            {
                //RaiseAllPropertiesChanged();
                if (_searching != value)
                {
                    _searching = value;
                    RaisePropertyChanged(() => Searching);

                    _messenger.Publish(new BusyIndicator(this)
                    {
                        Busy = _searching
                    });
                    //if (_searchAndNavigateCommand != null)
                    //  (_searchAndNavigateCommand as MvxCommandBase).RaiseCanExecuteChanged();
                    if (_lookupAndNavigateCommand != null)
                        InvokeOnMainThread(() =>
                            (_lookupAndNavigateCommand as MvxCommandBase).RaiseCanExecuteChanged());
                }
            }
        }

        private bool _busy;

        public bool Busy
        {
            get { return _busy; }
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

        private bool _errorInSearch;

        public bool ErrorInSearch
        {
            get { return _errorInSearch; }
            set
            {

                if (value != _errorInSearch)
                {
                    _errorInSearch = value;
                    RaisePropertyChanged(() => ErrorInSearch);
                }
            }
        }

        private List<DrugInfo> _searchresults = new List<DrugInfo>();

        public List<DrugInfo> SearchResults
        {
            get { return _searchresults; }
            set
            {
                _searchresults = value;
                RaisePropertyChanged(() => SearchResults);
            }
        }

        private string _searcherrormessage;

        public string SearchErrorMessage
        {
            get { return _searcherrormessage; }
            set { _searcherrormessage = value; }
        }

        private string _drugname;

        public string DrugName
        {
            get { return _drugname; }
            set
            {
                _drugname = value;
                RaisePropertyChanged(() => DrugName);
            }
        }

        private DrugInfo _checkresult;

        public DrugInfo CheckResult
        {
            get { return _checkresult; }
            set
            {
                _checkresult = value;
                RaisePropertyChanged(() => CheckResult);
            }
        }

        private List<Participant> _participants;

        public List<Participant> Participants
        {
            get { return _participants; }
            set
            {
                if (_participants != value)
                {
                    _participants = value;
                    RaisePropertyChanged(() => Participants);
                }

                ;
            }
        }

        private List<Participant> _participantsActive;

        public List<Participant> ParticipantsActive
        {
            get { return _participantsActive; }
            set
            {
                if (_participantsActive != value)
                {
                    _participantsActive = value;
                    RaisePropertyChanged(() => ParticipantsActive);
                }

                ;
            }
        }

        private PlanMember _planmember;

        public PlanMember PlanMember
        {
            get { return _planmember; }
            set
            {
                _planmember = value;
                RaisePropertyChanged(() => PlanMember);
            }
        }

        private DrugInfo _selecteddrug;

        public DrugInfo SelectedDrug
        {
            get { return _selecteddrug; }
            set
            {
                _selecteddrug = value;
                RaisePropertyChanged(() => SelectedDrug);
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
                    _selectedparticipant = value;
                    RaisePropertyChanged(() => SelectedParticipant);
                    //RaiseAllPropertiesChanged();
                }
            }
        }

        private bool _sentspecialauth = true;

        public bool SentSpecialAuth
        {
            get { return _sentspecialauth; }
            set
            {
                _sentspecialauth = value;
                RaisePropertyChanged(() => SentSpecialAuth);
            }
        }

        private string _errormessagesendspecialauth;

        public string ErrorMessageSendSpecialAuth
        {
            get { return _errormessagesendspecialauth; }
            set
            {
                _errormessagesendspecialauth = value;
                RaisePropertyChanged(() => ErrorMessageSendSpecialAuth);
            }
        }

        private string _specialauthemail;

        public string SpecialAuthEMail
        {
            get { return _specialauthemail; }
            set
            {
                _specialauthemail = value;
                RaisePropertyChanged(() => SpecialAuthEMail);
            }
        }

        private bool _sendspecialauthformrequested;

        public bool SendSpecialAuthFormRequested
        {
            get { return _sendspecialauthformrequested; }
            set
            {
                _sendspecialauthformrequested = value;
                RaisePropertyChanged(() => SendSpecialAuthFormRequested);
            }
        }

        public event EventHandler OnNoResults;

        protected virtual void RaiseOnNoResults(EventArgs e)
        {
            if (OnNoResults != null)
            {
                OnNoResults(this, e);
            }
        }

        public event EventHandler OnMissingDrugName;

        protected virtual void RaiseOnMissingDrugName(EventArgs e)
        {
            if (OnMissingDrugName != null)
            {
                OnMissingDrugName(this, e);
            }
        }

        public event EventHandler OnInvalidDrugName;

        protected virtual void RaiseOnInvalidDrugName(EventArgs e)
        {
            if (OnInvalidDrugName != null)
            {
                OnInvalidDrugName(this, e);
            }
        }

        public ICommand DownloadSpecialAuthorizationCommand
        {
            get
            {
                return new MvxCommand(() => { },
                    () => { return CheckResult != null; });
            }
        }

        public ICommand SendSpecialAuthorizationCommand
        {
            get
            {
                return new MvxCommand(() => { SendSpecialAuthFormRequested = true; },
                    () =>
                    {
                        return !string.IsNullOrEmpty(CheckResult.SpecialAuthFormName) &&
                               !string.IsNullOrEmpty(CheckResult.SpecialAuthFormName);
                    });
            }
        }

        public ICommand ExecuteSendSpecialAuthorizationCommand
        {
            get
            {
                return new MvxCommand(() => { },
                    () =>
                    {
                        return !string.IsNullOrEmpty(CheckResult.SpecialAuthFormName) &&
                               !string.IsNullOrEmpty(SpecialAuthEMail);
                    });
            }

        }

        private ICommand _lookupAndNavigateCommand;

        public ICommand LookupAndNavigateCommand
        {
            get
            {
                if (_lookupAndNavigateCommand == null)
                {
                    _lookupAndNavigateCommand = new MvxCommand<string>(name =>
                        {
                            ErrorInSearch = false;
                            Searching = true;

                            _messenger.Publish(
                                new ClearDrugSearchByNameResultsRequested(this));
                            _navigateaftersearch = true;

                            _searcherror = _messenger.Subscribe<DrugSearchByNameError>(message =>
                            {
                                ErrorInSearch = true;
                                Searching = false;
                                _messenger.Unsubscribe<DrugSearchByNameError>(_searcherror);
                                _messenger.Unsubscribe<SearchDrugByNameComplete>(_searchbynamecomplete);
                            });

                            _searchbynamecomplete = _messenger.Subscribe<SearchDrugByNameComplete>(message =>
                            {
                                ErrorInSearch = false;
                                Searching = false;
                                _messenger.Unsubscribe<SearchDrugByNameComplete>(_searchbynamecomplete);
                                _messenger.Unsubscribe<DrugSearchByNameError>(_searcherror);
                                if (_navigateaftersearch)
                                {
                                    Unsubscribe();
                                    ShowViewModel<DrugLookupByNameSearchResultsViewModel, DrugLookupByNameSearchResultsViewModelParameters>(new DrugLookupByNameSearchResultsViewModelParameters(SelectedParticipant.PlanMemberID));
                                }
                                else
                                {
                                    SearchResults = _lookupservice.SearchResult;
                                }
                            });
                            _lookupservice.GetByName(name);
                        },
                        name =>
                        {
                            return !string.IsNullOrEmpty(name) && SelectedParticipant != null && !Searching;
                        });
                }

                return _lookupAndNavigateCommand;
            }
        }

        public IMvxCommand SearchAndNavigateCommand => new MvxCommand(ExecuteSearchAndNavigate);

        private void ExecuteSearchAndNavigate()
        {
            ErrorInSearch = false;
            Searching = true;
            _messenger.Publish(new ClearDrugSearchResultsRequested(this));
            _messenger.Publish(new ClearDrugSearchByNameResultsRequested(this));
            _navigateaftersearch = true;

            _searcherror = _messenger.Subscribe<DrugSearchByNameError>(message =>
            {
                ErrorInSearch = true;
                _messenger.Unsubscribe<DrugSearchByNameError>(_searcherror);
                Searching = false;
                _messenger.Unsubscribe<SearchDrugByNameComplete>(_searchbynamecomplete);
                RaiseOnNoResults(new EventArgs());
            });

            _searchbynamecomplete = _messenger.Subscribe<SearchDrugByNameComplete>(message =>
            {
                ErrorInSearch = false;
                Searching = false;
                _messenger.Unsubscribe<SearchDrugByNameComplete>(_searchbynamecomplete);
                _messenger.Unsubscribe<DrugSearchByNameError>(_searcherror);
                if (_navigateaftersearch)
                {
                    Unsubscribe();
                    ShowViewModel<DrugLookupByNameSearchResultsViewModel, DrugLookupByNameSearchResultsViewModelParameters>(new DrugLookupByNameSearchResultsViewModelParameters(SelectedParticipant.PlanMemberID));
                }
                else
                {
                    SearchResults = _lookupservice.SearchResult;
                }
            });

            var validator = new DrugNameValidator();
            var result = validator.Validate(this, "DrugName");
            if (!result.IsValid)
            {
                _messenger.Unsubscribe<SearchDrugByNameComplete>(_searchbynamecomplete);
                _messenger.Unsubscribe<DrugSearchByNameError>(_searcherror);
                foreach (var failure in result.Errors)
                {
                    switch (failure.ErrorMessage)
                    {
                        case "Empty":
                            SearchErrorMessage = Resource.nameError;
                            RaiseOnMissingDrugName(new EventArgs());
                            break;
                        case "Too Short":
                            SearchErrorMessage = Resource.nameLengthError;
                            RaiseOnInvalidDrugName(new EventArgs());
                            break;
                    }
                }

                ErrorInSearch = true;
                Searching = false;
                return;
            }

            _lookupservice.GetByName(DrugName);
        }

        public ICommand LookupCommand
        {
            get
            {
                return new MvxCommand<string>(name =>
                {

                    _lookupservice.GetByName(name);
                },
                name =>
                {
                    if (!string.IsNullOrEmpty(name)) return true;
                    return false;
                });
            }
        }

        public ICommand SetSelectedParticipantCommand
        {
            get
            {
                return new MvxCommand<Participant>(participant =>
               {
                   SelectedParticipant = participant;
               });
            }
        }

        public ICommand GetDrugInfoCommand
        {
            get
            {
                return new MvxCommand<DrugInfo>(Drug =>
                {
                }, din => { return !string.IsNullOrEmpty(din.DIN.ToString()) && SelectedParticipant != null; });

            }
        }

        private void Unsubscribe()
        {
            _messenger.Unsubscribe<RetrievedPlanMemberMessage>(_participantchanged);
            _messenger.Unsubscribe<DrugParticipantSelected>(_participantselected);
        }
    }
}
