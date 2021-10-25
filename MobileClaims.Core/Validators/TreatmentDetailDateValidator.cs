using System;
using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class TreatmentDetailDateValidator : AbstractValidator<ClaimTreatmentDetailsEntryDentalViewModel>
    {
        public TreatmentDetailDateValidator()
        {
            RuleFor(tdvm => tdvm.DateOfTreatment)
                .NotNull()
                .WithMessage(Resource.InvalidTreatmentDate24MonthValidation);
            RuleFor(tdvm => tdvm.DateOfTreatment)
                .NotEmpty()
                .WithMessage(Resource.InvalidTreatmentDate24MonthValidation);
            RuleFor(tdvm => tdvm)
                .Must(x => x.DateOfTreatment >= DateTime.Today.AddMonths(TreatmentDetailsEntryValidationParams.TREATMENT_DATE_ALLOWED_WITHHIN_MONTHS))
                .WithMessage(Resource.InvalidTreatmentDate24MonthValidation);
        }
    }
}