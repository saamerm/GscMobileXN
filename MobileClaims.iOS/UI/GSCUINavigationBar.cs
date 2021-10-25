using CoreGraphics;
using Foundation;
using MobileClaims.iOS.Extensions;
using System;
using System.ComponentModel;
using UIKit;

namespace MobileClaims.iOS.UI
{
    [Register("ExtendedNavigationBar")]
    [DesignTimeVisible(true)]
    public class ExtendedNavigationBar : UIView
    {
        public ExtendedNavigationBar(IntPtr handler)
         : base(handler)
        { }

        public ExtendedNavigationBar()
        {
           
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            this.Layer.ShadowOffset = new CGSize(0, 1 / UIScreen.MainScreen.Scale);
            this.Layer.ShadowRadius = 0;

            this.Layer.ShadowColor = Colors.Black.CGColor;
            this.Layer.ShadowOpacity = 0.25f;
        }
    }

    //[Register("GscCustomNavigationBar")]
    //[DesignTimeVisible(true)]
    //public class GscCustomNavigationBar : UINavigationBar
    //{
    //    private UILabel _label1 = new UILabel();
    //    private UILabel _label2 = new UILabel();
    //    private UIStackView titleView = new UIStackView();

    //    public GscCustomNavigationBar(IntPtr handler)
    //        : base(handler)
    //    { }

    //    public GscCustomNavigationBar()
    //    {
    //        Initialize();
    //    }

    //    public override void AwakeFromNib()
    //    {
    //        base.AwakeFromNib();
    //        Initialize();
    //    }

    //    public override void LayoutSubviews()
    //    {
    //        base.LayoutSubviews();
    //        //Initialize();
    //        var viewWidth = Frame.Width;
    //        titleView.Frame = new CoreGraphics.CGRect(0, 0, viewWidth, _label1.Frame.Height + _label2.Frame.Height + 5);

    //    }

    //    public override CGSize SizeThatFits(CGSize size)
    //    {
    //        return base.SizeThatFits(size);
    //    }

    //    private void Initialize()
    //    {
    //        _label1.SetLabel(Constants.LEAGUE_GOTHIC, 18, Colors.HIGHLIGHT_COLOR);

    //        _label1.Text = "DIRECT DEPOSIT";
    //        _label1.SizeToFit();

    //        _label2 = new UILabel(new CGRect(0, 0, 0, 0))
    //        {
    //            BackgroundColor = UIColor.Clear,
    //            TextAlignment = UITextAlignment.Center,
    //            Lines = 0,
    //            LineBreakMode = UILineBreakMode.WordWrap,
    //            TextColor = UIColor.Black,
    //            Font = UIFont.FromName(Constants.NUNITO_REGULAR, 12)
    //        };

    //        var _label2Attributes = new UIStringAttributes
    //        {
    //            ForegroundColor = Colors.Black,
    //            Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.AuditListNotesFontSize),
    //            ParagraphStyle = new NSMutableParagraphStyle()
    //            {
    //                LineSpacing = Constants.AuditListNotesLineSpacing,
    //                LineHeightMultiple = Constants.AuditListNotesLineSpacing,
    //                Alignment = UITextAlignment.Center
    //            }
    //        };

    //        var subTitleText = "Get paid faster…\nSign up for Direct Deposit";
    //        var mutableAttributeString = new NSMutableAttributedString(subTitleText);

    //        mutableAttributeString.SetAttributes(_label2Attributes, new NSRange(0, subTitleText.Length));
    //        _label2.AttributedText = mutableAttributeString;
    //        _label2.SizeToFit();

    //        titleView = new UIStackView
    //        {
    //            Axis = UILayoutConstraintAxis.Vertical,
    //            Distribution = UIStackViewDistribution.Fill,
    //            BackgroundColor = Colors.Clear,
    //            Spacing = 5,
    //            Alignment = UIStackViewAlignment.Center,
    //        };

    //        var viewWidth = Frame.Width;
    //        titleView.AddArrangedSubview(_label1);
    //        titleView.AddArrangedSubview(_label2);
    //        titleView.Frame = new CoreGraphics.CGRect(0, 0, viewWidth, _label1.Frame.Height + _label2.Frame.Height + 5);
    //        //var customNavigationbarView = new UINavigationBar(new CGRect(0, 0, Frame.Size.Width, _label1.Frame.Height + _label2.Frame.Height + 5 + 10));
    //        //customNavigationbarView.BackgroundColor = Colors.Clear;

    //        //customNavigationbarView.AddSubview(titleView);
    //        //AddSubview(customNavigationbarView);

    //        BackgroundColor = Colors.Clear;
    //        AddSubview(titleView);
    //    }
    //}

    [Register("GSCUINavigationBar")]
    [DesignTimeVisible(true)]
    public class GSCUINavigationBar : UINavigationBar
    {
        private readonly UIFont _sizedFont = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.GS_BUTTON_FONT_SIZE);

        [Export("HideBackButtons")]
        [Browsable(true)]
        public bool HideBackButtons { get; set; }

        [Export("AnimateBackButtons")]
        [Browsable(true)]
        public bool AnimateBackButtons { get; set; }

        public GSCUINavigationBar(IntPtr handler)
            : base(handler)
        { }

        public GSCUINavigationBar()
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
            SetTitleFontAttributes();

            Frame = new CoreGraphics.CGRect(Frame.X, Frame.Y, Frame.Size.Width, Constants.NAV_HEIGHT);

            var backButtonItem = CreateBackButton();

            foreach (var item in this.Items)
            {
                item.Title = item.Title.tr();
                item.SetHidesBackButton(this.HideBackButtons, this.AnimateBackButtons);
                item.BackBarButtonItem = backButtonItem;
            }
        }

        private UIBarButtonItem CreateBackButton()
        {
            var backButtonImage = UIImage.FromBundle("ArrowBack");
           
            backButtonImage = backButtonImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            var backButton = new UIButton(UIButtonType.Custom)
            {
                HorizontalAlignment = UIControlContentHorizontalAlignment.Left,
                TitleEdgeInsets = new UIEdgeInsets(11.5f, 15f, 10f, 0f),
                ImageEdgeInsets = new UIEdgeInsets(1f, 8f, 0f, 0f)
            };

            backButton.SetTitle("asd", UIControlState.Normal);
            backButton.SetTitleColor(Colors.HIGHLIGHT_COLOR, UIControlState.Normal);
            backButton.SetTitleColor(Colors.LightGrayColor, UIControlState.Highlighted);
            backButton.SetImage(backButtonImage, UIControlState.Normal);
            backButton.SizeToFit();

            backButton.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width / 4, Constants.NAV_HEIGHT);
            var btnContainer = new UIView(new CGRect(0, 0, backButton.Frame.Width, backButton.Frame.Height));
            btnContainer.AddSubview(backButton);

            return new UIBarButtonItem(" ", UIBarButtonItemStyle.Plain, null)
            {
                CustomView = backButton
            };
        }

        private void SetTitleFontAttributes()
        {
            UIStringAttributes attributes = new UIStringAttributes
            {
                ForegroundColor = Colors.HIGHLIGHT_COLOR,
                Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.NAV_BAR_FONT_SIZE)        
            };            
            this.TitleTextAttributes = attributes;
        }
    }
}