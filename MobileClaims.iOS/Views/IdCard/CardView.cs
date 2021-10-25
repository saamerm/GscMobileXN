using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Acr.MvvmCross.Plugins.UserDialogs;
using Acr.UserDialogs;
using CoreGraphics;
using Foundation;
using Microsoft.AppCenter.Crashes;
using MobileClaims.Core;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using ObjCRuntime;
using PassKit;
using UIKit;

namespace MobileClaims.iOS.Views
{
    [Foundation.Register("CardView")]
    public class CardView : GSCBaseViewController
    {
        private int currentPage = 0;
        private IDataService _dataservice;
        private ICardService _cardService;

        private IUserDialogs _userDialogService;

        private CardViewModel _model;

        protected IDCard idcard;
        protected bool didRotate = false;
        protected int lastPage = 1;

        protected UIScrollView ScrollView;
        protected UIScrollView CardFrontScrollView;
        protected UIScrollView CardBackScrollView;
        protected UIScrollView DependentsScrollView = new UIScrollView();

        protected UIImageView cardFront;
        protected UIImageView cardBack;

        protected UIView cardContainerFront;
        protected UIView cardContainerBack;

        protected UIActivityIndicatorView ActivityIndicator;
        protected UIPageControl PageControlForScrollView;

        PKPassLibrary library;
        NSObject noteCenter;

        protected UILabel Name = new UILabel();
        protected UILabel PlanMemberID = new UILabel();
        protected UILabel ClientName = new UILabel();
        protected UILabel Comment = new UILabel();
        protected UIImageView Logo = new UIImageView();
        protected UIImageView LogoRight = new UIImageView();
        protected UILabel TravelNumber = new UILabel();
        protected UIButton frontExitLabel = new UIButton();
        protected UIButton appleButton = new UIButton();

        protected float screenWidth = (float)UIScreen.MainScreen.Bounds.Width;
        protected float screenHeight = (float)UIScreen.MainScreen.Bounds.Height;
        protected float FontScale;  //determines size of font
        protected float CardScale = 0.7f;   //determines size of card view
        protected float CardScaleIPad = 0.5f;
        protected float scale; //used to constrain images to screen size
        protected float TextHeight; //height of every label
        protected float FrontFontSize;
        protected float BackFontSize;

        protected bool init = false;

        public override void ViewDidLayoutSubviews()
        {
            if (init)
            {
                this.LayoutSubviews();
            }

            base.ViewDidLayoutSubviews();
        }

        public override void WillRotate(UIInterfaceOrientation toInterfaceOrientation, double duration)
        {
            base.WillRotate(toInterfaceOrientation, duration);
            lastPage = (int)PageControlForScrollView.CurrentPage;
            didRotate = true;
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.LightContent;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View = new UIView();
            View.BackgroundColor = Colors.BACKGROUND_COLOR;

            _model = (CardViewModel)ViewModel;

            if (Constants.IS_OS_VERSION_OR_LATER(8, 0) && Helpers.IsInLandscapeMode())
            {
                // all the logic is based on the old ratio so we switch if it is IOS 8 +
                screenWidth = (float)UIScreen.MainScreen.Bounds.Height;
                screenHeight = (float)UIScreen.MainScreen.Bounds.Width;
            }

            FontScale = Constants.IsPhone() ? screenWidth / 320 : 1.5f;
            TextHeight = 15 * FontScale;
            FrontFontSize = 14 * FontScale;
            BackFontSize = 12 * FontScale;

            if (this.NavigationController != null)
            {
                this.NavigationController.NavigationBarHidden = true;
            }

            if (_model.Card == null || _model.Card.FrontImageFilePath == null || _model.Card.BackImageFilePath == null)
            {
                ShowActivityIndicator();
                _model.PropertyChanged += ViewModelPropertyChanged;
            }
            else
            {
                DoInit();
            }
        }

        private bool _shouldLoadView = true;
        
