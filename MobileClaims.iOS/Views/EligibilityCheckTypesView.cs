using System;
using MobileClaims.Core.ViewModels;
using UIKit;
using CoreGraphics;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
    [Foundation.Register("EligibilityCheckTypesView")]
    public class EligibilityCheckTypesView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
    {
        protected EligibilityCheckTypesViewModel _model;

        protected UIScrollView scrollableContainer;
        protected UITableView submissionTableView;
        protected UILabel checkTypeLabel;

        protected UILabel noAccessLabel;

        public EligibilityCheckTypesView()
        {
        }

        public override void ViewDidLoad()
        {
            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };

            base.ViewDidLoad();

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.SetHidesBackButton(true, false);

            _model = (EligibilityCheckTypesViewModel)ViewModel;

            if (Constants.IS_OS_7_OR_LATER())
                this.AutomaticallyAdjustsScrollViewInsets = false;

            scrollableContainer = ((GSCBaseView)View).baseScrollContainer;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollableContainer);

            submissionTableView = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Plain);
            submissionTableView.RowHeight = Constants.SINGLE_SELECTION_CELL_HEIGHT;
            submissionTableView.TableHeaderView = new UIView();
            submissionTableView.SeparatorColor = Colors.Clear;
            submissionTableView.ShowsVerticalScrollIndicator = true;
            MvxDeleteTableViewSource eligibilitySource = new MvxDeleteTableViewSource(_model, submissionTableView, "EligibilityCheckTypesTableCell", typeof(EligibilityCheckTypesTableCell));
            submissionTableView.Source = eligibilitySource;
            submissionTableView.ScrollEnabled = false;
            scrollableContainer.AddSubview(submissionTableView);

            submissionTableView.ReloadData();

            checkTypeLabel = new UILabel();
            checkTypeLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            checkTypeLabel.TextColor = Colors.DARK_GREY_COLOR;
            checkTypeLabel.Text = "checkEligibilityTypeMessage".tr();
            checkTypeLabel.TextAlignment = UITextAlignment.Left;
            checkTypeLabel.BackgroundColor = Colors.Clear;
            checkTypeLabel.LineBreakMode = UILineBreakMode.WordWrap;
            checkTypeLabel.Lines = 0;
            scrollableContainer.AddSubview(checkTypeLabel);

            noAccessLabel = new UILabel();
            noAccessLabel.Text = "noEligibilityAccess".tr();
            noAccessLabel.BackgroundColor = Colors.Clear;
            noAccessLabel.TextColor = Colors.DARK_GREY_COLOR;
            noAccessLabel.TextAlignment = UITextAlignment.Center;
            noAccessLabel.Lines = 0;
            noAccessLabel.LineBreakMode = UILineBreakMode.WordWrap;
            noAccessLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
            View.AddSubview(noAccessLabel);

            noAccessLabel.Alpha = 0;

            var set = this.CreateBindingSet<EligibilityCheckTypesView, Core.ViewModels.EligibilityCheckTypesViewModel>();
            set.Bind(eligibilitySource).To(vm => vm.EligibilityCheckTypes);
            set.Bind(NavigationItem).For("Title").To(vm => vm.MyBenefitsTitle);
            this.CreateBinding(eligibilitySource).For(s => s.SelectionChangedCommand).To<EligibilityCheckTypesViewModel>(vm => vm.EligibilityCheckTypeSelectedCommand).Apply();
            set.Apply();

            _model.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case "EligibilityCheckTypes":
                        this.View.SetNeedsLayout();
                        break;
                    case "NoAccessToEligibilityChecks":
                        ShowNoAccessToEligibilityMessage();
                        break;
                    default:
                        break;
                }
            };


        }

        private bool messageHasShown;
        private void ShowNoAccessToEligibilityMessage()
        {
            if (_model.NoAccessToEligibilityChecks && !messageHasShown)
            {
                messageHasShown = true;

                checkTypeLabel.Hidden = true;
                submissionTableView.Hidden = true;
                noAccessLabel.Alpha = 1;

            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            View.SetNeedsLayout();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ((GSCBaseView)View).subscribeToBusyIndicator();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ((GSCBaseView)View).unsubscribeFromBusyIndicator();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            float yPos = ViewContentYPositionPadding;           
            float centerX = ViewContainerWidth / 2;

            noAccessLabel.Frame = new CGRect(10, (ViewContainerWidth / 2 - (float)noAccessLabel.Frame.Height / 2), ViewContainerWidth - 20, (float)noAccessLabel.Frame.Height);
            noAccessLabel.SizeToFit();
            noAccessLabel.Frame = new CGRect(10, (ViewContainerHeight / 2 - (float)noAccessLabel.Frame.Height / 2), ViewContainerWidth - 20, (float)noAccessLabel.Frame.Height);

            checkTypeLabel.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, 
                                              yPos,
                                              ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2,
                                               (float)checkTypeLabel.Frame.Height);
            checkTypeLabel.SizeToFit();

            yPos += (float)checkTypeLabel.Frame.Height + Constants.CLAIMS_DETAILS_COMPONENT_PADDING;;

            if (_model.EligibilityCheckTypes != null)
            {
                float listHeight = _model.EligibilityCheckTypes.Count * Constants.SINGLE_SELECTION_CELL_HEIGHT + Constants.SINGLE_SELECTION_VERTICAL_CELL_PADDING;
                submissionTableView.Frame = new CGRect(Constants.SINGLE_SELECTION_TOP_TABLE_PADDING, yPos, ViewContainerWidth - Constants.SINGLE_SELECTION_TOP_TABLE_PADDING * 2, listHeight);

                yPos += listHeight + Constants.CLAIMS_DETAILS_COMPONENT_PADDING;;
            }

            scrollableContainer.Frame = new CGRect(0, 0, ViewContainerWidth, ViewContainerHeight);
            scrollableContainer.ContentSize = new CGSize(ViewContainerWidth, yPos + GetBottomPadding());
        }

        public float GetViewContainerWidth()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            }
            else
            {
                return (float)base.View.Bounds.Width;
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
                return (float)base.View.Bounds.Height - Helpers.BottomNavHeight();
            }
        }

        float IGSCBaseViewImplementor.ViewContentYPositionPadding()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return Constants.CLAIMS_DETAILS_COMPONENT_PADDING;
            }
            else
            {
                return Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;
            }
        }
    }
}