using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;


using MobileClaims.Core.Entities;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace MobileClaims.Droid.Helpers
{
    public class AttachedFilesAdapter : RecyclerView.Adapter
    {
        private readonly IList<DocumentInfo> _attachments;
        private readonly Action<int> _deleteDocumentAction;

        public AttachedFilesAdapter(
            IList<DocumentInfo> attachments, 
            Action<int> deleteDocumentAction
        )
        {
            _attachments = attachments;
            _deleteDocumentAction = deleteDocumentAction;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (!(holder is AttachedFilesHolder viewHolder))
            {
                return;
            }
            var currentActivity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            var file = _attachments[position];

            viewHolder.FileType.SetImageDrawable(file.Type == DocumentType.Image
                ? currentActivity.GetDrawable(Resource.Drawable.image_thumbnail)
                : currentActivity.GetDrawable(Resource.Drawable.file_thumbnail));

            viewHolder.FileName.Text = file.Name;

            viewHolder.RemoveIcon.Tag = position.ToString();
            viewHolder.RemoveIcon.Click -= OnRemoveIconOnClick;
            viewHolder.RemoveIcon.Click += OnRemoveIconOnClick;
        }

        private void OnRemoveIconOnClick(object sender, EventArgs e)
        {
            if (sender is ImageView imageView)
            {
                var p = (string)imageView.Tag;
                if (int.TryParse(p, out var pos))
                {
                    _deleteDocumentAction.Invoke(pos);
                    NotifyDataSetChanged();
                }
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);
            var view = inflater.Inflate(Resource.Layout.attachment_item_layout, parent, false);

            return new AttachedFilesHolder(view);
        }

        public override int ItemCount => _attachments.Count;
    }
}