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
	public class LocateServiceProviderChooseSearchTypeFragment : BaseFragment, IMvxView 
	{
		LocateServiceProviderChooseSearchTypeViewModel _model;
		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.LocateServiceProviderChooseSearchTypeFragment, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			var locationList = Activity.FindViewById(Resource.Id.locateProviderSearchLocationList) as MvxListView;
			var searchTypeList = Activity.FindViewById(Resource.Id.locateProviderSearchTypeList) as MvxListView;


			if (locationList.Count > 0)
				Utility.setFullListViewHeight (locationList);

			if (searchTypeList.Count > 0)
				Utility.setFullListViewHeight (searchTypeList);

			_model = (LocateServiceProviderChooseSearchTypeViewModel)this.ViewModel;
			try{
			//			_model.SelectedLocationType = _model.LocationTypes [0];
			//			_model.SelectedSearchType = _model.SearchTypes [0];

			if (_model.SelectedLocationType != null) {
				locationList.SetItemChecked (_model.SelectedLocationType.TypeId - 1, true);
			}
			if (_model.SelectedSearchType != null) {
				searchTypeList.SetItemChecked (_model.SelectedSearchType.TypeId - 1, true);
			}

			//			this.View.Invalidate ();

			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
//				_model.SetSearchTypeWithNavigationCommand.Execute (_model.SelectedSearchType);
				_model.FindProviderCommand.Execute (null);
			} else {
				var locateProviderContinue = Activity.FindViewById (Resource.Id.locateProviderContinue) as Button;
				locateProviderContinue.Click += (sender, args) => {
					_model.SetSearchTypeWithNavigationCommand.Execute (_model.SelectedSearchType);
				};
			}
			}catch(Exception ex){
				System.Console.Write (ex.StackTrace);
			}
		}

		public override void OnDestroy()
		{
//			if (!this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
//				_model.GoBackCommand.Execute (null);
//			}
			base.OnDestroy();
		}
	}

}