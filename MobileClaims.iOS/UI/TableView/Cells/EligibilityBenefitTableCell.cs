using System;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("EligibilityBenefitTableCell")]
	public class EligibilityBenefitTableCell : SingleSelectionTableViewCell
	{
		public EligibilityBenefitTableCell () : base () {}
		public EligibilityBenefitTableCell (IntPtr handle) : base (handle) {}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<EligibilityBenefitTableCell, EligibilityBenefit>();
					set.Bind(this.label).To(item => item.Name).OneWay();
					set.Apply();
				});
		}
	}
}

