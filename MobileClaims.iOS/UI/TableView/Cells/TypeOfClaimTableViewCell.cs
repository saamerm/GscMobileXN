using System;
using MobileClaims.Core.Entities.HCSA;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("TypeOfClaimTableViewCell")]

	public class TypeOfClaimTableViewCell : SingleSelectionTableViewCell
	{
		public TypeOfClaimTableViewCell (): base () {}
		public TypeOfClaimTableViewCell (IntPtr handle) : base (handle) {}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<TypeOfClaimTableViewCell, ClaimType>();
					set.Bind(this.label).To(item => item.Name).WithConversion("StringCase").OneWay();
					set.Apply();
				});
		}
	}

}

