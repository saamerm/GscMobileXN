using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace MobileClaims.Droid.Helpers
{
    public class AttachedFilesSubmitHolder : RecyclerView.ViewHolder
    {
        public ImageView FileType { get; set; }
        public TextView FileName { get; set; }

        public AttachedFilesSubmitHolder(View itemView) : base(itemView)
        {
            FileType = itemView.FindViewById<ImageView>(Resource.Id.fileTypeSubmitImageView);
            FileName = itemView.FindViewById<TextView>(Resource.Id.fileNameSubmitTextView);
        }
    }
}