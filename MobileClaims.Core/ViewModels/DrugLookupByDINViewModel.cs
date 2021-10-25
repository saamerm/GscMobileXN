//#define FakingIt

using FluentValidation;
using MobileClaims.Core.Attributes;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using MobileClaims.Core.Validators;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MobileClaims.Core.ViewModelParameters;

namespace MobileClaims.Core.ViewModels
{
    [RequiresAuthentication(true)]
    public class DrugLookupByDINViewModel : ViewModelBase
    {
        private readonly IParticipantService _participantsvc;
        private readonly IMvxMessenger _messenger;
        private readonly IDrugLookupService _lookupservice;
        private readonly MvxSubscriptionToken _participantchanged;
        private  MvxSubscriptionToken _searchcomplete;
        private  MvxSubscriptionToken _searcherror;
		private readonly MvxSubscriptionToken _navigatingtobyname;
        private readonly MvxSubscriptionToken _participantselected;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private object _sync = new object();

        public DrugLookupByDINViewModel(IParticipantService participantsvc, IMvxMessenger messenger, IDrugLookupService lookupsvc, ILoginService loginservice) : base(messenger, loginservice)
        {
			_participantsvc = participantsvc;
            _messenger = messenger;
            _lookupservice = lookupsvc;

            if (_participantsvc.PlanMember == null)
            {
                try
                {
                    _participantsvc.GetParticipant(_loginservice.CurrentPlanMemberID);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            else
            {
                this.PlanMember = _participantsvc.PlanMember;
                List<Participant> participants = new List<Participant>();
                if (string.Equals(PlanMember.Status, "ACTIVE", StringComparison.OrdinalIgnoreCase))
                {
                    participants.Add((Participant)PlanMember);
                }
                foreach (Participant p in PlanMember.Dependents.Where((status) => status.Status.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase)))
                {
                    participants.Add(p);
                }
                this.Participants = participants;
                this.SelectedParticipant = _participantsvc != null ? _participantsvc.SelectedDrugParticipant : null;
            }

            _participantchanged = _messenger.Subscribe<RetrievedPlanMemberMessage>((message) =>
            {
                if (_participantsvc.PlanMember == null)
                {
                    _messenger.Unsubscribe<RetrievedPlanMemberMessage>(_participantchanged);
                    this.ShowViewModel<LoginViewModel>();
                }
                else
                {
                    this.PlanMember = _participantsvc.PlanMember;
                    List<Participant> participants = new List<Participant>();
                    participants.Add((Participant)PlanMember);
                    foreach (Participant p in PlanMember.Dependents)
                    {
                        participants.Add(p);
                    }
                    this.Participants = participants;
                }
            });

            _participantselected = _messenger.Subscribe<DrugParticipantSelected>((message) =>
            {
                this.SelectedParticipant = _participantsvc.SelectedDrugParticipant;
            });

			_navigatingtobyname = _messenger.Subscribe<RequestNavToSearchDrugByName>((message) =>
			{
				_messenger.Unsubscribe<RequestNavToSearchDrugByName>(_navigatingtobyname);
				Close(this);
			});

            _shouldcloseself = _messenger.Subscribe<ClearDrugSearchByDINRequested>((message) =>
            {
                _messenger.Unsubscribe<ClearDrugSearchByDINRequested>(_shouldcloseself);
                _messenger.Unsubscribe<RetrievedPlanMemberMessage>(_participantchanged);
                _messenger.Unsubscribe<DrugParticipantSelected>(_participantselected);
                _messenger.Unsubscribe<RequestNavToSearchDrugByName>(_navigatingtobyname);
                Close(this);
            });
        }

        private string _searcherrormessage;
        public string SearchErrorMessage
        {
            get
            {
                return _searcherrormessage;
            }
            set
            {
                _searcherrormessage = value;
            }
        }
        private bool _searching;
        public bool Searching
        {
            get
            {
                return _searching;
            }
            set
            {
                RaiseAllPropertiesChanged();
                if (_searching != value)
                {
                    _searching = value;
                    RaisePropertyChanged(() => Searching);

                    _messenger.Publish<BusyIndicator>(new BusyIndicator(this)
                    {
                        Busy = _searching
                    });
                    //(_searchAndNavigateCommand as MvxCommandBase).RaiseCanExecuteChanged();
                }
            }
        }

        private bool _errorinsearch;
        public bool ErrorInSearch
        {
            get { return _errorinsearch; }
            set
            {
                if (value != _errorinsearch)
                {
                    _errorinsearch = value;
                    RaisePropertyChanged(() => ErrorInSearch);
                }
            }
        }

        private string _din;
        public string DIN
        {
            get { return _din; }
            set
            {
                if (_din != value)
                {
                    _din = value;
                    RaisePropertyChanged(() => DIN);
                    RaiseAllPropertiesChanged();
                };
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
                };
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
        private Participant _selectedparticipant;
        public Participant SelectedParticipant
        {
            get
            {
                return _selectedparticipant;
            }
            set
            {
                if (_selectedparticipant != value)
                {
                    _selectedparticipant = value;
                    RaisePropertyChanged(() => SelectedParticipant);
                    RaiseAllPropertiesChanged();
                }
            }
        }

        private bool _sentspecialauth = false;
        public bool SentSpecialAuth
        {
            get
            {
                return _sentspecialauth;
            }
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
            if (this.OnNoResults != null)
            {
                OnNoResults(this, e);
            }
        }

        public event EventHandler OnMissingDIN;
        protected virtual void RaiseOnMissingDIN(EventArgs e)
        {
            if (this.OnMissingDIN != null)
            {
                OnMissingDIN(this, e);
            }
        }

        public event EventHandler OnInvalidDIN;
        protected virtual void RaiseOnInvalidDIN(EventArgs e)
        {
            if (this.OnInvalidDIN != null)
            {
                OnInvalidDIN(this, e);
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

        public ICommand SetSelectedParticipantCommand
		{
			get{
				return new MvxCommand<Participant> ((participant) => {
					this.SelectedParticipant = participant;
				});
			}
		}

        public ICommand DownloadSpecialAuthorizationCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                }, () =>
                {
                    return this.CheckResult != null;
                });
            }
        }

        public ICommand SendSpecialAuthorizationCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    SendSpecialAuthFormRequested = true;
                }, () => { return !string.IsNullOrEmpty(this.CheckResult.SpecialAuthFormName) && !string.IsNullOrEmpty(this.CheckResult.SpecialAuthFormName); });
            }
        }

        public ICommand ExecuteSendSpecialAuthorizationCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                }, () => { return !string.IsNullOrEmpty(CheckResult.SpecialAuthFormName) && !string.IsNullOrEmpty(SpecialAuthEMail); });
            }

        }

        public ICommand GetDrugInfoCommand
        {
            get
            {
                return new MvxCommand<string>((din) =>
                {
                }, (din) => { return !string.IsNullOrEmpty(din) && this.SelectedParticipant!=null; });
            }
        }
        private ICommand _searchAndNavigateCommand;
        public ICommand SearchAndNavigateCommand
        {
            get
            {
                if (_searchAndNavigateCommand == null)
                {
                    _searchAndNavigateCommand = new MvxCommand(() =>
                    {
                        ErrorInSearch = false;
                        Searching = true;
                        _messenger.Publish<ClearDrugSearchResultsRequested>(new ClearDrugSearchResultsRequested(this));
                        _messenger.Publish<ClearDrugSearchByNameResultsRequested>(new ClearDrugSearchByNameResultsRequested(this));

                        _searchcomplete = _messenger.Subscribe<DrugSearchComplete>((message) =>
                        {
                            Unsubscribe();
                            ErrorInSearch = false;
                            Searching = false;
                            _messenger.Unsubscribe<DrugSearchComplete>(_searchcomplete);
                            _messenger.Unsubscribe<DrugSearchByDINError>(_searcherror);

                            ShowViewModel<DrugLookupByNameSearchResultsViewModel, DrugLookupByNameSearchResultsViewModelParameters>(new DrugLookupByNameSearchResultsViewModelParameters(SelectedParticipant.PlanMemberID));
                        });

                        _searcherror = _messenger.Subscribe<DrugSearchByDINError>((message) =>
                        {
                            _messenger.Unsubscribe<DrugSearchByDINError>(_searcherror);
                            _messenger.Unsubscribe<DrugSearchComplete>(_searchcomplete);
                            RaiseOnNoResults(new EventArgs());
                            ErrorInSearch = true;
                            Searching = false;
                        });

                        var validator = new DINValidator();
                        var result = validator.Validate<DrugLookupByDINViewModel>(this, "DIN");
                        if (!result.IsValid)
                        {
                            _messenger.Unsubscribe<DrugSearchComplete>(_searchcomplete);
                            _messenger.Unsubscribe<DrugSearchByDINError>(_searcherror);
                            foreach (var failure in result.Errors)
                            {
                                switch (failure.ErrorMessage)
                                {
                                    case "Empty":
                                        SearchErrorMessage = Resource.DINMissingError;
                                        RaiseOnMissingDIN(new EventArgs());
                                        break;
                                    case "Not Numbers":
                                        SearchErrorMessage = Resource.DinFormatError;
                                        RaiseOnInvalidDIN(new EventArgs());
                                        break;
                                }
                            }
                            ErrorInSearch = true;
                            Searching = false;
                            return;
                        }
                        _lookupservice.GetByDIN(System.Convert.ToInt32(this.DIN));
                    }, () => !Searching);
                }
                return _searchAndNavigateCommand;
            }
        }

        private void Unsubscribe()
        {
            _messenger.Unsubscribe<DrugParticipantSelected>(_participantselected);
        }
    }
}
