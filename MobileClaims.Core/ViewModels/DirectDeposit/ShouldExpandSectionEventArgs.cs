using System;
namespace MobileClaims.Core.ViewModels.DirectDeposit
{
    public class ShouldExpandSectionEventArgs : EventArgs
    {
        public bool ShouldExpand { get; set; }

        public int StepNumber { get; set; }
    }
}
