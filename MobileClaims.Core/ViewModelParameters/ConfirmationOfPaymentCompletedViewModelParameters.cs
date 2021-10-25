using System;
using MobileClaims.Core.Models.Upload;

namespace MobileClaims.Core.ViewModelParameters
{
    public class ConfirmationOfPaymentCompletedViewModelParameters
    {
        public IClaimCompletedProperties ClaimCompletedProperties { get; set; }

        public ConfirmationOfPaymentCompletedViewModelParameters(IClaimCompletedProperties claimCompletedProperties)
        {
            ClaimCompletedProperties = claimCompletedProperties;
        }
    }
}
