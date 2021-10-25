using System;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("ClaimSubmissionLensCell")]
	public class ClaimSubmissionLensCell : SingleSelectionTableViewCell
	{
		public ClaimSubmissionLensCell () : base () {}
		public ClaimSubmissionLensCell (IntPtr handle) : base (handle) {}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<ClaimSubmissionLensCell, LensType >();
					set.Bind(this.label).To(item => item.Name).WithConversion("StringCase").OneWay();
					set.Apply();
				});
		}
	}

}

