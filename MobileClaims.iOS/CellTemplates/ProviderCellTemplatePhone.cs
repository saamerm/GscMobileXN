using System;
using CoreGraphics;
using Foundation;
using UIKit;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace MobileClaims.iOS
{
	public partial class ProviderCellTemplatePhone : MvxTableViewCell
	{
		public static readonly UINib Nib = UINib.FromName("ProviderCellTemplatePhone", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString("ProviderCellTemplatePhone");

		public ProviderCellTemplatePhone(IntPtr handle) : base(handle)
		{
			this.Accessory = UITableViewCellAccessory.DisclosureIndicator;

			UIView backView = new UIView ();

			backView.Frame = (CGRect)this.Frame;
			backView.BackgroundColor = Colors.HIGHLIGHT_COLOR;
			this.SelectedBackgroundView = backView;

			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<ProviderCellTemplatePhone,ServiceProviderType>();
					if(lblProvider==null) lblProvider = new UILabel(new CGRect(20,0,(float)base.Bounds.Width,(float)base.Bounds.Height));
					this.lblProvider.HighlightedTextColor = Colors.BACKGROUND_COLOR;
					set.Bind(lblProvider).To(item => item.Type).TwoWay();
					set.Apply();
				});
		}

		public static ProviderCellTemplatePhone Create()
		{
			return (ProviderCellTemplatePhone)Nib.Instantiate(null, null)[0];
		}
	}
}

