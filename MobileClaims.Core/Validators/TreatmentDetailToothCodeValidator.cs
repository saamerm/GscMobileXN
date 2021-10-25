using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class TreatmentDetailToothCodeValidator : AbstractValidator<ClaimTreatmentDetailsEntryDentalViewModel>
    {
        public TreatmentDetailToothCodeValidator()
        {
            RuleFor(tdvm => tdvm.ToothCode)
                .NotNull()
                .WithMessage(Resource.MissingToothCodeValidation);
            RuleFor(tdvm => tdvm.ToothCode)
                .NotEmpty()
                .WithMessage(Resource.MissingToothCodeValidation);
            RuleFor(tdvm => tdvm.ToothCode.ToString())
                .Matches(TreatmentDetailsEntryValidationParams.TOOTH_CODE_REGEX)
                .WithMessage(Resource.InvalidToothCodeValidation);
            RuleFor(tdvm => tdvm.ToothCode.ToString())
                .Length(TreatmentDetailsEntryValidationParams.TOOTH_CODE_LENGETH, TreatmentDetailsEntryValidationParams.TOOTH_CODE_LENGETH)
                .WithMessage(Resource.InvalidToothCodeValidation);
        }
    }
}
