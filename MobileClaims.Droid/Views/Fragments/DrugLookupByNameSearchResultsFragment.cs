using Android.OS;
using Android.Views;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MobileClaims.Droid.Views
{
	public class DrugLookupByNameSearchResultsFragment_ : BaseFragment
    {
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.DrugLookupByNameSearchResultsFragment, null);
		}
	}
}

