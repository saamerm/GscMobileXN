using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using System;

namespace MobileClaims.Droid
{
	[Region(Resource.Id.phone_main_region)]		
	public class EligibilityCheckTypesFragment_ : BaseFragment
	{
		EligibilityCheckTypesViewModel _model;
		TextView myBenefitsSubtitle;
		TextView noBenefitsMsg;

		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.EligibilityCheckTypesFragment, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			_model = (EligibilityCheckTypesViewModel)this.ViewModel;
			try{
			ProgressDialog progressDialog = null;
			noBenefitsMsg = this.Activity.FindViewById<TextView>(Resource.Id.noBenefits);
			myBenefitsSubtitle = this.Activity.FindViewById<TextView>(Resource.Id.myBenefitsSubtitle);
			if (_model.NoAccessToEligibilityChecks) {
				noBenefitsMsg.Visibility = ViewStates.Visible;
			}
			else {
				noBenefitsMsg.Visibility = ViewStates.Gone;
			}
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
				else if (e.PropertyName == "NoAccessToEligibilityChecks") {
					if (_model.NoAccessToEligibilityChecks) {
						noBenefitsMsg.Visibility = ViewStates.Visible;
						myBenefitsSubtitle.Visibility = ViewStates.Gone;
					}
					else {
						noBenefitsMsg.Visibility = ViewStates.Gone;
						myBenefitsSubtitle.Visibility = ViewStates.Visible;
					}
				}
			};
			}catch(Exception ex){
				System.Console.Write (ex.StackTrace);
			}
		}

		public override void OnDestroy()
		{
//			((FindServiceProviderView)this.Activity).CloseAll ();
//			_model.GoHomeCommand.Execute (null);
			base.OnDestroy();
		}
    }
}