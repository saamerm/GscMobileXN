using System;
using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class MedicalItemValidator : AbstractValidator<ClaimMedicalItemViewModel>
    {
        public MedicalItemValidator()
        {
            RuleFor(obvm => obvm.DateOfReferral).NotEmpty().Must(IsNotFutureDate).WithMessage("Future Date");
            RuleSet("DateOfReferral", () =>
            {
                RuleFor(obvm => obvm.TypeOfMedicalProfessional).NotEmpty().WithMessage("Empty TypeOfMedicalProfessional");
                RuleFor(obvm => obvm.DateOfReferral).Must(IsWithin12Months).WithMessage("DateOfReferral TooOld");
            });
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
        private bool IsWithin12Months(DateTime date)
        {
            DateTime oneYearAgo = DateTime.Today.AddMonths(-12);
            if (date >= oneYearAgo)
            {
                return true;
            }
            return false;
        }
    }
}