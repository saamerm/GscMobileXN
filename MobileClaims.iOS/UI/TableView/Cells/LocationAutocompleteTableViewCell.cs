using System;

using Foundation;
using UIKit;

namespace MobileClaims.iOS.UI.TableView.Cells
{
    public partial class LocationAutocompleteTableViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("LocationAutocompleteTableViewCell");
        public static readonly UINib Nib;

        public UILabel Label => TitleLabel;

        static LocationAutocompleteTableViewCell()
        {
            Nib = UINib.FromName("LocationAutocompleteTableViewCell", NSBundle.MainBundle);
        }

        protected LocationAutocompleteTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            TitleLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.BUTTON_FONT_SIZE);
            TitleLabel.TextColor = Colors.HIGHLIGHT_COLOR;
            TitleLabel.HighlightedTextColor = Colors.BACKGROUND_COLOR;
        }
    }
}
