using System;
using UIKit;
using CoreGraphics;

namespace MobileClaims.iOS
{
	public class ClaimTreatmentDetailsTitleAndToggle : UIView
	{

		public delegate void EventHandler(object sender, EventArgs e);
		public event EventHandler SwitchToggled;

		private UILabel titleLabel;

		public UISwitch toggleSwitch;

		private float FIELD_PADDING = 15;

		protected UIView disableOverlayView;

		protected bool _isEnabled = true;

		protected UIButton errorButton;

		public string ErrorString = "";

		public ClaimTreatmentDetailsTitleAndToggle (string titleText)
		{
			this.BackgroundColor = Colors.BACKGROUND_COLOR;

			titleLabel = new UILabel ();
			titleLabel.Text = titleText;
			titleLabel.BackgroundColor = Colors.Clear;
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			titleLabel.TextColor = Colors.DARK_GREY_COLOR;
			titleLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
			titleLabel.Lines = 0;
			AddSubview (titleLabel);

			toggleSwitch = new UISwitch ();
			toggleSwitch.OnTintColor = Colors.HIGHLIGHT_COLOR;
			toggleSwitch.ValueChanged += HandleSwitch;
			this.AddSubview (toggleSwitch);

			disableOverlayView = new UIView ();
			disableOverlayView.BackgroundColor = Colors.LightGrayColor;
			disableOverlayView.Alpha = 0.5f;

			errorButton = new UIButton ();
			errorButton.SetImage (UIImage.FromBundle("ErrorFlag"), UIControlState.Normal);
			errorButton.AdjustsImageWhenHighlighted = true;
			this.AddSubview (errorButton);
			errorButton.TouchUpInside += HandleErrorButton;

			hideError ();
		}

		protected void HandleErrorButton (object sender, EventArgs e)
		{
			UIAlertView _error = new UIAlertView ("", ErrorString, null, "ok".tr(), null);

			_error.Show ();
		}

		public void showError (string error_string =  null)
		{
			if (error_string != null)
				ErrorString = error_string;

			if(titleLabel != null)
				titleLabel.TextColor = Colors.ERROR_COLOR;

			errorButton.Alpha = 1;
			errorButton.UserInteractionEnabled = true;
		}

		public void hideError()
		{
			if (titleLabel != null)
				titleLabel.TextColor = Colors.DARK_GREY_COLOR;

			errorButton.Alpha = 0;
			errorButton.UserInteractionEnabled = false;
		}


		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			float contentWidth = (float)this.Frame.Width;
			float alertArea = 30;

			titleLabel.SizeToFit ();
			titleLabel.Frame = new CGRect (0, FIELD_PADDING, (float)this.Frame.Width/2 - alertArea, (float)titleLabel.Frame.Height);
			titleLabel.SizeToFit ();

			if (errorButton != null && errorButton.ImageView.Image != null) {
				float buttonImageWidth = (float)errorButton.ImageView.Image.Size.Width;
				float buttonImageHeight = (float)errorButton.ImageView.Image.Size.Height;
				float buttonImageX = contentWidth/2 - alertArea / 2 - buttonImageWidth / 2;
				float buttonImageY = ComponentHeight / 2 - buttonImageHeight / 2;
				errorButton.Frame = new CGRect (buttonImageX, buttonImageY, buttonImageWidth, buttonImageHeight);
			} 

			CGRect toggleFrame = (CGRect)toggleSwitch.Frame;

			toggleFrame.X = contentWidth - toggleFrame.Width;
			toggleFrame.Y = ComponentHeight /2 - toggleFrame.Height/2;
			toggleSwitch.Frame = toggleFrame;

			disableOverlayView.Frame = new CGRect(0,0, contentWidth, ComponentHeight);

		}

		float _componentHeight;
		public float ComponentHeight
		{
			get {
				return (float)titleLabel.Frame.Height + FIELD_PADDING*2;
			}
			set{
				_componentHeight = value;
			}

		}

		public void setIsEnabled (bool isEnabled, bool animated)
		{
			float animationDuration = animated ? Constants.TOGGLE_ANIMATION_DURATION : 0;

			if (_isEnabled == isEnabled)
				return;

			_isEnabled = isEnabled;

			if (isEnabled) {
				UIView.Animate (Constants.TOGGLE_ANIMATION_DURATION, 0, (UIViewAnimationOptions)UIViewAnimationOptions.CurveEaseInOut,
					() => {
						disableOverlayView.Alpha = 0;},
					() => {
						disableOverlayView.RemoveFromSuperview ();
					}
				);

				errorButton.Enabled = toggleSwitch.UserInteractionEnabled =  true;
			} else {
				disableOverlayView.Alpha = 0;
				this.AddSubview (disableOverlayView);
				UIView.Animate (animationDuration, 0, (UIViewAnimationOptions)UIViewAnimationOptions.CurveEaseInOut,
					() => {
						disableOverlayView.Alpha = 0.5f;},
					() => {

					}
				);
				errorButton.Enabled = toggleSwitch.UserInteractionEnabled = false;
			}
		}

		void HandleSwitch (object sender, EventArgs e)
		{
			//SWITCH
			if (this.SwitchToggled != null)
			{
				this.SwitchToggled(this, EventArgs.Empty);
			}
		}
	}
}

