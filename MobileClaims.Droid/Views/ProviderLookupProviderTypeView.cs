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
using Android.Content.PM;
using Java.Interop;


namespace MobileClaims.Droid
{
	[Activity(Label = "", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden, WindowSoftInputMode = SoftInput.StateAlwaysHidden | SoftInput.AdjustPan)]
	// , NoHistory = true
    public class ProviderLookupProviderTypeView : FragmentActivityBase, IMultiRegionHost
	{
		private readonly int[] _validRegionIds = new int[] {Resource.Id.right_region, Resource.Id.left_region, Resource.Id.modal_region, Resource.Id.nav_region, Resource.Id.popup_region};
		private View _transparentPopupView = null;

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{

			//			//Remove title bar
			//			this.RequestWindowFeature (WindowFeatures.NoTitle);
			//			//Remove notification bar
			//			this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

			// Set the layout
			SetContentView(Resource.Layout.MainActivity);

			// must call base implementation to create the activity before starting MvxApp.
			base.OnCreate(savedInstanceState);

			// set the click handler for the transparent popup background
			_transparentPopupView = FindViewById<FrameLayout>(Resource.Id.popup_background);
		}

		public override void OnBackPressed ()
		{
			int backStackCount = SupportFragmentManager.BackStackEntryCount;
			if (backStackCount == 1) {
				((ProviderLookupProviderTypeViewModel)this.ViewModel).GoBackCommand.Execute (null);
			}
			else {
				if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
					String fragmentTag = SupportFragmentManager.GetBackStackEntryAt (backStackCount - 1).Name;
					if (fragmentTag == "mobileclaims.droid.LocateServiceProviderResultView") {
						MvxFragment currentFragment = (MvxFragment)SupportFragmentManager.FindFragmentByTag (fragmentTag);
						int regionResourceId = currentFragment.GetRegionId ();
						if (regionResourceId == Resource.Id.right_region) {
							closeSplitFragments ();
						}
					} else if (fragmentTag == "mobileclaims.droid.LocateServiceProviderView") {
						MvxFragment currentFragment = (MvxFragment)SupportFragmentManager.FindFragmentByTag (fragmentTag);
						int regionResourceId = currentFragment.GetRegionId ();
						if (regionResourceId == Resource.Id.right_region) {
							closeSplitFragments ();
						}
					}
//				this.FragmentManager.PopBackStack ();
				}
			}
			base.OnBackPressed ();
		}

		private void closeSplitFragments() {
			int backStackCount = SupportFragmentManager.BackStackEntryCount;

			for (int i = backStackCount; i > 0; i--) {
				String fragmentTag = SupportFragmentManager.GetBackStackEntryAt (i - 1).Name;
				MvxFragment currentFragment = (MvxFragment)SupportFragmentManager.FindFragmentByTag (fragmentTag);
				int regionResourceId = currentFragment.GetRegionId ();
				if (regionResourceId == Resource.Id.left_region) {
					CloseFragment (currentFragment);
					//					SupportFragmentManager.PopBackStack ();
					return;
				} else {
					CloseFragment (currentFragment);
					//					SupportFragmentManager.PopBackStack ();
					base.OnBackPressed ();
				}
			}
			//			while (regionResourceId != Resource.Id.left_region) {
			//				backStackCount = SupportFragmentManager.BackStackEntryCount;
			//
			//			}
		}

		protected override void OnDestroy ()
		{
//			CloseAll ();
			base.OnDestroy ();
		}

		protected override void OnViewModelSet()
		{
			base.OnViewModelSet ();

			var presenter = Mvx.IoCProvider.Resolve<IMultiRegionPresenter> ();
			presenter.RegisterMultiRegionHost (this);

			// register this activity with the MultiRegionPresenter.
			ProviderLookupProviderTypeFragment cvf = new ProviderLookupProviderTypeFragment () {
				ViewModel = ViewModel
			};
			Show (cvf, true);

			MvxViewModelLoader _mvxloader = new MvxViewModelLoader ();
			MvxViewModelRequest mvxreq = MvxViewModelRequest.GetDefaultRequest (typeof(MainNavigationViewModel));
			//			MvxViewModel mnVM = (MvxViewModel)_mvxloader.LoadViewModel (mvxreq, null);
			//			MainNavigationView mnv = new MainNavigationView () {
			//				ViewModel = mnVM
			//			};
			//			Show (mnv);
			presenter.Show (mvxreq);

		}

