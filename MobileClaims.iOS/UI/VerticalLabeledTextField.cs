using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CoreGraphics;
using Foundation;
using MobileClaims.iOS.CellTemplates;
using MvvmCross.Base;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.UI
{
    [Register("VerticalLabeledTextField")]
    [DesignTimeVisible(true)]
    public class VerticalLabeledTextField : UIStackView
    {
        private string _text, _text2;
        private string _value;
        private UIKeyboardType _keyboardType;
        private CGColor _errorBorderColor;
        private CGColor _highlightBorderColor;
        private CGColor _borderColor = UIColor.FromRGB(224, 224, 224).CGColor;
        private UIColor _textColor;
        private UIColor _valueColor;
        private UITableView _errorTableView;

        protected UILabel Label { get; set; }
        protected UILabel Label2 { get; set; }
        protected UIView ErrorView { get; set; }

        public UITextField TextField { get; set; }

        public uint MaxLength { get; set; }

        [Export("Text")]
        [Browsable(true)]
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                Label.Text = _text;
                SetNeedsLayout();
            }
        }
        
        [Export("TextColor")]
        [Browsable(true)]
        public UIColor TextColor
        {
            get => _textColor;
            set
            {
                _textColor = value;
                Label.TextColor = _textColor;
            }
        }

        [Export("Value")]
        [Browsable(true)]
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                TextField.Text = _value;
                SetNeedsLayout();
            }
        }

        [Export("ValueColor")]
        [Browsable(true)]
        public UIColor ValueColor
        {
            get => _valueColor;
            set
            {
                _valueColor = value;
                TextField.TextColor = ValueColor;
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

        

        public CGColor BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                TextField.Layer.BorderColor = _borderColor;
                TextField.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;
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

        public string ErrorText { get; set; }

        private bool _shouldShowError;
        private bool _isEnabled = true;

        public VerticalLabeledTextField()
        {
            Initialize();
        }

        public VerticalLabeledTextField(IntPtr handler)
            : base(handler)
        {
            Initialize();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            Initialize();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            Label.TopAnchor.ConstraintEqualTo(this.TopAnchor).Active = true;
            Label.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor).Active = true;
            Label.WidthAnchor.ConstraintEqualTo(150).Active = true;
      
            TextField.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor).Active = true;
            TextField.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor).Active = true;
            TextField.HeightAnchor.ConstraintEqualTo(43).Active = true;
            TextField.WidthAnchor.ConstraintEqualTo(150).Active = true;

        }

        private void Initialize()
        {
            HighlighBorderColor = Colors.HIGHLIGHT_COLOR.CGColor;

            Spacing = 4.5f;
            Axis = UILayoutConstraintAxis.Vertical;
            Alignment = UIStackViewAlignment.Fill;
            Distribution = UIStackViewDistribution.Fill;

            Label = new UILabel
            {
                Text = string.Empty,
                //BackgroundColor = Colors.DARK_RED,
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
                TextColor = TextColor,
                Font = UIFont.FromName(Constants.NUNITO_BOLD, 14),
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            AddArrangedSubview(Label);
            TextField = new DefaultTextField
            {
                BackgroundColor = Colors.Clear,
                TextColor = ValueColor,
                Font = UIFont.FromName(Constants.NUNITO_BOLD, 14),
                TranslatesAutoresizingMaskIntoConstraints = false,
                ShouldChangeCharacters = (txt, range, replacementString) =>
                {
                    var newLength = txt.Text.Length + replacementString.Length - range.Length;
                    return newLength <= MaxLength;
                }
            };

            TextField.Layer.BorderColor = BorderColor;
            TextField.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;
            TextField.KeyboardType = UIKeyboardType.NumberPad;
            AddArrangedSubview(TextField);

            TextField.EditingDidBegin += TextField_EditingDidBegin;
            TextField.EditingDidEnd += TextField_EditingDidEnd;
            
        }

        private void TextField_EditingDidEnd(object sender, EventArgs e)
        {
            Label.TextColor = TextColor;
            if (TextField.Layer.BorderColor == HighlighBorderColor)
            {
                TextField.Layer.BorderColor = BorderColor;
            }
        }

        private void TextField_EditingDidBegin(object sender, EventArgs e)
        {
            Label.TextColor = Colors.HIGHLIGHT_COLOR;
            if (TextField.Layer.BorderColor == BorderColor)
            {
                TextField.Layer.BorderColor = HighlighBorderColor;
            }
        }
    }
    
}