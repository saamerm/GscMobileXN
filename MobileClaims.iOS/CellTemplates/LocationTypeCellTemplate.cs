using System;
using CoreGraphics;
using Foundation;
using UIKit;
using MobileClaims.Core;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace MobileClaims.iOS
{
    public partial class LocationTypeCellTemplate : MvxTableViewCell
    {
        public static readonly UINib Nib = UINib.FromName("LocationTypeCellTemplate", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("LocationTypeCellTemplate");

        public LocationTypeCellTemplate(IntPtr handle) : base(handle)
        {
			UIView backView = new UIView ();

			backView.Frame = (CGRect)this.Frame;
			backView.BackgroundColor = Colors.HIGHLIGHT_COLOR;
			this.SelectedBackgroundView = backView;

            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<LocationTypeCellTemplate,LocationType>();
                if(lblLocationType==null) lblLocationType = new UILabel(new CGRect(20,0,(float)base.Bounds.Width,(float)base.Bounds.Height));
				this.lblLocationType.HighlightedTextColor = Colors.BACKGROUND_COLOR;
				set.Bind(lblLocationType).To(item => item.TypeName).TwoWay();
                set.Apply();
            });
        }

        public static LocationTypeCellTemplate Create()
        {
            return (LocationTypeCellTemplate)Nib.Instantiate(null, null)[0];
        }
    }
}

