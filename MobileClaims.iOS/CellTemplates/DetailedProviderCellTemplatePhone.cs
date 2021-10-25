using System;
using Foundation;
using UIKit;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace MobileClaims.iOS
{
    public partial class DetailedProviderCellTemplatePhone : MvxTableViewCell
    {
        public static readonly UINib Nib = UINib.FromName("DetailedProviderCellTemplatePhone", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("DetailedProviderCellTemplatePhone");

        public DetailedProviderCellTemplatePhone(IntPtr handle) : base(handle)
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<DetailedProviderCellTemplatePhone,ServiceProvider>();
                set.Bind(lblProviderName).To(item => item.BusinessName).TwoWay();
                set.Bind(lblStreetAddress).To(item => item.City).TwoWay();
                set.Bind(lblCityProvincePostCode).To(item => item.FormattedAddress).TwoWay();
                set.Apply();
            });
        }

        public static DetailedProviderCellTemplatePhone Create()
        {
            return (DetailedProviderCellTemplatePhone)Nib.Instantiate(null, null)[0];
        }
    }
}

