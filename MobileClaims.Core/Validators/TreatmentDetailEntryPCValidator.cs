using System;
using FluentValidation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using MobileClaims.Core.Extensions;

namespace MobileClaims.Core.Validators
{
    public class TreatmentDetailEntryPCValidator : AbstractValidator<ClaimTreatmentDetailsEntryPCViewModel>
    {
        public TreatmentDetailEntryPCValidator()
        {
            RuleFor(vm => vm.TypeOfEyewear)
                .NotEmpty()
                .WithMessage("Empty Type");

            RuleFor(vm => vm.DateOfPurchase)
                .NotEmpty()
                .Must(x => x.IsNotFutureDate())
                .WithMessage("Future Date");

            RuleFor(vm => vm.DateOfPurchase)
                .Must(x=> x.IsWithin24Months())
                .WithMessage("Date TooOld");

            RuleFor(vm => vm.TotalAmountCharged)
                .NotEmpty()
                .WithMessage("Empty Amount");

            RuleFor(vm => vm.TotalAmountCharged)
                .Matches(GSCHelper.MONEY_REGEX)
                .WithMessage("Invalid Amount")
                .When(vm => !string.IsNullOrEmpty(vm.TotalAmountCharged));

            RuleSet("AlternateCarrier", () =>
            {
                RuleFor(vm => vm.AmountPaidByAlternateCarrier).NotEmpty().WithMessage("Empty AC");
                RuleFor(vm => vm.AmountPaidByAlternateCarrier).Matches(GSCHelper.MONEY_REGEX).WithMessage("Invalid AC").When(vm => !string.IsNullOrEmpty(vm.AmountPaidByAlternateCarrier));
                RuleFor(vm => vm.ACDollarValue).LessThanOrEqualTo(vm => vm.TotalDollarValue).WithMessage("BadValue AC").When(vm => !string.IsNullOrEmpty(vm.AmountPaidByAlternateCarrier));
            });

            RuleSet("AmountPaidByPPorGP", () =>
            {
                RuleFor(vm => vm.AmountPaidByPPorGP)
                    .NotEmpty()
                    .WithMessage("Empty AmountPaidByPPorGP");

                RuleFor(vm => vm.AmountPaidByPPorGP)
                    .Matches(GSCHelper.MONEY_REGEX)
                    .WithMessage("Invalid AmountPaidByPPorGP")
                    .When(vm => !string.IsNullOrEmpty(vm.AmountPaidByPPorGP));

                RuleFor(vm => vm.AmountPaidByPPorGPDollarValue)
                    .LessThanOrEqualTo(GSCHelper.MaxAmountPaidByPPorGP)
                    .WithMessage("BadValue AmountPaidByPPorGP")
                    .When(vm => !string.IsNullOrEmpty(vm.AmountPaidByPPorGP));
            });

            RuleSet("Pairs", () =>
            {
                RuleFor(vm => vm.RightSphere).NotEmpty().WithMessage("Empty RightSphere");
                RuleFor(vm => vm.RightSphere).Must(HaveANonEmptySelection).WithMessage("Empty RightSphere").When(vm => vm.RightSphere != null);
                RuleFor(vm => vm.LeftSphere).NotEmpty().WithMessage("Empty LeftSphere");
                RuleFor(vm => vm.LeftSphere).Must(HaveANonEmptySelection).WithMessage("Empty LeftSphere").When(vm => vm.LeftSphere != null);
            });

            RuleSet("Left", () =>
            {
                RuleFor(vm => vm.LeftSphere).NotEmpty().WithMessage("Empty LeftSphere");
                RuleFor(vm => vm.LeftSphere).Must(HaveANonEmptySelection).WithMessage("Empty LeftSphere").When(vm => vm.LeftSphere != null);
            });

            RuleSet("Right", () =>
            {
                RuleFor(vm => vm.RightSphere).NotEmpty().WithMessage("Empty RightSphere");
                RuleFor(vm => vm.RightSphere).Must(HaveANonEmptySelection).WithMessage("Empty RightSphere").When(vm => vm.RightSphere != null);
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

        private bool HaveANonEmptySelection(ClaimOption option)
        {
            if (option != null && !string.IsNullOrEmpty(option.ID))
            {
                return true;
            }

            return false;
        }
    }
}