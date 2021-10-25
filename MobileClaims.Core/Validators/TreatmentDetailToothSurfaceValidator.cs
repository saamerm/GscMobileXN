using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class TreatmentDetailToothSurfaceValidator : AbstractValidator<ClaimTreatmentDetailsEntryDentalViewModel>
    {
        public TreatmentDetailToothSurfaceValidator()
        {
            RuleFor(tdvm => tdvm.ToothSurfaces)
                .NotNull()
                .WithMessage(Resource.MissingToothSurfaceValidation);
            RuleFor(tdvm => tdvm.ToothSurfaces)
                .NotEmpty()
                .WithMessage(Resource.MissingToothSurfaceValidation);
            RuleFor(tdvm => tdvm.ToothSurfaces)
                .Matches(TreatmentDetailsEntryValidationParams.TOOTH_SURFACE_REGEX)
                .WithMessage(Resource.InvalidToothSurfaceValidation);
            RuleFor(tdvm => tdvm)
                .Must(x => x.ToothSurfaces?.Length == x.RequiredToothSurface)
                .When(x => !string.IsNullOrWhiteSpace(x.ToothSurfaces) && x.RequiredToothSurface > 0)
                .WithMessage(Resource.InvalidToothSurfaceValidation);
        }
    }
}