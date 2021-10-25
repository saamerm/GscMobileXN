using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using Java.Interop;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Views.Fragments;
using MvvmCross;
using MvvmCross.Platforms.Android.Views.Fragments;
using System;
using MobileClaims.Droid.Presenter;
using MvvmCross.ViewModels;
using MobileClaims.Droid.Interfaces;

namespace MobileClaims.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden, WindowSoftInputMode = SoftInput.StateAlwaysHidden | SoftInput.AdjustPan)]
    //	, NoHistory = true
    public class ChooseSpendingAccountTypeView : ActivityBase, IMultiRegionHost
    {
        private readonly int[] _validRegionIds = { Resource.Id.phone_main_region, Resource.Id.nav_region };
        bool dialogShown;
        
        private object _sync = new object();

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.MainLayout);

            base.OnCreate(savedInstanceState);
        }

        //public override void OnBackPressed()
        //{
        //    int backStackCount = FragmentManager.BackStackEntryCount;
        //    if (backStackCount == 1)
        //    {
        //        ((ChooseSpendingAccountTypeViewModel)this.ViewModel).GoBackCommand.Execute(null);
        //        base.OnBackPressed();
        //    }
        //    else
        //    {
        //        var fragments = FragmentManager.Fragments;
        //        if (fragments != null)
        //        {
        //            IBackPressedListener fr = null;
        //            foreach (var fragment in fragments)
        //            {
        //                fr = fragment as IBackPressedListener;
        //                if (fr != null)
        //                    break;
        //            }
        //            if (fr != null)
        //            {
        //                fr.OnBackPressed();
        //                base.OnBackPressed();
        //                return;
        //            }
        //        }
        //        base.OnBackPressed();
        //    }
        //}

        public override void OnBackPressed()
        {
            int backStackCount = FragmentManager.BackStackEntryCount;
            if (backStackCount == 1)
            {
                ((ChooseSpendingAccountTypeViewModel)this.ViewModel).GoBackCommand.Execute(null);
            }

            base.OnBackPressed();
        }
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            var presenter = Mvx.IoCProvider.Resolve<IMultiRegionPresenter>();
            presenter.RegisterMultiRegionHost(this);

            var cvf = new ChooseSpendingAccountTypeFragment_
            {
                ViewModel = ViewModel
            };
            Show(cvf, true);

            presenter.ShowBottomNavigation();
        }

        [Export("errorClickHandler")] // The value found in android:onClick attribute.
        public void errorClickHandler(View v) // Does not need to match value in above attribute.
        {
            if (v.Tag != null)
            {
                var toast = Toast.MakeText(this, v.Tag.ToString(), ToastLength.Long);
                toast.SetGravity(Gravity.GetAbsoluteGravity(GravityFlags.Center, GravityFlags.Center), 0, 0);
                toast.Show();
            }
        }

        void Error_Button_Click(String s)
        {

            if (!dialogShown)
            {
                if (s != null)
                {
                    this.dialogShown = true;
                    Android.Content.Res.Resources res = this.Resources;
                    AlertDialog.Builder builder;
                    builder = new AlertDialog.Builder(this);
                    builder.SetTitle(string.Format(res.GetString(Resource.String.claimServiceProviderErrorMsgTitle)));
                    builder.SetMessage(s);
                    builder.SetCancelable(true);
                    builder.SetPositiveButton(string.Format(res.GetString(Resource.String.OK)), delegate
                    {
                        dialogShown = false;
                    });
                    builder.Show();
                }
            }
        }

        public void ShowNotEligibleForVisionClaimsMessage()
        {
            if (!dialogShown)
            {

                this.RunOnUiThread(() =>
                {
                    this.dialogShown = true;
                    Android.Content.Res.Resources res = this.Resources;
                    AlertDialog.Builder builder;
                    builder = new AlertDialog.Builder(this);
                    builder.SetTitle(string.Format(res.GetString(Resource.String.claimServiceProviderErrorMsgTitle)));
                    builder.SetMessage(string.Format(res.GetString(Resource.String.notEligibleVisionClaimsMessage)));
                    builder.SetCancelable(true);
                    builder.SetPositiveButton(string.Format(res.GetString(Resource.String.OK)), delegate
                    {
                        dialogShown = false;
                    });
                    builder.Show();

                });
            }
        }
        public void Show(MvxFragment fragment)
        {
            Show(fragment, true);
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
                    FragmentManager.PopBackStack(null,PopBackStackFlags.Inclusive);
                    ft.AddToBackStack(fragment.Class.Name);
                }
                else if (backstackBehaviour == BackstackTypes.ADD)
                {
                    ft.AddToBackStack(fragment.Class.Name);
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

        void unSavedDataPopup()
        {

            if (!dialogShown)
            {

                this.dialogShown = true;
                Android.Content.Res.Resources res = this.Resources;
                AlertDialog.Builder builder;
                builder = new AlertDialog.Builder(this);
                builder.SetTitle(string.Format(res.GetString(Resource.String.claimUnsavedMsgTitle)));
                builder.SetMessage(string.Format(res.GetString(Resource.String.claimUnsavedMsg)));
                builder.SetCancelable(true);
                builder.SetNegativeButton(string.Format(res.GetString(Resource.String.cancel)), delegate
                {
                    dialogShown = false;
                    return;
                });
                builder.SetPositiveButton(string.Format(res.GetString(Resource.String.leaveBtn)), delegate
                {
                    dialogShown = false;
                    base.OnBackPressed();
                    
                });
                builder.Show();
            }
        }
    }
}
