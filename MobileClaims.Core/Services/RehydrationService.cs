using MobileClaims.Core.Messages;
using MobileClaims.Core.ViewModels;
using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Plugin.File;
using MvvmCross.Plugin.Messenger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MobileClaims.Core.Services
{
    public class DeserializableRehydrationService
    {

    }

    public class RehydrationService : IRehydrationService
    {
        private IMvxFileStore _filestore;
        private object _sync = new object();
        private readonly IMvxLog _log;
        private readonly IMvxMessenger _messenger;
        private readonly MvxSubscriptionToken _removeClearClaimServiceProviderVM;
        private readonly MvxSubscriptionToken _removeClaimServiceProviderSearchVM;
        private readonly MvxSubscriptionToken _removeClaimServiceProviderSearchResultsVM;
        private readonly MvxSubscriptionToken _removeClaimParticipantsVM;
        private readonly MvxSubscriptionToken _removeClaimDetailsVM;
        private readonly MvxSubscriptionToken _removeClaimTreatmentListVM;
        private readonly MvxSubscriptionToken _removeClaimSubmitTCVM;
        private readonly MvxSubscriptionToken _removeClaimSubmissionConfirmationVM;

        public RehydrationService(IMvxFileStore filestore,
            IMvxLog log)
        {
            _filestore = filestore;
            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            _log = log;

            // TODO: Are these subscription handlers even useful?
            // TODO: As we only perform any action if the current device type is WP or Windows
            _removeClearClaimServiceProviderVM = _messenger.Subscribe<ClearClaimServiceProviderViewRequested>(message =>
            {
                var deviceservice = Mvx.IoCProvider.Resolve<IDeviceService>();
                var claimservice = Mvx.IoCProvider.Resolve<IClaimService>();

                if (deviceservice.CurrentDevice == GSCHelper.OS.WP || deviceservice.CurrentDevice == GSCHelper.OS.Windows)
                {
                    BusinessProcess.Remove(typeof(ClaimServiceProvidersViewModel));
                    Save();
                }
            });
            _removeClaimServiceProviderSearchVM = _messenger.Subscribe<ClearClaimServiceProviderSearchViewRequested>(message =>
            {
                var deviceservice = Mvx.IoCProvider.Resolve<IDeviceService>();
                var claimservice = Mvx.IoCProvider.Resolve<IClaimService>();

                if (deviceservice.CurrentDevice == GSCHelper.OS.WP || deviceservice.CurrentDevice == GSCHelper.OS.Windows)
                {
                    Save();
                }
            });
            _removeClaimServiceProviderSearchResultsVM = _messenger.Subscribe<ClearClaimServiceProviderSearchResultsViewRequested>(message =>
            {
                var deviceservice = Mvx.IoCProvider.Resolve<IDeviceService>();
                var claimservice = Mvx.IoCProvider.Resolve<IClaimService>();
                if (deviceservice.CurrentDevice == GSCHelper.OS.WP || deviceservice.CurrentDevice == GSCHelper.OS.Windows)
                {
                    BusinessProcess.Remove(typeof(ClaimServiceProviderSearchResultsViewModel));
                    Save();
                }
            });
            _removeClaimParticipantsVM = _messenger.Subscribe<ClearClaimParticipantsViewRequested>(message =>
            {
                var deviceservice = Mvx.IoCProvider.Resolve<IDeviceService>();
                var claimservice = Mvx.IoCProvider.Resolve<IClaimService>();
                if (deviceservice.CurrentDevice == GSCHelper.OS.WP || deviceservice.CurrentDevice == GSCHelper.OS.Windows)
                {
                    BusinessProcess.Remove(typeof(ClaimParticipantsViewModel));
                    Save();
                }
            });
            _removeClaimDetailsVM = _messenger.Subscribe<ClearClaimDetailsViewRequested>(message =>
            {
                var deviceservice = Mvx.IoCProvider.Resolve<IDeviceService>();
                var claimservice = Mvx.IoCProvider.Resolve<IClaimService>();
                if (deviceservice.CurrentDevice == GSCHelper.OS.WP || deviceservice.CurrentDevice == GSCHelper.OS.Windows)
                {
                    BusinessProcess.Remove(typeof(ClaimDetailsViewModel));
                    Save();
                }
            });
            _removeClaimTreatmentListVM = _messenger.Subscribe<ClearClaimTreatmentDetailsListViewRequested>(message =>
            {
                var deviceservice = Mvx.IoCProvider.Resolve<IDeviceService>();
                var claimservice = Mvx.IoCProvider.Resolve<IClaimService>();
                if (deviceservice.CurrentDevice == GSCHelper.OS.WP || deviceservice.CurrentDevice == GSCHelper.OS.Windows)
                {
                    BusinessProcess.Remove(typeof(ClaimTreatmentDetailsListViewModel));
                    Save();
                }
            });
            _removeClaimSubmitTCVM = _messenger.Subscribe<ClearClaimSubmitTermsAndConditionsViewRequested>(message =>
            {
                var deviceservice = Mvx.IoCProvider.Resolve<IDeviceService>();
                var claimservice = Mvx.IoCProvider.Resolve<IClaimService>();
                if (deviceservice.CurrentDevice == GSCHelper.OS.WP || deviceservice.CurrentDevice == GSCHelper.OS.Windows)
                {
                    BusinessProcess.Remove(typeof(ClaimSubmitTermsAndConditionsViewModel));
                    Save();
                }
            });
            _removeClaimSubmissionConfirmationVM = _messenger.Subscribe<ClearClaimSubmissionConfirmationViewRequested>(message =>
            {
                var deviceservice = Mvx.IoCProvider.Resolve<IDeviceService>();
                var claimservice = Mvx.IoCProvider.Resolve<IClaimService>();
                if (deviceservice.CurrentDevice == GSCHelper.OS.WP || deviceservice.CurrentDevice == GSCHelper.OS.Windows)
                {
                    BusinessProcess.Remove(typeof(ClaimSubmissionConfirmationViewModel));
                    Save();
                }
            });
        }

        public bool HackingRehydration { get; set; }

        public int CurrentRehydrationIndex { get; set; } = -1;

        public bool Rehydrating { get; set; } = false;

        /// <summary>
        /// The name of the view model that represents the entry point for a business process.  For instance, ClaimSubmissionTypeViewModel is the first VM in the Claims process
        /// </summary>
        public string ProcessEntryPoint { get; set; }

        /// <summary>
        /// The VMs shown so far in the business process
        /// </summary>

        public List<Type> BusinessProcess { get; set; } = new List<Type>();

        /// <summary>
        /// Writes the process stack to local storage so we can rehydrate the navigation stack later
        /// </summary>
        public void Save()
        {
            lock (_sync)
            {
                var json = JsonConvert.SerializeObject(this);
                if (_filestore == null)
                {
                    _filestore = Mvx.IoCProvider.Resolve<IMvxFileStore>();
                }

                if (_filestore.Exists("resume.dat"))
                {
                    _filestore.DeleteFile("resume.dat");
                }

                _filestore.WriteFile("resume.dat", json);
            }
        }

        /// <summary>
        /// Reloads the process stack from local storage
        /// </summary>
        public void Reload()
        {
            lock (_sync)
            {
                _filestore.TryReadTextFile("resume.dat", out var json);
                if (string.IsNullOrEmpty(json))
                {
                    return;
                }
                try
                {
                    var _somethingelse = JsonConvert.DeserializeObject<dynamic>(json);
                    var _loadthis = JsonConvert.DeserializeObject<RehydrationService>(json);
                    BusinessProcess = _loadthis.BusinessProcess;
                    ProcessEntryPoint = _loadthis.ProcessEntryPoint;
                }
                catch (Exception ex)
                {
                    _log.Error(ex, ex.Message);
                }
            }
        }

        public void ClearData()
        {
            ProcessEntryPoint = string.Empty;
            BusinessProcess.Clear();
            Save();
        }

        public void ClearFromStartingPoint(Type start)
        {
            int begin = BusinessProcess.IndexOf(start);
            if (begin > -1)
            {
                for (int i = BusinessProcess.Count - 1; i > begin; i--)
                {
                    BusinessProcess.RemoveAt(i);
                    Save();
                }
            }

            Reload();
            var v = BusinessProcess;
        }
    }
}