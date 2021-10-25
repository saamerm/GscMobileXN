using System.Collections.ObjectModel;
using System.Linq;
using FluentValidation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class TreatmentDetailListValidator : AbstractValidator<ClaimTreatmentDetailsListViewModel>
    {
        public TreatmentDetailListValidator()
        {
            RuleFor(cdvm => cdvm.TreatmentDetails).NotNull().Must(NotEmpty);
        }
        private bool NotEmpty(ObservableCollection<TreatmentDetail> TreatmentDetails)
        {
            return (TreatmentDetails.Count() > 0);
        }
    }
}