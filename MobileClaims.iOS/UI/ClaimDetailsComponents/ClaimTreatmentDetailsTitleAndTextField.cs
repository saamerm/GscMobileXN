using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.ComponentModel;

namespace MobileClaims.iOS
{
    [Register("ClaimTreatmentDetailsTitleAndTextField")]
    [DesignTimeVisible(true)]
	public class ClaimTreatmentDetailsTitleAndTextField : UIView
	{
		public UILabel titleLabel;

		public DefaultTextField textField;

		protected UIButton errorButton;

        public string titleTextLabel;

		public string ErrorString = "";

		private float FIELD_PADDING = 15;

		protected UIView disableOverlayView;

		protected bool _isEnabled = true;

        [Export("TitleTextLabelTextField")]
        [Browsable(true)]
        public string TitleTextLabel
        {
            get { return titleTextLabel; }
            set
            {
                titleTextLabel = value;
                Initialize();
            }
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            Initialize();
        }

        public ClaimTreatmentDetailsTitleAndTextField(IntPtr handler) : base(handler)
        {
        }

		public ClaimTreatmentDetailsTitleAndTextField (string titleText)
		{
            titleTextLabel = titleText;
            Initialize();
		}

        public void Initialize()
        { 
            this.BackgroundColor = Colors.BACKGROUND_COLOR;

            titleLabel = new UILabel();
            titleLabel.Text = string.Format("{0}", titleTextLabel.tr());
            titleLabel.BackgroundColor = Colors.Clear;
            titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            titleLabel.TextColor = Colors.DARK_GREY_COLOR;
            titleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
            titleLabel.Lines = 0;
            AddSubview(titleLabel);

            textField = new DefaultTextField();
            textField.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
            textField.TextColor = Colors.DARK_GREY_COLOR;
            textField.BackgroundColor = Colors.LightGrayColor;
            this.AddSubview(textField);

            errorButton = new UIButton();
            errorButton.SetImage(UIImage.FromBundle("ErrorFlag"), UIControlState.Normal);
            errorButton.AdjustsImageWhenHighlighted = true;
            this.AddSubview(errorButton);
            errorButton.TouchUpInside += HandleErrorButton;

            disableOverlayView = new UIView();
            disableOverlayView.BackgroundColor = Colors.LightGrayColor;
            disableOverlayView.Alpha = 0.5f;

            setIsEnabled(true, false);
            hideError();
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

		float redrawCount = 0;
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			float contentWidth = (float)this.Frame.Width;
			float alertArea = 30;
			float fieldHeight = Constants.CLAIMS_DETAILS_FIELD_HEIGHT;

			float innerPadding = 10;

			titleLabel.Frame = new CGRect (0, FIELD_PADDING, (float)this.Frame.Width/2 - alertArea, (float)titleLabel.Frame.Height);
			titleLabel.SizeToFit ();			

		
			if (errorButton != null && errorButton.ImageView.Image != null) {
				float buttonImageWidth = (float)errorButton.ImageView.Image.Size.Width;
				float buttonImageHeight = (float)errorButton.ImageView.Image.Size.Height;
				float buttonImageX = contentWidth/2 - alertArea / 2 - buttonImageWidth / 2;
				float buttonImageY = ComponentHeight / 2 - 20;
				errorButton.Frame = new CGRect (buttonImageX-5, titleLabel.Frame.GetMidY() - 20, 30, 40);
			} 

			textField.Frame = new CGRect (contentWidth / 2, titleLabel.Frame.GetMidY() - fieldHeight/2,( contentWidth / 2), fieldHeight);

			disableOverlayView.Frame = new CGRect(0,0, contentWidth, ComponentHeight);

			if (redrawCount < 1) {
				redrawCount++;
				SetNeedsLayout ();
			}
		}

		float _componentHeight;
		public float ComponentHeight
		{
			get {
				return Math.Max( (float)titleLabel.Frame.Height + FIELD_PADDING*2, 50);
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

				errorButton.UserInteractionEnabled = true;
			} else {
				disableOverlayView.Alpha = 0;
				this.AddSubview (disableOverlayView);
				UIView.Animate (animationDuration, 0, (UIViewAnimationOptions)UIViewAnimationOptions.CurveEaseInOut,
					() => {
						disableOverlayView.Alpha = 0.5f;},
					() => {

					}
				);
				errorButton.UserInteractionEnabled = false;
			}
		}

		private bool _enabled = true;
		public bool Enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				_enabled = value;

				if (_enabled) {
					setIsEnabled (true, true);
				} else {
					setIsEnabled (false, true);
				}

			}
		}

	}
}

