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
using Android.Views.InputMethods;


namespace MobileClaims.Droid
{
	[Region(Resource.Id.right_region)]		
	public class LocateServiceProviderView : BaseFragment, IMvxView 
	{
		bool dialogShown;
		LocateServiceProviderViewModel _model;
		SeekBar theSlider;
//		TextView searchRadiusText;
		TextView cityLabel;
		TextView lastNameLabel;
		TextView phoneNumberLabel;
		TextView businessNameLabel;

		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);

			return this.BindingInflate(Resource.Layout.LocateServiceProviderView, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			theSlider = this.View.FindViewById<SeekBar>(Resource.Id.locateProviderSeekBar);
//			searchRadiusText = this.View.FindViewById<TextView>(Resource.Id.locateProviderDynamicSearchText);
			businessNameLabel = this.View.FindViewById<TextView>(Resource.Id.locateProviderBusinessNameLabel);
			phoneNumberLabel = this.View.FindViewById<TextView>(Resource.Id.locateProviderPhoneNumberLabel);
			lastNameLabel = this.View.FindViewById<TextView>(Resource.Id.locateProviderLastNameLabel);
			cityLabel = this.View.FindViewById<TextView>(Resource.Id.locateProviderCityLabel);

			_model = (LocateServiceProviderViewModel)ViewModel;
			_model.SearchRadius = 5;

			ProgressDialog progressDialog = null;
			_model.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
				if (e.PropertyName == "Searching") {
					if (_model.Searching) {
						this.Activity.RunOnUiThread (() => {
							progressDialog = ProgressDialog.Show (this.Activity, "", this.Activity.Resources.GetString(Resource.String.searchingIndicator), true);
						});
					} else {
						this.Activity.RunOnUiThread (() => {
							if (progressDialog != null && progressDialog.IsShowing)
								progressDialog.Dismiss ();
						});
					}
				}
			};

			SetSliderTitle ();

			/*if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
				_model.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
					if(e.PropertyName=="ProviderType")
					{
						SetSliderTitle();
					}
					if(e.PropertyName=="SelectedLocationType")
					{
						SetSliderPosition();
					}
					if(e.PropertyName=="SelectedSearchType")
					{
//						SetSearchTitles ();
					}
			};
			}*/


				((SeekBar)theSlider).ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
					int currentProgress = ((SeekBar)sender).Progress;
					currentProgress = currentProgress / 5;
					currentProgress = currentProgress * 5;
					((SeekBar)sender).Progress = currentProgress;
					_model.SearchRadius = currentProgress + 5;

					SetSliderTitle();
				};

//			SetSliderTitle ();
			//SetSliderPosition();
//			SetSearchTitles ();

			_model.NoProvidersFound += ShowSearchError;
//			if(_model.SelectedLocationType.TypeId == 3)
				_model.InvalidPostalCode += ShowInvalidPostalCode;
//			if(_model.SelectedLocationType.TypeId == 2)
				_model.MissingAddressOrPostalCode += MissingAddressOrPostalCode;
