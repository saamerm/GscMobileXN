using System;
using MobileClaims.Core.Entities;

namespace MobileClaims.Core.ViewModelParameters
{
    public class DirectDepositViewModelParameters
    {
      
        public DirectDepositInfo DirectDepositInfo { get; set; }
      
        public DirectDepositViewModelParameters(DirectDepositInfo directDepositInfo)
        {
            this.DirectDepositInfo = directDepositInfo;
        }
    }
}
