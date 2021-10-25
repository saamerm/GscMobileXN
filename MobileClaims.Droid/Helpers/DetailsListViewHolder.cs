using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace MobileClaims.Droid.Helpers
{
    public class DetailsListViewHolder : RecyclerView.ViewHolder
    {
        public TextView Title { get; set; }
        public TextView Name { get; set; }
        public ImageView DirectBilling { get; set; }
        public TextView Details { get; set; }
        public LinearLayout RatingLayout { get; set; }
        public RatingBar Stars{ get; set; }
        public TextView Score { get; set; }
        public TextView NotAvailable { get; set; }
        public ImageView Heart { get; set; }
        public TextView Distance { get; set; }
        public View DetailsListItem { get; set; }

        public DetailsListViewHolder(View itemView) : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.titleDetails);
            Name = itemView.FindViewById<TextView>(Resource.Id.nameDetails);
            DirectBilling = itemView.FindViewById<ImageView>(Resource.Id.direct_billing);
            Details = itemView.FindViewById<TextView>(Resource.Id.detailsDetails);
            RatingLayout = itemView.FindViewById<LinearLayout>(Resource.Id.ratingLayout);
            Stars = itemView.FindViewById<RatingBar>(Resource.Id.starsDetails);
            Score = itemView.FindViewById<TextView>(Resource.Id.scoreDetails);
            NotAvailable = itemView.FindViewById<TextView>(Resource.Id.scoreDetailsAvailability);
            Heart = itemView.FindViewById<ImageView>(Resource.Id.heartDetails);
            Distance = itemView.FindViewById<TextView>(Resource.Id.distanceDetails);
            DetailsListItem = itemView.FindViewById<View>(Resource.Id.detailsListItem);
        }
    }    
}