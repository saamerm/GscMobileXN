using System;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using QuickLook;
using UIKit;

namespace MobileClaims.iOS
{
    [Foundation.Register("DrugLookupResultsView")]
    public class DrugLookupResultsView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
    {
        protected DrugInfo drugInfo;

        protected UILabel planParticipantTitleLabel;
        protected UILabel planParticipantLabel;
        protected UILabel drugInformationLabel;
        protected UILabel notesLabel;

        protected GSSelectionButtonSmall downloadButton;
        protected GSSelectionButtonSmall emailButton;

        private GSSelectionButtonSmall _findPharmacyButton;

        protected UIBarButtonItem newSearchButton;

        protected UILabel notesField;

        protected InformationHeadingAndDetailsComponent drugNameDetails;
        protected InformationHeadingAndDetailsComponent dinDetails;
        protected InformationHeadingAndDetailsComponent coveredDetails;
        protected InformationHeadingAndDetailsComponent reimbursmentDetails;
        protected InformationHeadingAndDetailsComponent specialAuthDetails;
        protected InformationHeadingAndDetailsComponent specialMessages;

        protected UIView buttonContainer;

        protected UIScrollView informationScrollView;

        protected bool isNameLookup;

        protected QLPreviewController _previewController;

        protected DrugLookupResultsViewModel _model;

        private const float BUTTON_CONTAINER_HEIGHT = 60;
        private float BUTTON_WIDTH = Constants.IsPhone() ? 145 : 200;
        private const float BUTTON_PADDING = 20;
        private const float BUTTON_PADDING_IPAD = 20;
        private const float BUTTON_HEIGHT = 45;

        private MvxFluentBindingDescriptionSet<DrugLookupResultsView, Core.ViewModels.DrugLookupResultsViewModel> set;
        int redrawCount = 0;
        private bool componentsReady;

        public DrugLookupResultsView()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            base.NavigationItem.Title = "myDrugResults".tr();
            _model = (DrugLookupResultsViewModel)(this.ViewModel);
            if (Constants.IS_OS_7_OR_LATER())
                this.AutomaticallyAdjustsScrollViewInsets = false;

            base.NavigationItem.SetHidesBackButton(false, false);

            newSearchButton = new UIBarButtonItem();
            newSearchButton.Style = UIBarButtonItemStyle.Plain;
            newSearchButton.Clicked += NewSearchClicked;
            newSearchButton.Title = "newSearch".tr();
            newSearchButton.TintColor = Colors.HIGHLIGHT_COLOR;
            UITextAttributes attributes = new UITextAttributes();
            attributes.Font = UIFont.FromName(Constants.NUNITO_REGULAR, (nfloat)Constants.NAV_BAR_BUTTON_SIZE);
            newSearchButton.SetTitleTextAttributes(attributes, UIControlState.Normal);
            base.NavigationItem.RightBarButtonItem = newSearchButton;

            informationScrollView = new UIScrollView();
            informationScrollView.BackgroundColor = Colors.Clear;
            ((GSCBaseView)View).baseContainer.AddSubview(informationScrollView);

            //Instantiating buttons early for binding
            emailButton = new GSSelectionButtonSmall();
            downloadButton = new GSSelectionButtonSmall();

            _findPharmacyButton = new GSSelectionButtonSmall();

            set = this.CreateBindingSet<DrugLookupResultsView, Core.ViewModels.DrugLookupResultsViewModel>();
            set.Bind(this).For(v => v.Busy).To(vm => vm.Busy);
            set.Bind(this).For(v => v.sentSpecialAuth).To(vm => vm.SentSpecialAuth);
            set.Bind(this).For(v => v.specialAuthError).To(vm => vm.SpecialAuthError);
            set.Bind(downloadButton).To(vm => vm.DownloadSpecialAuthorizationCommand);
            set.Bind(_findPharmacyButton).To(vm => vm.ShowFindPharmaciesProviderCommand);

            set.Apply();

            if (!_model.Busy)
            {
                layoutComponents();
            }
            else
            {
                ((GSCBaseView)View).startLoading();
            }
        }

        void NewSearchClicked(object sender, EventArgs ea)
        {
            _model.NewSearchCommand.Execute(null);
        }

