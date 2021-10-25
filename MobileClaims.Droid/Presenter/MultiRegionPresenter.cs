using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.Requests;
using MobileClaims.Core.ViewModels;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using MobileClaims.Droid.Views;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Exceptions;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using Newtonsoft.Json;

namespace MobileClaims.Droid.Presenter
{
    /// <summary>
    ///     Presenter that displays MvxFragment views in specified regions of an associated IMultiRegionHost.  Views should be
    ///     tagged with a RegionAttribute to indicate
    ///     the region for display.
    /// </summary>
    public class MultiRegionPresenter : MvxAndroidViewPresenter, IMultiRegionPresenter
    {
        private readonly Stack<IMultiRegionHost> _hosts = new Stack<IMultiRegionHost>();

        private ActivityBase FirstActivityBase
        {
            get
            {
                if (_hosts == null || _hosts.Count == 0)
                {
                    return null;
                }

                if ((ActivityBase)_hosts.First() == null)
                {
                    return null;
                }

                return (ActivityBase)_hosts.First();
            }
        }

        public MultiRegionPresenter(IEnumerable<Assembly> assemblies)
            : base(assemblies)
        {
        }

        /// <summary>
        ///     Allows the IMultiRegionHost to regist itself with the presenter.  This is required as the presenter is constructed
        ///     before the IMultiRegionHost activity and
        ///     the activity is created outside of the Mvx IoC.
        /// </summary>
        /// <param name="host"></param>
        public void RegisterMultiRegionHost(IMultiRegionHost host)
        {
            _hosts.Push(host);
        }

        public void ShowBottomNavigation()
        {
            var mvxRequest = MvxViewModelRequest.GetDefaultRequest(typeof(MainNavigationViewModel));

            var viewType = GetViewType(mvxRequest.ViewModelType);

            if (IsFragmentAlreadyPresented(viewType))
            {
                return;
            }

            var bottomNavFragment = CreateView(viewType);
            // custom view implementation - loading view model handled manually
            LoadFragmentViewModel(bottomNavFragment, mvxRequest);

            if (_hosts?.Count > 0 && bottomNavFragment.HasRegionAttribute())
            {
                // bottom navigation has region attribute - show in the fragment host
                ((ActivityBase)_hosts.First()).CustomFragmentsBackStack.Add(bottomNavFragment);
                _hosts.First().Show(bottomNavFragment);
            }
        }

        private void LoadFragmentViewModel(MvxFragment fragment, MvxViewModelRequest mvxRequest)
        {
            var viewModelLoader = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
            fragment.ViewModel = viewModelLoader.LoadViewModel(mvxRequest, null);
        }

        protected override Task<bool> ShowActivity(Type view, MvxActivityPresentationAttribute attribute, MvxViewModelRequest request)
        {
            SetRehydration(request);

            if (request.ViewModelType == typeof(LoginViewModel))
            {
                ClearStackToFirst();

                if (_hosts.Count > 0)
                {
                    _hosts.Last().CloseAll();
                }
            }
            else if (request.ViewModelType == typeof(DashboardViewModel))
            {
                if (_hosts.Count > 0)
                {
                    _hosts.Last().CloseAll();
                }

                ClearStackToFirst();
            }

            return base.ShowActivity(view, attribute, request);
        }

        private void SetRehydration(MvxViewModelRequest request)
        {
            var rehydrationService = Mvx.IoCProvider.Resolve<IRehydrationService>();

            if (request.PresentationValues != null
                && request.PresentationValues.TryGetValue(NavigationRequestTypes.RequestedBy, out var requestedValue)
                && requestedValue != null && requestedValue.Equals(NavigationRequestTypes.Rehydration))
            {
                rehydrationService.Rehydrating = true;
            }
            else
            {
                rehydrationService.Rehydrating = false;
            }
        }

        /// <summary>
        ///     Shows the fragment's MvxViewModelRequest.  If the associated view is tagged with a RegionAttribute it is shown in the
        ///     IMultiRegionHost.  If not, it is shown using the
        ///     default Android presentation.
        /// </summary>
        /// <param name="request"></param>
        protected override Task<bool> ShowFragment(Type viewType, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)
        {
            if (IsFragmentAlreadyPresented(viewType))
            {
                var existingFragment = FirstActivityBase.CustomFragmentsBackStack.LastOrDefault();
                _hosts.First().Show(existingFragment);

                return Task.FromResult(true);
            }

            var fragment = CreateView(viewType);

            // Assigning viewmodel to fragment- to enable custom fragment show support in new mvvmcross.
            // MvxNavigationService provides an already instantiated ViewModel here
            if (request is MvxViewModelInstanceRequest instanceRequest)
            {
                fragment.ViewModel = instanceRequest.ViewModelInstance;
            }

            if (_hosts?.Count > 0 && fragment.HasRegionAttribute())
            {
                // view has region attribute - show in the fragment host
                FirstActivityBase.CustomFragmentsBackStack.Add(fragment);
                _hosts.First().Show(fragment);
                return Task.FromResult(true);
            }

            // view has no region attribute
            // use default MvxAndroidViewPresenter Show implementation
            return base.ShowFragment(viewType, attribute, request);
        }

