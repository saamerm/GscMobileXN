using Newtonsoft.Json;
using System;

namespace MobileClaims.Core.Entities.ClaimsHistory
{
    public class ClaimPayment
    {
        [JsonProperty("planMemberDisplayId")]
        public string PlanMemberDisplayID { get; set; }

        [JsonProperty("paymentMethodCode")]
        public string PaymentMethodCode { get; set; }

        [JsonIgnore]
        public string PaymentMethod
        {
            get
            {
                if (PaymentMethodCode == GSCHelper.ChequePaymentMethodCode)
                    return Resource.CH;
                else if (PaymentMethodCode == GSCHelper.EFTPaymentMethodCode)
                    return Resource.EF;
                else
                    return string.Empty;
            }
        }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonIgnore]
        public string PaymentAmountString => $"{Amount} {Resource.IncludesTheTotalOfAllClaims}";

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonIgnore]
        public string Currency
        {
            get
            {
                if (CurrencyCode == GSCHelper.CANCurrencyCode)
                    return Resource.CAN;
                else if (CurrencyCode == GSCHelper.USCurrencyCode)
                    return Resource.US;
                else
                    return string.Empty;
            }
        }

        [JsonProperty("depositNumber")]
        public string DepositNumber { get; set; }

        [JsonProperty("chequeNumber")]
        public string ChequeNumber { get; set; }

        [JsonProperty("statementDate")]
        public DateTime StatementDate { get; set; }

        [JsonProperty("depositDate")]
        public DateTime DepositDate { get; set; }

        [JsonProperty("paymentDate")]
        public DateTime PaymentDate { get; set; }

        [JsonProperty("cashedDate")]
        public DateTime? CashedDate { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        public bool IsDepositNumberVisible => PaymentMethodCode == GSCHelper.EFTPaymentMethodCode;

        public bool IsChequeNumberVisible => PaymentMethodCode == GSCHelper.ChequePaymentMethodCode;

        public bool IsStatementDateVisible => PaymentMethodCode == GSCHelper.EFTPaymentMethodCode;

        public bool IsDepositDateVisible => PaymentMethodCode == GSCHelper.EFTPaymentMethodCode;

        public bool IsPaymentDateVisible => PaymentMethodCode == GSCHelper.ChequePaymentMethodCode;

        public bool IsCashedDateVisible => PaymentMethodCode == GSCHelper.ChequePaymentMethodCode;

        public bool IsStatusVisible => PaymentMethodCode == GSCHelper.ChequePaymentMethodCode;

        public string IncludesTotalOfAllClaims => Resource.IncludesTheTotalOfAllClaims;
    }
}