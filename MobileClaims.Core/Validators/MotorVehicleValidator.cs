using System;
using FluentValidation;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Validators
{
    public class MotorVehicleValidator : AbstractValidator<ClaimMotorVehicleViewModel>
    {
        public MotorVehicleValidator()
        {
            //RuleFor(obvm => obvm.DateOfMotorVehicleAccident).NotEmpty().Must(VerifyValidDate).WithMessage("Empty Date");
            RuleFor(obvm => obvm.DateOfMotorVehicleAccident).NotEmpty().Must(IsNotFutureDate).WithMessage("Future Date");
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