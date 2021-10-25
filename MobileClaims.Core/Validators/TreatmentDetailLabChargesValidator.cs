using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class TreatmentDetailLabChargesValidator : AbstractValidator<ClaimTreatmentDetailsEntryDentalViewModel>
    {
        public TreatmentDetailLabChargesValidator()
        {
            RuleFor(tdvm => tdvm.LaboratoryCharge)
                .NotNull()
                .WithMessage(Resource.MissingLaboratoryChargeValidation);
            RuleFor(tdvm => tdvm.LaboratoryCharge)
                .NotEmpty()
                .WithMessage(Resource.MissingLaboratoryChargeValidation);
            RuleFor(tdvm => tdvm.LaboratoryCharge)
                .LessThanOrEqualTo(TreatmentDetailsEntryValidationParams.DENTIST_FEE_MAX)
                .WithMessage(Resource.InvalidLaboratoryChargeValidation);
        }
    }
}