using System;
using CoreGraphics;
using UIKit;
using Foundation;

namespace MobileClaims.iOS
{
    public class SelectionMenuItem : UIButton
    {
        UIView bottomBorder;
        UITableViewCell check;
        UITableViewCell disclosure;
        UIImageView plusImage;
        bool _selected;

        private const float CHECK_SIZE = 30;
        private const float CHECK_PADDING = 3;
        public SelectionMenuItem(Boolean hasBottomBorder)
        {
            this.BackgroundColor = Colors.BACKGROUND_COLOR;
            this.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
            this.ContentEdgeInsets = new UIEdgeInsets(0, Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, 0, 0);
            this.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, (nfloat)Constants.SELECTION_MENU_ITEM_FONT_SIZE);

            this.check = new UITableViewCell();
            this.check.Accessory = UITableViewCellAccessory.Checkmark;
            this.check.UserInteractionEnabled = false;
            this.check.Frame = (CGRect)this.Bounds;
            this.check.Alpha = 0;
            this.AddSubview(this.check);

            plusImage = new UIImageView(UIImage.FromBundle("Plus"));
            plusImage.BackgroundColor = Colors.Clear;
            plusImage.Opaque = false;
            this.AddSubview(plusImage);

            this.TitleLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, (nfloat)Constants.SELECTION_ITEM_FONT_SIZE);
            this.SetTitleColor(Colors.DARK_GREY_COLOR, UIControlState.Normal);

            _selected = false;

            plusImage.Alpha = 0;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            setHighlight(true);
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            setHighlight(_selected);
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            setHighlight(_selected);
        }

        public override bool Selected
        {
            get
            {
                return base.Selected;
            }
            set
            {
                base.Selected = value;

                if (value)
                {
                    ShowCheck();
                }
                else
                {
                    HideCheck();
                }

            }
        }

        protected void setHighlight(bool highlighted)
        {
            if (highlighted)
            {
                this.SetTitleColor(Colors.HIGHLIGHT_COLOR, UIControlState.Normal);
                this.SetTitleColor(Colors.HIGHLIGHT_COLOR, UIControlState.Selected);
            }
            else
            {
                this.SetTitleColor(Colors.DARK_GREY_COLOR, UIControlState.Normal);
            }
        }


        public void ShowCheck()
        {
            this.plusImage.Alpha = 1;
            _selected = true;
            this.setHighlight(true);
        }

        public void HideCheck()
        {
            this.plusImage.Alpha = 0;
            _selected = false;
            this.setHighlight(false);
        }

        public void showDisclosureIndicator()
        {
            this.disclosure.Alpha = 1;
            this.check.Alpha = 0;
        }

        public void hideDisclosureIndicator()
        {
            this.disclosure.Alpha = 0;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            this.check.Frame = (CGRect)this.Bounds;

            float size = 15.0f;

            this.TitleLabel.SizeToFit();

            plusImage.Frame = new CGRect(0, (float)this.Frame.Height / 2 - size / 2, size, size);

            this.check.Frame = (CGRect)this.Bounds;
            sizeFont();
            //this.disclosure.Frame = this.Bounds;
        }

        protected void sizeFont()
        {
            if (this.TitleLabel.Text != null)
            {

                UIFont sizedFont = UIFont.FromName(Constants.NUNITO_SEMIBOLD, (nfloat)Constants.SELECTION_MENU_ITEM_FONT_SIZE);

                int i;

                for (i = (int)Constants.SELECTION_MENU_ITEM_FONT_SIZE; i > 10; i = i - 2)
                {
                    sizedFont = UIFont.FromName(Constants.NUNITO_SEMIBOLD, (nfloat)i);

                    CGSize constraintSize = new CGSize((float)this.Frame.Width - 20, float.MaxValue);

                    CGSize labelSize = this.TitleLabel.Text.StringSize(sizedFont);
                    if (labelSize.Width <= (float)this.Frame.Width - 20)
                        break;
                }

                this.TitleLabel.Font = sizedFont;
            }
        }
    }
}