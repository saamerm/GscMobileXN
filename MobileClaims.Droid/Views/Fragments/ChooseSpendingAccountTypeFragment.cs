using System;
using Android.App;
using Android.OS;
using Android.Views;

using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;

namespace MobileClaims.Droid.Views.Fragments
{
    [Region(Resource.Id.phone_main_region, false, BackstackTypes.FIRST_ITEM)]
    public class ChooseSpendingAccountTypeFragment_ : BaseFragment
    {
        private ChooseSpendingAccountTypeViewModel _model;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.ChooseSpendingAccountTypeFragment, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ChooseSpendingAccountTypeViewModel)ViewModel;
        }
    }
}