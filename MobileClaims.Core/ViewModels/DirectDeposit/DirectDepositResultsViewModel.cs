using System;
using MvvmCross.Commands;
using MobileClaims.Core.ViewModelParameters;

namespace MobileClaims.Core.ViewModels.DirectDeposit

{
    public class DirectDepositResultsViewModel : ViewModelBase
    {

        public string Title { get; set; }

        public string DDResultTitle => Resource.DirectDepositSuccess;
        public string DDResultSubTitle => Resource.DDResultSuccessSubTitle;
        public string DDResultDisclaimer => Resource.DirectDepositFinePrints;
        public string DDResultDisclaimer1 => Resource.DDResultDisclaimer1;
        public string DDResultDisclaimer2 => Resource.DDResultDisclaimer2;
        public string DDResultDisclaimer3 => BrandResource.DDResultDisclaimer3;

        // For reusing Android's "directdepositheaderlayout" and "DirectDepositDisclaimerlayout"
        public string AndroidTitle => Resource.DirectDepositSuccess;
        public string SubTitle => Resource.DDResultSuccessSubTitle;
        public string DiscalimerNote1 => Resource.DDResultDisclaimer1;
        public string DiscalimerNote2 => Resource.DDResultDisclaimer2;
        public string DiscalimerNote3 => BrandResource.DDResultDisclaimer3;

        public DirectDepositResultsViewModel()
        {
            Title = Resource.DirectDepositSuccess;
            
        }
    }
}
