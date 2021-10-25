using System;
using UIKit;
using Foundation;
using System.ComponentModel;

namespace MobileClaims.iOS
{
    [Register("GreyNavButton")]
    [DesignTimeVisible(true)]
    public class GreyNavButton : UIButton
    {
        public GreyNavButton(IntPtr handler)
         : base(handler)
        {
        }

        public GreyNavButton()
        {
            Initialize();
        }

        private void Initialize()
        {
            BackgroundColor = Colors.LightGrayColor;
            HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
            SetTitleColor(Colors.DARK_GREY_COLOR, UIControlState.Normal);

            TintColor = Colors.Clear;
            TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            TitleLabel.TextAlignment = UITextAlignment.Center;
            TitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.GS_BUTTON_FONT_SIZE);

            SetTitleColor(Colors.DARK_GREY_COLOR, UIControlState.Normal);
            SetTitleColor(Colors.HIGHLIGHT_COLOR, UIControlState.Selected);
            SetTitleColor(Colors.HIGHLIGHT_COLOR, UIControlState.Highlighted);
            SetTitleColor(Colors.DARK_RED, UIControlState.Focused);
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            Initialize();
        }

        //public override void TouchesBegan(NSSet touches, UIEvent evt)
        //{
        //    base.TouchesBegan(touches, evt);
        //    setHighlight(true);
        //}

        //public override void TouchesEnded(NSSet touches, UIEvent evt)
        //{
        //    base.TouchesEnded(touches, evt);
        //    setHighlight(Selected);
        //}

        //public override void TouchesCancelled(NSSet touches, UIEvent evt)
        //{
        //    base.TouchesCancelled(touches, evt);
        //    setHighlight(Selected);
        //}

        //public override bool Selected
        //{
        //    get
        //    {
        //        return base.Selected;
        //    }
        //    set
        //    {
        //        base.Selected = value;
        //        setHighlight(value);
        //    }
        //}

        //protected void setHighlight(bool highlighted)
        //{
        //    if (highlighted)
        //    {
        //        SetTitleColor(Colors.HIGHLIGHT_COLOR, UIControlState.Normal);
        //    }
        //    else
        //    {
        //        SetTitleColor(Colors.DARK_GREY_COLOR, UIControlState.Normal);
        //    }
        //}

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
        }
    }
}

