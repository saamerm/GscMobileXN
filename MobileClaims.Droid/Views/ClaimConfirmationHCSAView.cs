using Android.OS;
using Android.Views;
using MobileClaims.Core.ViewModels.HCSA;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Views;

namespace MobileClaims.Droid.Views
{
    //[Region(Resource.Id.phone_main_region,false,BackstackTypes.ADD)]
    [Region(Resource.Id.phone_main_region, false, BackstackTypes.ADD)]
    public class ClaimConfirmationHCSAView : BaseFragment, IMvxView, ViewTreeObserver.IOnGlobalLayoutListener
    {
        ClaimConfirmationHCSAViewModel _model;
        public static NonSelectableList ClaimDetailsList;
        View rootView;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.ClaimConfirmationHCSAView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ClaimConfirmationHCSAViewModel)ViewModel;

            ClaimDetailsList = this.Activity.FindViewById<NonSelectableList>(Resource.Id.claim_confirmation_detailsList);
            if (ClaimDetailsList.Count > 0)
            {
                view.Post(() =>
                    {
                        Utility.setFullListViewHeightCH(ClaimDetailsList);
                    });
            }
            rootView = view;
        }

        public void OnGlobalLayout()
        {
            if (ClaimDetailsList.Count > 0)
                Utility.setFullListViewHeightCH(ClaimDetailsList);
            rootView.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            rootView.ViewTreeObserver.AddOnGlobalLayoutListener(this);
        }
    }
}