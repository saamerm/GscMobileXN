using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MobileClaims.Core.Constants;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Services;
using MobileClaims.Core.Util;
using MobileClaims.Core.ViewModelParameters;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Newtonsoft.Json;

namespace MobileClaims.Core.ViewModels
{
    public class AuditListViewModel : ViewModelBase
    {
        private bool _hasAnyClaims;
        private bool _noPhoneAlteration;
        private bool _phoneBusy;
        private TextAlteration _phoneNumber;

        private readonly IClaimExService _claimService;
        private readonly IUserDialogs _userDialogService;

        public event EventHandler ClaimsFetched;
        public string Title => Resource.AuditListPageTitle;
        public string PromptTextTop => Resource.AuditListInstrunction;
        public string ClaimSubmissionDateLabel => Resource.SubmissionDateLabel;
        public string ClaimDueDateLabel => Resource.AuditDueDateLabel;
        public string PromptTextBottom => Resource.AuditListNotes;
        public string NoAuditMessage => Resource.NoAuditMessage;

        public ObservableCollection<MyAlertsAuditClaim> AuditClaims { get; set; }

        public bool HasAnyClaims
        {
            get => _hasAnyClaims;
            set => SetProperty(ref _hasAnyClaims, value);
        }

        public bool NoPhoneAlteration
        {
            get => _noPhoneAlteration;
            set
            {
                _noPhoneAlteration = value;
                RaisePropertyChanged(() => NoPhoneAlteration);
            }
        }

        public bool PhoneBusy
        {
            get => _phoneBusy;
            set
            {
                if (_phoneBusy != value)
                {
                    _phoneBusy = value;
                    RaisePropertyChanged(() => PhoneBusy);
                }
            }
        }

        public TextAlteration PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                RaisePropertyChanged(() => PhoneNumber);
            }
        }

        public MvxCommand<MyAlertsAuditClaim> AuditSelectedCommand { get; }

        public AuditListViewModel(IClaimExService claimExService, ILoginService loginservice,
            IUserDialogs userDialogs)
        {
            _claimService = claimExService;
            _userDialogService = userDialogs;
            _loginservice = loginservice;

            AuditClaims = new ObservableCollection<MyAlertsAuditClaim>();
            AuditSelectedCommand = new MvxCommand<MyAlertsAuditClaim>(ExecuteAuditSelected);
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            _userDialogService.ShowLoading(Resource.Loading);

            try
            {
                await GetAuditClaims();
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                await LogoutWithAlertMessageAsync();
            }
            catch (Exception)
            {
                AuditClaims = new ObservableCollection<MyAlertsAuditClaim>();
                HasAnyClaims = AuditClaims.Any();
                _userDialogService.HideLoading();
            }
            finally
            {
                _userDialogService.HideLoading();
            }
        }

        private async Task GetAuditClaims()
        {
            var auditClaims = await _claimService.GetAuditClaimsAsync(_loginservice.CurrentPlanMemberID);
            foreach (var item in auditClaims)
            {
                AuditClaims.Add(GetMyAlertsAuditClaim(item));
            }

            HasAnyClaims = AuditClaims.Any();
            ClaimsFetched?.Invoke(this, EventArgs.Empty);
        }

        private MyAlertsAuditClaim GetMyAlertsAuditClaim(ClaimAudit item)
        {
            return new MyAlertsAuditClaim
            {
                ClaimFormId = item.ClaimFormID,
                ClaimSubmissionDate = item.SubmissionDate.ToString("d"),
                ClaimDueDate = item.DueDate.ToString("d"),
                ServiceDescription = item.BenefitTypeDescr,
                ClaimActionState = ClaimActionState.Audit
            };
        }

        private async void ExecuteAuditSelected(MyAlertsAuditClaim claimAudit)
        {
            IClaimSummaryProperties uploadable = (IClaimSummaryProperties)UploadFactory.Create(claimAudit.ClaimActionState, nameof(ClaimSummaryViewModel));

            var recentDashboardClaim = new DashboardRecentClaim
            {
                ClaimFormId = claimAudit.ClaimFormId
            };

            await ShowViewModel<AuditClaimSummaryViewModel, AuditClaimSummaryViewModelParameters>(new AuditClaimSummaryViewModelParameters(recentDashboardClaim, uploadable));
        }
    }
}