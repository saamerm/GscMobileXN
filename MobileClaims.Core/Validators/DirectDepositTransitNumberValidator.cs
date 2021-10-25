using FluentValidation;

namespace MobileClaims.Core.Validators
{
    public static class DirectDepositValidationParams
    {
        // Due to android edittext behaviour, these limits are also set in android layouts
        public const int TRANSIT_NUMBER_LENGTH = 5;
        public const int BANK_NUMBER_LENGTH = 3;
        public const int ACCOUNT_NUMBER_MIN_LENGTH = 6;
        public const int ACCOUNT_NUMBER_MAX_LENGTH = 18;

        internal const string TRANSIT_NUMBER_REGEX = @"^[0-9]{5}$";

        internal const string BANK_NUMBER_REGEX = @"^[0-9]{3}$";

        internal const string ACCOUNT_NUMBER_REGEX = @"^[0-9]{6,18}$";
    }

    public class DirectDepositTransitNumberValidator : AbstractValidator<string>
    {
        public DirectDepositTransitNumberValidator()
        {
            
            RuleFor(transitNumber => transitNumber)
                  .NotNull()
                  .WithMessage(Resource.InvalidTransitNumberValidation);
            RuleFor(transitNumber => transitNumber)
                .NotEmpty()
                  .WithMessage(Resource.InvalidTransitNumberValidation);
            RuleFor(transitNumber => transitNumber)
                .Matches(DirectDepositValidationParams.TRANSIT_NUMBER_REGEX)
                .WithMessage(Resource.InvalidTransitNumberValidation)
                .When(transitNumber => !string.IsNullOrWhiteSpace(transitNumber));
            RuleFor(transitNumber => transitNumber)
                .Length(DirectDepositValidationParams.TRANSIT_NUMBER_LENGTH, DirectDepositValidationParams.TRANSIT_NUMBER_LENGTH)
                .WithMessage(Resource.InvalidTransitNumberValidation)
                .When(transitNumber => !string.IsNullOrWhiteSpace(transitNumber));
        }
    }

    public class DirectDepositBankNumberValidator : AbstractValidator<string>
    {
        public DirectDepositBankNumberValidator()
        {
            RuleFor(bankNumber => bankNumber)
                  .NotNull()
                  .WithMessage(Resource.InvalidBankNumberValidation);
            RuleFor(bankNumber => bankNumber)
                .NotEmpty()
                  .WithMessage(Resource.InvalidBankNumberValidation);
            RuleFor(bankNumber => bankNumber)
                .Matches(DirectDepositValidationParams.BANK_NUMBER_REGEX)
                .WithMessage(Resource.InvalidBankNumberValidation)
                .When(bankNumber => !string.IsNullOrWhiteSpace(bankNumber));
            RuleFor(bankNumber => bankNumber)
                .Length(DirectDepositValidationParams.BANK_NUMBER_LENGTH, DirectDepositValidationParams.BANK_NUMBER_LENGTH)
                .WithMessage(Resource.InvalidBankNumberValidation)
                .When(bankNumber => !string.IsNullOrWhiteSpace(bankNumber));
        }
    }

    public class DirectDepositAccountNumberValidator : AbstractValidator<string>
    {
        public DirectDepositAccountNumberValidator()
        {
            // TODO: Update the text here to get the text from resource files
            RuleFor(accountNumber => accountNumber)
                  .NotNull()
                  .WithMessage(Resource.InvalidAccountNumberValidation);
            RuleFor(accountNumber => accountNumber)
                .NotEmpty()
                  .WithMessage(Resource.InvalidAccountNumberValidation);
            RuleFor(accountNumber => accountNumber)
                .Matches(DirectDepositValidationParams.ACCOUNT_NUMBER_REGEX)
                .WithMessage(Resource.InvalidAccountNumberValidation)
                .When(accountNumber => !string.IsNullOrWhiteSpace(accountNumber));
            RuleFor(accountNumber => accountNumber)
                .Length(DirectDepositValidationParams.ACCOUNT_NUMBER_MIN_LENGTH, DirectDepositValidationParams.ACCOUNT_NUMBER_MAX_LENGTH)
                .WithMessage(Resource.InvalidAccountNumberValidation)
                .When(accountNumber => !string.IsNullOrWhiteSpace(accountNumber));
        }
    }
}