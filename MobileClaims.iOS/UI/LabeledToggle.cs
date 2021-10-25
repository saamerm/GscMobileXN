using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MobileClaims.iOS.UI
{
    [Register("LabeledToggle")]
    [DesignTimeVisible(true)]
    public class LabeledToggle : UIView
    {
        private bool _switchValue;
        private bool _isUsedAsGroupHeader;
        private bool _isSwitchEnabled = true;
        private string _text;

        public event EventHandler SwitchValueChanged;

        protected UISwitch Switch { get; set; }
        protected UIView DisableOverlayView;
        protected UIButton ErrorButton;

        public UILabel Label { get; set; }
        public bool IsInitialized { get; set; }        
        public string ErrorText { get; set; }

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

        [Export("SwitchValue")]
        [Browsable(true)]
        public bool SwitchValue
        {
            get => _switchValue;
            set
            {
                _switchValue = value;
                Switch.On = _switchValue;
                SwitchValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Export("IsUsedAsGroupHeader")]
        [Browsable(true)]
        public bool IsUsedAsGroupHeader
        {
            get => _isUsedAsGroupHeader;
            set
            {
                _isUsedAsGroupHeader = value;
                SetFontSize();
            }
        }

        public nfloat EstimatedHeight { get; private set; }

        public LabeledToggle()
        {
            Initialize();
        }

        public LabeledToggle(IntPtr handler)
            : base(handler)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            Initialize();
        }

        private void Initialize()
        {
            this.BackgroundColor = Colors.BACKGROUND_COLOR;

            Label = new UILabel();
            Label.Text = Text;
            Label.Lines = 0;
            Label.LineBreakMode = UILineBreakMode.WordWrap;
            Label.TextColor = Colors.Black;
            Label.BackgroundColor = Colors.Clear;
            Label.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.ClaimDetailsSubquestionFotSize);
            Label.TranslatesAutoresizingMaskIntoConstraints = false;
            AddSubview(Label);

            Switch = new UISwitch();
            Switch.OnTintColor = Colors.HIGHLIGHT_COLOR;
            Switch.ValueChanged -= HandleSwitch;
            Switch.ValueChanged += HandleSwitch;
            Switch.TranslatesAutoresizingMaskIntoConstraints = false;
            this.AddSubview(Switch);

            ErrorButton = new UIButton();
            ErrorButton.Hidden = true;
            ErrorButton.SetImage(UIImage.FromBundle("ErrorFlag"), UIControlState.Normal);
            ErrorButton.AdjustsImageWhenHighlighted = true;
            ErrorButton.TouchUpInside -= HandleErrorButton;
            ErrorButton.TouchUpInside += HandleErrorButton;
            ErrorButton.TranslatesAutoresizingMaskIntoConstraints = false;
            this.AddSubview(ErrorButton);

            DisableOverlayView = new UIView();
            DisableOverlayView.Alpha = 0.5f;
            DisableOverlayView.BackgroundColor = Colors.LightGrayColor;
            DisableOverlayView.TranslatesAutoresizingMaskIntoConstraints = false;
            DisableOverlayView.Hidden = true;
            this.AddSubview(DisableOverlayView);

            SetEnabled(true);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            EstimatedHeight = GetEstimatedLabelSize();
            if (EstimatedHeight < 50)
            {
                EstimatedHeight = 50;
            }

            Label.HeightAnchor.ConstraintEqualTo(EstimatedHeight).Active = true;
            Label.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;
            Label.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor).Active = true;
            Label.TrailingAnchor.ConstraintEqualTo(ErrorButton.LeadingAnchor, -5).Active = true;

            if (ErrorButton != null && ErrorButton.ImageView.Image != null)
            {
                var buttonImageWidth = ErrorButton.ImageView.Image.Size.Width;
                var buttonImageHeight = ErrorButton.ImageView.Image.Size.Height;

                ErrorButton.HeightAnchor.ConstraintEqualTo(buttonImageHeight).Active = true;
                ErrorButton.WidthAnchor.ConstraintEqualTo(buttonImageWidth).Active = true;
            }

            ErrorButton.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;
            ErrorButton.TrailingAnchor.ConstraintEqualTo(Switch.LeadingAnchor, -5).Active = true;

            Switch.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;
            Switch.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor, -5).Active = true;

            DisableOverlayView.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor).Active = true;
            DisableOverlayView.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor).Active = true;
            DisableOverlayView.HeightAnchor.ConstraintEqualTo(EstimatedHeight).Active = true;
            DisableOverlayView.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;

            //SetEnabled(true);
        }

        public void SetEnabled(bool enabled, bool aniamte = false)
        {
            var duration = aniamte ? Constants.TOGGLE_ANIMATION_DURATION : 0;

            _isSwitchEnabled = enabled;
            if (_isSwitchEnabled)
            {
                Switch.UserInteractionEnabled = ErrorButton.UserInteractionEnabled = true;
                UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
                    () =>
                    {
                        DisableOverlayView.Alpha = 0;
                    },
                    () =>
                    {
                        DisableOverlayView.Hidden = true;
                    });
            }
            else
            {
                DisableOverlayView.Alpha = 0;
                DisableOverlayView.Hidden = false;
                Switch.UserInteractionEnabled = ErrorButton.UserInteractionEnabled = false;
                UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
                    () =>
                    {
                        DisableOverlayView.Alpha = 0.5f;
                    },
                    () =>
                    {
                        this.LayoutSubviews();
                    });
            }
        }

        protected void HandleErrorButton(object sender, EventArgs e)
        {
            UIAlertView _error = new UIAlertView("", ErrorText, null, "ok".tr(), null);
            _error.Show();
        }

        private void HandleSwitch(object sender, EventArgs e)
        {
            SwitchValue = (sender as UISwitch).On;
        }

        private void SetFontSize()
        {
            if (IsUsedAsGroupHeader)
            {
                Label.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.ClaimDetailsQuestionFontSize);
            }
            else
            {
                Label.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.ClaimDetailsSubquestionFotSize);
            }
        }

        private nfloat GetEstimatedLabelSize()
        {
            var approximateWidth = this.Bounds.Width - 5 - ErrorButton.Bounds.Width - 5 - Switch.Bounds.Width;
            var labelSize = new CGSize(approximateWidth, 1000);

            var attributesCount = new UIStringAttributes()
            {
                Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, IsUsedAsGroupHeader
                    ? Constants.ClaimDetailsQuestionFontSize
                    : Constants.ClaimDetailsSubquestionFotSize)                
            };

            if (string.IsNullOrWhiteSpace(Label.Text))
            {
                return 0;
            }

            var estimatedFrameCountvalue = new NSString(Label.Text).
                GetBoundingRect(labelSize, NSStringDrawingOptions.UsesLineFragmentOrigin, attributesCount, null);

            return estimatedFrameCountvalue.Height + 16;
        }
    }
}