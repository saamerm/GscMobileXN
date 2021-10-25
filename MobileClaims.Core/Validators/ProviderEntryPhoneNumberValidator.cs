using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class ProviderEntryPhoneNumberValidator : AbstractValidator<ClaimServiceProviderEntryViewModel>
    {
        private const string PhoneNumberRegEx = @"^(?!911|0)\d{10}$";

        public ProviderEntryPhoneNumberValidator()
        {
            RuleFor(spevm => spevm.Phone).NotNull();
            RuleFor(spevm => spevm.Phone).NotEmpty();
            RuleFor(spevm => spevm.Phone.PreparePhoneNumber()).Matches(PhoneNumberRegEx);
        }
    }
}