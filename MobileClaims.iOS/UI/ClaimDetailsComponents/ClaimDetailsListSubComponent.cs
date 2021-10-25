using System;
using UIKit;
using CoreGraphics;
using MvvmCross.Platforms.Ios.Views;

namespace MobileClaims.iOS
{
	public class ClaimDetailsListSubComponent : ClaimDetailsSubComponent
	{
		public UIPopoverController popoverController;
		private MvxViewController _parentController;

		public delegate void EventHandler(object sender, EventArgs e);
		public event EventHandler ItemSet;
		public event EventHandler ItemCleared;

		protected string _titleString;

		public UILabel detailsLabel;

		protected UIView listContainerBackground;

		private float COMPONENT_WIDTH = 200;
		private float COMPONENT_HEIGHT = 200;

		public ClaimListModal listController;//just a tableview with done button

		protected UIButton clearButton;

		bool listClear = false;
		bool listIsCleared = false;

		public ClaimDetailsListSubComponent (MvxViewController parentController, string titleString, bool _listClear = false)
		{
			this.BackgroundColor = Colors.BACKGROUND_COLOR;

			_parentController = parentController;

			listController = new ClaimListModal ();

			listController.View = new UIView (new CGRect (0, 0, 5, 5));
			listController.View.BackgroundColor = Colors.LightGrayColor;

			listContainerBackground = new UIView ();
			listContainerBackground.BackgroundColor = Colors.LightGrayColor;
			listContainerBackground.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
			listContainerBackground.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;
			this.AddSubview (listContainerBackground);

			titleLabel = new UILabel ();
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			titleLabel.Lines = 0;
			titleLabel.Text = _titleString = titleString;
			titleLabel.BackgroundColor = Colors.Clear;
			titleLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
			titleLabel.TextColor = Colors.DARK_GREY_COLOR;
			this.AddSubview (titleLabel);

			detailsLabel = new UILabel ();
			detailsLabel.LineBreakMode = UILineBreakMode.WordWrap;
			detailsLabel.Lines = 3;
			detailsLabel.BackgroundColor = Colors.Clear;
			detailsLabel.TextAlignment = UITextAlignment.Left;
			detailsLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
			detailsLabel.TextColor = Colors.DARK_GREY_COLOR;
			AddSubview (detailsLabel);

			listController.tableView = new UITableView(new CGRect(0,0,0,0), UITableViewStyle.Plain);
			listController.tableView.RowHeight = Constants.SINGLE_SELECTION_CELL_HEIGHT;
			listController.tableView.TableHeaderView = new UIView ();
			listController.tableView.SeparatorColor = Colors.Clear;
			listController.tableView.ShowsVerticalScrollIndicator = true;

			listController.View.AddSubview (listController.tableView);

			if (Constants.IsPhone ()) {
				listController.dismissButton = new GSButton ();
				listController.dismissButton.SetTitle ("done".tr (), UIControlState.Normal);
				listController.View.AddSubview (listController.dismissButton);
				listController.dismissButton.TouchUpInside += HandleDismissButton;
			} else {
				popoverController = new UIPopoverController (listController);
			}


			errorButton = new UIButton ();
			errorButton.SetImage (UIImage.FromBundle("ErrorFlag"), UIControlState.Normal);
			errorButton.AdjustsImageWhenHighlighted = true;
			this.AddSubview (errorButton);
			errorButton.TouchUpInside += HandleErrorButton;

			hideError ();

			if (_listClear) {
				ClearDetails ();
				listClear = listIsCleared = _listClear;

				clearButton = new UIButton ();
				clearButton.BackgroundColor = Colors.Clear;
				clearButton.SetImage (UIImage.FromBundle(Constants.ICON_PATH + "closeGrey.png"), UIControlState.Normal);
				clearButton.AdjustsImageWhenHighlighted = true;
				this.AddSubview (clearButton);
				clearButton.TouchUpInside += HandleClearButton;
				clearButton.Alpha = 0;
			}
		}

		public void ClearDetails()
		{
			detailsLabel.Alpha = 0;
		}

		public void ShowDetails()
		{
			detailsLabel.Alpha = 1;

			listIsCleared = false;

			if (clearButton != null) {
				clearButton.Alpha = 1;
			}
		}

		void HandleClearButton (object sender, EventArgs e)
		{
			listIsCleared = true;

			if (clearButton != null)
				clearButton.Alpha = 0;

			detailsLabel.Alpha = 0;

			if (this.ItemCleared != null)
			{
				this.ItemCleared(this, EventArgs.Empty);
			}

		}

		void HandleDismissButton (object sender, EventArgs e)
		{
			listController.DismissViewController (true, (Action)null);
		}