		[Export("errorClickHandler")] // The value found in android:onClick attribute.
		public void errorClickHandler(View v) // Does not need to match value in above attribute.
		{
			if (v.Tag != null)
				Toast.MakeText (this, v.Tag.ToString(), ToastLength.Long).Show ();
		}

		public void Show (MvxFragment fragment)
		{
			Show( fragment, true);
		}

		protected void Show(MvxFragment fragment, bool backStack)
		{
			if (!fragment.HasRegionAttribute())
			{
				throw new InvalidOperationException( "Fragment has no region attribute.");
			}

			int regionResourceId = fragment.GetRegionId();
			bool hidesNav = fragment.GetHidesNav (); 

			if (!_validRegionIds.Contains(regionResourceId))
			{
				throw new InvalidOperationException( "Id specified in resource attribute is invalid.");
			}

			//NavRegion RegionAttribute DICTATES WHETHER THE NAVIGATION REGION SHOULD SHOW OR NOT
			FrameLayout navRegion = (FrameLayout)FindViewById(Resource.Id.nav_region);

			if (hidesNav) {
				navRegion.Visibility = ViewStates.Gone;
			} else {
				navRegion.Visibility = ViewStates.Visible;
			}

			var ft = SupportFragmentManager.BeginTransaction();
			ft.SetCustomAnimations(Resource.Animation.enter,Resource.Animation.exit,Resource.Animation.popenter,Resource.Animation.popexit);

			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
				FrameLayout popupBackground = (FrameLayout)FindViewById(Resource.Id.popup_background);
				popupBackground.Visibility = ViewStates.Gone;

				if (regionResourceId == Resource.Id.left_region) {

					//REMOVE ALL REGIONS IF THE LEFT REGION IS BEING POPULATED
					CloseModal ();
					CloseRight ();
					ClosePopup ();

					FrameLayout transparency = (FrameLayout)FindViewById(Resource.Id.popup_background);
					transparency.Visibility = ViewStates.Gone;

					FrameLayout layout = (FrameLayout)FindViewById (Resource.Id.modal_region);
					layout.Visibility = ViewStates.Gone;

					FrameLayout leftlayout = (FrameLayout)FindViewById (Resource.Id.left_region);
					leftlayout.Visibility = ViewStates.Visible;

					FrameLayout rightlayout = (FrameLayout)FindViewById (Resource.Id.right_region);
					rightlayout.Visibility = ViewStates.Visible;

					//IF THE SimpleMessageText RegionAttribute IS DEFINED, IT GETS PUSHED TO THE RIGHT REGION AS A SimpleMessageView.

					string simpleMessageText = fragment.GetSimpleMessageText ();

					if (!string.IsNullOrEmpty (simpleMessageText)) {
						SimpleMessageView messageview = new SimpleMessageView ();
						messageview.setMessageText (simpleMessageText);
						ft.Replace (Resource.Id.right_region, ((Android.Support.V4.App.Fragment)messageview) );
					}
				} else if (regionResourceId == Resource.Id.modal_region) {
					CloseSplit();
					ClosePopup ();

					FrameLayout transparency = (FrameLayout)FindViewById(Resource.Id.popup_background);
					transparency.Visibility = ViewStates.Gone;

					FrameLayout layout = (FrameLayout)FindViewById (Resource.Id.modal_region);
					layout.Visibility = ViewStates.Visible;

					FrameLayout leftlayout = (FrameLayout)FindViewById (Resource.Id.left_region);
					leftlayout.Visibility = ViewStates.Gone;

					FrameLayout rightlayout = (FrameLayout)FindViewById (Resource.Id.right_region);
					rightlayout.Visibility = ViewStates.Gone;
				} else if (regionResourceId == Resource.Id.popup_region) {
					FrameLayout transparency = (FrameLayout)FindViewById(Resource.Id.popup_background);
					transparency.Visibility = ViewStates.Visible;
				}

			}

			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
				ft.Replace (regionResourceId, fragment, fragment.Class.Name);
			} else {

				//PHONES WILL NOT HAVE THE SPLIT LAYOUT. 
				//FRAGMENTS WILL BE PUSHED EITHER TO THE NAV REGION OR TO THE MAIN REGION

				if (regionResourceId == Resource.Id.nav_region) {
					ft.Replace (Resource.Id.nav_region, fragment);
				} else {
					ft.Replace (Resource.Id.phone_main_region, fragment);
				}
			}

			//MANAGE BACKSTACK BEHAVIOUR HERE
			int backstackBehaviour = fragment.GetBackstackBehaviour();

			if (backstackBehaviour == BackstackTypes.FIRST_ITEM) {

				//int id_val = SupportFragmentManager.GetBackStackEntryAt (SupportFragmentManager.BackStackEntryCount - 1).Id;
				SupportFragmentManager.PopBackStack (null, (int)PopBackStackFlags.Inclusive);
				ft.AddToBackStack(fragment.Class.Name);
			} else if (backstackBehaviour == BackstackTypes.ADD) {
				ft.AddToBackStack(fragment.Class.Name);
			}

			ft.Commit();

		}

		/// <summary>
		/// Closes the view associated with the given ViewModel.
		/// Called from the IMultiRegionPresenter.
		/// </summary>
		/// <param name="viewModel"></param>
		public void CloseViewModel(IMvxViewModel viewModel)
		{
			var fragment = GetActiveFragmentForViewModel(viewModel);
			if (fragment != null)
			{
				CloseFragment(fragment);
			}
		}

		/// <summary>
		/// Closes all active views.
		/// Called from the IMultiRegionPresenter.
		/// </summary>
		public void CloseAll()
		{
			MvxFragment fragment;
			fragment = SupportFragmentManager.FindFragmentById(Resource.Id.right_region) as MvxFragment;
			if (fragment != null)
			{
				CloseFragment(fragment);
			}
			fragment = SupportFragmentManager.FindFragmentById(Resource.Id.left_region) as MvxFragment;
			if (fragment != null)
			{
				CloseFragment(fragment);
			}
			fragment = SupportFragmentManager.FindFragmentById(Resource.Id.modal_region) as MvxFragment;
			if (fragment != null)
			{
				CloseFragment(fragment);
			}

		}

		private MvxFragment GetActiveFragmentForViewModel(IMvxViewModel viewModel)
		{
			foreach (int regionId in _validRegionIds)
			{
				MvxFragment fragment = SupportFragmentManager.FindFragmentById(regionId) as MvxFragment;
				if (fragment != null && fragment.ViewModel == viewModel)
					return fragment;
			}

			return null;
		}

		private void CloseFragment(MvxFragment fragment)
		{
			var ft = SupportFragmentManager.BeginTransaction();
			ft.Remove(fragment);
			ft.Commit();
		}

		private void CloseModal()
		{
			var viewFragment = SupportFragmentManager.FindFragmentById(Resource.Id.modal_region) as MvxFragment;
			if (viewFragment != null)
			{
				CloseFragment(viewFragment);
			}
		}

		private void CloseRight()
		{
			var rightFragment = SupportFragmentManager.FindFragmentById(Resource.Id.right_region) as MvxFragment;
			if (rightFragment != null)
			{
				CloseFragment(rightFragment);
			}
		}

		private void CloseSplit()
		{
			var leftFragment = SupportFragmentManager.FindFragmentById(Resource.Id.left_region) as MvxFragment;
			if (leftFragment != null)
			{
				CloseFragment(leftFragment);
			}

			CloseRight ();
		}

		private void ClosePopup()
		{
			//			_transparentPopupView.Visibility = ViewStates.Gone;

			var viewFragment = SupportFragmentManager.FindFragmentById(Resource.Id.popup_region) as MvxFragment;
			if (viewFragment != null)
			{
				CloseFragment(viewFragment);
			}
		}
	}
}