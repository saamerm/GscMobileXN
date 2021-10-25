using System;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("ClaimSubmissionBenefitCell")]
	public class ClaimSubmissionBenefitCell : SingleSelectionTableViewCell
	{
		public ClaimSubmissionBenefitCell () : base () {}
		public ClaimSubmissionBenefitCell (IntPtr handle) : base (handle) {}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<ClaimSubmissionBenefitCell, ClaimSubmissionBenefit >();
					set.Bind(this.label).To(item => item.Name).WithConversion("StringCase").OneWay();
					set.Apply();
				});
		}
	}

}

