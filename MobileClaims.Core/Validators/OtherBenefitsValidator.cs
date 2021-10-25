using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class OtherBenefitsValidator : AbstractValidator<ClaimOtherBenefitsViewModel>
    {
        public OtherBenefitsValidator()
        {
            RuleFor(obvm => obvm.OtherGSCNumber)
                .NotEmpty()
                .WithMessage(PlanMemberIdValidationMessages.EmptyString);
            RuleFor(obvm => obvm.OtherGSCNumber)
                .Matches(PlanMemberValidation.Regex)
                .WithMessage(PlanMemberIdValidationMessages.NonAlphaNumbericGSCPlanMemberId);
            RuleFor(obvm => obvm.OtherGSCNumber).
                Length(PlanMemberIdValidationMessages.PlanMemberIdMinLength, PlanMemberIdValidationMessages.PlanMemberIdMaxLength)
                .WithMessage(PlanMemberIdValidationMessages.InvalidGSCPlanMemberId);
            RuleFor(obvm => obvm.OtherGSCNumber)
                .Matches(ProviderEntryValidationParams.UnsafeCharsRegEx)
                .WithMessage(PlanMemberIdValidationMessages.InvalidGSCPlanMemberId);
        }
    }
}