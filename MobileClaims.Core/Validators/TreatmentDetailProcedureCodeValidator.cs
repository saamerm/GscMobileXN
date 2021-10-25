using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class TreatmentDetailProcedureCodeValidator : AbstractValidator<ClaimTreatmentDetailsEntryDentalViewModel>
    {
        public TreatmentDetailProcedureCodeValidator()
        {
            RuleFor(tdvm => tdvm.ProcedureCode)
                .NotNull()
                .WithMessage(Resource.MissingProcedureCodeValidation);
            RuleFor(tdvm => tdvm.ProcedureCode)
                .NotEmpty()
                .WithMessage(Resource.MissingProcedureCodeValidation);
            RuleFor(tdvm => tdvm.ProcedureCode.ToString())
                .Matches(TreatmentDetailsEntryValidationParams.PROCEDURE_CODE_REGEX)
                .WithMessage(Resource.InvalidProcedureCodeValidation)
                .When(tdvm => !string.IsNullOrWhiteSpace(tdvm.ProcedureCode));
            RuleFor(tdvm => tdvm.ProcedureCode.ToString())
                .Length(TreatmentDetailsEntryValidationParams.PROCEDURE_CODE_LENGETH, TreatmentDetailsEntryValidationParams.PROCEDURE_CODE_LENGETH)
                .WithMessage(Resource.InvalidProcedureCodeValidation)
                .When(tdvm => !string.IsNullOrWhiteSpace(tdvm.ProcedureCode));
        }
    }
}