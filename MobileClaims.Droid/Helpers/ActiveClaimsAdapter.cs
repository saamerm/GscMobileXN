using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Droid.Helpers
{
    public class ActiveClaimsAdapter : RecyclerView.Adapter
    {
        private readonly IList<TopCardViewData> _activeClaims;
        private readonly ChooseClaimOrHistoryViewModel _viewModel;

        public ActiveClaimsAdapter(IList<TopCardViewData> activeClaims, ChooseClaimOrHistoryViewModel viewModel)
        {
            _activeClaims = activeClaims;
            _viewModel = viewModel;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (!(holder is ActiveClaimsViewHolder viewHolder))
            {
                return;
            }

            var activeClaim = _activeClaims[position];

            viewHolder.ClaimDateTextView.Text = activeClaim.ServiceDate;
            viewHolder.ClaimFormNumberTextView.Text = activeClaim.ClaimForm;
            viewHolder.ClaimTypeTextView.Text = activeClaim.ServiceDescription;
            viewHolder.ClaimSumTextView.Text = activeClaim.ClaimedAmount;

            viewHolder.BorderLinearLayout.Click -= OnBorderLinearLayoutOnClick;
            viewHolder.BorderLinearLayout.Click += OnBorderLinearLayoutOnClick;

            void OnBorderLinearLayoutOnClick(object sender, EventArgs e)
            {
                _viewModel.SelectActiveClaimCommand.Execute(_activeClaims[position]);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);
            var view = inflater.Inflate(Resource.Layout.active_claim_item_layout, parent, false);

            return new ActiveClaimsViewHolder(view);
        }

        public override int ItemCount => _activeClaims.Count;
    }
}