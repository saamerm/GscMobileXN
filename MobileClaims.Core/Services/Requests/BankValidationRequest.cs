using System;
namespace MobileClaims.Core.Services.Requests
{
    public class BankValidationRequest
    {
        public long PlanMemberID { get; set; }

        public string TransitNumber { get; set; }

        public string BankNumber { get; set; }

        public BankValidationRequest(long id, string bNumber, string tNumber)
        {
            PlanMemberID = id;
            TransitNumber = tNumber;
            BankNumber = bNumber;
        }
    }
}
