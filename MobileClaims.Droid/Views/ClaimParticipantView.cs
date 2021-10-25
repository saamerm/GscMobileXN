using Android.OS;
using Android.Views;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Views;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class ClaimParticipantView : BaseFragment , IMvxView
    {
   //    ClaimParticipantViewModel _model;

		bool dialogShown;

		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.ClaimParticipantView, null);
		}

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            //  _model = (ClaimParticipantViewModel)ViewModel;
        }
    }
}