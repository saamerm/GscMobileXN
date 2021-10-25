using System;
using System.ComponentModel;
using Foundation;
using UIKit;

namespace MobileClaims.iOS.UI
{
    [Register("LabeledTextField")]
    [DesignTimeVisible(true)]
    public class LabeledTextField : UIView
    {
        private string _text;
        private string _value;
        private bool _shouldShowError;
        private bool _isEnabled = true;

        protected UIButton ErrorButton { get; set; }
        protected UIView DisableOverlayView { get; set; }

        public UILabel Label { get; set; }
        public DefaultTextField TextField { get; set; }
        public string ErrorText { get; set; }

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

        [Export("Value")]
        [Browsable(true)]
        public string Value { get; set; }
      
        public LabeledTextField()
        {
            Initialize();
        }

        public LabeledTextField(IntPtr handler)
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
            Label.BackgroundColor = Colors.BACKGROUND_COLOR;
            Label.Lines = 0;
            Label.LineBreakMode = UILineBreakMode.WordWrap;
            Label.TextColor = Colors.Black;
            Label.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.ClaimDetailsSubquestionFotSize);
            Label.TranslatesAutoresizingMaskIntoConstraints = false;
            AddSubview(Label);

            TextField = new DefaultTextField();
            TextField.Placeholder = "XXXXXX";
            TextField.Text = Value;
            TextField.TextColor = Colors.Black;
            TextField.BackgroundColor = Colors.LightGrayColor;
            TextField.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.ClaimDetailsSubquestionFotSize);
            TextField.TranslatesAutoresizingMaskIntoConstraints = false;
            AddSubview(TextField);

            ErrorButton = new UIButton();
            ErrorButton.Hidden = true;
            ErrorButton.SetImage(UIImage.FromBundle("ErrorFlag"), UIControlState.Normal);
            ErrorButton.AdjustsImageWhenHighlighted = true;
            ErrorButton.TouchUpInside -= HandleErrorButton;
            ErrorButton.TouchUpInside += HandleErrorButton;
            ErrorButton.TranslatesAutoresizingMaskIntoConstraints = false;
            AddSubview(ErrorButton);

            DisableOverlayView = new UIView();
            DisableOverlayView.Alpha = 0.5f;
            DisableOverlayView.BackgroundColor = Colors.LightGrayColor;
            DisableOverlayView.TranslatesAutoresizingMaskIntoConstraints = false;
            DisableOverlayView.Hidden = true;
            AddSubview(DisableOverlayView);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            
            Label.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor).Active = true;
            Label.TrailingAnchor.ConstraintEqualTo(ErrorButton.LeadingAnchor, -5).Active = true;
            Label.TopAnchor.ConstraintEqualTo(this.TopAnchor).Active = true;
            Label.BottomAnchor.ConstraintEqualTo(this.BottomAnchor).Active = true;

            if (ErrorButton != null && ErrorButton.ImageView.Image != null)
            {
                var buttonImageWidth = ErrorButton.ImageView.Image.Size.Width;
                var buttonImageHeight = ErrorButton.ImageView.Image.Size.Height;

                ErrorButton.HeightAnchor.ConstraintEqualTo(buttonImageHeight).Active = true;
                ErrorButton.WidthAnchor.ConstraintEqualTo(buttonImageWidth).Active = true;
            }

            ErrorButton.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;
            ErrorButton.TrailingAnchor.ConstraintEqualTo(TextField.LeadingAnchor, -5).Active = true;

            TextField.HeightAnchor.ConstraintEqualTo(40).Active = true;
            TextField.WidthAnchor.ConstraintEqualTo(this.WidthAnchor, 0.5f).Active = true;
            TextField.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;
            TextField.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor, -5).Active = true;

            DisableOverlayView.TopAnchor.ConstraintEqualTo(this.TopAnchor).Active = true;
            DisableOverlayView.BottomAnchor.ConstraintEqualTo(this.BottomAnchor).Active = true;
            DisableOverlayView.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor).Active = true;
            DisableOverlayView.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor).Active = true;
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
        }

        public void SetEnabled(bool isEnabled, bool animated)
        {
            float duration = animated ? Constants.TOGGLE_ANIMATION_DURATION : 0;

            _isEnabled = isEnabled;

            if (isEnabled)
            {
                TextField.UserInteractionEnabled = ErrorButton.UserInteractionEnabled = true;
                UIView.Animate(duration, 0, (UIViewAnimationOptions)UIViewAnimationOptions.CurveEaseInOut,
                    () =>
                    {
                        DisableOverlayView.Alpha = 0;
                    },
                    () =>
                    {
                        DisableOverlayView.Hidden = true;
                    }
                );
            }
            else
            {
                DisableOverlayView.Hidden = false;
                TextField.UserInteractionEnabled = ErrorButton.UserInteractionEnabled = false;
                UIView.Animate(duration, 0, (UIViewAnimationOptions)UIViewAnimationOptions.CurveEaseInOut,
                    () =>
                    {
                        DisableOverlayView.Alpha = 0.5f;
                    },
                    () =>
                    {
                    }
                );
            }
        }

        private void HandleErrorButton(object sender, EventArgs e)
        {
            UIAlertView _error = new UIAlertView("", ErrorText, null, "ok".tr(), null);

            _error.Show();
        }
    }
}