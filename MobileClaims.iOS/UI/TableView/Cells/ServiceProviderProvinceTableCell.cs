using System;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
    [Foundation.Register("ServiceProviderProvinceTableCell")]
    public class ServiceProviderProvinceTableCell: SingleSelectionTableViewCell
    {
        public ServiceProviderProvinceTableCell () : base () {}
        public ServiceProviderProvinceTableCell (IntPtr handle) : base (handle) {}

        public override void InitializeBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ServiceProviderProvinceTableCell, ServiceProviderProvince>();
                set.Bind(this.label).To(item => item.Name).OneWay();
                set.Apply();
            });
        }
    }
}

