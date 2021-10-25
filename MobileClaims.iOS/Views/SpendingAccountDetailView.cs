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
    [Register("SpendingAccountDetailView")]
    public class SpendingAccountDetailView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
    {

        protected UIScrollView accountsScrollView;
        private GSCMessageLabel LabNoData;

        UILabel totalRemainingTitle, totalRemainingAmount;
        UIView vewTotalRemaining;
        NSMutableArray accountSections;

        private SpendingAccountDetailViewModel _model;

        int redrawCount = 0;
        private float TOP_PADDING = Constants.IsPhone() ? 15.0f : 30.0f;
       
        public SpendingAccountDetailView()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            base.NavigationController.NavigationBarHidden = false;

            this.AutomaticallyAdjustsScrollViewInsets = false;

            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };

            _model = (SpendingAccountDetailViewModel)(this.ViewModel);

            accountsScrollView = new UIScrollView();
            ((GSCBaseView)View).baseContainer.AddSubview(accountsScrollView);

            accountSections = new NSMutableArray();

            vewTotalRemaining = new UIView();
            vewTotalRemaining.BackgroundColor = Colors.BACKGROUND_COLOR;
            vewTotalRemaining.Layer.BorderColor = Colors.Black.CGColor;
            vewTotalRemaining.Layer.BorderWidth = 2;
            accountsScrollView.AddSubview(vewTotalRemaining);

            totalRemainingTitle = new UILabel();
            totalRemainingTitle.Text = "totalRemaining".tr();
            totalRemainingTitle.TextColor = Colors.Black;
            totalRemainingTitle.BackgroundColor = Colors.Clear;
            totalRemainingTitle.TextAlignment = UITextAlignment.Left;
            totalRemainingTitle.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.LG_HEADING_FONT_SIZE);
            vewTotalRemaining.AddSubview(totalRemainingTitle);

            totalRemainingAmount = new UILabel();
            totalRemainingAmount.Text = "$";
            totalRemainingAmount.TextColor = Colors.HIGHLIGHT_COLOR;
            totalRemainingAmount.BackgroundColor = Colors.Clear;
            totalRemainingAmount.TextAlignment = UITextAlignment.Left;
            totalRemainingAmount.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.BUTTON_FONT_SIZE);
            vewTotalRemaining.AddSubview(totalRemainingAmount);

            LabNoData = new GSCMessageLabel();
            LabNoData.Text = "noDataForSpendingAccount".tr();
            LabNoData.BackgroundColor = Colors.Clear;
            LabNoData.TextColor = Colors.DARK_GREY_COLOR;
            LabNoData.TextAlignment = UITextAlignment.Center;
            LabNoData.Lines = 0;
            LabNoData.LineBreakMode = UILineBreakMode.WordWrap;
            LabNoData.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.HEADING_FONT_SIZE);
            accountsScrollView.Add(LabNoData);
            LabNoData.Hidden = true;

            var set = this.CreateBindingSet<SpendingAccountDetailView, SpendingAccountDetailViewModel>();
            set.Bind(this).For(v => v.accounts).To(vm => vm.SpendingAccountDetails.AccountRollups);
            set.Bind(this).For(v => v.account).To(vm => vm.SpendingAccountDetails.AccountType);
            set.Bind(this).For(v => v.Busy).To(vm => vm.Busy);
            set.Bind(totalRemainingAmount).To(vm => vm.SpendingAccountDetails.TotalRemaining).WithConversion("DollarSignDoublePrefix");
            set.Bind(vewTotalRemaining).For(b => b.Hidden).To(vm => vm.SpendingAccountDetails.IsTotalRemainingVisible).WithConversion("BoolOpposite");
            set.Bind(LabNoData).For(v => v.Hidden).To(vm => vm.SpendingAccountDetails.AccountRollups.Count).WithConversion("IntToBool").WithConversion("BoolOpposite");
            set.Apply();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            base.NavigationController.NavigationBarHidden = false;

            ((GSCBaseView)View).subscribeToBusyIndicator();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            ((GSCBaseView)View).unsubscribeFromBusyIndicator();
        }

        void populateList()
        {
            if (accounts == null)
            {
                LabNoData.Hidden = true;
                return;
            }
            clearList();
            int accountNumber = accounts.Count;
            if (accountNumber > 0)
            {
                for (var i = 0; i < accounts.Count; i++)
                {
                    if (Constants.IsPhone())
                    {
                        SpendingAccountDetailSection section = new SpendingAccountDetailSection(accounts[i]);
                        accountSections.Add(section);
                        accountsScrollView.AddSubview(section);
                    }
                    else
                    {
                        SpendingAccountDetailSectionIPad section = new SpendingAccountDetailSectionIPad(accounts[i]);
                        accountSections.Add(section);
                        accountsScrollView.AddSubview(section);
                    }
                }
            }
            else
            {
                LabNoData.Hidden = false;
            }
            this.View.SetNeedsLayout();
        }

        void clearList()
        {
            for (var i = 0; i < (uint)accountSections.Count; i++)
            {

                if (Constants.IsPhone())
                {
                    (accountSections.GetItem<SpendingAccountDetailSection>((nuint)i)).RemoveFromSuperview();
                }
                else
                {
                    (accountSections.GetItem<SpendingAccountDetailSectionIPad>((nuint)i)).RemoveFromSuperview();
                }
            }

            accountSections.RemoveAllObjects();

        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            System.Console.WriteLine("rotate");
            base.DidRotate(fromInterfaceOrientation);
        }


        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            nfloat startY = ViewContentYPositionPadding;

            nfloat contentY = startY;
            float contentWidth = ViewContainerWidth - Constants.BALANCES_SIDE_PADDING * 2;

            accountsScrollView.Frame = new CGRect(0, 0, ViewContainerWidth, GetBottomPadding(ViewContainerHeight + 10, ViewContainerHeight));

            int accountSectionNumber = (int)accountSections.Count;
            if (accountSectionNumber > 0)
            {
                LabNoData.Hidden = true;
                for (var i = 0; i < (uint)accountSections.Count; i++)
                {

                    if (Constants.IsPhone())
                    {
                        SpendingAccountDetailSection section = accountSections.GetItem<SpendingAccountDetailSection>((nuint)i);
                        section.setLocalFrames();
                        section.Frame = new CGRect(0, contentY, ViewContainerWidth, section.height);
                        contentY += section.height + TOP_PADDING;
                    }
                    else
                    {
                        SpendingAccountDetailSectionIPad section = accountSections.GetItem<SpendingAccountDetailSectionIPad>((nuint)i);
                        section.Frame = new CGRect(0, contentY, ViewContainerWidth, section.height);
                        section.setLocalFrames();
                        contentY += section.height + TOP_PADDING;
                    }
                }

                totalRemainingTitle.SizeToFit();
                totalRemainingAmount.SizeToFit();
                vewTotalRemaining.Frame = new CGRect(10, contentY, ViewContainerWidth - 20, totalRemainingTitle.Frame.Height + 20);
                totalRemainingTitle.Frame = new CGRect(10, 10, (float)totalRemainingTitle.Frame.Width, (float)totalRemainingTitle.Frame.Height);
                nfloat startYAmount = vewTotalRemaining.Frame.Height / 2 - totalRemainingAmount.Frame.Height / 2;
                totalRemainingAmount.Frame = new CGRect(vewTotalRemaining.Frame.Width - (float)totalRemainingAmount.Frame.Width - 10, startYAmount, totalRemainingAmount.Frame.Width, totalRemainingAmount.Frame.Height);
                contentY = vewTotalRemaining.Frame.GetMaxY() + 5;
                Busy = false;
            }
            else
            {
                LabNoData.Hidden = Busy;
                LabNoData.Frame = new CGRect(0, contentY, ViewContainerWidth, 200);
                contentY = ViewContainerHeight;
            }
            accountsScrollView.ContentSize = new CGSize(ViewContainerWidth, contentY);
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
                return (float)((GSCBaseView)View).baseContainer.Bounds.Height - Constants.NAV_HEIGHT;
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
                return Constants.CLAIMS_DETAILS_SUB_ITEM_PADDING;
            }
            else
            {
                return Constants.IOS_7_TOP_PADDING;
            }
        }

        #region Properties
        private SpendingAccountType _account;
        public SpendingAccountType account
        {
            get
            {
                return _account;
            }
            set
            {
                _account = value;
                string name = _account.ModelName;
                if (name != null)
                {
                    name = name.ToUpper();
                }
                base.NavigationItem.Title = name;
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
                    LabNoData.Hidden = true;
                    InvokeOnMainThread(() =>
                    {
                        ((GSCBaseView)View).startLoading();
                    });
                }

            }
        }

        private List<SpendingAccountPeriodRollup> _accounts;
        public List<SpendingAccountPeriodRollup> accounts
        {
            get
            {
                return _accounts;
            }
            set
            {
                _accounts = value;
                populateList();
            }
        }
        #endregion
    }
}