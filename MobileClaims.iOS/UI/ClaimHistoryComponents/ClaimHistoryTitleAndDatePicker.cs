using System;
using Foundation;
using UIKit;
using CoreGraphics;
using MvvmCross.Platforms.Ios.Views;

namespace MobileClaims.iOS
{
    public class ClaimHistoryTitleAndDatePicker: UIView
    {
        private UIPopoverController popoverController;
        private MvxViewController _parentController;

        public delegate void EventHandler(object sender, EventArgs e);
        public event EventHandler DateSet;
        public event EventHandler DateCleared;

        private UILabel titleLabel;

        public UILabel detailsLabel;

        public ClaimHistoryDateModal dateController;

        protected UIButton errorButton;
        protected UIButton clearButton;

        public string ErrorString = "";

        private float FIELD_PADDING = 15;

        protected UIView disableOverlayView;

        protected UIView listContainerBackground;

        protected bool _isEnabled = true;

        bool dateClear = false;
        bool dateIsCleared = false;
        bool errorIsShown = false;

        public ClaimHistoryTitleAndDatePicker (MvxViewController parentController, string titleText, bool _dateClear = false)
        {
            this.BackgroundColor = Colors.BACKGROUND_COLOR;

            _parentController = parentController;

            dateController = new ClaimHistoryDateModal ();

            dateController.View = new UIView (new CGRect (0, 0, 5, 5));
            dateController.View.BackgroundColor = Colors.LightGrayColor;

            listContainerBackground = new UIView ();
            listContainerBackground.BackgroundColor = Colors.LightGrayColor;
            listContainerBackground.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
            listContainerBackground.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;
            this.AddSubview (listContainerBackground);

            titleLabel = new UILabel ();
            titleLabel.Text = titleText;
            titleLabel.BackgroundColor = Colors.Clear;
            titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            titleLabel.TextColor = Colors.DARK_GREY_COLOR;
            titleLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
            titleLabel.Lines = 3;
            AddSubview (titleLabel);

            detailsLabel = new UILabel ();
            detailsLabel.LineBreakMode = UILineBreakMode.WordWrap;
            detailsLabel.TextColor = Colors.DARK_GREY_COLOR;
            detailsLabel.Lines = 3;
            detailsLabel.TextAlignment = UITextAlignment.Left;
            detailsLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
            detailsLabel.BackgroundColor = Colors.Clear;
            AddSubview (detailsLabel);

            DateTime dateMin = DateTime.Parse (Constants.MIN_DATE);
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
                dateController.dismissButton.SetTitle ("Close", UIControlState.Normal);
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

            disableOverlayView = new UIView ();
            disableOverlayView.BackgroundColor = Colors.LightGrayColor;
            disableOverlayView.Alpha = 0.5f;


        }

        public void ClearDate()
        {
            detailsLabel.Alpha = 0;

            this.SetNeedsLayout ();
        }

        void HandleClearButton (object sender, EventArgs e)
        {
            dateIsCleared = true;

            if (clearButton != null)
                clearButton.Alpha = 0;

            detailsLabel.Alpha = 0;

            if (this.DateCleared != null)
            {
                this.DateCleared(this, EventArgs.Empty);
            }

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

            if (!_isEnabled)
                return;

            CGPoint newPoint = (CGPoint)(touches.AnyObject as UITouch).LocationInView((UIView)this);

            if (newPoint.X < (float)this.Frame.Width / 2 || !_isEnabled)
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
                dateController.ModalPresentationStyle = UIModalPresentationStyle.CurrentContext;
                keyWindow.RootViewController.PresentViewController((UIViewController)dateController, true, (Action)null);


            } else {
                if (popoverController == null || popoverController.ContentViewController == null)
                {
                    popoverController = new UIPopoverController (dateController);
                }

                try
                {
                    float scrollOffset = 0;
                    if(_parentController != null && _parentController.View != null && _parentController.View.GetType() == typeof(GSCBaseView))
                    {
                        scrollOffset = (float)((GSCBaseView)_parentController.View).baseScrollContainer.ContentOffset.Y;
                    }

                    float popoverX = (float)this.Frame.X + (float)this.Frame.Width/2 + (float)this.Frame.Width/4;
                    float popoverY = (float)this.Frame.Y + (float)this.Frame.Height/2 - scrollOffset;
                    popoverController.SetPopoverContentSize( (CGSize)new CGSize(dateController.datePicker.Frame.Width, dateController.datePicker.Frame.Height), false);
                    popoverController.PresentFromRect((CGRect)new CGRect(popoverX, popoverY, 1, 1), (UIView)_parentController.View, (UIPopoverArrowDirection)UIPopoverArrowDirection.Up, true);
                } 
                catch (Exception e) 
                {
                    Console.WriteLine (e.Message);
                }
            }

        }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();

            float contentWidth = (float)this.Frame.Width;
            float alertArea = 30;

            float innerPadding = 10;
            float textFieldSectionWidth = contentWidth / 2;

            float textFieldWidth = contentWidth/2;

            float leftAdjustment = Constants.DRUG_LOOKUP_SIDE_PADDING;

            listContainerBackground.Frame = new CGRect (contentWidth/2, this.ComponentHeight / 2 - Constants.CLAIMS_DETAILS_FIELD_HEIGHT / 2, textFieldWidth, Constants.CLAIMS_DETAILS_FIELD_HEIGHT);

            titleLabel.SizeToFit ();
            titleLabel.Frame = new CGRect (0, FIELD_PADDING, (float)this.Frame.Width/2 - alertArea, (float)titleLabel.Frame.Height);
            titleLabel.SizeToFit ();

            float detailsX = (float)listContainerBackground.Frame.X;

            detailsLabel.Frame = new CGRect (detailsX + leftAdjustment, (float)titleLabel.Frame.Y, textFieldWidth - leftAdjustment*2, (float)titleLabel.Frame.Height); 

            if (errorButton != null && errorButton.ImageView.Image != null) {
                float buttonImageWidth = (float)errorButton.ImageView.Image.Size.Width;
                float buttonImageHeight = (float)errorButton.ImageView.Image.Size.Height;
                float buttonOffset = !dateIsCleared && dateClear ? (buttonImageHeight/2) + 1 : 0;
                float buttonImageX = contentWidth/2 - alertArea / 2 - buttonImageWidth / 2;
                float buttonImageY = ComponentHeight / 2 - buttonImageHeight / 2 - buttonOffset;
                errorButton.Frame = new CGRect (buttonImageX, buttonImageY, buttonImageWidth, buttonImageHeight);
            } 

            if (dateClear && clearButton != null && clearButton.ImageView.Image != null) {
                float buttonImageWidth = (float)errorButton.ImageView.Image.Size.Width;
                float buttonImageHeight = (float)errorButton.ImageView.Image.Size.Height;
                float buttonOffset = errorIsShown ? (buttonImageHeight/2) + 1 : 0;
                float buttonImageX = contentWidth/2 - alertArea / 2 - buttonImageWidth / 2;
                float buttonImageY = ComponentHeight / 2 - buttonImageHeight / 2 + buttonOffset;
                clearButton.Frame = new CGRect (buttonImageX, buttonImageY, buttonImageWidth, buttonImageHeight);
            }

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

                errorButton.UserInteractionEnabled =  true;
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

        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);
            if (newsuper != null)
            {
               // newsuper.mod
               // newsuper.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
            }
        } 
    }
}

