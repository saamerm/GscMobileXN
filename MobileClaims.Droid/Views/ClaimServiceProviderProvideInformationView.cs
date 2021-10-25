using Android.OS;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Views;
using System;
using Android.Util;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MobileClaims.Droid
{
    [Region(Resource.Id.phone_main_region)]
	public class ClaimServiceProviderProvideInformationView : BaseFragment, IMvxView
	{
		ClaimServiceProviderProvideInformationViewModel _model;
		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);

		
			try
			{
				return this.BindingInflate(Resource.Layout.ClaimServiceProviderProvideInformationView, null);
			}
			catch(Exception ex)
			{
                Log.Error("ClaimServiceProviderProvideInformationView", ex.ToString());
                return null;
			}
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);
            
			_model = (ClaimServiceProviderProvideInformationViewModel)ViewModel;

//			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
//				TextView serviceProviderSelectTextView = this.Activity.FindViewById<TextView> (Resource.Id.claim_service_provider_select_title);
//				serviceProviderSelectTextView.Text = _model.ClaimSubmissionType.ID;
//
//				_model.NavigateToViewModelCommand.Execute (null);
//			} else {
			//				TextView claimSelectProviderListTitle = this.Activity.FindViewById<TextView>(Resource.Id.claim_select_provider_list_title);
//				claimSelectProviderListTitle.Text = string.Format(Resources.GetString(Resource.String.selectProviderFromListLabel), (_model.ClaimSubmissionType.Name).ToLower());

				TextView stillCantFindProviderLabel = this.Activity.FindViewById<TextView>(Resource.Id.still_cant_find_provider_label);
				stillCantFindProviderLabel.Text = string.Format(Resources.GetString(Resource.String.stillCantFindProviderLabel), (_model.ClaimSubmissionType.Name).ToLower());

				TextView stillCantFindProviderDescLabel = this.Activity.FindViewById<TextView>(Resource.Id.still_cant_find_provider_desclabel);
                stillCantFindProviderDescLabel.Text = Resources.FormatterBrandKeywords(Resource.String.stillCantFindProviderDescLabel, new string[] { Resources.GetString(Resource.String.gsc) });

                Button myServiceProvider = this.Activity.FindViewById<Button>(Resource.Id.myServiceProvider);
				myServiceProvider.Text = string.Format(Resources.GetString(Resource.String.myServiceProviderLabel), (_model.ClaimSubmissionType.Name).ToLower());
//			}
		}
	}
}

