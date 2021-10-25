using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace MobileClaims.Droid.Helpers
{
    public class DashboardRecentClaimsViewHolder : RecyclerView.ViewHolder
    {
        public TextView Description { get; set; }
        public TextView TotalAmountClaimed { get; set; }
        public TextView ServiceDate { get; set; }

        public DashboardRecentClaimsViewHolder(View itemView) : base(itemView)
        {
            Description = itemView.FindViewById<TextView>(Resource.Id.dashboard_description);
            ServiceDate = itemView.FindViewById<TextView>(Resource.Id.dashboard_servicedate);
            TotalAmountClaimed = itemView.FindViewById<TextView>(Resource.Id.dashboard_totalamount);

            var nunitoBookFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/NunitoSansRegular.ttf");
            var nunitoHeavyFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/NunitoSansBold.ttf");

            Description.SetTypeface(nunitoHeavyFont, TypefaceStyle.Bold);
            ServiceDate.SetTypeface(nunitoBookFont, TypefaceStyle.Normal);
            TotalAmountClaimed.SetTypeface(nunitoBookFont, TypefaceStyle.Normal);
        }
    }
}