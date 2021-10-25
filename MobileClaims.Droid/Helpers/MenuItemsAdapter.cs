using System;
using System.Collections.Generic;
using System.Globalization;
using Android.Graphics.Drawables;
using Android.Support.V7.Widget;
using Android.Views;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Converters;

namespace MobileClaims.Droid.Helpers
{
    public class MenuItemsAdapter : RecyclerView.Adapter
    {
        private readonly LandingPageViewModel _viewModel;
        private readonly IList<MenuItem> _menuItems;

        public MenuItemsAdapter(LandingPageViewModel viewModel)
        {
            _viewModel = viewModel;
            _menuItems = _viewModel.DynamicMenuItems;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (!(holder is MenuItemsViewHolder viewHolder))
            {
                return;
            }

            viewHolder.IsRecyclable = false;

            var menuItem = _menuItems[position];

            var converter = new MenuItemNameToDrawableConverter();

            var icon = converter.Convert(menuItem.Links[0].MenuItemRel, typeof(Android.Resource.Drawable), null,
                CultureInfo.CurrentUICulture);

            viewHolder.MenuItemIcon.SetImageDrawable((Drawable)icon);

            viewHolder.MenuItemLabel.Text = menuItem.DisplayLabel;

            viewHolder.MenuItemsCountLabel.Text = menuItem.Count.ToString();
            viewHolder.MenuItemsCountLabel.Visibility = menuItem.Count > 0 ? ViewStates.Visible : ViewStates.Gone;

            viewHolder.ItemView.Tag = position;
            viewHolder.ItemView.Click -= ItemViewOnClick;
            viewHolder.ItemView.Click += ItemViewOnClick;
        }

        private void ItemViewOnClick(object sender, EventArgs e)
        {
            if (int.TryParse((string) (sender as View)?.Tag, out int position))
            {
                _viewModel.OpenMenuItemCommand.Execute(_menuItems[position]);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);
            var view = inflater.Inflate(Resource.Layout.item_menu, parent, false);

            return new MenuItemsViewHolder(view);
        }

        public override int ItemCount => _viewModel.DynamicMenuItems.Count;
    }
}