        protected override Bundle CreateActivityTransitionOptions(
            Intent intent,
            MvxActivityPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (request.ViewModelType == typeof(LoginViewModel)
                || request.ViewModelType == typeof(DashboardViewModel))
            {
                intent.SetFlags(ActivityFlags.ClearTop);
            }
            else if (request.ViewModelType == typeof(CardViewModel)
                || request.ViewModelType == typeof(ChooseSpendingAccountTypeViewModel)
                || request.ViewModelType == typeof(AuditListViewModel)
                || request.ViewModelType == typeof(DrugLookupModelSelectionViewModel)
                || request.ViewModelType == typeof(ChangeForLifeTermsAndConditionsViewModel)
                || request.ViewModelType == typeof(ChangeForLifeNoAccessViewModel)
                || request.ViewModelType == typeof(ConfirmationOfPaymentUploadViewModel)
                || request.ViewModelType == typeof(ActiveClaimDetailViewModel)
                || request.ViewModelType == typeof(ChooseClaimOrHistoryViewModel)
                || request.ViewModelType == typeof(TermsAndConditionsViewModel)
                || request.ViewModelType == typeof(SettingsViewModel)
                || request.ViewModelType == typeof(ClaimsHistoryResultsCountViewModel)
                || request.ViewModelType == typeof(SureHealthViewModel)
                || request.ViewModelType == typeof(ClaimSubmissionCompletedViewModel)
                || request.ViewModelType == typeof(EligibilityCheckTypesViewModel)
                || request.ViewModelType == typeof(ClaimDocumentsUploadViewModel))
            {
                intent.SetFlags(ActivityFlags.NewTask);
            }
            else if (request.ViewModelType == typeof(WebAgreementViewModel)
                    || request.ViewModelType == typeof(ChangeForLifeViewModel)
                    || request.ViewModelType == typeof(SupportCenterViewModel))
            {
                intent.SetFlags(ActivityFlags.NoHistory);
            }
            else if (request.ViewModelType == typeof(FindHealthProviderViewModel))
            {
                intent.SetFlags(ActivityFlags.NewTask);
                if (request.PresentationValues != null &&
                    request.PresentationValues.ContainsKey(FindHealthProviderViewModel.ProviderSearchKey))
                {
                    var parseSuccess =
                        Enum.TryParse(request.PresentationValues[FindHealthProviderViewModel.ProviderSearchKey],
                            out ProvidersId providerId);
                    if (parseSuccess)
                    {
                        intent.PutExtra(FindHealthProviderViewModel.ProviderSearchKey,
                            JsonConvert.SerializeObject(providerId));
                    }
                }
            }
            else if (request.ViewModelType == typeof(ClaimSubmissionTypeViewModel))
            {
                //no action derived from previous code, leaving on purpose in case some requirement turns in
            }

            return base.CreateActivityTransitionOptions(intent, attribute, request);
        }

        public override Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            if (hint is MvxClosePresentationHint presentationHint)
            {
                if (!(_hosts?.Count > 0))
                {
                    return Task.FromResult(true);
                }

                if (presentationHint.ViewModelToClose?.GetType() == ((MvxActivity)_hosts.First()).ViewModel.GetType())
                {
                    (_hosts.First() as MvxActivity)?.Finish();
                    //_hosts.First().CloseViewModel(presentationHint.ViewModelToClose);
                    _hosts.Pop();
                }
                else if (presentationHint.ViewModelToClose == null)
                {
                    ClearStackToFirst();
                }
                else
                {
                    if (FirstActivityBase != null && FirstActivityBase.CustomFragmentsBackStack.Any())
                    {
                        _hosts.First().CloseViewModel(presentationHint.ViewModelToClose);
                        if (FirstActivityBase.CustomFragmentsBackStack.Last().ViewModel
                                .GetType() == presentationHint.ViewModelToClose.GetType())
                        {
                            FirstActivityBase.CustomFragmentsBackStack.Remove(
                                FirstActivityBase.CustomFragmentsBackStack.Last());
                        }
                    }
                }
            }
            else
            {
                return base.ChangePresentation(hint);
            }

            return Task.FromResult(true);
        }

        private bool IsFragmentAlreadyPresented(Type typeofFragment)
        {
            var lastFragment = FirstActivityBase.CustomFragmentsBackStack.LastOrDefault();
            if (lastFragment != null)
            {
                return lastFragment.GetType() == typeofFragment;
            }
            return false;
        }

        /// <summary>
        ///     Creates a MvxFragment view initialized with the associated ViewModel.
        /// </summary>
        /// <param name="requestedViewType"></param>
        /// <returns></returns>
        // modified from Cirrious.MvvmCross.Wpf.Views.MvxWpfViewsContainer
        public MvxFragment CreateView(Type requestedViewType)
        {
            var view = Activator.CreateInstance(requestedViewType);
            if (view == null)
            {
                throw new MvxException("View not loaded for " + requestedViewType);
            }

            var fragment = view as MvxFragment;
            if (fragment == null)
            {
                throw new MvxException("Loaded View is not a MvxFragment " + requestedViewType);
            }

            return fragment;
        }

        private Type GetViewType(Type requestedViewModelType)
        {
            var viewFinder = Mvx.IoCProvider.Resolve<IMvxViewsContainer>();

            var viewType = viewFinder.GetViewType(requestedViewModelType);
            if (viewType == null)
            {
                throw new MvxException("View Type not found for " + requestedViewModelType);
            }
            return viewType;
        }

        private void ClearStackToFirst()
        {
            var hostsCountWithoutFirst = _hosts.Count - 1;
            for (var i = 0; i < hostsCountWithoutFirst; i++)
            {
                _hosts.Pop();
            }
        }
    }
}