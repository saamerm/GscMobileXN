using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Fragging;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Droid.Views;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Views;
using MobileClaims.Core.Entities;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Cirrious.MvvmCross.Views;


namespace MobileClaims.Droid
{
	[Region(Resource.Id.left_region)]
//	[Region(Resource.Id.left_region, false, BackstackTypes.FIRST_ITEM)]
	public class ProviderLookupProviderTypeFragment : BaseFragment, IMvxView
	{
		ProviderLookupProviderTypeViewModel _model;

		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.ProviderLookupProviderTypeFragment, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			_model = (ProviderLookupProviderTypeViewModel)this.ViewModel;
			try{
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
			}catch(Exception ex){
				System.Console.Write (ex.StackTrace);
			}
		}

		public override void OnDestroy()
		{
//			((ProviderLookupProviderTypeView)this.Activity).CloseAll ();
//			_model.GoHomeCommand.Execute (null);
			base.OnDestroy();
		}
	}

}