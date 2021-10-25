using System;
using System.ComponentModel;
using Foundation;
using UIKit;

namespace MobileClaims.iOS.UI
{
    [Register("GSCPageSubHeader")]
    [DesignTimeVisible(true)]
    public class GSCPageSubHeader : UILabel
    {
        public GSCPageSubHeader()
        {
            Initialize();
        }

        public GSCPageSubHeader(IntPtr handler) : base(handler)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            Initialize();
        }

        private void Initialize()
        {
            BackgroundColor = Colors.Clear;
            TextColor = Colors.DARK_GREY_COLOR;
            TextAlignment = UITextAlignment.Left;
            Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, (nfloat)Constants.SUB_HEADING_FONT_SIZE);
            Text = Text.tr();
        }
    }
}