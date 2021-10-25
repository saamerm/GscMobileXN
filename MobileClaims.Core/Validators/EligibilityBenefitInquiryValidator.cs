using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class EligibilityBenefitInquiryValidator : AbstractValidator<EligibilityBenefitInquiryViewModel>
    {
        public EligibilityBenefitInquiryValidator()
        {
            RuleFor(vm => vm.InquiryText).Length(0, 1000);
        }
    }
}