using System;
using CoreGraphics;
using Foundation;
using UIKit;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace MobileClaims.iOS
{
    public partial class ProviderCellTemplate : MvxTableViewCell
    {
        public static readonly UINib Nib = UINib.FromName("ProviderCellTemplate", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("ProviderCellTemplate");

        public ProviderCellTemplate(IntPtr handle) : base(handle)
        {
			this.Accessory = UITableViewCellAccessory.DisclosureIndicator;

			UIView backView = new UIView ();

			backView.Frame = (CGRect)this.Frame;
			backView.BackgroundColor = Colors.HIGHLIGHT_COLOR;
			this.SelectedBackgroundView = backView;

            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ProviderCellTemplate,ServiceProviderType>();
                if(lblProvider==null) lblProvider = new UILabel(new CGRect(20,0,(float)base.Bounds.Width,(float)base.Bounds.Height));
				this.lblProvider.HighlightedTextColor = Colors.BACKGROUND_COLOR;
				set.Bind(lblProvider).To(item => item.Type).TwoWay();
                set.Apply();
            });
        }

        public static ProviderCellTemplate Create()
        {
            return (ProviderCellTemplate)Nib.Instantiate(null, null)[0];
        }
    }
}

