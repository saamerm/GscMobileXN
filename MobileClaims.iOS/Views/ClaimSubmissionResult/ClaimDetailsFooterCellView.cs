using System.Drawing;
using Foundation;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimSubmissionResult
{
    public class ClaimDetailsFooterCellView : UICollectionReusableView
    {
        private string _footerText;
        private UILabel _label;
        private UIStringAttributes _labelTextAttributes;

        public static readonly NSString Key = new NSString("ClaimDetailsFooterCellView");

        public string FooterText
        {
            get
            {
                return _footerText;
            }
            set
            {
                _footerText = value;
                _label.AttributedText = SetAttributedText(value);
                SetNeedsDisplay();
            }
        }

        [Export("initWithFrame:")]
        public ClaimDetailsFooterCellView(RectangleF frame)
            : base(frame)
        {
            _label = new UILabel()
            {
                Text = string.Empty,
                TextColor = Colors.LightGrayColorForLabelValuePair,
                TranslatesAutoresizingMaskIntoConstraints = false,
                LineBreakMode = UILineBreakMode.WordWrap,
                Lines = 0
            };
            _label.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.ClaimSummaryFooterFontSize);
            AddSubview(_label);
            SetConstraint();
        }

        private void SetConstraint()
        {
            _label.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor).Active = true;
            _label.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor).Active = true;
            _label.TopAnchor.ConstraintEqualTo(this.TopAnchor).Active = true;
            _label.BottomAnchor.ConstraintEqualTo(this.BottomAnchor).Active = true;
        }

        private NSMutableAttributedString SetAttributedText(string value)
        {
            var attributes = new UIStringAttributes
            {
                ForegroundColor = Colors.LightGrayColorForLabelValuePair,
                Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.ClaimSummaryFooterFontSize),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.AuditListInstructionLineSpacing,
                    LineHeightMultiple = Constants.AuditListInstructionLineSpacing
                }
            };

            var attributeString = new NSMutableAttributedString(value);
            attributeString.SetAttributes(attributes, new NSRange(0, value.Length));
            //attributeString.AddAttribute(new NSString("NSParagraphStyleAttributeName"),
            //    paragraphStyle,
            //    new NSRange(0, value.Length));
            return attributeString;
        }
    }
}