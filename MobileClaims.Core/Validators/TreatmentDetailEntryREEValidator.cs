using System;
using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class TreatmentDetailEntryREEValidator : AbstractValidator<ClaimTreatmentDetailsEntryREEViewModel>
    {
        public TreatmentDetailEntryREEValidator()
        {
            //RuleFor(vm => vm.DateOfExamination).NotEmpty().Must(VerifyValidDate).WithMessage("Empty Date");
            RuleFor(vm => vm.DateOfExamination).NotEmpty().Must(IsNotFutureDate).WithMessage("Future Date");
            RuleFor(vm => vm.DateOfExamination).Must(IsWithin24Months).WithMessage("Date TooOld");
            RuleFor(vm => vm.TotalAmountOfExamination).NotEmpty().WithMessage("Empty Amount");
            RuleFor(vm => vm.TotalAmountOfExamination).Matches(GSCHelper.MONEY_REGEX).WithMessage("Invalid Amount").When(vm => !string.IsNullOrEmpty(vm.TotalAmountOfExamination));
            RuleFor(vm => vm.TotalDollarValue).LessThanOrEqualTo(9999.99).WithMessage("Amount TooHigh");
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