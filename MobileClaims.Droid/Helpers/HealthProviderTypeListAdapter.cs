using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using FFImageLoading.Cross;
using Java.Lang;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Views;
using static Android.Views.View;

namespace MobileClaims.Droid.Helpers
{
    public class HealthProviderTypeListAdapter : BaseExpandableListAdapter, IMarkElement, IOnClickListener
    {
        private readonly List<HealthProviderTypeViewModel> _healthProviderTypeList;
        private ExpandableListView _listView;
        private readonly Activity _context;
        private bool _isLastChild;
        private bool _isExpanded;

        public HealthProviderTypeListAdapter(ExpandableListView listView, List<HealthProviderTypeViewModel> healthProviderTypeList,
            Activity context)
        {
            _healthProviderTypeList = healthProviderTypeList;
            _context = context;
            _listView = listView;
        }

        public override int GroupCount => _healthProviderTypeList?.Count ?? 0;
        public override bool HasStableIds => true;

        public override long GetGroupId(int groupPosition)
        {
            return _healthProviderTypeList[groupPosition].Id;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            var currentProvider = _healthProviderTypeList[groupPosition];

            var groupView = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.health_provider_type_group_layout, null);

            var providerTypeImage = groupView.FindViewById<MvxCachedImageView>(Resource.Id.providerTypeImage);
            providerTypeImage.ImagePath = currentProvider.ImageUrl;

            var groupNameTextView = groupView.FindViewById<TextView>(Resource.Id.groupNameTextView);
            groupNameTextView.Text = currentProvider.Title;

            var checkmarkImageView = groupView.FindViewById<ImageView>(Resource.Id.checkmarkImageView);
            checkmarkImageView.SetOnClickListener(this);
            checkmarkImageView.Tag = groupPosition;

            var expandImageView = groupView.FindViewById<ImageView>(Resource.Id.expandImageView);
            expandImageView.SetOnClickListener(this);
            expandImageView.Tag = groupPosition;

            if (currentProvider.ChildItems.Count == 0)
            {
                _isExpanded = false;
                expandImageView.SetBackgroundColor(Color.White);

                if (currentProvider.IsSelected)
                {
                    groupNameTextView.SetTextColor(ContextCompat.GetColorStateList(_context, Resource.Color.highlight_color));
                    checkmarkImageView.SetImageDrawable(_context.ApplicationContext.GetDrawable(Resource.Drawable.checkmark));
                }
                else
                {
                    groupNameTextView.SetTextColor(Color.Black);
                    checkmarkImageView.SetImageDrawable(null);
                }
            }
            else
            {
                _isExpanded = isExpanded;
                expandImageView.SetImageDrawable(_context.ApplicationContext.GetDrawable(isExpanded
                    ? Resource.Drawable.icon_arrowup
                    : Resource.Drawable.icon_arrowdown));

                if (currentProvider.IsSelected)
                {
                    groupNameTextView.SetTextColor(ContextCompat.GetColorStateList(_context, Resource.Color.highlight_color));
                    checkmarkImageView.SetImageDrawable(_context.ApplicationContext.GetDrawable(Resource.Drawable.checkmark));
                }
                else
                {
                    checkmarkImageView.SetImageDrawable(null);
                }
            }

            return groupView;
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return _healthProviderTypeList[groupPosition].ChildItems[childPosition].Id;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return _healthProviderTypeList[groupPosition].ChildItems.Count;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            _isLastChild = isLastChild;

            var currentProvider = _healthProviderTypeList[groupPosition].ChildItems[childPosition];

            var itemView = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.health_provider_type_item_layout, null);

            var providerImage = itemView.FindViewById<MvxCachedImageView>(Resource.Id.itemImageView);
            providerImage.ImagePath = currentProvider.ImageUrl;

            var groupNameTextView = itemView.FindViewById<TextView>(Resource.Id.itemTextView);
            groupNameTextView.Text = currentProvider.Title;

            var checkmarkImageView = itemView.FindViewById<ImageView>(Resource.Id.checkmarkImageView);

            if (currentProvider.IsSelected)
            {
                _isExpanded = true;
                groupNameTextView.SetTextColor(ContextCompat.GetColorStateList(_context, Resource.Color.highlight_color));
                checkmarkImageView.SetImageDrawable(_context.ApplicationContext.GetDrawable(Resource.Drawable.checkmark));
            }
            else
            {
                checkmarkImageView.SetImageDrawable(null);
            }

            return itemView;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }

        public override Object GetGroup(int groupPosition)
        {
            return null;
        }

        public override Object GetChild(int groupPosition, int childPosition)
        {
            return null;
        }

        public void UnmarkAllButThis(HealthProviderTypeViewModel providerTypeElement)
        {
            int parentIndex = 0;
            int childIndex = -1;

            if (providerTypeElement.ParentId == null)
            {
                parentIndex = _healthProviderTypeList.IndexOf(providerTypeElement);
            }
            else
            {
                foreach (var parent in _healthProviderTypeList)
                {
                    childIndex = parent.ChildItems.IndexOf(providerTypeElement);
                    if (childIndex > -1)
                    {
                        parentIndex = _healthProviderTypeList.IndexOf(parent);
                        break;
                    }
                }
            }

            UnmarkAll();
            
            var view = childIndex > -1 ? GetChildView(parentIndex, childIndex, _isLastChild, null, null) 
                : GetGroupView(parentIndex, _isExpanded, null, null);

            var checkmark = view.FindViewById<ImageView>(Resource.Id.checkmarkImageView);
            checkmark.SetBackgroundResource(Resource.Drawable.checkmark);
        }

        private void UnmarkAll()
        {
            for (var i = 0; i < _healthProviderTypeList.Count; i++)
            {
                View view;
                if (_healthProviderTypeList[i].ChildItems.Count == 0)
                {
                    view = GetGroupView(i, _isExpanded, null, null);

                    var checkmarkImageView = view.FindViewById<ImageView>(Resource.Id.checkmarkImageView);
                    checkmarkImageView.SetImageDrawable(null);

                    var providerNameTextView = view.FindViewById<TextView>(Resource.Id.groupNameTextView);
                    providerNameTextView.SetTextColor(Color.Black);
                }
                else
                {
                    for (var j = 0; j < _healthProviderTypeList[i].ChildItems.Count; j++)
                    {
                        view = GetChildView(i, j, _isLastChild, null, null);

                        var checkmarkImageView = view.FindViewById<ImageView>(Resource.Id.checkmarkImageView);
                        checkmarkImageView.SetImageDrawable(null);

                        var providerNameTextView = view.FindViewById<TextView>(Resource.Id.itemTextView);
                        providerNameTextView.SetTextColor(Color.Black);
                    }
                }
            }
        }

        public void OnClick(View v)
        {
            var index = (int)v.Tag;
            if(_listView.IsGroupExpanded(index))
            {
                _listView.CollapseGroup(index);
            }
            else
            {
                _listView.ExpandGroup(index);
            }
        }
    }
}