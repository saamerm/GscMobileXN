using System;
using CoreGraphics;
using MobileClaims.Core;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS

{
    // TODO: Not used? Is it safe to remove it?
    [Foundation.Register("ClaimTermsAndConditionsView")]
    public class ClaimTermsAndConditionsView : GSCBaseViewController
	{
		protected UILabel agreementLabel;
		protected GSButton continueButton;
		protected UIScrollView scrollContainer;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad ();
			View = new UIView() { BackgroundColor = Colors.BACKGROUND_COLOR };
			base.NavigationController.NavigationBarHidden = false;
			base.NavigationItem.Title = "termsAndConditions".tr().ToUpper();
			base.NavigationItem.HidesBackButton = false;
			if(Constants.IS_OS_7_OR_LATER())
				this.AutomaticallyAdjustsScrollViewInsets = false;

			scrollContainer = new UIScrollView ();
			View.AddSubview (scrollContainer);

			continueButton = new GSButton ();
			continueButton.SetTitle (Resource.AgreeAndContinue ,UIControlState.Normal);
			if (Constants.IsPhone())
				scrollContainer.AddSubview (continueButton);

			agreementLabel = new UILabel();
			agreementLabel.Font = UIFont.FromName (Constants.NUNITO_REGULAR, (nfloat)Constants.SMALL_FONT_SIZE);
			agreementLabel.Text = "selectTypeOfTreamentBody".FormatWithBrandKeywords(LocalizableBrand.GSC);
			agreementLabel.Lines = 15;
			agreementLabel.SizeToFit ();
			agreementLabel.TextColor = Colors.MED_GREY_COLOR;
			agreementLabel.TextAlignment = UITextAlignment.Left;
			agreementLabel.BackgroundColor = Colors.Clear;
			agreementLabel.ContentMode = UIViewContentMode.ScaleToFill;
			scrollContainer.AddSubview (agreementLabel);

			var set = this.CreateBindingSet<ClaimTermsAndConditionsView, ClaimTermsAndConditionsViewModel> ();
			set.Bind (continueButton).To (vm => vm.AcceptTermsAndConditionsCommand);
			set.Apply ();
		}

		public override void ViewDidLayoutSubviews ()
		{
			SetFrames ();
			base.ViewDidLayoutSubviews ();
		}

		private void SetFrames()
		{
            if (View.Superview == null)
                return;
			float startY = Helpers.StatusBarHeight();
            float viewWidth = (float)View.Superview.Frame.Width;
            float viewHeight = (float)View.Superview.Frame.Height;

			float FIELD_WIDTH = Constants.IsPhone() ? 270 : 400;
			float FIELD_HEIGHT = Constants.IsPhone() ? 40 : 50;

			agreementLabel.Frame = new CGRect (Constants.DRUG_LOOKUP_SIDE_PADDING, startY, viewWidth-Constants.DRUG_LOOKUP_SIDE_PADDING*2, Constants.DRUG_LOOKUP_LABEL_HEIGHT*9);
            continueButton.Frame = new CGRect(viewWidth / 2 - FIELD_WIDTH / 2, (float)agreementLabel.Frame.Height + (float)agreementLabel.Frame.Y + 20, FIELD_WIDTH, FIELD_HEIGHT);
            scrollContainer.Frame = (CGRect)this.View.Frame;
            scrollContainer.ContentSize = new CGSize((float)View.Frame.Width, continueButton.Frame.Height + continueButton.Frame.Y + Helpers.BottomNavHeight() + 20);
		}
	}
}

