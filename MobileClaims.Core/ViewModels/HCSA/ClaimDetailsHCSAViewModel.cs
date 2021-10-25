using MobileClaims.Core.Services;
using MobileClaims.Core.Services.HCSA;
using System;
using System.Linq;
using MobileClaims.Core.Entities.HCSA;
using System.ComponentModel;
using System.Windows.Input;
using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using MobileClaims.Core.Messages;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.File;
using MvvmCross.Plugin.Messenger;
using System.Threading.Tasks;
using MobileClaims.Core.Validators;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels.HCSA
{
    public class ClaimDetailsHCSAViewModel : HCSAViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private IMvxFileStore _filesystem;
        private readonly IHCSAClaimService _claimservice;
        private ILoginService _loginservice;
        private readonly IParticipantService _participantservice;
        private ILoggerService _logger;
        private readonly MvxSubscriptionToken _typeorexpensehaschanged;
        private bool editmode = false;
        private MvxSubscriptionToken _claimparticipantselected;
        private ClaimDetail _fallback;

        public ClaimDetailsHCSAViewModel(IMvxMessenger messenger,
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
            _claimparticipantselected = _messenger.Subscribe<ClaimParticipantSelected>((message) =>
            {
                ClaimParticipantHasBeenSelected = message.SelectedParticipant != null;
            });
            ClaimParticipantHasBeenSelected = _participantservice.SelectedParticipant != null;
            _typeorexpensehaschanged = _messenger.Subscribe<Messages.HCSAClaimOrExpenseTypeHasChangedMessage>((message) =>
            {
                _messenger.Unsubscribe<Messages.HCSAClaimOrExpenseTypeHasChangedMessage>(_typeorexpensehaschanged);
                Close(this);
            });
            if(!string.IsNullOrEmpty(_claimservice.Claim.ParticipantNumber))
            {
                ClaimParticipantHasBeenSelected = true;
            }

			if(TreatmentDetailsCount > 0)
				_messenger.Publish<OnTreatmentDetailsListViewModelMessage>(new OnTreatmentDetailsListViewModelMessage(this));
        }
        protected override void Unsubscribe()
        {
            base.Unsubscribe();
            _messenger.Unsubscribe<ClaimParticipantSelected>(_claimparticipantselected);
        }
        public void Init(NavigationHelper navhelper)
        {
            if (navhelper != null && !string.IsNullOrEmpty(navhelper.claimDetailAsJSON))
            {
                ClaimDetail requestedClaimDetail = JsonConvert.DeserializeObject<ClaimDetail>(navhelper.claimDetailAsJSON);
                var qry = from ClaimDetail cd in _claimservice.Claim.Details
                          where cd.ClaimAmount == requestedClaimDetail.ClaimAmount &&
                                cd.ExpenseDate == requestedClaimDetail.ExpenseDate &&
                                cd.OtherPaidAmount == requestedClaimDetail.OtherPaidAmount
                          select cd;
                if (qry.FirstOrDefault() != null)
                {
                    this.ClaimDetails = qry.FirstOrDefault();
                }
                editmode = true;
                _fallback = new ClaimDetail()
                {
                    ClaimAmount = ClaimDetails.ClaimAmount,
                    ExpenseDate = ClaimDetails.ExpenseDate,
                    OtherPaidAmount = ClaimDetails.OtherPaidAmount,
                    ParentClaim = ClaimDetails.ParentClaim,
                    Selected = ClaimDetails.Selected
                };
            }
            else
            {
                editmode = false;
            }
            RaisePropertyChanged(() => IsEditing);
            NotifyCommands();
            if (ClaimDetails == null) ClaimDetails = new ClaimDetail();
            ClaimDetails.ParentClaim = _claimservice.Claim;
            if(_claimservice.Claim.ClaimType==null)
            {
                _claimservice.Claim.ClaimType = _claimservice.SelectedClaimType;
            }
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            if (_claimservice.Claim != null && _claimservice.Claim.Details != null)
            {
                var qry = from ClaimDetail cd in _claimservice.Claim.Details
                          where cd.Selected
                          select cd;
                if (qry.FirstOrDefault() != null)
                {
                    this.ClaimDetails = qry.FirstOrDefault();
                }
            }
            if (ClaimDetails == null)
            {
                ClaimDetails = new ClaimDetail();
                ClaimDetails.PropertyChanged += OnPropertyChanged;
            }
            if((ClaimDetails.ExpenseDate == null) || (ClaimDetails.ExpenseDate == DateTime.MinValue))
            {
                if(Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice==GSCHelper.OS.iOS)
                {
                    ClaimDetails.ExpenseDate = DateTime.Now;
                }
            }

        }

        public bool IsEditing => editmode;

        private bool _claimparticipanthasbeenselected;
        public bool ClaimParticipantHasBeenSelected
        {
            get => _claimparticipanthasbeenselected;
            set
            {
                _claimparticipanthasbeenselected = value;
                RaisePropertyChanged(() => ClaimParticipantHasBeenSelected);
            }
        }


        private bool _isinvalidclaimamount = false;

        public bool IsInvalidClaimAmount
        {
            get => _isinvalidclaimamount;
            set
            {
                if (_isinvalidclaimamount != value)
                {
                    _isinvalidclaimamount = value;
                    RaisePropertyChanged(() => IsInvalidClaimAmount);
                }
            }
        }


        private string _claimamountvalidationmessage;
        public string ClaimAmountValidationMesssage
        {
            get => _claimamountvalidationmessage;
            set
            {
                _claimamountvalidationmessage = value;
                RaisePropertyChanged(() => ClaimAmountValidationMesssage);
            }
        }

        private bool _isinvalidotherpaidamount=false;
        public bool IsInvalidOtherPaidAmount
        {
            get => _isinvalidotherpaidamount;
            set
            {
                _isinvalidotherpaidamount = value;
                RaisePropertyChanged(() => IsInvalidOtherPaidAmount);
            }
        }


        private string _otherpaidamountvalidationmessage;
        public string OtherPaidAmountValidationMessage
        {
            get => _otherpaidamountvalidationmessage;
            set
            {
                _otherpaidamountvalidationmessage = value;
                RaisePropertyChanged(() => OtherPaidAmountValidationMessage);
            }
        }


        private bool _isinvalidexpensedate = false;
        public bool IsInvalidExpenseDate
        {
            get => _isinvalidexpensedate;
            set
            {
                _isinvalidexpensedate = value;
                RaisePropertyChanged(() => IsInvalidExpenseDate);
            }
        }

        private string _expensedatevalidationmessage;
        public string ExpenseDateValidationMessage
        {
            get => _expensedatevalidationmessage;
            set
            {
                _expensedatevalidationmessage = value;
                RaisePropertyChanged(() => ExpenseDateValidationMessage);
            }
        }

        private ClaimDetail _claimdetails;
        public ClaimDetail ClaimDetails
        {
            get => _claimdetails;
            set
            {
                _claimdetails = value;
                RaisePropertyChanged(() => ClaimDetails);
            }
        }

        public int NumberOfClaimDetails
        {
            get
            {
                if (_claimservice.Claim != null && _claimservice.Claim.Details != null)
                    return _claimservice.Claim.Details.Count;
                else
                    return 0;
            }
        }

        public string DateOfExpenseLabel => Resource.DateOfExpenseNoColon;

        public string TotalAmountClaimedLabel => Resource.TotalAmountClaimed;

        public string AmountPreviouslyPaidLabel => Resource.AmountPreviouslyPaid;

        public string SaveLabel => Resource.Save;

        MvxCommand _CancelCommand;
        public System.Windows.Input.ICommand CancelCommand
        {
            get
            {
                _CancelCommand = _CancelCommand ?? new MvxCommand(() =>
                                                              {
                                                                  ClaimDetails.ClaimAmount = _fallback.ClaimAmount;
                                                                  ClaimDetails.ExpenseDate = _fallback.ExpenseDate;
                                                                  ClaimDetails.OtherPaidAmount = _fallback.OtherPaidAmount;
                                                                  ClaimDetails.ParentClaim = _fallback.ParentClaim;
                                                                  ClaimDetails.Selected = _fallback.Selected;
                                                                  SaveClaimDetailsCommand.Execute(null);
                                                            },
                                                          () =>
                                                              {
                                                                  return true; //Placeholder - replace with CanExecute logic
                                                      }
                                                         );
                return _CancelCommand;
            }
        }


        MvxCommand _saveclaimdetailscommand;
        public ICommand SaveClaimDetailsCommand
        {
            get
            {
                _saveclaimdetailscommand = _saveclaimdetailscommand ?? new MvxCommand(() =>
                                                               {
                                                                   IsInvalidOtherPaidAmount = false;
                                                                   OtherPaidAmountValidationMessage = "";
                                                                   IsInvalidExpenseDate = false;
                                                                   ExpenseDateValidationMessage = "";
                                                                   IsInvalidClaimAmount = false;
                                                                   ClaimAmountValidationMesssage = "";
                                                                   if (ValidateClaimDetail())
                                                                   {
                                                                       Unsubscribe();
                                                                       PublishMessages();
                                                                       if (!IsEditing)
                                                                       {
                                                                           if(ClaimDetails.ParentClaim==null)
                                                                           {
                                                                               ClaimDetails.ParentClaim = _claimservice.Claim;
                                                                           }
                                                                           _claimservice.Claim.Details.Add(ClaimDetails);
                                                                           _claimservice.PersistClaim();
                                                                           if (_claimservice.Claim.Details.Count == 1)  //added by Suhail
                                                                           {
                                                                               RaiseClaimTreatmentEntrySuccess(new EventArgs());
                                                                               _claimservice.HaveClaimDetailsAlreadyBeenInitialized=true;
                                                                               Close(this);
                                                                               ShowViewModel<ClaimReviewAndEditViewModel>(); //came from an empty claim, so nav to ReviewAndEdit
                                                                           }
                                                                           else
                                                                           {
                                                                               _claimservice.PersistClaim();
                                                                               RaiseClaimTreatmentEntrySuccess(new EventArgs());
                                                                               Close(this);
                                                                           }
                                                                       }
                                                                       else
                                                                       {
                                                                           RaiseClaimTreatmentEntrySuccess(new EventArgs());
                                                                           Close(this); //Editing an existing detail so we should close
                                                                       }
                                                                   }
                                                               },
                                                          () =>
                                                              {
                                                                 
                                                                  return ClaimParticipantHasBeenSelected;
                                                              }
                                                         );
                return _saveclaimdetailscommand;
            }
        }


        MvxCommand _DeleteCommand;
        public MvxCommand DeleteCommand
        {
            get
            {
                _DeleteCommand = _DeleteCommand ?? new MvxCommand(() =>
                                                    {
                                                        RaiseClaimTreatmentEntrySuccess(new EventArgs());
                                                        _claimservice.Claim.Details.Remove(this.ClaimDetails);
                                                        _claimservice.PersistClaim();
                                                                  
                                                        if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice == GSCHelper.OS.iOS)
                                                        {
                                                            if (_claimservice.Claim.Details.Count == 0)
                                                            {
                                                                var rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
                                                                if (rehydrationservice.BusinessProcess.Contains(typeof(ClaimConfirmationHCSAViewModel)))
                                                                {
                                                                    rehydrationservice.BusinessProcess.Remove(typeof(ClaimConfirmationHCSAViewModel));
                                                                    rehydrationservice.Save();
                                                                }
                                                            }
                                                            Close(this);
                                                            return;
                                                        }
                                                        else
                                                        {
                                                            if (_claimservice.Claim.Details.Count > 0)
                                                            {
                                                                Close(this);
                                                            }
                                                            else
                                                            {
                                                                var rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
                                                                if (rehydrationservice.BusinessProcess.Contains(typeof(ClaimConfirmationHCSAViewModel)))
                                                                {
                                                                    rehydrationservice.BusinessProcess.Remove(typeof(ClaimConfirmationHCSAViewModel));
                                                                    rehydrationservice.Save();
                                                                }

                                                                Close(this);
                                                                RaiseClaimTreatmentEntrySuccess(new EventArgs()); // added by Suhail
                                                                ShowViewModel<ClaimDetailsHCSAViewModel>();
                                                            }
                                                        }
                                                    },
                                                    () => { return editmode; }
                                                );
                return _DeleteCommand;
            }
        }


        private bool disposedValue = false;
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ClaimDetails.PropertyChanged -= OnPropertyChanged;
                }
            }
            base.Dispose(disposing);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyCommands();
        }

        private bool ValidateClaimDetail()
        {
            var validator = new ClaimDetailValidator
            {
                CascadeMode = CascadeMode.Continue
            };
            var result = new ValidationResult();
            result = validator.Validate<ClaimDetail>(ClaimDetails, "ClaimAmount", "ExpenseDate", "OtherPaidAmount", "ClaimAmountString", "OtherPaidAmountString");
            foreach (var failure in result.Errors)
            {
                switch (failure.PropertyName)
                {
                    case "ClaimAmountString":
                        {
                            IsInvalidClaimAmount = true;
                            ClaimAmountValidationMesssage = failure.ErrorMessage;
                            break;
                        }
                    case "OtherPaidAmountString":
                        {
                            IsInvalidOtherPaidAmount = true;
                            OtherPaidAmountValidationMessage = failure.ErrorMessage;
                            break;
                        }
                    case "ClaimAmount":
                        {
                            IsInvalidClaimAmount = true;
                            ClaimAmountValidationMesssage = failure.ErrorMessage;
                            //Set ClaimAmountValidationMessage to localized string
                            break;
                        }
                    case "ExpenseDate":
                        {
                            IsInvalidExpenseDate = true;
                            ExpenseDateValidationMessage = failure.ErrorMessage;
                            break;
                        }
                    case "OtherPaidAmount":
                        {
                            IsInvalidOtherPaidAmount = true;
                            OtherPaidAmountValidationMessage = failure.ErrorMessage;
                            break;
                        }
                }
            }
            return result.IsValid;
        }

        public event EventHandler ClaimTreatmentEntrySuccess;
        protected virtual void RaiseClaimTreatmentEntrySuccess(EventArgs e)
        {
            if (this.ClaimTreatmentEntrySuccess != null)
            {
                ClaimTreatmentEntrySuccess(this, e);
            }
        }

        public class NavigationHelper
        {
            public string claimDetailAsJSON { get; set; }
        }

        public string ClaimDetailsTitle => Resource.HCSAClaimDetailsTitle;

        public string EnterDetailLabel => Resource.ClaimDetailsHCSALabel;

        public string ErrorMessageLabel => Resource.error_message;


        public int TreatmentDetailsCount => _claimservice.Claim.Details.Count;
    }
}
