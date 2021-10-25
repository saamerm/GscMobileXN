using System;
using UIKit;
using CoreGraphics;


namespace MobileClaims.iOS
{
	public class ClaimDetailsToggleSubComponent : ClaimDetailsSubComponent
	{
		public UISwitch toggleSwitch;
		public delegate void EventHandler(object sender, EventArgs e);
		public event EventHandler VisibilityToggled;

		protected string _titleString;

		protected UIView disableOverlayView;

		protected bool _isEnabled = true;

		public ClaimDetailsToggleSubComponent (string titleString)
		{
			this.BackgroundColor = Colors.BACKGROUND_COLOR;

			titleLabel = new UILabel ();
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			titleLabel.Lines = 0;
			titleLabel.Text = _titleString = titleString;
			titleLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
			titleLabel.TextColor = Colors.DARK_GREY_COLOR;
			this.AddSubview (titleLabel);

			toggleSwitch = new UISwitch ();
			toggleSwitch.OnTintColor = Colors.HIGHLIGHT_COLOR;
			toggleSwitch.ValueChanged += HandleSwitch;
			this.AddSubview (toggleSwitch);

			errorButton = new UIButton ();
			errorButton.SetImage (UIImage.FromBundle("ErrorFlag"), UIControlState.Normal);
			errorButton.AdjustsImageWhenHighlighted = true;
			this.AddSubview (errorButton);

			disableOverlayView = new UIView ();
			disableOverlayView.BackgroundColor = Colors.LightGrayColor;
			disableOverlayView.Alpha = 0.5f;

			hideError ();
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

				toggleSwitch.UserInteractionEnabled = errorButton.UserInteractionEnabled = true;
			} else {
				disableOverlayView.Alpha = 0;
				this.AddSubview (disableOverlayView);
				UIView.Animate (animationDuration, 0, (UIViewAnimationOptions)UIViewAnimationOptions.CurveEaseInOut,
					() => {
						disableOverlayView.Alpha = 0.5f;},
					() => {

					}
				);
				toggleSwitch.UserInteractionEnabled = errorButton.UserInteractionEnabled = false;
			}
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
			float titlePadding = 15;
			float alertArea = 50;
			float innerPadding = 10;

			CGRect toggleFrame = (CGRect)toggleSwitch.Frame;

            _componentHeight = (float)(titlePadding * 2 + titleLabel.Frame.Height);

			disableOverlayView.Frame = new CGRect(0,0, contentWidth, _componentHeight);

			titleLabel.Frame = new CGRect (Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING, titlePadding + 5, contentWidth - toggleFrame.Width - alertArea - innerPadding - Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING, titleLabel.Frame.Height + 5); 
		
			toggleFrame.X = contentWidth - toggleFrame.Width - Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING;
			toggleFrame.Y = _componentHeight/2 - toggleFrame.Height/2;
			toggleSwitch.Frame = toggleFrame;

			if (errorButton != null && errorButton.ImageView.Image != null) {
				float buttonImageWidth = (float)errorButton.ImageView.Image.Size.Width;
				float buttonImageHeight = (float)errorButton.ImageView.Image.Size.Height;
				float buttonImageX = (float)(titleLabel.Frame.X + titleLabel.Frame.Width + alertArea / 2 - buttonImageWidth / 2);
				float buttonImageY = _componentHeight / 2 - buttonImageHeight / 2;
				errorButton.Frame = new CGRect (buttonImageX, buttonImageY, buttonImageWidth, buttonImageHeight);
			} 
		}

		void HandleSwitch (object sender, EventArgs e)
		{
			//SWITCH
			if (this.VisibilityToggled != null)
			{
				this.VisibilityToggled(this, EventArgs.Empty);
			}
		}

		float _componentHeight;
		public override float ComponentHeight
		{
			get {
                float contentWidth = (float)this.Frame.Width;
				float titlePadding = 15;

				CGRect toggleFrame = (CGRect)toggleSwitch.Frame;

				titleLabel.Frame = new CGRect (Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING, titlePadding + 5, contentWidth - toggleFrame.Width, titleLabel.Frame.Height + 5); 
				titleLabel.SizeToFit ();
				titleLabel.Frame = new CGRect (Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING, titlePadding + 5, contentWidth - toggleFrame.Width, titleLabel.Frame.Height + 5);
                return (float)(titlePadding * 2 + titleLabel.Frame.Height);
			}
			set{
				_componentHeight = value;
			}

		}
	}
}

