using Acr.UserDialogs;
using MobileClaims.Core.Attributes;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModelParameters;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.File;
using MvvmCross.Plugin.Messenger;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MobileClaims.Core.ViewModels
{
    [RequiresAuthentication(false)]
    public class CardViewModel : ViewModelBase<CardViewModelParameter>
    {
        private readonly IParticipantService _participantService;
        private readonly IMvxMessenger _messenger;
        private readonly IDataService _dataservice;
        private readonly IMvxFileStore _filesystem;
        private readonly IUserDialogs _userDialogs;
        private readonly ICardService _idCardService;

        private object _sync = new object();

        private bool fromloginscreen;
        public bool FromLoginScreen
        {
            get => fromloginscreen;
            set => SetProperty(ref fromloginscreen, value);
        }

        private bool _shouldShowAddToWalletButton;
        public bool ShouldShowAddToWalletButton
        {
            get => _shouldShowAddToWalletButton;
            set => SetProperty(ref _shouldShowAddToWalletButton, value);
        }

        private IDCard _card;
        public IDCard Card
        {
            get => _card;
            set => SetProperty(ref _card, value);
        }

        private PlanMember _member;
        public PlanMember Member
        {
            get => _member;
            set => SetProperty(ref _member, value);
        }

        private bool _showFrontView;
        public bool ShowFrontView
        {
            get => _showFrontView;
            set => SetProperty(ref _showFrontView, value);
        }

        private string _buttLeftLable;
        public string ButtLeftLable
        {
            get => _buttLeftLable;
            set => SetProperty(ref _buttLeftLable, value);
        }

        private string _buttRightLable;
        public string ButtRightLable
        {
            get => _buttRightLable;
            set => SetProperty(ref _buttRightLable, value);
        }

        private bool _showListScrollBar;
        public bool ShowListScrollBar
        {
            get => _showListScrollBar;
            set => SetProperty(ref _showListScrollBar, value);
        }

        private bool _haveBackImage;
        public bool HaveBackImage
        {
            get => _haveBackImage;
            set => SetProperty(ref _haveBackImage, value);
        }

        private IDCardParticipant _planMemberSpouse;
        public IDCardParticipant PlanMemberSpouse
        {
            get => _planMemberSpouse;
            set => SetProperty(ref _planMemberSpouse, value);
        }

        private string _planMemberName;
        public string PlanMemberName
        {
            get => _planMemberName;
            set => SetProperty(ref _planMemberName, value);
        }

        private string _planMemberSpouseName;
        public string PlanMemberSpouseName
        {
            get => _planMemberSpouseName;
            set => SetProperty(ref _planMemberSpouseName, value);
        }

        private string _bottomCardText;
        public string BottomCardText
        {
            get => _bottomCardText;
            set => SetProperty(ref _bottomCardText, value);
        }

        private string _certificateNo;
        public string CertificateNo
        {
            get => _certificateNo;
            set => SetProperty(ref _certificateNo, value);
        }

        private string _frontIdCardImagePath;
        public string FrontIdCardImagePath
        {
            get => _frontIdCardImagePath;
            set => SetProperty(ref _frontIdCardImagePath, value);
        }

        private string _backIdCardImagePath;
        public string BackIdCardImagePath
        {
            get => _backIdCardImagePath;
            set => SetProperty(ref _backIdCardImagePath, value);
        }

        private string _frontLeftLogoImagePath;
        public string FrontLeftLogoImagePath
        {
            get => _frontLeftLogoImagePath;
            set => SetProperty(ref _frontLeftLogoImagePath, value);
        }

        private string _frontRightLogoImagePath;
        public string FrontRightLogoImagePath
        {
            get => _frontRightLogoImagePath;
            set => SetProperty(ref _frontRightLogoImagePath, value);
        }

        private string _backTopLogoImagePath;
        public string BackTopLogoImagePath
        {
            get => _backTopLogoImagePath;
            set => SetProperty(ref _backTopLogoImagePath, value);
        }

        public ICommand CloseCommand { get; private set; }
        public ICommand OnButtShowBackViewClick { get; private set; }
        public ICommand OnButtShowFrontViewClick { get; private set; }

        public class CardViewModelNavHelper
        {
            private bool fromloginscreen;

            public bool FromLoginScreen
            {
                get { return fromloginscreen; }
                set { fromloginscreen = value; }
            }

            public CardViewModelNavHelper()
            {
            }

            public CardViewModelNavHelper(bool fromLoginScreen)
            {
                FromLoginScreen = fromLoginScreen;
            }
        }

        public void Init(CardViewModelNavHelper nav)
        {
            this.FromLoginScreen = nav.FromLoginScreen;
            ShouldShowAddToWalletButton = !FromLoginScreen;
        }

        public CardViewModel()
        {
            _participantService = Mvx.IoCProvider.Resolve<IParticipantService>();
            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            _loginservice = Mvx.IoCProvider.Resolve<ILoginService>();
            _dataservice = Mvx.IoCProvider.Resolve<IDataService>();
            _filesystem = Mvx.IoCProvider.Resolve<IMvxFileStore>();
            _userDialogs = Mvx.IoCProvider.Resolve<IUserDialogs>();
            _idCardService = Mvx.IoCProvider.Resolve<ICardService>();

            InitializedCommands();
        }

        public CardViewModel(IParticipantService participantservice,
            ICardService idCardService,
            IMvxMessenger messenger,
            ILoginService loginservice,
            IDataService dataservice,
            IMvxFileStore filesystem,
            IUserDialogs userDialogs)
        {
            lock (_sync)
            {
                _messenger = messenger;
                _loginservice = loginservice;
                _dataservice = dataservice;
                _filesystem = filesystem;
                _userDialogs = userDialogs;
                _participantService = participantservice;
                _idCardService = idCardService;
            }

            InitializedCommands();
        }

        public override void Prepare(CardViewModelParameter parameter)
        {
            this.FromLoginScreen = parameter.FromLoginScreen;
            ShouldShowAddToWalletButton = !FromLoginScreen;
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            _userDialogs.ShowLoading(Resource.Loading);

            Card = _dataservice.GetIDCard();

            if (Card != null
                && !string.IsNullOrWhiteSpace(Card.FrontImageFilePath)
                && !string.IsNullOrWhiteSpace(Card.BackImageFilePath))
            {
                if (!_filesystem.Exists(Card.FrontImageFilePath) || !_filesystem.Exists(Card.BackImageFilePath))
                {
                    Card = _idCardService.UpdateIdCardImage(Card);
                }
            }
            else
            {
                _loginservice.CancellationTokenSource.Cancel();
                Card = await _idCardService.GetIdCardAsync(_participantService.PlanMember.PlanMemberID, CancellationToken.None);
            }

            // TODO: Should be re-written for new MVVMCross library to split between Prepare() and Initialize()
            if (Card != null)
            {
                FrontIdCardImagePath = Card.FrontImageFilePath;
                BackIdCardImagePath = Card.BackImageFilePath;
                FrontLeftLogoImagePath = Card.FrontLeftLogoFilePath;
                FrontRightLogoImagePath = Card.FrontRightLogoFilePath;
                BackTopLogoImagePath = Card.BackTopLogoFilePath;
                PopulatePlanMembersInfo();
                _userDialogs.HideLoading();
            }
            else
            {
                await _userDialogs.AlertAsync(Resource.GenericErrorDialogMessage, Resource.GenericErrorDialogTitle, Resource.ok);
                _userDialogs.HideLoading();
                await ExecuteCloseCommand();
            }
        }

        private void InitializedCommands()
        {
            CloseCommand = new MvxAsyncCommand(ExecuteCloseCommand);
            OnButtShowBackViewClick = new MvxCommand(ExecuteShowBackView);
            OnButtShowFrontViewClick = new MvxCommand(ExecuteShowFrontView);
        }

        private void ExecuteShowFrontView()
        {
            ShowFrontView = true;
        }

        private void ExecuteShowBackView()
        {
            ShowFrontView = false;
        }

        private Task<bool> ExecuteCloseCommand()
        {
            return FromLoginScreen ? ShowViewModel<LoginViewModel>() : ShowViewModel<LandingPageViewModel>();
        }

        private void PopulatePlanMembersInfo()
        {
            PlanMember member = null;
            member = _dataservice.GetCardPlanMember();

            if (member != null)
            {
                Member = member;
            }
            else
            {
                _participantService.GetParticipant(_loginservice.CurrentPlanMemberID);
            }

            ShowFrontView = true;
            ShowListScrollBar = false;

            if (Card.Participants != null)
            {
                foreach (IDCardParticipant cardParticipant in Card.Participants)
                {
                    cardParticipant.PlanMemberDisplayID = Card.PlanMemberDisplayID;
                }

                if (Card.Participants.Count >= 6)
                {
                    ShowListScrollBar = true;
                }

                PlanMemberSpouse = Card.Participants.Find(participant => participant != null && string.Equals(participant.ParticipantType, "SP"));

                if (PlanMemberSpouse != null)
                {
#if CCQ
                    PlanMemberSpouseName = $"{PlanMemberSpouse.ParticipantLastName}, {PlanMemberSpouse.ParticipantFirstName}";
#else
                    PlanMemberSpouseName = $"{PlanMemberSpouse.ParticipantFullName}";
#endif
                }
            }

#if CCQ
            PlanMemberName = $"{Card.PlanMemberLastName}, {Card.PlanMemberFirstName}";
#else
            PlanMemberName = $"{Card.PlanMemberFullName}";
#endif

            if (!string.IsNullOrEmpty(Card.PolicyNo))
            {
                CertificateNo = Card.PlanMemberDisplayID.TrimStart(Card.PolicyNo.ToCharArray());
                BottomCardText = $"{Resource.PolicyNo} {Card.PolicyNo}   {Resource.CertificateNo} {CertificateNo}";
            }
        }
    }
}
