using Foundation;
using System;
using System.ComponentModel;
using UIKit;

namespace MobileClaims.iOS.UI
{
    [Register("GSCPageHeader")]
    [DesignTimeVisible(true)]
    public class GSCPageHeader : UILabel
    {
        public GSCPageHeader(IntPtr handler) : base(handler) { }

        public GSCPageHeader()
        {
            Initialize();
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
            LineBreakMode = UILineBreakMode.WordWrap;
            Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            Text = Text.tr();
        }
    }
}