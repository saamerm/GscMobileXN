using System;
using System.Collections.Generic;
using MobileClaims.Core.Entities;

namespace MobileClaims.Core.ViewModelParameters
{
    public class DrugLookupByNameSearchResultsViewModelParameters
    {
        public string PlanMemberID { get; set; }

        public DrugLookupByNameSearchResultsViewModelParameters(string planMemberId)
        {
            PlanMemberID = planMemberId;
        }
    }
}