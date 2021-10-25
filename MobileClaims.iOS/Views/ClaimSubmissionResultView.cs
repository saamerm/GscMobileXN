using System;
using CoreGraphics;
using MobileClaims.Core.Converters;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS
{
    [Foundation.Register("ClaimSubmissionResultView1")]
    public class ClaimSubmissionResultView1 : GSCBaseViewPaddingController, IGSCBaseViewImplementor
    {
        ClaimSubmissionResultViewModel _model;

        protected UIScrollView scrollContainer;

        UIBarButtonItem doneButton;
        
        protected UILabel RequiresCOPLabel;
        protected GSButton openCnfirmationOfPaymentButton;

        protected UILabel RequiresAuditLabel;
        protected GSButton openConfirmationOfPaymentButtonForAudit;
            
        protected GSButton submitAnotherButton;

        protected UITableView tableView;

        protected UILabel spouseTitleLabel;
        protected UILabel spouseDescriptionLabel;

        private float BUTTON_WIDTH = Constants.IsPhone() ? 270 : 400;
        private float BUTTON_HEIGHT = Constants.IsPhone() ? 40 : 50;

        private ClaimAuditModal auditInstructionsController;

        public ClaimSubmissionResultView1()
        {
        }

        public override void ViewDidLoad()
        {
            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };

            base.ViewDidLoad();
            base.NavigationItem.SetHidesBackButton(true, false);

            if (Constants.IS_OS_7_OR_LATER())
            {
                base.NavigationController.NavigationBar.TintColor = Colors.HIGHLIGHT_COLOR;
                base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
                base.NavigationController.View.BackgroundColor = Colors.BACKGROUND_COLOR;
            }
            else
            {
                base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
            }

            _model = (ClaimSubmissionResultViewModel)this.ViewModel;

            base.NavigationItem.Title = "claimSubmissionResult".tr();

            scrollContainer = new UIScrollView();
            scrollContainer.BackgroundColor = Colors.Clear;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollContainer);

            if (Constants.IS_OS_7_OR_LATER())
            {
                this.AutomaticallyAdjustsScrollViewInsets = false;
            }
            
            tableView = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Plain);
            tableView.UserInteractionEnabled = false;
            tableView.TableHeaderView = new UIView();
            tableView.SeparatorColor = Colors.Clear;
            tableView.ShowsVerticalScrollIndicator = true;

            submitAnotherButton = new GSButton();
            submitAnotherButton.SetTitle("submitAnotherClaim".tr(), UIControlState.Normal);

            RequiresCOPLabel = new UILabel();
            RequiresCOPLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);
            RequiresCOPLabel.Lines = 0;
            RequiresCOPLabel.LineBreakMode = UILineBreakMode.WordWrap;
            RequiresCOPLabel.TextAlignment = UITextAlignment.Center;
            RequiresCOPLabel.UserInteractionEnabled = false;
            RequiresCOPLabel.TextColor = Colors.DARK_RED;
            RequiresCOPLabel.BackgroundColor = Colors.Clear;

            RequiresAuditLabel = new UILabel();
            RequiresAuditLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);
            RequiresAuditLabel.Lines = 0;
            RequiresAuditLabel.LineBreakMode = UILineBreakMode.WordWrap;
            RequiresAuditLabel.TextAlignment = UITextAlignment.Center;
            RequiresAuditLabel.UserInteractionEnabled = false;
            RequiresAuditLabel.TextColor = Colors.DARK_RED;
            RequiresAuditLabel.BackgroundColor = Colors.Clear;

            openCnfirmationOfPaymentButton = new GSButton();
            openConfirmationOfPaymentButtonForAudit = new GSButton();

            scrollContainer.AddSubview(RequiresCOPLabel);
            scrollContainer.AddSubview(RequiresAuditLabel);
            scrollContainer.AddSubview(submitAnotherButton);
            scrollContainer.AddSubview(openCnfirmationOfPaymentButton);
            scrollContainer.AddSubview(openConfirmationOfPaymentButtonForAudit);

            scrollContainer.AddSubview(tableView);

            if (_model.SecondaryAccountDisabled || _model.SecondaryAccountHasntAcceptedAgreement || _model.SecondaryAccountNotRegistered)
            {

                spouseTitleLabel = new UILabel();
                spouseTitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
                spouseTitleLabel.TextColor = Colors.HIGHLIGHT_COLOR;
                spouseTitleLabel.BackgroundColor = Colors.Clear;
                spouseTitleLabel.TextAlignment = UITextAlignment.Center;
                spouseTitleLabel.Lines = 0;
                spouseTitleLabel.Text = "spouseResults".tr();
                scrollContainer.AddSubview(spouseTitleLabel);

                spouseDescriptionLabel = new UILabel();
                spouseDescriptionLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                spouseDescriptionLabel.TextColor = Colors.DARK_GREY_COLOR;
                spouseDescriptionLabel.BackgroundColor = Colors.Clear;
                spouseDescriptionLabel.TextAlignment = UITextAlignment.Left;
                spouseDescriptionLabel.Lines = 0;
                scrollContainer.AddSubview(spouseDescriptionLabel);

                if (_model.SecondaryAccountDisabled)
                {
                    spouseDescriptionLabel.Text = "secondaryAccountDisabled".FormatWithBrandKeywords(LocalizableBrand.GSC);
                }
                else if (_model.SecondaryAccountHasntAcceptedAgreement)
                {
                    spouseDescriptionLabel.Text = "secondaryAccountHasntAcceptedAgreement".tr();
                }
                else
                {
                    spouseDescriptionLabel.Text = "secondaryAccountNotRegistered".FormatWithBrandKeywords(LocalizableBrand.GSC);
                }

            }

            MvxResultsTableViewSource tableSource = new MvxResultsTableViewSource(_model, tableView, "ClaimResultsTableCell", typeof(ClaimResultsTableCell));
            tableView.Source = tableSource;

            var set = this.CreateBindingSet<ClaimSubmissionResultView1, ClaimSubmissionResultViewModel>();
            set.Bind(tableSource).To(vm => vm.Claim.Results);
            set.Bind(this).For(v => v.IsSelectedForAudit).To(vm => vm.IsSelectedForAudit);
            set.Bind(this.submitAnotherButton).To(vm => vm.SubmitAnotherClaimCommand);

            BoolOppositeValueConverter boolOppositeValueConverter = new BoolOppositeValueConverter();
            set.Bind(RequiresCOPLabel).For(x => x.Hidden).To(vm => vm.ShowUploadDocuments).WithConversion(boolOppositeValueConverter, null);
            set.Bind(RequiresCOPLabel).To(vm => vm.ShowUploadDocuments).WithConversion("RequiresCopToInfo");

            set.Bind(RequiresAuditLabel).For(x => x.Hidden).To(vm => vm.IsSelectedForAudit).WithConversion(boolOppositeValueConverter, null);
            set.Bind(RequiresAuditLabel).To(vm => vm.RequiredAuditLabel);

            set.Bind(openCnfirmationOfPaymentButton).For("Title").To(vm => vm.UploadDocuments);
            set.Bind(openCnfirmationOfPaymentButton).For(x => x.Hidden).To(vm => vm.ShowUploadDocuments).WithConversion(boolOppositeValueConverter, null);
            set.Bind(openCnfirmationOfPaymentButton).To(vm => vm.OpenConfirmationOfPaymentCommand);

            set.Bind(openConfirmationOfPaymentButtonForAudit).For("Title").To(vm => vm.UploadDocuments);
            set.Bind(openConfirmationOfPaymentButtonForAudit).For(x => x.Hidden).To(vm => vm.IsSelectedForAudit).WithConversion(boolOppositeValueConverter, null);
            set.Bind(openConfirmationOfPaymentButtonForAudit).To(vm => vm.OpenConfirmationOfPaymentCommand);

            set.Apply();

            tableView.ReloadData();
        }

        void HandleAuditInstructions(object sender, EventArgs e)
        {
            float auditControllerYPos = (Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING);

            auditInstructionsController = new ClaimAuditModal();

            auditInstructionsController.View = new GSCBaseView(); // new CGRect(0, 0, 5, 5));
            auditInstructionsController.View.BackgroundColor = Colors.BACKGROUND_COLOR;

            auditInstructionsController.scrollContainer = new UIScrollView();
            auditInstructionsController.scrollContainer.BackgroundColor = Colors.Clear;
            auditInstructionsController.View.AddSubview(auditInstructionsController.scrollContainer);

            auditInstructionsController.auditInstructionsTextArea = new UILabel();
            auditInstructionsController.auditInstructionsTextArea.BackgroundColor = Colors.Clear;
            auditInstructionsController.auditInstructionsTextArea.TextColor = Colors.DARK_GREY_COLOR;
            auditInstructionsController.auditInstructionsTextArea.BackgroundColor = Colors.Clear;
            auditInstructionsController.auditInstructionsTextArea.TextAlignment = UITextAlignment.Left;
            auditInstructionsController.auditInstructionsTextArea.LineBreakMode = UILineBreakMode.WordWrap;
            auditInstructionsController.auditInstructionsTextArea.Lines = 0;
            auditInstructionsController.auditInstructionsTextArea.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            auditInstructionsController.auditInstructionsTextArea.Text = "auditInstructions".FormatWithBrandKeywords(LocalizableBrand.GSC);
            auditInstructionsController.scrollContainer.AddSubview(auditInstructionsController.auditInstructionsTextArea);

            auditInstructionsController.dismissButton = new GSButton();
            auditInstructionsController.dismissButton.SetTitle("ok".tr(), UIControlState.Normal);
            auditInstructionsController.scrollContainer.AddSubview(auditInstructionsController.dismissButton);
            auditInstructionsController.dismissButton.TouchUpInside += HandleDismissButton;

            UIWindow keyWindow = UIApplication.SharedApplication.KeyWindow;

            auditInstructionsController.ModalInPopover = true;
            auditInstructionsController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            keyWindow.RootViewController.PresentViewController((UIViewController)auditInstructionsController, true, (Action)null);
        }

        void HandleDismissButton(object sender, EventArgs e)
        {
            auditInstructionsController.DismissViewController(true, (Action)null);
        }

        void HandleDone(object sender, EventArgs e)
        {
            _model.SubmitAnotherClaimCommand.Execute(null);
        }

        int redrawCount = 0;

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            float contentWidth = (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            float tableWidth = contentWidth;
            float itemPadding = Constants.CLAIMS_DETAILS_COMPONENT_PADDING;

            float yPos = ViewContentYPositionPadding;
            float auditYPos = itemPadding;
            float auditInfoYPos = itemPadding;

            float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;

            if (_model.ShowUploadDocuments)
            {
                var requiresCOPLabelWidth = ViewContainerWidth - 40;
                RequiresCOPLabel.SizeToFit();
                RequiresCOPLabel.Frame = new CGRect(ViewContainerWidth / 2 - requiresCOPLabelWidth / 2, yPos, requiresCOPLabelWidth, RequiresCOPLabel.Frame.Height);
                yPos += (float)(RequiresCOPLabel.Frame.Height + 5);

                openCnfirmationOfPaymentButton.Frame = new CGRect(ViewContainerWidth / 2 - BUTTON_WIDTH / 2, yPos, BUTTON_WIDTH, BUTTON_HEIGHT);
                yPos += (float)(openCnfirmationOfPaymentButton.Frame.Height + (itemPadding * 2));
            }

            int minusValue = 100;
            string lang = System.Globalization.CultureInfo.CurrentCulture.Name.ToString();

            if (Constants.IsPhone())
            {
                if (_model.Claim.Results != null && _model.Claim.Results.Count == 1)
                {
                    minusValue = 0;
                }
                if (_model.Claim.Results != null && _model.Claim.Results.Count == 2)
                {
                    if (lang.Contains("fr") || lang.Contains("Fr"))
                    {
                        if (Helpers.IsInPortraitMode())
                        {
                            minusValue = 100;
                        }
                    }
                    else
                    {
                        if (Helpers.IsInLandscapeMode())
                        {
                            minusValue = 70;
                        }
                    }
                }
                if (_model.Claim.Results != null && _model.Claim.Results.Count == 3)
                {
                    if (lang.Contains("fr") || lang.Contains("Fr"))
                    {
                        if (Helpers.IsInPortraitMode())
                        {
                            minusValue = 120;
                        }
                    }
                    else
                    {
                        if (Helpers.IsInLandscapeMode())
                        {
                            minusValue = 90;
                        }
                    }
                }
            }
            else
            {
                minusValue = 0;
            }

            tableView.Frame = new CGRect(0, yPos, tableWidth, (nfloat)tableView.ContentSize.Height + 50);
            yPos += (float)tableView.Frame.Height + itemPadding;
       
            if (spouseTitleLabel != null && spouseDescriptionLabel != null)
            {
                spouseTitleLabel.Frame = new CGRect(ViewContainerWidth / 2 - (float)spouseTitleLabel.Frame.Width / 2, yPos, ViewContainerWidth - sidePadding * 2, (float)spouseTitleLabel.Frame.Height);
                spouseTitleLabel.SizeToFit();
                spouseTitleLabel.Frame = new CGRect(ViewContainerWidth / 2 - (float)spouseTitleLabel.Frame.Width / 2, yPos, (float)spouseTitleLabel.Frame.Width, (float)spouseTitleLabel.Frame.Height);
                yPos += (float)spouseTitleLabel.Frame.Height + itemPadding;

                spouseDescriptionLabel.Frame = new CGRect(sidePadding, yPos, ViewContainerWidth - sidePadding * 2, (float)spouseDescriptionLabel.Frame.Height);
                spouseDescriptionLabel.SizeToFit();
                yPos += (float)spouseDescriptionLabel.Frame.Height + itemPadding;
            }
                       
            if (IsSelectedForAudit)
            {
                var requiresAuditLabelWidth = ViewContainerWidth - 40;
                RequiresAuditLabel.SizeToFit();
                RequiresAuditLabel.Frame = new CGRect(ViewContainerWidth / 2 - requiresAuditLabelWidth / 2, yPos, requiresAuditLabelWidth, RequiresAuditLabel.Frame.Height);
                yPos += (float)(RequiresAuditLabel.Frame.Height + 5);

                openConfirmationOfPaymentButtonForAudit.Frame = new CGRect(ViewContainerWidth / 2 - BUTTON_WIDTH / 2, yPos, BUTTON_WIDTH, BUTTON_HEIGHT);
                yPos += (float)(openConfirmationOfPaymentButtonForAudit.Frame.Height + (itemPadding * 2));
            }

            submitAnotherButton.Frame = new CGRect(ViewContainerWidth / 2 - BUTTON_WIDTH / 2, yPos, BUTTON_WIDTH, BUTTON_HEIGHT);
            yPos += (float)(submitAnotherButton.Frame.Height + (itemPadding * 2));

            yPos += (Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING);

            scrollContainer.Frame = new CGRect(0, 0, ViewContainerWidth, ViewContainerHeight);           
            scrollContainer.ContentSize = new CGSize(ViewContainerWidth, yPos);

            if (redrawCount < 3)
            {
                redrawCount++;
                this.View.SetNeedsLayout();
            }
        }

        private bool _isSelectedForAudit;
        public bool IsSelectedForAudit
        {
            get
            {
                return _isSelectedForAudit;
            }
            set
            {
                _isSelectedForAudit = value;

                this.View.SetNeedsLayout();
            }
        }

        private bool _isPlanLimitationVisible;
        public bool IsPlanLimitationVisible
        {
            get
            {
                return _isPlanLimitationVisible;
            }
            set
            {
                _isPlanLimitationVisible = value;


            }
        }

        private string htmlFormatString(string content)
        {
            return "<html> \n <head> \n <style type=\"text/css\"> \n body {font-family: \"" + Constants.HTML_FONT + "\"; font-size:" + Constants.TERMS_TEXT_FONT_SIZE + "; padding-left:" + Constants.TERMS_SIDE_PADDING + "px; padding-right:" + Constants.TERMS_SIDE_PADDING + "px; }\n </style> \n </head> \n <body>" + content + "</body> \n </html>";

        }

        public float GetViewContainerWidth()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            }
            else
            {
                return (float)base.View.Frame.Width;
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
                return (float)base.View.Frame.Height - Helpers.BottomNavHeight();
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

    }
}

