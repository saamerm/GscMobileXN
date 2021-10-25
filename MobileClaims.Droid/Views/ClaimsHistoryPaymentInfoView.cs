using Android.OS;
using Android.Views;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region, false, BackstackTypes.ADD)]
    public class ClaimsHistoryPaymentInfoView : BaseFragment
    {
        private ClaimsHistoryPaymentInfoViewModel _model;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            return this.BindingInflate(Resource.Layout.ClaimsHistoryPaymentInfo, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ClaimsHistoryPaymentInfoViewModel)ViewModel;
        }
    }
}