using System;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("ClaimOptionTableCell")]
	public class ClaimOptionTableCell : SingleSelectionTableViewCell
	{
		public ClaimOptionTableCell () : base () {}
		public ClaimOptionTableCell (IntPtr handle) : base (handle) {}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<ClaimOptionTableCell, ClaimOption  >();
					set.Bind(this.label).To(item => item.Name).WithConversion("StringCase").OneWay();
					set.Apply();
				});
		}
	}
}

