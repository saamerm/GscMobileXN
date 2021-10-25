using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class ProviderEntryAddress2Validator : AbstractValidator<ClaimServiceProviderEntryViewModel>
    {
        public ProviderEntryAddress2Validator()
        {
            RuleFor(spevm => spevm.Address2).Length(0, ProviderEntryValidationParams.ADDRESS2_MAX_LENGTH);
            RuleFor(spevm => spevm.Address2.ConvertToEnglish().Trim())
                .Matches(ProviderEntryValidationParams.UnsafeCharsRegExWithEmptyText);
        }
    }
}