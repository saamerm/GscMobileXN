using System;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels.DirectDeposit;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MobileClaims.Droid.Views.DirectDeposit.DirectDepositConfirmation
{
    [Region(Resource.Id.phone_main_region)]
    public class DDConfirmationFragment : BaseFragment<DirectDepositConfirmationViewModel>
    {
        ImageView _consentIndicator;
        LinearLayout _consentLayout;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            view = this.BindingInflate(Resource.Layout.DDConfirmationFragment, null);
            _consentIndicator = view.FindViewById<ImageView>(Resource.Id.ConsentIndicator);
            _consentIndicator.SetBackgroundDrawable(Context.GetDrawable(Resource.Drawable.large_round_checkbox_unselected));
            _consentLayout = view.FindViewById<LinearLayout>(Resource.Id.ConsentLayout);
            _consentLayout.Click -= ConsentLayout_Click;
            _consentLayout.Click += ConsentLayout_Click;

            return view;
        }
        private void ConsentLayout_Click(object sender, EventArgs e)
        {
            SetConsentLayoutBackgroundColor((LinearLayout)sender);
            ViewModel.IsAuthorisedDepositFund = true;
            _consentIndicator.SetBackgroundDrawable(Context.GetDrawable(Resource.Drawable.large_round_checkbox_selected));
        }

        private void SetConsentLayoutBackgroundColor(LinearLayout ConsentLayoutView)
        {
            int color = Context.GetColor(Resource.Color.highlight_color);
            ConsentLayoutView.SetBackgroundColor(new Android.Graphics.Color(color));
        }
    }
}
