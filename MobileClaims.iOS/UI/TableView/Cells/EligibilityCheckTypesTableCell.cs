using System;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{

	[Foundation.Register("EligibilityCheckTypesTableCell")]
	public class EligibilityCheckTypesTableCell : SingleSelectionTableViewCell
	{
		public EligibilityCheckTypesTableCell () : base () {}
		public EligibilityCheckTypesTableCell (IntPtr handle) : base (handle) {}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<EligibilityCheckTypesTableCell,EligibilityCheckType>();
					set.Bind(this.label).To(item => item.Name).WithConversion("StringCase").OneWay();
					set.Apply();
				});
		}
	}
}

