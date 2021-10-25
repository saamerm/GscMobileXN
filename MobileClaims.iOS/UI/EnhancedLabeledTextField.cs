using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MobileClaims.iOS.UI
{
    [Register("EnhancedLabeledTextField")]
    [DesignTimeVisible(true)]
    public class EnhancedLabeledTextField : UIView
    {
        private string _text;
        private string _placeholder;

        private bool _shouldShowError;
        private bool _isEnabled = true;

        private CGColor _errorBorderColor = Colors.DARK_RED.CGColor;
        private CGColor _highlightBorderColor = Colors.HIGHLIGHT_COLOR.CGColor;
        private UIKeyboardType _keyboardType;

        protected UIButton ErrorButton { get; set; }
        protected UIView DisableOverlayView { get; set; }

        public uint MaxLength { get; set; }
        public string ErrorText { get; set; }

        public UILabel Label { get; set; }
        public DefaultTextField TextField { get; set; }

        public bool Enabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                SetEnabled(value, true);
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                Label.Text = _text;
            }
        }

        public string Placeholder
        {
            get => _placeholder;
            set
            {
                _placeholder = value;
                TextField.Placeholder = _placeholder;
            }
        }

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

        public UIKeyboardType KeyboardType
        {
            get => _keyboardType;
            set
            {
                _keyboardType = value;
                this.TextField.KeyboardType = value;
            }
        }

        public CGColor ErrorBorderColor
        {
            get => _errorBorderColor;
            set
            {
                _errorBorderColor = value;
                TextField.Layer.BorderColor = _errorBorderColor;
            }
        }

        public CGColor HighlighBorderColor
        {
            get => _highlightBorderColor;
            set
            {
                _highlightBorderColor = value;
            }
        }

        public EnhancedLabeledTextField()
        {
            Initialize();
        }

        public EnhancedLabeledTextField(IntPtr handler)
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

            Label = new UILabel
            {
                BackgroundColor = Colors.BACKGROUND_COLOR,
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
                TextColor = Colors.Black,
                Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.ClaimDetailsSubquestionFotSize),
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            AddSubview(Label);

            TextField = new DefaultTextField
            {
                TextColor = Colors.Black,
                BackgroundColor = Colors.LightGrayColor,
                Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.ClaimDetailsSubquestionFotSize),
                TranslatesAutoresizingMaskIntoConstraints = false,
                ShouldChangeCharacters = (txt, range, replacementString) =>
                {
                    var newLength = txt.Text.Length + replacementString.Length - range.Length;
                    return newLength <= MaxLength;
                }
            };
            TextField.EditingDidBegin += TextField_EditingDidBegin;
            TextField.EditingDidEnd += TextField_EditingDidEnd;
            AddSubview(TextField);

            ErrorButton = new UIButton
            {
                Hidden = true
            };
            ErrorButton.SetImage(UIImage.FromBundle("ErrorFlag"), UIControlState.Normal);
            ErrorButton.AdjustsImageWhenHighlighted = true;
            ErrorButton.TouchUpInside -= HandleErrorButton;
            ErrorButton.TouchUpInside += HandleErrorButton;
            ErrorButton.TranslatesAutoresizingMaskIntoConstraints = false;
            AddSubview(ErrorButton);

            DisableOverlayView = new UIView
            {
                Alpha = 0.5f,
                BackgroundColor = Colors.LightGrayColor,
                TranslatesAutoresizingMaskIntoConstraints = false,
                Hidden = true
            };
            AddSubview(DisableOverlayView);
        }

        private void TextField_EditingDidEnd(object sender, EventArgs e)
        {
            Label.TextColor = Colors.Black;
            if (TextField.Layer.BorderColor == HighlighBorderColor)
            {
                TextField.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
            }
        }

        private void TextField_EditingDidBegin(object sender, EventArgs e)
        {
            Label.TextColor = Colors.HIGHLIGHT_COLOR;
            if (TextField.Layer.BorderColor == Colors.MED_GREY_COLOR.CGColor)
            {
                TextField.Layer.BorderColor = HighlighBorderColor;
            }
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

            DisableOverlayView.TopAnchor.ConstraintEqualTo(this.TopAnchor, -4).Active = true;
            DisableOverlayView.BottomAnchor.ConstraintEqualTo(this.BottomAnchor, 4).Active = true;
            DisableOverlayView.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor, -10).Active = true;
            DisableOverlayView.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor, 10).Active = true;
        }

        private void ToggleErrorVisibility(bool shouldShowError)
        {
            if (shouldShowError)
            {
                ErrorButton.Hidden = false;
                ErrorButton.UserInteractionEnabled = true;
                TextField.Layer.BorderColor = ErrorBorderColor;
            }
            else
            {
                ErrorButton.Hidden = true;
                ErrorButton.UserInteractionEnabled = false;
                TextField.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
            }
        }

        public void SetEnabled(bool isEnabled, bool animated)
        {
            float duration = animated ? Constants.TOGGLE_ANIMATION_DURATION : 0;

            _isEnabled = isEnabled;

            if (isEnabled)
            {
                TextField.UserInteractionEnabled = ErrorButton.UserInteractionEnabled = true;
                UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
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
                UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
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