//			if(_model.SelectedSearchType.TypeId == 2)
				_model.InvalidPhoneNumber += InvalidPhoneNumber;


			Button locateProviderSearch = this.Activity.FindViewById<Button>(Resource.Id.locateProviderSearch);

			locateProviderSearch.Click += (object sender, EventArgs e) => {
				InputMethodManager manager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
				manager.HideSoftInputFromWindow(locateProviderSearch.WindowToken, 0);
					

			};
		}



		void SetSliderPosition()
		{
			if (_model.SelectedLocationType.TypeName == "Address" || _model.SelectedLocationType.TypeName == "Postal Code")
			{
				((SeekBar)theSlider).SetPadding(0,50,0,0);

				if (_model.SelectedLocationType.TypeName == "Postal Code") {
					((SeekBar)theSlider).Max = 45;
				} else {
					((SeekBar)theSlider).Max = 45;
					((SeekBar)theSlider).KeyProgressIncrement = 5;

				}

			}
			else
			{
				((SeekBar)theSlider).SetPadding(0,0,0,0);
			}
		}

		void SetSliderTitle()
		{
//				((TextView)searchRadiusText).Text = Resources.GetString (Resource.String.detailsProviderRadiusInfoLabel) +
//				" " + _model.ProviderType.Type.ToUpper () +
//				" " + Resources.GetString (Resource.String.detailsProviderRadiusInfoLabelMiddle) +
//				" " + (((SeekBar)theSlider).Progress + 5).ToString () + " KM " +
//				Resources.GetString (Resource.String.detailsProviderRadiusInfoLabelEnd) +
//				" " + _model.SelectedLocationType.TypeName.ToUpper ();
			TextView lspProviderRadiusInfoLabelMylocationLbl = this.View.FindViewById<TextView>(Resource.Id.lspProviderRadiusInfoLabelMylocationLabel);
			lspProviderRadiusInfoLabelMylocationLbl.Text = string.Format(Resources.GetString(Resource.String.lspProviderRadiusInfoLabelMylocation), (_model.SearchRadius.ToString()));

			TextView lspProviderRadiusInfoLabelAddressLbl = this.Activity.FindViewById<TextView>(Resource.Id.lspProviderRadiusInfoLabelAddressLabel);
			lspProviderRadiusInfoLabelAddressLbl.Text = string.Format(Resources.GetString(Resource.String.lspProviderRadiusInfoLabelAddress), (_model.SearchRadius));

			TextView lspProviderRadiusInfoLabelPostalcodeLbl = this.Activity.FindViewById<TextView>(Resource.Id.lspProviderRadiusInfoLabelPostalcodeLabel);
			lspProviderRadiusInfoLabelPostalcodeLbl.Text = string.Format(Resources.GetString(Resource.String.lspProviderRadiusInfoLabelPostalcode), (_model.SearchRadius));

		}

		/*void SetSearchTitles()
		{
			businessNameLabel.Text = Resources.GetString(Resource.String.detailsProviderSearchLabel) + " " + _model.SelectedSearchType.TypeName.ToUpper() + ".";
			phoneNumberLabel.Text = Resources.GetString(Resource.String.detailsProviderSearchLabel) + " " + _model.SelectedSearchType.TypeName.ToUpper() + ".";
			lastNameLabel.Text = Resources.GetString(Resource.String.detailsProviderSearchLabel) + " " + _model.SelectedSearchType.TypeName.ToUpper() + ".";
			cityLabel.Text = Resources.GetString(Resource.String.detailsProviderSearchLabel) + " " + _model.SelectedSearchType.TypeName.ToUpper() + ".";
		}*/

		void ShowSearchError (object s, EventArgs e)
		{
			if (!dialogShown) {

				this.Activity.RunOnUiThread(() =>
					{
						this.dialogShown = true;
						Android.Content.Res.Resources res = this.Resources;
						AlertDialog.Builder builder;
						builder = new AlertDialog.Builder(this.Activity);
						builder.SetTitle( string.Format(res.GetString(Resource.String.detailsProviderErrorTitle)));
						builder.SetMessage(string.Format(res.GetString(Resource.String.detailsProviderErrorDesc)));
						builder.SetCancelable(false);
						builder.SetPositiveButton("OK", delegate { dialogShown = false; });
						builder.Show();
					}
				);
			}
		}

		void ShowInvalidPostalCode(object s, EventArgs e)
		{
				if (!dialogShown) {

					this.Activity.RunOnUiThread(() =>
						{
							this.dialogShown = true;
							Android.Content.Res.Resources res = this.Resources;
							AlertDialog.Builder builder;
							builder = new AlertDialog.Builder(this.Activity);
						builder.SetTitle( string.Format(res.GetString(Resource.String.detailsProviderPostalCodeErrorTitle)));
						builder.SetMessage(string.Format(res.GetString(Resource.String.detailsProviderPostalCodeErrorDesc)));
							builder.SetCancelable(false);
							builder.SetPositiveButton("OK", delegate { dialogShown = false; });
							builder.Show();
						}
					);
				}
		}

		void MissingAddressOrPostalCode(object s, EventArgs e)
		{
			if (!dialogShown) {

				this.Activity.RunOnUiThread(() =>
					{
						this.dialogShown = true;
						Android.Content.Res.Resources res = this.Resources;
						AlertDialog.Builder builder;
						builder = new AlertDialog.Builder(this.Activity);
						builder.SetTitle( string.Format(res.GetString(Resource.String.detailsProviderMissingAddressOrPostalCodeTitle)));
						builder.SetMessage(string.Format(res.GetString(Resource.String.detailsProviderMissingAddressOrPostalCodeDesc)));
						builder.SetCancelable(false);
						builder.SetPositiveButton("OK", delegate { dialogShown = false; });
						builder.Show();
					}
				);
			}
		}

		void InvalidPhoneNumber(object s, EventArgs e)
		{
			if (!dialogShown) {

				this.Activity.RunOnUiThread(() =>
					{
						this.dialogShown = true;
						Android.Content.Res.Resources res = this.Resources;
						AlertDialog.Builder builder;
						builder = new AlertDialog.Builder(this.Activity);
						builder.SetTitle( string.Format(res.GetString(Resource.String.detailsProviderPhoneNumberErrorTitle)));
						builder.SetMessage(string.Format(res.GetString(Resource.String.detailsProviderPhoneNumberErrorDesc)));
						builder.SetCancelable(false);
						builder.SetPositiveButton("OK", delegate { dialogShown = false; });
						builder.Show();
					}
				);
			}
		}
	}

}