using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MobileClaims.iOS.UI
{
    [Register("VerticalLabel")]
    [DesignTimeVisible(true)]
    public class VerticalLabel : UIView
    {
        private string _labelText;

        public UILabel Label { get; set; }

        public string LabelText
        {
            get => _labelText;
            set
            {
                _labelText = value;
                Label.Text = _labelText;
            }
        }

        public VerticalLabel()
        {
            Initialize();
        }

        public VerticalLabel(IntPtr handler)
            : base(handler)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            Initialize();
        }

        private void Initialize()
        {
            Label = new UILabel();            
            Label.BackgroundColor = Colors.Clear;

            this.AddSubview(Label);
            Label.Frame = this.Bounds;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            Label.Transform = CGAffineTransform.MakeIdentity();

            Label.Frame = new CGRect(CGPoint.Empty,
                new CGSize(this.Bounds.Height, this.Bounds.Width));
            Label.Transform = TransformRotate();

            Label.TopAnchor.ConstraintEqualTo(this.TopAnchor).Active = true;
            Label.BottomAnchor.ConstraintEqualTo(this.BottomAnchor).Active = true;
            Label.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor).Active = true;
            Label.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor).Active = true;
        }

        private CGAffineTransform TransformRotate()
        {
            var radian = (float)Math.PI * 90 / 180;
            var transform = CGAffineTransform.MakeIdentity();
            transform = CGAffineTransform.Translate(transform,
                (this.Bounds.Width / 2) - (this.Bounds.Height / 2),
                (this.Bounds.Height / 2) - (this.Bounds.Width / 2));
            transform = CGAffineTransform.Rotate(transform, radian);
            return transform;
        }
    }
}