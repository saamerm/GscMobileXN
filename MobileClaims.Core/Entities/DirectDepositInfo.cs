using System;
namespace MobileClaims.Core.Entities
{
    public class DirectDepositInfo
    {
        public bool IsDirectDepositAuthorized { get; set; }

        public bool IsEnrolledForEmailNotification { get; set; }

        public bool IsDirectDepositThroughEngine { get; set; }

        public string AccountNumber { get; set; }

        public string TransitNumber { get; set; }

        public string BankNumber { get; set; }

        public string BankName { get; set; }

        public DirectDepositInfo()
        {
        }
    }
}
