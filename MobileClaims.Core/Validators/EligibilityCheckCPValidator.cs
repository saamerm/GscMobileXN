using System;
using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class EligibilityCheckCPValidator : AbstractValidator<EligibilityCheckCPViewModel>
    {
        public EligibilityCheckCPValidator()
        {
            RuleFor(vm => vm.TotalAmountOfVisit).NotEmpty().WithMessage("Empty Amount");
            RuleFor(vm => vm.TotalAmountOfVisit).Matches(GSCHelper.MONEY_REGEX).WithMessage("Invalid Amount").When(vm => !string.IsNullOrEmpty(vm.TotalAmountOfVisit)); //money only
            RuleFor(vm => vm.TypeOfTreatment).NotEmpty().WithMessage("Empty TypeOfTreatment");
            RuleFor(vm => vm.DateOfTreatment).Must(BeWithin12Months).WithMessage("Date TooOld");
            RuleFor(vm => vm.DateOfTreatment).Must(NotBeFutureDate).WithMessage("Future Date");
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