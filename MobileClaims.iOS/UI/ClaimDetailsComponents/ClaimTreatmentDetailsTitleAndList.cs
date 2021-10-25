using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.ComponentModel;
using MvvmCross.Platforms.Ios.Views;

namespace MobileClaims.iOS
{
    [Register("ClaimTreatmentDetailsTitleAndList")]
    [DesignTimeVisible(true)]
	public class ClaimTreatmentDetailsTitleAndList : UIView
	{
		public UIPopoverController popoverController;
		public MvxViewController _parentController;

		public delegate void EventHandler(object sender, EventArgs e);
		public event EventHandler ItemSet;
		public event EventHandler ItemCleared;

		public ClaimListModal listController;

		private UILabel titleLabel;

		public UILabel detailsLabel;

		private float COMPONENT_WIDTH = 200;
		private float COMPONENT_HEIGHT = 200;

		protected UIButton errorButton;

		public string ErrorString = "";
        public string titleTextLabel;

		private float FIELD_PADDING = 15;

		protected UIView disableOverlayView;

		protected UIView listContainerBackground;

		protected bool _isEnabled = true;

		protected UIButton clearButton;

		bool listClear = false;
		bool listIsCleared = false;

        [Export("TitleTextLabel")]
        [Browsable(true)]
        public string TitleTextLabel
        {
            get { return titleTextLabel; }
            set
            {
                titleTextLabel = value;
            }
        }


        public ClaimTreatmentDetailsTitleAndList(IntPtr handler) : base(handler)
        {
        }

        public ClaimTreatmentDetailsTitleAndList (MvxViewController parentController, string titleText, bool _listClear = false) 
		{
            _parentController = parentController;
            listClear = listIsCleared = _listClear;
            titleTextLabel = titleText;
            Initialize();
		}

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            Initialize();
        }

        public void Initialize()
        {
            this.BackgroundColor = Colors.BACKGROUND_COLOR;

            listController = new ClaimListModal();

            listController.View = new UIView(new CGRect(0, 0, 5, 5));
            listController.View.BackgroundColor = Colors.LightGrayColor;

            listContainerBackground = new UIView();
            listContainerBackground.BackgroundColor = Colors.LightGrayColor;
            listContainerBackground.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
            listContainerBackground.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;
            this.AddSubview(listContainerBackground);

            titleLabel = new UILabel();
            titleLabel.Text = string.Format("{0}", titleTextLabel.tr());
            titleLabel.BackgroundColor = Colors.Clear;
            titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            titleLabel.TextColor = Colors.DARK_GREY_COLOR;
            titleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
            titleLabel.Lines = 0;
            AddSubview(titleLabel);

            detailsLabel = new UILabel();
            detailsLabel.LineBreakMode = UILineBreakMode.TailTruncation;
            detailsLabel.TextColor = Colors.DARK_GREY_COLOR;
            detailsLabel.Lines = 2;
            detailsLabel.Text = "";
            detailsLabel.TextAlignment = UITextAlignment.Left;
            detailsLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
            detailsLabel.BackgroundColor = Colors.Clear;
            AddSubview(detailsLabel);

            listController.tableView = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Plain);
            listController.tableView.RowHeight = Constants.SINGLE_SELECTION_CELL_HEIGHT;
            listController.tableView.TableHeaderView = new UIView();
            listController.tableView.SeparatorColor = Colors.Clear;
            listController.tableView.ShowsVerticalScrollIndicator = true;

            listController.View.AddSubview(listController.tableView);

            if (Constants.IsPhone())
            {
                listController.dismissButton = new GSButton();
                listController.dismissButton.SetTitle("done".tr(), UIControlState.Normal);
                listController.View.AddSubview(listController.dismissButton);
                listController.dismissButton.TouchUpInside += HandleDismissButton;
            }
            else
            {
                popoverController = new UIPopoverController(listController);
            }



            errorButton = new UIButton();
            errorButton.SetImage(UIImage.FromBundle("ErrorFlag"), UIControlState.Normal);
            errorButton.AdjustsImageWhenHighlighted = true;
            this.AddSubview(errorButton);
            errorButton.TouchUpInside += HandleErrorButton;

            disableOverlayView = new UIView();
            disableOverlayView.BackgroundColor = Colors.LightGrayColor;
            disableOverlayView.Alpha = 0.5f;

            hideError();

