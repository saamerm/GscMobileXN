using System;
using System.Globalization;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS
{
    [Register("DrugLookupModelSelectionView")]
    public class DrugLookupModelSelectionView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
    {
        private DrugLookupModelSelectionViewModel _model;
        private UIScrollView _scrollView;

        protected UIView buttonContainer;
        protected UILabel drugLookupModelSelectionLabel;
        protected UILabel drugLookupModelSelectionSubLabel;

        protected GSSelectionButton nameLookupButton;
        protected GSSelectionButton dinLookupButton;

        protected UIImageView notificationImage;

        private float BUTTON_PADDING = Constants.IsPhone() ? 20 : 30;

        public DrugLookupModelSelectionView()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = "drugsOnTheGo".tr();
            base.NavigationItem.SetHidesBackButton(true, false);

            base.NavigationController.NavigationBar.TintColor = Colors.BACKGROUND_COLOR;
            base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
            base.NavigationController.View.BackgroundColor = Colors.BACKGROUND_COLOR;

            _model = (DrugLookupModelSelectionViewModel)(this.ViewModel);

            _scrollView = new UIScrollView();
            _scrollView.BackgroundColor = Colors.Clear;
            ((GSCBaseView)View).baseContainer.AddSubview(_scrollView);

            drugLookupModelSelectionLabel = new UILabel();
            drugLookupModelSelectionLabel.Text = "drugLookupModelSelectionLabel".tr();
            drugLookupModelSelectionLabel.BackgroundColor = Colors.Clear;
            drugLookupModelSelectionLabel.TextColor = Colors.DARK_GREY_COLOR;
            drugLookupModelSelectionLabel.TextAlignment = UITextAlignment.Left;
            drugLookupModelSelectionLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.LG_HEADING_FONT_SIZE);
            _scrollView.AddSubview(drugLookupModelSelectionLabel);

            drugLookupModelSelectionSubLabel = new UILabel();
            drugLookupModelSelectionSubLabel.Text = "chooseDrug".tr();
            drugLookupModelSelectionSubLabel.BackgroundColor = Colors.Clear;
            drugLookupModelSelectionSubLabel.TextColor = Colors.DARK_GREY_COLOR;
            drugLookupModelSelectionSubLabel.TextAlignment = UITextAlignment.Left;
            drugLookupModelSelectionSubLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.SUB_HEADING_FONT_SIZE);
            _scrollView.AddSubview(drugLookupModelSelectionSubLabel);

            nameLookupButton = new GSSelectionButton();
            nameLookupButton.SetTitle("byDrugName".tr(), UIControlState.Normal);
            nameLookupButton.TouchUpInside += itemClicked;
            _scrollView.AddSubview(nameLookupButton);

            dinLookupButton = new GSSelectionButton();
            dinLookupButton.SetTitle("byDIN".tr(), UIControlState.Normal);
            dinLookupButton.TouchUpInside += itemClicked;
            _scrollView.AddSubview(dinLookupButton);

#if GSC
            // The Drugs are effective image is to only be shown for GSC customers on tablet devices
            if (!Constants.IsPhone())
            {
                if (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Contains("en"))
                {
                    notificationImage = new UIImageView(UIImage.FromBundle("DrugsDontWork-En"));
                }
                else
                {
                    notificationImage = new UIImageView(UIImage.FromBundle("DrugsDontWork-Fr"));
                }
                notificationImage.BackgroundColor = Colors.Clear;
                notificationImage.Opaque = false;
                _scrollView.AddSubview(notificationImage);
            }
#endif

            var set = this.CreateBindingSet<DrugLookupModelSelectionView, DrugLookupModelSelectionViewModel>();
            set.Bind(nameLookupButton).To(vm => vm.ByNameCommand);
            set.Bind(dinLookupButton).To(vm => vm.ByDINCommand);
            set.Apply();
        }

        void itemClicked(object sender, EventArgs ea)
        {
            dinLookupButton.Selected = false;
            nameLookupButton.Selected = false;

            ((GSSelectionButton)sender).Selected = true;
        }

        private bool hasFirstOpened = false;
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            _model = (DrugLookupModelSelectionViewModel)(this.ViewModel);           
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            nameLookupButton.Selected = dinLookupButton.Selected = false;
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            float startY = ViewContentYPositionPadding;

            drugLookupModelSelectionLabel.Frame = new CGRect(Constants.BUTTON_SIDE_PADDING, startY, ViewContainerWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
            startY += Constants.DRUG_LOOKUP_LABEL_HEIGHT + Constants.DRUG_LOOKUP_TOP_PADDING;

            drugLookupModelSelectionSubLabel.Frame = new CGRect(Constants.BUTTON_SIDE_PADDING, startY, ViewContainerWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
            startY += Constants.DRUG_LOOKUP_LABEL_HEIGHT + Constants.DRUG_LOOKUP_TOP_PADDING;

            nameLookupButton.Frame = new CGRect(Constants.BUTTON_SIDE_PADDING, startY, ViewContainerWidth - Constants.BUTTON_SIDE_PADDING * 2, Constants.DRUG_BUTTON_HEIGHT);
            startY += Constants.DRUG_BUTTON_HEIGHT + BUTTON_PADDING;

            dinLookupButton.Frame = new CGRect(Constants.BUTTON_SIDE_PADDING, startY, ViewContainerWidth - Constants.BUTTON_SIDE_PADDING * 2, Constants.DRUG_BUTTON_HEIGHT);
            startY += Constants.DRUG_BUTTON_HEIGHT + BUTTON_PADDING;

            if (notificationImage != null)
            {
                float imageWidth = (float)notificationImage.Image.Size.Width;
                float imageHeight = (float)notificationImage.Image.Size.Height;

                float imageYPos = ((float)dinLookupButton.Frame.Y + (float)dinLookupButton.Frame.Height) + (ViewContainerHeight - ((float)dinLookupButton.Frame.Y + (float)dinLookupButton.Frame.Height)) / 2 - imageHeight / 2;

                notificationImage.Frame = new CGRect(ViewContainerWidth / 2 - imageWidth / 2, imageYPos, imageWidth, imageHeight);
                startY += imageHeight;
            }
            var bottomPadding = Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;

            _scrollView.Frame = new CGRect(0, 0, ViewContainerWidth, ViewContainerHeight);
            _scrollView.ContentSize = new CGSize(ViewContainerWidth, startY + GetBottomPadding());
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
                return Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;
            }
        }
    }
}