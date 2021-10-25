using System;
using System.ComponentModel;
using Foundation;
using UIKit;

namespace MobileClaims.iOS.UI
{
    [Register("GSCSmallFontSizeLabel")]
    [DesignTimeVisible(true)]
    public class GSCSmallFontSizeLabel : UILabel
    {
        public GSCSmallFontSizeLabel()
        {
            Initialize();
        }

        public GSCSmallFontSizeLabel(IntPtr handler) : base(handler)
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
            LineBreakMode = UILineBreakMode.WordWrap;
            Lines = 0;
            Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            Text = Text.tr();
        }
    }
}
