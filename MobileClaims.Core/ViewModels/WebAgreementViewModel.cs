using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.FacadeEntities;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using MobileClaims.Core.Constants;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MobileClaims.Core.ViewModelParameters;
using System.Collections.Generic;

namespace MobileClaims.Core.ViewModels
{
    public class NavigationCatalog
    {
        public string NavigateTo { get; set; }
        public Type NavigateToType { get; set; }

        public IViewModelParameters NavigateToParameter { get; set; }

        public string NavigateFrom { get; set; }
    }

    public class WebAgreementViewModel : ViewModelBase<WebAgreementViewModelParameters>
    {
        private readonly IParticipantService _participantService;
        private readonly IUserDialogs _userDialogService;
        private readonly IClaimService _claimService;
        private readonly ILoginService _loginService;

        private ICommand _cancelTermsAndConditions;
        private ICommand _acceptTermsAndConditions;

        private bool _isAccepted;
        private bool _isLoadingDynamicText;
        private string _dynamicString;
        private string _dynamicDate;
        private TextAlterationWithDate _textAlteration;
        private string _termsAndConditions = "Terms and Conditions go here";
        private const string DynamicTextNotAvailable = "Text Alteration not available";
        private const string DynamicDateNotAvailable = "Text Alteration Date not available";

        public WebAgreementViewModel(IMvxMessenger messenger, IParticipantService participantService,
            IUserDialogs userDialogService, IClaimService claimService, ILoginService loginService)
        {
            _participantService = participantService;
            _userDialogService = userDialogService;
            _claimService = claimService;
            _loginService = loginService;
        }

        public string TermsAndConditions
        {
            get => _termsAndConditions;
            set
            {
                _termsAndConditions = value;
                RaisePropertyChanged(() => TermsAndConditions);
            }
        }

        public bool IsAccepted
        {
            get => _isAccepted;
            set
            {
                _isAccepted = value;
                RaisePropertyChanged(() => IsAccepted);
            }
        }

        public bool IsLoadingDynamicText
        {
            get => _isLoadingDynamicText;
            set
            {
                _isLoadingDynamicText = value;
                RaisePropertyChanged(() => IsLoadingDynamicText);
            }
        }

        public string DynamicString
        {
            get => _dynamicString;
            set
            {
                _dynamicString = value;
                RaisePropertyChanged(() => DynamicString);
            }
        }

        public string DynamicDate
        {
            get => _dynamicDate;
            set
            {
                _dynamicDate = value;
                RaisePropertyChanged(() => DynamicDate);
            }
        }

        public TextAlterationWithDate TextAlteration
        {
            get => _textAlteration;
            set
            {
                _textAlteration = value;
                RaisePropertyChanged(() => TextAlteration);
            }
        }

        public NavigationCatalog NavigationCatalog { get; set; }
        
        public ICommand AcceptTermsAndConditionsCommand
        {
            get
            {
                return _acceptTermsAndConditions ?? (_acceptTermsAndConditions = new MvxCommand(async () =>
                {
                    try
                    {
                        var response =
                            await _participantService.PutUserAgreementWCSAsync(new UserAgreement("WCS", true));
                        if (response.ResponseCode == System.Net.HttpStatusCode.OK && response.Results.IsAccepted)
                        {
                            await ShowViewModel(Type.GetType(NavigationCatalog.NavigateTo), NavigationCatalog.NavigateToParameter);
                        }
                        else
                        {
                            _userDialogService.Alert(Resource.GenericErrorDialogMessage,
                                Resource.GenericErrorDialogTitle, Resource.ok);
                        }
                    }
                    catch (Exception)
                    {
                        _userDialogService.Alert(Resource.GenericErrorDialogMessage, Resource.GenericErrorDialogTitle,
                            Resource.ok);
                    }
                }));
            }
        }

        public ICommand CancelTermsAndConditionsCommand
        {
            get
            {
                return _cancelTermsAndConditions ??
                       (_cancelTermsAndConditions = new MvxCommand(() => { Close(this); }));
            }
        }

        private async Task GetTextAlteration()
        {
            try
            {
                var response = await _claimService.GetClaimAgreementTextAsync(_loginService.GroupPlanNumber);

                if (response.ResponseCode == System.Net.HttpStatusCode.OK)
                {
                    var sb = new StringBuilder();
                    foreach (var text in response.Results.Select(x=>x.Text))
                    {                        
                        sb.AppendFormat("<p>");
                        sb.AppendFormat(text);
                        sb.AppendFormat("</p>");
                    }
                    DynamicString = sb.ToString();
                }
                else
                {
                    DynamicString = DynamicTextNotAvailable;
                }
            }
            catch (Exception)
            {
                DynamicString = DynamicTextNotAvailable;
                _userDialogService.HideLoading();
            }
        }

        private async Task GetTextAlterationDate()
        {
            try
            {
                var response = await _claimService.GetClaimAgreementDateAsync(_loginService.GroupPlanNumber);

                DynamicDate = response.ResponseCode == System.Net.HttpStatusCode.OK ? response.Results : DynamicDateNotAvailable;
            }
            catch (Exception)
            {
                DynamicString = DynamicDateNotAvailable;
                _userDialogService.HideLoading();
            }
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            await GetTextAlterationDate();
            await GetTextAlteration();
            IsLoadingDynamicText = true;
            TextAlteration = new TextAlterationWithDate()
            {
                TextAlteration = DynamicString,
                TextAlterationDate = DynamicDate
            };
            IsLoadingDynamicText = true;
            _userDialogService.HideLoading();
        }

        public override void Prepare(WebAgreementViewModelParameters parameter)
        {
            _userDialogService.ShowLoading();

            NavigationCatalog = parameter.Catalog;
        }
    }
}