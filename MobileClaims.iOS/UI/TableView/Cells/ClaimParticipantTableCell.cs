using System;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("ClaimParticipantTableCell")]
	public class ClaimParticipantTableCell : SingleSelectionTableViewCell
	{
		public ClaimParticipantTableCell () : base () {}
		public ClaimParticipantTableCell (IntPtr handle) : base (handle) {}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<ClaimParticipantTableCell, Participant>();
					set.Bind(this.label).To(item => item.FullName).OneWay();
					set.Apply();
				});
		}
	}
}

