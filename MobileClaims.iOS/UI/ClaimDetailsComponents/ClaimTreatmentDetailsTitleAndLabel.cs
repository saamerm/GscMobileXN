using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.ComponentModel;

namespace MobileClaims.iOS
{
    [Register("ClaimTreatmentDetailsTitleAndLabel")]
    [DesignTimeVisible(true)]
	public class ClaimTreatmentDetailsTitleAndLabel : UIView
	{
		public delegate void EventHandler(object sender, EventArgs e);
		public event EventHandler ItemSet;
		public event EventHandler ItemCleared;

		private UILabel titleLabel;

		public UILabel detailsLabel;

		private float COMPONENT_WIDTH = 200;
		private float COMPONENT_HEIGHT = 200;

		public string ErrorString = "";
        public string titleTextLabel;


		private float FIELD_PADDING = 15;

		protected UIView disableOverlayView;

		protected bool _isEnabled = true;

		bool listClear = false;
		bool listIsCleared = false;

        [Export("TextLabelTitleField")]
        [Browsable(true)]
        public string TitleTextLabel
        {
            get { return titleTextLabel; }
            set
            {
                titleTextLabel = value;
            }
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            Initialize();
        }

        public ClaimTreatmentDetailsTitleAndLabel(IntPtr handler) : base(handler)
        {
        }

		public ClaimTreatmentDetailsTitleAndLabel ( string titleText) 
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

            detailsLabel = new UILabel();
            detailsLabel.LineBreakMode = UILineBreakMode.WordWrap;
            detailsLabel.TextColor = Colors.DARK_GREY_COLOR;
            detailsLabel.Lines = 0;
            detailsLabel.Text = " ";
            detailsLabel.TextAlignment = UITextAlignment.Left;
            detailsLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
            detailsLabel.BackgroundColor = Colors.Clear;
            AddSubview(detailsLabel);

            disableOverlayView = new UIView();
            disableOverlayView.BackgroundColor = Colors.LightGrayColor;
            disableOverlayView.Alpha = 0.5f;
        }

		public void ClearDate()
		{
			detailsLabel.Alpha = 0;
		}



		float redrawCount = 0;
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			float contentWidth = (float)this.Frame.Width;
			float alertArea = 30;

			float innerPadding = 10;

			float textFieldSectionWidth = contentWidth / 2;

			float textFieldWidth = contentWidth/2;

			titleLabel.Frame = new CGRect (0, FIELD_PADDING, (float)this.Frame.Width/2 - innerPadding - alertArea, (float)titleLabel.Frame.Height);
			titleLabel.SizeToFit ();

			float detailsX = contentWidth/2;
			float detailsY = this.ComponentHeight / 2 - (float)detailsLabel.Frame.Height /2;

			detailsLabel.Frame = new CGRect (detailsX, detailsY, textFieldWidth, Constants.CLAIMS_DETAILS_FIELD_HEIGHT); 

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
			} else {
				disableOverlayView.Alpha = 0;
				this.AddSubview (disableOverlayView);
				UIView.Animate (animationDuration, 0, (UIViewAnimationOptions)UIViewAnimationOptions.CurveEaseInOut,
					() => {
						disableOverlayView.Alpha = 0.5f;},
					() => {

					}
				);
			}
		}
	}
}

