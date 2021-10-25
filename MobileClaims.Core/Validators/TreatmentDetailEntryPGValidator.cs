using System;
using FluentValidation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using MobileClaims.Core.Extensions;

namespace MobileClaims.Core.Validators
{
    public class TreatmentDetailEntryPGValidator : AbstractValidator<ClaimTreatmentDetailsEntryPGViewModel>
    {
        public TreatmentDetailEntryPGValidator()
        {
            RuleFor(vm => vm.DateOfPurchase)
                .NotEmpty()
                .Must(x=>x.IsNotFutureDate())
                .WithMessage("Future Date");

            RuleFor(vm => vm.DateOfPurchase)
                .Must(x=> x.IsWithin24Months())
                .WithMessage("Date TooOld");

            RuleFor(vm => vm.TypeOfEyewear)
                .NotEmpty()
                .WithMessage("Empty Type");

            // Frame and Lenses, Left Only, Right Only, or Pair
            When(vm => ((vm.TypeOfEyewear != null) && (vm.IsFrameLensAndFeeEntryVisible) && ((vm.TypeOfEyewear.ID == 70144) || (vm.TypeOfEyewear.ID == 70146) || (vm.TypeOfEyewear.ID == 70147) || (vm.TypeOfEyewear.ID == 70148))), () =>
            {
                RuleFor(vm => vm.EyeglassLensesAmount).NotEmpty().WithMessage("Empty Lenses Amount");
                RuleFor(vm => vm.EyeglassLensesAmount).Matches(GSCHelper.MONEY_REGEX).WithMessage("Invalid Lenses Amount").When(vm => !string.IsNullOrEmpty(vm.EyeglassLensesAmount));
                RuleFor(vm => vm.EyeglassLensesDollarValue).LessThanOrEqualTo(9999.99).WithMessage("Invalid Lenses Amount").When(vm => !string.IsNullOrEmpty(vm.EyeglassLensesAmount));
            });

            // Frame and Lenses or Frame Only
            When(vm => ((vm.TypeOfEyewear != null) && (vm.IsFrameLensAndFeeEntryVisible) && ((vm.TypeOfEyewear.ID == 70144) || (vm.TypeOfEyewear.ID == 70142))), () =>
            {
                RuleFor(vm => vm.FrameAmount).NotEmpty().WithMessage("Empty Frame Amount");
                RuleFor(vm => vm.FrameAmount).Matches(GSCHelper.MONEY_REGEX).WithMessage("Invalid Frame Amount").When(vm => !string.IsNullOrEmpty(vm.FrameAmount));
                RuleFor(vm => vm.FrameDollarValue).LessThanOrEqualTo(9999.99).WithMessage("Invalid Frame Amount").When(vm => !string.IsNullOrEmpty(vm.FrameAmount));
            });

            // When TotalAmountCharged is visible, it's required
            When((vm => vm.IsTotalAmountChargedVisible), () =>
            {
                RuleFor(vm => vm.TotalAmountCharged).NotEmpty().WithMessage("Empty TotalAmountCharged");
                RuleFor(vm => vm.TotalAmountCharged).Matches(GSCHelper.MONEY_REGEX).WithMessage("Invalid TotalAmountCharged").When(vm => !string.IsNullOrEmpty(vm.TotalAmountCharged));
            });

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

            RuleSet("RightBifocal", () =>
            {
                RuleFor(vm => vm.RightBifocal).NotEmpty().WithMessage("Empty RightBifocal");
                RuleFor(vm => vm.RightBifocal).Must(HaveANonEmptySelection).WithMessage("Empty RightBifocal").When(vm => vm.RightBifocal != null);
            });

            RuleSet("LeftBifocal", () =>
            {
                RuleFor(vm => vm.LeftBifocal).NotEmpty().WithMessage("Empty LeftBifocal");
                RuleFor(vm => vm.LeftBifocal).Must(HaveANonEmptySelection).WithMessage("Empty LeftBifocal").When(vm => vm.LeftBifocal != null);
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