using System;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("EligibilityProvinceTableCell")]
	public class EligibilityProvinceTableCell : SingleSelectionTableViewCell
	{
		public EligibilityProvinceTableCell () : base () {}
		public EligibilityProvinceTableCell (IntPtr handle) : base (handle) {}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<EligibilityProvinceTableCell, EligibilityProvince>();
					set.Bind(this.label).To(item => item.Name).OneWay();
					set.Apply();
				});
		}
	}
}

