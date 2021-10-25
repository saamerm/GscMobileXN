using Android.Content;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using MobileClaims.Droid.Views;
using MvvmCross;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Exceptions;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MobileClaims.Core.Services.Requests;
using Newtonsoft.Json;

namespace MobileClaims.Droid
{
    /// <summary>
    /// Implemented by the activity registered to the IMultiRegionPresenter.  Will show views in regions specitied by their RegionAttribute.
    /// </summary>
    public interface IMultiRegionHost
    {
        /// <summary>
        /// Shows the MvxFragment view in the region specified by its RegionAttribute.
        /// </summary>
        /// <param name="fragment"></param>
        void Show(MvxFragment fragment);

        /// <summary>
        /// Closes the view associated with the given ViewModel.
        /// </summary>
        /// <param name="viewModel"></param>
        void CloseViewModel(IMvxViewModel viewModel);

        /// <summary>
        /// Closes all active views.
        /// </summary>
        void CloseAll();
    }

    public interface IMultiRegionPresenter : IMvxAndroidViewPresenter
    {
        /// <summary>
        /// Allows the IMultiRegionHost to register itself with the presenter.  This is required as the presenter is constructed before the IMultiRegionHost activity and
        /// the activity is created outside of the Mvx IoC.
        /// </summary>
        void RegisterMultiRegionHost(IMultiRegionHost host);
    }

    /// <summary>
    /// Presenter that displays MvxFragment views in specified regions of an associated IMultiRegionHost.  Views should be tagged with a RegionAttribute to indicate
    /// the region for display.
    /// </summary>
    public class MultiRegionPresenter : MvxAndroidViewPresenter, IMultiRegionPresenter
    {
        private readonly Stack<IMultiRegionHost> _hosts = new Stack<IMultiRegionHost>();
        private object _sync = new object();

        public MultiRegionPresenter(IEnumerable<Assembly> assemblies)
            : base(assemblies)
        {
        }

        /// <summary>
        /// Allows the IMultiRegionHost to regist itself with the presenter.  This is required as the presenter is constructed before the IMultiRegionHost activity and
        /// the activity is created outside of the Mvx IoC.
        /// </summary>
        /// <param name="host"></param>
        public void RegisterMultiRegionHost(IMultiRegionHost host)
        {
            _hosts.Push(host);
        }

        /// <summary>
        /// Shows the MvxViewModelRequest.  If the associated view is tagged with a RegionAttribute it is shown in the IMultiRegionHost.  If not, it is shown using the
        /// default Android presentation.
        /// </summary>
        /// <param name="request"></param>
        public override Task<bool> Show(MvxViewModelRequest request)
        {
            lock (_sync)
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
                    requestedValue = string.Empty;
                    rehydrationService.Rehydrating = false;
                }

