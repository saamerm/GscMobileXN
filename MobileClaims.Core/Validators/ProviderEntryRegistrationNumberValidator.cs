using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class ProviderEntryRegistrationNumberValidator : AbstractValidator<ClaimServiceProviderEntryViewModel>
    {
        public ProviderEntryRegistrationNumberValidator()
        {
            RuleFor(spevm => spevm.RegistrationNumber).Length(0, ProviderEntryValidationParams.REGISTRATION_NUMBER_MAX_LENGTH);
            RuleFor(spevm => spevm.RegistrationNumber).Matches(ProviderEntryValidationParams.UnsafeCharsRegExWithEmptyText);
        }
    }
}