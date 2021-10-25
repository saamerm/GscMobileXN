using System;
namespace MobileClaims.Core.Services.Requests
{
    public class SubmitDirectDepositRequest
    {
        public string PaymentMethod { get; set; }

        public string EftEmailIndicator { get; set; }

        public string TransitNumber { get; set; }

        public string BankNumber { get; set; }

        public string AccountNumber { get; set; }
    }
}
