using System;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("EligibilityOptionTableCell")]
	public class EligibilityOptionTableCell : SingleSelectionTableViewCell
	{
		public EligibilityOptionTableCell () : base () {}
		public EligibilityOptionTableCell (IntPtr handle) : base (handle) {}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<EligibilityOptionTableCell, EligibilityOption>();
					set.Bind(this.label).To(item => item.Name).OneWay();
					set.Apply();
				});
		}
	}
}

