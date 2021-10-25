using Foundation;
using System;
using System.ComponentModel;
using UIKit;

namespace MobileClaims.iOS.UI
{
    [Register("GSCWarningLabel")]
    [DesignTimeVisible(true)]
    public class GSCWarningLabel : UILabel
    {
        public GSCWarningLabel(IntPtr handler) : base(handler) { }

        public GSCWarningLabel()
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
            BackgroundColor = UIColor.Clear;
            TextColor = Constants.DARK_GREY_COLOR;
            TextAlignment = UITextAlignment.Center;
            Lines = 0;
            LineBreakMode = UILineBreakMode.WordWrap;
            Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.HEADING_FONT_SIZE);
            Text = Text.tr();
        }
    }
}