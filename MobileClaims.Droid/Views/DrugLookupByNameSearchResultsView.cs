using Android.App;
using Android.OS;
using Android.Views;
using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Views;


namespace MobileClaims.Droid
{

	[Region(Resource.Id.phone_main_region)]
	public class DrugLookupByNameSearchResultsView : BaseFragment, IMvxView
	{
		DrugLookupByNameSearchResultsViewModel _model;

		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);

			return this.BindingInflate(Resource.Layout.DrugLookupByNameSearchResultsFragment, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			_model = (DrugLookupByNameSearchResultsViewModel)ViewModel;
			ProgressDialog progressDialog = null;
			if (_model.Busy) {
				this.Activity.RunOnUiThread (() => {
					progressDialog = ProgressDialog.Show (this.Activity, "", Resources.GetString(Resource.String.loadingIndicator), true);
				});
			}

			_model.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
				if (e.PropertyName == "Busy") {
					if (_model.Busy) {
						this.Activity.RunOnUiThread (() => {
							if (progressDialog == null) {
								progressDialog = ProgressDialog.Show (this.Activity, "", Resources.GetString(Resource.String.loadingIndicator), true);
							}
							else {
								if (!progressDialog.IsShowing) {
									progressDialog.Show();
								}
							}
						});
					}
					else {
						this.Activity.RunOnUiThread (() => {
							if (progressDialog != null && progressDialog.IsShowing)
								progressDialog.Dismiss ();
						});
					}
				}
			};
		}

	}


//	[Activity (Label = "Drug Results")]			
//	public class DrugLookupByNameSearchResultsView : MvxFragmentActivity
//	{
//		protected override void OnCreate(Bundle bundle)
//		{
//			base.OnCreate(bundle);
//			SetContentView(Resource.Layout.DrugLookupByNameSearchResultsFragment);
//
//		}
//	}
}

