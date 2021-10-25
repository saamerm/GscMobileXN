using CoreAnimation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using MobileClaims.iOS.Helper;
using MobileClaims.iOS.Services;
using MobileClaims.iOS.Views;
using MobileClaims.iOS.Views.ClaimsHistory;
using MobileClaims.iOS.Views.Dashboard;
using MobileClaims.iOS.Views.Login;
using MobileClaims.iOS.Views.TrreatmentDetails;
using MobileClaims.iOS.Views.WebClaimSubmissionAgreement;
using MvvmCross;
using MvvmCross.Platform.Platform;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using UIKit;

namespace MobileClaims.iOS.Presenters
{
    public class PhonePresenter : MvxIosViewPresenter, INavViewHeightProvider
    {
        private ContentNavViewController _cnvc;
        private object _sync = new object();
        private bool _navigating = false;
        private UIWindow _window;
        private ClaimsHistoryResultDetailView CHDetailView;

        public nfloat NavViewHeight => _cnvc.ViewNavigationController.View.Bounds.Height;

        public PhonePresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
            _cnvc = new ContentNavViewController();
            window.RootViewController = _cnvc;
            _window = window;
            _cnvc.Window = _window;
        }

        public override Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            if (!_navigating)
            {
                _navigating = true;

                Type viewType;

                if (hint is MvxClosePresentationHint closePresentationHint)
                {
                    var finder = Mvx.IoCProvider.Resolve<IMvxIosViewCreator>();
                    var castFinder = finder as MvxIosViewsContainer;
                    if (castFinder == null)
                    {
                        // TODO: For now not throwing exception
                        // throw new ArgumentNullException(nameof(castFinder));
                        return Task.FromResult(false);
                    }

                    viewType = castFinder.GetViewType(closePresentationHint.ViewModelToClose.GetType());

                    foreach (var childViewController in _window.RootViewController.ChildViewControllers)
                    {
                        if ((childViewController.GetType() == typeof(UINavigationController) ||
                             childViewController.GetType() == typeof(PresenterNavigator))
                            && ((UINavigationController)childViewController).TopViewController != null)
                        {
                            System.Diagnostics.Debug.WriteLine(((UINavigationController)childViewController)
                                .TopViewController.GetType().Name);
                            if (viewType.Name == "ClaimSubmissionTypeView")
                            {
                                UIViewController controller = null;
                                foreach (var subViewController in (childViewController as UINavigationController)
                                    .ViewControllers)
                                {
                                    if (subViewController.GetType().Name.Equals(nameof(ChooseClaimOrHistoryView)))
                                    {
                                        controller = subViewController;
                                        break;
                                    }
                                }

                                if (controller != null)
                                {
                                    ((UINavigationController)childViewController).PopToViewController(controller, false);
                                }
                                else
                                {
                                    ((UINavigationController)childViewController).PopToRootViewController(false);
                                }
                            }
                            else if (viewType.HasAttribute(typeof(FromBottomTransitionAttribute)))
                            {
                                var viewControllerToClose = ((UINavigationController)childViewController).TopViewController.ChildViewControllers.SingleOrDefault(x => x.GetType() == viewType);
                                CloseVertically(viewControllerToClose);
                            }
                            else if (((UINavigationController)childViewController).TopViewController.GetType().Name == viewType.Name)
                            {
                                if (viewType.HasAttribute(typeof(FromLeftToRightTransitionAttribute)))
                                {
                                    var transition = CATransition.CreateAnimation();
                                    transition.Duration = 0.25;
                                    transition.Type = CAAnimation.TransitionPush;
                                    transition.Subtype = CAAnimation.TransitionFromRight;
                                    _cnvc.View.Layer.AddAnimation(transition, CALayer.Transition);
                                    ((UINavigationController)childViewController).TopViewController.NavigationController.PopViewController(true);
                                    _cnvc.ShowNavigation();
                                }

                                var isEntry = string.Equals(viewType.Name, nameof(ClaimTreatmentDetailsEntry1View))
                                              || string.Equals(viewType.Name, nameof(ClaimTreatmentDetailsEntry2View))
                                              || string.Equals(viewType.Name, nameof(ClaimTreatmentDetailsEntryMIView))
                                              || string.Equals(viewType.Name, nameof(ClaimTreatmentDetailsEntryOMFView))
                                              || string.Equals(viewType.Name, nameof(ClaimTreatmentDetailsEntryPCView))
                                              || string.Equals(viewType.Name, nameof(ClaimTreatmentDetailsEntryPGView))
                                              || string.Equals(viewType.Name, nameof(ClaimTreatmentDetailsEntryREEView))
                                              || string.Equals(viewType.Name, nameof(ClaimTreatmentDetailsEntryDentalView))
                                              || string.Equals(viewType.Name, nameof(ClaimDetailsHCSAView));
                              
                                if (string.Equals(viewType.Name, nameof(AuditInformationView))
                                    || string.Equals(viewType.Name, nameof(ClaimTreatmentDetailsListView))
                                    || string.Equals(viewType.Name, nameof(EligibilityBenefitInquiryView))
                                    || string.Equals(viewType.Name, nameof(FindHealthProviderViewController))
                                    || string.Equals(viewType.Name, nameof(HealthProviderTypeListView))
                                    || isEntry)
                                {
                                    ((UINavigationController)childViewController).TopViewController.NavigationController.PopViewController(!isEntry);
                                }

                                if (viewType.Name == nameof(WebAgreementView))
                                {
                                    _cnvc.ShowNavigation();
                                    ((UINavigationController)childViewController).TopViewController.NavigationController.PopViewController(!isEntry);
                                }
                            }
                        }

                        System.Diagnostics.Debug.Write(childViewController.GetType().Name);
                    }
                }

                _navigating = false;
            }

