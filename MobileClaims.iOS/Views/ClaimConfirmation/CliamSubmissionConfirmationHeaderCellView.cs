using System.Drawing;
using Foundation;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimConfirmation
{
    public class CliamSubmissionConfirmationHeaderCellView : UICollectionReusableView
    {
        private UILabel _label;

        public static readonly NSString Key = new NSString("CliamSubmissionConfirmationHeaderCellView");

        public string HeaderText
        {
            get
            {
                return _label.Text;
            }
            set
            {
                _label.Text = value;
                SetNeedsDisplay();
            }
        }

        [Export("initWithFrame:")]
        public CliamSubmissionConfirmationHeaderCellView(RectangleF frame)
            : base(frame)
        {
            _label = new UILabel()
            {
                Text = "",
                TextColor = Colors.Black,
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            _label.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.ClaimSummaryClaimCounterFontSize);
            AddSubview(_label);

            _label.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor).Active = true;
            _label.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor).Active = true;
            _label.TopAnchor.ConstraintEqualTo(this.TopAnchor).Active = true;
            _label.BottomAnchor.ConstraintEqualTo(this.BottomAnchor).Active = true;
        }
    }
}