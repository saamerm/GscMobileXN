using System;
using MobileClaims.Core.Entities;

namespace MobileClaims.Core.ViewModelParameters
{
    public class DrugLookupResultsViewModelParameters
    {
        public string PlanMemberID { get; set; }
        public int DrugID { get; set; }

        public DrugLookupResultsViewModelParameters(int drugid, string planmemberid)
        {
            PlanMemberID = planmemberid;
            DrugID = drugid;
        }
    }
}