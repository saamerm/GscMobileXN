using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;


using MobileClaims.Core.Entities;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace MobileClaims.Droid.Helpers
{
    public class AttachedFilesSubmitAdapter : RecyclerView.Adapter
    {
        private readonly IList<DocumentInfo> _attachments;

        public AttachedFilesSubmitAdapter(IList<DocumentInfo> attachments)
        {
            _attachments = attachments;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (!(holder is AttachedFilesSubmitHolder viewHolder))
            {
                return;
            }
            var currentActivity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            var file = _attachments[position];

            viewHolder.FileType.SetImageDrawable(file.Type == DocumentType.Image
                ? currentActivity.GetDrawable(Resource.Drawable.image_thumbnail)
                : currentActivity.GetDrawable(Resource.Drawable.file_thumbnail));

            viewHolder.FileName.Text = file.Name;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);
            var view = inflater.Inflate(Resource.Layout.attachment_submit_item_layout, parent, false);

            return new AttachedFilesSubmitHolder(view);
        }

        public override int ItemCount => _attachments.Count;
    }
}