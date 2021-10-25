using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class ProviderEntryPostalCodeValidator : AbstractValidator<ClaimServiceProviderEntryViewModel>
    {
        private string postalRegEx = @"^[ABCEGHJ-NPRSTVXY]{1}[0-9]{1}[ABCEGHJ-NPRSTV-Z]{1}[ ]?[0-9]{1}[ABCEGHJ-NPRSTV-Z]{1}[0-9]{1}$";

        public ProviderEntryPostalCodeValidator()
        {
            RuleFor(spevm => spevm.PostalCode).NotNull();
            RuleFor(spevm => spevm.PostalCode).NotEmpty();
            RuleFor(spevm => spevm.PostalCode).Length(ProviderEntryValidationParams.POSTAL_CODE_MIN_LENGTH,
                ProviderEntryValidationParams.POSTAL_CODE_MAX_LENGTH);
            RuleFor(spevm => spevm.PostalCode.ToUpper()).Matches(postalRegEx);
            RuleFor(spevm => spevm.PostalCode).Matches(ProviderEntryValidationParams.UnsafeCharsRegEx);
        }
    }
}