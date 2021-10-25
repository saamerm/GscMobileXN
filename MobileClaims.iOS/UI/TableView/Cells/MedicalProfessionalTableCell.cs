using System;
using MobileClaims.Core.Entities.HCSA;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("MedicalProfessionalTableCell")]
	public class MedicalProfessionalTableCell : SingleSelectionTableViewCell
	{
		public MedicalProfessionalTableCell () : base () {}
		public MedicalProfessionalTableCell (IntPtr handle) : base (handle) {}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<MedicalProfessionalTableCell, HCSAReferralType  >();
					set.Bind(this.label).To(item => item.Text).WithConversion("StringCase").OneWay();
					set.Apply();
				});
		}
	}
}

