using System;
using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class TreatmentDetailEntry2Validator : AbstractValidator<ClaimTreatmentDetailsEntry2ViewModel>
    {
        public TreatmentDetailEntry2Validator()
        {
            RuleFor(vm => vm.TypeOfTreatment).NotEmpty().WithMessage("Empty Type");
            //RuleFor(vm => vm.DateOfTreatment).Must(VerifyValidDate).WithMessage("Empty Date");
            RuleFor(vm => vm.DateOfTreatment).Must(IsNotFutureDate).WithMessage("Future Date");
            RuleFor(vm => vm.DateOfTreatment).Must(IsWithin24Months).WithMessage("Date TooOld");
            RuleFor(vm => vm.TotalAmountOfVisit).NotEmpty().WithMessage("Empty Amount");
            RuleFor(vm => vm.TotalAmountOfVisit).Matches(GSCHelper.MONEY_REGEX).WithMessage("Invalid Amount").When(vm => !string.IsNullOrEmpty(vm.TotalAmountOfVisit)); //money only
            RuleSet("AlternateCarrier", () =>
            {
                RuleFor(vm => vm.AmountPaidByAlternateCarrier).NotEmpty().WithMessage("Empty AC");
                RuleFor(vm => vm.AmountPaidByAlternateCarrier).Matches(GSCHelper.MONEY_REGEX).WithMessage("Invalid AC").When(vm => !string.IsNullOrEmpty(vm.AmountPaidByAlternateCarrier));
                RuleFor(vm => vm.ACDollarValue).LessThanOrEqualTo(vm => vm.TotalDollarValue).WithMessage("BadValue AC").When(vm => !string.IsNullOrEmpty(vm.AmountPaidByAlternateCarrier));
            });

        }
        private bool VerifyValidDate(DateTime date)
        {
            int dc = DateTime.Compare(date.Date, DateTime.Now.Date);
            if (dc != 0)
            {
                return true;
            }
            return false;
        }
        private bool IsNotFutureDate(DateTime date)
        {
            int dc = DateTime.Compare(date.Date, DateTime.Now.Date);
            if (dc <= 0)
            {
                return true;
            }
            return false;
        }
        private bool IsWithin24Months(DateTime date)
        {
            DateTime twoYearsAgo = DateTime.Today.AddMonths(-24);
            if (date >= twoYearsAgo)
            {
                return true;
            }
            return false;
        }
    }
}