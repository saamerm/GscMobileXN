using System;
using MobileClaims.Core.ViewModels.DirectDeposit;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.DirectDeposit.DirectDepositResult
{
    public partial class DDResultsViewController : GSCBaseViewController<DirectDepositResultsViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.SetHidesBackButton(true, false);

            SetBindings();
            SetFont();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void SetBindings()
        {
            var set = this.CreateBindingSet<DDResultsViewController, DirectDepositResultsViewModel>();
            set.Bind(DDResultsSubTitleLabel).For(x => x.Text).To(vm => vm.DDResultSubTitle);
            set.Bind(DDResultsDisclaimerLabel).For(x => x.Text).To(vm => vm.DDResultDisclaimer);
            set.Bind(DDResultsDisclaimer1Label).For(x => x.Text).To(vm => vm.DDResultDisclaimer1);
            set.Bind(DDResultsDisclaimer2Label).For(x => x.Text).To(vm => vm.DDResultDisclaimer2);
            set.Bind(DDResultsDisclaimer3Label).For(x => x.Text).To(vm => vm.DDResultDisclaimer3);
            Title = MobileClaims.Core.Resource.DirectDeposit;

            set.Apply();
        }

        private void SetFont()
        {
            DDResultsSubTitleLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14);
            DDResultsDisclaimerLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, 14);
            DDResultsDisclaimer1Label.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14);
            DDResultsDisclaimer2Label.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14);
            DDResultsDisclaimer3Label.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14);
        }
    }
}

