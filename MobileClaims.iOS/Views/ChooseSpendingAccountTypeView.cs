using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS
{
    [Foundation.Register("ChooseSpendingAccountTypeView")]
    public class ChooseSpendingAccountTypeView : GSCBaseViewController
    {
        protected UIScrollView accountTypeScrollView;

        protected UILabel noAccountsLabel, lblAccountType;

        protected NSMutableArray buttonsArray;

        private ChooseSpendingAccountTypeViewModel _model;
        private bool loadFirstTime;

        public ChooseSpendingAccountTypeView()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = "balances".tr();
            base.NavigationController.NavigationItem.SetHidesBackButton(true, false);
            base.NavigationItem.SetHidesBackButton(true, false);
            base.NavigationItem.BackBarButtonItem = new UIBarButtonItem(" ".tr(), UIBarButtonItemStyle.Plain, null, null);

            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            ((GSCBaseView)View).subscribeToBusyIndicator();

            _model = (ChooseSpendingAccountTypeViewModel)this.ViewModel;

            this.AutomaticallyAdjustsScrollViewInsets = false;

            accountTypeScrollView = ((GSCBaseView)View).baseScrollContainer;
            ((GSCBaseView)View).baseContainer.AddSubview(accountTypeScrollView);

            lblAccountType = new UILabel();
            lblAccountType.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.LG_HEADING_FONT_SIZE);
            lblAccountType.TextColor = Colors.DARK_GREY_COLOR;
            lblAccountType.Text = "BalanceCheckSelectAccount".tr();
            lblAccountType.TextAlignment = UITextAlignment.Left;
            lblAccountType.BackgroundColor = Colors.Clear;
            lblAccountType.Hidden = true;
            ((GSCBaseView)View).baseContainer.AddSubview(lblAccountType);

            noAccountsLabel = new UILabel();
            noAccountsLabel.Text = "noAccounts".tr();
            noAccountsLabel.Lines = 2;
            noAccountsLabel.BackgroundColor = Colors.Clear;
            noAccountsLabel.TextColor = Colors.DARK_GREY_COLOR;
            noAccountsLabel.TextAlignment = UITextAlignment.Center;
            noAccountsLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.HEADING_FONT_SIZE);
            ((GSCBaseView)View).baseContainer.AddSubview(noAccountsLabel);

            noAccountsLabel.Hidden = true;

            buttonsArray = new NSMutableArray();

            this.populateTable();

            _model = (ChooseSpendingAccountTypeViewModel)(this.ViewModel);

            var set = this.CreateBindingSet<ChooseSpendingAccountTypeView, ChooseSpendingAccountTypeViewModel>();
            set.Bind(this).For(v => v.AccountTypes).To(vm => vm.AccountTypes);
            set.Bind(this).For(v => v.Busy).To(vm => vm.Busy);
            set.Apply();
            ((GSCBaseView)View).startLoading();
        }

        void populateTable()
        {
            clearTable();

            if (AccountTypes == null || AccountTypes.Count < 1)
            {
                return;
            }

            var set = this.CreateBindingSet<ChooseSpendingAccountTypeView, ChooseSpendingAccountTypeViewModel>();


            for (var i = 0; i < AccountTypes.Count; i++)
            {
                GSSelectionButton accountTypeButton = new GSSelectionButton();
                SpendingAccountType accountType = AccountTypes[i];
                string name = accountType.ModelName?.ToUpper();

                accountTypeButton.SetTitle(name, UIControlState.Normal);
                accountTypeButton.TouchUpInside += itemClicked;
                accountTypeScrollView.AddSubview(accountTypeButton);
                buttonsArray.Add(accountTypeButton);
                set.Bind(accountTypeButton).To(vm => vm.FillAccountDetailAndNavigateCommand).CommandParameter(accountType);
            }

            _model = (ChooseSpendingAccountTypeViewModel)(this.ViewModel);

            set.Apply();
            View.SetNeedsLayout();
        }

        void clearTable()
        {
            buttonsArray.RemoveAllObjects();
            foreach (UIView v in accountTypeScrollView)
            {
                v.RemoveFromSuperview();
            }
        }

        void itemClicked(object sender, EventArgs ea)
        {

            deselectAllButtons();
            ((GSSelectionButton)sender).Selected = true;
        }

        void deselectAllButtons()
        {
            for (var i = 0; i < (uint)buttonsArray.Count; i++)
            {
                GSSelectionButton accountTypeButton = buttonsArray.GetItem<GSSelectionButton>((nuint)i);
                accountTypeButton.Selected = false;
            }

        }

        void showNoAccounts()
        {
            accountTypeScrollView.Hidden = true;
            lblAccountType.Hidden = true;
            noAccountsLabel.Hidden = false;
        }

        void hideNoAccounts()
        {
            accountTypeScrollView.Hidden = false;
            lblAccountType.Hidden = false;
            noAccountsLabel.Hidden = true;
        }

        private bool messageHasShown;
        private void ShowNoAccountsMessage()
        {
            if (!messageHasShown)
            {
                messageHasShown = true;
                InvokeOnMainThread(() =>
              {
                  UIAlertView _error = new UIAlertView("", "noAccessSpendingAccounts".tr(), null, "close".tr(), null);
                  _error.Show();
              });
            }
        }

        public override void ViewDidLayoutSubviews()
        {
            float viewwidth = (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            float viewHeight = (float)((GSCBaseView)View).baseContainer.Bounds.Height - Helpers.BottomNavHeight();
            float contentY = 0.0f;
            float contentWidth = viewwidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2;
            float itemPadding = Constants.CLAIMS_DETAILS_SUB_ITEM_PADDING;
            float startY = Constants.IS_OS_VERSION_OR_LATER(11, 0) ? itemPadding : Constants.IOS_7_TOP_PADDING;

            if (Constants.IsPhone() && Helpers.IsInLandscapeMode())
            {
                startY -= 40;
            }

            lblAccountType.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, startY, contentWidth, (float)lblAccountType.Frame.Height);
            lblAccountType.SizeToFit();

            startY = (float)lblAccountType.Frame.GetMaxY();
            accountTypeScrollView.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, startY, contentWidth, viewHeight - startY);//Math.Min((uint)buttonsArray.Count + 60 * Constants.LIST_BUTTON_HEIGHT
            noAccountsLabel.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, viewHeight / 2 - (float)noAccountsLabel.Frame.Height / 2, contentWidth, 100);
            noAccountsLabel.SizeToFit();
            noAccountsLabel.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, viewHeight / 2 - (float)noAccountsLabel.Frame.Height / 2, contentWidth, (float)noAccountsLabel.Frame.Height);

            contentY += Constants.DRUG_LOOKUP_SIDE_PADDING;
            for (var i = 0; i < (uint)buttonsArray.Count; i++)
            {
                GSSelectionButton accountTypeButton = buttonsArray.GetItem<GSSelectionButton>((nuint)i);
                accountTypeButton.Frame = new CGRect(0, contentY, contentWidth, Constants.LIST_BUTTON_HEIGHT);
                contentY += Constants.LIST_BUTTON_HEIGHT;
                contentY += Constants.DRUG_LOOKUP_SIDE_PADDING;
            }

            accountTypeScrollView.ContentSize = new CGSize(contentWidth, contentY);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            base.NavigationController.NavigationBarHidden = false;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ((GSCBaseView)View).unsubscribeFromBusyIndicator();
            deselectAllButtons();
        }

        private List<SpendingAccountType> _accountTypes;
        public List<SpendingAccountType> AccountTypes
        {
            get
            {
                return _accountTypes;
            }
            set
            {
                if (value == null) return;
                _accountTypes = value;

                if (_accountTypes.Count > 0)
                {
                    populateTable();
                    hideNoAccounts();
                }
                else
                {
                    showNoAccounts();
                }
            }
        }

        private bool _busy;
        public bool Busy
        {
            get
            {
                return _busy;
            }
            set
            {
                _busy = value;
                if (value)
                {
                    ((GSCBaseView)View).startLoading();
                }
                else
                {
                    ((GSCBaseView)View).stopLoading();
                }

                if (!value && (_accountTypes == null || _accountTypes.Count < 1))
                {
                    showNoAccounts();
                }
            }
        }
    }
}

