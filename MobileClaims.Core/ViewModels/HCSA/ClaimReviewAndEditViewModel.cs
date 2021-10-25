using FluentValidation;
using MobileClaims.Core.Entities.HCSA;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.HCSA;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.File;
using MvvmCross.Plugin.Messenger;
using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels.HCSA
{
    public class ClaimReviewAndEditViewModel : HCSAViewModelBase
    {
        private IMvxMessenger _messenger;
        private IMvxFileStore _filesystem;
        private IHCSAClaimService _claimservice;
        private ILoginService _loginservice;
        private IParticipantService _participantservice;
        private ILoggerService _logger;
        private MvxSubscriptionToken _typeorexpensehaschanged;
        private readonly MvxSubscriptionToken _participantselected;
        private readonly MvxSubscriptionToken _shouldcloseself;

        public ClaimReviewAndEditViewModel(IMvxMessenger messenger,
                                     IMvxFileStore filesystem,
                                     IHCSAClaimService claimservice,
                                     ILoginService loginservice,
                                     IParticipantService participantservice,
                                     ILoggerService logger)
        {
            _messenger = messenger;
            _filesystem = filesystem;
            _claimservice = claimservice;
            _loginservice = loginservice;
            _participantservice = participantservice;
            _logger = logger;

            ReferralTypes = JsonConvert.DeserializeObject<ObservableCollection<HCSAReferralType>>(Resource.HCSAReferralTypes);
            Claim = claimservice.Claim;
            _claimservice.Claim.Details.CollectionChanged += Details_CollectionChanged;
            FiveTreatmentDetails = (Claim.Details .Count == 5);

            _participantselected = _messenger.Subscribe<ClaimParticipantSelected>((message) =>
            {  
                var rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
                    if (!rehydrationservice.Rehydrating && Participant != null && Claim.Details == null || Claim.Details.Count==0) //participant is being changed
                {
                    Claim.Details = new ObservableCollection<ClaimDetail>();
                    _messenger.Publish<Messages.ClearClaimDetailsViewRequested>(new MobileClaims.Core.Messages.ClearClaimDetailsViewRequested(this));
                }
                else
                {
                    //rehydrationservice.Rehydrating = false;
                }
                Participant = _participantservice.SelectedParticipant;
            });

            _shouldcloseself = _messenger.Subscribe<ClearClaimDetailsViewRequested>((message) =>
            {
                _messenger.Unsubscribe<ClearClaimDetailsViewRequested>(_shouldcloseself);
                _messenger.Unsubscribe<ClaimParticipantSelected>(_participantselected);
                Close(this);
            });

            _typeorexpensehaschanged = _messenger.Subscribe<Messages.HCSAClaimOrExpenseTypeHasChangedMessage>((message) =>
            {
                _messenger.Unsubscribe<Messages.HCSAClaimOrExpenseTypeHasChangedMessage>(_typeorexpensehaschanged);
                Close(this);
            });
			var rehydrationservice1 = Mvx.IoCProvider.Resolve<IRehydrationService>();
		//	if (!rehydrationservice1.Rehydrating && Participant != null) //participant is being changed
//            	_messenger.Publish<OnTreatmentDetailsListViewModelMessage>(new OnTreatmentDetailsListViewModelMessage(this));
            
        }

        private void Details_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaiseTreatmentDetailsChanged(new EventArgs());
            NotifyCommands();
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            this.Claim = _claimservice.Claim;
            this.IsReferralQuestionVisible = _claimservice.SelectedExpenseTypeRequiresReferral;
            this.Participant = _participantservice.SelectedParticipant;
        }

        private Claim _claim;
        public Claim Claim
        {
            get { return _claim; }
            set
            {
                if (_claim != value)
                {
                    _claim = value;
                    RaisePropertyChanged(() => Claim);
                }
            }
        }

        public string ExpenseType
        {
            get
            {
                if (Claim.ExpenseType != null && Claim.ExpenseType.ID !=0)
                {
                    return Claim.ExpenseType.Name;
                }
                else
                {
                    return Claim.ClaimType.Name;
                }
            }
        }

        private bool _isaddbuttonvisible;
        public bool IsAddButtonVisible
        {
            get
            {
                return _isaddbuttonvisible;
            }
            set
            {
                _isaddbuttonvisible = value;
                RaisePropertyChanged(() => IsAddButtonVisible);
            }
        }

        private bool _isinvalidreferral;
        public bool IsInvalidReferral
        {
            get
            {
                return _isinvalidreferral;
            }
            set
            {
                _isinvalidreferral = value;
                RaisePropertyChanged(() => IsInvalidReferral);
            }
        }

        private string _referralvalidationerror;
        public string ReferralValidationError
        {
            get
            {
                return _referralvalidationerror;
            }
            set
            {
                _referralvalidationerror = value;
                RaisePropertyChanged(() => ReferralValidationError);
            }
        }

        private bool _isinvalidclaimscount;
        public bool IsInvalidClaimsCount
        {
            get
            {
                return _isinvalidclaimscount;
            }
            set
            {
                _isinvalidclaimscount = value;
                RaisePropertyChanged(() => IsInvalidClaimsCount);
            }
        }

        private string _invalidclaimscounterrormessage;
        public string InvalidClaimsCountErrorMessage
        {
            get
            {
                return _invalidclaimscounterrormessage;
            }
            set
            {
                _invalidclaimscounterrormessage = value;
                RaisePropertyChanged(() => InvalidClaimsCountErrorMessage);
            }
        }

        private bool _isreferralquestionvisible;
        public bool IsReferralQuestionVisible
        {
            get
            {
                return _isreferralquestionvisible;
            }
            set
            {
                _isreferralquestionvisible = value;
                RaisePropertyChanged(() => IsReferralQuestionVisible);
            }
        }

        private ObservableCollection<HCSAReferralType> _referraltypes;
        public ObservableCollection<HCSAReferralType> ReferralTypes
        {
            get
            {
                return _referraltypes;
            }
            set
            {
                _referraltypes = value;
                if (_referraltypes.First() != null)
                {
                    _referraltypes.Insert(0, null);
                }
                RaisePropertyChanged(() => ReferralTypes);
            }
        }

        private HCSAReferralType _selectedreferraltype;
        public HCSAReferralType SelectedReferralType
        {
            get
            {
                return _selectedreferraltype;
            }
            set
            {
                _selectedreferraltype = value;
                _claimservice.Claim.MedicalProfessionalID = _selectedreferraltype.Code;
                RaisePropertyChanged(() => SelectedReferralType);
            }
        }


        private Entities.Participant _participant;
        public Entities.Participant Participant
        {
            get
            {
                return _participant;
            }
            set
            {
                _participant = value;
                RaisePropertyChanged(() => Participant);
            }
        }

        private bool _fiveTreatmentDetails = false;
        public bool FiveTreatmentDetails
        {
            get
            {
                return _fiveTreatmentDetails;
            }
            set
            {
                _fiveTreatmentDetails = value;
                RaisePropertyChanged(() => FiveTreatmentDetails);
            }
        }

        public string ClaimedAmountLabel
        {
            get
            {
                return Resource.ClaimedAmount;
            }
        }

        public string OtherPaidAmountLabel
        {
            get
            {
                return Resource.OtherPaidAmount;
            }
        }

        public string TitleLabel
        {
            get
            {
                return Resource.claimConfirm_detais;
            }
        }

        MvxCommand<int> _RemoveWithSwipeCommand;
        public MvxCommand<int> RemoveWithSwipeCommand
        {
            get
            {
                _RemoveWithSwipeCommand = _RemoveWithSwipeCommand ?? new MvxCommand<int>((rowId) =>
                                                              {
                                                                  var detail = Claim.Details[rowId];
                                                                  _removecommand.Execute(detail);
                                                              }
                                                         );
                return _RemoveWithSwipeCommand;
            }
        }


        private MvxCommand<ClaimDetail> _removecommand;
        public override ICommand RemoveCommand
        {
            get
            {
                _removecommand = _removecommand ?? new MvxCommand<ClaimDetail>((cd) =>
                                                    {
                                                        this.Claim.Details.Remove(cd);
                                                        try
                                                        {
                                                            _claimservice.Claim.Details.Remove(cd);
                                                            FiveTreatmentDetails = (Claim.Details.Count == 5);
                                                            _addcommand.RaiseCanExecuteChanged();
                                                        }
                                                        catch { }
                                                        _claimservice.PersistClaim();
                                                        if (_claimservice.Claim.Details.Count == 0)
                                                        {
                                                            var rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
                                                            if (rehydrationservice.BusinessProcess.Contains(typeof(ClaimConfirmationHCSAViewModel)))
                                                            {
                                                                rehydrationservice.BusinessProcess.Remove(typeof(ClaimConfirmationHCSAViewModel));
                                                                rehydrationservice.Save();
                                                            }

                                                            RaiseCloseReviewAndEditViewModel(new EventArgs());
                                                            Close(this);
                                                            ShowViewModel<ClaimDetailsHCSAViewModel>();
                                                        }
    
                                                    },
                                                (cd) =>
                                                    {
                                                        return cd != null;
                                                    });
                return _removecommand;
            }
        }

        MvxCommand _addcommand;
        public ICommand AddCommand
        {
            get
            {
                _addcommand = _addcommand ?? new MvxCommand(() =>
                                                              {
                                                                  ShowViewModel<ClaimDetailsHCSAViewModel>();
                                                              },
                                                          () =>
                                                              {
                                                                  bool canExecute = _claimservice.Claim.Details.Count < 5;
                                                                  IsAddButtonVisible = canExecute;
                                                                  return canExecute; //Placeholder - replace with CanExecute logic
                                                              }
                                                         );
                return _addcommand;
            }
        }


        MvxCommand<Entities.HCSA.ClaimDetail> _EditCommand;
        public MvxCommand<Entities.HCSA.ClaimDetail> EditCommand
        {
            get
            {
                _EditCommand = _EditCommand ?? new MvxCommand<Entities.HCSA.ClaimDetail>((cd) =>
                                                              {
                                                                  ClaimDetailsHCSAViewModel.NavigationHelper navHelper = new ClaimDetailsHCSAViewModel.NavigationHelper()
                                                                  {
                                                                      claimDetailAsJSON = JsonConvert.SerializeObject(cd)
                                                                  };
                                                                  //ShowViewModel<ClaimDetailsHCSAViewModel>(navHelper, null, MvxRequestedBy.UserAction);
                                                              }
                                                         );
                return _EditCommand;
            }
        }


        MvxCommand _confirmclaimsummarycommand;
        public System.Windows.Input.ICommand ConfirmClaimSummaryCommand
        {
            get
            {
                _confirmclaimsummarycommand = _confirmclaimsummarycommand ?? new MvxCommand(() =>
                { 
                    if (Claim.Details.Count == 0)
                    {
                        RaiseInvalidClaim(new EventArgs());                 
                    }
                    else
                    {
                        if (Validate())
                        {
                            Unsubscribe();
                            PublishMessages();
                            ShowViewModel<ClaimSubmitTermsAndConditionsViewModel>();
                        }
                    }
                },
                    () =>
                    {
                        return Claim.Details.Count >= 0;
                        return true; //Placeholder - replace with CanExecute logic
                    }
                );
                return _confirmclaimsummarycommand;
            }
        }

        MvxCommand _gobackcommand;
        public override ICommand GoBackCommand
        {
            get
            {
                _gobackcommand = _gobackcommand ?? new MvxCommand(() =>
                                                              {
                                                                  //special case -> we never want to go back to the Claim Detail VM
                                                                  Unsubscribe();
                                                                  PublishMessages();
                                                                  ShowViewModel<ClaimParticipantsViewModel>();
                                                              },
                                                          () =>
                                                              {
                                                                  return true; //Placeholder - replace with CanExecute logic
                                                              }
                                                         );
                return _gobackcommand;
            }
        }

        public event EventHandler OnInvalidClaim;
        protected virtual void RaiseInvalidClaim(EventArgs e)
        {
            if (this.OnInvalidClaim != null)
            {
                OnInvalidClaim(this, e);
            }
        }

        public event EventHandler CloseReviewAndEditViewModel;
        protected virtual void RaiseCloseReviewAndEditViewModel(EventArgs e)
        {
            if (this.CloseReviewAndEditViewModel != null)
            {
                CloseReviewAndEditViewModel(this, e);
            }
        }

        private bool Validate()
        {
            IsInvalidReferral = false;
            IsInvalidClaimsCount = false;
            ReferralValidationError = "";
            InvalidClaimsCountErrorMessage = "";
            HCSAClaimValidator validator = new HCSAClaimValidator();
            validator.CascadeMode = CascadeMode.Continue;
            var validationResult = validator.Validate(this, "Claim.Details", "Claim.MedicalProfessionalID");
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    switch (failure.PropertyName)
                    {
                        case "Claim.MedicalProfessionalID":
                            {
                                IsInvalidReferral = true;
                                ReferralValidationError = failure.ErrorMessage;
                                break;
                            }
                        case "Claim.Details.Count":
                            {
                                IsInvalidClaimsCount = true;
                                InvalidClaimsCountErrorMessage = failure.ErrorMessage;
                                break;
                            }
                    }
                }
            }

            return validationResult.IsValid;
        }

        public override void Dispose()
        {
            //Don't be a dirt-bag.  Prevent memory leaks before they happen!
            base.Dispose();
            _claimservice.Claim.Details.CollectionChanged -= Details_CollectionChanged;
        }

        public event EventHandler TreatmentDetailsChanged;
        protected virtual void RaiseTreatmentDetailsChanged(EventArgs e)
        {
            if (this.TreatmentDetailsChanged != null)
            {
                TreatmentDetailsChanged(this, e);
            }
        }

        public class HCSAClaimValidator : AbstractValidator<ClaimReviewAndEditViewModel>
        {
            public HCSAClaimValidator()
            {
                RuleFor(vm => vm.Claim.Details).Must(d => d.Count > 0).WithMessage(Resource.NoClaimDetailsEntered).OverridePropertyName("Claim.Details.Count");
                RuleFor(vm => vm.Claim.MedicalProfessionalID).NotEmpty().Unless(vm => !vm.IsReferralQuestionVisible).WithMessage(Resource.ReferralRequired);
                RuleFor(vm => vm.Claim.Details).Must(d => d.Count <= 5).WithMessage(Resource.MoreThanFiveClaims).OverridePropertyName("Claim.Details.Count");
            }
        }


        public string MoreThanFiveClaimsLabel
        {
            get
            {
                return Resource.MoreThanFiveClaims;
            }
        }
    }
}
