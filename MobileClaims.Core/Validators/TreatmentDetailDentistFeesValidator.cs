using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class TreatmentDetailDentistFeesValidator : AbstractValidator<ClaimTreatmentDetailsEntryDentalViewModel>
    {
        public TreatmentDetailDentistFeesValidator()
        {

            RuleFor(tdvm => tdvm.DentistsFee)
                .NotNull()
                .WithMessage(Resource.MissingDentistFeeValidation);
            RuleFor(tdvm => tdvm.DentistsFee)
                .NotEmpty()
                .WithMessage(Resource.MissingDentistFeeValidation);
            RuleFor(tdvm => tdvm.DentistsFee)
                .LessThanOrEqualTo(TreatmentDetailsEntryValidationParams.DENTIST_FEE_MAX)
                .WithMessage(Resource.InvalidDentistFeeValidation);
        }
    }
}