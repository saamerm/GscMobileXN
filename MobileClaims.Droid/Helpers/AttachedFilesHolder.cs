using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace MobileClaims.Droid.Helpers
{
    public class AttachedFilesHolder : RecyclerView.ViewHolder
    {
        public ImageView FileType { get; set; }
        public TextView FileName { get; set; }
        public ImageView RemoveIcon { get; set; }

        public AttachedFilesHolder(View itemView) : base(itemView)
        {
            FileType = itemView.FindViewById<ImageView>(Resource.Id.fileTypeImageView);
            FileName = itemView.FindViewById<TextView>(Resource.Id.fileNameTextView);
            RemoveIcon = itemView.FindViewById<ImageView>(Resource.Id.removeIconImageView);
        }
    }
}