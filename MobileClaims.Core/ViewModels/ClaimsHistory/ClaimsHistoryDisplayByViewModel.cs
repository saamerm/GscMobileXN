using FluentValidation;
using FluentValidation.Results;
using MobileClaims.Core.Entities.ClaimsHistory;
using MobileClaims.Core.Services.ClaimsHistory;
using MobileClaims.Core.ViewModels.HCSA;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace MobileClaims.Core.ViewModels.ClaimsHistory
{
    public class ClaimsHistoryDisplayByViewModel : HCSAViewModelBase 
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimsHistoryService _claimshistoryservice;

        public ClaimsHistoryDisplayByViewModel(IMvxMessenger messenger,IClaimsHistoryService calimsHistoryService)
        {
            _messenger = messenger;
            _claimshistoryservice = calimsHistoryService;
            Years = new ObservableCollection<DisplayByYear>();
            SelectedDisplayBy = new KeyValuePair<string, string>();
            SelectedStartDate = null;
            SelectedEndDate = null;
            InternalSelectedStartDate = new DateTime();
            InternalSelectedEndDate = new DateTime();
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            DisplayBy = new ObservableCollection<KeyValuePair<string, string>>();
            DisplayBy.Add(new KeyValuePair<string, string>(GSCHelper.DisplayByYearToDateKey, Resource.ClaimsHistorySearchViewModel_DisplayByYearToDate));
            DisplayBy.Add(new KeyValuePair<string, string>(GSCHelper.DisplayByDateRangeKey, Resource.ClaimsHistorySearchViewModel_DisplayByDateRange));
            if (_claimshistoryservice.SelectedClaimHistoryType != null && _claimshistoryservice.SelectedClaimHistoryType.ID != GSCHelper.ClaimHistoryMedicalItemID
                && _claimshistoryservice.SelectedClaimHistoryType.ID != GSCHelper.ClaimHistoryDentalPredeterminationID)
                DisplayBy.Add(new KeyValuePair<string, string>(GSCHelper.DisplayByYearKey, Resource.ClaimsHistorySearchViewModel_DisplayByYear));

            if (_claimshistoryservice.SelectedDisplayBy.Key != string.Empty)
                SelectedDisplayBy = _claimshistoryservice.SelectedDisplayBy;
            else
                SelectedDisplayBy = DisplayBy.FirstOrDefault(db => db.Key == "YTD");

            if (_claimshistoryservice.SelectedClaimHistoryType != null && _claimshistoryservice.SelectedClaimHistoryType.EarliestYear > 0)
                EarliestYear = _claimshistoryservice.SelectedClaimHistoryType.EarliestYear;
            else
                EarliestYear = GSCHelper.ClaimHistoryDefaultEarliestYear;

            PopulateDisplayByValuesOnVMLoad();
        }

        private ObservableCollection<KeyValuePair<string, string>> _displayBy;
        public ObservableCollection<KeyValuePair<string, string>> DisplayBy
        {
            get { return _displayBy; }
            set
            {
                if (_displayBy != value)
                {
                    _displayBy = value;
                    RaisePropertyChanged(() => DisplayBy);
                }
            }
        }

        private KeyValuePair<string, string> _selectedDisplayBy;
        public KeyValuePair<string, string> SelectedDisplayBy
        {
            get { return _selectedDisplayBy; }
            set
            {
                if (_selectedDisplayBy.Key != value.Key)
                {
                    _selectedDisplayBy = value;
                    RaisePropertyChanged(() => SelectedDisplayBy);
                    CheckDisplayByLogic(false);
                }
            }
        }

        public string YearToDateLabel
        {
            get
            {
                string startDateString = YearToDateStartDate.ToString(GSCHelper.YTD_DATE_FORMAT);
                string endDateString = YearToDateEndDate.ToString(GSCHelper.YTD_DATE_FORMAT);
                string ytdLabel = string.Format("{0} - {1}", startDateString, endDateString);
                return ytdLabel;
            }
        }

        public DateTime YearToDateStartDate
        {
            get { return GSCHelper.StartDateOfCurrentYear; }
        }

        public DateTime YearToDateEndDate
        {
            get { return DateTime.Now; }
        }

        private bool _isDateRangeVisible = false;
        public bool IsDateRangeVisible
        {
            get { return _isDateRangeVisible; }
            set
            {
                if (_isDateRangeVisible != value)
                {
                    _isDateRangeVisible = value;
                    RaisePropertyChanged(() => IsDateRangeVisible);
                }
            }
        }

        private DateTime? _selectedStartDate;
        public DateTime? SelectedStartDate
        {
            get { return _selectedStartDate; }
            set
            {
                if (_selectedStartDate != value)
                {
                    _selectedStartDate = value;

                    if (_selectedStartDate != null)
                    {
                        DateTime sd = new DateTime();
                        DateTime.TryParse(_selectedStartDate.ToString(), out sd);
                        _claimshistoryservice.SelectedStartDate = sd;
                        InternalSelectedStartDate = sd;
                    }
                    else
                    {
                        InternalSelectedStartDate = new DateTime();
                    }

                    RaisePropertyChanged(() => SelectedStartDate);
                }
            }
        }

        private DateTime _internalSelectedStartDate;
        public DateTime InternalSelectedStartDate
        {
            get { return _internalSelectedStartDate; }
            set
            {
                if (_internalSelectedStartDate != value)
                {
                    _internalSelectedStartDate = value;
                    RaisePropertyChanged(() => InternalSelectedStartDate);
                }
            }
        }

        private DateTime? _selectedEndDate;
        public DateTime? SelectedEndDate
        {
            get { return _selectedEndDate; }
            set
            {
                if (_selectedEndDate != value)
                {
                    _selectedEndDate = value;

                    if (_selectedEndDate != null)
                    {
                        DateTime ed = new DateTime();
                        DateTime.TryParse(_selectedEndDate.ToString(), out ed);
                        _claimshistoryservice.SelectedEndDate = ed;
                        InternalSelectedEndDate = ed;
                    }
                    else
                    {
                        InternalSelectedEndDate = new DateTime();
                    }

                    RaisePropertyChanged(() => SelectedEndDate);
                }
            }
        }

        private DateTime _internalSelectedEndDate;
        public DateTime InternalSelectedEndDate
        {
            get { return _internalSelectedEndDate; }
            set
            {
                if (_internalSelectedEndDate != value)
                {
                    _internalSelectedEndDate = value;
                    RaisePropertyChanged(() => InternalSelectedEndDate);
                }
            }
        }

        private bool _isYearsVisible = false;
        public bool IsYearsVisible
        {
            get { return _isYearsVisible; }
            set
            {
                if (_isYearsVisible != value)
                {
                    _isYearsVisible = value;
                    RaisePropertyChanged(() => IsYearsVisible);
                }
            }
        }

        private ObservableCollection<DisplayByYear> _years;
        public ObservableCollection<DisplayByYear> Years
        {
            get { return _years; }
            set
            {
                if (_years != value)
                {
                    _years = value;
                    RaisePropertyChanged(() => Years);
                }
            }
        }

        private DisplayByYear _selectedYear;
        public DisplayByYear SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    _claimshistoryservice.SelectedYear = _selectedYear.Year;
                    RaisePropertyChanged(() => SelectedYear);
                }
            }
        }

        private int _earliestYear;
        public int EarliestYear
        {
            get { return _earliestYear; }
            set
            {
                if (_earliestYear != value)
                {
                    _earliestYear = value;
                    RaisePropertyChanged(() => EarliestYear);
                    PopulateYears(SelectedDisplayBy.Key != GSCHelper.DisplayByYearKey);
                }
            }
        }

        public string DisplayByLabel
        {
            get { return Resource.DisplayBy; }
        }

        public string StartLabel
        {
            get { return Resource.Start; }
        }

        public string EndLabel
        {
            get { return Resource.End; }
        }

        public string DoneLabel
        {
            get { return Resource.Done; }
        }

        public string YearLabel
        {
            get { return Resource.ClaimsHistorySearchViewModel_DisplayByYear; }
        }

        public string CloseLabel
        {
            get { return Resource.ChangeForLife_MessageDialogButtonClose; }
        }

        private bool _startDateTooOld = false;
        public bool StartDateTooOld
        {
            get { return _startDateTooOld; }
            set
            {
                if (_startDateTooOld != value)
                {
                    _startDateTooOld = value;
                    RaisePropertyChanged(() => StartDateTooOld);
                }
            }
        }

        private bool _endDateTooOld = false;
        public bool EndDateTooOld
        {
            get { return _endDateTooOld; }
            set
            {
                if (_endDateTooOld != value)
                {
                    _endDateTooOld = value;
                    RaisePropertyChanged(() => EndDateTooOld);
                }
            }
        }

        private bool _emptyStartDate = false;
        public bool EmptyStartDate
        {
            get { return _emptyStartDate; }
            set
            {
                if (_emptyStartDate != value)
                {
                    _emptyStartDate = value;
                    RaisePropertyChanged(() => EmptyStartDate);
                }
            }
        }

        private bool _emptyEndDate = false;
        public bool EmptyEndDate
        {
            get { return _emptyEndDate; }
            set
            {
                if (_emptyEndDate != value)
                {
                    _emptyEndDate = value;
                    RaisePropertyChanged(() => EmptyEndDate);
                }
            }
        }

        private bool _futureStartDate = false;
        public bool FutureStartDate
        {
            get { return _futureStartDate; }
            set
            {
                if (_futureStartDate != value)
                {
                    _futureStartDate = value;
                    RaisePropertyChanged(() => FutureStartDate);
                }
            }
        }

        private bool _futureEndDate = false;
        public bool FutureEndDate
        {
            get { return _futureEndDate; }
            set
            {
                if (_futureEndDate != value)
                {
                    _futureEndDate = value;
                    RaisePropertyChanged(() => FutureEndDate);
                }
            }
        }

        private bool _startDateAfterEndDate = false;
        public bool StartDateAfterEndDate
        {
            get { return _startDateAfterEndDate; }
            set
            {
                if (_startDateAfterEndDate != value)
                {
                    _startDateAfterEndDate = value;
                    RaisePropertyChanged(() => StartDateAfterEndDate);
                }
            }
        }

        private bool _endDateBeforeStartDate = false;
        public bool EndDateBeforeStartDate
        {
            get { return _endDateBeforeStartDate; }
            set
            {
                if (_endDateBeforeStartDate != value)
                {
                    _endDateBeforeStartDate = value;
                    RaisePropertyChanged(() => EndDateBeforeStartDate);
                }
            }
        }

        private bool _emptyYear = false;
        public bool EmptyYear
        {
            get { return _emptyYear; }
            set
            {
                if (_emptyYear != value)
                {
                    _emptyYear = value;
                    RaisePropertyChanged(() => EmptyYear);
                }
            }
        }

        private string _startDateValidationMessage;
        public string StartDateValidationMessage
        {
            get { return _startDateValidationMessage; }
            set
            {
                if (_startDateValidationMessage != value)
                {
                    _startDateValidationMessage = value;
                    RaisePropertyChanged(() => StartDateValidationMessage);
                }
            }
        }

        private string _endDateValidationMessage;
        public string EndDateValidationMessage
        {
            get { return _endDateValidationMessage; }
            set
            {
                if (_endDateValidationMessage != value)
                {
                    _endDateValidationMessage = value;
                    RaisePropertyChanged(() => EndDateValidationMessage);
                }
            }
        }

        private string _yearValidationMessage;
        public string YearValidationMessage
        {
            get { return _yearValidationMessage; }
            set
            {
                if (_yearValidationMessage != value)
                {
                    _yearValidationMessage = value;
                    RaisePropertyChanged(() => YearValidationMessage);
                }
            }
        }

        private void CheckDisplayByLogic(bool setValuesInService)
        {
            if (SelectedDisplayBy.Key == GSCHelper.DisplayByDateRangeKey)
            {
                IsDateRangeVisible = true;
                IsYearsVisible = false;
                if (setValuesInService)
                {
                    _claimshistoryservice.SelectedStartDate = InternalSelectedStartDate;
                    _claimshistoryservice.SelectedEndDate = InternalSelectedEndDate;
                }
            }
            else if (SelectedDisplayBy.Key == GSCHelper.DisplayByYearKey)
            {
                IsDateRangeVisible = false;
                IsYearsVisible = true;
                if (setValuesInService)
                {
                    _claimshistoryservice.SelectedStartDate = new DateTime();
                    _claimshistoryservice.SelectedEndDate = new DateTime();
                }
            }
            else //GSCHelper.DisplayByYearToDateKey
            {
                IsDateRangeVisible = false;
                IsYearsVisible = false;
                if (setValuesInService)
                {
                    _claimshistoryservice.SelectedStartDate = YearToDateStartDate;
                    _claimshistoryservice.SelectedEndDate = YearToDateEndDate;
                }
            }
        }

        private void PopulateYears(bool setDefault)
        {
            int lastYear = DateTime.Now.Year-1;
            for (int i = lastYear; i >= EarliestYear; i--)
            {
                DisplayByYear yr = new DisplayByYear();
                yr.DisplayString = i.ToString();
                yr.Year = i;
                Years.Add(yr);
            }
            Years.Insert(0, new DisplayByYear { DisplayString = string.Empty, Year = -1 });

            if (setDefault)
                SelectedYear = Years[0];
        }

        private void PopulateDisplayByValuesOnVMLoad()
        {
            if (SelectedDisplayBy.Key == GSCHelper.DisplayByDateRangeKey)
            {
                if (_claimshistoryservice.SelectedStartDate != DateTime.MinValue)
                    SelectedStartDate = _claimshistoryservice.SelectedStartDate;

                if (_claimshistoryservice.SelectedEndDate != DateTime.MinValue)
                    SelectedEndDate = _claimshistoryservice.SelectedEndDate;
            }
            else if (SelectedDisplayBy.Key == GSCHelper.DisplayByYearKey)
            {
                SelectedYear = Years.FirstOrDefault(yr => yr.Year == _claimshistoryservice.SelectedYear);
            }
            else //GSCHelper.DisplayByYearToDateKey
            {
                //Handled by YearToDateLabel
            }
        }

        public ICommand YearSelectedCommand
        {
            get
            {
                return new MvxCommand<DisplayByYear>((selectedYear) =>
                {
                    this.SelectedYear = selectedYear;
                },
                (selectedYear) =>
                {
                    return true;
                });
            }
        }

        MvxCommand _doneCommand;
        public ICommand DoneCommand
        {
            get
            {
                _doneCommand = _doneCommand ?? new MvxCommand(() =>
                {
                    if (IsValid())
                    {
                        _claimshistoryservice.SelectedDisplayBy = _selectedDisplayBy;
                        CheckDisplayByLogic(true);
                        RaiseDisplayByEntryComplete(new EventArgs());
                        Close(this);
                    }
                },
                () =>
                {
                    return true;
                });
                return _doneCommand;
            }
        }

        public event EventHandler DisplayByEntryComplete;
        protected virtual void RaiseDisplayByEntryComplete(EventArgs e)
        {
            if (this.DisplayByEntryComplete != null)
            {
                DisplayByEntryComplete(this, e);
            }
        }

        public class ClaimsHistoryDisplayByValidator : AbstractValidator<ClaimsHistoryDisplayByViewModel>
        {
            public ClaimsHistoryDisplayByValidator()
            {
                RuleSet("DateRange", () =>
                {
                    RuleFor(vm => vm.SelectedStartDate).NotEmpty().WithMessage(Resource.StartDateIsNotAValidDate);
                    RuleFor(vm => vm.SelectedEndDate).NotEmpty().WithMessage(Resource.EndDateIsNotAValidDate);
                    RuleFor(vm => vm.SelectedStartDate).Must(BeWithinLast12Months).WithMessage(Resource.DateNotWithinLast12Months);
                    RuleFor(vm => vm.SelectedEndDate).Must(BeWithinLast12Months).WithMessage(Resource.DateNotWithinLast12Months);
                    RuleFor(vm => vm.SelectedStartDate).Must(NotBeAFutureDate).WithMessage(Resource.StartDateIsNotAValidDate);
                    RuleFor(vm => vm.SelectedEndDate).Must(NotBeAFutureDate).WithMessage(Resource.EndDateIsNotAValidDate);
                    RuleFor(vm => vm.SelectedStartDate).LessThanOrEqualTo(vm => vm.SelectedEndDate).When(vm => vm.SelectedEndDate != null).WithMessage(Resource.PleaseEnsureStartDatePreceedsEndDate);
                });
                RuleSet("Year", () =>
                {
                    RuleFor(vm => vm.SelectedYear).Must(HaveAValue).WithMessage(Resource.PleaseSelectAYear);
                });
            }
            public bool BeWithinLast12Months(DateTime? date)
            {
                if (date != null)
                    return date >= DateTime.Now.AddMonths(-12);
                else
                    return true;
            }
            public bool NotBeAFutureDate(DateTime? date)
            {
                if (date != null)
                    return date <= DateTime.Now;
                else
                    return true;
            }
            public bool HaveAValue(DisplayByYear year)
            {
                return year.Year != -1;
            }
        }

        private bool IsValid()
        {
            // Reset for validation
            StartDateTooOld = false;
            EndDateTooOld = false;
            EmptyStartDate = false;
            EmptyEndDate = false;
            FutureStartDate = false;
            FutureEndDate = false;
            StartDateAfterEndDate = false;
            EmptyYear = false;
            StartDateValidationMessage = string.Empty;
            EndDateValidationMessage = string.Empty;
            YearValidationMessage = string.Empty;

            ClaimsHistoryDisplayByValidator validator = new ClaimsHistoryDisplayByValidator();
            validator.CascadeMode = CascadeMode.Continue;
            ValidationResult result = new ValidationResult();
            if (SelectedDisplayBy.Key == GSCHelper.DisplayByDateRangeKey)
                result = validator.Validate<ClaimsHistoryDisplayByViewModel>(this, ruleSet: "DateRange");
            else if (SelectedDisplayBy.Key == GSCHelper.DisplayByYearKey)
                result = validator.Validate<ClaimsHistoryDisplayByViewModel>(this, ruleSet: "Year");
            else
                return true;

            foreach (var failure in result.Errors)
            {
                switch (failure.PropertyName)
                {
                    case "SelectedStartDate":
                        {
                            if (failure.ErrorMessage == Resource.DateNotWithinLast12Months)
                                StartDateTooOld = true;
                            else if (failure.ErrorMessage == Resource.StartDateIsNotAValidDate)
                            {
                                if (SelectedStartDate == null)
                                    EmptyStartDate = true;
                                else
                                    FutureStartDate = true;
                            }
                            else if (failure.ErrorMessage == Resource.PleaseEnsureStartDatePreceedsEndDate)
                                StartDateAfterEndDate = true;

                            if (string.IsNullOrEmpty(StartDateValidationMessage))
                                StartDateValidationMessage = failure.ErrorMessage;
                            else
                                StartDateValidationMessage += "\n" + failure.ErrorMessage;

                            break;
                        }
                    case "SelectedEndDate":
                        {
                            if (failure.ErrorMessage == Resource.DateNotWithinLast12Months)
                                EndDateTooOld = true;
                            else if (failure.ErrorMessage == Resource.EndDateIsNotAValidDate)
                            {
                                if (SelectedEndDate == null)
                                    EmptyEndDate = true;
                                else
                                    FutureEndDate = true;
                            }

                            if (string.IsNullOrEmpty(EndDateValidationMessage))
                                EndDateValidationMessage = failure.ErrorMessage;
                            else
                                EndDateValidationMessage += "\n" + failure.ErrorMessage;

                            break;
                        }
                    case "SelectedYear":
                        {
                            EmptyYear = true;
                            YearValidationMessage = failure.ErrorMessage;
                            break;
                        }
                }
            }
            return result.IsValid;
        }
    }
}
