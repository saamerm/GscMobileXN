using System;
using CoreGraphics;
using UIKit;
using Foundation;
using MobileClaims.Core.ViewModels;
using MobileClaims.Core.Entities;
using MobileClaims.iOS.UI;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.iOS
{
    [Foundation.Register("DrugLookupByNameSearchResultsView")]
    public class DrugLookupByNameSearchResultsView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
    {
        protected UILabel drugResultListLabel;
        protected UILabel drugResultListSubLabel;

        protected UIScrollView resultsScrollView;
        protected NSMutableArray buttonsArray;

        protected DottedLine headingBorder;

        private DrugLookupByNameSearchResultsViewModel _model;

        private IMvxMessenger _messenger;
        private MvxSubscriptionToken _iscurrentlybusy;

        public DrugLookupByNameSearchResultsView()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            base.NavigationItem.Title = "confirm".tr();

            _model = (DrugLookupByNameSearchResultsViewModel)(this.ViewModel);

            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();

            _iscurrentlybusy = _messenger.Subscribe<Core.Messages.BusyIndicator>((message) =>
            {
                if (message.Busy)
                {
                    System.Console.WriteLine("BUSY");
                }
                else
                {
                    System.Console.WriteLine("NOT BUSY");
                }
            });

            resultsScrollView = new UIScrollView();
            resultsScrollView.BackgroundColor = Colors.Clear;
            resultsScrollView.Layer.CornerRadius = Constants.CORNER_RADIUS;
            ((GSCBaseView)View).baseContainer.AddSubview(resultsScrollView);

            drugResultListLabel = new UILabel();
            drugResultListLabel.Text = "confirmDrug".tr();
            drugResultListLabel.BackgroundColor = Colors.Clear;
            drugResultListLabel.TextColor = Colors.DARK_GREY_COLOR;
            drugResultListLabel.TextAlignment = UITextAlignment.Left;
            drugResultListLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            resultsScrollView.AddSubview(drugResultListLabel);

            if (Constants.IS_OS_7_OR_LATER())
            {
                this.AutomaticallyAdjustsScrollViewInsets = false;
            }

            headingBorder = new DottedLine();
            resultsScrollView.AddSubview(this.headingBorder);

            resultsScrollView.AddSubview(drugResultListLabel);
            buttonsArray = new NSMutableArray();

            var set = this.CreateBindingSet<DrugLookupByNameSearchResultsView, Core.ViewModels.DrugLookupByNameSearchResultsViewModel>();
            set.Bind(this).For(v => v.Busy).To(vm => vm.Busy);

            for (var i = 0; i < _model.SearchResults.Count; i++)
            {
                SelectionMenuItem resultButton = new SelectionMenuItem(true);
                DrugInfo dInfo = ((DrugInfo)_model.SearchResults[i]);
                resultButton.SetTitle(dInfo.Name, UIControlState.Normal);
                set.Bind(resultButton).To(vm => vm.SelectAndNavigateCommand).CommandParameter(dInfo);
                resultsScrollView.AddSubview(resultButton);

                buttonsArray.Add(resultButton);
            }

            set.Apply();
        }


        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            bool isPhone = Constants.IsPhone();
            float navigationBarHeight = isPhone ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;

            float contentY = ViewContentYPositionPadding;
            float contentWidth = ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2;

            drugResultListLabel.Frame = new CGRect(Constants.BUTTON_SIDE_PADDING, contentY, contentWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
            contentY += Constants.DRUG_LOOKUP_LABEL_HEIGHT + Constants.DRUG_LOOKUP_BUTTON_PADDING;

            headingBorder.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, contentY, ViewContainerWidth - Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, 1);
            contentY += Constants.DIVIDER_HEIGHT;

            for (var i = 0; i < (uint)buttonsArray.Count; i++)
            {
                SelectionMenuItem resultButton = buttonsArray.GetItem<SelectionMenuItem>((nuint)i);
                resultButton.Frame = new CGRect(0, contentY, contentWidth, Constants.PARTICIPANT_LIST_BUTTON_HEIGHT);
                contentY += Constants.PARTICIPANT_LIST_BUTTON_HEIGHT;
            }

            resultsScrollView.Frame = new CGRect(0, 0, contentWidth, ViewContainerHeight - navigationBarHeight);
            resultsScrollView.ContentSize = new CGSize(contentWidth, contentY + GetBottomPadding());
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
                    ((GSCBaseView)View).stopLoading();
                }
                else
                {
                    ((GSCBaseView)View).startLoading();
                }

            }
        }
    }
}
