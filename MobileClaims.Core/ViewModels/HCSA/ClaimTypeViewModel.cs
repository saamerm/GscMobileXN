using MobileClaims.Core.Entities.HCSA;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.HCSA;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels.HCSA
{
    public class ClaimTypeViewModel : HCSAViewModelBase
    {
        private readonly IHCSAClaimService _claimservice;
        private readonly IMvxMessenger _messenger;
        private readonly ILoginService _loginservice;
        private readonly IDataService _dataservice;
        private readonly ILoggerService _logger;
        private bool _continueprocessingclaimtype;
        private IUserDialogs dlg;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private readonly IMvxLog _log;
        private bool _changedClaimType = false;

        public ClaimTypeViewModel(IHCSAClaimService claimservice,
                                  IMvxMessenger messenger,
                                  ILoginService loginservice,
                                  ILoggerService logger,
                                  IDataService dataservice,
                                  IMvxLog log)
        {
            _claimservice = claimservice;
            _messenger = messenger;
            _loginservice = loginservice;
            _logger = logger;
            _dataservice = dataservice;
            _log = log;
            MedicalProfessionalTypes = new ObservableCollection<HCSAReferralType>(JsonConvert.DeserializeObject<ObservableCollection<HCSAReferralType>>(Resource.HCSAReferralTypes).OrderBy(rt => rt.Text).ToArray<HCSAReferralType>());
            dlg = Mvx.IoCProvider.Resolve<IUserDialogs>();
            //CreatedFromRehydration = Mvx.IoCProvider.Resolve<IRehydrationService>().Rehydrating;

            _shouldcloseself = _messenger.Subscribe<ClearClaimTypeViewRequested>((message) =>
            {
                _messenger.Unsubscribe<ClearClaimTypeViewRequested>(_shouldcloseself);
                Close(this);
            });
        }

        public void Init()
        {
            if ((_claimservice.SelectedClaimType != null && _claimservice.SelectedClaimType.ID > 0)
                || (_claimservice.SelectedExpenseType != null && _claimservice.SelectedExpenseType.ID > 0))
                CreatedFromRehydration = true;

            if (!CreatedFromRehydration)
            {
                _selectedclaimtype = _claimservice.SelectedClaimType;
                _selectedclaimexpensetype = _claimservice.SelectedExpenseType;
                _selectedmedicalprofessionaltype = MedicalProfessionalTypes.Where(mpt => mpt.Code == _claimservice.Claim.MedicalProfessionalID).FirstOrDefault();

                this.IsExpenseTypeDescriptionCollapsed = true;
                this.IsExpenseTypeDescriptionVisible = IsDescriptionVisible;
                this.IsReferralQuestionVisible = SelectedMedicalProfessionalType != null;
            }
            if (_claimservice.SelectedClaimType != null && !CreatedFromRehydration)
            {
                _selectedclaimtype = _claimservice.SelectedClaimType;
                RaisePropertyChanged(() => SelectedClaimType);
            }

            if (CreatedFromRehydration)
            {

                if (ClaimTypes == null || (ClaimTypes != null && ClaimTypes.Count() == 0))
                {
                    _claimtypes = new ObservableCollection<ClaimType>(new ClaimType[] { _claimservice.SelectedClaimType });
                    //RaisePropertyChanged(() => ClaimTypes);
                }
                if (ClaimExpenseTypes == null || (ClaimExpenseTypes != null && ClaimExpenseTypes.Count() == 0))
                {
                    if (_claimservice.ExpenseTypes == null || _claimservice.ExpenseTypes.Count == 0)
                    {
                        _claimexpensetypes = new ObservableCollection<ExpenseType>(new ExpenseType[] { _claimservice.SelectedExpenseType });
                    }
                    else
                    {
                        ClaimExpenseTypes = new ObservableCollection<ExpenseType>(_claimservice.ExpenseTypes.ToArray<ExpenseType>());
                    }
                    _selectedclaimexpensetype = _claimservice.SelectedExpenseType;
                    _selectedmedicalprofessionaltype = MedicalProfessionalTypes.Where(mpt => mpt != null && mpt.Code == _claimservice.Claim.MedicalProfessionalID).FirstOrDefault();
                }
                _selectedclaimtype = _claimservice.SelectedClaimType;
                RaisePropertyChanged(() => SelectedClaimType);

                if (_claimservice.SelectedExpenseType != null)
                {
                    IsExpenseTypeVisible = true;
                }
                if (_claimservice.SelectedExpenseTypeRequiresReferral)
                {
                    IsReferralQuestionVisible = true;
                }
                if ((SelectedClaimExpenseType != null && !string.IsNullOrEmpty(SelectedClaimExpenseType.Description) || (SelectedClaimType != null && !string.IsNullOrEmpty(SelectedClaimType.Description))))
                {
                    this.IsDescriptionVisible = true;
                    this.Description = SelectedClaimExpenseType != null && !string.IsNullOrEmpty(SelectedClaimExpenseType.Description) ? SelectedClaimExpenseType.Description :
                    SelectedClaimType != null && !string.IsNullOrEmpty(SelectedClaimType.Description) ? SelectedClaimType.Description : string.Empty;
                }
                else
                {
                    this.IsDescriptionVisible = false;
                    this.IsExpenseTypeDescriptionVisible = false;
                }

                if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice == GSCHelper.OS.iOS)
                {
                    string planMemberId = Mvx.IoCProvider.Resolve<IParticipantService>().SelectedParticipant != null ? Mvx.IoCProvider.Resolve<IParticipantService>().SelectedParticipant.PlanMemberID : Mvx.IoCProvider.Resolve<ILoginService>().CurrentPlanMemberID;
                    if (!string.IsNullOrEmpty(planMemberId) && _selectedclaimtype != null)
                        _claimservice.GetClaimExpenseTypes(planMemberId, _selectedclaimtype.ID, () =>
                        {
                            _claimexpensetypes = new ObservableCollection<ExpenseType>(_claimservice.ExpenseTypes.ToArray<ExpenseType>());
                            _selectedclaimexpensetype = _claimexpensetypes.Where(cet => cet != null && cet.ID == _claimservice.SelectedExpenseType.ID).FirstOrDefault();
                            RaisePropertyChanged(() => ClaimExpenseTypes);
                            RaisePropertyChanged(() => SelectedClaimExpenseType);
                        },
                            (a, b) =>
                            {
                                ClaimExpenseTypes = null;
                                SelectedClaimExpenseType = null;
                            });
                }
                _selectedmedicalprofessionaltype = MedicalProfessionalTypes.Where(mpt => mpt != null && mpt.Code == _claimservice.Claim.MedicalProfessionalID).FirstOrDefault();
                //RaisePropertyChanged(() => SelectedClaimType);
                //RaisePropertyChanged(() => SelectedClaimExpenseType);
                RaisePropertyChanged(() => SelectedMedicalProfessionalType);
            }

            if (_claimservice.ClaimSubmissionTypes != null && _claimservice.ClaimSubmissionTypes.Count() > 0)
            {
                _claimtypes = new ObservableCollection<ClaimType>(_claimservice.ClaimSubmissionTypes);
                RaisePropertyChanged(() => ClaimTypes);
                if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice == GSCHelper.OS.Droid)
                {
                    RaisePropertyChanged(() => SelectedClaimType);
                }
                if (!CreatedFromRehydration)
                {
                    SelectedClaimType = _claimservice.SelectedClaimType;
                    SelectedClaimExpenseType = null;
                }
            }
            else
            {
                this.IsBusy = true;
                Mvx.IoCProvider.Resolve<IUserDialogs>().ShowLoading(Resource.Loading);
                _claimservice.GetClaimTypes
                    (_loginservice.CurrentPlanMemberID, () =>
                                                                {
                                                                    ClaimType[] ctypes = new ClaimType[_claimservice.ClaimSubmissionTypes.Count];
                                                                    _claimservice.ClaimSubmissionTypes.CopyTo(ctypes);
                                                                    _claimtypes = new ObservableCollection<ClaimType>(ctypes.ToList<ClaimType>());
                                                                    RaisePropertyChanged(() => ClaimTypes);
                                                                    if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice != GSCHelper.OS.Droid) RaisePropertyChanged(() => SelectedClaimType);
                                                                    if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice == GSCHelper.OS.Droid)
                                                                    {
                                                                        RaisePropertyChanged(() => SelectedClaimType);
                                                                    }
                                                                    if (CreatedFromRehydration)
                                                                    {
                                                                        if (_claimtypes.First() == null && _claimservice.SelectedClaimType != null)
                                                                        {
                                                                            _selectedclaimtype = _claimtypes.Where(ct => ct != null && ct.ID == _claimservice.SelectedClaimType.ID).FirstOrDefault();
                                                                        }
                                                                        else
                                                                        {
                                                                            _selectedclaimtype = _claimtypes.First();
                                                                            if (_selectedclaimtype == null)
                                                                            {
                                                                                IsBusy = false;
                                                                                Mvx.IoCProvider.Resolve<IUserDialogs>().HideLoading();
                                                                            }
                                                                        }
                                                                        //RaisePropertyChanged(() => SelectedClaimType);
                                                                        _selectedclaimexpensetype = _claimservice.SelectedExpenseType;

                                                                        if (ClaimExpenseTypes == null || ClaimExpenseTypes.Count == 0)
                                                                        {
                                                                            _claimservice.GetClaimExpenseTypes(Mvx.IoCProvider.Resolve<IParticipantService>().SelectedParticipant != null ? Mvx.IoCProvider.Resolve<IParticipantService>().SelectedParticipant.PlanMemberID : Mvx.IoCProvider.Resolve<ILoginService>().CurrentPlanMemberID, _selectedclaimtype.ID, () =>
                                                                            {
                                                                                ClaimExpenseTypes = new ObservableCollection<ExpenseType>(_claimservice.ExpenseTypes.ToArray<ExpenseType>());
                                                                                _selectedclaimexpensetype = _claimexpensetypes.Where(cet => cet != null && cet.ID == _claimservice.SelectedExpenseType.ID).FirstOrDefault();
                                                                                RaisePropertyChanged(() => ClaimExpenseTypes);
                                                                                RaisePropertyChanged(() => SelectedClaimExpenseType);
                                                                            },
                                                                            (a, b) =>
                                                                            {
                                                                                ClaimExpenseTypes = null;
                                                                                SelectedClaimExpenseType = null;
                                                                            });
                                                                        }
                                                                    }
                                                                    this.IsBusy = false;
                                                                    Mvx.IoCProvider.Resolve<IUserDialogs>().HideLoading();
                                                                }, (message, code) =>
                                                                {
                                                                    ClaimTypes = null;
                                                                    ErrorMessage = message;
                                                                    ErrorCode = code;
                                                                    this.IsBusy = false;
                                                                    Mvx.IoCProvider.Resolve<IUserDialogs>().HideLoading();
                                                                });
            }
            if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice == GSCHelper.OS.iOS)
            {
                //CreatedFromRehydration = false;
            }
        }


        public override async Task Initialize()
        {
            await base.Initialize();
            if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice == GSCHelper.OS.iOS)
            {
                CreatedFromRehydration = false;
            }

        }

        public string CannotBeSubmittedOnlineMessage => Resource.CannotBeSubmittedOnlineMessage;

        private bool _isexpensetypevisible;
        public bool IsExpenseTypeVisible
        {
            get
            {
                return _isexpensetypevisible;
            }
            set
            {
                _isexpensetypevisible = value;
                RaisePropertyChanged(() => IsExpenseTypeVisible);
            }
        }

        private bool _isexpensetypedescriptionvisible;
        public bool IsExpenseTypeDescriptionVisible
        {
            get
            {
                return _isexpensetypedescriptionvisible;
            }
            set
            {
                _isexpensetypedescriptionvisible = value;
                RaisePropertyChanged(() => IsExpenseTypeDescriptionVisible);
            }
        }

        private bool _isexpensetypedescriptioncollapsed = true;
        public bool IsExpenseTypeDescriptionCollapsed
        {
            get
            {
                return _isexpensetypedescriptioncollapsed;
            }
            set
            {
                _isexpensetypedescriptioncollapsed = value;
                RaisePropertyChanged(() => IsExpenseTypeDescriptionCollapsed);
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        private bool _isdescriptionvisible;
        public bool IsDescriptionVisible
        {
            get
            {
                return _isdescriptionvisible;
            }
            set
            {
                _isdescriptionvisible = value;
                RaisePropertyChanged(() => IsDescriptionVisible);
            }
        }

        private ClaimType _selectedclaimtype;
        public ClaimType SelectedClaimType
        {
            get
            {
                return _selectedclaimtype;
            }
            set
            {
                if (processingClaimType) return;
                if (_claimservice.SelectedClaimType != value)
                {
                    _claimservice.SelectedClaimType = value;
                    if (value != null)
                    {
                        _claimservice.Claim.ClaimTypeID = value.ID;
                        _claimservice.Claim.ClaimType = value;
                        _dataservice.PersistSelectedHCSAClaimType(value);
                    }
                }
                if (_selectedclaimtype != value)
                {
                    _selectedclaimtype = value;
                    if (value != null) { _selectedclaimtype.Selected = true; }
                    if (!CreatedFromRehydration)
                    {
                        _claimservice.SelectedClaimType = value;
                        if (value != null)
                        {
                            _claimservice.Claim.ClaimTypeID = value.ID;
                            _claimservice.Claim.ClaimType = value;
                        }
                        _dataservice.PersistSelectedHCSAClaimType(value);
                    }
                    RaisePropertyChanged(() => SelectedClaimType);
                    if (value != null)
                    {
                        if (!string.IsNullOrEmpty(value.Description))
                        {
                            Description = value.Description;
                            IsDescriptionVisible = true;
                        }
                    }
                    NotifyCommands();
                    if (value != null)
                    {
                        if (value.ID == 1029)
                        {
                            Mvx.IoCProvider.Resolve<IUserDialogs>().Alert(Resource.CannotBeSubmittedOnlineMessage, "", Resource.ok);
                        }
                    }
                }
            }
        }

        private ObservableCollection<ClaimType> _claimtypes;
        public ObservableCollection<ClaimType> ClaimTypes
        {
            get { return _claimtypes; }
            set
            {
                if (_claimtypes != value)
                {
                    _claimtypes = value;
                    if (_claimtypes.Count > 0 && _claimtypes.FirstOrDefault() != null)
                    {
                        _claimtypes.Insert(0, null);
                        if (!CreatedFromRehydration)
                        {
                            SelectedClaimType = _claimtypes.First();
                        }
                    }

                    RaisePropertyChanged(() => ClaimTypes);
                    if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice == GSCHelper.OS.Droid)
                    {
                        RaisePropertyChanged(() => SelectedClaimType);
                    }
                }
            }
        }

        private ObservableCollection<ExpenseType> _claimexpensetypes;
        public ObservableCollection<ExpenseType> ClaimExpenseTypes
        {
            get
            {
                return _claimexpensetypes;
            }
            set
            {
                _claimexpensetypes = value;
                if (_claimexpensetypes != null && _claimexpensetypes.Count > 0 && _claimexpensetypes.FirstOrDefault().Name != null)
                {
                    ExpenseType et = new ExpenseType();
                    et.Name = string.Empty;
                    _claimexpensetypes.Insert(0, et);
                }
                RaisePropertyChanged(() => ClaimExpenseTypes);
                //MvxTrace.Trace(ClaimExpenseTypes.Count().ToString());
            }
        }

        private ObservableCollection<HCSAReferralType> _medicalprofessionaltypes;
        public ObservableCollection<HCSAReferralType> MedicalProfessionalTypes
        {
            get
            {
                return _medicalprofessionaltypes;
            }
            set
            {
                _medicalprofessionaltypes = value;
                if (_medicalprofessionaltypes.First().Code != "-1")
                {
                    _medicalprofessionaltypes.Insert(0, new HCSAReferralType { Code = "-1", Text = "", Selected = true });
                }
                RaisePropertyChanged(() => MedicalProfessionalTypes);
            }
        }


        private bool _iscontinuebuttonvisible;
        public bool IsContinueButtonVisible
        {
            get
            {
                return _iscontinuebuttonvisible;
            }
            set
            {
                _iscontinuebuttonvisible = value;
                RaisePropertyChanged(() => IsContinueButtonVisible);
            }
        }

        public bool SelectedExpenseTypeRequiresReferral
        {
            get
            {
                return _claimservice.SelectedExpenseTypeRequiresReferral;
            }
        }

        private ExpenseType _selectedclaimexpensetype;
        public ExpenseType SelectedClaimExpenseType
        {
            get
            {
                return _selectedclaimexpensetype;
            }
            set
            {
                _selectedclaimexpensetype = value;
                if (value != null) { _selectedclaimexpensetype.Selected = true; }
                _claimservice.SelectedExpenseType = value;
                _claimservice.Claim.ExpenseType = value;
                _dataservice.PersistHCSAClaim(_claimservice.Claim);
                _dataservice.PersistSelectedHCSAExpenseType(value);
                if (value != null)
                {
                    _claimservice.Claim.ExpenseTypeID = value.ID;
                    if (!string.IsNullOrEmpty(value.Description))
                    {
                        Description = value.Description;
                        IsDescriptionVisible = true;
                    }
                    else
                    {
                        if (SelectedClaimType == null || string.IsNullOrEmpty(SelectedClaimType.Description))
                        {
                            IsDescriptionVisible = false;
                        }
                    }
                }
                if (!CreatedFromRehydration)
                {
                    RaisePropertyChanged(() => SelectedClaimExpenseType);
                }
                RaisePropertyChanged(() => SelectedExpenseTypeRequiresReferral);
                IsReferralQuestionVisible = _claimservice.SelectedExpenseTypeRequiresReferral;
                SelectedMedicalProfessionalType = SelectedMedicalProfessionalType == null ? MedicalProfessionalTypes.FirstOrDefault() : SelectedMedicalProfessionalType;
                NotifyCommands();
                if (value != null && value.ID == 28)
                {
                    Mvx.IoCProvider.Resolve<IUserDialogs>().AlertAsync(Resource.CannotBeSubmittedOnlineMessage, "", Resource.ok);
                }
            }
        }

        private string _errormessage;
        public string ErrorMessage
        {
            get { return _errormessage; }
            set
            {
                if (_errormessage != value)
                {
                    _errormessage = value;
                    RaisePropertyChanged(() => ErrorMessage);
                }
            }
        }

        private int _errorcode;
        public int ErrorCode
        {
            get { return _errorcode; }
            set
            {
                if (_errorcode != value)
                {
                    _errorcode = value;
                    RaisePropertyChanged(() => ErrorCode);
                }
            }
        }

        public string ContinueLabel => Resource.Continue;

        public string ChooseTypeOfClaimLabel => Resource.ChooseTypeOfClaim;

        public string ChooseTypeOfExpenseLabel => Resource.ChooseTypeOfExpense;

        public string ChooseMedicalProfessionalTypeLabel => Resource.ReferralRequired;

        public string DescriptionLabel => Resource.DescriptionLabel;

        public string TitleLabel => Resource.HCSAChooseClaimTypeScreenTitle;

        private HCSAReferralType _selectedmedicalprofessionaltype;
        public HCSAReferralType SelectedMedicalProfessionalType
        {
            get
            {
                return _selectedmedicalprofessionaltype;
            }
            set
            {
                _selectedmedicalprofessionaltype = value;
                _claimservice.Claim.MedicalProfessionalID = value != null && value.Code != "-1" ? value.Code : null;
                RaisePropertyChanged(() => SelectedMedicalProfessionalType);
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

        MvxCommand<HCSAReferralType> _SelectMedicalProfessionalTypeCommand;
        public MvxCommand<HCSAReferralType> SelectMedicalProfessionalTypeCommand
        {
            get
            {
                _SelectMedicalProfessionalTypeCommand = _SelectMedicalProfessionalTypeCommand ?? new MvxCommand<HCSAReferralType>((rt) =>
                                                              {
                                                                  SelectedMedicalProfessionalType = rt;
                                                                  _claimservice.PersistClaim();
                                                              });
                return _SelectMedicalProfessionalTypeCommand;
            }
        }

        MvxCommand _continuecommand;
        public System.Windows.Input.ICommand ContinueCommand
        {
            get
            {
                _continuecommand = _continuecommand ?? new MvxCommand(
                                                          () =>
                                                              {
                                                                  PublishMessages();
                                                                  Unsubscribe();
                                                                  ShowViewModel<ClaimParticipantsViewModel>();
                                                              },
                                                          () =>
                                                              {
                                                                  bool canExecute = _claimservice.SelectedClaimType != null
                                                                  && (_claimservice.SelectedExpenseTypeRequiresReferral ? SelectedMedicalProfessionalType != null && SelectedMedicalProfessionalType.Code != "-1" : true)
                                                                  && (IsExpenseTypeVisible ? this.SelectedClaimExpenseType != null && (SelectedClaimExpenseType.ID != 1029 && SelectedClaimExpenseType.ID != 0) : true)
                                                                  && (SelectedClaimExpenseType != null ? (SelectedClaimExpenseType.ID != 28) : true)
                                                                  && (SelectedClaimType != null ? (SelectedClaimType.ID != 1029) : true);
                                                                  IsContinueButtonVisible = canExecute;
                                                                  return canExecute;
                                                              }
                                                         );
                return _continuecommand;
            }
        }

        MvxCommand<ExpenseType> _selectexpensetypewithoutnavigatingcommand;
        public MvxCommand<ExpenseType> SelectExpenseTypeWithoutNavigatingCommand
        {
            get
            {
                _selectexpensetypewithoutnavigatingcommand = _selectexpensetypewithoutnavigatingcommand ?? new MvxCommand<ExpenseType>(async (expenseType) =>
                                                              {
                                                                  ExpenseType _currentexpensetype = this._selectedclaimexpensetype;
                                                                  _logger.WriteLine("Selecting claim expense type and suppressing navigation");
                                                                  if (expenseType != this.SelectedClaimExpenseType)
                                                                  {
                                                                      if (_claimservice.Claim.Details.Count > 0)
                                                                      {
                                                                          var shouldChange = await Mvx.IoCProvider.Resolve<IUserDialogs>().ConfirmAsync(Resource.ExistingHCSAClaimDetailsMessage, null, Resource.ok, Resource.Cancel);
                                                                          if (!shouldChange)
                                                                          {
                                                                              _selectedclaimexpensetype = expenseType;//hacking to get around weird databinding problem
                                                                              _selectedclaimexpensetype = _currentexpensetype;
                                                                              _claimservice.SelectedExpenseType = _currentexpensetype;
                                                                              _claimservice.Claim.ExpenseType = _currentexpensetype;
                                                                              PublishMessages();
                                                                              Unsubscribe();
                                                                              if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice == GSCHelper.OS.iOS)
                                                                              {
                                                                                  RaiseRefusedToChangeType(new EventArgs());
                                                                              }
                                                                              var deviceservice = Mvx.IoCProvider.Resolve<IDeviceService>();
                                                                              if (deviceservice.CurrentDevice == GSCHelper.OS.iOS)// && deviceservice.IsTablet)
                                                                              {
                                                                                  //HACK
                                                                                  Mvx.IoCProvider.Resolve<IRehydrationService>().Rehydrating = true;
                                                                                  Mvx.IoCProvider.Resolve<IRehydrationService>().HackingRehydration = true;
                                                                                  _log.Trace("Turned on rehydration");
                                                                                  ShowViewModel<ClaimParticipantsViewModel>();
                                                                              }
                                                                              RaisePropertyChanged(() => SelectedClaimExpenseType);
                                                                              ShowViewModel<ClaimReviewAndEditViewModel>();

                                                                              //if (ContinueCommand.CanExecute(null)) ContinueCommand.Execute(null);
                                                                              return;
                                                                          }
                                                                          else
                                                                          {
                                                                              _claimservice.Claim.Details.Clear();
                                                                              _claimservice.Claim.ParticipantNumber = string.Empty;
                                                                              SelectedMedicalProfessionalType.Selected = false;
                                                                              SelectedMedicalProfessionalType = MedicalProfessionalTypes.FirstOrDefault();
                                                                              Mvx.IoCProvider.Resolve<IParticipantService>().SelectedParticipant = null;
                                                                              _claimservice.HaveClaimDetailsAlreadyBeenInitialized = false;

                                                                              var _rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
                                                                              _rehydrationservice.ClearFromStartingPoint(typeof(ClaimTypeViewModel));

                                                                              if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice == GSCHelper.OS.iOS)
                                                                              {
                                                                                  _messenger.Publish<Messages.HCSAClaimOrExpenseTypeHasChangedMessage>(new Messages.HCSAClaimOrExpenseTypeHasChangedMessage(this));
                                                                              }
                                                                          }
                                                                      }
                                                                  }
                                                                  if (expenseType != null)
                                                                  {
                                                                      expenseType.Selected = true;
                                                                  }
                                                                  this.SelectedClaimExpenseType = expenseType;
                                                              },
                                                          (expenseType) =>
                                                              {
                                                                  return true; //Placeholder - replace with CanExecute logic
                                                              }
                                                         );
                return _selectexpensetypewithoutnavigatingcommand;
            }
        }


        MvxCommand<ClaimType> _selectClaimTypeWithoutNavigatingCommand;
        bool processingClaimType = false;
        public event EventHandler OnRefusedToChangeType;

        protected virtual void RaiseRefusedToChangeType(EventArgs e)
        {
            if (this.OnRefusedToChangeType != null)
            {
                OnRefusedToChangeType(this, e);
            }
        }

        public System.Windows.Input.ICommand SelectClaimTypeWithoutNavigatingCommand
        {
            get
            {
                _selectClaimTypeWithoutNavigatingCommand = _selectClaimTypeWithoutNavigatingCommand ?? new MvxCommand<ClaimType>(
                                              async (claimType) =>
                                              {
                                                  _logger.WriteLine("Selecting claim expense type and suppressing navigation");
                                                  ClaimType _currentselectedclaimtype = _selectedclaimtype;
                                                  if (claimType != this._selectedclaimtype)
                                                  {

                                                      if (_claimservice.Claim.Details.Count > 0)// && !CreatedFromRehydration)
                                                      {
                                                          processingClaimType = true;
                                                          bool shouldChange = await Mvx.IoCProvider.Resolve<IUserDialogs>().ConfirmAsync(new ConfirmConfig()
                                                          {
                                                              CancelText = Resource.Cancel,
                                                              Message = Resource.ExistingHCSAClaimDetailsMessage,
                                                              OkText = Resource.ok,
                                                              Title = string.Empty
                                                          });


                                                          if (!shouldChange)
                                                          {
                                                              processingClaimType = false;
                                                              //SelectedClaimType = claimType;//hacking to get around weird databinding problem
                                                              _selectedclaimtype = _currentselectedclaimtype;
                                                              _claimservice.Claim.ClaimTypeID = _currentselectedclaimtype.ID;
                                                              _claimservice.Claim.ClaimType = _currentselectedclaimtype;
                                                              _dataservice.PersistSelectedHCSAClaimType(_currentselectedclaimtype);
                                                              //var tmpExpense = SelectedClaimExpenseType;
                                                              //var tmpReferral = SelectedMedicalProfessionalType;
                                                              //SelectedClaimExpenseType = ClaimExpenseTypes != null && ClaimExpenseTypes.First().ID == 0 ? ClaimExpenseTypes.First() :  null;
                                                              //SelectedMedicalProfessionalType = MedicalProfessionalTypes.FirstOrDefault();
                                                              //SelectedClaimExpenseType = tmpExpense;
                                                              //SelectedMedicalProfessionalType = tmpReferral;
                                                              PublishMessages();
                                                              Unsubscribe();
                                                              if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice == GSCHelper.OS.iOS)
                                                              {
                                                                  RaiseRefusedToChangeType(new EventArgs());
                                                              }

                                                              var deviceservice = Mvx.IoCProvider.Resolve<IDeviceService>();
                                                              if (deviceservice.CurrentDevice == GSCHelper.OS.iOS)//&& deviceservice.IsTablet)
                                                              {
                                                                  //HACK
                                                                  Mvx.IoCProvider.Resolve<IRehydrationService>().Rehydrating = true;
                                                                  Mvx.IoCProvider.Resolve<IRehydrationService>().HackingRehydration = true;
                                                                  _log.Trace("Turned on rehydration");
                                                                  ShowViewModel<ClaimParticipantsViewModel>();
                                                              }
                                                              RaisePropertyChanged(() => SelectedClaimType);
                                                              ShowViewModel<ClaimReviewAndEditViewModel>();
                                                              //if (ContinueCommand.CanExecute(null)) ContinueCommand.Execute(null);
                                                              return;
                                                          }
                                                          else
                                                          {
                                                              _changedClaimType = true;
                                                              //var _rehydrator = Mvx.IoCProvider.Resolve<IRehydrationService>();
                                                              //_rehydrator.ClearFromStartingPoint(this.GetType());

                                                              processingClaimType = false;
                                                              _claimservice.Claim.Details.Clear();

                                                              _claimservice.SelectedClaimType = claimType;
                                                              _claimservice.Claim.ClaimType = claimType;
                                                              _claimservice.Claim.ClaimTypeID = claimType.ID;

                                                              if (ClaimExpenseTypes != null)
                                                              {
                                                                  SelectedClaimExpenseType = ClaimExpenseTypes.First();
                                                                  if (SelectedMedicalProfessionalType != null)
                                                                  {
                                                                      SelectedMedicalProfessionalType.Selected = false;
                                                                  }
                                                                  SelectedMedicalProfessionalType = MedicalProfessionalTypes.First();
                                                              }
                                                              else
                                                              {
                                                                  SelectedClaimExpenseType = ClaimExpenseTypes != null && ClaimExpenseTypes.First().ID == 0 ? ClaimExpenseTypes.First() : null;
                                                                  if (SelectedMedicalProfessionalType != null)
                                                                  {
                                                                      SelectedMedicalProfessionalType.Selected = false;
                                                                  }
                                                                  SelectedMedicalProfessionalType = MedicalProfessionalTypes.First();
                                                              }
                                                              _claimservice.Claim.ParticipantNumber = string.Empty;
                                                              Mvx.IoCProvider.Resolve<IParticipantService>().SelectedParticipant = null;
                                                              _claimservice.HaveClaimDetailsAlreadyBeenInitialized = false;

                                                              var _rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
                                                              _rehydrationservice.ClearFromStartingPoint(typeof(ClaimTypeViewModel));

                                                              if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice == GSCHelper.OS.iOS)
                                                              {
                                                                  _messenger.Publish<Messages.HCSAClaimOrExpenseTypeHasChangedMessage>(new Messages.HCSAClaimOrExpenseTypeHasChangedMessage(this));
                                                              }
                                                          }
                                                      }
                                                  }
                                                  if (SelectedClaimType == claimType) return;
                                                  if (SelectedMedicalProfessionalType != null)
                                                  {
                                                      SelectedMedicalProfessionalType.Selected = false;
                                                  }
                                                  SelectedMedicalProfessionalType = MedicalProfessionalTypes.First();
                                                  SelectedClaimType = claimType;
                                                  this.IsBusy = true;
                                                  Mvx.IoCProvider.Resolve<IUserDialogs>().ShowLoading();

                                                  _claimservice.SelectedClaimType = claimType;
                                                  if (claimType != null)
                                                  {
                                                      claimType.Selected = true;
                                                  }
                                                  _dataservice.PersistSelectedHCSAClaimType(claimType);
                                                  NotifyCommands();
                                                  IsReferralQuestionVisible = false;
                                                  IsExpenseTypeVisible = false;
                                                  _claimservice.GetClaimExpenseTypes(_loginservice.CurrentPlanMemberID,
                                                                                      SelectedClaimType != null ? this.SelectedClaimType.ID : 0,
                                                                                      () =>
                                                                                      {
                                                                                          this.ClaimExpenseTypes = new ObservableCollection<ExpenseType>(_claimservice.ExpenseTypes.ToArray());
                                                                                          if (ClaimExpenseTypes != null && ClaimExpenseTypes.Count > 0) //default to the null value
                                                                                            SelectedClaimExpenseType = ClaimExpenseTypes.First();
                                                                                          else
                                                                                              SelectedClaimExpenseType = null;

                                                                                        //set to selected value if the claim type hasn't changed
                                                                                        if (ClaimExpenseTypes != null && ClaimExpenseTypes.Count > 0 &&
                                                                                               _claimservice.SelectedExpenseType != null && ClaimExpenseTypes.Contains(_claimservice.SelectedExpenseType))
                                                                                              SelectedClaimExpenseType = _claimservice.SelectedExpenseType;

                                                                                          RaisePropertyChanged(() => SelectedClaimExpenseType);
                                                                                        //if (!CreatedFromRehydration)
                                                                                        //{
                                                                                        //    if (ClaimExpenseTypes != null)
                                                                                        //    {
                                                                                        //        this.SelectedClaimExpenseType = this.ClaimExpenseTypes.First();
                                                                                        //    }
                                                                                        //    else
                                                                                        //    {
                                                                                        //        SelectedClaimExpenseType = ClaimExpenseTypes != null && ClaimExpenseTypes.First().ID == 0 ? ClaimExpenseTypes.First() : null;
                                                                                        //    }
                                                                                        //}
                                                                                        //else
                                                                                        //{
                                                                                        //    if (_changedClaimType)
                                                                                        //        this.SelectedClaimExpenseType = ClaimExpenseTypes.First();
                                                                                        //    else
                                                                                        //        this.SelectedClaimExpenseType = _claimservice.SelectedExpenseType;

                                                                                        //    RaisePropertyChanged(() => SelectedClaimExpenseType);
                                                                                        //    _changedClaimType = false;
                                                                                        //}
                                                                                        this.IsBusy = false;
                                                                                          Mvx.IoCProvider.Resolve<IUserDialogs>().HideLoading();
                                                                                          this.IsExpenseTypeVisible = true;
                                                                                          Mvx.IoCProvider.Resolve<IUserDialogs>().HideLoading();
                                                                                      },
                                                                                      (message, code) =>
                                                                                      {
                                                                                          this.ErrorCode = code;
                                                                                          this.ErrorMessage = message;
                                                                                          this.IsBusy = false;
                                                                                          Mvx.IoCProvider.Resolve<IUserDialogs>().HideLoading();
                                                                                          IsExpenseTypeVisible = false;
                                                                                          ClaimExpenseTypes = null;
                                                                                          SelectedClaimExpenseType = ClaimExpenseTypes != null && ClaimExpenseTypes.First().ID == 0 ? ClaimExpenseTypes.First() : null;
                                                                                      });
                                              },
                                              (claimType) =>
                                              {
                                                  return true; //placeholder - replace with correct CanExecute logic
                                              });
                return _selectClaimTypeWithoutNavigatingCommand;
            }
        }
    }
}