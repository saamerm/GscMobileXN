using System;
using Foundation;
using UIKit;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace MobileClaims.iOS
{
    public partial class DetailedProviderCellTemplate : MvxTableViewCell
    {
        public static readonly UINib Nib = UINib.FromName("DetailedProviderCellTemplate", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("DetailedProviderCellTemplate");

        public DetailedProviderCellTemplate(IntPtr handle) : base(handle)
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<DetailedProviderCellTemplate,ServiceProvider>();
                set.Bind(lblProviderName).To(item => item.BusinessName).TwoWay();
                set.Bind(lblStreetAddress).To(item => item.City).TwoWay();
                set.Bind(lblCityProvincePostCode).To(item => item.FormattedAddress).TwoWay();
                set.Apply();
            });
        }

        public static DetailedProviderCellTemplate Create()
        {
            return (DetailedProviderCellTemplate)Nib.Instantiate(null, null)[0];
        }
    }
}

