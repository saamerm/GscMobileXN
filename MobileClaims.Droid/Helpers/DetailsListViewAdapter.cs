using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Environment = System.Environment;
using Android.Widget;
using MobileClaims.Core.ViewModelParameters;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace MobileClaims.Droid.Helpers
{
    public class DetailsListViewAdapter : RecyclerView.Adapter
    {
        private readonly Func<HealthProviderSummaryModel, Task> _showDetailsAction;
        private readonly Func<HealthProviderSummaryModel, Task> _toggleFavouriteAction;
        public List<HealthProviderSummaryModel> ServiceProviders { get; private set; }
        private readonly Func<HealthProviderSummaryModel, Task> _selectServiceProvider;

        public DetailsListViewAdapter(
            IEnumerable<HealthProviderSummaryModel> viewModelServiceProviders,
            Func<HealthProviderSummaryModel, Task> selectServiceProvider,
            Func<HealthProviderSummaryModel, Task> showDetailsAction,
            Func<HealthProviderSummaryModel, Task> toggleFavouriteAction)
        {
            _showDetailsAction = showDetailsAction;
            _toggleFavouriteAction = toggleFavouriteAction;
            ServiceProviders = viewModelServiceProviders?.ToList() ?? new List<HealthProviderSummaryModel>();
            _selectServiceProvider = selectServiceProvider;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (!(holder is DetailsListViewHolder viewHolder))
            {
                return;
            }

            var currentActivity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            var provider = ServiceProviders[position];
            viewHolder.Title.Text = provider.ProviderType;
            viewHolder.Name.Text = provider.Model.ProviderTradingName;

            viewHolder.DirectBilling.Visibility = !provider.IsDirectBill ? ViewStates.Gone : ViewStates.Visible;

            var score = (float)provider.Model.OverallScore;

            if (!provider.DisplayRating)
            {
                viewHolder.RatingLayout.Visibility = ViewStates.Gone;
            }

            viewHolder.Stars.Rating = score;

            if (provider.Model.OverallScore < 1.0)
            {
                viewHolder.Score.Visibility = ViewStates.Gone;
                viewHolder.NotAvailable.Visibility = ViewStates.Visible;
            }
            else
            {
                viewHolder.Score.Visibility = ViewStates.Visible;
                viewHolder.NotAvailable.Visibility = ViewStates.Gone;
                viewHolder.Score.Text = provider.ScoreString;
            }

            viewHolder.Details.Text = provider.FullAddressMultiline
                                      + Environment.NewLine
                                      + provider.OpeningHoursText;

            viewHolder.DetailsListItem.Tag = position.ToString();
            viewHolder.DetailsListItem.Click -= DetailsListItemOnClick;
            viewHolder.DetailsListItem.Click += DetailsListItemOnClick;

            SetFavouriteValue(viewHolder.Heart, provider.IsFavourite, currentActivity);
            viewHolder.Heart.Tag = position.ToString();
            viewHolder.Heart.Click -= HeartOnClick;
            viewHolder.Heart.Click += HeartOnClick;

            //TODO this is temporary - check MSGSC-705
            viewHolder.Distance.Visibility = ViewStates.Gone;

            viewHolder.Distance.Text = provider.DistanceText;
        }

        private async void HeartOnClick(object sender, EventArgs e)
        {
            if (sender is ImageView imageView)
            {
                var p = (string)imageView.Tag;
                if (int.TryParse(p, out var pos))
                {
                    var provider = ServiceProviders[pos];
                    await _toggleFavouriteAction(provider);
                    SetFavouriteValue(imageView, provider.IsFavourite, imageView.Context);
                }
            }
        }

        private async void DetailsListItemOnClick(object sender, EventArgs e)
        {
            if (sender is View view)
            {
                var p = (string)view.Tag;
                if (int.TryParse(p, out var pos))
                {
                    await Task.WhenAll(_selectServiceProvider(ServiceProviders[pos]), _showDetailsAction(ServiceProviders[pos]));
                }
            }
        }

        private static void SetFavouriteValue(
            ImageView heart,
            bool isFavourite, Context context)
        {
            heart.SetImageDrawable(isFavourite
                ? ContextCompat.GetDrawable(context, Resource.Drawable.heart_on)
                : ContextCompat.GetDrawable(context, Resource.Drawable.heart_off));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);
            var view = inflater.Inflate(Resource.Layout.details_list_item, parent, false);

            return new DetailsListViewHolder(view);
        }

        public override int ItemCount => ServiceProviders.Count;

        public void UpdateProviders(IEnumerable<HealthProviderSummaryModel> viewModelServiceProviders)
        {
            ServiceProviders = viewModelServiceProviders?.ToList() ?? new List<HealthProviderSummaryModel>();
        }
    }
}