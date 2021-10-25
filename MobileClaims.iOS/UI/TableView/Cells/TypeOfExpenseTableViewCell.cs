using System;
using MobileClaims.Core.Entities.HCSA;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("TypeOfExpenseTableViewCell")]

	public class TypeOfExpenseTableViewCell : SingleSelectionTableViewCell
	{
		public TypeOfExpenseTableViewCell (): base () {}
		public TypeOfExpenseTableViewCell (IntPtr handle) : base (handle) {}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<TypeOfExpenseTableViewCell, ExpenseType>();
					set.Bind(this.label).To(item => item.Name).WithConversion("StringCase").OneWay();
					set.Apply();
				});
		}
	}


}
