using System;
using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class EligibilityCheckEyeValidator : AbstractValidator<EligibilityCheckEyeViewModel>
    {
        public EligibilityCheckEyeValidator()
        {
            RuleFor(vm => vm.TotalCharge).NotEmpty().WithMessage("Empty Amount");
            RuleFor(vm => vm.TotalCharge).Matches(GSCHelper.MONEY_REGEX).WithMessage("Invalid Amount").When(vm => !string.IsNullOrEmpty(vm.TotalCharge)); //money only
            RuleFor(vm => vm.TypeOfTreatment).NotEmpty().WithMessage("Empty TypeOfTreatment");
            RuleFor(vm => vm.DateOfPurchaseOrService).Must(BeWithin12Months).WithMessage("Date TooOld");
            RuleFor(vm => vm.DateOfPurchaseOrService).Must(NotBeFutureDate).WithMessage("Future Date");
        }

        private bool BeWithin12Months(DateTime date)
        {
            DateTime oneYearAgo = DateTime.Today.AddMonths(-12);
            if (date >= oneYearAgo)
            {
                return true;
            }
            return false;
        }

        private bool NotBeFutureDate(DateTime date)
        {
            int dc = DateTime.Compare(date.Date, DateTime.Now.Date);
            if (dc <= 0)
            {
                return true;
            }
            return false;
        }
    }
}