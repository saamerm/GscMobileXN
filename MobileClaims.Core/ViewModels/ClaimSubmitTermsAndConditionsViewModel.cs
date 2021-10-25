using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MobileClaims.Core.Constants;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Models.Upload.Specialized.PerType;
using MobileClaims.Core.Services;
using MobileClaims.Core.Util;
using MobileClaims.Core.ViewModelParameters;
using MobileClaims.Core.ViewModels.HCSA;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Newtonsoft.Json;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimSubmitTermsAndConditionsViewModel : ViewModelBase<ClaimSubmitTermsAndConditionsViewModelParameters>, IDisclaimerProperties
    {
        private readonly IClaimService _claimService;
        private readonly IUserDialogs _userDialogs;

        private bool _claimError = true;
        private string _title;
        private string _firstParagraph;
        private string _secondParagraph;
        private string _thirdParagraph;
        private NonRealTimeClaimType _claimType;
        private string _comment;
        private ObservableCollection<DocumentInfo> _attachments = new ObservableCollection<DocumentInfo>();

        public string AgreeButtonLabel => Resource.AgreeAndContinue;
        public string DisclaimerLabel => Resource.Disclaimer.ToUpperInvariant();
        public string ClaimErrorText => Resource.ConnectionError;

        public bool ClaimError
        {
            get => _claimError;
            set => SetProperty(ref _claimError, value);
        }

        public string Title
        {
            get => _title;
            private set => SetProperty(ref _title, value);
        }

        public string FirstParagraph
        {
            get => _firstParagraph;
            set => SetProperty(ref _firstParagraph, value);
        }

        public string SecondParagraph
        {
            get => _secondParagraph;
            set => SetProperty(ref _secondParagraph, value);
        }

        public string ThirdParagraph
        {
            get => _thirdParagraph;
            set => SetProperty(ref _thirdParagraph, value);
        }

        public IMvxCommand AcceptTermsAndConditionsCommand { get; }

        public ClaimSubmitTermsAndConditionsViewModel(
            IClaimService claimService,
            IUserDialogs userDialogs)
        {
            _claimService = claimService;
            _userDialogs = userDialogs;

            Title = Resource.SubmitClaim;
            AcceptTermsAndConditionsCommand = new MvxCommand(ExecuteAcceptDisclaimerCommand);
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            _userDialogs.ShowLoading(Resource.Loading);

            try
            {
                await _claimService.GetClaimDisclaimer(_loginservice.GroupPlanNumber);
                var customDisclaimerText = _claimService.ClaimDisclaimerTextAlterations;
                ClaimError = false;
                if (customDisclaimerText != null)
                {
                    FirstParagraph = string.Join(Environment.NewLine + Environment.NewLine, customDisclaimerText.Select(x => x.Text));
                    SecondParagraph = string.Empty;
                }
                else
                {
                    FirstParagraph = BrandResource.DisclaimerFirstParagraph;
                    SecondParagraph = BrandResource.DisclaimerSecondParagraph;
                }
                ThirdParagraph = string.Empty;
            }
            catch (Exception)
            {
                _userDialogs.HideLoading();
                ClaimError = true;
                FirstParagraph = Resource.ConnectionError;
                SecondParagraph = string.Empty;
            }
            finally
            {
                _userDialogs.HideLoading();
            }
        }

        private void ExecuteAcceptDisclaimerCommand()
        {
            _claimService.Claim.TermsAndConditionsAccepted = true;
            if (_claimService.IsHCSAClaim)
            {
                ShowViewModel<ClaimConfirmationHCSAViewModel>();
            }
            else
            {
                IClaimSubmitProperties properties = (IClaimSubmitProperties)NonRealTimeUploadFactory.Create(_claimType, nameof(ClaimSubmissionConfirmationViewModel));
                ShowViewModel<ClaimSubmissionConfirmationViewModel, ClaimSubmissionConfirmationViewModelParameters>(new ClaimSubmissionConfirmationViewModelParameters(_claimType, properties, _attachments, _comment));
            }
        }

        public override void Prepare(ClaimSubmitTermsAndConditionsViewModelParameters parameter)
        {
            _claimType = parameter.NonRealTimeClaimType;
            _attachments = parameter.Attachments;
            _comment = parameter.Comment;

            Title = parameter.Uploadable.Title;
            FirstParagraph = parameter.Uploadable.FirstParagraph;
            SecondParagraph = parameter.Uploadable.SecondParagraph;
            ThirdParagraph = parameter.Uploadable.ThirdParagraph;
        }

    }
}