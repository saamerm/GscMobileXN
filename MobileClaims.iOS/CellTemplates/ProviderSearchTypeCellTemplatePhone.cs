using System;
using CoreGraphics;
using Foundation;
using UIKit;
using MobileClaims.Core;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace MobileClaims.iOS
{
    public partial class ProviderSearchTypeCellTemplatePhone : MvxTableViewCell
    {
        public static readonly UINib Nib = UINib.FromName("ProviderSearchTypeCellTemplatePhone", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("ProviderSearchTypeCellTemplatePhone");

        public ProviderSearchTypeCellTemplatePhone(IntPtr handle) : base(handle)
        {
			UIView backView = new UIView ();

			backView.Frame = (CGRect)this.Frame;
			backView.BackgroundColor = Colors.HIGHLIGHT_COLOR;
			this.SelectedBackgroundView = backView;

            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ProviderSearchTypeCellTemplatePhone,SearchType>();
                if(lblSearchType==null) lblSearchType = new UILabel(new CGRect(20,0,(float)base.Bounds.Width,(float)base.Bounds.Height));
				this.lblSearchType.HighlightedTextColor = Colors.BACKGROUND_COLOR;
				set.Bind(lblSearchType).To(item => item.TypeName).TwoWay();
                set.Apply();
            });
        }

        public static ProviderSearchTypeCellTemplatePhone Create()
        {
            return (ProviderSearchTypeCellTemplatePhone)Nib.Instantiate(null, null)[0];
        }
    }
}

