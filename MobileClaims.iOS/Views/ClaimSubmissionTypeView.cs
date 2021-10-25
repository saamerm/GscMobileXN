using System;
using CoreGraphics;
using Foundation;
using MobileClaims.Core;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugin.Messenger;
using UIKit;
using WebKit;

namespace MobileClaims.iOS
{
    [Register("ClaimSubmissionTypeView")]
    public class ClaimSubmissionTypeView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
    {
        protected UIScrollView scrollContainer;
        protected UITableView submissionTableView;
        protected UIScrollView providerScrollView;
        protected UILabel submissionTypeLabel;
        protected UILabel webBenefitsDisclaimerLabel;
        protected UILabel noClaimsLabel;

        protected ClaimSubmissionTypeViewModel _model;

        private IMvxMessenger _messenger;
        private MvxSubscriptionToken _iscurrentlybusy;

        private MvxSubscriptionToken _notEligibleForVisionClaims;
        private string language;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _model = (ClaimSubmissionTypeViewModel)ViewModel;
            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();

            ILanguageService langService = Mvx.IoCProvider.Resolve<ILanguageService>();
            language = langService.CurrentLanguage;

            View = new GSCBaseView()
            {
                BackgroundColor = Colors.BACKGROUND_COLOR
            };
            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.SetHidesBackButton(false, false);
            base.NavigationItem.Title = "claimSubmission".tr().ToUpper();

            base.NavigationController.NavigationBar.TintColor = Colors.HIGHLIGHT_COLOR;
            base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
            base.NavigationController.View.BackgroundColor = Colors.BACKGROUND_COLOR;

            this.AutomaticallyAdjustsScrollViewInsets = false;

            scrollContainer = new UIScrollView();
            scrollContainer.BackgroundColor = Colors.Clear;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollContainer);

