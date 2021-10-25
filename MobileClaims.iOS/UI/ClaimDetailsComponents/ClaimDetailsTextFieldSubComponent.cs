using System;
using UIKit;
using CoreGraphics;
using MvvmCross.Platforms.Ios.Views;

namespace MobileClaims.iOS
{
	public class ClaimDetailsTextFieldSubComponent : ClaimDetailsSubComponent
	{

		public DefaultTextField textField;

		protected string _titleString;

		private MvxViewController _parentController;

		private UIView disableOverlayView;

		protected bool _isEnabled = true;

		public ClaimDetailsTextFieldSubComponent (MvxViewController parentController, string titleString)
		{
			_parentController = parentController;

			this.BackgroundColor = Colors.BACKGROUND_COLOR;

			titleLabel = new UILabel ();
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			titleLabel.Lines = 0;
			titleLabel.Text = _titleString = titleString;
			titleLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
			titleLabel.TextColor = Colors.DARK_GREY_COLOR;
			this.AddSubview (titleLabel);

			textField = new DefaultTextField ();
			textField.Placeholder = "XXXXXX";
			textField.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
			textField.TextColor = Colors.DARK_GREY_COLOR;
			this.AddSubview (textField);

			errorButton = new UIButton ();
			errorButton.SetImage (UIImage.FromBundle("ErrorFlag"), UIControlState.Normal);
			errorButton.AdjustsImageWhenHighlighted = true;
			this.AddSubview (errorButton);
			errorButton.TouchUpInside += HandleErrorButton;

			disableOverlayView = new UIView ();
			disableOverlayView.BackgroundColor = Colors.LightGrayColor;
			disableOverlayView.Alpha = 0.5f;

			hideError ();
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

				textField.UserInteractionEnabled = errorButton.UserInteractionEnabled = true;
			} else {
				disableOverlayView.Alpha = 0;
				this.AddSubview (disableOverlayView);
				UIView.Animate (animationDuration, 0, (UIViewAnimationOptions)UIViewAnimationOptions.CurveEaseInOut,
					() => {
						disableOverlayView.Alpha = 0.5f;},
					() => {

					}
				);
				textField.UserInteractionEnabled = errorButton.UserInteractionEnabled = false;
			}
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			float contentWidth = (float)(this.Frame.Width);
			float titlePadding = Constants.CLAIMS_DETAILS_ITEM_V_PADDING;
			float textFieldSectionWidth = contentWidth / 2;
			float alertArea = 50;



			titleLabel.Frame = new CGRect (Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING, titlePadding, contentWidth - textFieldSectionWidth - alertArea, titleLabel.Frame.Height); 
			titleLabel.SizeToFit ();

			float textFieldWidth = textFieldSectionWidth - Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING * 2;

			textField.Frame = new CGRect (textFieldSectionWidth + Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING, this.ComponentHeight / 2 - Constants.CLAIMS_DETAILS_FIELD_HEIGHT / 2, textFieldWidth, Constants.CLAIMS_DETAILS_FIELD_HEIGHT);
		
			if (errorButton != null && errorButton.ImageView.Image != null) {
                float buttonImageWidth = (float)errorButton.ImageView.Image.Size.Width;
                float buttonImageHeight = (float)errorButton.ImageView.Image.Size.Height;
                float buttonImageX = (float)(titleLabel.Frame.X + titleLabel.Frame.Width + alertArea / 2 - buttonImageWidth / 2);
                float buttonImageY = (float)(titleLabel.Frame.Y + titleLabel.Frame.Height / 2 - buttonImageHeight / 2);
				errorButton.Frame = new CGRect (buttonImageX, buttonImageY, buttonImageWidth, buttonImageHeight);
			} 

			disableOverlayView.Frame = new CGRect(0,0, contentWidth, ComponentHeight);

		}

		void HandleSwitch (object sender, EventArgs e)
		{
			//SWITCH
		}

		float _componentHeight;
		public override float ComponentHeight
		{
			get {
                float contentWidth = (float)this.Frame.Width;
				float titlePadding = Constants.CLAIMS_DETAILS_ITEM_V_PADDING;

                return (float)(titleLabel.Frame.Height + titlePadding * 2);
			}
			set{
				_componentHeight = value;
			}

		}
	}
}
