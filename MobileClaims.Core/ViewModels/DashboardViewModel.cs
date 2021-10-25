using Acr.UserDialogs;
using MobileClaims.Core.Constants;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Extensions;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Models;
using MobileClaims.Core.Services;
using MobileClaims.Core.Util;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using EventArgs = System.EventArgs;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.ViewModelParameters;

namespace MobileClaims.Core.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly IClaimExService _claimService;
        private readonly IDeviceService _deviceService;
        private readonly IProviderLookupService _providerService;
        private readonly IParticipantService _participantService;
        private readonly IEligibilityService _eligibilityService;
        private readonly IMvxMessenger _messenger;
        private readonly IUserDialogs _userDialogs;
        private readonly ISpendingAccountService _spendingAccountService;
        private readonly IMenuService _menuService;

        private bool _hasAnyClaims;
        private bool _hasRecentClaims = true;
        private string _userName;

        private bool _isTabletDeviceType;
        private bool _shouldRibbonBeDisplayed;

        private MvxSubscriptionToken _eligibilityCheckCompleteToken;
        private MvxSubscriptionToken _eligibilityCheckFailureToken;

        private ObservableCollection<DashboardRecentClaim> _recentClaims;
        private Dictionary<DashboardEligibilityCheckType, IMvxAsyncCommand> _eligibilityCheckToCommandsMapping;

        public event EventHandler ClaimsFetched;
        public EventHandler DashboardEntered;

        public string ActionRequiredLabel => Resource.ActionRequiredLabel;
        public string WelcomeTitle => Resource.Welcome;
        public string RecentClaimsTitle => Resource.RecentClaims;
        public string MyBenefitsTitle => Resource.CheckCoverage;
        public string NoRecentClaimsLabel => Resource.NoRecentClaims;
        public string DentalRecallExam => Resource.DentalRecallExam;
        public string ChiropracticTreatment => Resource.ChiropracticTreatment;
        public string MassageTherapy => Resource.MassasgeTherapy;
        public string DrugsOnTheGo => Resource.DrugsOnTheGo;
        public string ViewAll => Resource.ViewAll;

        public IMvxAsyncCommand OpenAuditCommand { get; }
        public IMvxAsyncCommand ShowAllClaimsCommand { get; }
        public IMvxAsyncCommand ShowDentalRecallExamCommand { get; }
        public IMvxAsyncCommand ShowChiropracticCommand { get; }
        public IMvxAsyncCommand ShowMassageCommand { get; }
        public IMvxAsyncCommand ShowDrugsOnTheGoCommand { get; }
        public IMvxAsyncCommand ShowHcsaCommand { get; }
        public IMvxAsyncCommand ShowPsaCommand { get; }
        public MvxAsyncCommand<DashboardEligibilityCheckType> ShowEligibilityCheckCommand { get; }
        public MvxAsyncCommand<DashboardRecentClaim> SelectRecentClaimCommand { get; }

        public bool IsFeatureMarketingAvailable => false;

        private SpendingAccountType _hcsaAccountType;
        private SpendingAccountType _psaAccountType;

        public string HcsaTitle => HcsaAccountType?.ModelName.ToUpperInvariant();
        public string PsaTitle => _psaAccountType?.ModelName.ToUpperInvariant();
        public bool IsHcsaVisible => !string.IsNullOrWhiteSpace(HcsaTitle);
        public bool IsPsaVisible => !string.IsNullOrWhiteSpace(PsaTitle);

        public static DashboardViewModel CurrentDashboardInstance;

        public SpendingAccountType HcsaAccountType
        {
            get => _hcsaAccountType;
            set
            {
                SetProperty(ref _hcsaAccountType, value);
                RaisePropertyChanged(nameof(HcsaTitle));
                RaisePropertyChanged(nameof(IsHcsaVisible));
            }
        }
        public SpendingAccountType PsaAccountType
        {
            get => _psaAccountType;
            set
            {
                SetProperty(ref _psaAccountType, value);
                RaisePropertyChanged(nameof(PsaTitle));
                RaisePropertyChanged(nameof(IsPsaVisible));
            }
        }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public ObservableCollection<DashboardRecentClaim> RecentClaims
        {
            get => _recentClaims;
            set => SetProperty(ref _recentClaims, value);
        }

        public bool HasRecentClaims
        {
            get => _hasRecentClaims;
            set => SetProperty(ref _hasRecentClaims, value);
        }

        public bool HasAnyClaims
        {
            get => _hasAnyClaims;
            set
            {
                SetProperty(ref _hasAnyClaims, value);
                SetProperty(ref _hasRecentClaims, value, nameof(HasRecentClaims));
            }
        }

        public bool IsTabletDeviceType
        {
            get => _isTabletDeviceType;
            set
            {
                SetProperty(ref _isTabletDeviceType, value);
                _deviceService.IsTablet = IsTabletDeviceType;
            }
        }

        public bool ShouldRibbonBeDisplayed
        {
            get => _shouldRibbonBeDisplayed;
            set => SetProperty(ref _shouldRibbonBeDisplayed, value);
        }

        public DashboardViewModel(IProviderLookupService providerService, ILoginService loginService,
            IParticipantService participantService, IDeviceService deviceService, IEligibilityService eligibilityService,
            IMvxMessenger messenger, IClaimExService claimService, IUserDialogs userDialogs,
            ISpendingAccountService spendingAccountService, IMenuService menuService)
        {
            _providerService = providerService;
            _loginservice = loginService;
            _participantService = participantService;
            _deviceService = deviceService;
            _eligibilityService = eligibilityService;
            _messenger = messenger;
            _claimService = claimService;
            _userDialogs = userDialogs;
            _spendingAccountService = spendingAccountService;
            _menuService = menuService;

            RecentClaims = new ObservableCollection<DashboardRecentClaim>();
            
            OpenAuditCommand = new MvxAsyncCommand(ExecuteOpenAudit);
            ShowAllClaimsCommand = new MvxAsyncCommand(ExecuteShowAllClaims);
            ShowDentalRecallExamCommand = new MvxAsyncCommand(ExecuteShowDentalRecallExam);
            ShowChiropracticCommand = new MvxAsyncCommand(ExecuteShowChiropractorTreatment);
            ShowMassageCommand = new MvxAsyncCommand(ExecuteMassageTherapy);
            ShowDrugsOnTheGoCommand = new MvxAsyncCommand(ExecuteShowDrugsOnTheGo);
            ShowEligibilityCheckCommand = new MvxAsyncCommand<DashboardEligibilityCheckType>(ExecuteShowEligibilityCheck);
            ShowHcsaCommand = new MvxAsyncCommand(ExecuteShowHcsa);
            ShowPsaCommand = new MvxAsyncCommand(ExecuteShowPsa);
            SelectRecentClaimCommand = new MvxAsyncCommand<DashboardRecentClaim>(ExecuteSelectRecentClaim);
            PrepareMappings();
        }

        public override async Task Initialize()
        {
            CurrentDashboardInstance = this;
            _userDialogs.ShowLoading(Resource.Loading);
            base.Start();

            try
            {
                RefreshShouldRibbonBeDisplayed();

                if (_providerService.PreviousServiceProviders == null)
                {
                    await _providerService.GetPreviousServiceProviders(_loginservice.CurrentPlanMemberID);
                }

                if (_eligibilityService.EligibilityCheckTypes == null ||
                    !_eligibilityService.EligibilityCheckTypes.Any())
                {
                    await _eligibilityService.GetEligibilityCheckTypes(_loginservice.CurrentPlanMemberID);
                }

                if (_spendingAccountService.AccountTypes == null || !_spendingAccountService.AccountTypes.Any())
                {
                    await _spendingAccountService.GetSpendingAccountTypes(_participantService.PlanMember.PlanMemberID);
                }

                await GetRecentClaims();

                HcsaAccountType = _spendingAccountService.AccountTypes?.FirstOrDefault(x =>
                    x.ModelID.Equals("hcsa", StringComparison.OrdinalIgnoreCase));
                PsaAccountType = _spendingAccountService.AccountTypes?.FirstOrDefault(x =>
                    x.ModelID.Equals("psa", StringComparison.OrdinalIgnoreCase));

                UserName = SetFirstLetterToUpper(_participantService.PlanMember.FirstName);

                DashboardEntered -= OnDashboardEntered;
                DashboardEntered += OnDashboardEntered;

            }
            catch (Exception)
            {
                _userDialogs.HideLoading();
                _userDialogs.Alert(Resource.GenericErrorDialogMessage);
            }
            finally
            {
                _userDialogs.HideLoading();
            }
        }

        public void RefreshShouldRibbonBeDisplayed()
        {
            ShouldRibbonBeDisplayed = _menuService.IsAnyClaimForAudit();
        }

        private async void OnDashboardEntered(object sender, EventArgs e)
        {
            _userDialogs.ShowLoading(Resource.Loading);
            UserName = SetFirstLetterToUpper(_participantService.PlanMember.FirstName);
            try
            {
                RecentClaims.Clear();
                await GetRecentClaims();
            }
            catch (Exception)
            {
                _userDialogs.HideLoading();
                _userDialogs.Alert(Resource.GenericErrorDialogMessage);
            }
            finally
            {
                _userDialogs.HideLoading();
            }
        }

        private async Task GetRecentClaims()
        {
            try
            {
                var recentClaims = await _claimService.GetRecentClaimsAsync(_loginservice.CurrentPlanMemberID);
                foreach (var claim in recentClaims)
                {
                    RecentClaims.Add(GetDashboardRecentClaim(claim));
                }
            }
            catch (Exception)
            {
                RecentClaims = new ObservableCollection<DashboardRecentClaim>();
            }            
            HasAnyClaims = RecentClaims.Any();
            ClaimsFetched?.Invoke(this, EventArgs.Empty);
        }

        private static DashboardRecentClaim GetDashboardRecentClaim(RecentClaim recentClaim)
        {
            return new DashboardRecentClaim
            {
                ClaimFormId = recentClaim.ClaimFormId.ToString(),
                ClaimDetailId = recentClaim.ClaimDetailId.ToString(),
                ServiceDescription = recentClaim.ServiceDescription,
                ServiceDate = recentClaim.ServiceDate.ToString("d"),
                ClaimedAmount = recentClaim.totalCdRendAmt.AddDolarSignBasedOnCulture(),
                ActionRequired = recentClaim.ClaimActionStatus != ClaimActionState.None,
                ClaimActionState = recentClaim.ClaimActionStatus
            };
        }

        private static string SetFirstLetterToUpper(string planMemberFirstName)
        {
            if (string.IsNullOrEmpty(planMemberFirstName))
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();

            var nameSubstring = planMemberFirstName.Substring(1);
            var firstLetter = planMemberFirstName[0].ToString().ToUpper();
            var restOfName = nameSubstring.ToLower();

            stringBuilder.Append(firstLetter);
            stringBuilder.Append(restOfName);

            return stringBuilder.ToString();
        }

        private async Task ExecuteOpenAudit()
        {
            await ShowViewModel<AuditListViewModel>();
        }

        private async Task ExecuteShowAllClaims()
        {
            await ShowViewModel<ClaimsHistoryResultsCountViewModel>();
        }

        private async Task ExecuteShowEligibilityCheck(DashboardEligibilityCheckType typeId)
        {
            var eligibilityCheckType = _eligibilityService.EligibilityCheckTypes.FirstOrDefault(x => x.ID.Equals(typeId.ToString(), StringComparison.OrdinalIgnoreCase));

            _eligibilityService.EligibilitySelectedParticipant = null;
            _eligibilityService.SelectedEligibilityCheckType = eligibilityCheckType;
            _eligibilityService.EligibilityCheck = new EligibilityCheck
            {
                EligibilityCheckType = _eligibilityService.SelectedEligibilityCheckType,
                EligibilityCheckTypeID = _eligibilityService.SelectedEligibilityCheckType?.ID,
                PlanMemberID = long.Parse(_loginservice.GroupPlanNumber),
                ParticipantNumber = _loginservice.ParticipantNumber
            };

            await _eligibilityCheckToCommandsMapping[typeId]?.ExecuteAsync();
        }

        private async Task ExecuteShowDentalRecallExam()
        {
            _userDialogs.ShowLoading(Resource.Loading);
            _eligibilityCheckCompleteToken = _messenger.Subscribe<EligibilityCheckSubmissionComplete>(async (message2) =>
            {
                _userDialogs.HideLoading();
                _messenger.Unsubscribe<EligibilityCheckSubmissionComplete>(_eligibilityCheckCompleteToken);
                _messenger.Unsubscribe<EligibilityCheckSubmissionError>(_eligibilityCheckCompleteToken);
                await ShowViewModel<EligibilityCheckDREViewModel>();
            });
            _eligibilityCheckFailureToken = _messenger.Subscribe<EligibilityCheckSubmissionError>(async (message) =>
            {
                _userDialogs.HideLoading();
                _messenger.Unsubscribe<EligibilityCheckSubmissionComplete>(_eligibilityCheckCompleteToken);
                _messenger.Unsubscribe<EligibilityCheckSubmissionError>(_eligibilityCheckFailureToken);
                _userDialogs.Alert(Resource.GenericErrorDialogMessage);
            });
            _eligibilityService.CheckEligibility();
        }

        private async Task ExecuteShowChiropractorTreatment()
        {
            await ShowViewModel<EligibilityParticipantsViewModel>();
        }

        private async Task ExecuteMassageTherapy()
        {
            await ShowViewModel<EligibilityParticipantsViewModel>();
        }

        private async Task ExecuteShowDrugsOnTheGo()
        {
            if (_deviceService.CurrentDevice == GSCHelper.OS.iOS)
            {
                await ShowViewModel<MainNavigationViewModel>();
            }

            await ShowViewModel<DrugLookupModelSelectionViewModel>();
        }

        private async Task ExecuteShowHcsa()
        {
            if (HcsaAccountType == null)
            {
                return;
            }

            _spendingAccountService.SelectedSpendingAccountType = HcsaAccountType;

            await ShowViewModel<SpendingAccountDetailViewModel>();
        }

        private async Task ExecuteShowPsa()
        {
            if (PsaAccountType == null)
            {
                return;
            }

            _spendingAccountService.SelectedSpendingAccountType = PsaAccountType;

            await ShowViewModel<SpendingAccountDetailViewModel>();
        }

        private async Task ExecuteSelectRecentClaim(DashboardRecentClaim selectedRecentClaim)
        {
            var uploadable = (IClaimSummaryProperties) UploadFactory.Create(selectedRecentClaim.ClaimActionState, nameof(ClaimSummaryViewModel));

            if (selectedRecentClaim.ClaimActionState == ClaimActionState.Audit)
            {
                await ShowViewModel<AuditClaimSummaryViewModel, AuditClaimSummaryViewModelParameters>(new AuditClaimSummaryViewModelParameters(selectedRecentClaim, uploadable));
            }
            else
            {
                await ShowViewModel<ClaimSummaryViewModel, AuditClaimSummaryViewModelParameters>(new AuditClaimSummaryViewModelParameters(selectedRecentClaim, uploadable));
            }
        }

        private void PrepareMappings()
        {
            _eligibilityCheckToCommandsMapping = new Dictionary<DashboardEligibilityCheckType, IMvxAsyncCommand>
            {
                { DashboardEligibilityCheckType.CHIRO, ShowChiropracticCommand },
                { DashboardEligibilityCheckType.RECALLEXAM, ShowDentalRecallExamCommand },
                { DashboardEligibilityCheckType.MASSAGE, ShowMassageCommand }
            };
        }
    }
}