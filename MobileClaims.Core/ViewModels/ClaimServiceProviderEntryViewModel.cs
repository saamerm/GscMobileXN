using FluentValidation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using System.ComponentModel;
using MobileClaims.Core.Validators;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimServiceProviderEntryViewModel : ViewModelBase
    {
        public delegate EventHandler MissingFields(object sender, EventArgs e);

        public string Title => Resource.ProviderEntryTitle;
        public string EnterDetailsLabel => Resource.ProviderEntryEnterDetails;
        public string MandatoryFieldsLabel => Resource.ProviderEntryMandatoryFields;
        public string NameLabel => Resource.ProviderEntryName;
        public string Address1Label => Resource.ProviderEntryAddress1;
        public string Address2Label => Resource.ProviderEntryAddress2;
        public string CityLabel => Resource.ProviderEntryCity;
        public string ProvinceLabel => Resource.ProviderEntryProvince;
        public string PostalCodeLabel => Resource.ProviderEntryPostalCode;
        public string PostalCodeLabelAndroid => Resource.ProviderEntryPostalCodeAndroid;
        public string PhoneLabel => Resource.ProviderEntryPhone;
        public string RegistrationLabel => Resource.ProviderEntryRegistration;
        public string ButtonLabel => Resource.ProviderEntryButtonText;

        public ICommand UseThisProviderCommand => new MvxCommand(ExecuteUseThisProvider);
        public ClaimSubmissionType ClaimSubmissionType => _claimService.SelectedClaimSubmissionType;

        private readonly IClaimService _claimService;
        private readonly IMvxMessenger _messenger;

        // MERGE
        private readonly IProviderLookupService _providerService;
        private readonly MvxSubscriptionToken _shouldCloseSelf;
        private readonly IUserDialogs _dialogService;

        private readonly ProviderEntryNameValidator _nameValidator;
        private readonly ProviderEntryAddress1Validator _address1Validator;
        private readonly ProviderEntryAddress2Validator _address2Validator;
        private readonly ProviderEntryCityValidator _cityValidator;
        private readonly ProviderEntryPhoneNumberValidator _phoneValidator;
        private readonly ProviderEntryPostalCodeValidator _postalCodeValidator;
        private readonly ProviderEntryProvinceValidator _provinceValidator;
        private readonly ProviderEntryRegistrationNumberValidator _registrationNumberValidator;

        private string _name;
        private bool? _nameValid;

        private string _address1;
        private bool? _address1Valid;

        private string _address2;
        private bool? _address2Valid;

        private string _city;
        private bool? _cityValid;

        private string _phone;
        private bool? _phoneValid;

        private string _postalCode;
        private bool? _postalCodeValid;

        private List<ServiceProviderProvince> _provinces;

        private string _registrationNumber;
        private bool? _registrationNumberValid;

        private ServiceProviderProvince _selectedProvince;
        private bool? _selectedProvinceValid;

        public ClaimServiceProviderEntryViewModel(
            IMvxMessenger messenger,
            IClaimService claimService,
            IProviderLookupService providerService)
        {
            _messenger = messenger;
            _claimService = claimService;
            _providerService = providerService;
            _dialogService = Mvx.IoCProvider.Resolve<IUserDialogs>();


            _address1Validator = new ProviderEntryAddress1Validator();
            _address2Validator = new ProviderEntryAddress2Validator();
            _cityValidator = new ProviderEntryCityValidator();
            _provinceValidator = new ProviderEntryProvinceValidator();
            _nameValidator = new ProviderEntryNameValidator();
            _phoneValidator = new ProviderEntryPhoneNumberValidator();
            _postalCodeValidator = new ProviderEntryPostalCodeValidator();
            _registrationNumberValidator = new ProviderEntryRegistrationNumberValidator();

            // Initializing optional values to prevent null pointer exceptions from fluent validation lib
            Address2 = string.Empty;
            RegistrationNumber = string.Empty;

            if (_providerService.ServiceProviderProvinces != null && _providerService.ServiceProviderProvinces.Count > 0)
            {
                Provinces = _providerService.ServiceProviderProvinces;
                RepopulateValues();
            }
            else
            {
                _dialogService.ShowLoading();
                _providerService.GetServiceProviderProvinces(() =>
                {
                    Provinces = _providerService.ServiceProviderProvinces;
                    RepopulateValues();
                    _dialogService.HideLoading();
                }, (message, code) =>
                {
                    RepopulateValues();
                    _dialogService.HideLoading();
                });
            }

            if (_claimService.Claim?.Provider != null && Provinces != null)
            {
                _selectedProvince = Provinces.FirstOrDefault(x => x.ID.Equals(_claimService.Claim.Provider.Province, StringComparison.OrdinalIgnoreCase));
            }
            else if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice == GSCHelper.OS.iOS)
            {
                // this is to prevent setter from calling valid, and setting valid property to false for first time load. 
                _selectedProvince = new ServiceProviderProvince();
                _selectedProvince.Name = ProvinceLabel;
            }

            _shouldCloseSelf = _messenger.Subscribe<ClearClaimServiceProviderEntryViewRequested>(message =>
            {
                _messenger.Unsubscribe<ClearClaimServiceProviderEntryViewRequested>(_shouldCloseSelf);
                Close(this);
            });
        }

        public string Name
        {
            get => _name;
            set
            {
                if (value != null && value.Length <= ProviderEntryValidationParams.NAME_MAX_LENGTH)
                {
                    _name = value;
                    NameValid = _nameValidator.Validate(this).IsValid;
                }
                RaisePropertyChanged();
            }
        }

        public bool? NameValid
        {
            get => _nameValid;
            set => SetProperty(ref _nameValid, value);
        }

        public override Task RaisePropertyChanged(System.ComponentModel.PropertyChangedEventArgs changedArgs)
        {
            _claimService.PersistClaim();
            return base.RaisePropertyChanged(changedArgs);
        }

        public override Task RaiseAllPropertiesChanged()
        {
            _claimService.PersistClaim();
            return base.RaiseAllPropertiesChanged();
        }

        public string Address1
        {
            get => _address1;
            set
            {
                if (value != null && value.Length <= ProviderEntryValidationParams.ADDRESS1_MAX_LENGTH)
                {
                    _address1 = value;
                    Address1Valid = _address1Validator.Validate(this).IsValid;
                }
                RaisePropertyChanged();
            }
        }

        public bool? Address1Valid
        {
            get => _address1Valid;
            set => SetProperty(ref _address1Valid, value);
        }

        public string Address2
        {
            get => _address2;
            set
            {
                if (value != null && value.Length <= ProviderEntryValidationParams.ADDRESS2_MAX_LENGTH)
                {
                    _address2 = value;
                    Address2Valid = _address2Validator.Validate(this).IsValid;
                }
                RaisePropertyChanged();
            }
        }

        public bool? Address2Valid
        {
            get => _address2Valid;
            set => SetProperty(ref _address2Valid, value);
        }

        public string City
        {
            get => _city;
            set
            {
                if (value != null && value.Length <= ProviderEntryValidationParams.CITY_MAX_LENGTH)
                {
                    _city = value;
                    CityValid = _cityValidator.Validate(this).IsValid;
                }
                RaisePropertyChanged();
            }
        }

        public bool? CityValid
        {
            get => _cityValid;
            set => SetProperty(ref _cityValid, value);
        }

        public ServiceProviderProvince SelectedProvince
        {
            get => _selectedProvince;
            set
            {
                SetProperty(ref _selectedProvince, value);
                if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice == GSCHelper.OS.Droid)
                {
                    SelectedProvinceValid = value != null;
                }
                else
                {
                    SelectedProvinceValid = _provinceValidator.Validate(this).IsValid;
                }
            }
        }

        public bool? SelectedProvinceValid
        {
            get => _selectedProvinceValid;
            set => SetProperty(ref _selectedProvinceValid, value);
        }

        public string PostalCode
        {
            get => _postalCode;
            set
            {
                if (value != null && value.Length <= ProviderEntryValidationParams.POSTAL_CODE_MAX_LENGTH)
                {
                    _postalCode = value;
                    PostalCodeValid = _postalCodeValidator.Validate(this).IsValid;
                }
                RaisePropertyChanged();
            }
        }

        public bool? PostalCodeValid
        {
            get => _postalCodeValid;
            set => SetProperty(ref _postalCodeValid, value);
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (value != null && value.Length <= ProviderEntryValidationParams.PHONE_MAX_LENGTH)
                {
                    _phone = value;
                    PhoneValid = _phoneValidator.Validate(this).IsValid;
                }
                RaisePropertyChanged();
            }
        }

        public bool? PhoneValid
        {
            get => _phoneValid;
            set => SetProperty(ref _phoneValid, value);
        }

        public string RegistrationNumber
        {
            get => _registrationNumber;
            set
            {
                if (value != null && value.Length <= ProviderEntryValidationParams.REGISTRATION_NUMBER_MAX_LENGTH)
                {
                    _registrationNumber = value;
                    RegistrationNumberValid = _registrationNumberValidator.Validate(this).IsValid;
                }
                RaisePropertyChanged();
            }
        }

        public bool? RegistrationNumberValid
        {
            get => _registrationNumberValid;
            set => SetProperty(ref _registrationNumberValid, value);
        }

        public List<ServiceProviderProvince> Provinces
        {
            get => _provinces;
            set => SetProperty(ref _provinces, value);
        }

        private void ExecuteUseThisProvider()
        {
            if (!SubmitValuesValid())
            {
                return;
            }

            //save values in a provider, as well as in the special properties
            _claimService.Claim.ServiceProviderEntryName = Name.ConvertToEnglish().Trim();
            _claimService.Claim.ServiceProviderEntryAddress1 = Address1.ConvertToEnglish().Trim();
            _claimService.Claim.ServiceProviderEntryAddress2 = Address2?.ConvertToEnglish().Trim();
            _claimService.Claim.ServiceProviderEntryCity = City.ConvertToEnglish().Trim();
            _claimService.Claim.ServiceProviderEntryProvince = SelectedProvince;
            _claimService.Claim.ServiceProviderEntryPostalCode = PostalCode.RemoveWhitespace().ToUpper();
            _claimService.Claim.ServiceProviderEntryPhone = Phone.PreparePhoneNumber();
            _claimService.Claim.ServiceProviderEntryRegistrationNumber = RegistrationNumber;

            var provider = new ServiceProvider { ID = -1, BusinessName = Name, Address = Address1 };
            if (!string.IsNullOrEmpty(Address2))
            {
                provider.AddressLine2 = Address2;
            }

            provider.City = City.ConvertToEnglish().Trim();
            provider.Province = SelectedProvince.ID;
            provider.PostalCode = PostalCode.RemoveWhitespace().ToUpper();
            provider.Phone = Phone.PreparePhoneNumber();
            provider.RegistrationNumber = RegistrationNumber;

            _claimService.Claim.Provider = provider;
            _claimService.PersistClaim();
            PublishMessages();
            ShowViewModel<ClaimParticipantsViewModel>();
        }

        private bool SubmitValuesValid()
        {
            if (new List<bool>
            {
                string.IsNullOrWhiteSpace(Name),
                string.IsNullOrWhiteSpace(Address1),
                string.IsNullOrWhiteSpace(City),
                string.IsNullOrWhiteSpace(SelectedProvince.ID),
                string.IsNullOrWhiteSpace(PostalCode),
                string.IsNullOrWhiteSpace(Phone)
            }.Any(empty => empty))
            {
                _dialogService.Alert(Resource.ProviderEntryAllMissing);
                NameValid = !string.IsNullOrWhiteSpace(Name) && _nameValidator.Validate(this).IsValid;
                Address1Valid = !string.IsNullOrWhiteSpace(Address1) && _address1Validator.Validate(this).IsValid;
                CityValid = !string.IsNullOrWhiteSpace(City) && _cityValidator.Validate(this).IsValid;
                SelectedProvinceValid = SelectedProvince != null && _provinceValidator.Validate(this).IsValid;
                PostalCodeValid = !string.IsNullOrWhiteSpace(PostalCode) && _postalCodeValidator.Validate(this).IsValid;
                PhoneValid = !string.IsNullOrWhiteSpace(Phone) && _phoneValidator.Validate(this).IsValid;
                return false;
            }

            if (!_nameValidator.Validate(this).IsValid)
            {
                NameValid = false;
                _dialogService.Alert(Resource.ProviderEntryNameInvalid);
                return false;
            }
            NameValid = true;

            if (!_address1Validator.Validate(this).IsValid)
            {
                Address1Valid = false;
                _dialogService.Alert(Resource.ProviderEntryAddressInvalid);
                return false;
            }
            Address1Valid = true;

            if (!_address2Validator.Validate(this).IsValid)
            {
                Address2Valid = false;
                _dialogService.Alert(Resource.ProviderEntryAddressInvalid);
                return false;
            }
            Address2Valid = true;

            if (!_cityValidator.Validate(this).IsValid)
            {
                CityValid = false;
                _dialogService.Alert(Resource.ProviderEntryCityInvalid);
                return false;
            }
            CityValid = true;

            if (!_provinceValidator.Validate(this).IsValid)
            {
                SelectedProvinceValid = false;
                _dialogService.Alert(Resource.ProviderEntryAllMissing);
                return false;
            }
            SelectedProvinceValid = true;

            if (!_postalCodeValidator.Validate(this).IsValid)
            {
                PostalCodeValid = false;
                _dialogService.Alert(Resource.ProviderEntryPostalCodeInvalid);
                return false;
            }
            PostalCodeValid = true;

            if (!_phoneValidator.Validate(this).IsValid)
            {
                PhoneValid = false;
                _dialogService.Alert(Resource.ProviderEntryPhoneInvalid);
                return false;
            }
            PhoneValid = true;

            if (!_registrationNumberValidator.Validate(this).IsValid)
            {
                RegistrationNumberValid = false;
                _dialogService.Alert(Resource.ProviderEntryRegistrationNumberInvalid);
                return false;
            }
            RegistrationNumberValid = true;

            return true;
        }

        public event EventHandler CloseServiceProviderEntryPopup;

        protected virtual void OnCloseServiceProviderEntryPopup(EventArgs e)
        {
            CloseServiceProviderEntryPopup?.Invoke(this, e);
        }

        private void PublishMessages()
        {
            _messenger.Publish(new ClearClaimTreatmentDetailsViewRequested(this));
            _messenger.Publish(new ClearClaimParticipantsViewRequested(this));
            _messenger.Publish(new ClearClaimDetailsViewRequested(this));
            _messenger.Publish(new ClearClaimTreatmentDetailsListViewRequested(this));
            _messenger.Publish(new ClearClaimSubmitTermsAndConditionsViewRequested(this));
            _messenger.Publish(new ClearClaimSubmissionConfirmationViewRequested(this));
            _messenger.Publish(new ClearClaimSubmissionResultViewRequested(this));
            OnCloseServiceProviderEntryPopup(new EventArgs());
        }

        private void RepopulateValues()
        {
            if (_claimService.Claim != null)
            {
                Name = _claimService.Claim.ServiceProviderEntryName;
                Address1 = _claimService.Claim.ServiceProviderEntryAddress1;
                Address2 = _claimService.Claim.ServiceProviderEntryAddress2;
                City = _claimService.Claim.ServiceProviderEntryCity;
                PostalCode = _claimService.Claim.ServiceProviderEntryPostalCode;
                Phone = _claimService.Claim.ServiceProviderEntryPhone;
                RegistrationNumber = _claimService.Claim.ServiceProviderEntryRegistrationNumber;

                if (Provinces != null && Provinces.Count > 0 &&
                    _claimService.Claim.ServiceProviderEntryProvince != null
                )
                {
                    SelectedProvince = Provinces.FirstOrDefault(p => p.ID == _claimService.Claim.ServiceProviderEntryProvince.ID);
                }
            }
        }
    }

    public static class ProviderEntryValidationParams
    {
        // Due to android edittext behaviour, these limits are also set in android layouts
        public const int NAME_MAX_LENGTH = 60;
        public const int ADDRESS1_MAX_LENGTH = 40;
        public const int ADDRESS2_MAX_LENGTH = 40;
        public const int CITY_MAX_LENGTH = 40;
        public const int POSTAL_CODE_MIN_LENGTH = 6;
        public const int POSTAL_CODE_MAX_LENGTH = 7;
        public const int PHONE_MAX_LENGTH = 10;
        public const int REGISTRATION_NUMBER_MAX_LENGTH = 50;

        public const string UnsafeCharsRegEx = @"^[^<>%""”“;&+]+$";
        public const string UnsafeCharsRegExWithEmptyText = @"^[^<>%""”“;&+]+$|^$";

        public static string ConvertToEnglish(this string input)
        {
            if (input == null)
            {
                return null;
            }

            var newString = input;
            newString = Regex.Replace(newString, @"[\xC0-\xC5]", "A"); // Convert À, Á, Â, Ã, Ä, and Å to A
            newString = Regex.Replace(newString, @"[\xC6]", "AE"); // Convert Æ to AE
            newString = Regex.Replace(newString, @"[\xC7]", "C"); // Convert Ç to C
            newString = Regex.Replace(newString, @"[\xC8-\xCB]", "E"); // Convert È, É, Ê, and Ë to E
            newString = Regex.Replace(newString, @"[\xCC-\xCF]", "I"); // Convert Ì, Í, Î, and Ï to I
            newString = Regex.Replace(newString, @"[\xD1]", "N"); // Convert Ñ to N
            newString = Regex.Replace(newString, @"[\xD2-\xD6]", "O"); // Convert Ò, Ó, Ô, Õ and Ö to O
            newString = Regex.Replace(newString, @"[\xD9-\xDC]", "U"); // Convert Ù, Ú, Û, and Ü to U
            newString = Regex.Replace(newString, @"[\xE0-\xE5]", "a"); // Convert à, á, â, ã, ä, and å to a
            newString = Regex.Replace(newString, @"[\xE6]", "ae"); // Convert æ to ae
            newString = Regex.Replace(newString, @"[\xE7]", "c"); // Convert ç to c
            newString = Regex.Replace(newString, @"[\xE8-\xEB]", "e"); // Convert è, é, ê, and ë to e
            newString = Regex.Replace(newString, @"[\xEC-\xEF]", "i"); // Convert ì, í, î, and ï to i
            newString = Regex.Replace(newString, @"[\xF1]", "n"); // Convert ñ to n
            newString = Regex.Replace(newString, @"[\xF2-\xF6]", "o"); // Convert ò, ó, ô, õ and ö to o
            newString = Regex.Replace(newString, @"[\xF9-\xFC]", "u"); // Convert ù, ú, û, and ü to u
            newString = Regex.Replace(newString, "[Œ]", "OE"); // Convert Œ to OE
            newString = Regex.Replace(newString, "[œ]", "oe"); // Convert œ to oe
            newString = Regex.Replace(newString, "[Ÿ]", "Y"); // Convert Ÿ to Y

            return input;
        }

        public static string PreparePhoneNumber(this string input)
        {
            // Remove common phone number inputs
            return input?.Replace("-", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace(" ", "");
        }

        public static string RemoveWhitespace(this string input)
        {
            if (input == null)
            {
                return null;
            }

            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }
    }
}