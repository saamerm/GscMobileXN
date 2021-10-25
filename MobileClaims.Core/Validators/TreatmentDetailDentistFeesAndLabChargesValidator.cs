using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class TreatmentDetailDentistFeesAndLabChargesValidator : AbstractValidator<ClaimTreatmentDetailsEntryDentalViewModel>
    {
        public TreatmentDetailDentistFeesAndLabChargesValidator()
        {
            When(tdvm => !tdvm.LaboratoryCharge.HasValue && !tdvm.DentistsFee.HasValue, () =>
            {
                RuleFor(x => x.DentistsFee)
                  .NotNull()
                  .WithMessage(Resource.MissingDentistFeeOrLaboratoryChargeValidation);
                RuleFor(x => x.LaboratoryCharge)
                  .NotNull()
                  .WithMessage(Resource.MissingDentistFeeOrLaboratoryChargeValidation);
            })
            .Otherwise(() =>
            {
                RuleFor(tdvm => tdvm.DentistsFee)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty()
                    .When(x => !x.LaboratoryCharge.HasValue || (x.LaboratoryCharge.HasValue && x.DentistsFee.HasValue), ApplyConditionTo.CurrentValidator)
                    .WithMessage(Resource.MissingDentistFeeValidation)

                    .LessThanOrEqualTo(TreatmentDetailsEntryValidationParams.DENTIST_FEE_MAX)
                    .WithMessage(Resource.InvalidDentistFeeValidation);

                RuleFor(tdvm => tdvm.LaboratoryCharge)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty()
                    .When(x => !x.DentistsFee.HasValue || (x.LaboratoryCharge.HasValue && x.DentistsFee.HasValue), ApplyConditionTo.CurrentValidator)
                    .WithMessage(Resource.MissingLaboratoryChargeValidation)

                    .LessThanOrEqualTo(TreatmentDetailsEntryValidationParams.DENTIST_FEE_MAX)
                    .WithMessage(Resource.InvalidLaboratoryChargeValidation);
            });
        }
    }
}