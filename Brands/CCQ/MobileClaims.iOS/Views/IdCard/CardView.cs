using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Acr.MvvmCross.Plugins.UserDialogs;
using CoreGraphics;
using Foundation;
using MobileClaims.Core;
using MobileClaims.Core.Converters;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Converters;
using MobileClaims.iOS.UI;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using PassKit;
using UIKit;

namespace MobileClaims.iOS.Views.IdCard
{
    public partial class CardView : GSCBaseViewController
    {
        private CardViewModel _viewModel;
        private int currentPage;
        private float _bnbHeight;
        private float _requiredHeight;
        private float _requiredWidth;

        public UIImage FrontImage { get; set; }

        public UIImage BackImage { get; set; }

        public CardView()
            : base()
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationController.SetNavigationBarHidden(true, false);

            _bnbHeight = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;
            _bnbHeight = _bnbHeight + (Constants.Bottom / 2);
            ScrollViewBottomConstraint.Constant = -(_bnbHeight);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            AddToAppleWalletButton.TouchUpInside -= AddToAppleWalletButton_TouchUpInside;

            FrontScrollView.DidZoom -= FrontScrollView_DidZoom;
            FrontScrollView.ViewForZoomingInScrollView -= FrontScrollView_ViewForZoomingInScrollView;

            BackScrollView.DidZoom -= FrontScrollView_DidZoom;
            BackScrollView.ViewForZoomingInScrollView -= BackScrollView_ViewForZoomingInScrollView;

            ScrollView.Scrolled -= ScrollView_Scrolled;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.NavigationController.NavigationBarHidden = true;

            _viewModel = (CardViewModel)ViewModel;

            _viewModel.ShouldShowAddToWalletButton = !_viewModel.FromLoginScreen && PKAddPassesViewController.CanAddPasses;

            AddToAppleWalletButton.TouchUpInside += AddToAppleWalletButton_TouchUpInside;
            FrontScrollView.DidZoom += FrontScrollView_DidZoom;
            FrontScrollView.ViewForZoomingInScrollView += FrontScrollView_ViewForZoomingInScrollView;
            BackScrollView.DidZoom += FrontScrollView_DidZoom;
            BackScrollView.ViewForZoomingInScrollView += BackScrollView_ViewForZoomingInScrollView;

            ScrollView.Scrolled += ScrollView_Scrolled;

            if (Constants.IsPhone())
            {
                SetLabels();
            }
            else
            {
                SetLabelsForIPad();
            }

            PageControl.CurrentPageIndicatorTintColor = Colors.BottomNavBarBackgroundColor;
            PageControl.PageIndicatorTintColor = Colors.DarkGrayColor;
            PageControl.Pages = 2;

            SetBindings();

            if (!string.IsNullOrWhiteSpace(_viewModel.FrontIdCardImagePath))
            {
                FrontImage = GetImage(_viewModel.FrontIdCardImagePath);
                IDCardFrontImageView.Image = FrontImage;
            }

            if (!string.IsNullOrWhiteSpace(_viewModel.BackIdCardImagePath))
            {
                BackImage = GetImage(_viewModel.BackIdCardImagePath);
                IDCardBackImageView.Image = BackImage;
            }

            _viewModel.PropertyChanged += _viewModel_PropertyChanged;
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            if (!string.IsNullOrEmpty(_viewModel.FrontIdCardImagePath))
            {
                FrontImage = GetImage(_viewModel.FrontIdCardImagePath);
                IDCardFrontImageView.Image = FrontImage;

                var availableHeight = this.View.Frame.Size.Height - _bnbHeight - ScrollView.Frame.Y - 40 - 40 - 51 - 10;

                var targetSize = CalculateRequireSize(availableHeight);
                _requiredWidth = (float)targetSize.Width;
                _requiredHeight = (float)targetSize.Height;
                IdCardFrontImageViewHeightConstraint.Constant = _requiredHeight;
                IdCardFrontImageViewWidthConstraint.Constant = _requiredWidth;
            }

            if (!string.IsNullOrWhiteSpace(_viewModel.BackIdCardImagePath))
            {
                BackImage = GetImage(_viewModel.BackIdCardImagePath);
                IDCardBackImageView.Image = BackImage;
            }

            SetConstraints();
        }

