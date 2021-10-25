using System;
using CoreGraphics;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS
{
    [Foundation.Register("ClaimServiceProviderProvideInformationView")]
    public class ClaimServiceProviderProvideInformationView : GSCBaseViewController
	{
		protected UIScrollView scrollContainer;
		protected UILabel submissionTypeLabel,submissionDesLabel;
		protected GSButton continueButton;

		private float BUTTON_WIDTH = Constants.IsPhone() ? 270 : 400;
		private float BUTTON_HEIGHT = Constants.IsPhone() ? 40 : 60;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad ();

			ClaimServiceProviderProvideInformationViewModel _model = (ClaimServiceProviderProvideInformationViewModel)ViewModel;

			View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
			base.NavigationController.NavigationBarHidden = false;
			base.NavigationItem.HidesBackButton = true;
			if(Constants.IS_OS_7_OR_LATER())
				this.AutomaticallyAdjustsScrollViewInsets = false;

			scrollContainer = ((GSCBaseView)View).baseScrollContainer;
			((GSCBaseView)View).baseContainer.AddSubview (scrollContainer);
			((GSCBaseView)View).ViewTapped += HandleViewTapped;

//			submissionTypeLabel = new UILabel();
//			submissionTypeLabel.Text = "BLANK";
//			submissionTypeLabel.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, Constants.LG_HEADING_FONT_SIZE);
//			submissionTypeLabel.TextColor = Colors.VERY_DARK_GREY_COLOR;
//			submissionTypeLabel.Lines = 15;
//			submissionTypeLabel.TextAlignment = UITextAlignment.Left;
//			submissionTypeLabel.BackgroundColor = Colors.Clear;
//			scrollContainer.AddSubview (submissionTypeLabel);

			submissionDesLabel = new UILabel();
			submissionDesLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
			submissionDesLabel.Text = "BLANK";
			submissionDesLabel.TextColor = Colors.DARK_GREY_COLOR;
			submissionDesLabel.Lines = 0;
			submissionDesLabel.LineBreakMode = UILineBreakMode.WordWrap;
			submissionDesLabel.TextAlignment = UITextAlignment.Left;
			submissionDesLabel.BackgroundColor = Colors.Clear;
			scrollContainer.AddSubview (submissionDesLabel);

			continueButton = new GSButton ();
			scrollContainer.AddSubview (continueButton);

			var set = this.CreateBindingSet<ClaimServiceProviderProvideInformationView, ClaimServiceProviderProvideInformationViewModel> ();
			set.Bind (continueButton).To (vm => vm.EnterServiceProviderCommand);
			set.Apply ();

			SetLabels ();
			_model.PropertyChanged += (sender, args) =>
			{
				switch (args.PropertyName)
				{
				case "ProviderType":
					SetFrames();
					SetLabels();
					break;
				default:
					break;
				}
			};

		}

		void HandleViewTapped (object sender, EventArgs e)
		{
			dismissKeyboard ();
		}

		void dismissKeyboard()
		{
			this.View.EndEditing (true);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			((GSCBaseView)View).subscribeToBusyIndicator ();
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			dismissKeyboard ();
			((GSCBaseView)View).unsubscribeFromBusyIndicator ();
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
			float startY = Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;
			startY += +Constants.DRUG_LOOKUP_LABEL_HEIGHT;
            float viewWidth = (float)View.Superview.Frame.Width;
            float viewHeight = (float)View.Superview.Frame.Height;

			float innerPadding = 30;

			//submissionTypeLabel.Frame = new RectangleF (Constants.DRUG_LOOKUP_SIDE_PADDING, startY, viewWidth-Constants.DRUG_LOOKUP_SIDE_PADDING*2, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
			submissionDesLabel.SizeToFit ();
            submissionDesLabel.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, startY, viewWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, (float)submissionDesLabel.Frame.Height);
			submissionDesLabel.SizeToFit ();
            continueButton.Frame = new CGRect(viewWidth / 2 - viewWidth * 0.7f / 2, (float)submissionDesLabel.Frame.Y + (float)submissionDesLabel.Frame.Height + 40, viewWidth * 0.7f, BUTTON_HEIGHT);
            scrollContainer.Frame = (CGRect)View.Frame;
            scrollContainer.ContentSize = new CGSize((float)View.Frame.Width, continueButton.Frame.Height + continueButton.Frame.Y + Helpers.BottomNavHeight() + 20);
		}

		private void SetLabels()
		{
			ClaimServiceProviderProvideInformationViewModel _model = (ClaimServiceProviderProvideInformationViewModel)ViewModel;			
			submissionDesLabel.Text = "stillCantFindYourProvider".tr () + "\r\n\r\n";
			submissionDesLabel.Text += "ifYouCannotFindYourProvider".FormatWithBrandKeywords(LocalizableBrand.GSC); 			
			continueButton.SetTitle ("provideInformationAboutMyProvider".tr(), UIControlState.Normal);
		}

		private bool _busy = true;
		public bool Busy {
			get {
				return _busy;
			}
			set {
				_busy = value;
				if (!_busy) {
					InvokeOnMainThread (() => {
						((GSCBaseView)View).stopLoading ();
					});
				} else {
					InvokeOnMainThread (() => {
						((GSCBaseView)View).startLoading ();
					});
				}

			}
		}
	}
}

