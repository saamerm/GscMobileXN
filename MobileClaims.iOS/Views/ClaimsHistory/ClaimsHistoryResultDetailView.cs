using Carousels;
using Cirrious.FluentLayouts.Touch;
using MobileClaims.Core;
using MobileClaims.Core.Entities.ClaimsHistory;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services.ClaimsHistory;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using MobileClaims.iOS.UI;
using MobileClaims.iOS.UI.ClaimHistoryComponents;
using System;
using System.Collections.Generic;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugin.Messenger;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimsHistory
{
    public class ClaimsHistoryResultDetailView : GSCBaseViewController
    {
        private LeagueGothic24Label currentClaimTxt;
        private LeagueGothic24Label ofLab;
        private LeagueGothic24Label totalClaimsTxt;
        private LeagueGothic24Label selectedParticipantTxt;
        private IMvxMessenger _messenger;

        private CustomCarousel carousel;
        private iCarouselDataSource source;

        private UIScrollView rootScrollView;
        private float topMargin;
        private float leftMargin = 20f;
        private float bottomMargin;
        private float belowMargin = 10f;
        private UIViewController pageViewController;
        private float pageIndicatorHeight = 20f;
        private int countOfResult;
        private float lefPanelWidth;
        private bool isRotating = false;
        private UIActivityIndicatorView indicator;
        public UIView activityIndicatorContainer;
        private bool isFromStack = false;
        private UIInterfaceOrientation orientationFromStack;
        private int selectedItemIndex = -1;
        private bool isDoinglayout = false;
        private MvxSubscriptionToken _requestedclaimhistoryresultdetailclose;
        private ClaimsHistoryResultDetailViewModel model;

        private float screenWidth = (float)UIScreen.MainScreen.Bounds.Width;
        private float heightOutOfRootScrollView = 0f;
        private List<ClaimHistoryDetailComponent> coll;
        private IClaimsHistoryService _claimshistoryservice;

        private readonly UIPageControl pageControl = new UIPageControl
        {
            CurrentPageIndicatorTintColor = Colors.HIGHLIGHT_COLOR,
            PageIndicatorTintColor = Colors.DARK_GREY_COLOR
        };

        private int page;

        public int Page
        {
            get { return page; }
            set
            {
                pageControl.CurrentPage = value;
                page = value;
            }
        }

        public ClaimsHistoryResultDetailView()
        {
            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            _claimshistoryservice = Mvx.IoCProvider.Resolve<IClaimsHistoryService>();
            _requestedclaimhistoryresultdetailclose = _messenger.Subscribe<RequestClaimHistoryResultDetailBack>((message) =>
            {
                _messenger.Unsubscribe<RequestClaimHistoryResultDetailBack>(_requestedclaimhistoryresultdetailclose);
            });
        }

        void CreateBindings()
        {
            var set = this.CreateBindingSet<ClaimsHistoryResultDetailView, ClaimsHistoryResultDetailViewModel>();
            set.Bind(totalClaimsTxt).To(vm => vm.SearchResults).WithConversion("ClaimHistorySearchResultsToCount");
            set.Bind(selectedParticipantTxt).To(vm => vm.SelectedParticipant.FullName);
            set.Apply();
        }

        void RootScrollView()
        {
            rootScrollView = ((GSCFluentLayoutBaseView)View).baseScrollContainer;
            rootScrollView.ScrollEnabled = true;
            View.Add(rootScrollView);
        }

        CarouselDelegate c_delegate = new CarouselDelegate();
        void CreateCarousel()
        {
            carousel = new CustomCarousel();
            carousel.ParentController = this;
            carousel.BackgroundColor = Colors.BACKGROUND_COLOR;
            carousel.Type = iCarouselType.Linear;
            carousel.StopAtItemBoundary = true;
            carousel.PagingEnabled = true;
            carousel.ClipsToBounds = true;
            carousel.Delegate = c_delegate;
            //c_delegate.OnCurrentItemIndexChanged += Carousel_CurrentItemIndexChanged;
            //carousel.CurrentItemIndexChanged += Carousel_CurrentItemIndexChanged;
            if (Constants.IsPhone())
            {
                rootScrollView.AddSubview(carousel);
            }
            else
            {
                View.AddSubview(carousel);
            }
        }

        void CreateLabels()
        {
            selectedParticipantTxt = new LeagueGothic24Label();
            currentClaimTxt = new LeagueGothic24Label();
            ofLab = new LeagueGothic24Label();
            ofLab.Text = model.Of;
            ofLab.TextColor = Colors.MED_GREY_COLOR;
            totalClaimsTxt = new LeagueGothic24Label();
            if (Constants.IsPhone())
            {
                rootScrollView.AddSubview(selectedParticipantTxt);
                rootScrollView.AddSubview(currentClaimTxt);
                rootScrollView.AddSubview(ofLab);
                rootScrollView.AddSubview(totalClaimsTxt);

                rootScrollView.AddSubview(pageControl);
            }
            else
            {
                View.AddSubview(selectedParticipantTxt);
                View.AddSubview(currentClaimTxt);
                View.AddSubview(ofLab);
                View.AddSubview(totalClaimsTxt);

                View.AddSubview(pageControl);
            }


        }

        private void SetupConstraints()
        {
            topMargin = 10f + Constants.NAV_HEIGHT;
            bottomMargin = Helpers.BottomNavHeight();
            heightOutOfRootScrollView = bottomMargin + topMargin + pageIndicatorHeight * 3;
            View.RemoveConstraints(View.Constraints);
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            try
            {
                if (Constants.IsPhone())
                {
                    rootScrollView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
                    if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
                    {
                        View.AddConstraints(
                            rootScrollView.AtTopOf(View, View.SafeAreaInsets.Top),
                            rootScrollView.AtLeftOf(View, View.SafeAreaInsets.Left),
                            rootScrollView.AtRightOf(View, View.SafeAreaInsets.Right),
                            rootScrollView.AtBottomOf(View, Helpers.BottomNavHeight()),

                            //selectedParticipantTxt.AtTopOf(rootScrollView, View.SafeAreaInsets.Top > 0 ? 30 : 10),
                            selectedParticipantTxt.AtTopOf(rootScrollView, 10),
                            selectedParticipantTxt.AtLeftOf(rootScrollView, leftMargin));
                    }
                    else
                    {
                        View.AddConstraints(
                            rootScrollView.AtTopOf(View, topMargin),
                            rootScrollView.AtLeftOf(View),
                            rootScrollView.AtRightOf(View),
                            rootScrollView.AtBottomOf(View, Constants.NAV_BUTTON_SIZE_IPHONE),

                            selectedParticipantTxt.AtTopOf(rootScrollView, 10),
                            selectedParticipantTxt.AtLeftOf(rootScrollView, leftMargin));
                    }

                    View.AddConstraints(
                        currentClaimTxt.ToLeftOf(ofLab).Minus(leftMargin / 4),
                        currentClaimTxt.WithSameCenterY(selectedParticipantTxt),

                        ofLab.ToLeftOf(totalClaimsTxt).Minus(leftMargin / 4),
                        ofLab.WithSameCenterY(currentClaimTxt),

                        totalClaimsTxt.Right().EqualTo().RightOf(View).Minus(leftMargin),
                        totalClaimsTxt.WithSameCenterY(ofLab),

                        carousel.AtTopOf(selectedParticipantTxt, 20),
                        carousel.AtLeftOf(rootScrollView),
                        carousel.WithSameWidth(rootScrollView),
                        carousel.WithSameHeight(rootScrollView).Minus(bottomMargin),

                        pageControl.WithSameCenterX(carousel),
                        pageControl.Below(carousel),
                        pageControl.WithSameWidth(carousel),
                        pageControl.Height().EqualTo(pageIndicatorHeight)
                   );
                }
                else
                {
                    View.AddConstraints(
                        carousel.AtTopOf(View, topMargin + 30),
                        carousel.AtLeftOf(View),
                        carousel.WithSameWidth(View),
                        carousel.WithSameHeight(View).Minus(heightOutOfRootScrollView),

                        selectedParticipantTxt.AtTopOf(View, topMargin),
                        selectedParticipantTxt.AtLeftOf(View, leftMargin),

                        currentClaimTxt.ToLeftOf(ofLab).Minus(leftMargin / 4),
                        currentClaimTxt.WithSameCenterY(selectedParticipantTxt),

                        ofLab.ToLeftOf(totalClaimsTxt).Minus(leftMargin / 4),
                        ofLab.WithSameCenterY(currentClaimTxt),

                        totalClaimsTxt.Right().EqualTo().RightOf(View).Minus(leftMargin),
                        totalClaimsTxt.WithSameCenterY(ofLab),

                        pageControl.WithSameCenterX(carousel),
                        pageControl.Below(carousel),
                        pageControl.WithSameWidth(carousel),
                        pageControl.Height().EqualTo(pageIndicatorHeight)
                    );
                }
                View.LayoutSubviews();
            }
            catch (Exception ex)
            {
                MvxTrace.Trace(ex.ToString());
            }
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            //SetupConstraints();
            base.DidRotate(fromInterfaceOrientation);
            View.LayoutSubviews();

            (carousel.DataSource as ClaimsHistoryCarouselDatasource)._viewbounds = carousel.Bounds;

            carousel.ReloadData();

            carousel.ScrollToItemAt(model.SearchResults.IndexOf(_claimshistoryservice.SelectedSearchResult), false);
            View.LayoutSubviews();
            carousel.LayoutSubviews();
            carousel.AutosizesSubviews = true;
        }
        private bool allowautorotation = true;
        public override bool ShouldAutorotate()
        {
            return allowautorotation;
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            if (!allowautorotation)
            {
                return UIInterfaceOrientationMask.Portrait;
            }
            else
            {
                return UIInterfaceOrientationMask.All;
            }
        }


        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetupConstraints();
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            model = ViewModel as ClaimsHistoryResultDetailViewModel;

            this.NavigationItem.SetHidesBackButton(false, false);
            this.NavigationItem.Title = Resource.ClaimDetails;

            foreach (ClaimState cs in model.SearchResults)
            {
                if (cs.IsStricken)
                {
                    MvxTrace.Trace(cs.ClaimFormID.ToString());
                }
            }
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
            {
                if (UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.Portrait || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.PortraitUpsideDown)
                {
                    allowautorotation = false;
                }
            }
            View = new GSCFluentLayoutBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            model = ViewModel as ClaimsHistoryResultDetailViewModel;
            pageControl.Pages = (model.SearchResults.Count > 10) ? 10 : model.SearchResults.Count;
            if (Constants.IsPhone())
            {
                RootScrollView();
            }
            CreateCarousel();
            CreateLabels();
            CreateBindings();
            SetupConstraints();
        }

        public override void ViewDidAppear(bool animated)
        {
            if (View.Superview != null)
            {
                View.Superview.LayoutSubviews();
            }

            carousel.DataSource = new ClaimsHistoryCarouselDatasource(model, carousel.Bounds);
            carousel.ReloadData();
            View.LayoutSubviews();
            carousel.LayoutSubviews();
            carousel.ScrollToItemAt(model.SearchResults.IndexOf(_claimshistoryservice.SelectedSearchResult), false);
            currentClaimTxt.Text = (carousel.CurrentItemIndex + 1).ToString();
            DidRotate((UIInterfaceOrientation)UIDevice.CurrentDevice.Orientation);
            base.ViewDidAppear(animated);
        }

        public void Carousel_CurrentItemIndexChanged(object sender, EventArgs e)
        {
            currentClaimTxt.Text = (carousel.CurrentItemIndex + 1).ToString();
            model.SelectedSearchResult = model.SearchResults[((int)carousel.CurrentItemIndex)];
            pageControl.CurrentPage = carousel.CurrentItemIndex + 1;
            MvxTrace.Trace(carousel.CurrentItemIndex.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            carousel.CurrentItemIndexChanged -= Carousel_CurrentItemIndexChanged;
            base.Dispose(disposing);
        }
    }
    public class CustomCarousel : iCarousel, IiCarouselDelegate
    {
        public UIViewController ParentController { get; set; }
        public override void SubviewAdded(UIView uiview)
        {
            base.SubviewAdded(uiview);

        }
        public override nint CurrentItemIndex
        {
            get
            {
                return base.CurrentItemIndex;
            }
            set
            {
                base.CurrentItemIndex = value;
            }
        }
        public override void WillRemoveSubview(UIView uiview)
        {
            base.WillRemoveSubview(uiview);
            uiview.RemoveConstraints(uiview.Constraints);
        }
        public override nfloat ItemWidth
        {
            get
            {
                return (nfloat)(this.DataSource as ClaimsHistoryCarouselDatasource)._viewbounds.Width;
            }
        }
    }
    public class CarouselDelegate : iCarouselDelegate
    {
        public override nfloat GetItemWidth(iCarousel carousel)
        {
            return carousel.Frame.Width;
        }
        public override void OnCurrentItemIndexChanged(iCarousel carousel)
        {

            var c = carousel as CustomCarousel;
            (c.ParentController as ClaimsHistoryResultDetailView).Carousel_CurrentItemIndexChanged(carousel, new EventArgs());
        }

    }
}

