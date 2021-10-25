using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace MobileClaims.Droid.Helpers
{
    public class ActiveClaimsViewHolder : RecyclerView.ViewHolder
    {
        public LinearLayout BorderLinearLayout { get; set; }
        public TextView ActionRequiredTextView { get; set; }
        public TextView ClaimDateTextView { get; set; }
        public TextView ClaimFormNumberTextView { get; set; }
        public TextView ClaimTypeTextView { get; set; }
        public TextView ClaimSumTextView { get; set; }

        public ActiveClaimsViewHolder(View itemView) : base(itemView)
        {
            BorderLinearLayout = itemView.FindViewById<LinearLayout>(Resource.Id.borderLinearLayout);
            ActionRequiredTextView = itemView.FindViewById<TextView>(Resource.Id.actionRequriedTextView);
            ClaimDateTextView = itemView.FindViewById<TextView>(Resource.Id.claimDateTextView);
            ClaimFormNumberTextView = itemView.FindViewById<TextView>(Resource.Id.claimFormNumberTextView);
            ClaimTypeTextView = itemView.FindViewById<TextView>(Resource.Id.claimTypeTextView);
            ClaimSumTextView = itemView.FindViewById<TextView>(Resource.Id.claimSumTextView);
        }
    }
}