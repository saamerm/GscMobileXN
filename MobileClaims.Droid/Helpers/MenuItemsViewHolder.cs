using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace MobileClaims.Droid.Helpers
{
    public class MenuItemsViewHolder : RecyclerView.ViewHolder
    {
        public ImageView MenuItemIcon { get; set; }
        public TextView MenuItemLabel { get; set; }
        public TextView MenuItemsCountLabel { get; set; }
        
        public MenuItemsViewHolder(View itemView) : base(itemView)
        {
            MenuItemIcon = itemView.FindViewById<ImageView>(Resource.Id.menuIconImageView);
            MenuItemLabel = itemView.FindViewById<TextView>(Resource.Id.menuIconLabelTextView);
            MenuItemsCountLabel = itemView.FindViewById<TextView>(Resource.Id.menuItemsCountLabel);

            var leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/LeagueGothic.ttf");
            MenuItemLabel.SetTypeface(leagueFont, TypefaceStyle.Normal);
            MenuItemsCountLabel.SetTypeface(leagueFont, TypefaceStyle.Normal);
        }
    }
}