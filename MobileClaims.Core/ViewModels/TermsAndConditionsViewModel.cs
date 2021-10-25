using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MobileClaims.Core.Attributes;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    [RequiresAuthentication(false)]
    public class TermsAndConditionsViewModel : ViewModelBase
    {
        private readonly IClaimService _claimservice;
        private readonly IDataService _dataservice;

        private readonly MvxSubscriptionToken _gettextalterationclaim;
        private readonly MvxSubscriptionToken _gettextalterationclaimerror;
        private readonly MvxSubscriptionToken _gettextalterationphone;
        private readonly MvxSubscriptionToken _gettextalterationphoneerror;
        private readonly IMvxMessenger _messenger;
        private readonly MvxSubscriptionToken _notextalterationclaim;
        private readonly MvxSubscriptionToken _notextalterationphone;
        private readonly MvxSubscriptionToken _OrientationChageToken;

        private bool _acceptedTC;
        private List<TextAlteration> _claimAgreement;
        private bool _claimBusy;
        private bool _claimError;

        /// <summary>
        ///     Label content for check and accept term and conditions
        /// </summary>
        private string _labelAcceptedTC;

        private bool _noClaimAlteration;
        private bool _noPhoneAlteration;
        private bool _phoneBusy;
        private bool _phoneError;

        private TextAlteration _phoneNumber;

        /// <summary>
        ///     Scroll Viewer Height
        /// </summary>
        private double _scrollVHeight;

        /// <summary>
        ///     Scroll Viewr Width
        /// </summary>
        private double _scrollVWidth;

        /// <summary>
        ///     Collection for holding the term and condition's contents
        /// </summary>
        private ObservableCollection<string> _tCContents;

        /// <summary>
        ///     for whole text block's width
        /// </summary>
        private double _textBlockWidth;

        /// <summary>
        ///     Stack Panel height
        /// </summary>
        private double _totalHeight;

        /// <summary>
        ///     Stack Panel width
        /// </summary>
        private double _totalWidth;

        private string orientationString = string.Empty;
        private double pageHeight;
        private double pageWidth;

        public string General => BrandResource.General;
        public string Security => BrandResource.Security;
        public string Privacy => BrandResource.Privacy;
        public string Legal => BrandResource.Legal;
        public string IAgreeToTC => BrandResource.IAgreeToTC;

        public string TermsAndConditionsContent1 => BrandResource.TermsAndConditionsGeneral1;
        public string TermsAndConditionsContent2 => BrandResource.TermsAndConditionsGeneral2;
        public string TermsAndConditionsContent3 => BrandResource.TermsAndConditionsGeneral3;
        public string TermsAndConditionsContent4 => BrandResource.TermsAndConditionsGeneral4;
        public string TermsAndConditionsContent5 => BrandResource.TermsAndConditionsGeneral5;
        public string TermsAndConditionsContent6 => BrandResource.TermsAndConditionsGeneral6;
        public string TermsAndConditionsContent7 => BrandResource.TermsAndConditionsGeneral7;
        public string TermsAndConditionsContent_Phone => BrandResource.TermsAndConditionsGeneral8;

        public bool AcceptedTC
        {
            get => _acceptedTC;
            set => SetProperty(ref _acceptedTC, value);
        }

        public bool PhoneError
        {
            get => _phoneError;
            set => SetProperty(ref _phoneError, value);
        }

        public bool ClaimError
        {
            get => _claimError;
            set => SetProperty(ref _claimError, value);
        }

        public bool NoPhoneAlteration
        {
            get => _noPhoneAlteration;
            set => SetProperty(ref _noPhoneAlteration, value);
        }

        public bool NoClaimAlteration
        {
            get => _noClaimAlteration;
            set => SetProperty(ref _noClaimAlteration, value);
        }

        public List<TextAlteration> ClaimAgreement
        {
            get => _claimAgreement;
            set => SetProperty(ref _claimAgreement, value);
        }

        public TextAlteration PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        public bool PhoneBusy
        {
            get => _phoneBusy;
            set => SetProperty(ref _phoneBusy, value);
        }

        public bool ClaimBusy
        {
            get => _claimBusy;
            set => SetProperty(ref _claimBusy, value);
        }

        public string LabelAcceptedTC
        {
            get => _labelAcceptedTC;
            set => SetProperty(ref _labelAcceptedTC, value);
        }

        public ObservableCollection<string> TCContents
        {
            get => _tCContents;
            set => SetProperty(ref _tCContents, value);
        }

        public double TextBlockWidth
        {
            get => _textBlockWidth;
            set => SetProperty(ref _textBlockWidth, value);
        }

        public double TotalWidth
        {
            get => _totalWidth;
            set => SetProperty(ref _totalWidth, value);
        }

        public double TotalHeight
        {
            get => _totalHeight;
            set => SetProperty(ref _totalHeight, value);
        }

        public double ScrollVHeight
        {
            get => _scrollVHeight;
            set => SetProperty(ref _scrollVHeight, value);
        }

        public double ScrollVWidth
        {
            get => _scrollVWidth;
            set => SetProperty(ref _scrollVWidth, value);
        }

        public IMvxAsyncCommand AcceptTermsAndConditions { get; }

        public IMvxAsyncCommand AcceptTermsAndConditionsDroid { get; }

        public IMvxAsyncCommand ShowLandingPage { get; }

        public event EventHandler OnError;

        public TermsAndConditionsViewModel(
            IDataService dataservice,
            IMvxMessenger messenger,
            IClaimService claimservice)
        {
            _dataservice = dataservice;
            _messenger = messenger;
            _claimservice = claimservice;
            _acceptedTC = _dataservice.GetAcceptedTC();

            AcceptTermsAndConditions = new MvxAsyncCommand(ExecuteAcceptTermsAndConditions, CanExecuteAcceptTermsAndConditions);
            AcceptTermsAndConditionsDroid = new MvxAsyncCommand(ExecuteAcceptTermsAndConditionsDroid, CanExecuteAcceptTermsAndConditionsDroid);
            ShowLandingPage = new MvxAsyncCommand(ExecuteShowLandingPage);

            _gettextalterationclaim = _messenger.Subscribe<GetTextAlterationClaimAgreementComplete>(message =>
            {
                ClaimAgreement = _claimservice.ClaimAgreementTextAlterations;
                ClaimBusy = false;
            });

            _gettextalterationclaimerror = _messenger.Subscribe<GetTextAlterationClaimAgreementError>(message =>
            {
                if (!PhoneError)
                {
                    RaiseClaimError(new EventArgs());
                }

                ClaimError = true;
                ClaimBusy = false;
            });

            _notextalterationclaim = _messenger.Subscribe<NoTextAlterationClaimAgreementFound>(message =>
            {
                NoClaimAlteration = true;
                ClaimBusy = false;
            });

            _gettextalterationphone = _messenger.Subscribe<GetTextAlterationPhoneComplete>(message =>
            {
                PhoneNumber = _claimservice.PhoneText;
                PhoneBusy = false;
            });

            _gettextalterationphoneerror = _messenger.Subscribe<GetTextAlterationPhoneError>(message =>
            {
                if (!ClaimError)
                {
                    RaiseError(new EventArgs());
                }

                PhoneError = true;
                PhoneBusy = false;
            });

            _notextalterationphone = _messenger.Subscribe<NoTextAlterationPhoneFound>(message =>
            {
                NoPhoneAlteration = true;
                PhoneBusy = false;
            });

            _OrientationChageToken = _messenger.Subscribe<OrientationChange>(message =>
            {
                //if (message.Sender.GetType().FullName == this.GetType().FullName) return;
                if (message != null)
                {
                    pageHeight = message.Height;
                    pageWidth = message.Width;
                    orientationString = message.OrientationStr;

                    if (orientationString == GSCHelper.orientation_LandscapeRight ||
                        orientationString == GSCHelper.orientation_LandscapeLeft)
                    {
                        SetLandscapeLayout(pageHeight, pageWidth);
                    }
                    else
                    {
                        SetPortraitLayout(pageHeight, pageWidth);
                    }
                }
            });

            if (_acceptedTC)
            {
                if (!_claimservice.ClaimError && !NoClaimAlteration)
                {
                    if (_claimservice.ClaimAgreementTextAlterations == null)
                    {
                        ClaimBusy = true;
                        _claimservice.GetClaimAgreement(_loginservice.CurrentPlanMemberID);
                    }
                    else
                    {
                        ClaimAgreement = _claimservice.ClaimAgreementTextAlterations;
                        ClaimBusy = false;
                    }
                }
                else
                {
                    ClaimError = _claimservice.ClaimError;
                }

                if (!_claimservice.PhoneError && !NoPhoneAlteration)
                {
                    if (_claimservice.PhoneTextAlterations == null)
                    {
                        PhoneBusy = true;

                        _claimservice.GetPhoneNumber(_loginservice.CurrentPlanMemberID);
                    }
                    else
                    {
                        PhoneNumber = _claimservice.PhoneText;
                        PhoneBusy = false;
                    }
                }
                else
                {
                    PhoneError = _claimservice.PhoneError;
                }
            }

            _messenger.Unsubscribe<LoggedOutMessage>(_loggedouttoken);
        }

        private Task ExecuteShowLandingPage()
        {
            Unsubscribe();
            return ShowViewModel<LandingPageViewModel>();
        }

        private Task ExecuteAcceptTermsAndConditions()
        {
            Unsubscribe();
            Task.Delay(500);
            _dataservice.PersistAcceptedTC(true);
            return ShowViewModel<LoginViewModel>();
        }

        private async Task ExecuteAcceptTermsAndConditionsDroid()
        {
            Unsubscribe();
            _dataservice.PersistAcceptedTC(true);
            await ShowViewModel<LoginViewModel>();
            await Close(this);
        }

        private bool CanExecuteAcceptTermsAndConditions()
        {
            return !AcceptedTC;
        }

        private bool CanExecuteAcceptTermsAndConditionsDroid()
        {
            return AcceptedTC;
        }

        protected virtual void RaiseError(EventArgs e)
        {
            OnError?.Invoke(this, e);
        }

        public event EventHandler OnClaimError;

        protected virtual void RaiseClaimError(EventArgs e)
        {
            OnClaimError?.Invoke(this, e);
        }

        private void Unsubscribe()
        {
            _messenger.Unsubscribe<OrientationChange>(_OrientationChageToken);
            _messenger.Unsubscribe<GetTextAlterationPhoneComplete>(_gettextalterationphone);
            _messenger.Unsubscribe<GetTextAlterationPhoneError>(_gettextalterationphoneerror);
            _messenger.Unsubscribe<NoTextAlterationPhoneFound>(_notextalterationphone);
            _messenger.Unsubscribe<GetTextAlterationClaimAgreementComplete>(_gettextalterationclaim);
            _messenger.Unsubscribe<GetTextAlterationClaimAgreementError>(_gettextalterationclaimerror);
            _messenger.Unsubscribe<NoTextAlterationClaimAgreementFound>(_notextalterationclaim);
        }

        public void SetPortraitLayout(double height, double width)
        {
            TotalHeight = height - 100;
            TotalWidth = width;
            TextBlockWidth = width - 20;
            ScrollVHeight = height - 40;
            ScrollVWidth = TextBlockWidth + 10;
        }

        public void SetLandscapeLayout(double height, double width)
        {
            TotalHeight = height - 100;
            TotalWidth = width;
            TextBlockWidth = width - 50;
            ScrollVHeight = height - 40;
            ScrollVWidth = TextBlockWidth + 10;
        }
    }
}