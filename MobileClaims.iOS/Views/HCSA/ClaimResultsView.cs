using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Converters;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Entities.HCSA;
using MobileClaims.Core.ViewModels.HCSA;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS
{
    public class ClaimResultsView : GSCBaseViewController
    {
        public UIView planInfoView;
        public UILabel lblPlanInformation;
        public UILabel lblGreenShieldId;
        public LabelNunitoSemiBold13_NunitoBold12 lblGreenShieldIdValue;
        public UILabel lblParticipentName;
        public LabelNunitoSemiBold13_NunitoBold12 lblParticipentNameValue;
        public UILabel lblProfessionalName;
        public UILabel lblProfessionalNameValue;
        public UILabel lblSubmissionDate;
        public LabelNunitoSemiBold13_NunitoBold12 lblSubmissionDateValue;

        public UILabel lblClaimDetails;
        public UITableView claimsTableView;

        public UILabel lblTotal;
        public UIView totalView;
        //for border
        public UILabel lblClaimAmount;
        public UILabel lblClaimAmountValue;

        public UILabel lblOtherPaidAmount;
        public UILabel lblOtherPaidAmountValue;

        public UILabel lblPaidAmount;
        public UILabel lblPaidAmountValue;

        public UILabel lblDescription;
        public UIView descriptionBackgroundView;
        public UILabel RequiresCOPLabel;
        public UILabel RequiresAuditLabel;

        int referralTypeVisibleNo;

        bool isiPadlandscape;
        public GSButton btnSubmitClaim;
        public GSButton OpenDocumentUploadButton;
        public GSButton OpenDocumentUploadButtonForAudit;
        public UIScrollView scrollableContainer;
        public CGRect tableFrame;
        private float topAdjustment = 4f;

        ClaimResultsViewModel _model;

        public ClaimResultsView()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _model = (ClaimResultsViewModel)this.ViewModel;

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = _model.TitleLabel.ToUpper();

            base.NavigationItem.SetHidesBackButton(true, false);

            View = new UIView()
            {
                BackgroundColor = Colors.BACKGROUND_COLOR
            };

            this.AutomaticallyAdjustsScrollViewInsets = false;

            scrollableContainer = new UIScrollView();
            scrollableContainer.ScrollEnabled = true;
            View.AddSubview(scrollableContainer);

            planInfoView = new UIView();
         
            isiPadlandscape = Helpers.IsInLandscapeMode();
            ClaimResultGSC result = null;
            if (_model.Claim.Results != null && _model.Claim.Results.Count > 0)
            {
                ObservableCollection<ClaimResultGSC> _result = _model.Claim.Results;
                result = _result[0];
            }

            lblPlanInformation = new UILabel();
            lblPlanInformation.BackgroundColor = Colors.Clear;
            lblPlanInformation.TextColor = Colors.DARK_GREY_COLOR;
            lblPlanInformation.TextAlignment = UITextAlignment.Left;
            lblPlanInformation.Lines = 1;
            lblPlanInformation.LineBreakMode = UILineBreakMode.WordWrap;
            lblPlanInformation.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, 24);

            lblGreenShieldId = new UILabel();
            lblGreenShieldId.BackgroundColor = Colors.Clear;
            lblGreenShieldId.TextColor = Colors.DARK_GREY_COLOR;
            lblGreenShieldId.TextAlignment = UITextAlignment.Left;
            lblGreenShieldId.Lines = 2;
            lblGreenShieldId.LineBreakMode = UILineBreakMode.WordWrap;
            lblGreenShieldId.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);

            lblGreenShieldIdValue = new LabelNunitoSemiBold13_NunitoBold12();

            lblParticipentName = new UILabel();
            lblParticipentName.BackgroundColor = Colors.Clear;
            lblParticipentName.TextColor = Colors.DARK_GREY_COLOR;
            lblParticipentName.TextAlignment = UITextAlignment.Left;
            lblParticipentName.Lines = 1;
            lblParticipentName.LineBreakMode = UILineBreakMode.WordWrap;
            lblParticipentName.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);

            lblParticipentNameValue = new LabelNunitoSemiBold13_NunitoBold12();

            lblSubmissionDate = new UILabel();
            lblSubmissionDate.BackgroundColor = Colors.Clear;
            lblSubmissionDate.TextColor = Colors.DARK_GREY_COLOR;
            lblSubmissionDate.TextAlignment = UITextAlignment.Left;
            lblSubmissionDate.Lines = 0;
            lblSubmissionDate.LineBreakMode = UILineBreakMode.WordWrap;
            lblSubmissionDate.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);

            lblSubmissionDateValue = new LabelNunitoSemiBold13_NunitoBold12();

            lblClaimDetails = new UILabel();
            lblClaimDetails.BackgroundColor = Colors.Clear;
            lblClaimDetails.TextColor = Colors.DARK_GREY_COLOR;
            lblClaimDetails.TextAlignment = UITextAlignment.Left;
            lblClaimDetails.Lines = 1;
            lblClaimDetails.LineBreakMode = UILineBreakMode.WordWrap;
            lblClaimDetails.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, 24);

            claimsTableView = new UITableView();
            claimsTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            lblTotal = new UILabel();
            lblTotal.BackgroundColor = Colors.Clear;
            lblTotal.TextColor = Colors.DARK_GREY_COLOR;
            lblTotal.TextAlignment = UITextAlignment.Left;
            lblTotal.Lines = 1;
            lblTotal.LineBreakMode = UILineBreakMode.WordWrap;
            lblTotal.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, 24);

            totalView = new UIView();
            totalView.Layer.BorderColor = Colors.DARK_GREY_COLOR.CGColor;
            totalView.Layer.BorderWidth = 2.0f;

            lblClaimAmount = new UILabel();
            lblClaimAmount.TextColor = Colors.DARK_GREY_COLOR;
            lblClaimAmount.TextAlignment = UITextAlignment.Left;
            lblClaimAmount.Lines = 2;
            lblClaimAmount.LineBreakMode = UILineBreakMode.WordWrap;
            lblClaimAmount.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);

            lblClaimAmountValue = new UILabel();
            lblClaimAmountValue.Text = "$" + result.ClaimedAmountTotal;

            lblClaimAmountValue.BackgroundColor = Colors.Clear;
            if (Constants.IsPhone())
            {
                lblClaimAmountValue.TextColor = Colors.DarkGrayColor;

            }
            else
            {
                lblClaimAmountValue.TextColor = Colors.Black;

            }
            lblClaimAmountValue.TextAlignment = UITextAlignment.Left;

            lblClaimAmountValue.Lines = 1;
            lblClaimAmountValue.LineBreakMode = UILineBreakMode.WordWrap;
            lblClaimAmountValue.Font = UIFont.FromName(Constants.NUNITO_BOLD, 12);
            
            lblPaidAmount = new UILabel();
            lblPaidAmount.BackgroundColor = Colors.Clear;
            lblPaidAmount.TextColor = Colors.DARK_GREY_COLOR;
            lblPaidAmount.TextAlignment = UITextAlignment.Left;
            lblPaidAmount.Lines = 0;
            lblPaidAmount.LineBreakMode = UILineBreakMode.WordWrap;
            lblPaidAmount.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);

            lblPaidAmountValue = new UILabel();
            lblPaidAmountValue.Text = "$" + result.OtherPaidAmountTotal;//changed

            lblPaidAmountValue.BackgroundColor = Colors.Clear;

            if (Constants.IsPhone())
            {
                lblPaidAmountValue.TextColor = Colors.DarkGrayColor;
            }
            else
            {
                lblPaidAmountValue.TextColor = Colors.Black;
            }

            lblPaidAmountValue.TextAlignment = UITextAlignment.Left;
            lblPaidAmountValue.Lines = 1;
            lblPaidAmountValue.LineBreakMode = UILineBreakMode.WordWrap;
            lblPaidAmountValue.Font = UIFont.FromName(Constants.NUNITO_BOLD, 12);

            lblOtherPaidAmount = new UILabel();
            lblOtherPaidAmount.BackgroundColor = Colors.Clear;
            lblOtherPaidAmount.TextColor = Colors.DARK_GREY_COLOR;
            lblOtherPaidAmount.TextAlignment = UITextAlignment.Left;
            lblOtherPaidAmount.Lines = 0;
            lblOtherPaidAmount.LineBreakMode = UILineBreakMode.WordWrap;
            lblOtherPaidAmount.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);

            lblOtherPaidAmountValue = new UILabel();
            lblOtherPaidAmountValue.Text = "$" + result.PaidAmountTotal;

            lblOtherPaidAmountValue.BackgroundColor = Colors.Clear;

            if (Constants.IsPhone())
            {
                lblOtherPaidAmountValue.TextColor = Colors.DarkGrayColor;
            }
            else
            {
                lblOtherPaidAmountValue.TextColor = Colors.Black;
            }

            lblOtherPaidAmountValue.TextAlignment = UITextAlignment.Left;
            lblOtherPaidAmountValue.Lines = 1;
            lblOtherPaidAmountValue.LineBreakMode = UILineBreakMode.WordWrap;
            lblOtherPaidAmountValue.Font = UIFont.FromName(Constants.NUNITO_BOLD, 12);

            descriptionBackgroundView = new UIView();

            lblDescription = new UILabel();
            lblDescription.Text = _model.DescriptionLabel;
            lblDescription.BackgroundColor = Colors.Clear;
            lblDescription.TextColor = Colors.DarkGrayColor;
            lblDescription.Lines = 0;
            lblDescription.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 12);

            btnSubmitClaim = new GSButton();
            btnSubmitClaim.TitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.GREEN_BUTTON_FONT_SIZE);
            btnSubmitClaim.SetTitle("submitAnotherClaim".tr(), UIControlState.Normal);

            RequiresCOPLabel = new UILabel
            {
                Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE),
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
                TextAlignment = UITextAlignment.Center,
                UserInteractionEnabled = false,
                TextColor = Colors.DARK_RED,
                BackgroundColor = Colors.Clear
            };

            RequiresAuditLabel = new UILabel
            {
                Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE),
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
                TextAlignment = UITextAlignment.Center,
                UserInteractionEnabled = false,
                TextColor = Colors.DARK_RED,
                BackgroundColor = Colors.Clear
            };

            OpenDocumentUploadButton = new GSButton();
            OpenDocumentUploadButton.TitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.GREEN_BUTTON_FONT_SIZE);

            OpenDocumentUploadButtonForAudit = new GSButton();
            OpenDocumentUploadButtonForAudit.TitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.GREEN_BUTTON_FONT_SIZE);

            var source = new ClaimsListResultsTableViewSource(_model, claimsTableView);
            claimsTableView.ScrollEnabled = false;
            claimsTableView.Source = source;

            scrollableContainer.AddSubview(RequiresCOPLabel);
            scrollableContainer.AddSubview(OpenDocumentUploadButton);
            scrollableContainer.AddSubview(lblPlanInformation);
            scrollableContainer.AddSubview(lblGreenShieldId);
            scrollableContainer.AddSubview(lblGreenShieldIdValue);
            scrollableContainer.AddSubview(lblParticipentName);
            scrollableContainer.AddSubview(lblParticipentNameValue);
            scrollableContainer.AddSubview(lblSubmissionDate);
            scrollableContainer.AddSubview(lblSubmissionDateValue);
            scrollableContainer.AddSubview(lblClaimDetails);
            scrollableContainer.AddSubview(claimsTableView);

            scrollableContainer.AddSubview(lblTotal);
            scrollableContainer.AddSubview(totalView);

            totalView.AddSubview(lblClaimAmount);
            totalView.AddSubview(lblClaimAmountValue);
            totalView.AddSubview(lblPaidAmount);
            totalView.AddSubview(lblPaidAmountValue);
            totalView.AddSubview(lblOtherPaidAmount);
            totalView.AddSubview(lblOtherPaidAmountValue);

            descriptionBackgroundView.AddSubview(lblDescription);
            scrollableContainer.AddSubview(descriptionBackgroundView);

            scrollableContainer.AddSubview(RequiresAuditLabel);
            scrollableContainer.AddSubview(OpenDocumentUploadButtonForAudit);

            scrollableContainer.AddSubview(btnSubmitClaim);

            if (_model.IsReferrerVisible)
                referralTypeVisibleNo = 1;
            else
                referralTypeVisibleNo = 0;

            _model.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
            {

                if (e.PropertyName == "IsReferrerVisible")
                {
                    if (_model.IsReferrerVisible)
                    {
                        referralTypeVisibleNo = 1;// button under referral type
                    }
                    else
                    {
                        referralTypeVisibleNo = 0;// button under description

                    }
                }
                SetConstraints();
            };

            SetConstraints();
            var set = this.CreateBindingSet<ClaimResultsView, ClaimResultsViewModel>();
            set.Bind(source).To(vm => vm.Claim.Details);

            set.Bind(this.lblPlanInformation).To(vm => vm.PlanInfoLabel).WithConversion("StringCase");
            set.Bind(this.lblGreenShieldId).To(vm => vm.IDNumberLabel);
            set.Bind(lblGreenShieldIdValue).For(l => l.TextContent).To(vm => vm.Claim.Results[0].PlanMemberDisplayID);
            set.Bind(this.lblParticipentName).To(vm => vm.ParticipantLabel);
            set.Bind(lblParticipentNameValue).For(l => l.TextContent).To(vm => vm.Claim.Results[0].ParticipantFullName);
            set.Bind(this.lblSubmissionDate).To(vm => vm.SubmissionDateLabel);
            set.Bind(lblSubmissionDateValue).For(l => l.TextContent).To(vm => vm.Claim.Results[0].SubmissionDate).WithConversion("DateToString");
            set.Bind(this.lblClaimDetails).To(vm => vm.DetailsLabel).WithConversion("StringCase");
            set.Bind(this.lblTotal).To(vm => vm.TotalLabel).WithConversion("StringCase");
            set.Bind(this.lblClaimAmount).To(vm => vm.ClaimedAmountLabel);
            set.Bind(this.lblPaidAmount).To(vm => vm.OtherPaidAmountLabel);
            set.Bind(this.lblOtherPaidAmount).To(vm => vm.PaidAmountLabel);
            set.Bind(this.btnSubmitClaim).To(vm => vm.SubmitAnotherClaimCommand);

            BoolOppositeValueConverter boolOppositeValueConverter = new BoolOppositeValueConverter();
            set.Bind(RequiresCOPLabel).For(x => x.Hidden).To(vm => vm.ShowUploadDocuments).WithConversion(boolOppositeValueConverter, null);
            set.Bind(RequiresCOPLabel).To(vm => vm.ShowUploadDocuments).WithConversion("RequiresCopToInfo");

            set.Bind(RequiresAuditLabel).For(x => x.Hidden).To(vm => vm.IsSelectedForAudit).WithConversion(boolOppositeValueConverter, null);
            set.Bind(RequiresAuditLabel).To(vm => vm.RequiredAuditLabel);

            set.Bind(OpenDocumentUploadButton).To(vm => vm.OpenConfirmationOfPaymentCommand);
            set.Bind(OpenDocumentUploadButton).For("Title").To(vm => vm.UploadDocuments);
            set.Bind(OpenDocumentUploadButton).For(b => b.Hidden).To(vm => vm.ShowUploadDocuments).WithConversion(boolOppositeValueConverter, null);

            set.Bind(OpenDocumentUploadButtonForAudit).To(vm => vm.OpenConfirmationOfPaymentCommand);
            set.Bind(OpenDocumentUploadButtonForAudit).For("Title").To(vm => vm.UploadDocuments);
            set.Bind(OpenDocumentUploadButtonForAudit).For(b => b.Hidden).To(vm => vm.IsSelectedForAudit).WithConversion(boolOppositeValueConverter, null);

            set.Bind(ClaimResults).To(vm => vm.Claim.Results);

            set.Bind(this.btnSubmitClaim).For("Title").To(vm => vm.SubmitButtonLabel).WithConversion("StringCase");
            set.Apply();
        }


        private ObservableCollection<ClaimResultGSC> _results;

        public ObservableCollection<ClaimResultGSC> ClaimResults
        {
            get
            {
                return _results;
            }
            set
            {
                _results = value;

                if (_results != null && _results.Count > 0)
                {
                    ClaimResultGSC result = _results[0];
                    lblPaidAmountValue.Text = "$" + result.OtherPaidAmountTotal;
                    lblOtherPaidAmountValue.Text = "$" + result.PaidAmountTotal;
                    lblClaimAmountValue.Text = "$" + result.ClaimedAmountTotal;
                    lblSubmissionDateValue.Text = result.SubmissionDate.ToString("d");//WithConversion("FormattedStringFromDate");
                    lblParticipentNameValue.Text = result.ParticipantFullName;
                    lblGreenShieldIdValue.Text = result.PlanMemberDisplayID;

                }
                else
                {
                    lblPaidAmountValue.Text = "";
                    //lblProfessionalNameValue.Text = "";
                    lblOtherPaidAmountValue.Text = "";
                    lblClaimAmountValue.Text = "";
                    lblSubmissionDateValue.Text = "";
                    lblParticipentNameValue.Text = "";
                    lblGreenShieldIdValue.Text = "";

                }
            }
        }

        public class ClaimsListResultsTableViewSource : MvxTableViewSource
        {

            private ClaimResultsViewModel _model;

            public ClaimsListResultsTableViewSource(ClaimResultsViewModel _model, UITableView tableView) : base(tableView)
            {
                this._model = _model;
                tableView.RegisterClassForCellReuse(typeof(ResultsDetailsTableviewCell), new NSString("ResultsDetailsTableviewCell"));
            }

            protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
            {
                return (UITableViewCell)tableView.DequeueReusableCell("ResultsDetailsTableviewCell");
            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                nfloat result = Constants.RESULTSDETAILSTABLEVIEWCELLHEIGHT;
                int eoBContentLines = 0;
                int linesOfExpenseType = 0;
                List<ClaimEOBMessageGSC> Messages = _model.Claim.Details[indexPath.Row].EOBMessages;
                if (Messages != null)
                {
                    int countMessages = Messages.Count;
                    if (countMessages > 0)
                    {
                        int linesForeachMessage = 0;
                        for (int i = 0; i < countMessages; i++)
                        {
                            string MessageStr = Messages[i].Message.ToString();
                            if (!string.IsNullOrEmpty(MessageStr))
                            {
                                int longOfStr = MessageStr.Length;
                                if (longOfStr > 0)
                                {
                                    int messageLong = Helpers.IsInPortraitMode() ? Constants.EOBMESSAGELIMATELENGTH_PORTRAIT : Constants.EOBMESSAGELIMATELENGTH_LANDSCAPE;
                                    linesForeachMessage = (int)(longOfStr / messageLong);
                                }
                            }
                            eoBContentLines += linesForeachMessage;
                        }

                    }
                }
                result += eoBContentLines * Constants.EOBMESSAGE_LINEHEIGHT;

                if (_model.ExpenseType != null)
                {
                    linesOfExpenseType = 0;
                    if (!string.IsNullOrEmpty(_model.ExpenseType))
                    {
                        int longOfStr = _model.ExpenseType.Length;
                        if (longOfStr > 0)
                        {
                            int messageLong = Helpers.IsInPortraitMode() ? Constants.EOBMESSAGELIMATELENGTH_PORTRAIT : Constants.EOBMESSAGELIMATELENGTH_LANDSCAPE;
                            linesOfExpenseType = (int)(longOfStr / messageLong);
                        }
                    }
                }
                result += linesOfExpenseType * Constants.EOBMESSAGE_LINEHEIGHT;
                return (nfloat)result;
            }

        }

        public override void WillAnimateRotation(UIInterfaceOrientation toInterfaceOrientation, double duration)
        {
            base.WillAnimateRotation(toInterfaceOrientation, duration);
            switch (toInterfaceOrientation)
            {

                case UIInterfaceOrientation.LandscapeLeft:
                    isiPadlandscape = true;
                    SetConstraints();

                    break;

                case UIInterfaceOrientation.LandscapeRight:
                    isiPadlandscape = true;
                    SetConstraints();
                    break;

                case UIInterfaceOrientation.Portrait:
                    isiPadlandscape = false;
                    SetConstraints();
                    break;

                case UIInterfaceOrientation.PortraitUpsideDown:
                    isiPadlandscape = false;
                    SetConstraints();
                    break;
            }
        }

        private void SetConstraints()
        {
            lblDescription.BackgroundColor = Colors.LightGrayColor;
            descriptionBackgroundView.BackgroundColor = Colors.LightGrayColor;

            tableFrame = claimsTableView.Frame;
            nfloat result = 0;
            int eoBContentLines = 0;
            int linesOfExpenseType = 0;
            if (_model.Claim != null && _model.Claim.Details != null && _model.Claim.Details.Count > 0)
            {
                ObservableCollection<ClaimDetail> crg = _model.Claim.Details as ObservableCollection<ClaimDetail>;

                if (crg != null && crg.Count > 0)
                {
                    for (int i = 0; i < crg.Count; i++)
                    {

                        List<ClaimEOBMessageGSC> Messages = crg[i].EOBMessages;
                        if (Messages != null)
                        {
                            int countMessages = Messages.Count;
                            if (countMessages > 0)
                            {
                                int linesForeachMessage = 0;
                                for (int j = 0; j < countMessages; j++)
                                {
                                    string MessageStr = Messages[j].Message.ToString();
                                    if (!string.IsNullOrEmpty(MessageStr))
                                    {
                                        int longOfStr = MessageStr.Length;
                                        if (longOfStr > 0)
                                        {
                                            int messageLong = Helpers.IsInPortraitMode() ? Constants.EOBMESSAGELIMATELENGTH_PORTRAIT : Constants.EOBMESSAGELIMATELENGTH_LANDSCAPE;
                                            linesForeachMessage = (int)(longOfStr / messageLong);
                                        }
                                    }
                                    eoBContentLines += linesForeachMessage;
                                }
                            }
                        }

                    }
                    result = eoBContentLines * Constants.EOBMESSAGE_LINEHEIGHT;
                }
            }

            if (_model.ExpenseType != null)
            {
                linesOfExpenseType = 0;
                if (!string.IsNullOrEmpty(_model.ExpenseType))
                {
                    int longOfStr = _model.ExpenseType.Length;
                    if (longOfStr > 0)
                    {
                        int messageLong = Helpers.IsInPortraitMode() ? Constants.EOBMESSAGELIMATELENGTH_PORTRAIT : Constants.EOBMESSAGELIMATELENGTH_LANDSCAPE;
                        linesOfExpenseType = (int)(longOfStr / messageLong);
                    }
                }
            }
            tableFrame.Height = (nfloat)(Constants.RESULTSDETAILSTABLEVIEWCELLHEIGHT * _model.Claim.Details.Count + result + linesOfExpenseType * Constants.EOBMESSAGE_LINEHEIGHT * _model.Claim.Details.Count);
            claimsTableView.Frame = tableFrame;

            View.RemoveConstraints(View.Constraints);
            scrollableContainer.RemoveConstraints(scrollableContainer.Constraints);
            totalView.RemoveConstraints(totalView.Constraints);
            descriptionBackgroundView.RemoveConstraints(descriptionBackgroundView.Constraints);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            scrollableContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            totalView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            descriptionBackgroundView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            float requiresAuditLabelTopPadding = _model.IsSelectedForAudit ? 25 : 0;
            float uploadButtonHeight = _model.IsSelectedForAudit ? Constants.GREEN_BUTTON_HEIGHT : 0;
            float uploadButtonTopPadding = _model.IsSelectedForAudit ? 5 : 0;
            if (Constants.IsPhone())
            {
                float yOffset = Constants.TOP_PADDING;
                if (Helpers.IsInLandscapeMode())
                    yOffset = 50;

                if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
                {
                    View.AddConstraints(
                        scrollableContainer.AtTopOf(View, View.SafeAreaInsets.Top + 10),
                        scrollableContainer.AtLeftOf(View, View.SafeAreaInsets.Left),
                        scrollableContainer.AtRightOf(View, View.SafeAreaInsets.Right),
                        scrollableContainer.WithSameWidth(View).Minus(View.SafeAreaInsets.Left * 2),
                        scrollableContainer.AtBottomOf(View, View.SafeAreaInsets.Bottom),

                        RequiresCOPLabel.AtTopOf(scrollableContainer),
                        RequiresCOPLabel.WithSameCenterX(scrollableContainer),
                        RequiresCOPLabel.AtLeftOf(scrollableContainer, 20),
                        RequiresCOPLabel.AtRightOf(scrollableContainer, 20),

                        OpenDocumentUploadButton.Below(RequiresCOPLabel, 5),
                        OpenDocumentUploadButton.WithSameCenterX(scrollableContainer),
                        OpenDocumentUploadButton.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                        OpenDocumentUploadButton.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),

                        lblPlanInformation.Below(OpenDocumentUploadButton, 25),
                        lblPlanInformation.AtLeftOf(scrollableContainer, 20),
                        lblPlanInformation.AtRightOf(View, 20),
                        lblPlanInformation.Height().EqualTo(28),

                        lblGreenShieldId.Below(lblPlanInformation, 12),
                        lblGreenShieldId.AtLeftOf(scrollableContainer, 20),
                        lblGreenShieldId.WithRelativeWidth(View, 0.5f).Minus(15),

                        lblGreenShieldIdValue.WithSameTop(lblGreenShieldId).Plus(topAdjustment),
                        lblGreenShieldIdValue.ToRightOf(lblGreenShieldId, 10),
                        lblGreenShieldIdValue.AtRightOf(View, 20),

                        lblParticipentName.Below(lblGreenShieldId, 9),
                        lblParticipentName.AtLeftOf(scrollableContainer, 20),
                        lblParticipentName.WithRelativeWidth(View, 0.5f).Minus(15),
                        lblParticipentName.WithSameHeight(lblParticipentNameValue),

                        lblParticipentNameValue.WithSameTop(lblParticipentName).Plus(topAdjustment),
                        lblParticipentNameValue.ToRightOf(lblParticipentName, 10),
                        lblParticipentNameValue.AtRightOf(View, 20),

                        lblSubmissionDate.Below(lblParticipentName, 9),
                        lblSubmissionDate.AtLeftOf(scrollableContainer, 20),
                        lblSubmissionDate.WithRelativeWidth(scrollableContainer, 0.5f).Minus(50),

                        lblSubmissionDateValue.WithSameTop(lblSubmissionDate).Plus(topAdjustment),
                        lblSubmissionDateValue.WithSameLeft(lblParticipentNameValue),
                        lblSubmissionDateValue.WithSameWidth(lblSubmissionDate)
                    );

                    View.AddConstraints(
                        lblClaimDetails.Below(lblSubmissionDate, 26),//need change line here if referred show
                        lblClaimDetails.Height().EqualTo(28),
                        lblClaimDetails.AtLeftOf(scrollableContainer, 20),
                        lblClaimDetails.AtRightOf(View, 20),

                        claimsTableView.Below(lblClaimDetails, 12),
                        claimsTableView.AtLeftOf(scrollableContainer, 20),
                        claimsTableView.AtRightOf(scrollableContainer, 20),
                        claimsTableView.Height().EqualTo(tableFrame.Height),// need to change

                        lblTotal.Below(claimsTableView, 12),
                        lblTotal.Height().EqualTo(28),
                        lblTotal.AtLeftOf(scrollableContainer, 20),
                        lblTotal.AtRightOf(View, 20),

                        totalView.Below(lblTotal, 10),
                        totalView.AtLeftOf(scrollableContainer, 20),
                        totalView.AtRightOf(scrollableContainer, 20),

                        lblClaimAmount.AtTopOf(totalView, 9),
                        lblClaimAmount.AtLeftOf(totalView, 10),
                        lblClaimAmount.WithRelativeWidth(scrollableContainer, 0.5f).Minus(25),

                        lblClaimAmountValue.AtTopOf(totalView, 9),
                        lblClaimAmountValue.ToRightOf(lblClaimAmount, 8),
                        lblClaimAmountValue.AtRightOf(totalView, 5),
                        lblClaimAmountValue.WithSameHeight(lblClaimAmount),
                        lblClaimAmountValue.WithSameWidth(lblClaimAmount).Minus(20),

                        lblPaidAmount.Below(lblClaimAmount, 9),
                        lblPaidAmount.AtLeftOf(totalView, 10),
                        lblPaidAmount.WithRelativeWidth(scrollableContainer, 0.5f).Minus(25),

                        lblPaidAmountValue.Below(lblClaimAmount, 9),
                        lblPaidAmountValue.ToRightOf(lblPaidAmount, 8),
                        lblPaidAmountValue.Height().EqualTo(18),
                        lblPaidAmountValue.WithSameWidth(lblPaidAmount).Minus(20),

                        lblOtherPaidAmount.Below(lblPaidAmount, 9),
                        lblOtherPaidAmount.AtLeftOf(totalView, 10),
                        lblOtherPaidAmount.WithRelativeWidth(scrollableContainer, 0.5f).Minus(25),
                        lblOtherPaidAmount.AtBottomOf(totalView, 20),

                        lblOtherPaidAmountValue.Below(lblPaidAmount, 9),
                        lblOtherPaidAmountValue.ToRightOf(lblOtherPaidAmount, 8),
                        lblOtherPaidAmountValue.Height().EqualTo(18),
                        lblOtherPaidAmountValue.WithSameWidth(lblOtherPaidAmount).Minus(20),

                        descriptionBackgroundView.Below(totalView, 14),
                        descriptionBackgroundView.AtLeftOf(scrollableContainer, 20),
                        descriptionBackgroundView.AtRightOf(scrollableContainer, 20),

                        lblDescription.AtTopOf(descriptionBackgroundView, 10),
                        lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                        lblDescription.AtRightOf(descriptionBackgroundView, 10),
                        lblDescription.AtBottomOf(descriptionBackgroundView, 10),
                        
                        RequiresAuditLabel.Below(descriptionBackgroundView, requiresAuditLabelTopPadding),
                        RequiresAuditLabel.WithSameCenterX(scrollableContainer),
                        RequiresAuditLabel.WithSameWidth(scrollableContainer).Minus(40),

                        OpenDocumentUploadButtonForAudit.Below(RequiresAuditLabel, uploadButtonTopPadding),
                        OpenDocumentUploadButtonForAudit.WithSameCenterX(scrollableContainer),
                        OpenDocumentUploadButtonForAudit.Height().EqualTo(uploadButtonHeight),
                        OpenDocumentUploadButtonForAudit.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),

                        btnSubmitClaim.Below(OpenDocumentUploadButtonForAudit, Constants.CLAIMS_DETAILS_COMPONENT_PADDING * 2),
                        btnSubmitClaim.WithSameCenterX(scrollableContainer),
                        btnSubmitClaim.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                        btnSubmitClaim.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                        btnSubmitClaim.AtBottomOf(scrollableContainer, 100)
                    );
                }
                else
                {
                    View.AddConstraints(
                       scrollableContainer.AtTopOf(View, yOffset),// scrollContainerTopMargin),
                       scrollableContainer.AtLeftOf(View),
                       scrollableContainer.WithSameWidth(View),
                       scrollableContainer.AtBottomOf(View),

                        RequiresCOPLabel.AtTopOf(scrollableContainer, 0),
                        RequiresCOPLabel.WithSameCenterX(scrollableContainer),
                        RequiresCOPLabel.AtLeftOf(scrollableContainer, 20),
                        RequiresCOPLabel.AtRightOf(scrollableContainer, 20),

                        OpenDocumentUploadButton.Below(RequiresCOPLabel, 5),
                        OpenDocumentUploadButton.WithSameCenterX(scrollableContainer),
                        OpenDocumentUploadButton.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                        OpenDocumentUploadButton.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),

                        lblPlanInformation.Below(OpenDocumentUploadButton, 25),
                        lblPlanInformation.AtLeftOf(scrollableContainer, 20),
                       lblPlanInformation.AtRightOf(View, 20),
                       lblPlanInformation.Height().EqualTo(28),

                       lblGreenShieldId.Below(lblPlanInformation, 12),
                       lblGreenShieldId.AtLeftOf(scrollableContainer, 20),
                       lblGreenShieldId.WithRelativeWidth(View, 0.5f).Minus(15),

                       lblGreenShieldIdValue.WithSameTop(lblGreenShieldId).Plus(topAdjustment),
                       lblGreenShieldIdValue.ToRightOf(lblGreenShieldId, 10),
                       lblGreenShieldIdValue.AtRightOf(View, 20),


                       lblParticipentName.Below(lblGreenShieldId, 9),
                       lblParticipentName.AtLeftOf(scrollableContainer, 20),
                       lblParticipentName.WithRelativeWidth(View, 0.5f).Minus(15),
                       lblParticipentName.WithSameHeight(lblParticipentNameValue),

                       lblParticipentNameValue.WithSameTop(lblParticipentName).Plus(topAdjustment),
                       lblParticipentNameValue.ToRightOf(lblParticipentName, 10),
                       lblParticipentNameValue.AtRightOf(View, 20),

                       lblSubmissionDate.Below(lblParticipentName, 9),
                       lblSubmissionDate.AtLeftOf(scrollableContainer, 20),
                       lblSubmissionDate.WithRelativeWidth(scrollableContainer, 0.5f).Minus(50),

                       lblSubmissionDateValue.WithSameTop(lblSubmissionDate).Plus(topAdjustment),
                       lblSubmissionDateValue.WithSameLeft(lblParticipentNameValue),
                       lblSubmissionDateValue.WithSameWidth(lblSubmissionDate)
                   );

                    View.AddConstraints(
                        lblClaimDetails.Below(lblSubmissionDate, 26),//need change line here if referred show
                        lblClaimDetails.Height().EqualTo(28),
                        lblClaimDetails.AtLeftOf(scrollableContainer, 20),
                        lblClaimDetails.AtRightOf(View, 20),

                        claimsTableView.Below(lblClaimDetails, 12),
                        claimsTableView.AtLeftOf(scrollableContainer, 20),
                        claimsTableView.AtRightOf(View, 20),
                        claimsTableView.Height().EqualTo(tableFrame.Height),// need to change

                        lblTotal.Below(claimsTableView, 12),
                        lblTotal.Height().EqualTo(28),
                        lblTotal.AtLeftOf(scrollableContainer, 20),
                        lblTotal.AtRightOf(View, 20),

                        totalView.Below(lblTotal, 10), //give border
                                                       //                   totalView.Height ().EqualTo (80),//have given bottom
                        totalView.AtLeftOf(scrollableContainer, 20),
                        totalView.AtRightOf(View, 20),

                        lblClaimAmount.AtTopOf(totalView, 9),
                        lblClaimAmount.AtLeftOf(totalView, 10),
                        lblClaimAmount.WithRelativeWidth(View, 0.5f).Minus(20),

                        lblClaimAmountValue.AtTopOf(totalView, 9),
                        lblClaimAmountValue.ToRightOf(lblClaimAmount, 8),
                        lblClaimAmountValue.WithSameHeight(lblClaimAmount),

                        lblPaidAmount.Below(lblClaimAmount, 9),
                        lblPaidAmount.AtLeftOf(totalView, 10),
                        lblPaidAmount.WithRelativeWidth(View, 0.5f).Minus(20),


                        lblPaidAmountValue.Below(lblClaimAmount, 9),
                        lblPaidAmountValue.ToRightOf(lblPaidAmount, 8),
                        lblPaidAmountValue.Height().EqualTo(18),

                        lblOtherPaidAmount.Below(lblPaidAmount, 9),
                        lblOtherPaidAmount.AtLeftOf(totalView, 10),
                        lblOtherPaidAmount.WithRelativeWidth(View, 0.5f).Minus(20),
                        lblOtherPaidAmount.AtBottomOf(totalView, 20),

                        lblOtherPaidAmountValue.Below(lblPaidAmount, 9),
                        lblOtherPaidAmountValue.ToRightOf(lblOtherPaidAmount, 8),
                        lblOtherPaidAmountValue.Height().EqualTo(18),

                        descriptionBackgroundView.Below(totalView, 14),
                        descriptionBackgroundView.AtLeftOf(scrollableContainer, 20),
                        descriptionBackgroundView.AtRightOf(View, 20),

                        lblDescription.AtTopOf(descriptionBackgroundView, 8),
                        lblDescription.AtLeftOf(descriptionBackgroundView, 8),
                        lblDescription.AtRightOf(descriptionBackgroundView, 8),
                        lblDescription.AtBottomOf(descriptionBackgroundView, 10),
                        
                        RequiresAuditLabel.Below(descriptionBackgroundView, requiresAuditLabelTopPadding),
                        RequiresAuditLabel.WithSameCenterX(scrollableContainer),
                        RequiresAuditLabel.WithSameWidth(scrollableContainer).Minus(40),

                        OpenDocumentUploadButtonForAudit.Below(RequiresAuditLabel, uploadButtonTopPadding),
                        OpenDocumentUploadButtonForAudit.WithSameCenterX(scrollableContainer),
                        OpenDocumentUploadButtonForAudit.Height().EqualTo(uploadButtonHeight),
                        OpenDocumentUploadButtonForAudit.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),

                        btnSubmitClaim.Below(OpenDocumentUploadButtonForAudit, Constants.CLAIMS_DETAILS_COMPONENT_PADDING * 2),

                        btnSubmitClaim.WithSameCenterX(scrollableContainer),
                        btnSubmitClaim.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                        btnSubmitClaim.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                        btnSubmitClaim.AtBottomOf(scrollableContainer, 100)
                    );
                }
            }
            else
            {
                View.AddConstraints(
                    scrollableContainer.AtTopOf(View, Constants.TOP_PADDING),
                    scrollableContainer.AtLeftOf(View),
                    scrollableContainer.WithSameWidth(View),
                    scrollableContainer.AtBottomOf(View),

                    RequiresCOPLabel.AtTopOf(scrollableContainer, 0),
                    RequiresCOPLabel.WithSameCenterX(scrollableContainer),
                    RequiresCOPLabel.AtLeftOf(scrollableContainer, 20),
                    RequiresCOPLabel.AtRightOf(scrollableContainer, 20),

                    OpenDocumentUploadButton.Below(RequiresCOPLabel, 5),
                    OpenDocumentUploadButton.WithSameCenterX(scrollableContainer),
                    OpenDocumentUploadButton.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                    OpenDocumentUploadButton.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),

                    lblPlanInformation.Below(OpenDocumentUploadButton, 25),
                    lblPlanInformation.AtLeftOf(scrollableContainer, 20),
                    lblPlanInformation.AtRightOf(View, 20),
                    lblPlanInformation.Height().EqualTo(28),

                    lblGreenShieldId.Below(lblPlanInformation, 12),
                    lblGreenShieldId.AtLeftOf(scrollableContainer, 35),
                    lblGreenShieldId.WithRelativeWidth(scrollableContainer, 0.5f).Minus(30),

                    lblGreenShieldIdValue.WithSameTop(lblGreenShieldId).Plus(topAdjustment),
                    lblGreenShieldIdValue.ToRightOf(lblGreenShieldId, 10),
                    lblGreenShieldIdValue.WithRelativeWidth(scrollableContainer, 0.5f).Minus(45),

                    lblParticipentName.Below(lblGreenShieldId, 9),
                    lblParticipentName.AtLeftOf(scrollableContainer, 35),
                    lblParticipentName.WithRelativeWidth(scrollableContainer, 0.5f).Minus(50),

                    lblParticipentNameValue.WithSameTop(lblParticipentName).Plus(topAdjustment),
                    lblParticipentNameValue.WithSameLeft(lblGreenShieldIdValue),
                    lblParticipentNameValue.WithSameWidth(lblGreenShieldIdValue),

                    lblSubmissionDate.Below(lblParticipentName, 9),
                    lblSubmissionDate.AtLeftOf(scrollableContainer, 35),
                    lblSubmissionDate.WithRelativeWidth(scrollableContainer, 0.5f).Minus(50),

                    lblSubmissionDateValue.WithSameTop(lblSubmissionDate).Plus(topAdjustment),
                    lblSubmissionDateValue.WithSameLeft(lblParticipentNameValue),
                    lblSubmissionDateValue.WithSameWidth(lblParticipentNameValue)
                );

                View.AddConstraints(
                    lblClaimDetails.Below(lblSubmissionDateValue, 25),
                    lblClaimDetails.Height().EqualTo(28),
                    lblClaimDetails.AtLeftOf(scrollableContainer, 20),
                    lblClaimDetails.AtRightOf(View, 20),

                    claimsTableView.Below(lblClaimDetails, 12),
                    claimsTableView.AtLeftOf(scrollableContainer, 20),
                    claimsTableView.AtRightOf(View, 20),
                    claimsTableView.WithSameWidth(scrollableContainer).Minus(40),
                    claimsTableView.Height().EqualTo(tableFrame.Height),

                    lblTotal.Below(claimsTableView, 12),
                    lblTotal.Height().EqualTo(28),
                    lblTotal.AtLeftOf(scrollableContainer, 20),
                    lblTotal.AtRightOf(View, 20),

                    totalView.Below(lblTotal, 10),
                    totalView.AtLeftOf(scrollableContainer, 20),
                    totalView.AtRightOf(View, 20),

                    lblClaimAmount.AtTopOf(totalView, 9),
                    lblClaimAmount.AtLeftOf(totalView, 18),
                    lblClaimAmount.Height().EqualTo(18),
                    lblClaimAmount.WithRelativeWidth(scrollableContainer, 0.5f).Minus(60),

                    lblClaimAmountValue.AtTopOf(totalView, 9),
                    lblClaimAmountValue.ToRightOf(lblClaimAmount, 35),
                    lblClaimAmountValue.AtRightOf(totalView, 5),
                    lblClaimAmountValue.Height().EqualTo(18),

                    lblPaidAmount.Below(lblClaimAmount, 9),
                    lblPaidAmount.AtLeftOf(totalView, 18),
                    lblPaidAmount.WithRelativeWidth(scrollableContainer, 0.5f).Minus(60),

                    lblPaidAmountValue.Below(lblClaimAmount, 9),
                    lblPaidAmountValue.ToRightOf(lblPaidAmount, 35),
                    lblPaidAmountValue.AtRightOf(totalView, 5),
                    lblPaidAmountValue.Height().EqualTo(18),

                    lblOtherPaidAmount.Below(lblPaidAmount, 9),
                    lblOtherPaidAmount.AtLeftOf(totalView, 18),
                    lblOtherPaidAmount.Height().EqualTo(18),
                    lblOtherPaidAmount.WithRelativeWidth(scrollableContainer, 0.5f).Minus(60),
                    lblOtherPaidAmount.AtBottomOf(totalView, 20),

                    lblOtherPaidAmountValue.Below(lblPaidAmount, 9),
                    lblOtherPaidAmountValue.ToRightOf(lblOtherPaidAmount, 35),
                    lblOtherPaidAmountValue.AtRightOf(totalView, 5),
                    lblOtherPaidAmountValue.Height().EqualTo(18),

                    descriptionBackgroundView.Below(totalView, 50),
                    descriptionBackgroundView.AtLeftOf(scrollableContainer, 20),
                    descriptionBackgroundView.AtRightOf(View, 20),

                    lblDescription.AtTopOf(descriptionBackgroundView, 16),
                    lblDescription.AtLeftOf(descriptionBackgroundView, 16),
                    lblDescription.AtRightOf(descriptionBackgroundView, 16),
                    lblDescription.AtBottomOf(descriptionBackgroundView, 16),
                    
                    RequiresAuditLabel.Below(descriptionBackgroundView, requiresAuditLabelTopPadding),
                    RequiresAuditLabel.WithSameCenterX(scrollableContainer),
                    RequiresAuditLabel.WithSameWidth(scrollableContainer).Minus(40),

                    OpenDocumentUploadButtonForAudit.Below(RequiresAuditLabel, uploadButtonTopPadding),
                    OpenDocumentUploadButtonForAudit.WithSameCenterX(scrollableContainer),
                    OpenDocumentUploadButtonForAudit.Height().EqualTo(uploadButtonHeight),
                    OpenDocumentUploadButtonForAudit.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),

                    btnSubmitClaim.Below(OpenDocumentUploadButtonForAudit, Constants.CLAIMS_DETAILS_COMPONENT_PADDING * 2),
                    btnSubmitClaim.WithSameCenterX(scrollableContainer),
                    btnSubmitClaim.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                    btnSubmitClaim.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                    btnSubmitClaim.AtBottomOf(scrollableContainer, 100)
                );
            }
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetConstraints();
        }
    }
}