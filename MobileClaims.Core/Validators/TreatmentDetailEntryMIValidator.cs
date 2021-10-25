using System;
using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class TreatmentDetailEntryMIValidator : AbstractValidator<ClaimTreatmentDetailsEntryMIViewModel>
    {
        public TreatmentDetailEntryMIValidator()
        {
            RuleFor(vm => vm.ItemDescription).NotEmpty().WithMessage("Empty Item");
            //RuleFor(vm => vm.PickupDate).NotEmpty().Must(VerifyValidDate).WithMessage("Empty Date");
            RuleFor(vm => vm.PickupDate).NotEmpty().Must(IsNotFutureDate).WithMessage("Future Date");
            RuleFor(vm => vm.PickupDate).Must(IsWithin24Months).WithMessage("Date TooOld");
            RuleFor(vm => vm.Quantity).NotEmpty().WithMessage("Empty Quantity");
            RuleFor(vm => vm.Quantity).Matches(@"^[1-9]\d*$").WithMessage("Invalid Quantity"); //positive integers only
            RuleFor(vm => vm.TotalAmountCharged).NotEmpty().WithMessage("Empty Amount");
            RuleFor(vm => vm.TotalAmountCharged).Matches(GSCHelper.MONEY_REGEX).WithMessage("Invalid Amount").When(vm => !string.IsNullOrEmpty(vm.TotalAmountCharged));
            RuleFor(vm => vm.TotalDollarValue).LessThanOrEqualTo(9999.99).WithMessage("Amount TooHigh");
            RuleSet("AlternateCarrier", () =>
            {
                RuleFor(vm => vm.AmountPaidByAlternateCarrier).NotEmpty().WithMessage("Empty AC");
                RuleFor(vm => vm.AmountPaidByAlternateCarrier).Matches(GSCHelper.MONEY_REGEX).WithMessage("Invalid AC").When(vm => !string.IsNullOrEmpty(vm.AmountPaidByAlternateCarrier));
                RuleFor(vm => vm.ACDollarValue).LessThanOrEqualTo(vm => vm.TotalDollarValue).WithMessage("BadValue AC").When(vm => !string.IsNullOrEmpty(vm.AmountPaidByAlternateCarrier));
            });
            RuleSet("DateOfReferral", () =>
            {
                RuleFor(obvm => obvm.TypeOfMedicalProfessional).NotEmpty().WithMessage("Empty TypeOfMedicalProfessional");
                RuleFor(obvm => obvm.DateOfReferral).Must(IsWithin12Months).WithMessage("DateOfReferral TooOld");
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
        private bool IsWithin12Months(DateTime date)
        {
            DateTime oneYearAgo = DateTime.Today.AddMonths(-12);
            if (date >= oneYearAgo)
            {
                return true;
            }
            return false;
        }
    }
}