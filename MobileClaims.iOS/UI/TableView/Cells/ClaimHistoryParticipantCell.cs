using System;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;
using UIKit; 

namespace MobileClaims.iOS
{
    [Foundation.Register("ClaimHistoryParticipantCell")] 
    public class ClaimHistoryParticipantCell : SingleSelectionTableViewCellNoFont
    {
        public ClaimHistoryParticipantCell (): base () {

            base. label.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC,(nfloat)Constants.HEADING_CLAIMTYPES);
        }
        public ClaimHistoryParticipantCell (IntPtr handle) : base (handle) {

            base.label.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC,(nfloat)Constants.HEADING_CLAIMTYPES);
        }

        public override void InitializeBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ClaimHistoryParticipantCell, Participant>();
                set.Bind(this.label).To(item => item.FullName).WithConversion("StringCase").OneWay();
                set.Apply();
            });
        }
    }

}

