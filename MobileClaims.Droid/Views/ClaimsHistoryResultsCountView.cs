using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Interop;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using MobileClaims.Droid.Views.Fragments;
using MvvmCross;
using MvvmCross.Platforms.Android.Views.Fragments;
using System;
using MobileClaims.Droid.Presenter;
using MvvmCross.ViewModels;

namespace MobileClaims.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden, WindowSoftInputMode = SoftInput.StateAlwaysHidden | SoftInput.AdjustPan)]
    public class ClaimsHistoryResultsCountView : ActivityBase, IMultiRegionHost
    {
        private readonly int[] _validRegionIds = { Resource.Id.phone_main_region, Resource.Id.nav_region, Resource.Id.full_region };
        private bool dialogShown;
       
        private object _sync = new object();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.MainLayout);

            base.OnCreate(savedInstanceState);
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            var presenter = Mvx.IoCProvider.Resolve<IMultiRegionPresenter>();
            presenter.RegisterMultiRegionHost(this);

            var cvf = new ClaimsHistoryResultsCountFragment_
            {
                ViewModel = ViewModel
            };
            Show(cvf, true);

            CustomFragmentsBackStack.Add(cvf);

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
                    dialogShown = true;
                    Resources res = Resources;
                    AlertDialog.Builder builder;
                    builder = new AlertDialog.Builder(this);
                    builder.SetTitle(string.Format(res.GetString(Resource.String.claimServiceProviderErrorMsgTitle)));
                    builder.SetMessage(s);
                    builder.SetCancelable(true);
                    builder.SetPositiveButton(string.Format(res.GetString(Resource.String.OK)),
                        delegate { dialogShown = false; });
                    builder.Show();
                }
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

                var ft = FragmentManager.BeginTransaction();
                //ft.SetCustomAnimations(Resource.Animation.enter, Resource.Animation.exit, Resource.Animation.popenter, Resource.Animation.popexit);

                if (regionResourceId == Resource.Id.nav_region)
                {
                    ft.Replace(Resource.Id.nav_region, fragment);
                }
                else if (regionResourceId == Resource.Id.full_region)
                {
                    ft.Replace(Resource.Id.full_region, fragment);
                }
                else
                {
                    ft.Replace(Resource.Id.phone_main_region, fragment);
                }

                //MANAGE BACKSTACK BEHAVIOUR HERE
                int backstackBehaviour = fragment.GetBackstackBehaviour();

                if (backstackBehaviour == BackstackTypes.FIRST_ITEM)
                {
                    FragmentManager.PopBackStackImmediate(null, PopBackStackFlags.Inclusive);
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

        public override void OnBackPressed()
        {
            int backStackCount = FragmentManager.BackStackEntryCount;
            if (backStackCount == 1)
            {
                ((ClaimsHistoryResultsCountViewModel)this.ViewModel).GoBackCommand.Execute(null);
            }
            base.OnBackPressed();
        }

        private void CloseFragment(MvxFragment fragment)
        {
            var ft = FragmentManager.BeginTransaction();
            ft.Remove(fragment);
            ft.Commit();
        }
    }
}