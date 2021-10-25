using System;
using CoreGraphics;
using UIKit;
using Foundation;
using System.ComponentModel;

namespace MobileClaims.iOS
{
    [Register("GSButton")]
    [DesignTimeVisible(true)]
    public class GSButton : UIButton
    {
        private float _textHeight = Constants.IsPhone() ? 25 : 45;

        [Export("FixedFontSize")]
        [Browsable(true)]
        public bool FixedFontSize { get; set; }

        [Export("IsSelectable")]
        [Browsable(true)]
        public bool IsSelectable { get; set; }

        public override bool Selected
        {
            get
            {
                return base.Selected;
            }
            set
            {
                base.Selected = value;
                if (IsSelectable)
                {
                    SetHighlightForSelectableButton(value);
                }
            }
        }
        
        public GSButton(IntPtr handler)
            : base(handler)
        {
        }

        public GSButton()
        {
            Initialize();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            Initialize();
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            if (!IsSelectable)
            {
                SetHighlight(true);
            }
            else
            {
                SetHighlightForSelectableButton(true);
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            if (!IsSelectable)
            {
                SetHighlight(false);
            }
            else
            {
                SetHighlightForSelectableButton(this.Selected);
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);
            if (!IsSelectable)
            {
                SetHighlight(false);
            }
            else
            {
                SetHighlightForSelectableButton(this.Selected);
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (!FixedFontSize && !IsSelectable)
            {
                if (this.TitleLabel.Text != null)
                {
                    UIFont sizedFont = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.GS_BUTTON_FONT_SIZE);

                    for (int i = (int)Constants.GS_BUTTON_FONT_SIZE; i > 10; i = i - 2)
                    {
                        sizedFont = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)i);

                        CGSize constraintSize = new CGSize(260.0f, float.MaxValue);
                        CGSize labelSize = this.TitleLabel.Text.StringSize(sizedFont);
                        if (labelSize.Height <= _textHeight)
                        {
                            break;
                        }
                    }
                    this.TitleLabel.Font = sizedFont;
                }
            }
        }

        private void Initialize()
        {
            if (!IsSelectable)
            {
                SetGSButtonProperties();
            }
            else
            {
                SetGSSelectionButtonProperties();
            }
        }

        // Set properties as per GSButton
        private void SetGSButtonProperties()
        {
            this.BackgroundColor = Colors.HIGHLIGHT_COLOR;

            // TODO: This two property aren't set for original GCButtonFixedFontSize
            this.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
            this.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;

            this.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
            this.SetTitleColor(Colors.BACKGROUND_COLOR, UIControlState.Normal);

            this.TitleLabel.Text = this.TitleLabel.Text.tr();
            this.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            this.TitleLabel.TextAlignment = UITextAlignment.Center;
            this.TitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.GS_BUTTON_FONT_SIZE);

            ContentEdgeInsets = new UIEdgeInsets(3, 0, 0, 0);
        }

        // Set properties as per GSSelectionButton
        private void SetGSSelectionButtonProperties()
        {
            this.BackgroundColor = Colors.LightGrayColor;
            this.Layer.BorderColor = Colors.Clear.CGColor;
            this.Layer.BorderWidth = 0;
            this.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
            this.SetTitleColor(Colors.DARK_GREY_COLOR, UIControlState.Normal);

            this.TitleLabel.Text = this.TitleLabel.Text.tr();
            this.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            this.TitleLabel.TextAlignment = UITextAlignment.Center;
            this.TitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.GS_SELECTION_BUTTON);
        }

        // Set hightlight properties as per GSButton
        private void SetHighlight(bool highlighted)
        {
            if (highlighted)
            {             
                this.BackgroundColor = Colors.LightGrayColor;
                this.SetTitleColor(Colors.DARK_GREY_COLOR, UIControlState.Normal);
            }
            else
            {
                this.BackgroundColor = Colors.HIGHLIGHT_COLOR;
                this.SetTitleColor(Colors.BACKGROUND_COLOR, UIControlState.Normal);
            }
        }

        // Set hightlight properties as per GSSelectionButton
        private void SetHighlightForSelectableButton(bool highlighted)
        {
            if (highlighted)
            {
                this.TintColor = Colors.HIGHLIGHT_COLOR;
                this.BackgroundColor = Colors.HIGHLIGHT_COLOR;
                this.SetTitleColor(Colors.BACKGROUND_COLOR, UIControlState.Normal);
            }
            else
            {
                this.TintColor = Colors.LightGrayColor;
                this.BackgroundColor = Colors.LightGrayColor;
                this.SetTitleColor(Colors.DARK_GREY_COLOR, UIControlState.Normal);
            }
        }
    }
}