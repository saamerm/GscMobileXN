using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FluentValidation.Results;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.Requests;
using MobileClaims.Core.Services.Responses;
using MobileClaims.Core.Util;
using MobileClaims.Core.Validators;
using MvvmCross.Commands;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimTreatmentDetailsEntryDentalViewModel : ViewModelBase
    {
        private bool _editMode;
        private bool _isToothCodeRequired;
        private bool _isToothSurfaceRequired;
        private bool _isLabChargeRequired;

        private DateTime _dateOfTreatment;
        private string _procedureCode;

        private int? _toothCode;
        private string _toothSurfaces;
        private string _dentistsFeeStr;
        private double? _dentistsFee;
        private string _laboratoryChargeStr;
        private double? _laboratoryCharge;
        private string _alternateCarrierAmountStr;
        private double? _alternateCarrierAmount;

        private bool? _dateValid;
        private bool? _procedureCodeValid;
        private bool? _toothCodeValid;
        private bool? _toothSurfacesValid;
        private bool? _dentistsFeeValid;
        private bool? _laboratoryChargeValid;
        private bool? _alternateCarrierAmountValid;
        private bool _isAlternateCarrierAmountVisible;

        private string _dateOfTreatmentErrorText;
        private string _procedureCodeErrorText;
        private string _toothCodeErrorText;
        private string _toothSurfaceErrorText;
        private string _dentistFeesErrorText;
        private string _labChargesErrorText;
        private string _alternateCarrierAmountErrorText;

        private Guid _selectedTreatmentId { get; set; }

        private readonly IClaimService _claimService;
        private readonly ITreatmentDetailService _treatmentDetailService;
        private readonly ILoginService _loginService;
        private readonly IUserDialogs _userDialog;
        private readonly ClaimSubmissionType SelectedClaimSubmissionType;

        private readonly TreatmentDetailDateValidator _treatmentDateValidator;
        private readonly TreatmentDetailProcedureCodeValidator _procedureCodeValidator;
        private readonly TreatmentDetailToothCodeValidator _toothCodeValidator;
        private readonly TreatmentDetailToothSurfaceValidator _toothSurfaceValidator;
        private readonly TreatmentDetailDentistFeesValidator _dentistFeesValidator;
        private readonly TreatmentDetailLabChargesValidator _labChargesValidator;
        private readonly TreatmentDetailAmountPaidValidator _amountPaidValidator;

        public string Title => Resource.TreatmentTitle;
        public string DateOfTreatmentLabel { get; set; } = Resource.TreatmentDetailsDateOfTreatment;
        public string ProcedureCodeLabel => Resource.TreatmentDetailsProcedureCode;
        public string ToothCodeLabel => Resource.TreatmentDetailsToothCode;
        public string ToothSurfacesLabel => Resource.TreatmentDetailsToothSurface;
        public string DentistsFeeLabel => Resource.TreatmentDetailsDentistFee;
        public string LaboratoryChargeLabel => Resource.TreatmentDetailsLabCharge;
        public string AlternateCarrierAmountLabel => Resource.TreatmentDetailsAmountPaidAlt;
        public string SaveLabel => Resource.Save;
        public string CancelLabel => Resource.Cancel;
        public string DeleteLabel => Resource.DeleteTreatmentDetail;
        public string PlaceHolderText => "-";

        public bool EditMode
        {
            get => _editMode;
            set => SetProperty(ref _editMode, value);
        }

        public DateTime DateOfTreatment
        {
            get => _dateOfTreatment;
            set
            {
                SetProperty(ref _dateOfTreatment, value);
                var validationResult = _treatmentDateValidator.Validate(this);
                DateValid = validationResult.IsValid;

                var errors = validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidToothSurfaceValidation))
                    ?? validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidTreatmentDate24MonthValidation));
                DateOfTreatmentErrorText = errors?.ErrorMessage;
            }
        }

        public bool? DateValid
        {
            get => _dateValid;
            set => SetProperty(ref _dateValid, value);
        }

        public string DateOfTreatmentErrorText
        {
            get => _dateOfTreatmentErrorText;
            set => SetProperty(ref _dateOfTreatmentErrorText, value);
        }

        public string ProcedureCode
        {
            get => _procedureCode;
            set
            {
                SetProperty(ref _procedureCode, value);
                var validationResult = _procedureCodeValidator.Validate(this);
                ProcedureCodeValid = validationResult.IsValid;

                var validationErros = validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingProcedureCodeValidation))
                    ?? validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidProcedureCodeValidation));
                ProcedureCodeErrorText = validationErros?.ErrorMessage;

                if (validationResult.IsValid)
                {
                    ValidateDentalTreatmentDetailCommand.Execute();
                }
            }
        }

        public bool? ProcedureCodeValid
        {
            get => _procedureCodeValid;
            set => SetProperty(ref _procedureCodeValid, value);
        }

        public string ProcedureCodeErrorText
        {
            get => _procedureCodeErrorText;
            set => SetProperty(ref _procedureCodeErrorText, value);
        }

        public bool IsToothCodeRequired
        {
            get => _isToothCodeRequired;
            set => SetProperty(ref _isToothCodeRequired, value);
        }

        public int? ToothCode
        {
            get => _toothCode;
            set
            {
                SetProperty(ref _toothCode, value);
                var validationResult = _toothCodeValidator.Validate(this);
                ToothCodeValid = validationResult.IsValid;

                var validationErrors = validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingToothCodeValidation))
                    ?? validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidToothCodeValidation));
                ToothCodeErrorText = validationErrors?.ErrorMessage;
            }
        }

        public bool? ToothCodeValid
        {
            get => _toothCodeValid;
            set => SetProperty(ref _toothCodeValid, value);
        }

        public string ToothCodeErrorText
        {
            get => _toothCodeErrorText;
            set => SetProperty(ref _toothCodeErrorText, value);
        }

        public bool IsToothSurfaceRequired
        {
            get => _isToothSurfaceRequired;
            set => SetProperty(ref _isToothSurfaceRequired, value);
        }

        public int RequiredToothSurface { get; set; }

        public string ToothSurfaces
        {
            get => _toothSurfaces;
            set
            {
                SetProperty(ref _toothSurfaces, value);
                var validationResult = _toothSurfaceValidator.Validate(this);
                ToothSurfacesValid = validationResult.IsValid;

                var validationErrors = validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingToothSurfaceValidation))
                    ?? validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidToothSurfaceValidation));
                ToothSurfaceErrorText = validationErrors?.ErrorMessage;
            }
        }

        public bool? ToothSurfacesValid
        {
            get => _toothSurfacesValid;
            set => SetProperty(ref _toothSurfacesValid, value);
        }

        public string ToothSurfaceErrorText
        {
            get => _toothSurfaceErrorText;
            set => SetProperty(ref _toothSurfaceErrorText, value);
        }

        public string DentistsFeeStr
        {
            get => _dentistsFeeStr;
            set
            {
                SetProperty(ref _dentistsFeeStr, value);
                if (!String.IsNullOrWhiteSpace(_dentistsFeeStr))
                {
                    DentistsFee = GSCHelper.GetDollarAmount(value);
                }
                else
                {
                    DentistsFee = null;
                }
            }
        }

        public double? DentistsFee
        {
            get => _dentistsFee;
            set
            {
                SetProperty(ref _dentistsFee, value);
                if (!IsLabChargeRequired)
                {
                    var dentistFeeValidationResult = _dentistFeesValidator.Validate(this);
                    DentistsFeeValid = dentistFeeValidationResult.IsValid;

                    var dentistFeeValidationErrors = dentistFeeValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingDentistFeeValidation, StringComparison.OrdinalIgnoreCase))
                        ?? dentistFeeValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidDentistFeeValidation, StringComparison.OrdinalIgnoreCase));
                    DentistFeesErrorText = dentistFeeValidationErrors?.ErrorMessage;
                }
                else
                {
                    var validator = new TreatmentDetailDentistFeesAndLabChargesValidator();
                    var dentistFeeValidtionResult = validator.Validate(this);
                    var isBothFieldsEmptyErrors = dentistFeeValidtionResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingDentistFeeOrLaboratoryChargeValidation, StringComparison.OrdinalIgnoreCase));
                    if (isBothFieldsEmptyErrors == null)
                    {
                        var dentistsFeeErrors = dentistFeeValidtionResult.Errors?.Where(x => string.Equals(x.PropertyName, nameof(DentistsFee), StringComparison.OrdinalIgnoreCase));
                        var dentistFeeValidationErrors = dentistsFeeErrors?.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingDentistFeeValidation, StringComparison.OrdinalIgnoreCase))
                            ?? dentistsFeeErrors?.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidDentistFeeValidation, StringComparison.OrdinalIgnoreCase));
                        DentistFeesErrorText = dentistFeeValidationErrors?.ErrorMessage;

                        var labChargesErrors = dentistFeeValidtionResult.Errors?.Where(x => string.Equals(x.PropertyName, nameof(LaboratoryCharge), StringComparison.OrdinalIgnoreCase));
                        var labChangesValidationErrors = labChargesErrors?.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingLaboratoryChargeValidation, StringComparison.OrdinalIgnoreCase))
                            ?? labChargesErrors?.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidLaboratoryChargeValidation, StringComparison.OrdinalIgnoreCase));
                        LabChargesErrorText = dentistFeeValidationErrors?.ErrorMessage;

                        DentistsFeeValid = string.IsNullOrWhiteSpace(DentistFeesErrorText);
                        LaboratoryChargeValid = string.IsNullOrWhiteSpace(LabChargesErrorText);
                    }
                    else
                    {
                        DentistsFeeValid = false;
                        LaboratoryChargeValid = false;
                        DentistFeesErrorText = isBothFieldsEmptyErrors?.ErrorMessage;
                        LabChargesErrorText = isBothFieldsEmptyErrors?.ErrorMessage;
                    }

                    var amountPaidValidationResult = _amountPaidValidator.Validate(this);
                    AlternateCarrierAmountValid = amountPaidValidationResult.IsValid;

                    var alternateCarrierValidationErrors = amountPaidValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingAmountPaidValidation, StringComparison.OrdinalIgnoreCase))
                        ?? amountPaidValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidAmountPaidValidation, StringComparison.OrdinalIgnoreCase));
                    AlternateCarrierAmountErrorText = alternateCarrierValidationErrors?.ErrorMessage;
                }
            }
        }

        public bool? DentistsFeeValid
        {
            get => _dentistsFeeValid;
            set => SetProperty(ref _dentistsFeeValid, value);
        }

        public string DentistFeesErrorText
        {
            get => _dentistFeesErrorText;
            set => SetProperty(ref _dentistFeesErrorText, value);
        }

        public bool IsLabChargeRequired
        {
            get => _isLabChargeRequired;
            set
            {
                SetProperty(ref _isLabChargeRequired, value);
            }
        }

        public string LaboratoryChargeStr
        {
            get => _laboratoryChargeStr;
            set
            {
                SetProperty(ref _laboratoryChargeStr, value);
                if (!String.IsNullOrWhiteSpace(_laboratoryChargeStr))
                {
                    LaboratoryCharge = GSCHelper.GetDollarAmount(value);
                }
                else
                {
                    LaboratoryCharge = null;
                }
            }
        }

        public double? LaboratoryCharge
        {
            get => _laboratoryCharge;
            set
            {
                SetProperty(ref _laboratoryCharge, value);
                if (!IsLabChargeRequired)
                {
                    var labChargeValidationResult = _labChargesValidator.Validate(this);
                    LaboratoryChargeValid = labChargeValidationResult.IsValid;
                    var labChargeErrors = labChargeValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingLaboratoryChargeValidation, StringComparison.OrdinalIgnoreCase))
                        ?? labChargeValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidLaboratoryChargeValidation, StringComparison.OrdinalIgnoreCase));
                    LabChargesErrorText = labChargeErrors?.ErrorMessage;
                }
                else
                {
                    var validator = new TreatmentDetailDentistFeesAndLabChargesValidator();
                    var dentistFeeValidationResult = validator.Validate(this);
                    var isBothFieldsEmptyErrors = dentistFeeValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingDentistFeeOrLaboratoryChargeValidation, StringComparison.OrdinalIgnoreCase));
                    if (isBothFieldsEmptyErrors == null)
                    {
                        var dentistsFeeErrors = dentistFeeValidationResult.Errors?.Where(x => string.Equals(x.PropertyName, nameof(DentistsFee), StringComparison.OrdinalIgnoreCase));
                        var dentistFeeValidationErrors = dentistsFeeErrors?.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingDentistFeeValidation, StringComparison.OrdinalIgnoreCase))
                            ?? dentistsFeeErrors?.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidDentistFeeValidation, StringComparison.OrdinalIgnoreCase));
                        DentistFeesErrorText = dentistFeeValidationErrors?.ErrorMessage;

                        var labChargesErrors = dentistFeeValidationResult.Errors?.Where(x => string.Equals(x.PropertyName, nameof(LaboratoryCharge), StringComparison.OrdinalIgnoreCase));
                        var labChangesValidationErrors = labChargesErrors?.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingLaboratoryChargeValidation, StringComparison.OrdinalIgnoreCase))
                            ?? labChargesErrors?.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidLaboratoryChargeValidation, StringComparison.OrdinalIgnoreCase));
                        LabChargesErrorText = labChangesValidationErrors?.ErrorMessage;

                        DentistsFeeValid = string.IsNullOrWhiteSpace(DentistFeesErrorText);
                        LaboratoryChargeValid = string.IsNullOrWhiteSpace(LabChargesErrorText);
                    }
                    else
                    {
                        DentistsFeeValid = false;
                        LaboratoryChargeValid = false;
                        DentistFeesErrorText = isBothFieldsEmptyErrors?.ErrorMessage;
                        LabChargesErrorText = isBothFieldsEmptyErrors?.ErrorMessage;
                    }
                }

                var amountPaidValidationResult = _amountPaidValidator.Validate(this);
                AlternateCarrierAmountValid = amountPaidValidationResult.IsValid;

                var alternateCarrierValidationErrors = amountPaidValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingAmountPaidValidation, StringComparison.OrdinalIgnoreCase))
                    ?? amountPaidValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidAmountPaidValidation, StringComparison.OrdinalIgnoreCase));
                AlternateCarrierAmountErrorText = alternateCarrierValidationErrors?.ErrorMessage;
            }
        }

        public bool? LaboratoryChargeValid
        {
            get => _laboratoryChargeValid;
            set => SetProperty(ref _laboratoryChargeValid, value);
        }

        public string LabChargesErrorText
        {
            get => _labChargesErrorText;
            set => SetProperty(ref _labChargesErrorText, value);
        }

        public string AlternateCarrierAmountStr
        {
            get => _alternateCarrierAmountStr;
            set
            {
                SetProperty(ref _alternateCarrierAmountStr, value);
                if (!String.IsNullOrWhiteSpace(_alternateCarrierAmountStr))
                {
                    AlternateCarrierAmount = GSCHelper.GetDollarAmount(value);
                }
                else
                {
                    AlternateCarrierAmount = null;
                }
            }
        }

        public double? AlternateCarrierAmount
        {
            get => _alternateCarrierAmount;
            set
            {
                SetProperty(ref _alternateCarrierAmount, value);
                var amountPaidValidationResult = _amountPaidValidator.Validate(this);
                AlternateCarrierAmountValid = amountPaidValidationResult.IsValid;

                var alternateCarrierValidationErrors = amountPaidValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingAmountPaidValidation, StringComparison.OrdinalIgnoreCase))
                    ?? amountPaidValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidAmountPaidValidation, StringComparison.OrdinalIgnoreCase));
                AlternateCarrierAmountErrorText = alternateCarrierValidationErrors?.ErrorMessage;
            }
        }

        public bool? AlternateCarrierAmountValid
        {
            get => _alternateCarrierAmountValid;
            set => SetProperty(ref _alternateCarrierAmountValid, value);
        }

        public string AlternateCarrierAmountErrorText
        {
            get => _alternateCarrierAmountErrorText;
            set => SetProperty(ref _alternateCarrierAmountErrorText, value);
        }

        public bool IsAlternateCarrierAmountVisible
        {
            get => _isAlternateCarrierAmountVisible;
            set => SetProperty(ref _isAlternateCarrierAmountVisible, value);
        }

        public IMvxCommand SaveTreatmentCommand { get; }

        public IMvxCommand DeleteTreatmentCommand { get; }

        public IMvxCommand CancelCommand { get; }

        public IMvxCommand ValidateDentalTreatmentDetailCommand { get; }

        public ClaimTreatmentDetailsEntryDentalViewModel(IClaimService claimService,
            ITreatmentDetailService treatmentDetailService,
            ILoginService loginService,
            IUserDialogs userDialogs)
        {
            IsToothCodeRequired = false;
            IsToothSurfaceRequired = false;
            IsLabChargeRequired = false;

            _treatmentDateValidator = new TreatmentDetailDateValidator();
            _procedureCodeValidator = new TreatmentDetailProcedureCodeValidator();
            _toothCodeValidator = new TreatmentDetailToothCodeValidator();
            _toothSurfaceValidator = new TreatmentDetailToothSurfaceValidator();
            _dentistFeesValidator = new TreatmentDetailDentistFeesValidator();
            _labChargesValidator = new TreatmentDetailLabChargesValidator();
            _amountPaidValidator = new TreatmentDetailAmountPaidValidator();

            DateOfTreatment = DateTime.Now;

            _claimService = claimService;
            _treatmentDetailService = treatmentDetailService;
            _loginService = loginService;
            _userDialog = userDialogs;

            SaveTreatmentCommand = new MvxCommand(ExecuteSaveTreatmentCommand);
            DeleteTreatmentCommand = new MvxCommand(ExecuteDeleteTreatmentCommand);
            CancelCommand = new MvxCommand(ExecuteCancelCommand);
            ValidateDentalTreatmentDetailCommand = new MvxCommand(ExecuteValidateDentalTreatmentDetailCommand);

            if (_claimService.Claim != null)
            {
                IsAlternateCarrierAmountVisible = _claimService.Claim.HasClaimBeenSubmittedToOtherBenefitPlan;
            }

            SelectedClaimSubmissionType = _claimService.SelectedClaimSubmissionType;
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            if (_claimService.SelectedTreatmentDetailID != null && _claimService.SelectedTreatmentDetailID != Guid.Empty)
            {
                EditMode = true;
                _selectedTreatmentId = _claimService.SelectedTreatmentDetailID;
                PopulateFields();
            }
        }

        public event EventHandler ClaimTreatmentEntrySuccess;
        protected virtual void RaiseClaimTreatmentEntrySuccess(EventArgs e)
        {
            ClaimTreatmentEntrySuccess?.Invoke(this, e);
        }

        private async void ExecuteValidateDentalTreatmentDetailCommand()
        {
            try
            {
                await ValidateDentalTreatmentUsingApi(CheckForApiValidationFailure, CheckForApiValidationError);
            }
            //404,400,500 or other errornous http response
            catch (Exception ex) when (ex is ApiException)
            {
                CheckForApiValidationError(null);
                SetProcedureCodeWithErrorMessage();
            }
            catch (Exception)
            {
                ResetFields();
                _userDialog.Alert(Resource.GenericErrorDialogMessage, Resource.GenericErrorDialogTitle, Resource.ok);
            }

        }

        private async Task ValidateDentalTreatmentUsingApi(
            Action<List<ValidateDentalTreatmentResponse>> checkForApiValidationFailure,
            Action<List<ValidateDentalTreatmentResponse>> checkForApiValidationError)
        {
            var validationResult = await _treatmentDetailService.ValidateDentalTreatmentAsync(new ValidateDentalTreatmentRequest()
            {
                PlanMemberId = _loginService.GroupPlanNumber,
                DentalProcedureCode = ProcedureCode.ToString(),
                ServiceDate = DateOfTreatment.ToString("d"),
                ProviderProvinceCode = _claimService.Claim.Provider.Province,
                ToothCode = string.Empty,
                ToothSurface = string.Empty
            });

            checkForApiValidationFailure?.Invoke(validationResult);

            checkForApiValidationError?.Invoke(validationResult);
        }

        private void CheckForApiValidationFailure(List<ValidateDentalTreatmentResponse> validationResultsList)
        {
            if (validationResultsList?.FirstOrDefault() == null)
            {
                ResetFields();
            }
        }

        private void CheckForApiValidationFailureForSaveButtonClick(List<ValidateDentalTreatmentResponse> validationResultsList)
        {
            if (validationResultsList?.FirstOrDefault() == null)
            {
                SetProcedureCodeWithErrorMessage();
            }
        }

        private void CheckForApiValidationError(List<ValidateDentalTreatmentResponse> validationResultsList)
        {
            if (validationResultsList == null || validationResultsList.FirstOrDefault() == null)
            {
                return;
            }

            var validationResult = validationResultsList.FirstOrDefault();
            IsToothCodeRequired = validationResult.IsToothCodeRequired;
            if (!IsToothCodeRequired)
            {
                ToothCode = null;
                ToothCodeErrorText = null;
                ToothCodeValid = null;
            }

            RequiredToothSurface = validationResult.RequiredToothSurfaceCount;
            IsToothSurfaceRequired = validationResult.IsToothSurfaceRequired;
            if (!IsToothSurfaceRequired)
            {
                ToothSurfaces = null;
                ToothSurfaceErrorText = null;
                ToothSurfacesValid = null;
            }

            IsLabChargeRequired = validationResult.IsLaboratoryChargeRequired;
            if (!IsLabChargeRequired)
            {
                LaboratoryCharge = null;
                LabChargesErrorText = null;
                LaboratoryChargeValid = null;
            }
        }

        private async void ExecuteSaveTreatmentCommand()
        {
            ValidationFailure toothCodeValidationErrors = null;
            ValidationFailure toothSurfaceValidationErrors = null;

            int.TryParse(ProcedureCode, out int procedureCode);

            var treatmentDateValidationResult = _treatmentDateValidator.Validate(this);
            DateValid = treatmentDateValidationResult.IsValid;
            var treatmentDateValidationErrors = treatmentDateValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidToothSurfaceValidation))
                ?? treatmentDateValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidTreatmentDate24MonthValidation));
            DateOfTreatmentErrorText = treatmentDateValidationErrors?.ErrorMessage;

            var procedureCodeValidationResult = _procedureCodeValidator.Validate(this);
            ProcedureCodeValid = procedureCodeValidationResult.IsValid;
            var procedureCodeValidationErrors = procedureCodeValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingProcedureCodeValidation))
                ?? procedureCodeValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidProcedureCodeValidation));
            ProcedureCodeErrorText = procedureCodeValidationErrors?.ErrorMessage;

            if (procedureCodeValidationResult.IsValid)
            {
                try
                {
                    await ValidateDentalTreatmentUsingApi(CheckForApiValidationFailureForSaveButtonClick, CheckForApiValidationError);
                }
                //404,400,500 or other errornous http response
                catch (Exception ex) when (ex is ApiException)
                {
                    CheckForApiValidationError(null);
                    SetProcedureCodeWithErrorMessage();
                }
                catch (Exception ex)
                {
                    ResetFields();
                    _userDialog.Alert(Resource.GenericErrorDialogMessage, Resource.GenericErrorDialogTitle, Resource.ok);
                }
            }

            if (IsToothCodeRequired)
            {
                var toothCodeValidationResult = _toothCodeValidator.Validate(this);
                ToothCodeValid = toothCodeValidationResult.IsValid;
                toothCodeValidationErrors = toothCodeValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingToothCodeValidation))
                    ?? toothCodeValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidToothCodeValidation));
                ToothCodeErrorText = toothCodeValidationErrors?.ErrorMessage;
            }

            if (IsToothSurfaceRequired)
            {
                var toothSurfaceValidationResult = _toothSurfaceValidator.Validate(this);
                ToothSurfacesValid = toothSurfaceValidationResult.IsValid;
                toothSurfaceValidationErrors = toothSurfaceValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingToothSurfaceValidation))
                    ?? toothSurfaceValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidToothSurfaceValidation));
                ToothSurfaceErrorText = toothSurfaceValidationErrors?.ErrorMessage;
            }

            if (IsLabChargeRequired)
            {
                var validator = new TreatmentDetailDentistFeesAndLabChargesValidator();
                var dentistFeeValidationResult = validator.Validate(this);
                var isBothFieldsEmptyErrors = dentistFeeValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingDentistFeeOrLaboratoryChargeValidation, StringComparison.OrdinalIgnoreCase));
                if (isBothFieldsEmptyErrors == null)
                {
                    var dentistsFeeErrors = dentistFeeValidationResult.Errors?.Where(x => string.Equals(x.PropertyName, nameof(DentistsFee), StringComparison.OrdinalIgnoreCase));
                    var dentistFeeValidationErrors = dentistsFeeErrors?.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingDentistFeeValidation, StringComparison.OrdinalIgnoreCase))
                        ?? dentistsFeeErrors?.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidDentistFeeValidation, StringComparison.OrdinalIgnoreCase));
                    DentistFeesErrorText = dentistFeeValidationErrors?.ErrorMessage;

                    var labChargesErrors = dentistFeeValidationResult.Errors?.Where(x => string.Equals(x.PropertyName, nameof(LaboratoryCharge), StringComparison.OrdinalIgnoreCase));
                    var labChangesValidationErrors = labChargesErrors?.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingLaboratoryChargeValidation, StringComparison.OrdinalIgnoreCase))
                        ?? labChargesErrors?.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidLaboratoryChargeValidation, StringComparison.OrdinalIgnoreCase));
                    LabChargesErrorText = labChangesValidationErrors?.ErrorMessage;

                    DentistsFeeValid = string.IsNullOrWhiteSpace(DentistFeesErrorText);
                    LaboratoryChargeValid = string.IsNullOrWhiteSpace(LabChargesErrorText);
                }
                else
                {
                    DentistsFeeValid = false;
                    LaboratoryChargeValid = false;
                    DentistFeesErrorText = isBothFieldsEmptyErrors?.ErrorMessage;
                    LabChargesErrorText = isBothFieldsEmptyErrors?.ErrorMessage;
                }
            }
            else
            {
                var dentistFeeValidationResult = _dentistFeesValidator.Validate(this);
                DentistsFeeValid = dentistFeeValidationResult.IsValid;
                var dentistFeeValidationErrors = dentistFeeValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingDentistFeeValidation, StringComparison.OrdinalIgnoreCase))
                    ?? dentistFeeValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidDentistFeeValidation, StringComparison.OrdinalIgnoreCase));
                DentistFeesErrorText = dentistFeeValidationErrors?.ErrorMessage;
            }

            var amountPaidValidationResult = _amountPaidValidator.Validate(this);
            AlternateCarrierAmountValid = amountPaidValidationResult.IsValid;
            var alternateCarrierValidationErrors = amountPaidValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.MissingAmountPaidValidation, StringComparison.OrdinalIgnoreCase))
                ?? amountPaidValidationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidAmountPaidValidation, StringComparison.OrdinalIgnoreCase));
            AlternateCarrierAmountErrorText = alternateCarrierValidationErrors?.ErrorMessage;

            if (!(DateValid.HasValue && DateValid.Value)
                || !(ProcedureCodeValid.HasValue && ProcedureCodeValid.Value)
                || (IsToothCodeRequired && !(ToothCodeValid.HasValue && ToothCodeValid.Value))
                || (IsToothSurfaceRequired && !(ToothSurfacesValid.HasValue && ToothSurfacesValid.Value))
                || !(DentistsFeeValid.HasValue && DentistsFeeValid.Value)
                || (IsLabChargeRequired && !(LaboratoryChargeValid.HasValue && LaboratoryChargeValid.Value))
                || !(AlternateCarrierAmountValid.HasValue && AlternateCarrierAmountValid.Value))
            {
                return;
            }

            if (EditMode)
            {
                var selectedTreatmentDetail = _claimService.Claim.GetTreatmentDetailByID(_selectedTreatmentId);
                if (selectedTreatmentDetail != null)
                {
                    selectedTreatmentDetail.ID = Guid.NewGuid();
                    selectedTreatmentDetail.ClaimSubmissionType = SelectedClaimSubmissionType;
                    selectedTreatmentDetail.DentalSubmisionBenefit = new ClaimSubmissionBenefit()
                    {
                        ID = procedureCode,
                        Name = $"{SelectedClaimSubmissionType.Name} - {ProcedureCode}",
                        ProcedureCode = ProcedureCode
                    };
                    selectedTreatmentDetail.TreatmentDate = DateOfTreatment;
                    selectedTreatmentDetail.ToothCode = ToothCode;
                    selectedTreatmentDetail.ToothSurface = ToothSurfaces;
                    selectedTreatmentDetail.ProcedureCode = ProcedureCode;
                    selectedTreatmentDetail.DentistFees = DentistsFee;
                    selectedTreatmentDetail.LaboratoryCharges = LaboratoryCharge;
                    selectedTreatmentDetail.AlternateCarrierPayment = AlternateCarrierAmount ?? 0;

                    selectedTreatmentDetail.IsLaboratoryChargesRequired = IsLabChargeRequired;
                    selectedTreatmentDetail.IsToothCodeRequired = IsToothCodeRequired;
                    selectedTreatmentDetail.IsToothSurfaceRequired = IsToothSurfaceRequired;
                    selectedTreatmentDetail.IsAlternateCarrierPaymentVisible = IsAlternateCarrierAmountVisible;
                    _claimService.SelectedTreatmentDetailID = Guid.Empty;
                    _claimService.PersistClaim();
                    Close(this);
                    RaiseClaimTreatmentEntrySuccess(new EventArgs());
                }
                else
                {
                    // TODO: Decide what to do? show error?
                }
            }
            else
            {
                var treatmentDetails = new TreatmentDetail()
                {
                    ID = Guid.NewGuid(),
                    ClaimSubmissionType = SelectedClaimSubmissionType,
                    DentalSubmisionBenefit = new ClaimSubmissionBenefit()
                    {
                        ID = procedureCode,
                        Name = $"{SelectedClaimSubmissionType.Name} - {ProcedureCode}",
                        ProcedureCode = ProcedureCode
                    },
                    TreatmentDate = DateOfTreatment,
                    ToothCode = ToothCode,
                    ToothSurface = ToothSurfaces,
                    ProcedureCode = ProcedureCode,
                    DentistFees = DentistsFee,
                    LaboratoryCharges = LaboratoryCharge,
                    AlternateCarrierPayment = AlternateCarrierAmount ?? 0,
                    IsToothCodeRequired = IsToothCodeRequired,
                    IsToothSurfaceRequired = IsToothSurfaceRequired,
                    IsLaboratoryChargesRequired = IsLabChargeRequired,
                    IsAlternateCarrierPaymentVisible = IsAlternateCarrierAmountVisible
                };

                _claimService.Claim.TreatmentDetails.Add(treatmentDetails);
                _claimService.PersistClaim();
                Close(this);
                RaiseClaimTreatmentEntrySuccess(new EventArgs());
                if (_claimService.Claim.TreatmentDetails.Count >= 1 && !_claimService.IsTreatmentDetailsListInNavStack)
                {
                    ShowViewModel<ClaimTreatmentDetailsListViewModel>();
                }
            }
        }

        private void ExecuteDeleteTreatmentCommand()
        {
            TreatmentDetail tdToRemove = _claimService.Claim.GetTreatmentDetailByID(_selectedTreatmentId);
            _claimService.Claim.TreatmentDetails.Remove(tdToRemove);
            _claimService.SelectedTreatmentDetailID = Guid.Empty;
            _claimService.PersistClaim();
            Close(this);
        }

        private void ExecuteCancelCommand()
        {
            Close(this);
        }

        private void PopulateFields()
        {
            var treatmentDetail = _claimService.Claim.GetTreatmentDetailByID(_selectedTreatmentId);

            if (treatmentDetail != null)
            {
                IsToothCodeRequired = treatmentDetail.IsToothCodeRequired;
                IsToothSurfaceRequired = treatmentDetail.IsToothSurfaceRequired;
                IsLabChargeRequired = treatmentDetail.IsLaboratoryChargesRequired;

                DateOfTreatment = treatmentDetail.TreatmentDate;
                ProcedureCode = treatmentDetail.ProcedureCode.ToString();

                ToothCode = treatmentDetail.ToothCode;
                ToothSurfaces = treatmentDetail.ToothSurface;
                DentistsFee = treatmentDetail.DentistFees;
                DentistsFeeStr = treatmentDetail.DentistFees.ToString();
                LaboratoryCharge = treatmentDetail.LaboratoryCharges;
                LaboratoryChargeStr = treatmentDetail.LaboratoryCharges.ToString();
                AlternateCarrierAmount = treatmentDetail.AlternateCarrierPayment;
                AlternateCarrierAmountStr = treatmentDetail.AlternateCarrierPayment.ToString();

            }
        }

        private void ResetFields()
        {
            IsToothCodeRequired = false;
            IsToothSurfaceRequired = false;
            IsLabChargeRequired = false;
            RequiredToothSurface = 0;

            SetProcedureCodeWithErrorMessage();

            ToothCode = null;
            ToothCodeErrorText = null;
            ToothCodeValid = null;

            ToothSurfaces = null;
            ToothSurfaceErrorText = null;
            ToothSurfacesValid = null;

            DentistsFee = null;
            DentistFeesErrorText = null;
            DentistsFeeValid = null;

            LaboratoryCharge = null;
            LabChargesErrorText = null;
            LaboratoryChargeValid = null;

            AlternateCarrierAmount = null;
            AlternateCarrierAmountErrorText = null;
            AlternateCarrierAmountValid = null;
        }

        private void SetProcedureCodeWithErrorMessage()
        {
            ProcedureCodeErrorText = Resource.InvalidProcedureCodeValidation;
            ProcedureCodeValid = false;
        }
    }
}