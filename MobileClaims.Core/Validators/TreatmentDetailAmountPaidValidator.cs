using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class TreatmentDetailAmountPaidValidator : AbstractValidator<ClaimTreatmentDetailsEntryDentalViewModel>
    {
        public TreatmentDetailAmountPaidValidator()
        {
            RuleFor(tdvm => tdvm)
                .Must(x => x.AlternateCarrierAmount <= (x.DentistsFee.HasValue ? x.DentistsFee : 0) +
                                                       (x.LaboratoryCharge.HasValue ? x.LaboratoryCharge :0))
                .When(x=>x.AlternateCarrierAmount != null)
                .WithMessage(Resource.InvalidAmountPaidValidation);
        }
    }
}