using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class ProviderEntryAddress1Validator : AbstractValidator<ClaimServiceProviderEntryViewModel>
    {
        public ProviderEntryAddress1Validator()
        {
            RuleFor(spevm => spevm.Address1).NotNull();
            RuleFor(spevm => spevm.Address1.Trim()).NotEmpty();
            RuleFor(spevm => spevm.Address1.ConvertToEnglish().Trim())
                .Length(1, ProviderEntryValidationParams.ADDRESS1_MAX_LENGTH);
            RuleFor(spevm => spevm.Address1.ConvertToEnglish().Trim())
                .Matches(ProviderEntryValidationParams.UnsafeCharsRegEx);
        }
    }
}