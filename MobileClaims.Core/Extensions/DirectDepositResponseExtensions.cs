using System;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services.Responses;

namespace MobileClaims.Core.Extensions
{
    public static class DirectDepositResponseExtensions
    {
        public static DirectDepositInfo ToDirectDepositInfo(this DirectDepositResponse directDepositResponse)
        {
            if (directDepositResponse == null)
            {
                throw new ArgumentNullException(nameof(directDepositResponse));
            }

            return new DirectDepositInfo
            {
                AccountNumber = directDepositResponse.AccountNumber,
                BankNumber = directDepositResponse.BankNumber,
                TransitNumber = directDepositResponse.TransitNumber,
                BankName = directDepositResponse.BankFullName,
                IsDirectDepositAuthorized = directDepositResponse.PaymentMethod.Equals("EF", StringComparison.OrdinalIgnoreCase) ? true : false,
                IsEnrolledForEmailNotification = directDepositResponse.EftEmailIndicator == "Y" ? true : false,
                IsDirectDepositThroughEngine = directDepositResponse.EftThruEeInd == "Y" ? true : false
            };
        }

        public static DirectDepositBankDetails ToDirectDepositBankDetails(this DirectDepositBankDetailsResponse directDepositBankDetailsResponse)
        {
            if (directDepositBankDetailsResponse == null)
            {
                throw new ArgumentNullException(nameof(directDepositBankDetailsResponse));
            }

            return new DirectDepositBankDetails
            {
                BankName = directDepositBankDetailsResponse.BankName,
                IsValidBankInfo = directDepositBankDetailsResponse.ValidBankInfo,
                ValidateStatusCode = directDepositBankDetailsResponse.ValidationStatusCode
            };
        }
    }
}