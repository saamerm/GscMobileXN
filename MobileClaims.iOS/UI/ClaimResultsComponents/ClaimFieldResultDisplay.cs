using System;
using UIKit;
using CoreGraphics;

namespace MobileClaims.iOS
{
    public class ClaimFieldResultDisplay : UIView
    {
        public UILabel titleLabel;
        public UILabel fieldLabel;

        public ClaimFieldResultDisplay(string title, string fieldText = null)
        {
            titlePadding = 10;
            titleLabel = new UILabel();
            titleLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.ClaimSummaryInfoLabelFontSize);
            titleLabel.TextColor = Colors.LightGrayColorForLabelValuePair;
            titleLabel.TextAlignment = UITextAlignment.Left;
            titleLabel.BackgroundColor = Colors.Clear;
            titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            titleLabel.Lines = 0;
            titleLabel.Text = title;
            AddSubview(titleLabel);

            fieldLabel = new UILabel();
            fieldLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.ClaimSummaryInfoValueFontSize);
            fieldLabel.TextColor = Colors.Black;
            fieldLabel.BackgroundColor = Colors.Clear;
            fieldLabel.TextAlignment = UITextAlignment.Left;
            fieldLabel.LineBreakMode = UILineBreakMode.WordWrap;
            fieldLabel.Lines = 0;
            if (fieldText != null)
            {
                fieldLabel.Text = fieldText;
            }

            AddSubview(fieldLabel);
        }

        public ClaimFieldResultDisplay(string title, string fieldText = null, bool bold = false)
        {
            titlePadding = 2;
            titleLabel = new UILabel();
            if (bold)
            {
                titleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.SMALL_FONT_SIZE);
                titleLabel.TextColor = Colors.Black;
            }
            else
            {
                titleLabel.Font = UIFont.FromName(Constants.NUNITO_BLACK, Constants.SMALL_FONT_SIZE);
                titleLabel.TextColor = Colors.DARK_GREY_COLOR;
            }

            titleLabel.TextAlignment = UITextAlignment.Left;
            titleLabel.BackgroundColor = Colors.Clear;
            titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            titleLabel.Lines = 0;
            titleLabel.Text = title;
            AddSubview(titleLabel);

            fieldLabel = new UILabel();
            if (bold)
            {
                fieldLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.SMALL_FONT_SIZE);
                fieldLabel.TextColor = Colors.Black;
            }
            else
            {
                fieldLabel.Font = UIFont.FromName(Constants.NUNITO_BLACK, Constants.SMALL_FONT_SIZE);
                fieldLabel.TextColor = Colors.DARK_GREY_COLOR;
            }
            fieldLabel.TextAlignment = UITextAlignment.Left;
            fieldLabel.LineBreakMode = UILineBreakMode.WordWrap;
            fieldLabel.Lines = 0;
            fieldLabel.BackgroundColor = Colors.Clear;
            if (fieldText != null)
                fieldLabel.Text = fieldText;
            AddSubview(fieldLabel);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
            float contentWidth = (float)this.Frame.Width;
            float innerPadding = 10;

            var titleLabelWidth = Constants.IsPhone() ? contentWidth * 0.55f : contentWidth * 0.45f;
            var fieldLabelWidth = Constants.IsPhone() ? contentWidth * 0.45f : contentWidth * 0.55f;

            titleLabel.Frame = new CGRect(sidePadding, titlePadding, titleLabelWidth - sidePadding, (float)titleLabel.Frame.Height);
            titleLabel.SizeToFit();

            _componentHeight = titlePadding * 2 + (float)titleLabel.Frame.Height;

            fieldLabel.Frame = new CGRect(titleLabelWidth + innerPadding, titlePadding, fieldLabelWidth - innerPadding- sidePadding, (float)fieldLabel.Frame.Height);
            fieldLabel.SizeToFit();
            fieldLabel.Frame = new CGRect(titleLabelWidth + innerPadding, titlePadding, fieldLabelWidth - innerPadding - sidePadding, (float)fieldLabel.Frame.Height);
        }

        float titlePadding;
        float _componentHeight;
        public float ComponentHeight
        {
            get
            {
                float contentWidth = (float)(((CGRect)this.Frame).Width);
                float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
                float innerPadding = 10;

                var titleLabelWidth = Constants.IsPhone() ? contentWidth * 0.55f : contentWidth * 0.45f;
                var fieldLabelWidth = Constants.IsPhone() ? contentWidth * 0.45f : contentWidth * 0.55f;

                titleLabel.Frame = new CGRect(sidePadding, titlePadding, titleLabelWidth - sidePadding, (float)titleLabel.Frame.Height);

                titleLabel.SizeToFit();
                titleLabel.Frame = new CGRect(sidePadding, titlePadding, titleLabelWidth - sidePadding, (float)titleLabel.Frame.Height);

                fieldLabel.Frame = new CGRect(titleLabelWidth + innerPadding,
                    titlePadding,
                    fieldLabelWidth - innerPadding - sidePadding,
                    (float)fieldLabel.Frame.Height);

                fieldLabel.SizeToFit();
                fieldLabel.Frame = new CGRect(titleLabelWidth + innerPadding,
                    titlePadding,
                    fieldLabelWidth - innerPadding - sidePadding,
                    (float)fieldLabel.Frame.Height);

                return (float)(titlePadding * 2 + Math.Max(titleLabel.Frame.Height, fieldLabel.Frame.Height));
            }
            set
            {
                _componentHeight = value;
            }
        }
    }
}

