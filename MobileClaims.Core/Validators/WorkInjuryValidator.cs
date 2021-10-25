using System;
using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class WorkInjuryValidator : AbstractValidator<ClaimWorkInjuryViewModel>
    {
        public WorkInjuryValidator()
        {
            //RuleFor(vm => vm.DateOfWorkRelatedInjury).NotEmpty().Must(VerifyValidDate).WithMessage("Empty Date");
            RuleFor(vm => vm.DateOfWorkRelatedInjury).NotEmpty().Must(IsNotFutureDate).WithMessage("Future Date");
            RuleFor(vm => vm.WorkRelatedInjuryCaseNumber).NotEmpty().WithMessage("Empty Case Number");
        }

        private bool VerifyValidDate(DateTime date)
        {
            int dc = DateTime.Compare(date.Date, DateTime.Now.Date);
            if (dc != 0)
            {
                return true;
            }
            return false;
        }
        private bool IsNotFutureDate(DateTime date)
        {
            int dc = DateTime.Compare(date.Date, DateTime.Now.Date);
            if (dc <= 0)
            {
                return true;
            }
            return false;
        }
    }
}