            base.ChangePresentation(hint);
            return Task.FromResult(true);
        }

        public override Task<bool> Show(MvxViewModelRequest request)
        {
            UIViewController viewController;
            IRehydrating vc = null;

            try
            {
                viewController = (UIViewController)Mvx.IoCProvider.Resolve<IMvxIosViewCreator>().CreateView(request);
                var isTopViewController = _cnvc.ContentView.TopViewController?.GetType() == viewController.GetType();

                if (viewController.GetType() == typeof(Dashboard))
                {
                    if (isTopViewController)
                    {
                        // Return if already on top of stack
                        return Task.Factory.StartNew(() => true);
                    }

                    // Bug-4725: If login view is still on the navigation stack, that means we app has just loaded and we wont to remove it from stack
                    // to avoid navigating back to login page. 
                    _cnvc.ContentView.ViewControllers = new UIViewController[] { };
                }

                var existingViewController = _cnvc.ContentView.ViewControllers.SingleOrDefault(x => x.GetType() == viewController.GetType());
                if (existingViewController != null)
                {
                    // Pop to view container and return, if existingViewController is already somewhere in the stack
                    _cnvc.ContentView.PopToViewController(existingViewController, true);
                    return Task.FromResult(false);
                }

                if (viewController.GetType() == typeof(LoginView))
                {
                    _cnvc.ContentView.ViewControllers = new UIViewController[] { };
                }
            }
            catch (Exception ex)
            {
                MvxTrace.Trace(MvxTraceLevel.Error, ex.ToString());
                return Task.FromResult(false);
            }

            if (request.ViewModelType.Name.StartsWith("claim", StringComparison.OrdinalIgnoreCase))
            {
                vc = viewController as IRehydrating;
            }

            var rehydrationService = Mvx.IoCProvider.Resolve<IRehydrationService>();
            var shouldSetRehydration = false;
            string requestedValue = null;
            if (request.PresentationValues != null &&
                request.PresentationValues.TryGetValue(NavigationRequestTypes.RequestedBy, out requestedValue))
            {
                if (string.Equals(requestedValue, NavigationRequestTypes.Rehydration, StringComparison.OrdinalIgnoreCase))
                {
                    shouldSetRehydration = true;
                }
            }

            if (shouldSetRehydration)
            { 
                rehydrationService.Rehydrating = true;
                if (vc != null)
                {
                    vc.Rehydrating = true;
                }
            }
            else
            {
                rehydrationService.Rehydrating = false;
                if (vc != null)
                {
                    vc.Rehydrating = false;
                }
            }

            if (viewController.GetType().HasAttribute(typeof(FromBottomTransitionAttribute)))
            {
                OpenVertically(viewController);
                return Task.Factory.StartNew(() => true);
            }

            // POSITION VIEW MODELS BASED ON TYPE
            if (request.ViewModelType == typeof(MainNavigationViewModel))
            {
                _cnvc.SetNav(viewController);
            }
            else
            {
                if (viewController.GetType().HasAttribute(typeof(FromLeftToRightTransitionAttribute)))
                {
                    var transition = CATransition.CreateAnimation();
                    transition.Duration = 0.25;
                    transition.Type = CAAnimation.TransitionPush;
                    transition.Subtype = CAAnimation.TransitionFromLeft;
                    _cnvc.View.Layer.AddAnimation(transition, CALayer.Transition);
                }

                var hidesNav = false;

                if (request.ViewModelType == typeof(LoginViewModel) ||
                    request.ViewModelType == typeof(CardViewModel) &&
                    _cnvc.ContentView.TopViewController.GetType() == typeof(LoginView) ||
                    request.ViewModelType == typeof(ClaimsHistorySearchViewModel) ||
                    request.ViewModelType == typeof(ClaimsHistoryDisplayByViewModel) ||
                    request.ViewModelType == typeof(ClaimsHistoryParticipantsViewModel) ||
                    request.ViewModelType == typeof(RefineSearchViewModel) ||
                    request.ViewModelType == typeof(HealthProviderTypeListViewModel)
                    || request.ViewModelType == typeof(WebAgreementViewModel))
                {
                    hidesNav = true;
                }

                _cnvc.SetContent(viewController, requestedValue, hidesNav);
            }

            return Task.FromResult(true);
        }

        private void OpenVertically(UIViewController viewController)
        {
            var topController = (FindHealthProviderViewController)_cnvc.ContentView.TopViewController;
            viewController.View.TranslatesAutoresizingMaskIntoConstraints = false;

            if (topController.ChildViewControllers.Any(x => x.GetType() == viewController.GetType()))
            {
                return;
            }

            topController.AddChildViewController(viewController);
            topController.View.AddSubview(viewController.View);

            var topConstraint =
                viewController.View.TopAnchor.ConstraintEqualTo(topController.View.TopAnchor,
                    topController.View.Frame.Height);
            topConstraint.SetIdentifier(viewController.GetType() + "animatedConstraint");
            NSLayoutConstraint.ActivateConstraints(new[]
            {
                topConstraint,
                viewController.View.LeadingAnchor.ConstraintEqualTo(topController.View.LeadingAnchor),
                viewController.View.TrailingAnchor.ConstraintEqualTo(topController.View.TrailingAnchor),
                viewController.View.BottomAnchor.ConstraintEqualTo(topController.View.BottomAnchor)
            });
            topController.View.LayoutIfNeeded();

            UIView.Animate(.3f, 0.0f, UIViewAnimationOptions.CurveEaseOut, () =>
                {
                    var topMargin = viewController.GetType() == typeof(ServiceDetailsListViewController)
                        ? topController.MapView.Frame.Height
                        : topController.View.Frame.Height;
                    topConstraint.Constant = topController.View.Frame.Height - topMargin;
                    topController.View.LayoutIfNeeded();
                },
                () =>
                {
                    topController.View.LayoutIfNeeded();
                    viewController.DidMoveToParentViewController(topController);
                });
        }

        private void CloseVertically(UIViewController viewController)
        {
            var topController = (FindHealthProviderViewController)_cnvc.ContentView.TopViewController;

            if (viewController != null)
            {
                var topConstraint = viewController.View.Superview.Constraints
                    .Single(
                        x => x.GetIdentifier() == viewController.GetType() + "animatedConstraint");

                viewController.WillMoveToParentViewController(null);

                topController.View.LayoutIfNeeded();

                UIView.Animate(.3f, 0.0f, UIViewAnimationOptions.CurveEaseOut, () =>
                    {
                        topConstraint.Constant = topController.View.Frame.Height;
                        topController.View.LayoutIfNeeded();
                    },
                    () =>
                    {
                        viewController.View.RemoveFromSuperview();
                        viewController.RemoveFromParentViewController();
                    });
            }
        }
    }
}