using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MvvmCross;
using MvvmCross.Platforms.Android.Views.Fragments;
using Plugin.Permissions;
using System.Linq;
using MvvmCross.ViewModels;
using Android.Content;
using MobileClaims.Core.Services.Requests;
using MobileClaims.Droid.Presenter;
using Newtonsoft.Json;

namespace MobileClaims.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden, WindowSoftInputMode = SoftInput.StateAlwaysHidden | SoftInput.AdjustPan)]
    public class FindHealthProviderView : ActivityBase, IMultiRegionHost
    {
        private const int InitialFragmentsNumber = 3;

        private MapView _fragmentWithMap;
        private FindHealthProviderViewModel _viewModel;

        private readonly int[] _validRegionIds =
        {
            Resource.Id.full_region,
            Resource.Id.right_region,
            Resource.Id.left_region,
            Resource.Id.nav_region,
            Resource.Id.search_region,
            Resource.Id.phone_main_region,
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.FindHealthProviderLayout);
            base.OnCreate(savedInstanceState);
            _viewModel = ViewModel as FindHealthProviderViewModel;
        }

        public override void OnBackPressed()
        {
            if (CustomFragmentsBackStack.Count > InitialFragmentsNumber)
            {
                var lastFragment = FragmentManager.Fragments.Last();

                RemoveFragmentWithCustomAnimation(lastFragment);
                CustomFragmentsBackStack.RemoveAt(CustomFragmentsBackStack.Count - 1);
                RefreshRefineSearchView(lastFragment);
            }
            else
            {
                _viewModel.GoBackCommand.Execute(null);
                base.OnBackPressed();
            }

        }

        private void RefreshRefineSearchView(Fragment lastPopFragment)
        {
            if (!(lastPopFragment is HealthProviderTypeListView))
            {
                return;
            }

            var refineSearchFragment =
                FragmentManager.Fragments.SingleOrDefault(x => x.GetType() == typeof(RefineSearchView));
            var refreshTransaction = FragmentManager.BeginTransaction();
            refreshTransaction.Detach(refineSearchFragment);
            refreshTransaction.Attach(refineSearchFragment);
            refreshTransaction.Commit();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            var presenter = Mvx.IoCProvider.Resolve<IMultiRegionPresenter>();
            presenter.RegisterMultiRegionHost(this);

            DisplayMap();

            DisplaySearch();

            presenter.ShowBottomNavigation();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            string stringExtra = intent.GetStringExtra(FindHealthProviderViewModel.ProviderSearchKey);
            if (!string.IsNullOrEmpty(stringExtra))
            {
                var providersId = JsonConvert.DeserializeObject<ProvidersId>(stringExtra);
                if (providersId != null)
                {
                    _viewModel.onAndroidPharmacyOrFavourite(providersId);
                }
            }
            else
            {
                _viewModel.onAndroidHealthProvider();
            }
        }

        private void DisplayMap()
        {
            _fragmentWithMap = new MapView
            {
                ViewModel = ViewModel
            };

            Show(_fragmentWithMap, false);
            CustomFragmentsBackStack.Add(_fragmentWithMap);
        }

        private void DisplaySearch()
        {
            var fragmentWithSearch = new SearchHealthProviderView
            {
                ViewModel = ((FindHealthProviderViewModel)ViewModel).SearchViewModel
            };

            Show(fragmentWithSearch, false);
            CustomFragmentsBackStack.Add(fragmentWithSearch);
        }

        public void Show(MvxFragment fragment)
        {
            if (IsAlreadyInBackStack(fragment))
            {
                return;
            }

            Show(fragment, true);
        }

        protected void Show(MvxFragment fragment, bool addToBackStack)
        {
            var regionResourceId = fragment.GetRegionId();
            var hidesNav = fragment.GetHidesNav();

            var navRegion = FindViewById<FrameLayout>(Resource.Id.nav_region);
            navRegion.Visibility = hidesNav ? ViewStates.Gone : ViewStates.Visible;

            var ft = FragmentManager.BeginTransaction();

            int container;
            switch (regionResourceId)
            {
                case Resource.Id.nav_region:
                    container = Resource.Id.nav_region;
                    break;
                case Resource.Id.search_region:
                    container = Resource.Id.search_region;
                    break;
                case Resource.Id.full_region:
                    container = Resource.Id.full_region;
                    break;
                case Resource.Id.right_region:
                    container = Resource.Id.right_region;
                    break;
                case Resource.Id.left_region:
                    container = Resource.Id.left_region;
                    break;
                default:
                    container = Resource.Id.left_region;
                    break;
            }

            if (addToBackStack)
            {
                SetTransactionCustomAnimations(fragment, ft);
                ft.Add(container, fragment, fragment.Tag);
            }
            else
            {
                //ft.SetCustomAnimations(Resource.Animation.enter, Resource.Animation.exit);
                ft.Replace(container, fragment);
            }

            ft.CommitAllowingStateLoss();
        }

        private bool IsAlreadyInBackStack(MvxFragment fragment)
        {
            bool contains = false;

            var foundOnCustomStack = CustomFragmentsBackStack.SingleOrDefault(x => x.GetType() == fragment.GetType());
            var foundOnBackStack = FragmentManager.Fragments.SingleOrDefault(x => x.GetType() == fragment.GetType());

            if (foundOnCustomStack != null && foundOnBackStack != null)
            {
                if (foundOnCustomStack == CustomFragmentsBackStack.Last())
                {
                    foundOnCustomStack.ViewModel = fragment.ViewModel;
                    return true;
                }

                BringExistingFragmentToTop(fragment, foundOnBackStack, foundOnCustomStack);

                contains = true;
            }

            return contains;
        }

        private void BringExistingFragmentToTop(MvxFragment fragment, Fragment foundOnBackStack, MvxFragment foundOnCustomStack)
        {
            if (fragment is MainNavigationView)
            {
                return;
            }

            var ftr = FragmentManager.BeginTransaction();
            if (fragment is RefineSearchView)
            {
                ftr.SetCustomAnimations(Resource.Animation.slide_in_right, Resource.Animation.disappear);
            }
            else if (fragment is ServiceDetailsListView)
            {
                ftr.SetCustomAnimations(Resource.Animation.slide_in_up, Resource.Animation.disappear);
            }

            ftr.Remove(foundOnBackStack);
            ftr.Commit();
            CustomFragmentsBackStack.Remove(foundOnCustomStack);

            var fta = FragmentManager.BeginTransaction();
            if (fragment is RefineSearchView)
            {
                fta.SetCustomAnimations(Resource.Animation.slide_in_right, Resource.Animation.disappear);
            }
            else if (fragment is ServiceDetailsListView)
            {
                fta.SetCustomAnimations(Resource.Animation.slide_in_up, Resource.Animation.disappear);
            }

            fta.Add(Resource.Id.left_region, foundOnCustomStack, foundOnCustomStack.Class.Name);
            fta.AddToBackStack(foundOnCustomStack.Class.Name);
            fta.Commit();
            CustomFragmentsBackStack.Add(fragment);
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
            fragment = FragmentManager.FindFragmentById(Resource.Id.search_region) as MvxFragment;
            if (fragment != null)
            {
                CloseFragment(fragment);
            }
            fragment = FragmentManager.FindFragmentById(Resource.Id.full_region) as MvxFragment;
            if (fragment != null)
            {
                CloseFragment(fragment);
            }
        }

        private MvxFragment GetActiveFragmentForViewModel(IMvxViewModel viewModel)
        {
            foreach (var regionId in _validRegionIds)
            {
                MvxFragment fragment = (MvxFragment)FragmentManager.FindFragmentById(regionId);
                if (fragment?.ViewModel != null && fragment.ViewModel.GetType() == viewModel.GetType())
                {
                    return fragment;
                }
            }

            return null;
        }

        //this is called when Close(this) is invoked in VM
        private void CloseFragment(MvxFragment fragment)
        {
            if (CustomFragmentsBackStack.Count > InitialFragmentsNumber)
            {
                CustomFragmentsBackStack.RemoveAt(CustomFragmentsBackStack.Count - 1);
                RemoveFragmentWithCustomAnimation(fragment);

                RefreshRefineSearchView(fragment);
            }
        }

        private void RemoveFragmentWithCustomAnimation(Fragment fragment)
        {
            var ft = FragmentManager.BeginTransaction();
            SetTransactionCustomAnimations(fragment, ft);
            ft.Remove(fragment);
            ft.Commit();
        }

        private static void SetTransactionCustomAnimations(Fragment fragment, FragmentTransaction ft)
        {
            switch (fragment)
            {
                case RefineSearchView _:
                    //ft.SetCustomAnimations(Resource.Animation.slide_in_right, Resource.Animation.slide_out_left);
                    break;
                case HealthProviderTypeListView _:
                    //ft.SetCustomAnimations(Resource.Animation.slide_in_left, Resource.Animation.slide_out_left);
                    break;
                case ProviderDetailsInformationView _:
                case ServiceDetailsListView _:
                    //ft.SetCustomAnimations(Resource.Animation.slide_in_up, Resource.Animation.slide_out_down);
                    break;
                default:
                    //ft.SetCustomAnimations(Resource.Animation.enter, Resource.Animation.exit);
                    break;
            }
        }
    }
}