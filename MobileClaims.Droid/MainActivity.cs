using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.ViewModels;
using System;
using MobileClaims.Droid.Presenter;
using MvvmCross.Platforms.Android.Views;

namespace MobileClaims.Droid
{
	/// <summary>
	/// Main activity for the application.  Collaborates with MultiRegionPresenter to create a composite UI made up of multiple MvxFragment views.
	/// </summary>
	/// 
	//[Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.ScreenSize)]

	//	android:windowSoftInputMode="stateAlwaysHidden|adjustResize|adjustPan"
	// Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen"
	[Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden, WindowSoftInputMode = SoftInput.StateAlwaysHidden | SoftInput.AdjustResize)]
	public class MainActivity : MvxActivity, IMultiRegionHost
	{
		// Timeout for back button exit
		private const int lengthToWait = 3;
        private object _sync = new object();
		// Initialize to something that won't exit on the first back button press
		private DateTime LastBackbuttonPress = DateTime.Now.AddSeconds(lengthToWait*-1);

		private readonly int[] _validRegionIds = new int[] {Resource.Id.right_region, Resource.Id.left_region, Resource.Id.phone_main_region, Resource.Id.nav_region};


		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            //UserDialogs.Init(() => Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity);

//			//Remove title bar
//			this.RequestWindowFeature (WindowFeatures.NoTitle);
//			//Remove notification bar
//			this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

			// Set the layout
			SetContentView(Resource.Layout.MainLayout);

			// must call base implementation to create the activity before starting MvxApp.
			base.OnCreate(savedInstanceState);

			// register this activity with the MultiRegionPresenter.
			var presenter = Mvx.IoCProvider.Resolve<IMultiRegionPresenter>();
			presenter.RegisterMultiRegionHost( this);

			StartMvxApp();
		}
        protected override void OnStop()
        {
            base.OnStop();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        
		/// <summary>
		/// Starts the Mvx application.  
        /// The StartupSplashSreen overrides the default implementation of TriggerFirstNavigate to do nothing, as the
		/// app must be started after the MainActivity is created.
		/// </summary>
		protected void StartMvxApp()
		{
			var starter = Mvx.IoCProvider.Resolve<IMvxAppStart>();
			starter.Start();
		}
        
		// Custom back button event handling
		/* public override void OnBackPressed()
        {
            // TODO: Any back functionality will likely have to tie into Mvx to show view and manage history.

            //DateTime now = DateTime.Now;
            //TimeSpan offset = new TimeSpan(0, 0, lengthToWait);

            //// Close the popup if it is open
            //if (((FirstViewModel)ViewModel).ShowPopup)
            //{
            //    ((FirstViewModel)ViewModel).ClosePopupCommand();
            //}
            //// Stop app from immediately exiting unless the back button is 
            ////    pressed twice within "lengthToWait" seconds
            //else if ((now - LastBackbuttonPress).TotalSeconds > offset.TotalSeconds)
            //{
            //    // Back button was just pressed
            //    LastBackbuttonPress = now;
            //    Toast.MakeText(this, "Press the back button again to exit.", ToastLength.Long).Show();
            //}
            //else
            //{
            //    // Normal back button behavior
            //    base.OnBackPressed();
            //}
        }*/

		/// <summary>
		/// Shows the MvxFragment view in the region specified by its RegionAttribute.
		/// Called from the IMultiRegionPresenter.
		/// </summary>
		/// <param name="fragment"></param>
		public void Show(MvxFragment fragment)
		{
			Show( fragment, true);
		}

        protected void Show(MvxFragment fragment, bool backStack)
		{
            lock (_sync)
            {
                int regionResourceId = fragment.GetRegionId();
                bool hidesNav = fragment.GetHidesNav(); 
                
                //NavRegion RegionAttribute DICTATES WHETHER THE NAVIGATION REGION SHOULD SHOW OR NOT
                FrameLayout navRegion = (FrameLayout)FindViewById(Resource.Id.nav_region);

                if (hidesNav)
                {
                    navRegion.Visibility = ViewStates.Gone;
                }
                else
                {
                    navRegion.Visibility = ViewStates.Visible;
                }

                var ft = FragmentManager.BeginTransaction();
                //ft.SetCustomAnimations(Resource.Animation.enter, Resource.Animation.exit, Resource.Animation.popenter, Resource.Animation.popexit);

                if (regionResourceId == Resource.Id.nav_region)
                {
                    ft.Replace(Resource.Id.nav_region, fragment);
                }
                else
                {
                    ft.Replace(Resource.Id.phone_main_region, fragment);
                }

                //MANAGE BACKSTACK BEHAVIOUR HERE
                int backstackBehaviour = fragment.GetBackstackBehaviour();

                if (backstackBehaviour == BackstackTypes.FIRST_ITEM)
                {

                    //int id_val = FragmentManager.GetBackStackEntryAt (FragmentManager.BackStackEntryCount - 1).Id;
                    FragmentManager.PopBackStack(null,PopBackStackFlags.Inclusive);
                    ft.AddToBackStack(null);
                }
                else if (backstackBehaviour == BackstackTypes.ADD)
                {
                    ft.AddToBackStack(null);
                }

                ft.Commit();
            }
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
			fragment = FragmentManager.FindFragmentById(Resource.Id.right_region) as MvxFragment;
			if (fragment != null)
			{
				CloseFragment(fragment);
			}
			fragment = FragmentManager.FindFragmentById(Resource.Id.left_region) as MvxFragment;
			if (fragment != null)
			{
				CloseFragment(fragment);
			}
			fragment = FragmentManager.FindFragmentById(Resource.Id.phone_main_region) as MvxFragment;
			if (fragment != null)
			{
				CloseFragment(fragment);
			}
        }

		private MvxFragment GetActiveFragmentForViewModel(IMvxViewModel viewModel)
		{
			foreach (int regionId in _validRegionIds)
			{
				MvxFragment fragment = FragmentManager.FindFragmentById(regionId) as MvxFragment;
				if (fragment != null && fragment.ViewModel == viewModel)
					return fragment;
			}

			return null;
		}

		private void CloseFragment(MvxFragment fragment)
		{
			var ft = FragmentManager.BeginTransaction();
			ft.Remove(fragment);
			ft.Commit();
		}

		private void CloseModal()
		{
			var viewFragment = FragmentManager.FindFragmentById(Resource.Id.phone_main_region) as MvxFragment;
			if (viewFragment != null)
			{
				CloseFragment(viewFragment);
			}
		}

		private void CloseRight()
		{
			var rightFragment = FragmentManager.FindFragmentById(Resource.Id.right_region) as MvxFragment;
			if (rightFragment != null)
			{
				CloseFragment(rightFragment);
			}
		}

		private void CloseSplit()
		{
			var leftFragment = FragmentManager.FindFragmentById(Resource.Id.left_region) as MvxFragment;
			if (leftFragment != null)
			{
				CloseFragment(leftFragment);
			}

			CloseRight ();
		}

		public override void OnConfigurationChanged (Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);

			for (var i = 0; i < this.FragmentManager.Fragments.Count; i++) {
				MvxFragment frag = (MvxFragment)this.FragmentManager.Fragments [i];
				//frag.OnConfigurationChanged (newConfig);

			}

		}



	}
}