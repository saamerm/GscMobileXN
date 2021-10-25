using System;
using UIKit;
using Foundation;

namespace MobileClaims.iOS
{
    public class LabelValue: UILabel
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

        public LabelValue()
        {  
//            if (string.IsNullOrEmpty(text))
//                TextContent = "abcdefghijklmn\n123456789\nABCDEFGHIJKLMNOPQRSTUVWXYZ";
            NSMutableParagraphStyle paragraStyle = new NSMutableParagraphStyle();
            paragraStyle.FirstLineHeadIndent =0f;
            paragraStyle.HeadIndent = 10f;
            paragraStyle.TailIndent = -10f;
            paragraStyle.LineSpacing = 10f;
            paragraStyle.Alignment = UITextAlignment.Left;
           // paragraStyle.ParagraphSpacingBefore = 10f;
            UIFont font = UIFont.FromName (Constants.AVENIR_STD_HEAVY,12f);
            if (font == null) {
                font = UIFont.SystemFontOfSize (12f);
            }

            stringAttrubutes = new UIStringAttributes();
            stringAttrubutes.Font = font;
            stringAttrubutes.ForegroundColor =UIColor.Black;
            stringAttrubutes.BackgroundColor = UIColor.Yellow;
            stringAttrubutes.ParagraphStyle = paragraStyle;
            stringAttrubutes.BaselineOffset = -5f; 

            attribute = new NSAttributedString (TextContent,stringAttrubutes); 
            this.AttributedText = attribute;
            this.LineBreakMode = UILineBreakMode.WordWrap;
            this.Lines = 0;
            this.BackgroundColor = UIColor.Yellow;  
        } 
    }
}

