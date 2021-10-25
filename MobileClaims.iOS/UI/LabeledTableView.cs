using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MobileClaims.iOS.UI
{
    [Register("LabeledTableView")]
    [DesignTimeVisible(true)]
    public class LabeledTableView : UIView
    {
        public event EventHandler ItemSelected;
        public event EventHandler ItemSelectionRemoved;

        private UIStackView _labelStackView;
        private UIStackView _selectionStackView;

        private UITapGestureRecognizer _gesture;

        private bool listClear;
        private bool _isListCleared;
        private bool _canRemoveSelection = true;
        private bool _shouldShowError;

        private string _text;
        private string _detailsText;
        private float _componentheight = 200;
        private float _componentWidth = 200;

        protected UIButton ErrorButton { get; set; }
        protected UIView ListContainerBackground { get; set; }

        public UILabel Label { get; set; }
        public UILabel DetailsLabel { get; set; }
        public UIButton ClearButton { get; private set; }
        public UIPopoverController PopoverController { get; set; }
        public ClaimListModal TableViewController { get; set; }

        public bool IsEnabled { get; protected set; }
        public nfloat EstimatedHeight { get; protected set; }

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
            get => _text;
            set
            {
                _text = value;
                Label.Text = _text;
            }
        }

        [Export("CanRemoveSelection")]
        [Browsable(true)]
        public bool CanRemoveSelection
        {
            get => _canRemoveSelection;
            set
            {
                _canRemoveSelection = value;
                ClearButton.Hidden = !_canRemoveSelection;
            }
        }

        [Export("ErrorText")]
        [Browsable(true)]
        public string ErrorText { get; set; }

        public LabeledTableView()
        {
        }

        public LabeledTableView(IntPtr handler)
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

            EstimatedHeight = GetEstimatedLabelSize();
            if (EstimatedHeight < 50)
            {
                EstimatedHeight = 50;
            }

            _labelStackView.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;
            _labelStackView.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor).Active = true;
            _labelStackView.HeightAnchor.ConstraintGreaterThanOrEqualTo(EstimatedHeight).Active = true;
            _labelStackView.TrailingAnchor.ConstraintEqualTo(_selectionStackView.LeadingAnchor, -3).Active = true;

            Label.TopAnchor.ConstraintEqualTo(_labelStackView.TopAnchor).Active = true;
            Label.BottomAnchor.ConstraintEqualTo(_labelStackView.BottomAnchor).Active = true;

            _selectionStackView.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;
            _selectionStackView.HeightAnchor.ConstraintEqualTo(this.HeightAnchor).Active = true;
            _selectionStackView.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor, -5).Active = true;

            if (ErrorButton != null && ErrorButton.ImageView.Image != null)
            {
                var buttonImageWidth = ErrorButton.ImageView.Image.Size.Width;
                var buttonImageHeight = ErrorButton.ImageView.Image.Size.Height;

                ErrorButton.HeightAnchor.ConstraintEqualTo(buttonImageHeight).Active = true;
                ErrorButton.WidthAnchor.ConstraintEqualTo(buttonImageWidth).Active = true;
                ErrorButton.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;
            }

            if (ClearButton != null && ClearButton.ImageView.Image != null)
            {
                var buttonImageWidth = ClearButton.ImageView.Image.Size.Width;
                var buttonImageHeight = ClearButton.ImageView.Image.Size.Height;

                ClearButton.HeightAnchor.ConstraintEqualTo(buttonImageHeight).Active = true;
                ClearButton.WidthAnchor.ConstraintEqualTo(buttonImageWidth).Active = true;
                ClearButton.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;
            }

            if (!Constants.IsPhone())
            {
                TableViewController.tableView.Frame = new CGRect(0, 0, _componentWidth, _componentheight);
            }

            ListContainerBackground.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;
            ListContainerBackground.WidthAnchor.ConstraintEqualTo(this.WidthAnchor, 0.5f).Active = true;
            ListContainerBackground.HeightAnchor.ConstraintEqualTo(40).Active = true;

            DetailsLabel.LeadingAnchor.ConstraintEqualTo(ListContainerBackground.LeadingAnchor, 8).Active = true;
            DetailsLabel.TrailingAnchor.ConstraintEqualTo(ListContainerBackground.TrailingAnchor, -8).Active = true;
            DetailsLabel.HeightAnchor.ConstraintEqualTo(40).Active = true;
            DetailsLabel.CenterYAnchor.ConstraintEqualTo(ListContainerBackground.CenterYAnchor).Active = true;
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

            TableViewController = new ClaimListModal();
            TableViewController.View = new UIView(new CGRect(0, 0, 5, 5));
            TableViewController.View.BackgroundColor = Colors.LightGrayColor;
            TableViewController.tableView = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Plain);
            TableViewController.tableView.RowHeight = Constants.SINGLE_SELECTION_CELL_HEIGHT;
            TableViewController.tableView.TableHeaderView = new UIView();
            TableViewController.tableView.SeparatorColor = Colors.Clear;
            TableViewController.tableView.ShowsVerticalScrollIndicator = true;
            TableViewController.View.AddSubview(TableViewController.tableView);            

            if (Constants.IsPhone())
            {
                TableViewController.dismissButton = new GSButton();
                TableViewController.dismissButton.SetTitle("done".tr(), UIControlState.Normal);
                TableViewController.View.AddSubview(TableViewController.dismissButton);
                TableViewController.dismissButton.TouchUpInside += HandleDismissButton;
            }
            else
            {
                PopoverController = new UIPopoverController(TableViewController);
            }

            _labelStackView = new UIStackView();
            _labelStackView.Axis = UILayoutConstraintAxis.Horizontal;
            _labelStackView.Alignment = UIStackViewAlignment.Center;
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

            _selectionStackView = new UIStackView();
            _selectionStackView.Axis = UILayoutConstraintAxis.Horizontal;
            _selectionStackView.Alignment = UIStackViewAlignment.Center;
            _selectionStackView.Distribution = UIStackViewDistribution.Fill;
            _selectionStackView.TranslatesAutoresizingMaskIntoConstraints = false;
            _selectionStackView.Spacing = 3;

            ErrorButton = new UIButton();
            ErrorButton.SetImage(UIImage.FromBundle("ErrorFlag"), UIControlState.Normal);
            ErrorButton.AdjustsImageWhenHighlighted = true;
            ErrorButton.TouchUpInside -= HandleErrorButton;
            ErrorButton.TouchUpInside += HandleErrorButton;
            ErrorButton.Hidden = true;
            ErrorButton.TranslatesAutoresizingMaskIntoConstraints = false;
            _selectionStackView.AddArrangedSubview(ErrorButton);

            ListContainerBackground = new UIView();
            ListContainerBackground.BackgroundColor = Colors.LightGrayColor;
            ListContainerBackground.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
            ListContainerBackground.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;

            _gesture = new UITapGestureRecognizer(HandleGesture);
            _gesture.NumberOfTapsRequired = 1;
            ListContainerBackground.AddGestureRecognizer(_gesture);

            _selectionStackView.AddArrangedSubview(ListContainerBackground);

            DetailsLabel = new UILabel();
            DetailsLabel.BackgroundColor = Colors.Clear;
            DetailsLabel.LineBreakMode = UILineBreakMode.WordWrap;
            DetailsLabel.TextColor = Colors.Black;
            DetailsLabel.Lines = 0;
            DetailsLabel.TextAlignment = UITextAlignment.Left;
            DetailsLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.SUB_HEADING_FONT_SIZE);
            DetailsLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            ListContainerBackground.AddSubview(DetailsLabel);

            this.AddSubview(_selectionStackView);

            LayoutIfNeeded();
        }
        
        private void HandleGesture()
        {
            if (!IsEnabled)
            {
                return;
            }

            _isListCleared = false;
            DetailsLabel.Hidden = false;
            ClearButton.Hidden = !CanRemoveSelection;

            if (!_isListCleared)
            {
                this.ItemSelected?.Invoke(this, EventArgs.Empty);
            }

            if (Constants.IsPhone())
            {
                UIWindow keyWindow = UIApplication.SharedApplication.KeyWindow;
                TableViewController.ModalInPopover = true;
                TableViewController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
                keyWindow.RootViewController.PresentViewController(TableViewController, true, null);
            }
            else
            {
                try
                {
                    var popoverX = this.Frame.X + 0.75 * this.Frame.Width;
                    var popoverY = this.Frame.Y;

                    PopoverController
                        .SetPopoverContentSize(new CGSize(TableViewController.tableView.Frame.Width, _componentheight), false);

                    PopoverController
                        .PresentFromRect((CGRect)new CGRect(popoverX, popoverY, 1, 1), this, UIPopoverArrowDirection.Up, true);
                }
                catch (Exception ex)
                {
                }
            }
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

        public void ShowDetails()
        {
            DetailsLabel.Hidden = false;

            _isListCleared = false;

            ClearButton.Hidden = false;
        }

        private void HandleDismissButton(object sender, EventArgs e)
        {
            TableViewController.DismissViewController(true, null);
        }

        private void HandleErrorButton(object sender, EventArgs e)
        {
            UIAlertView _error = new UIAlertView("", ErrorText, null, "ok".tr(), null);
            _error.Show();
        }

        private void HandleClearButton(object sender, EventArgs e)
        {
            _isListCleared = true;
            ClearButton.Hidden = true;
            DetailsLabel.Hidden = true;
            this.ItemSelectionRemoved?.Invoke(this, EventArgs.Empty);
        }
    }
}
