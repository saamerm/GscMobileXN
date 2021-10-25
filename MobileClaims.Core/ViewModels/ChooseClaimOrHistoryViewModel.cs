using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using MobileClaims.Core.Constants;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Extensions;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.HCSA;
using MobileClaims.Core.Util;
using MobileClaims.Core.ViewModelParameters;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using MobileClaims.Core.ViewModels.DirectDeposit;
using MobileClaims.Core.ViewModels.HCSA;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Newtonsoft.Json;

namespace MobileClaims.Core.ViewModels
{
    public class ChooseClaimOrHistoryViewModel : HCSAViewModelBase
    {
        private readonly IRehydrationService _rehydrationservice;
        private readonly IClaimService _claimservice;
        private readonly IDeviceService _deviceservice;
        private readonly IHCSAClaimService _hcsaservice;
        private readonly IParticipantService _participantService;
        private readonly IUserDialogs _userDialogs;
        private readonly IMenuService _menuService;
        private readonly IDirectDepositService _directDepositService;
        public EventHandler ClaimsCollectionIsSet;

        public string NoActiveClaims => BrandResource.NoActiveClaimsLabel;
        public string ClaimsCommandLabel => Resource.NewClaim;
        public string ClaimsHistoryLabel => Resource.ClaimsHistoryCaps;
        public string ActiveClaims => Resource.ActiveClaimsListLabel;
        public string ActionRequiredLabel => Resource.ActionRequiredLabel;

        public bool AreAnyActiveClaims => COPClaims.Any();

        private ObservableCollection<TopCardViewData> _copClaims = new ObservableCollection<TopCardViewData>();
        public ObservableCollection<TopCardViewData> COPClaims
        {
            get => _copClaims;
            set => SetProperty(ref _copClaims, value);
        }

        public MvxCommand<TopCardViewData> SelectActiveClaimCommand { get; }
        public IMvxCommand OpenAuditCommand { get; }

        private bool _shouldRibbonBeDisplayed;
        public bool ShouldRibbonBeDisplayed
        {
            get => _shouldRibbonBeDisplayed;
            set => SetProperty(ref _shouldRibbonBeDisplayed, value);
        }

        public ChooseClaimOrHistoryViewModel(IRehydrationService rehydrationservice,
                                             ILoginService loginservice,
                                             IClaimService claimservice,
                                             IDeviceService deviceservice,
                                             IHCSAClaimService hcsaservice,
                                             IParticipantService participantService,
                                             IMenuService menuService,
                                             IUserDialogs userDialogs,
                                             IDirectDepositService directDepositService)
        {
            _rehydrationservice = rehydrationservice;
            _loginservice = loginservice;
            _claimservice = claimservice;
            _deviceservice = deviceservice;
            _hcsaservice = hcsaservice;
            _participantService = participantService;
            _userDialogs = userDialogs;
            _menuService = menuService;
            _directDepositService = directDepositService;

            COPClaims.CollectionChanged += OnCopClaimsCollectionChanged;

            ClaimCommand = new MvxCommand(ExecuteClaimCommand);
            ClaimsHistoryCommand = new MvxCommand(ExecuteClaimsHistory);
            SelectActiveClaimCommand = new MvxCommand<TopCardViewData>(ExecuteSelectActiveClaim);
            OpenAuditCommand = new MvxCommand(ExecuteOpenAudit);
        }

        private async void ExecuteSelectActiveClaim(TopCardViewData arg)
        {
            var claimSummary = await GetCOPClaimSummaryAsync(arg.ClaimForm);
            if (claimSummary == null)
            {
                return;
            }

            var coPPlanMemberData = new UploadDocumentsFormData
            {
                ClaimFormId = claimSummary.ClaimFormID,
                ParticipantNumber = claimSummary.ParticipantNumber
            };

            var topCardViewWithSummaryData = AddSummaryToTopCardViewData(claimSummary);
            IClaimSummaryProperties uploadable = (IClaimSummaryProperties)UploadFactory.Create(topCardViewWithSummaryData.ClaimActionState, nameof(ActiveClaimDetailViewModel));

            await ShowViewModel<ActiveClaimDetailViewModel, ActiveClaimDetailViewModelParameters>(new ActiveClaimDetailViewModelParameters(topCardViewWithSummaryData, coPPlanMemberData, uploadable));
        }

        private async Task<ClaimSummary> GetCOPClaimSummaryAsync(string claimFormId)
        {
            IEnumerable<ClaimSummary> claimSummary;
            try
            {
                _userDialogs.ShowLoading(Resource.Loading);
                claimSummary = await _claimservice.GetCOPClaimSummaryAsync(claimFormId);
                if (claimSummary == null)
                {
                    throw new NullResponseException();
                }
            }
            catch (NullResponseException e)
            {
                _userDialogs.HideLoading();
                await _userDialogs.AlertAsync(Resource.ServerError_Description, Resource.ServerError_Title);
                return null;
            }
            finally
            {
                _userDialogs.HideLoading();
            }

            return claimSummary.FirstOrDefault();
        }

        public override async void Start()
        {
            base.Start();
            try
            {
                ShouldRibbonBeDisplayed = _menuService.IsAnyClaimForAudit();

                _userDialogs.ShowLoading(Resource.Loading);
                var planMember = _participantService.PlanMember;
                if (planMember == null)
                {
                    throw new NullResponseException();
                }

                await GetCopClaims();
            }
            catch (NullResponseException e)
            {
                _userDialogs.HideLoading();
                await _userDialogs.AlertAsync(Resource.ServerError_Description, Resource.ServerError_Title);
            }
            finally
            {
                _userDialogs.HideLoading();
            }
        }

