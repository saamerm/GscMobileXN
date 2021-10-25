using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class DINValidator : AbstractValidator<DrugLookupByDINViewModel>
    {
        public DINValidator()
        {
            RuleFor(dvm => dvm.DIN).NotEmpty().WithMessage("Empty");
            RuleFor(dvm => dvm.DIN).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(BeNumeric).WithMessage("Not Numbers");
        }
        private bool BeNumeric(string input)
        {
            bool isCorrectFormat = false;
            int result;
            if (input.Length <= 8)
            {
                isCorrectFormat = int.TryParse(input, out result);
            }
            return isCorrectFormat;
        }
    }
}