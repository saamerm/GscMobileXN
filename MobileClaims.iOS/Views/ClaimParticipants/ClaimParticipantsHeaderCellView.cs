using System;
using Foundation;
using MobileClaims.iOS.Extensions;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimParticipants
{
    public class ClaimParticipantsHeaderCellView : UICollectionReusableView
    {
        private string _headerText;
        private string _secondaryHeader;
        private UILabel _headerLabel;
        private UILabel _subheaderLabel;
        private UIStackView _stackView;

        public static readonly NSString Key = new NSString("ClaimParticipantsHeaderCellView");

        public string HeaderText
        {
            get
            {
                return _headerText;
            }
            set
            {
                _headerText = value;
                _headerLabel.Text = _headerText;
                SetNeedsLayout();
            }
        }

        public string SecondaryHeader
        {
            get
            {
                return _secondaryHeader;
            }
            set
            {
                _secondaryHeader = value;
                _subheaderLabel.Text = _secondaryHeader;
                SetNeedsLayout();
            }
        }

        [Export("initWithFrame:")]
        public ClaimParticipantsHeaderCellView()
            : base()
        {
            _headerLabel.SetFontAndTextColor(Constants.NUNITO_REGULAR, Constants.ClaimSummaryFooterFontSize, Colors.LightGrayColorForLabelValuePair);
            _subheaderLabel.SetFontAndTextColor(Constants.NUNITO_REGULAR, Constants.ClaimSummaryFooterFontSize, Colors.LightGrayColorForLabelValuePair);
            _stackView = new UIStackView()
            {
                Spacing = 20
            };

            _stackView.AddArrangedSubview(_headerLabel);
            _stackView.AddArrangedSubview(_subheaderLabel);
            AddSubview(_stackView);

            SetConstraint();
        }

        private void SetConstraint()
        {
            _stackView.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor).Active = true;
            _stackView.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor).Active = true;
            _stackView.TopAnchor.ConstraintEqualTo(this.TopAnchor).Active = true;
            _stackView.BottomAnchor.ConstraintEqualTo(this.BottomAnchor).Active = true;

            _headerLabel.LeadingAnchor.ConstraintEqualTo(_stackView.LeadingAnchor).Active = true;
            _headerLabel.TrailingAnchor.ConstraintEqualTo(_stackView.TrailingAnchor).Active = true;
            _headerLabel.TopAnchor.ConstraintEqualTo(_stackView.TopAnchor).Active = true;
            _headerLabel.BottomAnchor.ConstraintEqualTo(_subheaderLabel.TopAnchor, 20).Active = true;

            _subheaderLabel.LeadingAnchor.ConstraintEqualTo(_stackView.LeadingAnchor).Active = true;
            _subheaderLabel.TrailingAnchor.ConstraintEqualTo(_stackView.TrailingAnchor).Active = true;
            _subheaderLabel.BottomAnchor.ConstraintEqualTo(_stackView.BottomAnchor).Active = true;
        }
    }
}