            submissionTableView = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Plain);
            submissionTableView.RowHeight = Constants.SINGLE_SELECTION_CELL_HEIGHT;
            submissionTableView.TableHeaderView = new UIView();
            submissionTableView.SeparatorColor = Colors.Clear;
            submissionTableView.ShowsVerticalScrollIndicator = false;
            submissionTableView.ScrollEnabled = false;
            MvxDeleteTableViewSource providerSource = new MvxDeleteTableViewSource(_model, submissionTableView, "ClaimSubmissionTableCell", typeof(ClaimSubmissionTableCell));
            submissionTableView.Source = providerSource;
            scrollContainer.AddSubview(submissionTableView);

            noClaimsLabel = new UILabel();
            noClaimsLabel.Text = "noClaimAccess".tr();
            noClaimsLabel.BackgroundColor = Colors.Clear;
            noClaimsLabel.TextColor = Colors.DARK_GREY_COLOR;
            noClaimsLabel.TextAlignment = UITextAlignment.Center;
            noClaimsLabel.Lines = 0;
            noClaimsLabel.LineBreakMode = UILineBreakMode.WordWrap;
            noClaimsLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.HEADING_FONT_SIZE);
            View.AddSubview(noClaimsLabel);

            noClaimsLabel.Alpha = 0;

            var set = this.CreateBindingSet<ClaimSubmissionTypeView, ClaimSubmissionTypeViewModel>();
            set.Bind(providerSource).To(vm => vm.ClaimSubmissionTypes);
            set.Bind(providerSource).For(item => item.SelectedItem).To(vm => vm.SelectedClaimSubmissionType);//.Mode(Cirrious.MvvmCross.Binding.MvxBindingMode.OneWay);
            this.CreateBinding(providerSource).For(s => s.SelectionChangedCommand).To<ClaimSubmissionTypeViewModel>(vm => vm.ClaimSubmissionTypeSelectedCommand).Apply();
            set.Apply();

            submissionTableView.ReloadData();

            submissionTypeLabel = new UILabel();
            submissionTypeLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.LG_HEADING_FONT_SIZE);
            submissionTypeLabel.TextColor = Colors.DARK_GREY_COLOR;
            if (Constants.IsPhone())
            {
                submissionTypeLabel.Text = "selectTypeOfTreatmentMessageTitle".tr();
            }
            else
            {
                submissionTypeLabel.Text = "selectTypeOfTreatmentMessageTitle".tr();
            }
            submissionTypeLabel.TextAlignment = UITextAlignment.Left;
            submissionTypeLabel.BackgroundColor = Colors.Clear;
            submissionTypeLabel.LineBreakMode = UILineBreakMode.WordWrap;
            submissionTypeLabel.Lines = 0;
            scrollContainer.AddSubview(submissionTypeLabel);

            webBenefitsDisclaimerLabel = new UILabel(View.Bounds);
            webBenefitsDisclaimerLabel.BackgroundColor = Colors.Clear;

            webBenefitsDisclaimerLabel.Text = Resource.BenefitsDisclaimer1 + " " + Resource.BenefitsDisclaimer2;
            scrollContainer.AddSubview(webBenefitsDisclaimerLabel);

            _model.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case "ClaimSubmissionTypes":
                        SetFrames();
                        break;
                    case "NoAccessToOnlineClaimSubmission":
                        ShowNoAccessToClaimsMessage();

                        break;
                    default:
                        break;
                }
            };

            ShowNoAccessToClaimsMessage();
        }

        public override void ViewDidLayoutSubviews()
        {
            try
            {
                SetFrames();
            }
            catch (Exception ex)
            {
                MvxTrace.Trace(MvxTraceLevel.Error, ex.ToString());
            }
            base.ViewDidLayoutSubviews();
            SetFrames();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();

            _notEligibleForVisionClaims = _messenger.Subscribe<ClaimParticipantChangeRequested>((message) =>
            {
                showVisionClaimError();
            });
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ShowNoAccessToClaimsMessage();

            if (_model.SelectedClaimSubmissionType != null)
            {
                try
                {
                    int participantIndex = _model.ClaimSubmissionTypes.IndexOf(_model.SelectedClaimSubmissionType);
                    NSIndexPath path = NSIndexPath.FromRowSection((nint)participantIndex, (nint)0);
                    submissionTableView.SelectRow(path, false, UITableViewScrollPosition.None);
                }
                catch (Exception ex)
                {
                    MvxTrace.Trace(MvxTraceLevel.Error, ex.ToString());
                }
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            _messenger.Unsubscribe<ClaimParticipantChangeRequested>(_notEligibleForVisionClaims);
            //_messenger.Unsubscribe<BusyIndicator>(_iscurrentlybusy);
        }

        private bool messageHasShown;
        private void ShowNoAccessToClaimsMessage()
        {
            if (_model.NoAccessToOnlineClaimSubmission && !messageHasShown)
            {
                messageHasShown = true;

                submissionTypeLabel.Hidden = true;
                submissionTableView.Hidden = true;
                webBenefitsDisclaimerLabel.Hidden = true;

                noClaimsLabel.Alpha = 1;

                //              InvokeOnMainThread ( () => {
                //                  UIAlertView _error = new UIAlertView ("", "noClaimAccess".tr(), null, "close".tr(), null);
                //                  _error.Show ();
                //              });
            }
        }

        protected void showVisionClaimError()
        {
            InvokeOnMainThread(() =>
            {
                UIAlertView _error = new UIAlertView("", "errorMultipleMatch".tr(), null, "ok".tr(), null);
                _error.Show();
            });
        }

        private void SetFrames()
        {
            base.ViewDidLayoutSubviews();

            webBenefitsDisclaimerLabel.Text = Resource.BenefitsDisclaimer1 + " " + Resource.BenefitsDisclaimer2;
            webBenefitsDisclaimerLabel.TextColor = Colors.DARK_GREY_COLOR;
            webBenefitsDisclaimerLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.TERMS_TEXT_FONT_SIZE);
            var myAttributedString = webBenefitsDisclaimerLabel.AttributedText;
            var myMutableString = new NSMutableAttributedString(myAttributedString);
            UIStringAttributes atSelection = new UIStringAttributes { ForegroundColor = Colors.DARK_RED, Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.TERMS_TEXT_FONT_SIZE) };           
            myMutableString.AddAttributes(atSelection.Dictionary, new NSRange(0, Resource.BenefitsDisclaimer1.Length));
            webBenefitsDisclaimerLabel.AttributedText = myMutableString;
            webBenefitsDisclaimerLabel.Lines = 0;
            if (View == null || View.Superview == null)
            {
                return;
            }
            ClaimSubmissionTypeViewModel _model = (ClaimSubmissionTypeViewModel)ViewModel;

            float yPos = ViewContentYPositionPadding;

            submissionTypeLabel.Frame = new CGRect(Constants.CONTENT_SIDE_PADDING, yPos, ViewContainerWidth - Constants.CONTENT_SIDE_PADDING * 2, (float)submissionTypeLabel.Frame.Height);
            submissionTypeLabel.SizeToFit();

            noClaimsLabel.Frame = new CGRect(10, ViewContainerHeight / 2 - (float)noClaimsLabel.Frame.Height / 2, ViewContainerWidth - 20, (float)noClaimsLabel.Frame.Height);
            noClaimsLabel.SizeToFit();
            noClaimsLabel.Frame = new CGRect(10, ViewContainerHeight / 2 - (float)noClaimsLabel.Frame.Height / 2, ViewContainerWidth - 20, (float)noClaimsLabel.Frame.Height);

            yPos += (float)submissionTypeLabel.Frame.Height + Constants.CLAIMS_TOP_PADDING;

            float benefitsDesclaimerLabelHeight;
            if (Helpers.IsInPortraitMode())
            {
                if (Constants.IsPhone())
                {
                    benefitsDesclaimerLabelHeight = 90;
                    if (language.Contains("fr") || language.Contains("Fr"))
                    {
                        benefitsDesclaimerLabelHeight = 120;
                    }
                }
                else
                {
                    if (language.Contains("fr") || language.Contains("Fr"))
                    {
                        benefitsDesclaimerLabelHeight = 170;
                    }
                    else
                    {
                        benefitsDesclaimerLabelHeight = 120;
                    }
                }
            }
            else
            {
                if (Constants.IsPhone())
                {
                    benefitsDesclaimerLabelHeight = 75;
                    if (language.Contains("fr") || language.Contains("Fr"))
                    {
                        benefitsDesclaimerLabelHeight = 85;
                    }
                }
                else
                {
                    if (language.Contains("fr") || language.Contains("Fr"))
                    {
                        benefitsDesclaimerLabelHeight = 170;
                    }
                    else
                    {
                        benefitsDesclaimerLabelHeight = 120;
                    }
                }
            }

            webBenefitsDisclaimerLabel.Frame = new CGRect(Constants.CONTENT_SIDE_PADDING, yPos, ViewContainerWidth - Constants.CONTENT_SIDE_PADDING * 2, benefitsDesclaimerLabelHeight);

            yPos += (float)webBenefitsDisclaimerLabel.Frame.Height + Constants.CLAIMS_TOP_PADDING;

            if (_model.ClaimSubmissionTypes != null)
            {
                float listHeight = _model.ClaimSubmissionTypes.Count * (Constants.SINGLE_SELECTION_CELL_HEIGHT) + Constants.SINGLE_SELECTION_VERTICAL_CELL_PADDING;
                submissionTableView.Frame = new CGRect(Constants.SINGLE_SELECTION_TOP_TABLE_PADDING, yPos, ViewContainerWidth - Constants.SINGLE_SELECTION_TOP_TABLE_PADDING * 2, listHeight);
                yPos += listHeight;
            }

            scrollContainer.Frame = new CGRect(0, 0, ViewContainerWidth, ViewContainerHeight);
            scrollContainer.ContentSize = new CGSize(ViewContainerWidth, yPos + Constants.CLAIMS_TOP_PADDING + GetBottomPadding());
        }

        public float GetViewContainerWidth()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            }
            else
            {
                return (float)View.Superview.Frame.Width;
            }
        }

        public float GetViewContainerHeight()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return (float)((GSCBaseView)View).baseContainer.Bounds.Height - Helpers.BottomNavHeight();
            }
            else
            {
                return (float)View.Superview.Frame.Height - Helpers.BottomNavHeight();
            }
        }

        float IGSCBaseViewImplementor.ViewContentYPositionPadding()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return 10;
            }
            else
            {
                return (Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING);
            }

        }

        private bool _busy = true;
        public bool Busy
        {
            get
            {
                return _busy;
            }
            set
            {
                _busy = value;
                if (!_busy)
                {
                    InvokeOnMainThread(() =>
                    {
                        ((GSCBaseView)View).stopLoading();
                    });
                }
                else
                {
                    InvokeOnMainThread(() =>
                    {
                        ((GSCBaseView)View).startLoading();
                    });
                }

            }
        }
    }

    [Foundation.Register("ClaimSubmissionTableCell")]
    public class ClaimSubmissionTableCell : SingleSelectionTableViewCell
    {
        public ClaimSubmissionTableCell() : base() { }
        public ClaimSubmissionTableCell(IntPtr handle) : base(handle) { }

        public override void InitializeBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ClaimSubmissionTableCell, ClaimSubmissionType>();
                set.Bind(this.label).To(item => item.Name).WithConversion("StringCase").OneWay();
                set.Apply();
            });
        }
    }
}

