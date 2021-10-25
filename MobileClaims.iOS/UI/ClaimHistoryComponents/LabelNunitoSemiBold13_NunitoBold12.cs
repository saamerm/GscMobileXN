using UIKit;
using Foundation;

namespace MobileClaims.iOS
{
    public class LabelNunitoSemiBold13_NunitoBold12: UILabel
    {
        private string _textContent=string.Empty ;
        private  NSAttributedString attribute;
        private UIStringAttributes stringAttrubutes;
        public string TextContent
        {
            get{ return _textContent;}
            set
            { 
                _textContent = value;
                attribute = new NSAttributedString (TextContent,stringAttrubutes); 
                this.AttributedText = attribute;
            }
        }

        public LabelNunitoSemiBold13_NunitoBold12()
        {  
            //if (string.IsNullOrEmpty(TextContent))
//                TextContent = "abcdefghijklmn\n123456789\nABCDEFGHIJKLMNOPQRSTUVWXYZ";
           // TextContent = "abcdefghijklmn\n123456789";
            NSMutableParagraphStyle paragraStyle = new NSMutableParagraphStyle();
            paragraStyle.FirstLineHeadIndent =0f;
            paragraStyle.HeadIndent = 0f;
            paragraStyle.TailIndent = 0f;
            paragraStyle.LineSpacing = 5f;
            paragraStyle.Alignment = UITextAlignment.Left;
           // paragraStyle.ParagraphSpacingBefore = 10f;
            UIFont font = UIFont.FromName (Constants.NUNITO_BOLD,12f);
            if (font == null) {
                font = UIFont.SystemFontOfSize (12f);
            }

            stringAttrubutes = new UIStringAttributes();
            stringAttrubutes.Font = font;
            stringAttrubutes.ForegroundColor =Colors.DarkGrayColor;
            stringAttrubutes.BackgroundColor = Colors.Clear;
            stringAttrubutes.ParagraphStyle = paragraStyle;
            //stringAttrubutes.BaselineOffset = -2f; 
             

            attribute = new NSAttributedString (TextContent,stringAttrubutes); 
            this.AttributedText = attribute;
            this.LineBreakMode = UILineBreakMode.WordWrap;
            this.Lines = 0;
            this.BackgroundColor = Colors.Clear;  
        } 
    }
}

