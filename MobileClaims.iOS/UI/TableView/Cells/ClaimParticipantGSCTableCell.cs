using System;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("ClaimParticipantGSCTableCell")]
	public class ClaimParticipantGSCTableCell : SingleSelectionTableViewCell
	{
		public ClaimParticipantGSCTableCell () : base () {}
		public ClaimParticipantGSCTableCell (IntPtr handle) : base (handle) {}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<ClaimParticipantGSCTableCell, ParticipantGSC>();
					set.Bind(this.label).To(item => item.FullNameWithID).OneWay();
					set.Apply();
				});
		}
	}
}