        private async void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!init && _model.Card.FrontImageFilePath != null && _model.Card.BackImageFilePath != null)
            {
                await DoInit();
            }
            else if (init
                && ((!string.IsNullOrEmpty(_model.Card.FrontLeftLogoFilePath) && Logo.Image == null)
                    || (!string.IsNullOrEmpty(_model.Card.FrontLeftLogoFilePath) && Logo.Image == null)))
            {
                await DoInit();
            }
        }

        private async Task DoInit()
        {
            try
            {
                Init();
            }
            catch (Exception ex)
            {
                _shouldLoadView = false;
                _userDialogService = Mvx.IoCProvider.Resolve<IUserDialogs>();
                var confirmConfig = new Acr.UserDialogs.ConfirmConfig
                {
                    Title = Resource.GenericErrorDialogTitle,
                    Message = Resource.GenericErrorDialogMessage,
                    OkText = "Send report",
                    CancelText = "Do not send report"
                    
                };
                var crashData = new Dictionary<string, string>();
                var confirmAns = await _userDialogService.ConfirmAsync(confirmConfig);
                if (confirmAns)
                {
                    crashData.Add("PlanMemberId", _model.Card.PlanMemberID);
                }
                Crashes.TrackError(ex, crashData);
                CloseIdCardViewController();
            }
        }

        private void CloseIdCardViewController()
        {
            this.NavigationController.PopViewController(false);
        }

        void Init()
        {
            HideActivityIndicator();
            init = true;

            //build the interface
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
            {
                EdgesForExtendedLayout = UIRectEdge.None;
            }

            //setup main scroller
            SetupMainScrollView();

            //setup exit button
            SetupExitButton();

            // TODO: create ViewModel.
            string[] imagePaths = new string[]
            {
                _model.Card.FrontImageFilePath,
                _model.Card.BackImageFilePath
            };

            // TODO: Remove following code; this should be done through viewmodel
            bool doesFileExist = File.Exists(imagePaths[0]) && File.Exists(imagePaths[1]);
            if (!doesFileExist)
            {
                if (_cardService == null)
                {
                    _cardService = Mvx.IoCProvider.Resolve<ICardService>();
                }

                _cardService.UpdateIdCardImage(_model.Card);
                imagePaths = new string[] { _model.Card.FrontImageFilePath, _model.Card.BackImageFilePath };
            }

            var set = this.CreateBindingSet<CardView, CardViewModel>();

            //add views to main scroller
            for (var i = 0; i < imagePaths.Length; i++)
            {
                //tracks x value of current view
                var curX = i * screenWidth;
                UIView curView = new UIView(new CGRect(0, 0, ScrollView.Frame.Width, ScrollView.Frame.Height));

                //position and scale current image
                UIImageView imageView = new UIImageView(UIImage.FromFile(imagePaths[i]));
                scale = screenWidth / (int)imageView.Image.CGImage.Height * (Constants.IsPhone() ? CardScale : CardScaleIPad);
                imageView.Frame = new CGRect(0, 0, imageView.Image.CGImage.Width * scale, imageView.Image.CGImage.Height * scale);

                imageView.BackgroundColor = Colors.Clear;
                imageView.Opaque = false;

                //holds all elements pertaining to the card and its data
                UIView cardContainerView = new UIView
                    (new CGRect(screenWidth / 2 - imageView.Frame.Width / 2,
                                screenHeight / 2 - imageView.Frame.Height / 2,
                                imageView.Frame.Width,
                                imageView.Frame.Height));

                cardContainerView.Add(imageView);

                //instantiate and position content based on current image
                if (imagePaths[i].IndexOf("front") >= 0)
                {
                    cardFront = imageView;
                    cardFront.BackgroundColor = Colors.Clear;
                    cardFront.Opaque = false;
                    cardContainerFront = cardContainerView;

                    SetClientNameLabel(imageView, cardContainerView);
                    SetPlanMemeberIdLabel(cardContainerView);
                    SetNameLabel(cardContainerView);

                    cardContainerView.Add(ClientName);
                    cardContainerView.Add(PlanMemberID);
                    cardContainerView.Add(Name);

                    var verticalPosition = Name.Frame.Y + Name.Frame.Height;
                    verticalPosition = SetCommentLabel(cardContainerView, verticalPosition);

                    SetFrontLeftLogo(imageView, cardContainerView, verticalPosition);
                    SetFrontRightLogo(cardContainerView);

                    set.Bind(ClientName).To(vm => vm.Card.PlanMemberFullName);
                    set.Bind(PlanMemberID).To(vm => vm.Card.PlanMemberID);
                    set.Bind(Name).To(vm => vm.Card.ClientBusinessName);
                    set.Bind(Comment).To(vm => vm.Card.Comment);
                }
                else if (imagePaths[i].IndexOf("back") >= 0)
                {
                    cardBack = imageView;
                    cardContainerBack = cardContainerView;

                    //setup dependent sscroller
                    SetupDependentScroller(cardContainerView);

                    if (_model.Card.Participants != null)
                    {
                        var participantsCounter = SetAllDependents();

                        // Resizing dependentsScrollView ??
                        DependentsScrollView.ContentSize = new CGSize(DependentsScrollView.Frame.Width - 10, TextHeight * participantsCounter);
                        DependentsScrollView.SizeToFit();

                        cardContainerView.Add(DependentsScrollView);

                        SetTravelNumberLabel(imageView, cardContainerView);

                        set.Bind(TravelNumber).To(vm => vm.Card.TravelGroupPolicyNumber).WithConversion("NumberSign");
                        cardContainerView.Add(TravelNumber);
                        if (string.IsNullOrEmpty(_model.Card.TravelGroupPolicyNumber))
                        {
                            TravelNumber.Hidden = true;
                        }
                        else
                        {
                            TravelNumber.Hidden = false;
                        }
                    }
                }

                //rotate the card view
                if (Helpers.IsInLandscapeMode() && Constants.IsPhone())
                {
                    cardContainerView.Transform = CoreGraphics.CGAffineTransform.MakeRotation((nfloat)(float)System.Math.PI + (float)System.Math.PI / 2);
                }
                cardContainerView.AutosizesSubviews = true;
                cardContainerView.ContentMode = UIViewContentMode.ScaleAspectFit;

                //setup individual card scroller
                UIScrollView curScrollView = new UIScrollView(new CGRect(curX, 0, screenWidth, screenHeight));
                curScrollView.PagingEnabled = false;
                curScrollView.ShowsHorizontalScrollIndicator = false;
                curScrollView.ContentSize = new CGSize((float)ScrollView.Frame.Width, (float)ScrollView.Frame.Height);
                curScrollView.MaximumZoomScale = 3.0F;
                curScrollView.MinimumZoomScale = 1.0F;

                curScrollView.DidZoom += CurScrollView_DidZoom;

                if (i == 0)
                {
                    CardFrontScrollView = curScrollView;
                }
                else
                {
                    CardBackScrollView = curScrollView;
                }

                //define view hierarchy
                curView.AddSubview(cardContainerView);
                curScrollView.AddSubview(curView);
                curScrollView.ViewForZoomingInScrollView += (UIScrollView sv) => { return curView; };
                ScrollView.AddSubview(curScrollView);
                ScrollView.AddSubview(appleButton);

            }

            set.Apply();

            View.AddSubview(ScrollView);

            if (_model.FromLoginScreen)
            {
                View.AddSubview(frontExitLabel);
                appleButton.RemoveFromSuperview();
            }

            var cardBottomY = cardContainerFront.Frame.Y + cardContainerFront.Frame.Height;
            //setup page indicator
            PageControlForScrollView = new UIPageControl();
            PageControlForScrollView.UserInteractionEnabled = false;
            PageControlForScrollView.Frame = new CGRect(
                (View.Frame.Width / 2) - (PageControlForScrollView.Frame.Width / 2),
                (cardBottomY) + 15,
                PageControlForScrollView.Frame.Width,
                30);
            PageControlForScrollView.CurrentPageIndicatorTintColor = Colors.HIGHLIGHT_COLOR;
            PageControlForScrollView.PageIndicatorTintColor = Colors.DARK_GREY_COLOR;
            PageControlForScrollView.Pages = imagePaths.Length;
            this.Add(PageControlForScrollView);
            ScrollView.Scrolled += ScrollEvent;

            if (Constants.IsPhone())
            {
                float viewWidth = (float)View.Frame.Width;
                float cardWidth = (float)cardContainerFront.Frame.Width;
                float cardXPos = (float)cardContainerFront.Frame.X;
                float curScrollXPos = (float)PageControlForScrollView.Frame.X;
                float FIELD_WIDTH = Constants.IsPhone() ? 270 : 400;
                float centerX = viewWidth / 4;
                float fieldXPos = centerX - (float)appleButton.Frame.Width / 2;
                //setup apple wallet button
                appleButton.SetImage(UIImage.FromBundle("cardImage".tr()), UIControlState.Normal);
                appleButton.UserInteractionEnabled = true;
                appleButton.TouchUpInside += HandleButtonClickAsync;
                appleButton.Frame = new CGRect(cardXPos * -3.50, (float)(cardBottomY) + 20, 25, 25);
                appleButton.SizeToFit();
                UIGraphics.BeginImageContext(new SizeF(20, 20));
                appleButton.Draw(new RectangleF(0, 0, 20, 20));
                UIGraphics.EndImageContext();
            }

            this.View.SetNeedsLayout();
        }

        private void CurScrollView_DidZoom(object sender, EventArgs e)
        {
            var curScrollview = sender as UIScrollView;
            if (curScrollview.ZoomScale > 1.0F)
            {
                PageControlForScrollView.Hidden = true;
                curScrollview.ScrollEnabled = true;
            }
            else
            {
                PageControlForScrollView.Hidden = false;
                ScrollView.ScrollEnabled = true;
                curScrollview.ScrollEnabled = false;
            }
        }

        private void SetupMainScrollView()
        {
            ScrollView = new UIScrollView(new CGRect(0, 0, screenWidth, screenHeight))
            {
                ContentSize = new CGSize(screenWidth * 2, screenHeight),
                PagingEnabled = true,
                ShowsHorizontalScrollIndicator = false,
                UserInteractionEnabled = true,
                MaximumZoomScale = 3.0F,
                MinimumZoomScale = 1.0F,
                ContentInsetAdjustmentBehavior = Constants.IS_OS_VERSION_OR_LATER(11, 0)
                        ? UIScrollViewContentInsetAdjustmentBehavior.Never
                        : ScrollView.ContentInsetAdjustmentBehavior
            };
        }

        private void SetupExitButton()
        {
            frontExitLabel.Frame = new CGRect(5, Constants.IS_OS_7_OR_LATER() ? 30 : 0, 33, 33);
            frontExitLabel.BackgroundColor = Colors.Clear;
            frontExitLabel.SetImage(UIImage.FromBundle("Close"), UIControlState.Normal);
            frontExitLabel.UserInteractionEnabled = true;
            frontExitLabel.TouchUpInside += (sender, e) =>
            {
                _model.CloseCommand.Execute(true);
            };
        }

        private void SetClientNameLabel(UIImageView curImageView, UIView cardContainerView)
        {
            ClientName.Frame = new CGRect(
                curImageView.Frame.Width * 0.05f,
                curImageView.Frame.Height * 0.375f,
                (float)Math.Round(cardContainerView.Frame.Width * 0.9),
                TextHeight);
            ClientName.Text = "MEMBER NAME";
            ClientName.Font = UIFont.FromName("Arial-BoldMT", (nfloat)FrontFontSize);
            ClientName.Layer.BorderColor = Colors.Clear.CGColor;
            ClientName.Layer.BorderWidth = 1.1f;
            ClientName.BackgroundColor = Colors.Clear;
        }

        private void SetPlanMemeberIdLabel(UIView cardContainerView)
        {
            PlanMemberID.Frame = new CGRect(
                ClientName.Frame.X,
                ClientName.Frame.Y + TextHeight + 10f,
                cardContainerView.Frame.Width * 0.9f,
                TextHeight);
            PlanMemberID.Text = "0000000-00";
            PlanMemberID.Font = UIFont.FromName("Arial-BoldMT", (nfloat)FrontFontSize);
            PlanMemberID.BackgroundColor = Colors.Clear;
        }

        private void SetNameLabel(UIView cardContainerView)
        {
            Name.Frame = new CGRect(
                PlanMemberID.Frame.X,
                PlanMemberID.Frame.Y + TextHeight + 3f,
                cardContainerView.Frame.Width * 0.9f,
                TextHeight);
            Name.Text = "Client Name";
            Name.Font = UIFont.FromName("ArialMT", (nfloat)FrontFontSize);
            Name.BackgroundColor = Colors.Clear;
        }

        private nfloat SetCommentLabel(UIView cardContainerView, nfloat yPos)
        {
            Comment.Frame = new CGRect(
                Name.Frame.X, Name.Frame.Y + TextHeight + 3,
                cardContainerView.Frame.Width * 0.9,
                (Constants.SMALL_FONT_SIZE + 1) * FontScale);
            Comment.Text = " ";
            Comment.BackgroundColor = Colors.Clear;
            Comment.Font = UIFont.FromName("ArialMT", Constants.SMALL_FONT_SIZE * FontScale);

            if (string.IsNullOrEmpty(_model.Card.Comment))
            {
                Comment.Hidden = true;
            }
            else
            {
                yPos = (float)Comment.Frame.Y + (float)Comment.Frame.Height;
                cardContainerView.Add(Comment);
                Comment.Hidden = false;
            }

            return yPos;
        }

        private void SetFrontLeftLogo(UIImageView curImageView, UIView cardContainerView, nfloat yPos)
        {
            Logo = new UIImageView();
            cardContainerView.InsertSubviewBelow(Logo, Name);
            Logo.Hidden = true;

            if (!string.IsNullOrEmpty(_model.Card.FrontLeftLogoFilePath))
            {
                Logo.Image = UIImage.FromFile(_model.Card.FrontLeftLogoFilePath);
                Logo.Frame = new CGRect(ClientName.Frame.X,
                    cardFront.Frame.Height - (Logo.Image.CGImage.Height * scale) - 15.0f,
                    Logo.Image.CGImage.Width * scale,
                    Logo.Image.CGImage.Height * scale);

                var availHeight = curImageView.Frame.Height - yPos - 15;
                nfloat newHeight;
                nfloat newScale;

                if ((float)Logo.Frame.Height > availHeight)
                {

                    newHeight = availHeight;
                    newScale = newHeight / (int)Logo.Image.CGImage.Height;

                    Logo.Frame = new CGRect(
                        ClientName.Frame.X,
                        cardFront.Frame.Height - (Logo.Image.CGImage.Height * newScale) - 15.0f,
                        Logo.Image.CGImage.Width * newScale,
                        Logo.Image.CGImage.Height * newScale);
                }
                Logo.Hidden = false;
            }
        }

        private void SetFrontRightLogo(UIView cardContainerView)
        {
            LogoRight = new UIImageView();
            cardContainerView.InsertSubviewBelow(LogoRight, Name);
            LogoRight.Hidden = true;

            if (!string.IsNullOrEmpty(_model.Card.FrontRightLogoFilePath))
            {
                LogoRight.Image = UIImage.FromFile(_model.Card.FrontRightLogoFilePath);
                LogoRight.Frame = new CGRect(
                    cardFront.Frame.Width - (LogoRight.Image.CGImage.Width * scale) - 20.0f,
                    cardFront.Frame.Height - (LogoRight.Image.CGImage.Height * scale) - 15.0f,
                    LogoRight.Image.CGImage.Width * scale,
                    LogoRight.Image.CGImage.Height * scale);
                LogoRight.Hidden = false;
            }
        }

        private void SetupDependentScroller(UIView cardContainerView)
        {
            DependentsScrollView.Frame = (new CGRect(
                cardContainerView.Frame.Width * 0.05f,
                cardContainerView.Frame.Height * 0.37f,
                cardContainerView.Frame.Width * 0.9f,
                cardContainerView.Frame.Height * 0.34f));

            DependentsScrollView.ScrollEnabled = true;
            DependentsScrollView.ContentSize = new CGSize(cardContainerView.Frame.Width * 0.9f, TextHeight * 20);
            DependentsScrollView.ShowsVerticalScrollIndicator =
                DependentsScrollView.ContentSize.Height > DependentsScrollView.Frame.Height ? true : false;
            DependentsScrollView.SizeToFit();
            DependentsScrollView.Hidden = true;

            if (_model.Card.Participants != null)
            {
                if (_model.Card.Participants.Count > 0)
                {
                    DependentsScrollView.Hidden = false;
                }
                else
                {
                    DependentsScrollView.Hidden = true;
                }

                var frame = DependentsScrollView.Frame;

                // TODO: why dependentsScrollView is assigned a new instance of UIScrollView ?????
                DependentsScrollView = new UIScrollView(frame);
            }
        }

        private UILabel SetupDependentLabel(int participantsCounter, IDCardParticipant person)
        {
            var curLabel = new UILabel(
                new CGRect(0, participantsCounter * TextHeight, DependentsScrollView.Frame.Width, TextHeight));
            curLabel.Text = _model.Card.PlanMemberDisplayID + "-" + person.ParticipantNumber + "    " + person.ParticipantFullName.ToUpper();
            curLabel.Font = UIFont.FromName("Arial-BoldMT", BackFontSize);
            curLabel.BackgroundColor = Colors.Clear;
            return curLabel;
        }

        private int SetAllDependents()
        {
            var participantsCounter = 0;
            foreach (IDCardParticipant person in _model.Card.Participants)
            {
                var curLabel = SetupDependentLabel(participantsCounter, person);
                DependentsScrollView.Add(curLabel);
                participantsCounter++;
            }

            return participantsCounter;
        }

        private void SetTravelNumberLabel(UIImageView curImageView, UIView cardContainerView)
        {
            TravelNumber.Font = UIFont.FromName("Arial-BoldMT", FrontFontSize);
            TravelNumber.BackgroundColor = Colors.Clear;
            TravelNumber.TextAlignment = UITextAlignment.Right;
            TravelNumber.Frame = new CGRect(
                curImageView.Frame.Width * 0.05,
                curImageView.Frame.Height * 0.25,
                cardContainerView.Frame.Width * 0.91,
                TextHeight);
        }

        private async void HandleButtonClickAsync(object sender, EventArgs e)
        {
            var planMember = Mvx.IoCProvider.Resolve<IParticipantService>().PlanMember;

            if (planMember == null)
            {
                Mvx.IoCProvider.Resolve<IUserDialogService>().Alert("connectionError".tr());
                return;
            }

            var planMemberId = planMember.PlanMemberID;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
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
                    trimmedPlanMemberID = trimmedPlanMemberID.Substring(0, trimmedPlanMemberID.IndexOf('-'));

                byte[] result = await response.Content.ReadAsByteArrayAsync();

                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var directoryname = Path.Combine(documents, "..", "Library", "Caches");
                var files = Path.Combine(directoryname, string.Format("{0}.pkpass", trimmedPlanMemberID));
                File.WriteAllBytes(files, result);

                AddingToWallet(trimmedPlanMemberID, directoryname);
            }
            catch (Exception)
            {
                var alert = new UIAlertView("Error", "Unable to retrieve pass", null, "OK", null);
            }
        }

        private void AddingToWallet(string trimmedPlanMemberID, string directoryname)
        {
            if (PKPassLibrary.IsAvailable)
            {
                try
                {
                    var newFilePath = Path.Combine(directoryname, string.Format("{0}.pkpass", trimmedPlanMemberID));
                    var builtInPassPath = Path.Combine(System.Environment.CurrentDirectory, string.Format("{0}.pkpass", trimmedPlanMemberID));

                    NSData nsdata;
                    using (FileStream oStream = File.Open(newFilePath, FileMode.Open))
                    {
                        nsdata = NSData.FromStream(oStream);
                    }

                    var err = new NSError(new NSString("42"), -42);
                    var newPass = new PKPass(nsdata, out err);

                    library = new PKPassLibrary();
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

        private void ShowActivityIndicator()
        {
            if (ActivityIndicator == null)
            {
                ActivityIndicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge)
                {
                    BackgroundColor = new UIColor(65 / 255F, 65 / 255F, 65 / 255F, 1F),
                    Opaque = true
                };
                ActivityIndicator.Frame = new CGRect(0, 0, Helpers.OrientationAwareScreenWidth(), Helpers.OrientationAwareScreenHeight());

                Add(ActivityIndicator);
                ActivityIndicator.StartAnimating();
            }
        }

        private void HideActivityIndicator()
        {
            if (ActivityIndicator != null)
            {
                ActivityIndicator.RemoveFromSuperview();
                ActivityIndicator = null;
            }
        }

        private void LayoutSubviews()
        {
            if (!_shouldLoadView)
            {
                return;
            }

            var iWidth = Helpers.IsInLandscapeMode() || (Helpers.IsInPortraitMode() && !Constants.IsPhone()) ? (int)cardBack.Image.CGImage.Width : (int)cardBack.Image.CGImage.Height;
            var iHeight = Helpers.IsInLandscapeMode() || (Helpers.IsInPortraitMode() && !Constants.IsPhone()) ? (int)cardBack.Image.CGImage.Height : (int)cardBack.Image.CGImage.Width;
            var newWidth = Helpers.OrientationAwareScreenWidth();
            var newHeight = Helpers.OrientationAwareScreenHeight() - Helpers.StatusBarHeight();
            if (!_model.FromLoginScreen)
            {
                if (Constants.IS_OS_7_OR_LATER())
                {
                    newHeight += Helpers.BottomNavHeight();
                }
                else
                {
                    newHeight -= Helpers.BottomNavHeight();
                }
            }
            else
            {

                if (Constants.IS_OS_7_OR_LATER())
                {
                    newHeight += Helpers.BottomNavHeight() + 50;
                }
            }

            var viewHieght = (float)base.View.Frame.Height;

            var scrollPageHeight = 30;

            if (Helpers.IsInLandscapeMode() || (Helpers.IsInPortraitMode() && !Constants.IsPhone()))
            {
                cardContainerBack.Transform = CoreGraphics.CGAffineTransform.MakeRotation((nfloat)0);
            }
            else
            {
                cardContainerBack.Transform = CoreGraphics.CGAffineTransform.MakeRotation((nfloat)3.14159f * 90f / 180f);
            }
            cardContainerFront.Transform = cardContainerBack.Transform;

            ScrollView.Frame = new CGRect(0, 10, newWidth, newHeight - Constants.STATUS_BAR_OFFSET);
            ScrollView.ContentSize = new CGSize((float)ScrollView.Frame.Width * 2, (float)ScrollView.Frame.Height);

            CardBackScrollView.Frame = new CGRect((float)ScrollView.Frame.Width, 0, (float)ScrollView.Frame.Width, (float)ScrollView.Frame.Height);
            CardBackScrollView.ContentSize = new CGSize((float)ScrollView.Frame.Width, (float)ScrollView.Frame.Height);
            cardContainerBack.Frame = new CGRect(
                ((float)ScrollView.Frame.Width / 2 - (iWidth * scale) / 2),
(float)ScrollView.Frame.Height / 2.8 - scrollPageHeight / 4.5 - (iHeight * scale) / 2.5 - Constants.STATUS_BAR_CARD_OFFSET,
                iWidth * scale,
                iHeight * scale
            );

            if (Helpers.IsInLandscapeMode() && Constants.IsPhone())
            {
                cardContainerBack.Frame = new CGRect(((float)ScrollView.Frame.Width / 2 - (iWidth * scale) / 2), (float)ScrollView.Frame.Height / 2 - scrollPageHeight / 4 - (iHeight * scale) / 2 - Constants.STATUS_BAR_CARD_OFFSET,
                                                     iWidth * scale, iHeight * scale);
            }

            CardFrontScrollView.Frame = new CGRect(0, 0, (float)ScrollView.Frame.Width, (float)ScrollView.Frame.Height);
            CardFrontScrollView.ContentSize = (CGSize)CardBackScrollView.ContentSize;
            cardContainerFront.Frame = (CGRect)cardContainerBack.Frame;

            var cardBottomY = (float)cardContainerFront.Frame.Y + (float)cardContainerFront.Frame.Height;
            PageControlForScrollView.Frame = new CGRect(
(float)View.Frame.Width / 2 - (float)PageControlForScrollView.Frame.Width / 2,
                (float)(cardBottomY) + (Constants.IsPhone() ? Constants.PHONE_STATUS_CARD_PAGING_OFFSET : 25),
(float)PageControlForScrollView.Frame.Width,
                scrollPageHeight
            );

            if (Constants.IsPhone() && Helpers.IsInPortraitMode() && !_model.FromLoginScreen)
            {
                appleButton.Frame = new CGRect((float)ScrollView.Frame.Width / 2 - (float)appleButton.Bounds.Width / 2, (float)(cardBottomY) + 50, (float)appleButton.Bounds.Width, (float)appleButton.Bounds.Height);
            }

            if (didRotate)
            {
                didRotate = false;
                ScrollView.SetContentOffset((CGPoint)new CGPoint(newWidth * lastPage, 0.0f), false);
            }
        }

        private void ScrollEvent(object sender, System.EventArgs e)
        {
            float pageOffset = currentPage > 0 ? (float)ScrollView.Frame.Size.Width / 2 : 0;
            PageControlForScrollView.CurrentPage = (int)System.Math.Floor(((float)ScrollView.ContentOffset.X + pageOffset) / (float)ScrollView.Frame.Size.Width);
            DependentsScrollView.FlashScrollIndicators();

            if ((int)PageControlForScrollView.CurrentPage != currentPage)
            {
                currentPage = (int)PageControlForScrollView.CurrentPage;
                if (currentPage == 0)
                {
                    CardBackScrollView.SetZoomScale((nfloat)1, true);
                }
                else
                {
                    CardFrontScrollView.SetZoomScale((nfloat)1, true);
                }

                this.View.SetNeedsLayout();
            }
        }
    }
}