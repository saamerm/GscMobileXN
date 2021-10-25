 using System;
using UIKit;
using CoreGraphics;
using Foundation;
 using MvvmCross.Platforms.Ios.Views;

 namespace MobileClaims.iOS
{
	public class ClaimDetailsDatePickerSubComponent : ClaimDetailsSubComponent
	{
		private UIPopoverController popoverController;
		private MvxViewController _parentController;

		public delegate void EventHandler(object sender, EventArgs e);
		public event EventHandler DateSet;
		public event EventHandler DateCleared;

		public UILabel detailsLabel;

		protected UIView listContainerBackground;

		protected string _titleString;

		public ClaimDateModal dateController;

		protected UIButton clearButton;

		bool dateClear = false;
		bool dateIsCleared = false;
		bool errorIsShown = false;

		public ClaimDetailsDatePickerSubComponent (MvxViewController parentController, string titleString, bool _dateClear = false)
		{
			this.BackgroundColor = Colors.BACKGROUND_COLOR;

			_parentController = parentController;

			dateController = new ClaimDateModal ();

			dateController.View = new UIView (new CGRect (0, 0, 5, 5));
			dateController.View.BackgroundColor = Colors.LightGrayColor;

			listContainerBackground = new UIView ();
			listContainerBackground.BackgroundColor = Colors.LightGrayColor;
			listContainerBackground.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
			listContainerBackground.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;
			this.AddSubview (listContainerBackground);

			titleLabel = new UILabel ();
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			titleLabel.Lines = 0;
			titleLabel.BackgroundColor = Colors.Clear;
			titleLabel.Text = _titleString = titleString;
			titleLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
			titleLabel.TextColor = Colors.DARK_GREY_COLOR;
			this.AddSubview (titleLabel);

			detailsLabel = new UILabel ();
			detailsLabel.LineBreakMode = UILineBreakMode.WordWrap;
			detailsLabel.Lines = 0;
			detailsLabel.BackgroundColor = Colors.Clear;
			detailsLabel.TextAlignment = UITextAlignment.Left;
			detailsLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
			detailsLabel.TextColor = Colors.DARK_GREY_COLOR;
			AddSubview (detailsLabel);

            DateTime dateMin = DateTime.Parse(Constants.MIN_DATE);
            NSDate nsDateMin = (NSDate)DateTime.SpecifyKind(dateMin, DateTimeKind.Utc);
			DateTime dateMax = DateTime.Now;
            NSDate nsDateMax = (NSDate)DateTime.SpecifyKind(dateMax, DateTimeKind.Utc);

			dateController.datePicker = new UIDatePicker ();
			dateController.datePicker.Mode = UIDatePickerMode.Date;
			dateController.datePicker.MinimumDate = nsDateMin;
			dateController.datePicker.MaximumDate = nsDateMax;
			dateController.View.AddSubview (dateController.datePicker);

			if (Constants.IsPhone ()) {
				dateController.dismissButton = new GSButton ();
				dateController.dismissButton.SetTitle ("done".tr(), UIControlState.Normal);
				dateController.View.AddSubview (dateController.dismissButton);
				dateController.dismissButton.TouchUpInside += HandleDismissButton;
			}

			errorButton = new UIButton ();
			errorButton.SetImage (UIImage.FromBundle("ErrorFlag"), UIControlState.Normal);
			errorButton.AdjustsImageWhenHighlighted = true;
			this.AddSubview (errorButton);
			errorButton.TouchUpInside += HandleErrorButton;
			hideError ();

			if (_dateClear) {
				ClearDate ();
				dateClear = dateIsCleared = _dateClear;

				clearButton = new UIButton ();
				clearButton.BackgroundColor = Colors.Clear;
				clearButton.SetImage (UIImage.FromBundle(Constants.ICON_PATH + "closeGrey.png"), UIControlState.Normal);
				clearButton.AdjustsImageWhenHighlighted = true;
				this.AddSubview (clearButton);
				clearButton.TouchUpInside += HandleClearButton;
				clearButton.Alpha = 0;
			}
				

		}

		public void ClearDate()
		{
			detailsLabel.Alpha = 0;

			this.SetNeedsLayout ();
		}

		public void ShowDate()
		{
			detailsLabel.Alpha = 1;

			dateIsCleared = false;

			if (clearButton != null) {
				clearButton.Alpha = 1;
			}

			this.SetNeedsLayout ();
		}

