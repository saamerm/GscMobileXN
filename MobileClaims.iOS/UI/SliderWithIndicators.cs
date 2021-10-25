using System;
using CoreGraphics;
using System.Collections.Generic;
using UIKit;
using Foundation;
using System.ComponentModel;

namespace MobileClaims.iOS
{
    [Register("SliderWithIndicators")]
    [DesignTimeVisible(true)]
    public class SliderWithIndicators : UISlider
    {
        private string _suffix;
        private int _numberOfIndicators;
        private int _indicatorInset;
        private List<UILabel> _indicators;
        private List<UILabel> _ticks;
        private UIView _indicatorContainer;

        [Export("MinValue")]
        [Browsable(true)]
        public override float MinValue
        {
            get
            {
                return base.MinValue;
            }
            set
            {
                base.MinValue = value;
                Draw((CGRect)this.Frame);
            }
        }

        [Export("MaxValue")]
        [Browsable(true)]
        public override float MaxValue
        {
            get
            {
                return base.MaxValue;
            }
            set
            {
                base.MaxValue = value;
                Draw((CGRect)this.Frame);
            }
        }

        [Export("NumberOfIndicators")]
        [Browsable(true)]
        public int NumberOfIndicators
        {
            get
            {
                return _numberOfIndicators;
            }
            set
            {
                _numberOfIndicators = value;
                Draw((CGRect)this.Frame);
            }
        }

        [Export("Suffix")]
        [Browsable(true)]
        public string Suffix
        {
            get
            {
                return _suffix;
            }
            set
            {
                _suffix = value; Draw((CGRect)this.Frame);
            }
        }

        public int IndicatorInset
        {
            get
            {
                return _indicatorInset;
            }
            set
            {
                _indicatorInset = value;
                Draw((CGRect)this.Frame);
            }
        }

        public SliderWithIndicators(IntPtr handler)
            : base(handler)
        {
        }

        public SliderWithIndicators()
        {
            Initialize();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            Draw((CGRect)this.Frame);
        }

        public override void Draw(CGRect rect)
        {
            if (_indicators != null)
            {
                for (var i = 0; i < _indicators.Count; i++)
                {
                    _indicators[i].RemoveFromSuperview();
                    _ticks[i].RemoveFromSuperview();
                }
            }

            var scale = (MaxValue - MinValue) / (NumberOfIndicators - 1);
            float indWidthBounds = (float)(rect.Width - IndicatorInset * 2);
            float totalWidth = 0;
            float height = 20;
            UILabel tempLabel = new UILabel();
            UILabel tempTick;
            _indicators = new List<UILabel>();
            _ticks = new List<UILabel>();

            for (var i = 0; i < NumberOfIndicators; i++)
            {
                tempLabel = new UILabel
                {
                    Text = Math.Round(MinValue + scale * i).ToString() + Suffix,
                    BackgroundColor = Colors.Clear,
                    Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE),
                    TextAlignment = UITextAlignment.Center,
                    TextColor = Colors.VERY_DARK_GREY_COLOR
                };

                _indicators.Add(tempLabel);
                _indicatorContainer.AddSubview(tempLabel);

                tempTick = new UILabel
                {
                    Text = "|",
                    TextColor = new UIColor(112 / 255f, 112 / 255f, 112 / 255f, 1.0f),
                    BackgroundColor = Colors.Clear,
                    Font = UIFont.SystemFontOfSize((nfloat)Constants.SMALL_FONT_SIZE),
                    TextAlignment = UITextAlignment.Center
                };

                _ticks.Add(tempTick);
                _indicatorContainer.AddSubview(tempTick);


                tempLabel.Frame = new CGRect((float)((indWidthBounds / (_numberOfIndicators - 1)) * i),
                                                     (float)tempTick.Frame.Height + (float)tempTick.Frame.Height + 10,
                                                     50,
                                                     height);
                tempTick.Frame = new CGRect((float)tempLabel.Frame.X,
                                            (float)tempLabel.Frame.Y - (float)tempLabel.Frame.Height,
                                            (float)tempLabel.Frame.Width,
                                            (float)tempLabel.Frame.Height);

                totalWidth = (float)tempLabel.Frame.X + (float)tempLabel.Frame.Width;
            }

            _indicatorContainer.Frame = new CGRect(rect.Width / 2 - totalWidth / 2,
                                                   height * 2 + (height / 2),
                                                   totalWidth,
                                                   (float)tempLabel.Frame.Y + (float)tempLabel.Frame.Height);
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            Initialize();
        }

        private void Initialize()
        {
            _indicatorInset = 5;
            _indicatorContainer = new UIView();
            this.MinimumTrackTintColor = Colors.HIGHLIGHT_COLOR;
            this.AddSubview(_indicatorContainer);
        }
    }
}