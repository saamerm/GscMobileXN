using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class ProviderEntryCityValidator : AbstractValidator<ClaimServiceProviderEntryViewModel>
    {
        private const string ProviderCityRegEx = @"^[A-Za-z\xC0-\xFF][A-Za-z\xC0-\xFF\.\-\'\’\‘ ]*";
        private string unsafeCharsRegExForCity = @"^[^<>%""”“;&()+]+$";

        public ProviderEntryCityValidator()
        {
            RuleFor(spevm => spevm.City).NotNull();
            RuleFor(spevm => spevm.City.Trim()).NotEmpty();
            RuleFor(spevm => spevm.City.ConvertToEnglish().Trim())
                .Length(1, ProviderEntryValidationParams.CITY_MAX_LENGTH);
            RuleFor(spevm => spevm.City.ConvertToEnglish().Trim()).Matches(ProviderCityRegEx);
            RuleFor(spevm => spevm.City.ConvertToEnglish().Trim())
                .Matches(unsafeCharsRegExForCity);
        }
    }
}