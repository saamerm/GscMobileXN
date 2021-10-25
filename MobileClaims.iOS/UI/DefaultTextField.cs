using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MobileClaims.iOS
{
    [Register("DefaultTextField")]
    [DesignTimeVisible(true)]
    public class DefaultTextField : UITextField
    {
        protected float leftAdjustment = 0;

        public DefaultTextField(IntPtr handler)
            : base(handler)
        {
        }
    
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            Initialize(TextAlignment == UITextAlignment.Center);
        }

        public DefaultTextField(bool isCentered = false)
        {
            Initialize(isCentered);
        }

        public override CGRect PlaceholderRect(CGRect forBounds)
        {
            CGRect adjustedRect = (CGRect)forBounds;

            if (!Constants.IS_OS_7_OR_LATER())
            {
                adjustedRect.Y += 3;
            }

            adjustedRect.X = leftAdjustment;
            adjustedRect.Width = (float)this.Frame.Width - (2 * leftAdjustment);

            return base.PlaceholderRect(adjustedRect);
        }

        public override CGRect TextRect(CGRect forBounds)
        {
            CGRect adjustedRect = forBounds;

            adjustedRect.X = leftAdjustment;
            adjustedRect.Width = (float)this.Frame.Width - (2 * leftAdjustment);

            return base.TextRect(adjustedRect);
        }

        public override CGRect EditingRect(CGRect forBounds)
        {
            CGRect adjustedRect = forBounds;

            adjustedRect.X = leftAdjustment;
            adjustedRect.Width = (float)this.Frame.Width - (2 * leftAdjustment);

            return base.EditingRect(adjustedRect);
        }

        public void setHighlighted(bool isHighlighted)
        {
            if (isHighlighted)
            {
                this.Layer.BorderColor = Colors.ERROR_COLOR.CGColor;
            }
            else
            {
                this.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
            }
        }

        private void Initialize(bool isCentered)
        {
            this.BackgroundColor = Colors.LightGrayColor;
            this.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
            this.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;
            this.TextColor = Colors.VERY_DARK_GREY_COLOR;

            if (isCentered)
            {
                this.TextAlignment = UITextAlignment.Center;
            }
            else
            {
                this.TextAlignment = UITextAlignment.Left;
                leftAdjustment = Constants.DRUG_LOOKUP_SIDE_PADDING;
            }

            this.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
            this.VerticalAlignment = UIControlContentVerticalAlignment.Center;
            this.Font = UIFont.FromName(Constants.NUNITO_REGULAR, (nfloat)Constants.TEXT_FIELD_FONT_SIZE);

            this.AutocorrectionType = UITextAutocorrectionType.No;
            this.AutocapitalizationType = UITextAutocapitalizationType.None;

            this.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };
        }
    }
}