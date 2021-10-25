using Android.Support.V7.Widget;
using Android.Views;
using MobileClaims.Core.Models;
using System.Collections.Generic;

namespace MobileClaims.Droid.Helpers
{
    public class AuditClaimSummaryAdapter : RecyclerView.Adapter
    {
        private readonly IList<ClaimFormClaimSummary> _auditClaims;

        public AuditClaimSummaryAdapter(IList<ClaimFormClaimSummary> auditClaims)
        {
            _auditClaims = auditClaims;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            holder.IsRecyclable = false;
            var currentClaim = _auditClaims[position];
            if (!(holder is AuditClaimSummaryViewHolder auditClaimSummaryHolder))
            {
                return;
            }

            auditClaimSummaryHolder.AuditClaimNumberXofY.Text = currentClaim.CountValue;
            auditClaimSummaryHolder.AuditClaimServiceData.Text = currentClaim.ServiceDateValue;
            auditClaimSummaryHolder.AuditClaimServiceDescription.Text = currentClaim.ServiceDescriptionValue;
            auditClaimSummaryHolder.AuditClaimClaimedAmount.Text = currentClaim.ClaimedAmountValue;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);
            var view = inflater.Inflate(Resource.Layout.item_audit_claim_summary, parent, false);

            return new AuditClaimSummaryViewHolder(view);
        }

        public override int ItemCount => _auditClaims.Count;
    }
}