using System.Collections.Generic;
using System.Linq;

namespace MobileClaims.Core.Entities
{
    public class SpendingAccountPeriodRollup
    {
        public List<SpendingAccountDetail> SpendingAccounts
        {
            get;
            set;
        }

        public double TotalRemaining
        {
            get
            {
                return SpendingAccounts.Sum(sad => sad.Remaining);
            }
        }

        public int Year
        {
            get;
            set;
        }

        public string StartDateAsString
        {
            get
            {
                var qry = from SpendingAccountDetail sad in SpendingAccounts
                          select sad.StartDate;
                return qry.Distinct().First().ToString("MMMM dd, yyyy");
            }
        }
        public string EndDateAsString
        {
            get
            {
                var qry = from SpendingAccountDetail sad in SpendingAccounts
                          select sad.EndDate;
                return qry.Distinct().First().ToString("MMMM dd, yyyy");
            }
        }
        public string UseByDateAsString
        {
            get
            {
                var qry = from SpendingAccountDetail sad in SpendingAccounts
                          select sad.UseByDateAsString;
                return qry.Distinct().First();
            }
        }
    }
}