        private void _viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.FrontIdCardImagePath))
            {
                FrontImage = GetImage(_viewModel.FrontIdCardImagePath);
                IDCardFrontImageView.Image = FrontImage;
            }
            else if (e.PropertyName == nameof(_viewModel.BackIdCardImagePath))
            {
                BackImage = GetImage(_viewModel.BackIdCardImagePath);
                IDCardBackImageView.Image = BackImage;
            }
        }

        private UIView BackScrollView_ViewForZoomingInScrollView(UIScrollView scrollView)
        {
            return IDCardBackContainer;
        }

        private UIView FrontScrollView_ViewForZoomingInScrollView(UIScrollView scrollView)
        {
            return IdCardFrontContainer;
        }

        private void ScrollView_Scrolled(object sender, EventArgs e)
        {
            var pageOffset = currentPage > 0 ? ScrollView.Frame.Size.Width / 2 : 0;
            PageControl.CurrentPage = (int)Math.Floor((ScrollView.ContentOffset.X + pageOffset) / ScrollView.Frame.Size.Width);

            if (PageControl.CurrentPage != currentPage)
            {
                currentPage = (int)PageControl.CurrentPage;
                if (currentPage == 0)
                {
                    BackScrollView.SetZoomScale(1, true);
                }
                else
                {
                    FrontScrollView.SetZoomScale(1, true);
                }
            }
        }

        private void FrontScrollView_DidZoom(object sender, EventArgs e)
        {
            var scrollView = sender as UIScrollView;
            if (scrollView.ZoomScale > 1.0f)
            {
                scrollView.ScrollEnabled = true;
                PageControl.Hidden = true;
                AddToAppleWalletButton.Hidden = true;
            }
            else
            {
                scrollView.ScrollEnabled = false;
                PageControl.Hidden = false;
                AddToAppleWalletButton.Hidden = false;
            }
        }
             
        private CGSize CalculateRequireSize(nfloat availableHeight)
        {
            var imageWidth = FrontImage.Size.Width;
            var imageHeight = FrontImage.Size.Height;
            var viewWidth = UIScreen.MainScreen.Bounds.Width;

            float percentagePadding = 0.10f;
            do
            {
                _requiredWidth = (float)(viewWidth - 2 * (viewWidth * percentagePadding));
                _requiredHeight = (float)(imageHeight * _requiredWidth / imageWidth);
                percentagePadding = percentagePadding + 0.01f;
            }
            while (_requiredHeight > availableHeight);

            return new CGSize(_requiredWidth, _requiredHeight);
        }

        private void SetConstraints()
        {
            var assumedWidthDuringDesign = 334f;
            var assumedHeightDuringDesign = 547.67f;

            var trailingParticipant = 180f;
            var leadingParticipant = 138f;
            var topParticipant = 47f;
            var bottomParticipant = 26.67f;

            var trailingSpouse = 226f;
            var leadingSpouse = 92f;

            var leadingOtherInfo = 21f;
            var trailingOtherInfo = 291f;
            var topOtherInfo = 34f;
            var bottomOtherInfo = 96.67f;

            var leadingId = 276f;
            var topId = 337f;
            var trailingId = 42f;
            var bottomId = 26.67f;

            var newTrailingParticipant = _requiredWidth * trailingParticipant / assumedWidthDuringDesign;
            var newLeadingParticipant = _requiredWidth * leadingParticipant / assumedWidthDuringDesign;
            var newTopParticipant = _requiredHeight * topParticipant / assumedHeightDuringDesign;
            var newBottomParticipant = _requiredHeight * bottomParticipant / assumedHeightDuringDesign;

            var newTrailingSpouse = _requiredWidth * trailingSpouse / assumedWidthDuringDesign;
            var newLeadingSpouse = _requiredWidth * leadingSpouse / assumedWidthDuringDesign;

            var newtrailingOtherInfo = _requiredWidth * trailingOtherInfo / assumedWidthDuringDesign;
            var newleadingOtherInfo = _requiredWidth * leadingOtherInfo / assumedWidthDuringDesign;
            var newtopOtherInfo = _requiredHeight * topOtherInfo / assumedHeightDuringDesign;
            var newbottomOtherInfo = _requiredHeight * bottomOtherInfo / assumedHeightDuringDesign;

            var newLeadingId = _requiredWidth * leadingId / assumedWidthDuringDesign;
            var newTrailingId = _requiredWidth * trailingId / assumedWidthDuringDesign;
            var newTopId = _requiredHeight * topId / assumedHeightDuringDesign;
            var newBottomId = _requiredHeight * bottomId / assumedHeightDuringDesign;

            NameViewLeadingConstraint.Constant = newLeadingParticipant;
            NameViewTrailingConstraint.Constant = newTrailingParticipant;
            NameViewTopConstraint.Constant = newTopParticipant;
            NameViewBottomConstraint.Constant = newBottomParticipant;

            SpouseDisplayNameContainerLeadingConstraint.Constant = newLeadingSpouse;
            SpouseDisplayNameContainerTrailingConstraint.Constant = newTrailingSpouse;
            SpouseDisplayNameContainerTopConstraint.Constant = newTopParticipant;
            SpouseDisplayNameConstraintBottomConstraint.Constant = newBottomParticipant;

            InfoViewLeadingConstraint.Constant = newleadingOtherInfo;
            InfoViewTrailingConstraint.Constant = newtrailingOtherInfo;
            InfoViewTopConstraint.Constant = newtopOtherInfo;
            InfoViewBottomConstraint.Constant = newbottomOtherInfo;

            ParticiapntIdLeadingConstraint.Constant = newLeadingId;
            ParticipantIdTrailingConstraint.Constant = newTrailingId;
            ParticipantIdTopConstraint.Constant = newTopId;
            ParticipantIdBottomConstraint.Constant = newBottomId;
        }

        private void SetLabels()
        {
            var screenWidth = UIScreen.MainScreen.Bounds.Width;
            float fontScale = Constants.IsPhone() ? (float)screenWidth / 320f : 1.5f;

            var fontName = "ArialMT";
            float participantLabelFontSize = 14f * (float)fontScale;
            float otherFontSize = 11.0f * (float)fontScale;

            NameView.Label.Font = UIFont.FromName(fontName, participantLabelFontSize);
            NameView.Label.AdjustsFontSizeToFitWidth = true;
            NameView.Label.TextColor = Colors.HIGHLIGHT_COLOR;

            SpouseDisplayNameContainerView.Label.Font = UIFont.FromName(fontName, participantLabelFontSize);
            SpouseDisplayNameContainerView.Label.AdjustsFontSizeToFitWidth = true;
            SpouseDisplayNameContainerView.Label.TextColor = Colors.HIGHLIGHT_COLOR;
            SpouseDisplayNameContainerView.Label.SizeToFit();

            InfoView.Label.Font = UIFont.FromName(fontName, otherFontSize);
            InfoView.Label.AdjustsFontSizeToFitWidth = true;
            InfoView.Label.TextColor = Colors.HIGHLIGHT_COLOR;

            ParticipantIdContainerView.Label.Font = UIFont.FromName(fontName, otherFontSize);
            ParticipantIdContainerView.Label.TextAlignment = UITextAlignment.Right;
            ParticipantIdContainerView.Label.AdjustsFontSizeToFitWidth = true;
            ParticipantIdContainerView.Label.TextColor = Colors.HIGHLIGHT_COLOR;
            ParticipantIdContainerView.Label.SizeToFit();
        }

        private void SetLabelsForIPad()
        {
            var fontName = "ArialMT";
            var scale = 1.5;
            float participantLabelFontSize = 13.33f * (float)scale;
            float otherFontSize = 12f * (float)scale;

            ParticipantDisplayName2Label.Font = UIFont.FromName(fontName, participantLabelFontSize);
            ParticipantDisplayName2Label.TextColor = Colors.HIGHLIGHT_COLOR;

            SpouseDisplayName2Label.Font = UIFont.FromName(fontName, participantLabelFontSize);
            SpouseDisplayName2Label.TextColor = Colors.HIGHLIGHT_COLOR;

            ParticipantId2Label.Font = UIFont.FromName(fontName, otherFontSize);
            ParticipantId2Label.TextColor = Colors.HIGHLIGHT_COLOR;

            Info2Label.Font = UIFont.FromName(fontName, otherFontSize);
            Info2Label.TextColor = Colors.HIGHLIGHT_COLOR;
        }

        private void SetBindings()
        {
            var stringToUIImageConverter = new ImageFilePathToUIImageViewConverter();            
            var boolOppositeValueConverter = new BoolOppositeValueConverter();

            var set = this.CreateBindingSet<CardView, CardViewModel>();

            set.Bind(NameView).For(x => x.LabelText).To(vm => vm.PlanMemberName);
            set.Bind(SpouseDisplayNameContainerView).For(x => x.LabelText).To(vm => vm.PlanMemberSpouseName);
            set.Bind(InfoView).For(x => x.LabelText).To(vm => vm.BottomCardText);
            set.Bind(ParticipantIdContainerView).For(x => x.LabelText).To(vm => vm.CertificateNo);

            set.Bind(CloseButton).To(vm => vm.CloseCommand);
            set.Bind(CloseButton).For(x => x.Hidden).To(vm => vm.FromLoginScreen).WithConversion(boolOppositeValueConverter, null);

            set.Bind(ParticipantDisplayName2Label).To(vm => vm.PlanMemberName);
            set.Bind(SpouseDisplayName2Label).To(vm => vm.PlanMemberSpouseName);
            set.Bind(ParticipantId2Label).To(vm => vm.CertificateNo);
            set.Bind(Info2Label).To(vm => vm.BottomCardText);

            set.Bind(AddToAppleWalletButton).For(x => x.Hidden).To(vm => vm.ShouldShowAddToWalletButton).WithConversion(boolOppositeValueConverter, null);

            set.Apply();
        }

        private UIImage GetImage(string path)
        {
            var image = UIImage.FromFile(path);
            if (!Constants.IsPhone())
            {
                return image;
            }

            var radian = 90 * (float)Math.PI / 180;
            var view = new UIView(new CGRect(0, 0, image.Size.Width, image.Size.Height));
            var transform = CGAffineTransform.MakeRotation(radian);
            view.Transform = transform;
            var size = view.Frame.Size;

            UIGraphics.BeginImageContext(size);
            var context = UIGraphics.GetCurrentContext();
            context.TranslateCTM(size.Width / 2, size.Height / 2);
            context.RotateCTM(radian);
            context.ScaleCTM(1, -1);
            context.DrawImage(new CGRect(-image.Size.Width / 2, -image.Size.Height / 2, image.Size.Width, image.Size.Height), image.CGImage);

            var duplicateImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return duplicateImage;
        }
        
  
        private async void AddToAppleWalletButton_TouchUpInside(object sender, EventArgs e)
        {
            var planMember = Mvx.IoCProvider.Resolve<IParticipantService>().PlanMember;

            if (planMember == null)
            {
                Mvx.IoCProvider.Resolve<IUserDialogService>().Alert("connectionError".tr());
                return;
            }

            var planMemberId = planMember.PlanMemberID;
            var parameters = new Dictionary<string, string>();
            parameters.Add("description", LocalizableBrand.WalletPKPassDescription);
            parameters.Add("logoText", NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleDisplayName").ToString());

            ApiClient<HttpResponseMessage> service = new ApiClient<HttpResponseMessage>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL), HttpMethod.Post, string.Format("/" + GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/pkpass", planMemberId), apiBody: parameters, useDefaultHeaders: true);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/vnd.apple.pkpass");
            await RetrieveWalletCard(planMemberId, service);
        }

        private async Task RetrieveWalletCard(string planMemberId, ApiClient<HttpResponseMessage> service)
        {
            try
            {
                var response = await service.ExecuteRequest();

                string trimmedPlanMemberID = planMemberId;
                if (trimmedPlanMemberID.IndexOf('-') > -1)
                {
                    trimmedPlanMemberID = trimmedPlanMemberID.Substring(0, trimmedPlanMemberID.IndexOf('-'));
                }

                byte[] result = await response.Content.ReadAsByteArrayAsync();

                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var directoryname = Path.Combine(documents, "..", "Library", "Caches");
                var files = Path.Combine(directoryname, string.Format("{0}.pkpass", trimmedPlanMemberID));
                File.WriteAllBytes(files, result);

                AddingToWallet(trimmedPlanMemberID, directoryname);
            }
            catch (Exception ex)
            {
                new UIAlertView("Error", "Unable to retrieve pass", null, "OK", null);
            }
        }

        private void AddingToWallet(string trimmedPlanMemberID, string directoryname)
        {
            if (!PKPassLibrary.IsAvailable)
            {
                return;
            }

            if (!PKAddPassesViewController.CanAddPasses)
            {
                return;
            }

            try
            {
                var newFilePath = Path.Combine(directoryname, string.Format("{0}.pkpass", trimmedPlanMemberID));
                var builtInPassPath = Path.Combine(Environment.CurrentDirectory, string.Format("{0}.pkpass", trimmedPlanMemberID));

                NSData nsdata;
                using (FileStream oStream = File.Open(newFilePath, FileMode.Open))
                {
                    nsdata = NSData.FromStream(oStream);
                }

                var err = new NSError(new NSString("42"), -42);
                var newPass = new PKPass(nsdata, out err);

                var library = new PKPassLibrary();
                bool alreadyExists = library.Contains(newPass);

                if (alreadyExists)
                {
                    library.Remove(newPass);
                    var pkapvc = new PKAddPassesViewController(newPass);
                    NavigationController.PresentModalViewController(pkapvc, true);
                }
                else
                {

                    var pkapvc = new PKAddPassesViewController(newPass);
                    NavigationController.PresentModalViewController(pkapvc, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION ERROR : {0}", ex.ToString());
                Mvx.IoCProvider.Resolve<IUserDialogService>().Alert("connectionError".tr());
            }
        }
    }
}