        private async Task GetCopClaims()
        {
            var copClaims = await _claimservice.GetCOPClaimsAsync();
            if (copClaims != null)
            {
                foreach (var arg in copClaims)
                {
                    var topCardViewData = GetTopCardViewData(arg);
                    COPClaims.Add(topCardViewData);
                }
            }

            ClaimsCollectionIsSet?.Invoke(this, EventArgs.Empty);
        }

        private TopCardViewData GetTopCardViewData(ClaimCOP claimCOP)
        {
            //claimCOP.IsSelectedForAudit = true;
            //claimCOP.IsCop = false;
            return new TopCardViewData
            {
                ClaimForm = claimCOP.ClaimFormID.ToString(),
                ClaimedAmount = claimCOP.TotalCdRendAmt?.AddDolarSignBasedOnCulture(),
                ServiceDate = claimCOP.MinServeDate.Date.ToString("d") == claimCOP.MaxServeDate.Date.ToString("d")
                                        ? claimCOP.MinServeDate.Date.ToString("d")
                                        : $"{claimCOP.MinServeDate.Date:d} - {claimCOP.MaxServeDate.Date:d}",
                ServiceDescription = claimCOP.BenefitTypeDescr,
                ClaimActionState = claimCOP.IsCop ? ClaimActionState.Cop : claimCOP.IsSelectedForAudit ? ClaimActionState.Audit : ClaimActionState.None
            };
        }

        private TopCardViewData AddSummaryToTopCardViewData(ClaimSummary claimSummary)
        {
            var topCardViewData = GetTopCardViewData(claimSummary);
            topCardViewData.ParticipantNumber = claimSummary.ParticipantNumber;
            topCardViewData.UserName = claimSummary.ParticipantName;
            topCardViewData.EobMessages = string.Join(Environment.NewLine + Environment.NewLine, 
                                            claimSummary.EOBMessages.Select(x => x.Message).Distinct().ToList());
            return topCardViewData;
        }

        private void OnCopClaimsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(() => COPClaims);
            RaisePropertyChanged(() => AreAnyActiveClaims);
        }

        public MvxCommand ClaimCommand { get; }

        private async void ExecuteClaimCommand()
        {
            // Display Direct Deposit page if no EFT information for PlanMember and does not allow to submit Claim
#if GSC || WestJet || CLAC || ENCON
            if (!(await _directDepositService.GetHasEFTInfoAsync()))
            {
                await ShowViewModel<DirectDepositViewModel>();
                return;
            }
#endif
            try
            {
                if (_claimservice.Claim?.Type != null ||
                 _claimservice.IsHCSAClaim && _hcsaservice.Claim != null)
                {
                    var config = new ConfirmConfig
                    {
                        CancelText = Resource.No,
                        OkText = Resource.Yes,
                        Message = Resource.RehydrationMessage
                    };

                    var continueNotCompletedClaim = await Mvx.IoCProvider.Resolve<IUserDialogs>().ConfirmAsync(config);
                    if (!continueNotCompletedClaim)
                    {
                        _claimservice.ClearClaimDetails();
                        _rehydrationservice.ClearData();
                        Unsubscribe();
                        await ShowViewModel<ClaimSubmissionTypeViewModel>();
                    }
                    else
                    {
                        _rehydrationservice.Reload();
                        Unsubscribe();
                        if (_deviceservice.CurrentDevice != GSCHelper.OS.Droid)
                        {
                            await RehydrateProcess();
                        }
                        else
                        {
                            if (_rehydrationservice.BusinessProcess.Count > 0)
                            {
                                _rehydrationservice.Rehydrating = true;
                                await RehydrateDroidProcess();
                            }
                            else
                            {
                                Unsubscribe();
                                _claimservice.SelectedClaimSubmissionType = null;
                                _claimservice.IsHCSAClaim = false;
                                await ShowViewModel<ClaimSubmissionTypeViewModel>();
                            }
                        }
                    }
                }
                else
                {
                    Unsubscribe();
                    await _participantService.GetUserAgreementWCS();
                    var ua = _participantService.UserAgreement;
                    if (ua == null)
                    {
                        throw new NullResponseException();
                    }
                    if (ua.IsAccepted)
                    {
                        await ShowViewModel<ClaimSubmissionTypeViewModel>();
                    }
                    else
                    {
                        var catalog = new NavigationCatalog
                        {
                            NavigateTo = typeof(ClaimSubmissionTypeViewModel).FullName,
                            NavigateToParameter = null,
                            NavigateFrom = typeof(ChooseClaimOrHistoryViewModel).FullName
                        };
           
                        await ShowViewModel<WebAgreementViewModel, WebAgreementViewModelParameters>(new WebAgreementViewModelParameters(catalog));
                    }
                }
            }
            catch (NullResponseException e)
            {
                await _userDialogs.AlertAsync(Resource.ServerError_Description, Resource.ServerError_Title);
            }
        }

        public ICommand ClaimsHistoryCommand { get; }

        private void ExecuteClaimsHistory()
        {
            ShowViewModel<ClaimsHistoryResultsCountViewModel>();

        }
        private void ExecuteOpenAudit()
        {
            ShowViewModel<AuditListViewModel>();
        }

        public event EventHandler OnExistingClaim;
        protected virtual void RaiseExistingClaim(EventArgs e)
        {
            OnExistingClaim?.Invoke(this, e);
        }
    }
}