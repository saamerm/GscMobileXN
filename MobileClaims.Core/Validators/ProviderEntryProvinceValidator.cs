using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class ProviderEntryProvinceValidator : AbstractValidator<ClaimServiceProviderEntryViewModel>
    {
        public ProviderEntryProvinceValidator()
        {
            RuleFor(spevm => spevm.SelectedProvince).NotNull();
            RuleFor(spevm => spevm.SelectedProvince.ID).NotNull();
            RuleFor(spevm => spevm.SelectedProvince.ID).NotEmpty();
            //must be in Provinces
        }
    }
}