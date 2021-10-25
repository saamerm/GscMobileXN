using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace MobileClaims.iOS.UI
{
    [Register("LabeledDatePicker")]
    [DesignTimeVisible(true)]
    public class LabeledDatePicker : UIView
    {
        private bool _canClearDate;
        private bool _isDateCleared;
        private bool _isErrorVisible;
        private bool _shouldShowError;
        private bool _shouldUseCurrentDefaultDate;

        private string _text;
        private string _detailsText;

        private UITapGestureRecognizer _gesture;
        private UIStackView _labelStackView;
        private UIStackView _dateStackView;
        private UIPopoverController _popoverController;

        protected UIButton ClearButton { get; private set; }
        protected UIButton ErrorButton { get; set; }
        protected UIView ListContainerBackground { get; set; }
        protected UIView DisableOverlayView { get; set; }

        public event EventHandler DateSelectionRemoved;
        public event EventHandler DateSelected;

        public bool IsEnabled { get; protected set; }
        public bool IsUsedInClaimHistoryDisplayByView { get; set; }

        public UILabel Label { get; set; }
        public UILabel DetailsLabel { get; set; }
        public ClaimDateModal DatePickerController { get; set; }
        public MvxViewController ParentController { get; set; }

        public bool ShouldShowError
        {
            get
            {
                return _shouldShowError;
            }
            set
            {
                _shouldShowError = value;
                ToggleErrorVisibility(_shouldShowError);
            }
        }

        [Export("Text")]
        [Browsable(true)]
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                Label.Text = _text;
            }
        }

        public bool ShouldUseCurrentDefaultDate
        {
            get => _shouldUseCurrentDefaultDate;
            set
            {
                _shouldUseCurrentDefaultDate = value;
                LoadWithDefaultCurrentDate(_shouldUseCurrentDefaultDate);
            }
        }

        [Export("ErrorText")]
        [Browsable(true)]
        public string ErrorText { get; set; }

        [Export("CanClearDate")]
        [Browsable(true)]
        public bool CanClearDate
        {
            get => _canClearDate;
            set
            {
                _canClearDate = value;
                ClearButton.Hidden = !_canClearDate;
            }
        }

        public LabeledDatePicker()
        {
        }

        public LabeledDatePicker(IntPtr handler)
            : base(handler)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            Initialize();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var height = GetEstimatedLabelSize();
            if (height < 50)
            {
                height = 50;
            }

            _labelStackView.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;
            _labelStackView.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor).Active = true;
            _labelStackView.HeightAnchor.ConstraintGreaterThanOrEqualTo(height).Active = true;
            _labelStackView.TrailingAnchor.ConstraintEqualTo(_dateStackView.LeadingAnchor, -3).Active = true;

            _dateStackView.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;
            _dateStackView.HeightAnchor.ConstraintEqualTo(this.HeightAnchor).Active = true;
            _dateStackView.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor, -5).Active = true;

            if (ErrorButton != null && ErrorButton.ImageView.Image != null)
            {
                var buttonImageWidth = ErrorButton.ImageView.Image.Size.Width;
                var buttonImageHeight = ErrorButton.ImageView.Image.Size.Height;

                ErrorButton.HeightAnchor.ConstraintEqualTo(buttonImageHeight).Active = true;
                ErrorButton.WidthAnchor.ConstraintEqualTo(buttonImageWidth).Active = true;
                ErrorButton.CenterYAnchor.ConstraintEqualTo(CenterYAnchor).Active = true;
            }

            if (ClearButton != null && ClearButton.ImageView.Image != null)
            {
                var buttonImageWidth = ClearButton.ImageView.Image.Size.Width;
                var buttonImageHeight = ClearButton.ImageView.Image.Size.Height;

                ClearButton.HeightAnchor.ConstraintEqualTo(buttonImageHeight).Active = true;
                ClearButton.WidthAnchor.ConstraintEqualTo(buttonImageWidth).Active = true;
                ClearButton.CenterYAnchor.ConstraintEqualTo(CenterYAnchor).Active = true;
            }

            ListContainerBackground.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;
            ListContainerBackground.WidthAnchor.ConstraintEqualTo(this.WidthAnchor, 0.5f).Active = true;
            ListContainerBackground.HeightAnchor.ConstraintEqualTo(40).Active = true;

            DetailsLabel.LeadingAnchor.ConstraintEqualTo(ListContainerBackground.LeadingAnchor, 8).Active = true;
            DetailsLabel.TrailingAnchor.ConstraintEqualTo(ListContainerBackground.TrailingAnchor, -8).Active = true;
            DetailsLabel.HeightAnchor.ConstraintEqualTo(40).Active = true;
            DetailsLabel.CenterYAnchor.ConstraintEqualTo(ListContainerBackground.CenterYAnchor).Active = true;
        }

        private void ToggleErrorVisibility(bool shouldShowError)
        {
            if (shouldShowError)
            {
                Label.TextColor = Colors.DARK_RED;
                ErrorButton.Hidden = false;
                ErrorButton.UserInteractionEnabled = true;
            }
            else
            {
                Label.TextColor = Colors.Black;
                ErrorButton.Hidden = true;
                ErrorButton.UserInteractionEnabled = false;
            }
            this.SetNeedsLayout();
        }

        public void LoadWithDefaultCurrentDate(bool shouldUseDefaultCurrentDate)
        {
            DetailsLabel.Hidden = !shouldUseDefaultCurrentDate;
            _isDateCleared = !shouldUseDefaultCurrentDate;
            ClearButton.Hidden = !shouldUseDefaultCurrentDate;
            this.SetNeedsLayout();
        }

        private nfloat GetEstimatedLabelSize()
        {
            var approximateWidth = this.Bounds.Width / 2 - 21 - 16;
            var labelSize = new CGSize(approximateWidth, 1000);

            var attributesCount = new UIStringAttributes()
            {
                Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.ClaimDetailsSubquestionFotSize)
            };

            if (string.IsNullOrWhiteSpace(Label.Text))
            {
                return 0;
            }

            var estimatedFrameCountvalue = new NSString(Label.Text).
                GetBoundingRect(labelSize, NSStringDrawingOptions.UsesLineFragmentOrigin, attributesCount, null);

            return estimatedFrameCountvalue.Height;
        }

        private void Initialize()
        {
            IsEnabled = true;

            this.BackgroundColor = Colors.Clear;

            var dateMax = DateTime.Now;
            var dateMin = DateTime.Parse(Constants.MIN_DATE);
            var nsDateMin = (NSDate)DateTime.SpecifyKind(dateMin, DateTimeKind.Utc);
            var nsDateMax = (NSDate)DateTime.SpecifyKind(dateMax, DateTimeKind.Utc);

            DatePickerController = new ClaimDateModal();
            DatePickerController.View = new UIView(new CGRect(0, 0, 5, 5));
            DatePickerController.View.BackgroundColor = Colors.LightGrayColor;
            DatePickerController.datePicker = new UIDatePicker();
            DatePickerController.datePicker.Mode = UIDatePickerMode.Date;
            DatePickerController.datePicker.MinimumDate = nsDateMin;
            DatePickerController.datePicker.MaximumDate = nsDateMax;
            DatePickerController.View.AddSubview(DatePickerController.datePicker);

            if (Constants.IsPhone())
            {
                DatePickerController.dismissButton = new GSButton();
                DatePickerController.dismissButton.SetTitle("done".tr(), UIControlState.Normal);
                DatePickerController.View.AddSubview(DatePickerController.dismissButton);
                DatePickerController.dismissButton.TouchUpInside += HandleDismissButton;
            }

            _labelStackView = new UIStackView();
            _labelStackView.Axis = UILayoutConstraintAxis.Horizontal;
            _labelStackView.Alignment = UIStackViewAlignment.Fill;
            _labelStackView.Distribution = UIStackViewDistribution.Fill;
            _labelStackView.Spacing = 3;
            _labelStackView.TranslatesAutoresizingMaskIntoConstraints = false;

            Label = new UILabel();
            Label.Text = Text;
            Label.BackgroundColor = Colors.Clear;
            Label.Lines = 0;
            Label.LineBreakMode = UILineBreakMode.WordWrap;
            Label.TextColor = Colors.Black;
            Label.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.ClaimDetailsSubquestionFotSize);
            Label.TranslatesAutoresizingMaskIntoConstraints = false;
            _labelStackView.AddArrangedSubview(Label);

            ClearButton = new UIButton();
            ClearButton.BackgroundColor = Colors.Clear;
            ClearButton.SetImage(UIImage.FromBundle(Constants.ICON_PATH + "closeGrey.png"), UIControlState.Normal);
            ClearButton.AdjustsImageWhenHighlighted = true;
            ClearButton.TouchUpInside -= HandleClearButton;
            ClearButton.TouchUpInside += HandleClearButton;
            ClearButton.TranslatesAutoresizingMaskIntoConstraints = false;
            ClearButton.Hidden = true;
            _labelStackView.AddArrangedSubview(ClearButton);
            this.AddSubview(_labelStackView);

            _dateStackView = new UIStackView();
            _dateStackView.Axis = UILayoutConstraintAxis.Horizontal;
            _dateStackView.Alignment = UIStackViewAlignment.Center;
            _dateStackView.Distribution = UIStackViewDistribution.Fill;
            _dateStackView.TranslatesAutoresizingMaskIntoConstraints = false;
            _dateStackView.Spacing = 3;

            ErrorButton = new UIButton();
            ErrorButton.SetImage(UIImage.FromBundle("ErrorFlag"), UIControlState.Normal);
            ErrorButton.AdjustsImageWhenHighlighted = true;
            ErrorButton.TouchUpInside -= HandleErrorButton;
            ErrorButton.TouchUpInside += HandleErrorButton;
            ErrorButton.Hidden = true;
            ErrorButton.TranslatesAutoresizingMaskIntoConstraints = false;
            _dateStackView.AddArrangedSubview(ErrorButton);

            ListContainerBackground = new UIView();
            ListContainerBackground.BackgroundColor = Colors.LightGrayColor;
            ListContainerBackground.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
            ListContainerBackground.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;
            ListContainerBackground.TranslatesAutoresizingMaskIntoConstraints = false;

            _gesture = new UITapGestureRecognizer(HandleGesture);
            _gesture.NumberOfTapsRequired = 1;
            ListContainerBackground.AddGestureRecognizer(_gesture);

            _dateStackView.AddArrangedSubview(ListContainerBackground);

            DetailsLabel = new UILabel();
            DetailsLabel.LineBreakMode = UILineBreakMode.WordWrap;
            DetailsLabel.TextColor = Colors.Black;
            DetailsLabel.UserInteractionEnabled = false;
            DetailsLabel.Lines = 0;
            DetailsLabel.TextAlignment = UITextAlignment.Left;
            DetailsLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.ClaimDetailsSubquestionFotSize);
            DetailsLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            ListContainerBackground.AddSubview(DetailsLabel);

            this.AddSubview(_dateStackView);

            SetNeedsLayout();
        }

        private void HandleDismissButton(object sender, EventArgs e)
        {
            DatePickerController.DismissViewController(true, null);
        }

        private void HandleClearButton(object sender, EventArgs e)
        {
            _isDateCleared = true;
            ClearButton.Hidden = true;
            DetailsLabel.Text = string.Empty;
            ToggleErrorVisibility(false);
            DateSelectionRemoved?.Invoke(this, EventArgs.Empty);
        }

        private void HandleErrorButton(object sender, EventArgs e)
        {
            UIAlertView _error = new UIAlertView("", ErrorText, null, "ok".tr(), null);
            _error.Show();
        }

        private void HandleGesture()
        {
            if (!IsEnabled)
            {
                return;
            }

            _isDateCleared = false;
            DetailsLabel.Hidden = false;
            ClearButton.Hidden = !CanClearDate;

            if (!(ShouldUseCurrentDefaultDate && !_isDateCleared))
            {
                this.DateSelected?.Invoke(this, EventArgs.Empty);
            }

            if (Constants.IsPhone())
            {
                UIWindow keyWindow = UIApplication.SharedApplication.KeyWindow;
                DatePickerController.ModalInPopover = true;
                DatePickerController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
                keyWindow.RootViewController.PresentViewController(DatePickerController, true, null);
            }
            else
            {
                if (_popoverController == null || _popoverController.ContentViewController == null)
                {
                    _popoverController = new UIPopoverController(DatePickerController);
                }

                try
                {
                    float scrollOffset = 0;
                    if (ParentController != null && ParentController.View != null && ParentController.View.GetType() == typeof(GSCBaseView))
                    {
                        scrollOffset = (float)((GSCBaseView)ParentController.View).baseScrollContainer.ContentOffset.Y;
                    }

                    var popoverX = this.Frame.X + 0.75 * this.Frame.Width;
                    var popoverY = IsUsedInClaimHistoryDisplayByView
                        ? this.Frame.GetMaxY() + 160
                        : this.Frame.GetMaxY();

                    _popoverController.ContentViewController.View.Layer.ShadowRadius = 0.5f;

                    _popoverController
                        .SetPopoverContentSize(new CGSize(DatePickerController.datePicker.Frame.Width, DatePickerController.datePicker.Frame.Height), false);

                    _popoverController
                        .PresentFromRect(new CGRect(popoverX, popoverY, 1, 1), this, UIPopoverArrowDirection.Up, true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}