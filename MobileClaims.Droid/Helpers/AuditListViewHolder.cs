using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace MobileClaims.Droid.Helpers
{
    public class AuditListViewHolder : RecyclerView.ViewHolder
    {
        public TextView ClaimFormId { get; set; }
        public TextView SubmissionDateLabel { get; set; }
        public TextView SubmissionDate { get; set; }
        public TextView DueDateLabel { get; set; }
        public TextView DueDate { get; set; }
        public TextView Description { get; set; }

        public AuditListViewHolder(View itemView) : base(itemView)
        {
            ClaimFormId = itemView.FindViewById<TextView>(Resource.Id.auditclaim_claimformid);
            SubmissionDateLabel = itemView.FindViewById<TextView>(Resource.Id.auditclaim_submissiondatelabel);
            SubmissionDate = itemView.FindViewById<TextView>(Resource.Id.auditclaim_submissiondate);
            DueDateLabel = itemView.FindViewById<TextView>(Resource.Id.auditclaim_duedatelabel);
            DueDate = itemView.FindViewById<TextView>(Resource.Id.auditclaim_duedate);
            Description = itemView.FindViewById<TextView>(Resource.Id.auditclaim_description);
        }
    }
}