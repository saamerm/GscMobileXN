using System;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MobileClaims.Core.Constants;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Models.Upload.Specialized;
using MobileClaims.Core.Services;
using MobileClaims.Core.Util;
using MobileClaims.Core.ViewModelParameters;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels
{
    public class DisclaimerViewModel : ViewModelBase<DisclaimerViewModelParameters>, IDisclaimerProperties
    {
        private readonly IClaimService _claimService;
        private readonly ILoginService _loginService;
        private readonly IUserDialogs _userDialogs;

        private bool isAudit = false;

        private string _firstParagraph;
        private string _secondParagraph;
        private string _thirdParagraph;

        public string Disclaimer => Resource.Disclaimer;

        public string Title { get; private set; }

        public string FirstParagraph
        {
            get => _firstParagraph;
            private set => SetProperty(ref _firstParagraph, value);
        }

        public string SecondParagraph
        {
            get => _secondParagraph;
            private set => SetProperty(ref _secondParagraph, value);
        }

        public string ThirdParagraph
        {
            get => _thirdParagraph;
            private set => SetProperty(ref _thirdParagraph, value);
        }

        public DisclaimerViewModel(IClaimService claimService, ILoginService loginService, IUserDialogs userDialogs)
        {
            _claimService = claimService;
            _loginService = loginService;
            _userDialogs = userDialogs;
        }

        public override async Task Initialize()
        {

            if (isAudit)
            {
                // Use local disclaimer.
                return;
            }

            try
            {
                _userDialogs.ShowLoading(Resource.Loading);

                await _claimService.GetClaimDisclaimer(_loginService.GroupPlanNumber);
                var customDisclaimerText = _claimService.ClaimDisclaimerTextAlterations;

                if (customDisclaimerText != null)
                {
                    FirstParagraph = string.Join(Environment.NewLine + Environment.NewLine, customDisclaimerText.Select(x => x.Text));
                    SecondParagraph = string.Empty;
                    ThirdParagraph = string.Empty;
                }

                _userDialogs.HideLoading();
            }
            catch (NullResponseException)
            {
                _userDialogs.HideLoading();
                await _userDialogs.AlertAsync(Resource.GenericErrorDialogMessage);
            }
        }

        public override void Prepare(DisclaimerViewModelParameters parameter)
        {
            if(parameter.DisclaimerProperties is AuditDisclaimerProperties)
            {
                isAudit = true;
            }

            Title = parameter.DisclaimerProperties.Title;
            FirstParagraph = parameter.DisclaimerProperties.FirstParagraph;
            SecondParagraph = parameter.DisclaimerProperties.SecondParagraph;
            ThirdParagraph = parameter.DisclaimerProperties.ThirdParagraph;
        }
    }
}