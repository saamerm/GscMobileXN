using System;
using System.ComponentModel;
using System.Windows.Input;
using CoreGraphics;
using Foundation;
using MobileClaims.iOS.Extensions;
using UIKit;

namespace MobileClaims.iOS.UI
{
    [Register("RadioButtonWithLargeImage")]
    [DesignTimeVisible(true)]
    public class RadioButtonWithLargeImage : UIButton
    {
        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                SetTitle(_text, UIControlState.Normal);
            }
        }

        public override bool Selected
        {
            get => base.Selected;
            set
            {
                base.Selected = value;
                if (base.Selected)
                {
                    BackgroundColor = Colors.HIGHLIGHT_COLOR;
                }
                else
                {
                    BackgroundColor = Colors.Clear;
                }
            }
        }

        public RadioButtonWithLargeImage(IntPtr handle)
            : base(handle)
        {
        }

        public RadioButtonWithLargeImage()
        {
            Initialize();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            Initialize();
        }

        public override void SetImage(UIImage image, UIControlState forState)
        {
            var newImage = image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            base.SetImage(newImage, forState);
        }

        private void Initialize()
        {
            this.HorizontalAlignment = UIControlContentHorizontalAlignment.Leading;

            TintColor = UIColor.Clear;
            this.TouchUpInside += OnTouchUpInside;

            SetImage(UIImage.FromBundle("LargeRoundCheckboxSelected"), UIControlState.Selected);
            SetImage(UIImage.FromBundle("LargeRoundCheckboxUnselected"), UIControlState.Normal);

            SetTitleColor(UIColor.FromRGB(27, 27, 27), UIControlState.Normal);
            SetTitleColor(Colors.BACKGROUND_COLOR, UIControlState.Selected);

            TitleLabel.Lines = 0;
            TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            TitleLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14.0f);

            var iamgePos = ImageView.Bounds;

            TitleEdgeInsets = new UIEdgeInsets(0, 40, 0, -40);

            ImageEdgeInsets = new UIEdgeInsets(0, 20, 0, 0);

            ContentEdgeInsets = new UIEdgeInsets(0, 0, 0, 40);

            Layer.BorderColor = UIColor.FromRGB(0.88f, 0.88f, 0.88f).CGColor;
            Layer.BorderWidth = 1.0f;
        }

        private void OnTouchUpInside(object sender, EventArgs e)
        {
            this.Selected = !this.Selected;
        }
    }

    [Register("RadioButtonWithBorder")]
    [DesignTimeVisible(true)]
    public class RadioButtonWithBorder : UIView
    {
        private string _text;
        private UITapGestureRecognizer _gesture;
        private bool _selectionValue;

        internal UIButton OverlayButton { get; set; }
        internal UIImageView RadioButtonImage { get; set; }
        internal UILabel Title { get; set; }

        public event EventHandler RadioButtonValueChanged;

        [Export("Text")]
        [Browsable(true)]
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                Title.Text = _text;
                SetNeedsLayout();
            }
        }

        [Export("SelectionValue")]
        [Browsable(true)]
        public bool SelectionValue
        {
            get => _selectionValue;
            set
            {
                _selectionValue = value;
                OverlayButton.Selected = _selectionValue;
                ToggleImages();
                RadioButtonValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public RadioButtonWithBorder()
        {
            Initialize();
        }

        public RadioButtonWithBorder(IntPtr handle)
            : base(handle)
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

            var estimatedHeight = GetEstimatedLabelSize();
            if (estimatedHeight < 50)
            {
                estimatedHeight = 50;
            }

            this.HeightAnchor.ConstraintEqualTo(estimatedHeight + 20).Active = true;

            RadioButtonImage.HeightAnchor.ConstraintEqualTo(32).Active = true;
            RadioButtonImage.WidthAnchor.ConstraintEqualTo(32).Active = true;
            RadioButtonImage.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor).Active = true;
            RadioButtonImage.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor, 20).Active = true;

            //Title.HeightAnchor.ConstraintEqualTo(estimatedHeight).Active = true;
            Title.LeadingAnchor.ConstraintEqualTo(RadioButtonImage.TrailingAnchor, 20).Active = true;
            Title.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor, -15).Active = true;
            Title.TopAnchor.ConstraintEqualTo(this.TopAnchor, 10).Active = true;
            Title.BottomAnchor.ConstraintEqualTo(this.BottomAnchor, -10).Active = true;

            //OverlayButton.HeightAnchor.ConstraintEqualTo(estimatedHeight).Active = true;
            //OverlayButton.WidthAnchor.ConstraintEqualTo(this.WidthAnchor).Active = true;
            //OverlayButton.HeightAnchor.ConstraintEqualTo(this.HeightAnchor).Active = true;

            OverlayButton.TopAnchor.ConstraintEqualTo(this.TopAnchor).Active = true;
            OverlayButton.BottomAnchor.ConstraintEqualTo(this.BottomAnchor).Active = true;
            OverlayButton.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor).Active = true;
            OverlayButton.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor).Active = true;
        }

        private void Initialize()
        {
            Title = new UILabel
            {
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
                BackgroundColor = Colors.DARK_RED,
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            Title.SetLabel(Constants.NUNITO_SEMIBOLD, 14, Colors.Black);

            RadioButtonImage = new UIImageView
            {
                Image = UIImage.FromBundle("LargeRoundCheckboxUnselected"),
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            OverlayButton = new UIButton();
            OverlayButton.Frame = new CGRect(0, 0, Frame.Width, 50);
            OverlayButton.UserInteractionEnabled = true;
            OverlayButton.TranslatesAutoresizingMaskIntoConstraints = false;
            OverlayButton.TouchUpInside += OverlayButton_TouchUpInside;

            Layer.BorderColor = UIColor.FromRGB(0.88f, 0.88f, 0.88f).CGColor;
            Layer.BorderWidth = 1.0f;

            AddSubview(RadioButtonImage);
            AddSubview(Title);
            AddSubview(OverlayButton);
        }

        private void OverlayButton_TouchUpInside(object sender, EventArgs e)
        {
            SelectionValue = !SelectionValue;
            OverlayButton.Selected = SelectionValue;
            ToggleImages();
        }

        private void ToggleImages()
        {
            if (SelectionValue)
            {
                RadioButtonImage.Image = UIImage.FromBundle("LargeRoundCheckboxSelected");
                this.BackgroundColor = Colors.HIGHLIGHT_COLOR;
                Title.TextColor = Colors.BACKGROUND_COLOR;
            }
            else
            {
                RadioButtonImage.Image = UIImage.FromBundle("LargeRoundCheckboxUnselected");
                this.BackgroundColor = Colors.Clear;
                Title.TextColor = Colors.Black;
            }
        }

        private nfloat GetEstimatedLabelSize()
        {
            var approximateWidth = this.Bounds.Width - 20 - 20 - RadioButtonImage.Bounds.Width - 15;
            var labelSize = new CGSize(approximateWidth, 1000);

            var attributesCount = new UIStringAttributes()
            {
                Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14)
            };

            if (string.IsNullOrWhiteSpace(Title.Text))
            {
                return 0;
            }

            var estimatedFrameCountvalue = new NSString(Title.Text).
                GetBoundingRect(labelSize, NSStringDrawingOptions.UsesLineFragmentOrigin, attributesCount, null);

            return estimatedFrameCountvalue.Height + 16;
        }
    }
}