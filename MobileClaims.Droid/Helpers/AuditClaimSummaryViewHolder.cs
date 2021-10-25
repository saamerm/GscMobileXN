using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace MobileClaims.Droid.Helpers
{
    public class AuditClaimSummaryViewHolder : RecyclerView.ViewHolder
    {
        public TextView AuditClaimNumberXofY { get; set; }
        public TextView AuditClaimServiceData { get; set; }
        public TextView AuditClaimServiceDescription { get; set; }
        public TextView AuditClaimClaimedAmount { get; set; }

        public AuditClaimSummaryViewHolder(View itemView) : base(itemView)
        {
            AuditClaimNumberXofY = itemView.FindViewById<TextView>(Resource.Id.auditClaimNumberXof);
            AuditClaimServiceData = itemView.FindViewById<TextView>(Resource.Id.auditClaimServiceData);
            AuditClaimServiceDescription = itemView.FindViewById<TextView>(Resource.Id.auditClaimServiceDescription);
            AuditClaimClaimedAmount = itemView.FindViewById<TextView>(Resource.Id.auditClaimClaimedAmount);
        }
    }
}