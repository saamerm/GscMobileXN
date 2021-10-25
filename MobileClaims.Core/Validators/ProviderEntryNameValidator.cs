using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class ProviderEntryNameValidator : AbstractValidator<ClaimServiceProviderEntryViewModel>
    {
        private const string ProviderNameRegEx = @"^[A-Za-z\xC0-\xFF][A-Za-z\xC0-\xFF\.\-\'\’\‘() ]*$";

        public ProviderEntryNameValidator()
        {
            RuleFor(spevm => spevm.Name).NotNull();
            RuleFor(spevm => spevm.Name.Trim()).NotEmpty();
            RuleFor(spevm => spevm.Name.ConvertToEnglish().Trim())
                .Length(1, ProviderEntryValidationParams.NAME_MAX_LENGTH);
            RuleFor(spevm => spevm.Name.ConvertToEnglish().Trim()).Matches(ProviderNameRegEx);
            RuleFor(spevm => spevm.Name.ConvertToEnglish().Trim())
                .Matches(ProviderEntryValidationParams.UnsafeCharsRegEx);
        }
    }
}