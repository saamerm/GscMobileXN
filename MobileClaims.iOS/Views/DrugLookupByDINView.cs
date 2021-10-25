using System;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS
{
    [Register("DrugLookupByDINView")]
    public class DrugLookupByDINView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
    {
        private DrugLookupByDINViewModel _model;
        protected DefaultTextField drugDINField;
        protected UIBarButtonItem searchButton;
        protected UILabel planParticpantLabel;
        protected UIScrollView lookupScrollView;
        protected UIView participantContainer;
        protected NSMutableArray buttonsArray;
        protected bool controllerOpening;

        protected UILabel titleLabel;

        public DrugLookupByDINView()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };

            _model = (DrugLookupByDINViewModel)this.ViewModel;

            base.NavigationItem.Title = "drugDINFieldText".tr();
            searchButton = new UIBarButtonItem();
            searchButton.Style = UIBarButtonItemStyle.Plain;
            searchButton.Title = "search".tr();
            searchButton.TintColor = Colors.HIGHLIGHT_COLOR;
            UITextAttributes attributes = new UITextAttributes();
            attributes.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.NAV_BAR_BUTTON_SIZE);
            searchButton.SetTitleTextAttributes(attributes, UIControlState.Normal);
            base.NavigationItem.RightBarButtonItem = searchButton;

            this.AutomaticallyAdjustsScrollViewInsets = false;

            lookupScrollView = ((GSCBaseView)View).baseScrollContainer;
            ((GSCBaseView)View).baseContainer.AddSubview(lookupScrollView);

            ((GSCBaseView)View).ViewTapped += HandleViewTapped;

            titleLabel = new UILabel();
            titleLabel.Text = "enterDIN".tr();
            titleLabel.BackgroundColor = Colors.Clear;
            titleLabel.TextColor = Colors.DARK_GREY_COLOR;
            titleLabel.TextAlignment = UITextAlignment.Left;
            titleLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.SUB_HEADING_FONT_SIZE);
            lookupScrollView.AddSubview(titleLabel);

            drugDINField = new DefaultTextField();
            drugDINField.Placeholder = "drugDINPlaceholder".tr();
            lookupScrollView.AddSubview(drugDINField);

            planParticpantLabel = new UILabel();
            planParticpantLabel.Text = "choosePlanParticipant".tr();
            planParticpantLabel.TextColor = Colors.DARK_GREY_COLOR;
            planParticpantLabel.BackgroundColor = Colors.Clear;
            planParticpantLabel.TextAlignment = UITextAlignment.Left;
            planParticpantLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.SUB_HEADING_FONT_SIZE);
            lookupScrollView.AddSubview(planParticpantLabel);

            participantContainer = new UIScrollView();
            participantContainer.BackgroundColor = Colors.Clear;
            lookupScrollView.AddSubview(participantContainer);

            buttonsArray = new NSMutableArray();

            var set = this.CreateBindingSet<DrugLookupByDINView, DrugLookupByDINViewModel>();
            set.Bind(this).For(v => v.CheckResult).To(vm => vm.CheckResult);
            set.Bind(this).For(v => v.planMember).To(vm => vm.PlanMember);
            set.Bind(drugDINField).To(vm => vm.DIN);
            set.Bind(searchButton).To(vm => vm.SearchAndNavigateCommand).CommandParameter(drugDINField.Text);
            set.Apply();

            _model.OnNoResults += HandleOnNoResults;
            _model.OnInvalidDIN += HandleOnInvalidDIN;
            _model.OnMissingDIN += HandleOnMissingDDIN;

        }

        void HandleOnMissingDDIN(object sender, EventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                UIAlertView _error = new UIAlertView("", "dinError".tr(), null, "ok".tr(), null);
                _error.Show();
            });
        }

        void HandleOnInvalidDIN(object sender, EventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                UIAlertView _error = new UIAlertView("", "dinFormatError".tr(), null, "ok".tr(), null);
                _error.Show();
            });
        }

        void HandleOnNoResults(object sender, EventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                UIAlertView _error = new UIAlertView("noresults".tr(), "noresultsdetails".tr(), null, "ok".tr(), null);
                _error.Show();
            });
        }

        void populateTable()
        {
            clearTable();

            if (_model.Participants == null)
                return;

            for (var i = 0; i < _model.Participants.Count; i++)
            {
                SelectionMenuItem participantMenuItem = new SelectionMenuItem(i == _model.Participants.Count - 1 ? false : true);
                Participant participant = ((Participant)_model.Participants[i]);
                participantMenuItem.SetTitle(participant.FullName, UIControlState.Normal);
                participantContainer.AddSubview(participantMenuItem);
                buttonsArray.Add(participantMenuItem);

                if (i == 0)
                {
                    participantMenuItem.ShowCheck();
                    _model.SelectedParticipant = participant;
                }
                participantMenuItem.TouchUpInside += (sender, ea) =>
                {
                    for (var mi = 0; mi < (uint)buttonsArray.Count; mi++)
                    {
                        buttonsArray.GetItem<SelectionMenuItem>((nuint)mi).HideCheck();
                    }

                    _model.SelectedParticipant = participant;
                    participantMenuItem.ShowCheck();
                };
            }
        }

        void clearTable()
        {
            foreach (UIView v in participantContainer)
            {
                v.RemoveFromSuperview();
            }

            buttonsArray.RemoveAllObjects();
        }

        void HandleViewTapped(object sender, EventArgs e)
        {
            dismissKeyboard();
        }

        void dismissKeyboard()
        {
            this.View.EndEditing(true);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            controllerOpening = false;
            base.NavigationController.NavigationBarHidden = false;

            ((GSCBaseView)View).subscribeToBusyIndicator();

            View.SetNeedsLayout();

        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            ((GSCBaseView)View).unsubscribeFromBusyIndicator();
        }

        void SearchClicked(object sender, EventArgs ea)
        {
            _model = (DrugLookupByDINViewModel)(this.ViewModel);
            _model.GetDrugInfoCommand.Execute(drugDINField.Text);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            float contentY = 0.0f;
            float buttonY = 0.0f;
            float contentWidth = ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2;
            float listHeight = (uint)buttonsArray.Count * Constants.PARTICIPANT_LIST_BUTTON_HEIGHT;
            float itemPadding = Constants.CLAIMS_DETAILS_COMPONENT_PADDING;
            float startY = ViewContentYPositionPadding;
            float extraPos = Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;
            float drugNameFieldHeight = Constants.DRUG_BUTTON_HEIGHT;
            lookupScrollView.Frame = new CGRect(0, 0, ViewContainerWidth, ViewContainerHeight);

            titleLabel.Frame = new CGRect(Constants.BUTTON_SIDE_PADDING, startY + Constants.DRUG_LOOKUP_LABEL_HEIGHT, ViewContainerWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);

            drugDINField.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, startY + Constants.DRUG_LOOKUP_LABEL_HEIGHT * 2 + Constants.DRUG_LOOKUP_BUTTON_PADDING, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, drugNameFieldHeight);

            planParticpantLabel.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, (float)drugDINField.Frame.Y + drugNameFieldHeight + Constants.DRUG_LOOKUP_TOP_PADDING, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, Constants.DRUG_LOOKUP_LABEL_HEIGHT);

            for (var i = 0; i < (uint)buttonsArray.Count; i++)
            {
                SelectionMenuItem participantMenuItem = buttonsArray.GetItem<SelectionMenuItem>((nuint)i);
                participantMenuItem.Frame = new CGRect(0, buttonY, contentWidth, Constants.PARTICIPANT_LIST_BUTTON_HEIGHT);
                buttonY += Constants.PARTICIPANT_LIST_BUTTON_HEIGHT;
            }

            participantContainer.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, (float)planParticpantLabel.Frame.Y + (float)planParticpantLabel.Frame.Height, contentWidth, buttonY);

            contentY = (float)participantContainer.Frame.Y + (float)participantContainer.Frame.Height + itemPadding;

            lookupScrollView.ContentSize = new CGSize(contentWidth, contentY + GetBottomPadding());
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
                return 10;
            }
            else
            {
                return Constants.IOS_7_TOP_PADDING;
            }
        }

        private bool _errorInSearch = true;
        public bool errorInSearch
        {
            get
            {
                return _errorInSearch;
            }
            set
            {
                if (_errorInSearch != value)
                {
                    _errorInSearch = value;
                    if (_errorInSearch)
                    {
                        UIAlertView alert = new UIAlertView("noresults".tr(), "noresultsdetails".tr(), new UIAlertViewDelegate(), "OK", null);
                        alert.Show();
                    }
                }
            }
        }

        private DrugInfo _checkResult;
        public DrugInfo CheckResult
        {
            get
            {
                return _checkResult;
            }
            set
            {
                _checkResult = value;
                if (_checkResult != null && !controllerOpening)
                {
                    controllerOpening = true;
                }

            }
        }

        private PlanMember _planMember = new PlanMember();
        public PlanMember planMember
        {
            get
            {
                return _planMember;
            }
            set
            {

                if (_planMember != value)
                {
                    _planMember = value;
                    populateTable();
                }
            }
        }
    }
}