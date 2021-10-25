using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views
{
    [Register("UniversalView")]
    public class UniversalView : UIView
    {
        public UniversalView()
        {
            Initialize();
        }

        public UniversalView(CGRect bounds)
            : base(bounds)
        {
            Initialize();
        }

        void Initialize()
        {
            BackgroundColor = Colors.BACKGROUND_COLOR;
        }
    }

    [Register("DrugLookupByNameView")]
    public class DrugLookupByNameView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
    {
        private DrugLookupByNameViewModel _model;
        protected DefaultTextField drugNameField;
        protected UIBarButtonItem searchButton;
        protected UILabel planParticpantLabel;
        protected UIScrollView lookupScrollView;
        protected UIView participantContainer;
        protected NSMutableArray buttonsArray;
        protected bool controllerOpening;

        protected UILabel titleLabel;

        public DrugLookupByNameView()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };

            _model = (DrugLookupByNameViewModel)ViewModel;

            base.NavigationItem.Title = "drugNameFieldText".tr();
            searchButton = new UIBarButtonItem();
            searchButton.Style = UIBarButtonItemStyle.Plain;
            searchButton.Clicked += SearchClicked;
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
            titleLabel.Text = "enterName".tr();
            titleLabel.BackgroundColor = Colors.Clear;
            titleLabel.TextColor = Colors.VERY_DARK_GREY_COLOR;
            titleLabel.TextAlignment = UITextAlignment.Left;
            titleLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.SUB_HEADING_FONT_SIZE);
            lookupScrollView.AddSubview(titleLabel);

            drugNameField = new DefaultTextField();
            drugNameField.Placeholder = "drugNamePlaceholder".tr();
            lookupScrollView.AddSubview(drugNameField);

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

            var set = this.CreateBindingSet<DrugLookupByNameView, DrugLookupByNameViewModel>();
            set.Bind(this).For(v => v.SearchResults).To(vm => vm.SearchResults);
            set.Bind(this).For(v => v.planMember).To(vm => vm.PlanMember);
            set.Bind(drugNameField).To(vm => vm.DrugName);
            set.Apply();

            _model.OnNoResults += HandleOnNoResults;
            _model.OnInvalidDrugName += HandleOnInvalidDrugName;
            _model.OnMissingDrugName += HandleOnMissingDrugName;
        }

        void HandleOnMissingDrugName(object sender, EventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                UIAlertView _error = new UIAlertView("", "nameError".tr(), null, "ok".tr(), null);
                _error.Show();
            });
        }

        void HandleOnInvalidDrugName(object sender, EventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                UIAlertView _error = new UIAlertView("", "drugLengthError".tr(), null, "ok".tr(), null);
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

        void SearchClicked(object sender, EventArgs ea)
        {
            _model = (DrugLookupByNameViewModel)(this.ViewModel);
            _model.SearchAndNavigateCommand.Execute(drugNameField.Text);
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

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            dismissKeyboard();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            ((GSCBaseView)View).unsubscribeFromBusyIndicator();

        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            float itemPadding = Constants.CLAIMS_DETAILS_COMPONENT_PADDING;
            float extraPos = Constants.IOS_7_TOP_PADDING;

            float contentY = 0.0f;
            float buttonY = 0.0f;
            float contentWidth = ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2;
            float listHeight = (uint)buttonsArray.Count * Constants.PARTICIPANT_LIST_BUTTON_HEIGHT;
            float drugNameFieldHeight = Constants.DRUG_BUTTON_HEIGHT;

            lookupScrollView.Frame = new CGRect(0, 0, ViewContainerWidth, ViewContainerHeight);

            titleLabel.Frame = new CGRect(Constants.BUTTON_SIDE_PADDING, ViewContentYPositionPadding + Constants.DRUG_LOOKUP_LABEL_HEIGHT, ViewContainerWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);

            drugNameField.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, ViewContentYPositionPadding + Constants.DRUG_LOOKUP_LABEL_HEIGHT * 2 + Constants.DRUG_LOOKUP_BUTTON_PADDING, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, drugNameFieldHeight);

            planParticpantLabel.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, (float)drugNameField.Frame.Y + drugNameFieldHeight + Constants.DRUG_LOOKUP_TOP_PADDING, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, Constants.DRUG_LOOKUP_LABEL_HEIGHT);

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
                return 0;
            }
            else
            {
                return Constants.IOS_7_TOP_PADDING;
            }

        }

        #region Properties

        private List<DrugInfo> _searchResults;
        public List<DrugInfo> SearchResults
        {
            get
            {
                return _searchResults;
            }
            set
            {
                _searchResults = value;
                if (_searchResults.Count > 0 && !controllerOpening)
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
        #endregion
    }
}