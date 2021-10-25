using Android.OS;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MobileClaims.Droid.Views
{
	[Region(Resource.Id.phone_main_region, false, BackstackTypes.FIRST_ITEM)]
	public class ChangeForLifeNoAccessFragment_ : BaseFragment, IMvxView
	{
        public IMvxViewModel ViewModel { get; set; }

		//	ChangeForLifeTermsandConditionsFragment changeForLifeTermsandConditionsFragment;
		ChangeForLifeNoAccessViewModel _model;

		LinearLayout checkContainer;
		//CheckBox termsCheck;
		//private ILoginService _loginservice;
		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			// Set the layout
			return this.BindingInflate(Resource.Layout.ChangeForLifeNoAccessView, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
        }
    }
}