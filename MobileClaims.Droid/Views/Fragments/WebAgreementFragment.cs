using Android.OS;
using Android.Views;
using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MobileClaims.Droid.Views.Fragments
{
    [Region(Resource.Id.phone_main_region, false, BackstackTypes.FIRST_ITEM)]
    public class WebAgreementFragment_ : BaseFragment
    {
        private WebAgreementViewModel _model;
        private View _view;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);

            _view = this.BindingInflate(Resource.Layout.WebAgreementView, null);
            return _view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _model = (WebAgreementViewModel)ViewModel;
        }
    }
}