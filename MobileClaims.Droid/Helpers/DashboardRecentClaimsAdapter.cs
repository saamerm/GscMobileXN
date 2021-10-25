using Android.Support.V7.Widget;
using Android.Views;
using MobileClaims.Core.ViewModels;
using System;

namespace MobileClaims.Droid.Helpers
{
    public class DashboardRecentClaimsAdapter : RecyclerView.Adapter
    {
        private readonly DashboardViewModel _viewModel;

        public DashboardRecentClaimsAdapter(DashboardViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            holder.IsRecyclable = false;

            if (holder is DashboardRecentClaimsViewHolder viewHolder)
            {
                viewHolder.Description.Text = _viewModel.RecentClaims[position].ServiceDescription;
                viewHolder.ServiceDate.Text = _viewModel.RecentClaims[position].ServiceDate;
                viewHolder.TotalAmountClaimed.Text = _viewModel.RecentClaims[position].ClaimedAmount;

                viewHolder.ItemView.Tag = position;
                viewHolder.ItemView.Click -= ItemViewOnClick;
                viewHolder.ItemView.Click += ItemViewOnClick;
            }

            if (holder is DashboardRecentClaimsActionRequiredViewHolder viewHolderActionRequired)
            {
                viewHolderActionRequired.Description.Text = _viewModel.RecentClaims[position].ServiceDescription;
                viewHolderActionRequired.ServiceDate.Text = _viewModel.RecentClaims[position].ServiceDate;
                viewHolderActionRequired.TotalAmountClaimed.Text = _viewModel.RecentClaims[position].ClaimedAmount;

                viewHolderActionRequired.ItemView.Tag = position;
                viewHolderActionRequired.ItemView.Click -= ItemViewOnClick;
                viewHolderActionRequired.ItemView.Click += ItemViewOnClick;
            }
        }

        public override int GetItemViewType(int position)
        {
            return _viewModel.RecentClaims[position].ActionRequired ? 1 : 0;
        }

        private void ItemViewOnClick(object sender, EventArgs e)
        {
            if (int.TryParse((string)(sender as View)?.Tag, out int position))
            {
                _viewModel.SelectRecentClaimCommand.Execute(_viewModel.RecentClaims[position]);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);
            View view;

            switch (viewType)
            {
                case 0:
                    view = inflater.Inflate(Resource.Layout.item_dashboard_recent_claim, parent, false);
                    return new DashboardRecentClaimsViewHolder(view);
                default:
                    view = inflater.Inflate(Resource.Layout.item_dashboard_recent_claim_action_required, parent, false);
                    return new DashboardRecentClaimsActionRequiredViewHolder(view);
            }
        }

        public override int ItemCount => _viewModel.RecentClaims.Count;
    }
}