		public override void TouchesBegan (Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);

			CGPoint newPoint = (CGPoint)((touches.AnyObject as UITouch).LocationInView(this));

			if (newPoint.X < this.Frame.Width / 2)
				return;


			if (listIsCleared) {
				detailsLabel.Alpha = 1;
				listIsCleared = false;

				hideError ();

				if (clearButton != null) {
					clearButton.Alpha = 1;
				}

				if (this.ItemSet != null)
				{
					this.ItemSet(this, EventArgs.Empty);
				}
			}

			if (Constants.IsPhone ()) {

				UIWindow keyWindow = UIApplication.SharedApplication.KeyWindow; //on clicking popup button

				listController.ModalInPopover = true;
				listController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
				keyWindow.RootViewController.PresentViewController((UIViewController)listController, true, (Action)null);


			} else {

				try
				{
					float adjustmentForParentHeight = 50;

					float popoverX = (float)(this.Frame.X + this.Frame.Width/2 + this.Frame.Width/4);
					float popoverY = (float)(this.Frame.Y + this.Frame.Height/2 + adjustmentForParentHeight);
					popoverController.SetPopoverContentSize( (CGSize)new CGSize(listController.tableView.Frame.Width, COMPONENT_HEIGHT), false);
                    popoverController.PresentFromRect((CGRect)new CGRect(popoverX, popoverY, 1, 1), (UIView)_parentController.View, (UIPopoverArrowDirection)UIPopoverArrowDirection.Up, true);
                } 
				catch (Exception e) 
				{
					Console.WriteLine (e.Message);
				}
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
			float textFieldSectionWidth = contentWidth / 2;

			float textFieldWidth = textFieldSectionWidth - Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING * 2;

			float leftAdjustment = Constants.DRUG_LOOKUP_SIDE_PADDING;

			listContainerBackground.Frame = new CGRect (textFieldSectionWidth + Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING, this.ComponentHeight / 2 - Constants.CLAIMS_DETAILS_FIELD_HEIGHT / 2, textFieldWidth, Constants.CLAIMS_DETAILS_FIELD_HEIGHT);

			titleLabel.Frame = new CGRect (Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING,titlePadding, contentWidth/2 - alertArea, titleLabel.Frame.Height);

            _componentHeight = (float)(titlePadding * 2 + titleLabel.Frame.Height);


			if (!Constants.IsPhone()) {
				listController.tableView.Frame = new CGRect(0,0, COMPONENT_WIDTH, COMPONENT_HEIGHT);
			}


			titleLabel.Frame = new CGRect (Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING, _componentHeight/2 - titleLabel.Frame.Height/2, contentWidth/2 - alertArea, titleLabel.Frame.Height); 
			titleLabel.SizeToFit ();

			float detailsX = (float)listContainerBackground.Frame.X;

			detailsLabel.Frame = new CGRect (detailsX + leftAdjustment, titleLabel.Frame.Y, textFieldWidth - leftAdjustment*2, titleLabel.Frame.Height); 

			if (errorButton != null && errorButton.ImageView.Image != null) {
                float buttonImageWidth = (float)(errorButton.ImageView.Image.Size.Width);
                float buttonImageHeight = (float)(errorButton.ImageView.Image.Size.Height);
                float buttonImageX = (float)(titleLabel.Frame.X + titleLabel.Frame.Width + alertArea / 2 - buttonImageWidth / 2);
                float buttonImageY = (float)(titleLabel.Frame.Y + titleLabel.Frame.Height / 2 - buttonImageHeight / 2);
				errorButton.Frame = new CGRect (buttonImageX, buttonImageY, buttonImageWidth, buttonImageHeight);
			} 

			if (listClear && clearButton != null && clearButton.ImageView.Image != null) {
                float buttonImageWidth = (float)(errorButton.ImageView.Image.Size.Width);
                float buttonImageHeight = (float)(errorButton.ImageView.Image.Size.Height);
                float buttonImageX = (float)(titleLabel.Frame.X + titleLabel.Frame.Width + alertArea / 2 - buttonImageWidth / 2);
                float buttonImageY = (float)(titleLabel.Frame.Y + titleLabel.Frame.Height / 2 - buttonImageHeight / 2);
				clearButton.Frame = new CGRect (buttonImageX, buttonImageY, buttonImageWidth, buttonImageHeight);
			}

			//pickerFrame.X = contentWidth / 2 - Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING;
			//datePicker.Frame = pickerFrame;
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
				float titlePadding = 15;

                return (float)(titleLabel.Frame.Height + titlePadding * 2);
			}
			set{
				_componentHeight = value;
			}

		}
	}
}
