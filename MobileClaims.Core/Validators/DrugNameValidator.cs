using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class DrugNameValidator : AbstractValidator<DrugLookupByNameViewModel>
    {
        public DrugNameValidator()
        {
            RuleFor(vm => vm.DrugName).NotEmpty().WithMessage("Empty");
            RuleFor(vm => vm.DrugName).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(MoreThanThreeChars).WithMessage("Too Short");
        }
        private bool MoreThanThreeChars(string input)
        {
            return (input.Length > 2);
        }
    }
}