		void HandleClearButton (object sender, EventArgs e)
		{
			dateIsCleared = true;

			if (clearButton != null)
				clearButton.Alpha = 0;
				
			hideError ();

			detailsLabel.Alpha = 0;

			if (this.DateCleared != null)
			{
				this.DateCleared(this, EventArgs.Empty);
			}

		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

            float contentWidth = (float)this.Frame.Width;
			float titlePadding = 15;
			float alertArea = 50;
			float textFieldSectionWidth = contentWidth / 2;

			float textFieldWidth = textFieldSectionWidth - Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING * 2;

			float leftAdjustment = Constants.DRUG_LOOKUP_SIDE_PADDING;

            _componentHeight = (float)(titlePadding * 2 + titleLabel.Frame.Height);

			listContainerBackground.Frame = new CGRect (textFieldSectionWidth + Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING, this.ComponentHeight / 2 - Constants.CLAIMS_DETAILS_FIELD_HEIGHT / 2, textFieldWidth, Constants.CLAIMS_DETAILS_FIELD_HEIGHT);

			titleLabel.Frame = new CGRect (Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING, _componentHeight/2 - titleLabel.Frame.Height/2, contentWidth/2 - alertArea, titleLabel.Frame.Height); 
			titleLabel.SizeToFit ();

			float detailsX = (float)listContainerBackground.Frame.X;

			detailsLabel.Frame = new CGRect (detailsX + leftAdjustment, titleLabel.Frame.Y, textFieldWidth - leftAdjustment*2, titleLabel.Frame.Height); 

			if (errorButton != null && errorButton.ImageView.Image != null) {
                float buttonImageWidth = (float)errorButton.ImageView.Image.Size.Width;
                float buttonImageHeight = (float)errorButton.ImageView.Image.Size.Height;
				float buttonOffset = !dateIsCleared  && dateClear ? (buttonImageHeight/2) + 1 : 0;
                float buttonImageX = (float)(titleLabel.Frame.X + titleLabel.Frame.Width + alertArea / 2 - buttonImageWidth / 2);
                float buttonImageY = (float)(titleLabel.Frame.Y + titleLabel.Frame.Height / 2 - buttonImageHeight / 2 - buttonOffset);
				errorButton.Frame = new CGRect (buttonImageX, buttonImageY, buttonImageWidth, buttonImageHeight);
			}

			if (dateClear && clearButton != null && clearButton.ImageView.Image != null) {
				float buttonImageWidth = Constants.IsPhone() ? 16 : 21;
				float buttonImageHeight = Constants.IsPhone() ? 16 : 21;
				float buttonOffset = errorIsShown ? (buttonImageHeight/2) + 1 : 0;
                float buttonImageX = (float)(titleLabel.Frame.X + titleLabel.Frame.Width + alertArea / 2 - buttonImageWidth / 2);
                float buttonImageY = (float)(titleLabel.Frame.Y + titleLabel.Frame.Height / 2 - buttonImageHeight / 2 + buttonOffset);
				clearButton.Frame = new CGRect (buttonImageX, buttonImageY, buttonImageWidth, buttonImageHeight);
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

			errorIsShown = true;

			this.SetNeedsLayout ();
		}

		public void hideError()
		{
			if (titleLabel != null)
				titleLabel.TextColor = Colors.DARK_GREY_COLOR;

			errorButton.Alpha = 0;
			errorButton.UserInteractionEnabled = false;

			errorIsShown = false;

			this.SetNeedsLayout ();
		}

		void HandleDismissButton (object sender, EventArgs e)
		{
			dateController.DismissViewController (true, (Action)null);

		}

		public override void TouchesBegan (Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);

			CGPoint newPoint = (CGPoint)((touches.AnyObject as UITouch).LocationInView(this));

			if (newPoint.X < this.Frame.Width / 2)
				return;

			if (dateIsCleared) {
				detailsLabel.Alpha = 1;
				dateIsCleared = false;

				if (clearButton != null) {
					clearButton.Alpha = 1;
				}

				if (this.DateSet != null)
				{
					this.DateSet(this, EventArgs.Empty);
				}
			}

			if (Constants.IsPhone ()) {

				UIWindow keyWindow = UIApplication.SharedApplication.KeyWindow;

				dateController.ModalInPopover = true;
				dateController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
				keyWindow.RootViewController.PresentViewController((UIViewController)dateController, true, (Action)null);


			} else {
				if (popoverController == null || popoverController.ContentViewController == null)
				{
					popoverController = new UIPopoverController (dateController);
				}

				try
				{
					float adjustmentForParentHeight = 50;

					float popoverX = (float)(this.Frame.X + this.Frame.Width/2 + this.Frame.Width/4);
					float popoverY = (float)(this.Frame.Y + this.Frame.Height/2 +adjustmentForParentHeight);
					popoverController.SetPopoverContentSize( (CGSize)new CGSize(dateController.datePicker.Frame.Width, dateController.datePicker.Frame.Height), false);
					popoverController.PresentFromRect((CGRect)new CGRect( popoverX,popoverY,1,1), (UIView)_parentController.View, (UIPopoverArrowDirection)UIPopoverArrowDirection.Up, true);				} 
				catch (Exception e) 
				{
					Console.WriteLine (e.Message);
				}
			}

		}

		float _componentHeight;
		public override float ComponentHeight
		{
			get {
                float contentWidth = (float)this.Frame.Width;
				float titlePadding = 15;

                return (float)(titleLabel.Frame.Height + titlePadding * 2);
			}
			set{
				_componentHeight = value;
			}

//			get {
//				float contentWidth = this.Frame.Width;
//				float titlePadding = 15;
//
//				RectangleF pickerFrame = datePicker.Frame;
//
//				return pickerFrame.Height;
//			}
//			set{
//				_componentHeight = value;
//			}

		}
	}
}