        public void layoutComponents()
        {
            _model = (DrugLookupResultsViewModel)(this.ViewModel);
            drugInfo = _model.Drug;

            _model.FetchSpecialAuthComplete += HandleFetchSpecialAuthComplete;
            _model.OnEmailComplete += HandleOnEmailComplete;
            _model.ErrorFetchingSpecialAuth += HandleErrorFetchingSpecialAuth;
            _model.OnEmailError += HandleOnEmailError;

            if (_model.Drug != null && _model.Drug.SpecialAuthFormName != null)
            {

                if (downloadButton != null)
                    downloadButton.Enabled = true;

                if (emailButton != null)
                    emailButton.Enabled = true;
            }

            planParticipantTitleLabel = new UILabel();
            planParticipantTitleLabel.Text = "planParticipant".tr();
            planParticipantTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
            planParticipantTitleLabel.TextAlignment = UITextAlignment.Right;
            planParticipantTitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            informationScrollView.AddSubview(planParticipantTitleLabel);

            planParticipantLabel = new UILabel();
            planParticipantLabel.Text = _model.Participant.FullName;
            planParticipantLabel.TextColor = Colors.HIGHLIGHT_COLOR;
            planParticipantLabel.Font = UIFont.FromName(Constants.NUNITO_BLACK, (nfloat)Constants.PARTICPAINT_RESULTS_FONT_SIZE);
            planParticipantLabel.BackgroundColor = Colors.Clear;
            planParticipantLabel.TextAlignment = UITextAlignment.Right;
            informationScrollView.AddSubview(planParticipantLabel);

            drugInformationLabel = new UILabel();
            drugInformationLabel.Text = "drugInformation".tr();
            drugInformationLabel.TextColor = Colors.DARK_GREY_COLOR;
            drugInformationLabel.BackgroundColor = Colors.Clear;
            drugInformationLabel.TextAlignment = UITextAlignment.Left;
            drugInformationLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            informationScrollView.AddSubview(drugInformationLabel);

            drugNameDetails = new InformationHeadingAndDetailsComponent(true, "drugNameAndStrength".tr(), drugInfo.Name);
            informationScrollView.AddSubview(drugNameDetails);

            dinDetails = new InformationHeadingAndDetailsComponent(true, "drugDINFieldText".tr(), drugInfo.DIN.ToString());
            informationScrollView.AddSubview(dinDetails);

            coveredDetails = new InformationHeadingAndDetailsComponent(true, "drugCovered".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada.ToUpperInvariant()) , (!string.IsNullOrEmpty(drugInfo.CoveredMessage)) ? drugInfo.CoveredMessage : "no".tr());
            informationScrollView.AddSubview(coveredDetails);

            reimbursmentDetails = new InformationHeadingAndDetailsComponent(true, "reimbursement".tr(), String.IsNullOrEmpty(drugInfo.Reimbursement) ? "" : drugInfo.Reimbursement);
            informationScrollView.AddSubview(reimbursmentDetails);
            reimbursmentDetails.Hidden = String.IsNullOrEmpty(drugInfo.Reimbursement);

            specialAuthDetails = new InformationHeadingAndDetailsComponent(false, "specialAuthRequired".tr(), drugInfo.SpecialAuthRequired ? drugInfo.AuthorizationMessage : "");
            informationScrollView.AddSubview(specialAuthDetails);
            specialAuthDetails.Hidden = !drugInfo.SpecialAuthRequired;

            specialMessages = new InformationHeadingAndDetailsComponent(false, "specialMessages".tr(), "reimbursementMessage".tr());
            informationScrollView.AddSubview(specialMessages);
            specialMessages.Hidden = !drugInfo.LowCostReplacementOccurred;

            if (drugInfo.SpecialAuthRequired && !string.IsNullOrEmpty(_model.Drug.SpecialAuthFormName))
            {

                buttonContainer = new UIView();
                buttonContainer.BackgroundColor = Colors.BACKGROUND_COLOR;
                buttonContainer.Layer.MasksToBounds = true;
                informationScrollView.AddSubview(this.buttonContainer);

                downloadButton.SetTitle("downloadAndView".tr(), UIControlState.Normal);
                downloadButton.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
                downloadButton.TitleLabel.TextAlignment = UITextAlignment.Center;
                buttonContainer.AddSubview(this.downloadButton);

                emailButton.SetTitle("sendByEmail".tr(), UIControlState.Normal);
                emailButton.TouchUpInside += sendClicked;
                emailButton.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
                emailButton.TitleLabel.TextAlignment = UITextAlignment.Center;
                buttonContainer.AddSubview(this.emailButton);
            }
            
