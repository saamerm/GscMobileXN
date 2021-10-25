using System;
using System.ComponentModel;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MobileClaims.iOS.UI
{
    [Register("ExpandableView")]
    [DesignTimeVisible(true)]
    public class ExpandableView : UIView
    {
        private bool _isExpanded;
        private string _stepNumber;
        private string _stepName;
        private UITapGestureRecognizer _gesture;
        private UIView _topBorderView;
        private UIView _bottomBorderView;
        private bool _isStepCompleted;

        public event EventHandler ViewExpanded;

        public UILabel StepNameLabel { get; set; }
        public UIImageView CheckboxImageView { get; set; }
        public UIImageView ArrowImage { get; set; }

        public nfloat EstimatedHeight { get; private set; }

        [Export("IsExpanded")]
        [Browsable(true)]
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                ToggleImages();
                ViewExpanded?.Invoke(this, EventArgs.Empty);
                SetNeedsLayout();
            }
        }

        [Export("StepNumber")]
        [Browsable(true)]
        public string StepNumber
        {
            get => _stepNumber;
            set
            {
                _stepNumber = value;
                SetNeedsLayout();
            }
        }

        [Export("StepName")]
        [Browsable(true)]
        public string StepName
        {
            get => _stepName;
            set
            {
                _stepName = value;
                SetNeedsLayout();
            }
        }

        [Export("IsStepCompleted")]
        [Browsable(true)]
        public bool IsStepCompleted
        {
            get => _isStepCompleted;
            set
            {
                _isStepCompleted = value;
                ToggleRoundImage();
            }
        }

        public ExpandableView()
        {
            Initialize();
        }

        public ExpandableView(IntPtr handler)
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

            if (string.IsNullOrWhiteSpace(StepName) || string.IsNullOrWhiteSpace(StepNumber))
            {
                return;
            }

            EstimatedHeight = GetEstimatedLabelSize();
            if (EstimatedHeight < 56)
            {
                EstimatedHeight = 56;
            }

            CheckboxImageView.HeightAnchor.ConstraintEqualTo(20).Active = true;
            CheckboxImageView.WidthAnchor.ConstraintEqualTo(20).Active = true;
            CheckboxImageView.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;
            CheckboxImageView.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor, 20).Active = true;
            CheckboxImageView.TrailingAnchor.ConstraintEqualTo(StepNameLabel.LeadingAnchor, -16).Active = true;

            StepNameLabel.HeightAnchor.ConstraintEqualTo(EstimatedHeight).Active = true;
            StepNameLabel.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;
            StepNameLabel.TrailingAnchor.ConstraintEqualTo(ArrowImage.LeadingAnchor, -16).Active = true;

            ArrowImage.WidthAnchor.ConstraintEqualTo(23).Active = true;
            ArrowImage.HeightAnchor.ConstraintEqualTo(12).Active = true;
            ArrowImage.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;
            ArrowImage.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor, -20).Active = true;

            _topBorderView.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor).Active = true;
            _topBorderView.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor).Active = true;
            _topBorderView.TopAnchor.ConstraintEqualTo(this.TopAnchor).Active = true;
            _topBorderView.HeightAnchor.ConstraintEqualTo(1).Active = true;

            _bottomBorderView.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor).Active = true;
            _bottomBorderView.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor).Active = true;
            _bottomBorderView.BottomAnchor.ConstraintEqualTo(this.BottomAnchor).Active = true;
            _bottomBorderView.HeightAnchor.ConstraintEqualTo(1).Active = true;
        }

        public void ToggleBottomBorderView(bool hideBorder)
        {
            _bottomBorderView.Hidden = hideBorder;
        }

        private void Initialize()
        {
            this.BackgroundColor = Colors.BACKGROUND_COLOR;

            CheckboxImageView = new UIImageView();
            CheckboxImageView.Image = UIImage.FromBundle("RoundCheckBoxUnselected");
            CheckboxImageView.TranslatesAutoresizingMaskIntoConstraints = false;
            this.AddSubview(CheckboxImageView);

            StepNameLabel = new UILabel();
            StepNameLabel.Lines = 0;
            StepNameLabel.LineBreakMode = UILineBreakMode.WordWrap;
            StepNameLabel.TextColor = Colors.Black;
            StepNameLabel.BackgroundColor = Colors.Clear;
            StepNameLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.ClaimDetailsSubquestionFotSize);
            StepNameLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            AddSubview(StepNameLabel);

            ArrowImage = new UIImageView();
            ArrowImage.Image = UIImage.FromBundle("ArrowDownGrayMedium");
            ArrowImage.TranslatesAutoresizingMaskIntoConstraints = false;
            this.AddSubview(ArrowImage);

            _topBorderView = new UIView(new CGRect(0, 0, this.Frame.Width, 1));
            _topBorderView.BackgroundColor = Colors.HIGHLIGHT_COLOR;
            _topBorderView.TranslatesAutoresizingMaskIntoConstraints = false;
            this.AddSubview(_topBorderView);

            _bottomBorderView = new UIView(new CGRect(0, 0, this.Frame.Width, 1));
            _bottomBorderView.BackgroundColor = Colors.DARK_RED;
            _bottomBorderView.TranslatesAutoresizingMaskIntoConstraints = false;
            _bottomBorderView.Hidden = true;
            this.AddSubview(_bottomBorderView);

            _gesture = new UITapGestureRecognizer(HandleGesture);
            _gesture.NumberOfTapsRequired = 1;
            AddGestureRecognizer(_gesture);
        }

        private void HandleGesture()
        {
            IsExpanded = !IsExpanded;
            ToggleImages();
            ViewExpanded?.Invoke(this, EventArgs.Empty);
        }

        private void ToggleImages()
        {
            if (IsExpanded)
            {
                ArrowImage.Image = UIImage.FromBundle("ArrowUpGrayMedium");
               // _topBorderView.BackgroundColor = Colors.DARK_RED;
               // _bottomBorderView.Hidden = true;
            }
            else
            {
                ArrowImage.Image = UIImage.FromBundle("ArrowDownGrayMedium");
                //_topBorderView.BackgroundColor = Colors.HIGHLIGHT_COLOR;
               // _bottomBorderView.Hidden = true;
            }
        }

        private void ToggleRoundImage()
        {
            if (IsStepCompleted)
            {
                CheckboxImageView.Image = UIImage.FromBundle("RoundCheckboxSelected");
            }
            else
            {
                CheckboxImageView.Image = UIImage.FromBundle("RoundCheckBoxUnselected");
            }
        }

        private nfloat GetEstimatedLabelSize()
        {
            var stepTextAttributes = new UIStringAttributes
            {
                ForegroundColor = Colors.Black,
                Font = UIFont.FromName(Constants.NUNITO_BOLD, 14),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.AuditListNotesLineSpacing,
                    LineHeightMultiple = Constants.AuditListNotesLineSpacing
                }
            };

            var stepNameTextAttributes = new UIStringAttributes
            {
                ForegroundColor = Colors.HIGHLIGHT_COLOR,
                Font = UIFont.FromName(Constants.NUNITO_BOLD, 14),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.AuditListNotesLineSpacing,
                    LineHeightMultiple = Constants.AuditListNotesLineSpacing
                }
            };

            var mutableAttributeString = new NSMutableAttributedString($"{StepNumber} {StepName}");

            mutableAttributeString.SetAttributes(stepTextAttributes,
                new NSRange(0, StepNumber.Length + 1));

            mutableAttributeString.SetAttributes(stepNameTextAttributes,
              new NSRange(StepNumber.Length + 1, StepName.Length));

            StepNameLabel.AttributedText = mutableAttributeString;

            var approximateWidth = this.Bounds.Width - CheckboxImageView.Bounds.Width - 16 - ArrowImage.Bounds.Width - 16;
            var labelSize = new CGSize(approximateWidth, 1000);

            if (string.IsNullOrWhiteSpace(StepNameLabel.Text))
            {
                return 0;
            }

            var estimatedFrameStepTextvalue1 = new NSString(StepNumber + string.Empty).
                GetBoundingRect(labelSize, NSStringDrawingOptions.UsesLineFragmentOrigin, stepTextAttributes, null);

            var estimatedFrameStepNameValue2 = new NSString(StepName).
                GetBoundingRect(labelSize, NSStringDrawingOptions.UsesLineFragmentOrigin, stepNameTextAttributes, null);

            return estimatedFrameStepTextvalue1.Height + estimatedFrameStepNameValue2.Height + 16;
        }
    }
}