                if (request.ViewModelType == typeof(LoginViewModel))
                {
                    ClearStackToFirst();

                    if (_hosts.Count > 0)
                    {
                        _hosts.Last().CloseAll();
                    }

                    var mvxAndroidViewModelRequestTranslator = Mvx.IoCProvider.Resolve<IMvxAndroidViewModelRequestTranslator>();
                    var intentFor = mvxAndroidViewModelRequestTranslator.GetIntentFor(request);
                    intentFor.SetFlags(ActivityFlags.ClearTop);

                    CurrentActivity.StartActivity(intentFor);
                }
                else if (request.ViewModelType == typeof(DashboardViewModel))
                {
                    var mvxAndroidViewModelRequestTranslator = Mvx.IoCProvider.Resolve<IMvxAndroidViewModelRequestTranslator>();
                    var intentFor = mvxAndroidViewModelRequestTranslator.GetIntentFor(request);
                    intentFor.SetFlags(ActivityFlags.ClearTop);
                    if (_hosts.Count > 0)
                    {
                        _hosts.Last().CloseAll();
                    }

                    ClearStackToFirst();

                    CurrentActivity.StartActivity(intentFor);
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
                    var mvxAndroidViewModelRequestTranslator = Mvx.IoCProvider.Resolve<IMvxAndroidViewModelRequestTranslator>();
                    var intentFor = mvxAndroidViewModelRequestTranslator.GetIntentFor(request);
                    intentFor.SetFlags(ActivityFlags.SingleTop);

                    CurrentActivity.StartActivity(intentFor);
                }
                else if (request.ViewModelType == typeof(WebAgreementViewModel)
                         || request.ViewModelType == typeof(ChangeForLifeViewModel)
                         || request.ViewModelType == typeof(SupportCenterViewModel))
                {
                    var mvxAndroidViewModelRequestTranslator = Mvx.IoCProvider.Resolve<IMvxAndroidViewModelRequestTranslator>();
                    var intentFor = mvxAndroidViewModelRequestTranslator.GetIntentFor(request);
                    intentFor.SetFlags(ActivityFlags.NoHistory);

                    CurrentActivity.StartActivity(intentFor);
                }
                else if (request.ViewModelType == typeof(FindHealthProviderViewModel))
                {
                    var mvxAndroidViewModelRequestTranslator = Mvx.IoCProvider.Resolve<IMvxAndroidViewModelRequestTranslator>();
                    var intentFor = mvxAndroidViewModelRequestTranslator.GetIntentFor(request);
                    intentFor.SetFlags(ActivityFlags.SingleTop);

                    if (request.ParameterValues != null && request.ParameterValues.ContainsKey(FindHealthProviderViewModel.ProviderSearchKey))
                    {
                        bool parseSuccess = Enum.TryParse(request.ParameterValues[FindHealthProviderViewModel.ProviderSearchKey], out ProvidersId providerId);
                        if (parseSuccess)
                        {
                            intentFor.PutExtra(FindHealthProviderViewModel.ProviderSearchKey, JsonConvert.SerializeObject(providerId));
                        }
                    }
                    CurrentActivity.StartActivity(intentFor);
                }
                else if (request.ViewModelType == typeof(ClaimSubmissionTypeViewModel) && request.PresentationValues == null)
                {
                    var mvxAndroidViewModelRequestTranslator = Mvx.IoCProvider.Resolve<IMvxAndroidViewModelRequestTranslator>();
                    var intentFor = mvxAndroidViewModelRequestTranslator.GetIntentFor(request);
                    CurrentActivity.StartActivity(intentFor);
                }
                else if (request.ViewModelType == typeof(ClaimSubmissionTypeViewModel)
                         && string.IsNullOrWhiteSpace(requestedValue))
                {
                    var mvxAndroidViewModelRequestTranslator = Mvx.IoCProvider.Resolve<IMvxAndroidViewModelRequestTranslator>();
                    var intentFor = mvxAndroidViewModelRequestTranslator.GetIntentFor(request);
                    CurrentActivity.StartActivity(intentFor);
                }
                else if (request.ViewModelType == typeof(ClaimSubmissionTypeViewModel) &&
                         string.Equals(requestedValue, NavigationRequestTypes.Rehydration))
                {
                    var mvxAndroidViewModelRequestTranslator = Mvx.IoCProvider.Resolve<IMvxAndroidViewModelRequestTranslator>();
                    var intentFor = mvxAndroidViewModelRequestTranslator.GetIntentFor(request);
                    CurrentActivity.StartActivity(intentFor);
                }
                else
                {
                    var fragment = CreateView(request);

                    if (IsFragmentAlreadyPresented(fragment))
                    {
                        return Task.FromResult(true);
                    }

                    if (_hosts?.Count > 0 && fragment.HasRegionAttribute())
                    {
                        // view has region attribute - show in the fragment host
                        ((FragmentActivityBase) _hosts.First()).CustomFragmentsBackStack.Add(fragment);
                        _hosts.First().Show(fragment);
                    }
                    else
                    {
                        // view has no region attribute - use default MvxAndroidViewPresenter Show implementation
                        base.Show(request);
                    }
                }

                return Task.FromResult(true);
            }
        }

        private bool IsFragmentAlreadyPresented(MvxFragment fragment)
        {
            if (((FragmentActivityBase) _hosts.First()).CustomFragmentsBackStack.LastOrDefault()?.GetType() ==
                fragment.GetType())
            {
                return true;
            }

            return false;
        }

        public override Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            if (_hosts?.Count > 0)
            {
                if (hint is MvxClosePresentationHint presentationHint)
                {
                    if (presentationHint.ViewModelToClose?.GetType() == ((MvxFragmentActivity) _hosts.First()).ViewModel.GetType())
                    {
                        _hosts.First().CloseViewModel(presentationHint.ViewModelToClose);
                        _hosts.Pop();
                    }
                    else if (presentationHint.ViewModelToClose == null)
                    {
                        ClearStackToFirst();
                    }
                    else
                    {
                        _hosts.First().CloseViewModel(presentationHint.ViewModelToClose);

                        if (((FragmentActivityBase) _hosts.First()).CustomFragmentsBackStack.Last().ViewModel.GetType() == presentationHint.ViewModelToClose.GetType())
                        {
                            ((FragmentActivityBase) _hosts.First()).CustomFragmentsBackStack.Remove(
                                ((FragmentActivityBase) _hosts.First()).CustomFragmentsBackStack.Last());
                        }
                    }
                }
            }

            return base.ChangePresentation(hint);
        }

        /// <summary>
        /// Creates a MvxFragment view initialized with the associated ViewModel.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        // modified from Cirrious.MvvmCross.Wpf.Views.MvxWpfViewsContainer
        public MvxFragment CreateView(MvxViewModelRequest request)
        {
            var viewFinder = Mvx.IoCProvider.Resolve<IMvxViewsContainer>();

            var viewType = viewFinder.GetViewType(request.ViewModelType);
            if (viewType == null)
            {
                throw new MvxException("View Type not found for " + request.ViewModelType);
            }

            // , request
            var viewObject = Activator.CreateInstance(viewType);
            if (viewObject == null)
            {
                throw new MvxException("View not loaded for " + viewType);
            }

            var fragment = viewObject as MvxFragment;
            if (fragment == null)
            {
                throw new MvxException("Loaded View is not a MvxFragment " + viewType);
            }

            var viewModelLoader = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();

            fragment.ViewModel = viewModelLoader.LoadViewModel(request, null);

            return fragment;
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