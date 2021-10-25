using FluentValidation;
using MobileClaims.Core.Entities.HCSA;
using System;
using System.Text.RegularExpressions;

namespace MobileClaims.Core.Validators
{
    public class ClaimDetailValidator : AbstractValidator<ClaimDetail>
    {
        public ClaimDetailValidator()
        {
            RuleFor(cd => cd.ClaimAmount).GreaterThan(0d).WithMessage(Resource.EmptyExpenseAmount);
            RuleFor(cd => cd.ExpenseDate).NotEmpty().WithMessage(Resource.EmptyExpenseDate);
            RuleFor(cd => cd.ClaimAmountString).Must(BeANumber).WithMessage(Resource.ExpenseAmountIsInvalid);
            RuleFor(cd => cd.OtherPaidAmountString).Must(BeANumber).WithMessage(Resource.OtherAmountIsInvalid);
            RuleFor(cd => cd.ExpenseDate).Must(NotBeFutureDate).WithMessage(Resource.ExpenseIsFutureDated);
            RuleFor(cd => cd.ExpenseDate).Must(BeWithinLast48Months).WithMessage(Resource.DateOfExpenseTooOld);
            RuleFor(cd => cd.ClaimAmount).Must(BeValidCurrencyValue).When(cd => cd.ClaimAmount > 0).WithMessage(Resource.ExpenseAmountIsInvalid);
            RuleFor(cd => cd.ClaimAmount).LessThanOrEqualTo(99000d).WithMessage(Resource.ExpenseAmountIsInvalid);
            RuleFor(cd => cd.OtherPaidAmount).Must(BeValidCurrencyValue).When(cd => cd.OtherPaidAmount > 0).WithMessage(Resource.OtherAmountIsInvalid);
            RuleFor(cd => cd.OtherPaidAmount).LessThanOrEqualTo(cd => cd.ClaimAmount).When(cd => cd.ClaimAmount > 0).WithMessage(Resource.TotalAmountMustBeGreaterErrorMessage);
        }
        public bool BeANumber(string number)
        {
            if (string.IsNullOrEmpty(number)) return true;
            double result;
            return double.TryParse(number, out result);
        }
        public bool NotBeFutureDate(DateTime? date)
        {
            date ??= DateTime.MinValue;
            return date <= DateTime.Now;
        }
        public bool BeValidCurrencyValue(double value)
        {
            var validate = value.ToString();
            return Regex.Match(validate, GSCHelper.MONEY_REGEX).Success;
        }
        public bool BeWithinLast48Months(DateTime? date)
        {
            date ??= DateTime.MinValue;
            return date >= DateTime.Now.AddMonths(-48);
        }
    }
}