            if (listClear)
            {
                ClearDate();
                //listClear = listIsCleared = _listClear;

                clearButton = new UIButton();
                clearButton.BackgroundColor = Colors.Clear;
                clearButton.SetImage(UIImage.FromBundle(Constants.ICON_PATH + "closeGrey.png"), UIControlState.Normal);
                clearButton.AdjustsImageWhenHighlighted = true;
                this.AddSubview(clearButton);
                clearButton.TouchUpInside += HandleClearButton;
                clearButton.Alpha = 0;
            }
        }

		public void ClearDate()
		{
			detailsLabel.Alpha = 0;
		}

		void HandleClearButton (object sender, EventArgs e)
		{
			listIsCleared = true;

			if (clearButton != null)
				clearButton.Alpha = 0;

			hideError ();

			detailsLabel.Alpha = 0;

			if (this.ItemCleared != null)
			{
				this.ItemCleared(this, EventArgs.Empty);
			}

		}

		public void ShowDetails()
		{
			detailsLabel.Alpha = 1;

			listIsCleared = false;

			if (clearButton != null) {
				clearButton.Alpha = 1;
			}
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

		void HandleDismissButton (object sender, EventArgs e)
		{
			listController.DismissViewController (true, (Action)null);
			redrawCount = 0;
			SetNeedsLayout ();
		}

		public override void TouchesBegan (Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);

			if (!_isEnabled)
				return;

			CGPoint newPoint = (CGPoint)(touches.AnyObject as UITouch).LocationInView((UIView)this);

			if (newPoint.X < (float)this.Frame.Width / 2)
				return;


			if (listIsCleared) {
				detailsLabel.Alpha = 1;
				listIsCleared = false;

				if (clearButton != null) {
					clearButton.Alpha = 1;
				}

				if (this.ItemSet != null)
				{
					this.ItemSet(this, EventArgs.Empty);
				}
			}

			if (Constants.IsPhone ()) {

				UIWindow keyWindow = UIApplication.SharedApplication.KeyWindow;

				listController.ModalInPopover = true;
				listController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
				keyWindow.RootViewController.PresentViewController((UIViewController)listController, true, (Action)null);


			} else {
				if (popoverController == null || popoverController.ContentViewController == null)
				{
					popoverController = new UIPopoverController (listController);
				}

				try
				{
					float scrollOffset = 0;
					if(_parentController != null && _parentController.View != null && _parentController.View.GetType() == typeof(GSCBaseView))
					{
						scrollOffset = (float)(((GSCBaseView)_parentController.View).baseScrollContainer.ContentOffset.Y);
					}


					float popoverX = (float)this.Frame.X + (float)this.Frame.Width/2 + (float)this.Frame.Width/4;
					float popoverY = (float)this.Frame.Y + (float)this.Frame.Height/2 - scrollOffset; 
					UIPopoverArrowDirection arrowDirection = ((popoverY + COMPONENT_HEIGHT) < (float)_parentController.View.Frame.Height) ? UIPopoverArrowDirection.Up : UIPopoverArrowDirection.Down;
					popoverController.SetPopoverContentSize( (CGSize)new CGSize(listController.tableView.Frame.Width, COMPONENT_HEIGHT), false);
                    popoverController.PresentFromRect((CGRect)new CGRect(popoverX, popoverY, 1, 1), (UIView)_parentController.View, (UIPopoverArrowDirection)arrowDirection, true);
                } 
				catch (Exception e) 
				{
					Console.WriteLine (e.Message);
				}
			}

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

			float leftAdjustment = Constants.DRUG_LOOKUP_SIDE_PADDING;

			titleLabel.Frame = new CGRect (0, FIELD_PADDING, (float)this.Frame.Width/2 - alertArea, (float)titleLabel.Frame.Height);
			titleLabel.SizeToFit ();

			listContainerBackground.Frame = new CGRect (contentWidth/2, this.ComponentHeight / 2 - Constants.CLAIMS_DETAILS_FIELD_HEIGHT / 2, textFieldWidth, Constants.CLAIMS_DETAILS_FIELD_HEIGHT);

			float detailsX = (float)listContainerBackground.Frame.X;

			detailsLabel.Frame = new CGRect (detailsX + leftAdjustment, (float)listContainerBackground.Frame.Y + (float)listContainerBackground.Frame.Height/2 - (float)detailsLabel.Frame.Height /2 , textFieldWidth - leftAdjustment*2, (float)listContainerBackground.Frame.Height); 

			if (!Constants.IsPhone()) {
				listController.tableView.Frame = new CGRect(0,0, COMPONENT_WIDTH, COMPONENT_HEIGHT);
			}
		
			if (errorButton != null && errorButton.ImageView.Image != null) {
				float buttonImageWidth = (float)errorButton.ImageView.Image.Size.Width;
				float buttonImageHeight = (float)errorButton.ImageView.Image.Size.Height;
				float buttonImageX = contentWidth/2 - alertArea / 2 - buttonImageWidth / 2;
				float buttonImageY = ComponentHeight / 2 - buttonImageHeight / 2;
				errorButton.Frame = new CGRect (buttonImageX, buttonImageY, buttonImageWidth, buttonImageHeight);
			} 

			if (listClear && clearButton != null && clearButton.ImageView.Image != null) {
				float buttonImageWidth = (float)clearButton.ImageView.Image.Size.Width;
				float buttonImageHeight = (float)clearButton.ImageView.Image.Size.Height;
				float buttonImageX = contentWidth/2 - alertArea / 2 - buttonImageWidth / 2;
				float buttonImageY = ComponentHeight / 2 - buttonImageHeight / 2;
				clearButton.Frame = new CGRect (buttonImageX, buttonImageY, buttonImageWidth, buttonImageHeight);
			}

			disableOverlayView.Frame = new CGRect(0,0, contentWidth, ComponentHeight);

			int redrawCountValue = Constants.IS_OS_7_OR_LATER() ? 1 : 3;

			if (redrawCount < redrawCountValue) {
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

