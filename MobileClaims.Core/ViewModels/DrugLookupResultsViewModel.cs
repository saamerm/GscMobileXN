using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.Requests;
using System;
using System.Linq;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MobileClaims.Core.ViewModelParameters;
using System.Threading.Tasks;

namespace MobileClaims.Core.ViewModels
{
    public class DrugLookupResultsViewModel : ViewModelBase<DrugLookupResultsViewModelParameters>
    {
        private readonly IMvxMessenger _messenger;
        private readonly IParticipantService _participantservice;
        private readonly IDrugLookupService _drugservice;
        private readonly MvxSubscriptionToken _searchcomplete;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private readonly MvxSubscriptionToken _specialauthcomplete;
        private readonly MvxSubscriptionToken _specialautherrortoken;
        private readonly MvxSubscriptionToken _emailcomplete;
        private readonly MvxSubscriptionToken _emailerror;
        private object _sync = new object();

        public DrugLookupResultsViewModel(IMvxMessenger messenger, IParticipantService participantservice, IDrugLookupService drugservice)
        {
            _messenger = messenger;
            _participantservice = participantservice;
            _drugservice = drugservice;

            _searchcomplete = _messenger.Subscribe<CheckDrugEligibilityComplete>((message) =>
            {
                _messenger.Unsubscribe<CheckDrugEligibilityComplete>(_searchcomplete);
               
                this.Drug = _drugservice.EligibilityResult;
                this.Reimbursable = !string.IsNullOrEmpty(this.Drug.Reimbursement);
                this.LowCostReplacementOccurred = this.Drug.LowCostReplacementOccurred;
				Busy = false;
            });

            _shouldcloseself = _messenger.Subscribe<ClearDrugSearchResultsRequested>((message) =>
            {
                _messenger.Unsubscribe<ClearDrugSearchResultsRequested>(_shouldcloseself);
                _messenger.Unsubscribe<CheckDrugEligibilityComplete>(_searchcomplete);
                Unsubscribe();
                Close(this);
            });

            _specialauthcomplete = _messenger.Subscribe<GetSpecialAuthorizationFormComplete>((message) =>
            {
                PathToSpecialAuthForm = _drugservice.SpecialAuthorizationFormPath;
                RaiseFetchSpecialAuthComplete(new EventArgs());
            });

            _specialautherrortoken = _messenger.Subscribe<GetSpecialAuthorizationFormError>((message) =>
            {
                RaiseErrorFetchingSpecialAuth(new EventArgs());
            });

            _emailcomplete = _messenger.Subscribe<EmailSpecialAuthorizationFormComplete>((message) =>
            {
                RaiseEmailComplete(new EventArgs());
            });

            _emailerror = _messenger.Subscribe<EmailSpecialAuthorizationFormError>((message) =>
            {
                RaiseEmailError(new EventArgs());
            });
        }

        private string _pathtospecialauthform;
        public string PathToSpecialAuthForm
        {
            get
            {
                return _pathtospecialauthform;
            }
            set
            {
                if (_pathtospecialauthform != value)
                {
                    _pathtospecialauthform = value;
                    RaisePropertyChanged(() => PathToSpecialAuthForm);
                }
            }
        }

        private bool _sendspecialauthrequested;
        public bool SendSpecialAuthRequested
        {
            get { return _sendspecialauthrequested; }
            set
            {
                if (_sendspecialauthrequested != value)
                {
                    _sendspecialauthrequested = value;
                    RaisePropertyChanged(() => SendSpecialAuthRequested);
                };
            }
        }

        private string _specialautherror;
        public string SpecialAuthError
        {
            get { return _specialautherror; }
            set
            {
                if (_specialautherror != value)
                {
                    _specialautherror = value;
                    RaisePropertyChanged(() => SpecialAuthError);
                };
            }
        }

        private bool _sentspecialauth;
        public bool SentSpecialAuth
        {
            get { return _sentspecialauth; }
            set
            {
                if (_sentspecialauth != value)
                {
                    _sentspecialauth = value;
                    RaisePropertyChanged(() => SentSpecialAuth);
                };
            }
        }

        private string _specialauthemail;
        public string SpecialAuthEMail
        {
            get { return _specialauthemail; }
            set
            {
                if (_specialauthemail != value)
                {
                    _specialauthemail = value;
                    RaisePropertyChanged(() => SpecialAuthEMail);
                };
            }
        }

		private bool _busy = true;
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

