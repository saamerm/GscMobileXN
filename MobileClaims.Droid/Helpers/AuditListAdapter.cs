using Android.Support.V7.Widget;
using Android.Views;
using MobileClaims.Core.ViewModels;
using System;

namespace MobileClaims.Droid.Helpers
{
    public class AuditListAdapter : RecyclerView.Adapter
    {
        private readonly AuditListViewModel _auditListViewModel;

        public AuditListAdapter(AuditListViewModel auditListViewModel)
        {
            _auditListViewModel = auditListViewModel;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (!(holder is AuditListViewHolder viewHolder))
            {
                return;
            }

            var auditClaim = _auditListViewModel.AuditClaims[position];

            viewHolder.ClaimFormId.Text = auditClaim.ClaimFormId;
            viewHolder.SubmissionDateLabel.Text = _auditListViewModel.ClaimSubmissionDateLabel;
            viewHolder.SubmissionDate.Text = auditClaim.ClaimSubmissionDate;
            viewHolder.DueDateLabel.Text = _auditListViewModel.ClaimDueDateLabel;
            viewHolder.DueDate.Text = auditClaim.ClaimDueDate;
            viewHolder.Description.Text = auditClaim.ServiceDescription;

            viewHolder.ItemView.Tag = position;
            viewHolder.ItemView.Click -= OnAuditClaimClick;
            viewHolder.ItemView.Click += OnAuditClaimClick;
        }

        private void OnAuditClaimClick(object sender, EventArgs e)
        {
            if (sender is View view)
            {
                if(int.TryParse((string)view.Tag, out var position))
                {
                    _auditListViewModel.AuditSelectedCommand.Execute(_auditListViewModel.AuditClaims[position]);
                }
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);
            var view = inflater.Inflate(Resource.Layout.item_audit_claim, parent, false);

            return new AuditListViewHolder(view);
        }

        public override int ItemCount => _auditListViewModel.AuditClaims.Count;
    }
}