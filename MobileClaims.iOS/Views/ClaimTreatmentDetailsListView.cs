using System;
using MobileClaims.Core.ViewModels;
using UIKit;
using CoreGraphics;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
    [Foundation.Register("ClaimTreatmentDetailsListView")]
    public class ClaimTreatmentDetailsListView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
    {
        protected ClaimTreatmentDetailsListViewModel _model;

        UIBarButtonItem addEntryButton;

        protected UILabel planParticpantLabel;
        protected UILabel planParticpantSubLabel;

        UITableView treatmentTableView;
        MvxDeleteTableViewSource treatmentSource;

        protected UIScrollView scrollContainer;

        protected GSButton submitButton;

        private float BUTTON_WIDTH = Constants.IsPhone() ? 270 : 400;
        private float BUTTON_HEIGHT = Constants.IsPhone() ? 40 : 50;

        public ClaimTreatmentDetailsListView()
        {

        }

        public override void ViewDidLoad()
        {
            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            base.ViewDidLoad();
            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = "claimDetailsTitle".tr();

            if (Constants.IS_OS_7_OR_LATER())
            {
                this.AutomaticallyAdjustsScrollViewInsets = false;
                base.NavigationController.NavigationBar.TintColor = Colors.HIGHLIGHT_COLOR;
                base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
                base.NavigationController.View.BackgroundColor = Colors.BACKGROUND_COLOR;
            }
            else
            {
                base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
            }

            _model = (ClaimTreatmentDetailsListViewModel)this.ViewModel;

            scrollContainer = new UIScrollView();
            scrollContainer.BackgroundColor = Colors.Clear;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollContainer);

            addEntryButton = new UIBarButtonItem();
            addEntryButton.Style = UIBarButtonItemStyle.Plain;
            addEntryButton.Clicked += HandleAddEntry;
            addEntryButton.Title = "addEntry".tr();
            addEntryButton.TintColor = Colors.HIGHLIGHT_COLOR;
            UITextAttributes attributes = new UITextAttributes();
            attributes.Font = UIFont.FromName(Constants.NUNITO_REGULAR, (nfloat)Constants.NAV_BAR_BUTTON_SIZE);
            addEntryButton.SetTitleTextAttributes(attributes, UIControlState.Normal);
            base.NavigationItem.RightBarButtonItem = addEntryButton;

            if (Constants.IS_OS_7_OR_LATER())
                this.AutomaticallyAdjustsScrollViewInsets = false;

            //			if (_model.TreatmentDetails == null) {
            //				TreatmentDetail treatmentdetail = new TreatmentDetail ();
            //				treatmentdetail.TreatmentDuration = "durationTest";
            //				treatmentdetail.TypeOfTreatment = "treatmentType";
            //
            //				List<TreatmentDetail> treatmentDetails = new List<TreatmentDetail> ();
            //				treatmentDetails.Add (treatmentdetail);
            //
            //				_model.TreatmentDetails = treatmentDetails;
            //			}

            planParticpantLabel = new UILabel();
            planParticpantLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            planParticpantLabel.TextColor = Colors.DARK_GREY_COLOR;
            planParticpantLabel.BackgroundColor = Colors.Clear;
            planParticpantLabel.TextAlignment = UITextAlignment.Left;
            planParticpantLabel.LineBreakMode = UILineBreakMode.WordWrap;
            planParticpantLabel.Lines = 0;
            scrollContainer.AddSubview(planParticpantLabel);

            planParticpantSubLabel = new UILabel();
            planParticpantSubLabel.Text = "swipeNotification".tr();
            planParticpantSubLabel.BackgroundColor = Colors.Clear;
            planParticpantSubLabel.LineBreakMode = UILineBreakMode.WordWrap;
            planParticpantSubLabel.Lines = 0;
            planParticpantSubLabel.TextColor = Colors.DARK_GREY_COLOR;
            planParticpantSubLabel.TextAlignment = UITextAlignment.Left;
            planParticpantSubLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, (nfloat)Constants.SUB_HEADING_FONT_SIZE);
            scrollContainer.AddSubview(planParticpantSubLabel);

            treatmentTableView = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Plain);
            treatmentTableView.RowHeight = Constants.SINGLE_SELECTION_ACCENTED_TITLE_CELL_HEIGHT;
            treatmentTableView.TableHeaderView = new UIView();
            treatmentTableView.SeparatorColor = Colors.Clear;
            treatmentTableView.ScrollEnabled = false;
            scrollContainer.AddSubview(treatmentTableView);

            treatmentSource = new MvxDeleteTableViewSource(_model, treatmentTableView, "ClaimTreatmentDetailListTableViewCell", typeof(ClaimTreatmentDetailListTableViewCell));
            treatmentTableView.Source = treatmentSource;

            submitButton = new GSButton();
            submitButton.SetTitle("continueButton".tr(), UIControlState.Normal);
            scrollContainer.AddSubview(submitButton);

            var set = this.CreateBindingSet<ClaimTreatmentDetailsListView, ClaimTreatmentDetailsListViewModel>();
            set.Bind(planParticpantLabel).To(vm => vm.Participant.FullName).WithConversion("ClaimsForPrefix");
            set.Bind(treatmentSource).To(vm => vm.TreatmentDetails);
            set.Bind(this).For(v => v.FiveTreatmentDetails).To(vm => vm.FiveTreatmentDetails);
            this.CreateBinding(treatmentSource).For(lts => lts.SelectionChangedCommand).To<ClaimTreatmentDetailsListViewModel>(vm => vm.SelectTreatmentDetailCommand).Apply();
            set.Bind(this.submitButton).To(vm => vm.SubmitClaimCommand);
            set.Apply();

            treatmentTableView.ReloadData();

            _model.OnInvalidClaim += (object sender, EventArgs e) =>
            {
                InvokeOnMainThread(() =>
                {
                    if (_model.MissingTreatmentDetails)
                    {
                        UIAlertView _error = new UIAlertView("", "noClaimDetailEntered".tr(), null, "ok".tr(), null);

                        _error.Show();
                    }
                });

            };
        }

        void HandleAddEntry(object sender, EventArgs e)
        {
            //if (_model.FiveTreatmentDetails)
            //{
            //    InvokeOnMainThread(() =>
            //    {
            //        UIAlertView _error = new UIAlertView("", "maximumFiveClaims".tr(), null, "ok".tr(), null);
            //        _error.Show();
            //    });

            //}
            //else
            //{
            _model.AddTreatmentDetailCommand.Execute(null);
            //}
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            View.SetNeedsLayout();

            if (treatmentTableView != null)
                treatmentTableView.ReloadData();
        }

        bool firstAppeared = false;
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (firstAppeared)
                return;

            firstAppeared = true;

            //			if (_model.TreatmentDetails.Count < 1)
            //				_model.AddTreatmentDetailCommand.Execute (null);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            float startY = Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;
            //float ViewContainerWidth = (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            //float ViewContainerHeight = (float)((GSCBaseView)View).baseContainer.Bounds.Height - Helpers.BottomNavHeight();
            float itemPadding = Constants.CLAIMS_DETAILS_COMPONENT_PADDING;

            scrollContainer.Frame = new CGRect(0, 0, ViewContainerWidth, ViewContainerHeight);

            float yPos = ViewContentYPositionPadding;
            float listHeight = _model.TreatmentDetails.Count * (Constants.SINGLE_SELECTION_ACCENTED_TITLE_CELL_HEIGHT) + Constants.SINGLE_SELECTION_VERTICAL_CELL_PADDING;

            planParticpantLabel.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, yPos, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, (float)planParticpantLabel.Frame.Height);
            planParticpantLabel.SizeToFit();
            yPos += (float)planParticpantLabel.Frame.Height + itemPadding;

            planParticpantSubLabel.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, yPos, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, (float)planParticpantSubLabel.Frame.Height);
            planParticpantSubLabel.SizeToFit();
            yPos += (float)planParticpantSubLabel.Frame.Height + itemPadding;

            treatmentTableView.Frame = new CGRect(Constants.SINGLE_SELECTION_TOP_TABLE_PADDING, yPos, ViewContainerWidth - Constants.SINGLE_SELECTION_TOP_TABLE_PADDING * 2, listHeight);
            submitButton.Frame = new CGRect(ViewContainerWidth / 2 - BUTTON_WIDTH / 2, (float)treatmentTableView.Frame.Y + (float)treatmentTableView.Frame.Height + itemPadding, BUTTON_WIDTH, BUTTON_HEIGHT);

            scrollContainer.ContentSize = new CGSize(ViewContainerWidth, submitButton.Frame.Y + submitButton.Frame.Height + GetBottomPadding());

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

        private bool _fiveTreatmentDetails = false;
        public bool FiveTreatmentDetails
        {
            get
            {
                return _fiveTreatmentDetails;
            }
            set
            {
                _fiveTreatmentDetails = value;

                if (value)
                {

                }
                else
                {

                }

            }
        }
    }
}