        //Used to determine if we need to hide the title or not
        private bool _reimbursable;
        public bool Reimbursable
        {
            get { return _reimbursable; }
            set
            {
                if (_reimbursable != value)
                {
                    _reimbursable = value;
                    RaisePropertyChanged(() => Reimbursable);
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

        private bool _lowCostReplacementOccurred;
        public bool LowCostReplacementOccurred
        {
            get
            {
                return _lowCostReplacementOccurred;
            }
            set
            {
                _lowCostReplacementOccurred = value;
                RaisePropertyChanged(() => LowCostReplacementOccurred);
            }
        }

        public event EventHandler ErrorFetchingSpecialAuth;
        protected virtual void RaiseErrorFetchingSpecialAuth(EventArgs e)
        {
            if (this.ErrorFetchingSpecialAuth!= null)
            {
                ErrorFetchingSpecialAuth(this, e);
            }
        }

        public event EventHandler FetchSpecialAuthComplete;
        protected virtual void RaiseFetchSpecialAuthComplete(EventArgs e)
        {
            if (this.FetchSpecialAuthComplete!= null)
            {
                FetchSpecialAuthComplete(this, e);
            }
        }

        public event EventHandler OnEmailError;
        protected virtual void RaiseEmailError(EventArgs e)
        {
            if (this.OnEmailError != null)
            {
                OnEmailError(this, e);
            }
        }

        public event EventHandler OnEmailComplete;
        protected virtual void RaiseEmailComplete(EventArgs e)
        {
            if (this.OnEmailComplete != null)
            {
                OnEmailComplete(this, e);
            }
        }

        public ICommand NewSearchCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _messenger.Publish<Messages.ClearDrugLookupModelSelectionRequested>(new ClearDrugLookupModelSelectionRequested(this));
                    _messenger.Publish<Messages.ClearDrugSearchByDINRequested>(new ClearDrugSearchByDINRequested(this));
                    _messenger.Publish<Messages.ClearSearchByDrugNameRequested>(new ClearSearchByDrugNameRequested(this));
                    _messenger.Publish<Messages.ClearDrugSearchByNameResultsRequested>(new ClearDrugSearchByNameResultsRequested(this));
                    _messenger.Publish<Messages.ClearDrugSearchResultsRequested>(new ClearDrugSearchResultsRequested(this));
                    this.ShowViewModel<DrugLookupModelSelectionViewModel>();
                });
            }
        }


        public ICommand DownloadSpecialAuthorizationCommand
        {
            get
            {
                return new MvxCommand(async() =>
                {
                    await _drugservice.GetSpecialAuthorizationForm(this.Drug.SpecialAuthFormName);
                }
                //, () =>
                //{
                //    return this.Drug != null && !string.IsNullOrEmpty(this.Drug.SpecialAuthFormName);
                //}
                );
            }
        }

        public ICommand SendSpecialAuthorizationCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    SendSpecialAuthRequested = true;
                }
                //, () =>
                //{
                //    return !string.IsNullOrEmpty(this.Drug.SpecialAuthFormName)
                //            && !string.IsNullOrEmpty(this.Drug.SpecialAuthFormName);
                //}
                );
            }
        }

		public ICommand DownloadSpecialAuthorizationDroidCommand
		{
			get
			{
				return new MvxCommand(() =>
					{
						_drugservice.GetSpecialAuthorizationForm(this.Drug.SpecialAuthFormName);
					}
				);
			}
		}

		public ICommand SendSpecialAuthorizationDroidCommand
		{
			get
			{
				return new MvxCommand(() =>
					{
						SendSpecialAuthRequested = true;
					}
				);
			}
		}
        public ICommand SendSpecialAuthorizationDroidCommandCancel
        {
            get
            {
                return new MvxCommand(() =>
                    {
                        SendSpecialAuthRequested = false;
                    }
                );
            }
        }
        public ICommand ExecuteSendSpecialAuthorizationCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    EmailRequest er = new EmailRequest()
                    {
                        RecipientAddress = this.SpecialAuthEMail,
                        SenderAddress = "",
                        SenderName = "",
                        Subject = "",
                        Body = ""
                    };
					SendSpecialAuthRequested = false;
                    _drugservice.EmailSpecialAuthorizationForm(this.Drug.SpecialAuthFormName, er);
                }, () =>
                {
					SendSpecialAuthRequested = false;
                    return !string.IsNullOrEmpty(Drug.SpecialAuthFormName)
                        && !string.IsNullOrEmpty(SpecialAuthEMail);
                });
            }

        }

        public ICommand ShowFindPharmaciesProviderCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    lock (_sync)
                    {
                        Unsubscribe();
                        ShowViewModel<FindHealthProviderViewModel, FindHealthProviderViewModelParameter>(new FindHealthProviderViewModelParameter(ProvidersId.Pharmacy));
                    }
                });
            }
        }

        private void Unsubscribe()
        {
            _messenger.Unsubscribe<EmailSpecialAuthorizationFormComplete>(_emailcomplete);
            _messenger.Unsubscribe<GetSpecialAuthorizationFormComplete>(_specialauthcomplete);
            _messenger.Unsubscribe<EmailSpecialAuthorizationFormError>(_emailerror);
            _messenger.Unsubscribe<GetSpecialAuthorizationFormError>(_specialautherrortoken);
        }

        public override void Prepare(DrugLookupResultsViewModelParameters parameter)
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

            if (_drugservice.SearchResult.Where(d => d.DIN == parameter.DrugID).FirstOrDefault() != null)
            {
                Drug = _drugservice.SearchResult.Where(d => d.DIN == parameter.DrugID).FirstOrDefault();
                Reimbursable = !string.IsNullOrEmpty(Drug.Reimbursement);
            }
            else
            {
                if (_drugservice.SearchByDINResult != null && _drugservice.SearchByDINResult.DIN == parameter.DrugID)
                {
                    Drug = _drugservice.SearchByDINResult;
                    Reimbursable = !string.IsNullOrEmpty(Drug.Reimbursement);
                }
            }
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            Busy = true;
            await _drugservice.CheckEligibility(Participant, Drug);
        }
    }
}
