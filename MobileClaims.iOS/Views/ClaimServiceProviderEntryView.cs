using System;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Converters;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS
{
    [Register("ClaimServiceProviderEntryView")]
    public class ClaimServiceProviderEntryView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
    {
        protected UIScrollView scrollContainer;
        protected UILabel submissionTypeLabel, mandatoryLabel, titleLabel;
        protected DefaultTextField nameText, add1Text, add2Text, cityText, provinceText, postalText, phoneText, registText;
        protected GSButton continueButton;
        protected GSButton closeButton;
        protected ProvinceListSubComponent provinces;
        UIButton customBackButton;

        private float BUTTON_WIDTH = Constants.IsPhone() ? 270 : 400;
        private float BUTTON_HEIGHT = Constants.IsPhone() ? 40 : 60;

        private float FIELD_HEIGHT = 40;

        protected ClaimServiceProviderEntryViewModel _model;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.Frame = new CGRect((float)View.Frame.Width / 2 - (float)View.Frame.Width * 0.8f / 2,
                (float)View.Frame.Height / 2 - (float)View.Frame.Height * 0.8f / 2,
                (float)View.Frame.Width * 0.8f,
                (float)View.Frame.Height * 0.8f);

            _model = (ClaimServiceProviderEntryViewModel)ViewModel;
           
            View = new GSCBaseView()
            {
                BackgroundColor = Colors.BACKGROUND_COLOR
            };

            this.AutomaticallyAdjustsScrollViewInsets = false;

            scrollContainer = ((GSCBaseView)View).baseScrollContainer;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollContainer);
            ((GSCBaseView)View).ViewTapped += HandleViewTapped;

            base.NavigationController.NavigationBarHidden = false;

            submissionTypeLabel = new UILabel();
            submissionTypeLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.LG_HEADING_FONT_SIZE);
            submissionTypeLabel.TextColor = Colors.DARK_GREY_COLOR;
            submissionTypeLabel.TextAlignment = UITextAlignment.Left;
            submissionTypeLabel.LineBreakMode = UILineBreakMode.WordWrap;
            submissionTypeLabel.Lines = 0;
            submissionTypeLabel.BackgroundColor = Colors.Clear;
            scrollContainer.AddSubview(submissionTypeLabel);

            mandatoryLabel = new UILabel();
            mandatoryLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.SMALL_FONT_SIZE);
            mandatoryLabel.TextColor = Colors.MED_GREY_COLOR;
            mandatoryLabel.TextAlignment = UITextAlignment.Left;            
            mandatoryLabel.BackgroundColor = Colors.Clear;
            scrollContainer.AddSubview(mandatoryLabel);

            nameText = new DefaultTextField();            
            scrollContainer.AddSubview(nameText);

            add1Text = new DefaultTextField();
            scrollContainer.AddSubview(add1Text);

            add2Text = new DefaultTextField();
            scrollContainer.AddSubview(add2Text);

            cityText = new DefaultTextField();
            scrollContainer.AddSubview(cityText);

            provinces = new ProvinceListSubComponent(this, "province".tr() + "*");
            provinces.LabelLeftMargin = 10f;
            provinces.LabelHeight = FIELD_HEIGHT;
            DismmssVCTableViewSource tableSource = new DismmssVCTableViewSource(provinces.popoverController, provinces.listController.tableView, "ServiceProviderProvinceTableCell", typeof(ServiceProviderProvinceTableCell));
            provinces.listController.tableView.Source = tableSource;
            scrollContainer.AddSubview(provinces);

            postalText = new DefaultTextField();
            scrollContainer.AddSubview(postalText);

            phoneText = new DefaultTextField();
            phoneText.KeyboardType = UIKeyboardType.NumberPad;
            scrollContainer.AddSubview(phoneText);

            registText = new DefaultTextField();
            scrollContainer.AddSubview(registText);

            continueButton = new GSButton();
            scrollContainer.AddSubview(continueButton);

            var boolToCGColorValueConverter = new BoolToCGColorValueConverter();
            var set = this.CreateBindingSet<ClaimServiceProviderEntryView, ClaimServiceProviderEntryViewModel>();
            set.Bind(NavigationItem).For(x => x.Title).To(vm => vm.Title);
            set.Bind(submissionTypeLabel).To(vm => vm.EnterDetailsLabel);
            set.Bind(mandatoryLabel).To(vm => vm.MandatoryFieldsLabel);
            set.Bind(nameText).For(x=>x.Placeholder).To(vm => vm.NameLabel);
            set.Bind(add1Text).For(x => x.Placeholder).To(vm => vm.Address1Label);
            set.Bind(add2Text).For(x => x.Placeholder).To(vm => vm.Address2Label);
            set.Bind(cityText).For(x => x.Placeholder).To(vm => vm.CityLabel);
            // set.Bind(provinceText).To(vm => vm.ProvinceLabel);
            set.Bind(postalText).For(x => x.Placeholder).To(vm => vm.PostalCodeLabel);
            set.Bind(phoneText).For(x => x.Placeholder).To(vm => vm.PhoneLabel);
            set.Bind(registText).For(x => x.Placeholder).To(vm => vm.RegistrationLabel);
            set.Bind(continueButton).For("Title").To(vm => vm.ButtonLabel);

            set.Bind(nameText).To(vm => vm.Name);
            set.Bind(add1Text).To(vm => vm.Address1);
            set.Bind(add2Text).To(vm => vm.Address2);
            set.Bind(cityText).To(vm => vm.City);
            set.Bind(tableSource).To(vm => vm.Provinces);
            set.Bind(tableSource).For(s => s.SelectedItem).To(vm => vm.SelectedProvince);
            set.Bind(provinces.detailsLabel).To(vm => vm.SelectedProvince.Name);
            set.Bind(provinces.detailsLabel).For(l => l.TextColor).To(vm => vm.SelectedProvince).WithConversion("PlaceHolderTextColor");
            set.Bind(postalText).To(vm => vm.PostalCode);
            set.Bind(phoneText).To(vm => vm.Phone);
            set.Bind(registText).To(vm => vm.RegistrationNumber);
            set.Bind(continueButton).To(vm => vm.UseThisProviderCommand);

            set.Bind(nameText.Layer).For(x => x.BorderColor).To(vm => vm.NameValid).WithConversion(boolToCGColorValueConverter);
            set.Bind(add1Text.Layer).For(x => x.BorderColor).To(vm => vm.Address1Valid).WithConversion(boolToCGColorValueConverter);
            set.Bind(add2Text.Layer).For(x => x.BorderColor).To(vm => vm.Address2Valid).WithConversion(boolToCGColorValueConverter);
            set.Bind(cityText.Layer).For(x => x.BorderColor).To(vm => vm.CityValid).WithConversion(boolToCGColorValueConverter);
            set.Bind(provinces.listContainerBackground.Layer).For(x => x.BorderColor).To(vm => vm.SelectedProvinceValid).WithConversion(boolToCGColorValueConverter);
            set.Bind(postalText.Layer).For(x => x.BorderColor).To(vm => vm.PostalCodeValid).WithConversion(boolToCGColorValueConverter);
            set.Bind(phoneText.Layer).For(x => x.BorderColor).To(vm => vm.PhoneValid).WithConversion(boolToCGColorValueConverter);
            set.Bind(registText.Layer).For(x => x.BorderColor).To(vm => vm.RegistrationNumberValid).WithConversion(boolToCGColorValueConverter);
            set.Apply();
            
            SetLabels();
            _model.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case "ClaimSubmissionType":
                        SetLabels();
                        break;
                    default:
                        break;
                }
            };

            if (!Constants.IsPhone())
            {
                ((GSCBaseView)View).subscribeToBusyIndicator();
            }
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

            if (Constants.IsPhone())
            {
                ((GSCBaseView)View).subscribeToBusyIndicator();
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            dismissKeyboard();
            ((GSCBaseView)View).unsubscribeFromBusyIndicator();
        }

        public override void ViewDidLayoutSubviews()
        {
            SetFrames();
            base.ViewDidLayoutSubviews();
        }

        private void SetFrames()
        {
            base.ViewDidLayoutSubviews();

            if (View.Superview == null)
            {
                return;
            }

            float startY = ViewContentYPositionPadding;
            float titleAreaHeight = Constants.IsPhone() ? 0 : 40;
            float bottomPadding = Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;

            float contentWidth = ViewContainerWidth - Constants.CONTENT_SIDE_PADDING * 2;
            float xPos = Constants.CONTENT_SIDE_PADDING;

            float innerPadding = 10;

            if (customBackButton != null)
            {
                customBackButton.Frame = new CGRect(5, 5, 75, titleAreaHeight - 10);
            }

            if (titleLabel != null)
            {
                titleLabel.Frame = new CGRect((float)View.Frame.Width / 2 - (float)titleLabel.Frame.Width / 2, titleAreaHeight / 2 - (float)titleLabel.Frame.Height / 2, (float)titleLabel.Frame.Width, (float)titleLabel.Frame.Height);
                titleLabel.SizeToFit();
                titleLabel.Frame = new CGRect((float)View.Frame.Width / 2 - (float)titleLabel.Frame.Width / 2, titleAreaHeight / 2 - (float)titleLabel.Frame.Height / 2, (float)titleLabel.Frame.Width, (float)titleLabel.Frame.Height);
            }

            submissionTypeLabel.Frame = new CGRect(Constants.CONTENT_SIDE_PADDING, startY, ViewContainerWidth - Constants.CONTENT_SIDE_PADDING * 2, (float)submissionTypeLabel.Frame.Height);
            submissionTypeLabel.SizeToFit();

            mandatoryLabel.Frame = new CGRect(Constants.CONTENT_SIDE_PADDING, (float)submissionTypeLabel.Frame.Height + (float)submissionTypeLabel.Frame.Y + 10, (float)submissionTypeLabel.Frame.Width, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
            nameText.Frame = new CGRect(xPos, (float)mandatoryLabel.Frame.Height + (float)mandatoryLabel.Frame.Y + 10, contentWidth, FIELD_HEIGHT);
            add1Text.Frame = new CGRect(xPos, nameText.Frame.Height + nameText.Frame.Y + innerPadding, contentWidth, FIELD_HEIGHT);
            add2Text.Frame = new CGRect(xPos, add1Text.Frame.Height + add1Text.Frame.Y + innerPadding, contentWidth, FIELD_HEIGHT);
            cityText.Frame = new CGRect(xPos, add2Text.Frame.Height + add2Text.Frame.Y + innerPadding, contentWidth, FIELD_HEIGHT);
            provinces.Frame = new CGRect(xPos, cityText.Frame.Height + cityText.Frame.Y + innerPadding, contentWidth, FIELD_HEIGHT);
            postalText.Frame = new CGRect(xPos, provinces.Frame.Height + provinces.Frame.Y + innerPadding, contentWidth, FIELD_HEIGHT);
            phoneText.Frame = new CGRect(xPos, postalText.Frame.Height + postalText.Frame.Y + innerPadding, contentWidth, FIELD_HEIGHT);
            registText.Frame = new CGRect(xPos, phoneText.Frame.Height + phoneText.Frame.Y + innerPadding, contentWidth, FIELD_HEIGHT);
            continueButton.Frame = new CGRect(ViewContainerWidth / 2 - BUTTON_WIDTH / 2, registText.Frame.Height + registText.Frame.Y + 30, BUTTON_WIDTH, BUTTON_HEIGHT);

            scrollContainer.Frame = new CGRect(0, titleAreaHeight, ViewContainerWidth, ViewContainerHeight);
            scrollContainer.ContentSize = new CGSize(ViewContainerWidth, continueButton.Frame.Y + continueButton.Frame.Height + bottomPadding);
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
            float titleAreaHeight = Constants.IsPhone() ? 0 : 40;
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return (float)((GSCBaseView)View).baseContainer.Bounds.Height - (Constants.IsPhone() ? Helpers.BottomNavHeight() : titleAreaHeight);
            }
            else
            {
                return (float)View.Superview.Frame.Height - (Constants.IsPhone() ? Helpers.BottomNavHeight() : titleAreaHeight);
            }
        }

        float IGSCBaseViewImplementor.ViewContentYPositionPadding()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return 20;
            }
            else
            {
                return (Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING);
            }

        }

        private void SetLabels()
        {
            continueButton.SetTitle("useThisProvider".tr(), UIControlState.Normal);
        }
    }
}