            _findPharmacyButton.SetTitle("findPharmacy".tr(), UIControlState.Normal);
            _findPharmacyButton.TitleLabel.TextAlignment = UITextAlignment.Center;
            informationScrollView.AddSubview(_findPharmacyButton);

            notesLabel = new UILabel();
            notesLabel.Text = "notes".tr();
            notesLabel.TextColor = Colors.DARK_GREY_COLOR;
            notesLabel.BackgroundColor = Colors.Clear;
            notesLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SUB_HEADING_FONT_SIZE);
            notesLabel.TextAlignment = UITextAlignment.Left;
            informationScrollView.AddSubview(notesLabel);

            notesField = new UILabel();
            if (drugInfo != null && drugInfo.Notes != null)
            {
                notesField.Text = drugInfo.Notes + "\r\n\r\n" + "notesFieldText".FormatWithBrandKeywords(LocalizableBrand.GSC);
            }
            notesField.TextColor = Colors.DARK_GREY_COLOR;
            notesField.TextAlignment = UITextAlignment.Left;
            notesField.Font = UIFont.FromName(Constants.NUNITO_REGULAR, (nfloat)Constants.SUB_HEADING_FONT_SIZE);
            notesField.BackgroundColor = Colors.Clear;
            notesField.Lines = 0;
            notesField.LineBreakMode = UILineBreakMode.WordWrap;
            informationScrollView.AddSubview(notesField);


            componentsReady = true;
            View.SetNeedsLayout();
        }

        void HandleOnEmailError(object sender, EventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                UIAlertView _error = new UIAlertView("", "emailFailMessage".tr(), null, "ok".tr(), null);
                _error.Show();
            });
        }

        void HandleErrorFetchingSpecialAuth(object sender, EventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                UIAlertView _error = new UIAlertView("", "downloadErrorMessage".tr(), null, "ok".tr(), null);
                _error.Show();
            });
        }

        void HandleOnEmailComplete(object sender, EventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                UIAlertView _error = new UIAlertView("", "emailSentMessage".tr(), null, "ok".tr(), null);
                _error.Show();
            });
        }

        void HandleFetchSpecialAuthComplete(object sender, EventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                UIModalPresentationStyle style = new UIModalPresentationStyle();
                _previewController = new QLPreviewController();
                _previewController.DataSource = new DataSource(_model);
                _previewController.Delegate = new PreviewDelegate(this);
                _previewController.CurrentPreviewItemIndex = 0;
                _previewController.ModalPresentationStyle = style;
                PresentViewController(_previewController, true, null);
            });

        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            this.View.SetNeedsLayout();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            if (!componentsReady)
                return;

            //Status bar height acting peculiar in landscape. TODO: Figure better way to get status bar height. Tried: UIApplication.SharedApplication.StatusBarFrame.Size.Height
            float statusBarHeight = 20;

            //float viewwidth = (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            //float viewHeight = (float)((GSCBaseView)View).baseContainer.Bounds.Height - Helpers.BottomNavHeight();

            float startY = ViewContentYPositionPadding;
            float contentWidth = ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2;
            float buttonPadding = Constants.IsPhone() ? BUTTON_PADDING : BUTTON_PADDING_IPAD;
            float pharmacyButtonWidth = buttonPadding * 4;

            //TODO: Scroll views behaving strangely. Added tweaks here to fix. Remove eventually
            informationScrollView.Frame = new CGRect(0, 0, ViewContainerWidth, ViewContainerHeight);

            float itemPadding = Constants.CLAIMS_DETAILS_COMPONENT_PADDING;

            float yPos = ViewContentYPositionPadding; // itemPadding;

            planParticipantTitleLabel.Frame = new CGRect(View.Frame.Width - Constants.DRUG_LOOKUP_LEFT_TEXT_INSET - planParticipantTitleLabel.Frame.Width, yPos, planParticipantTitleLabel.Frame.Width, planParticipantTitleLabel.Frame.Height);
            if (UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeLeft || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeRight)
            {
                planParticipantTitleLabel.Frame = new CGRect(ViewContainerWidth - Constants.DRUG_LOOKUP_LEFT_TEXT_INSET - planParticipantTitleLabel.Frame.Width, yPos, planParticipantTitleLabel.Frame.Width, planParticipantTitleLabel.Frame.Height);
            }
            planParticipantTitleLabel.SizeToFit();
            yPos += (float)planParticipantTitleLabel.Frame.Height + 5;
            planParticipantLabel.Frame = new CGRect(View.Frame.Width - Constants.DRUG_LOOKUP_LEFT_TEXT_INSET - planParticipantLabel.Frame.Width, yPos, planParticipantLabel.Frame.Width, planParticipantLabel.Frame.Height);
            if (UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeLeft || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeRight)
            {
                planParticipantLabel.Frame = new CGRect(ViewContainerWidth - Constants.DRUG_LOOKUP_LEFT_TEXT_INSET - planParticipantLabel.Frame.Width, yPos, planParticipantLabel.Frame.Width, planParticipantLabel.Frame.Height);
            }
            planParticipantLabel.SizeToFit();
            yPos += (float)planParticipantLabel.Frame.Height + Constants.DRUG_LOOKUP_TOP_PADDING;
            drugInformationLabel.Frame = new CGRect(Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, yPos, ViewContainerWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
            yPos += (float)drugInformationLabel.Frame.Height;

            drugNameDetails.Frame = new CGRect(0, yPos, ViewContainerWidth, drugNameDetails.Frame.Height);
            drugNameDetails.SetNeedsLayout();
            drugNameDetails.Frame = new CGRect(0, yPos, ViewContainerWidth, drugNameDetails.Frame.Height);
            yPos += drugNameDetails.ComponentHeight;

            dinDetails.Frame = new CGRect(0, yPos, ViewContainerWidth, dinDetails.Frame.Height);
            dinDetails.SetNeedsLayout();
            dinDetails.Frame = new CGRect(0, yPos, ViewContainerWidth, dinDetails.Frame.Height);
            yPos += dinDetails.ComponentHeight;

            if (!coveredDetails.Hidden)
            {
                coveredDetails.Frame = new CGRect(0, yPos, ViewContainerWidth, coveredDetails.Frame.Height);
                coveredDetails.SetNeedsLayout();
                yPos += coveredDetails.ComponentHeight;
            }

            if (!reimbursmentDetails.Hidden)
            {
                reimbursmentDetails.Frame = new CGRect(0, yPos, ViewContainerWidth, reimbursmentDetails.Frame.Height);
                reimbursmentDetails.SetNeedsLayout();
                yPos += reimbursmentDetails.ComponentHeight;
            }


            if (!specialAuthDetails.Hidden)
            {
                specialAuthDetails.Frame = new CGRect(0, yPos, ViewContainerWidth, specialAuthDetails.Frame.Height);
                specialAuthDetails.SetNeedsLayout();
                yPos += specialAuthDetails.ComponentHeight;
            }

            if (!specialMessages.Hidden)
            {
                specialMessages.Frame = new CGRect(0, yPos, ViewContainerWidth, specialMessages.Frame.Height);
                specialMessages.SetNeedsLayout();
                yPos += specialMessages.ComponentHeight;
            }

            yPos += Constants.DRUG_LOOKUP_TOP_PADDING;

            if ((buttonContainer != null) && (emailButton != null))
            {
                buttonContainer.Frame = new CGRect(0, yPos, ViewContainerWidth, BUTTON_CONTAINER_HEIGHT);
                downloadButton.Frame = new CGRect(ViewContainerWidth / 2 - BUTTON_WIDTH - buttonPadding / 2, BUTTON_CONTAINER_HEIGHT / 2 - BUTTON_HEIGHT / 2, BUTTON_WIDTH, BUTTON_HEIGHT);
                emailButton.Frame = new CGRect(ViewContainerWidth / 2 + buttonPadding / 2, BUTTON_CONTAINER_HEIGHT / 2 - BUTTON_HEIGHT / 2, BUTTON_WIDTH, BUTTON_HEIGHT);
                yPos += (float)buttonContainer.Frame.Height + Constants.DRUG_LOOKUP_TOP_PADDING;
            }

            _findPharmacyButton.Frame = new CGRect(pharmacyButtonWidth,
                                                   yPos,
                                                   ViewContainerWidth - 2 * pharmacyButtonWidth, 
                                                   BUTTON_HEIGHT);

            yPos += BUTTON_HEIGHT + buttonPadding;

            notesLabel.Frame = new CGRect(Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, yPos, ViewContainerWidth - Constants.DRUG_LOOKUP_LEFT_TEXT_INSET * 2, Constants.DRUG_LOOKUP_LABEL_HEIGHT);

            notesField.Frame = new CGRect(Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, notesLabel.Frame.Y + notesLabel.Frame.Height, ViewContainerWidth - Constants.DRUG_LOOKUP_LEFT_TEXT_INSET * 2, notesField.Frame.Height);
            notesField.SizeToFit();

            informationScrollView.ContentSize = new CGSize(contentWidth, notesField.Frame.Y + 
                                                           notesField.Frame.Height + 
                                                           GetBottomPadding(Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING, 10));

            //TODO: Another timing issue. Need to force redraw for the time being
            if (redrawCount < 2)
            {
                redrawCount++;
                this.View.SetNeedsLayout();
            }
        }

        public void QuickLookWillDismiss()
        {
            //Might be unnecessary
        }

        public override void DismissViewController(bool animated, Action completionHandler)
        {
            base.DismissViewController(animated, (Action)completionHandler);
        }

        void sendClicked(object s, EventArgs ea)
        {
            int buttonClicked = -1;

            UIAlertView alert = new UIAlertView("sendAuthForm".tr(), "", null, "cancelCaps".tr(), null);
            alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
            UITextField textFieldForKeyboard = alert.GetTextField((nint)0);
            textFieldForKeyboard.KeyboardType = UIKeyboardType.EmailAddress;
            alert.AddButton("send".tr());

            alert.DismissWithClickedButtonIndex((nint)1, true);

            alert.Clicked += delegate (object a, UIButtonEventArgs b)
            {
                if ((int)b.ButtonIndex == 1)
                {
                    UITextField textField = alert.GetTextField((nint)0);
                    System.Console.WriteLine(textField.Text);
                    _model.SpecialAuthEMail = textField.Text;
                    _model.ExecuteSendSpecialAuthorizationCommand.Execute(null);
                }

            };
            alert.Show();
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


        #region Properties
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
                    layoutComponents();
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

        private bool _sentSpecialAuth = false;
        public bool sentSpecialAuth
        {
            get
            {
                return _sentSpecialAuth;
            }
            set
            {
                _sentSpecialAuth = value;

            }
        }

        private string _specialAuthError = "";
        public string specialAuthError
        {
            get
            {
                return _specialAuthError;
            }
            set
            {
                _specialAuthError = value;
                if (!string.IsNullOrEmpty(_specialAuthError) && !sentSpecialAuth)
                {
                    UIAlertView alert = new UIAlertView("Error", _specialAuthError, new UIAlertViewDelegate(), "OK", null);
                    alert.Show();
                }

            }
        }
        #endregion

        public class DataSource : QLPreviewControllerDataSource
        {
            DrugLookupResultsViewModel _model;


            public DataSource(DrugLookupResultsViewModel model)
            {
                _model = model;
            }

            public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
            {
                return (IQLPreviewItem)new PreviewItem(_model.PathToSpecialAuthForm, _model.Drug.Name);
            }

            public override nint PreviewItemCount(QLPreviewController controller)
            {
                return (nint)1;
            }
        }

        public class PreviewDelegate : QLPreviewControllerDelegate
        {
            protected DrugLookupResultsView _view;

            public PreviewDelegate(DrugLookupResultsView view)
            {
                _view = view;
            }
            public override void DidDismiss(QLPreviewController controller)
            {
                _view.QuickLookWillDismiss();

            }
            public override void WillDismiss(QLPreviewController controller)
            {
                _view.QuickLookWillDismiss();
            }
        }

        public class PreviewItem : QLPreviewItem
        {
            protected NSUrl _url;
            protected String _title;

            public PreviewItem(String path, String title)
            {
                _url = NSUrl.FromFilename(path);
                _title = title;
            }

            public override NSUrl ItemUrl
            {
                get { return _url; }
            }

            public override String ItemTitle
            {
                get { return _title; }
            }
        }
    }


}

