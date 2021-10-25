using System.Collections.Generic;
using System.Linq;

namespace MobileClaims.Core.Entities
{
    public class SpendingAccountTypeRollup
    {
        public List<SpendingAccountPeriodRollup> AccountRollups
        { 
            get;
            set;
        }

        public SpendingAccountType AccountType
        {
            get;
            set;
        }

        public double TotalRemaining
        {
            get
            {
                return AccountRollups.Sum(ar => ar.TotalRemaining);
            }
        }

        public bool IsTotalRemainingVisible
        {
            get
            {
                if (AccountRollups.Count <= 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
