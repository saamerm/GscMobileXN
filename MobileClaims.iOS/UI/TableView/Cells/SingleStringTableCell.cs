using System;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("SingleStringTableCell")]
	public class SingleStringTableCell : SingleSelectionTableViewCell
	{
		public SingleStringTableCell () : base () {}
		public SingleStringTableCell (IntPtr handle) : base (handle) {}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<SingleStringTableCell, string >();
					set.Bind(this.label).To(item => item).WithConversion("StringCase").OneWay();
					set.Apply();
				});
		}
	}
}

