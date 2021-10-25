using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Entities.HCSA;
using MobileClaims.Core.ViewModels.HCSA;
using MobileClaims.iOS.UI;
using MobileClaims.iOS.Views.Claims;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS
{
    public class ClaimConfirmationHCSAView : GSCBaseViewController
    {
        int referralTypeVisibleNo;
        bool isiPadlandscape;
        ClaimConfirmationHCSAViewModel _model;

        public UIView planInfoView;
        public UILabel lblPlanInformation;
        public UILabel lblGreenShieldId;
        public UILabel lblGreenShieldIdValue;
        public UILabel lblParticipentName;
        public UILabel lblParticipentNameValue;
        public UILabel lblProfessionalName;
        public UILabel lblProfessionalNameValue;

        public UILabel lblClaimDetails;
        public UITableView claimsTableView;

        public UILabel lblTotal;
        public UIView totalView;
        public UILabel lblClaimAmount;
        public UILabel lblClaimAmountValue;

        public UILabel lblAmtPreviouslyPaid;
        public UILabel lblAmtPreviouslyPaidValue;
        public UIView descriptionBackgroundView;

        public UILabel lblDescription;

        public GSButton btnSubmitClaim;
        public UIScrollView scrollableContainer;
        public CGRect tableFrame;

        public UIView vewTableTitle;
        public UILabel lblGreenLine, lblGrayLine, lblTitleDateOfExpense, lblTitleExpenseType, lblTitleClaimAmount, lblTitleAmtPreviouslyPaid;

        public ClaimConfirmationHCSAView()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _model = (ClaimConfirmationHCSAViewModel)this.ViewModel;

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = _model.TitleLabel;

            isiPadlandscape = Helpers.IsInLandscapeMode();
            base.NavigationItem.SetHidesBackButton(true, false);
            UIBarButtonItem cancelButton = new UIBarButtonItem();
            cancelButton.Style = UIBarButtonItemStyle.Plain;
            cancelButton.Clicked += HandleCancelButton;
            cancelButton.Title = "cancel".tr();
            cancelButton.TintColor = Colors.HIGHLIGHT_COLOR;
            UITextAttributes attributes = new UITextAttributes();
            attributes.Font = UIFont.FromName(Constants.NUNITO_REGULAR, (nfloat)Constants.NAV_BAR_BUTTON_SIZE);
            cancelButton.SetTitleTextAttributes(attributes, UIControlState.Normal);
            base.NavigationItem.LeftBarButtonItem = cancelButton;

            View = new UIView()
            {
                BackgroundColor = Colors.BACKGROUND_COLOR
            };
            this.AutomaticallyAdjustsScrollViewInsets = false;

            scrollableContainer = new UIScrollView();
            scrollableContainer.ScrollEnabled = true;

            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                scrollableContainer.ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Never;
            }
            View.AddSubview(scrollableContainer);

            planInfoView = new UIView();
            //
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
            lblGreenShieldId.Lines = 0;
            lblGreenShieldId.LineBreakMode = UILineBreakMode.WordWrap;
            lblGreenShieldId.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);

            lblGreenShieldIdValue = new UILabel();
            lblGreenShieldIdValue.BackgroundColor = Colors.Clear;
            lblGreenShieldIdValue.TextColor = Colors.DarkGrayColor;
            lblGreenShieldIdValue.TextAlignment = UITextAlignment.Left;
            lblGreenShieldIdValue.Lines = 0;
            lblGreenShieldIdValue.LineBreakMode = UILineBreakMode.WordWrap;
            lblGreenShieldIdValue.Font = UIFont.FromName(Constants.NUNITO_BOLD, 12);

            lblParticipentName = new UILabel();
            lblParticipentName.BackgroundColor = Colors.Clear;
            lblParticipentName.TextColor = Colors.DARK_GREY_COLOR;
            lblParticipentName.TextAlignment = UITextAlignment.Left;
            lblParticipentName.Lines = 1;
            lblParticipentName.LineBreakMode = UILineBreakMode.WordWrap;
            lblParticipentName.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);

            lblParticipentNameValue = new UILabel();
            lblParticipentNameValue.BackgroundColor = Colors.Clear;
            lblParticipentNameValue.TextColor = Colors.DarkGrayColor;
            lblParticipentNameValue.TextAlignment = UITextAlignment.Left;
            lblParticipentNameValue.Lines = 1;
            lblParticipentNameValue.LineBreakMode = UILineBreakMode.WordWrap;
            lblParticipentNameValue.Font = UIFont.FromName(Constants.NUNITO_BOLD, 12);

            lblProfessionalName = new UILabel();
            lblProfessionalName.BackgroundColor = Colors.Clear;
            lblProfessionalName.TextColor = Colors.DARK_GREY_COLOR;
            lblProfessionalName.TextAlignment = UITextAlignment.Left;
            lblProfessionalName.Lines = 3;
            lblProfessionalName.LineBreakMode = UILineBreakMode.WordWrap;
            lblProfessionalName.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);

            lblProfessionalNameValue = new UILabel();
            lblProfessionalNameValue.BackgroundColor = Colors.Clear;
            lblProfessionalNameValue.TextColor = Colors.DarkGrayColor;
            lblProfessionalNameValue.TextAlignment = UITextAlignment.Left;
            lblProfessionalNameValue.Lines = 3;
            lblProfessionalNameValue.LineBreakMode = UILineBreakMode.WordWrap;
            lblProfessionalNameValue.Font = UIFont.FromName(Constants.NUNITO_BOLD, 12);

            lblClaimDetails = new UILabel();
            lblClaimDetails.BackgroundColor = Colors.Clear;
            lblClaimDetails.TextColor = Colors.DARK_GREY_COLOR;
            lblClaimDetails.TextAlignment = UITextAlignment.Left;
            lblClaimDetails.Lines = 1;
            lblClaimDetails.LineBreakMode = UILineBreakMode.WordWrap;
            lblClaimDetails.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, 24);

            scrollableContainer.AddSubview(lblPlanInformation);
            scrollableContainer.AddSubview(lblGreenShieldId);
            scrollableContainer.AddSubview(lblGreenShieldIdValue);
            scrollableContainer.AddSubview(lblParticipentName);
            scrollableContainer.AddSubview(lblParticipentNameValue);
            scrollableContainer.AddSubview(lblProfessionalName);
            scrollableContainer.AddSubview(lblProfessionalNameValue);
            scrollableContainer.AddSubview(lblClaimDetails);

            if (!Constants.IsPhone())
            {

                vewTableTitle = new UIView();
                lblGreenLine = new UILabel();
                lblGrayLine = new UILabel();
                lblGrayLine.BackgroundColor = UIColor.LightGray;
                lblGreenLine.BackgroundColor = Colors.HIGHLIGHT_COLOR;

                lblTitleDateOfExpense = new UILabel();
                lblTitleDateOfExpense.BackgroundColor = Colors.Clear;
                lblTitleDateOfExpense.TextAlignment = UITextAlignment.Left;
                lblTitleDateOfExpense.Lines = 1;
                lblTitleDateOfExpense.LineBreakMode = UILineBreakMode.WordWrap;
                lblTitleDateOfExpense.Font = UIFont.FromName(Constants.NUNITO_BOLD, 13);
                lblTitleDateOfExpense.TextColor = Colors.DARK_GREY_COLOR;

                lblTitleExpenseType = new UILabel();
                lblTitleExpenseType.BackgroundColor = Colors.Clear;
                lblTitleExpenseType.TextAlignment = UITextAlignment.Left;
                lblTitleExpenseType.Lines = 1;
                lblTitleExpenseType.LineBreakMode = UILineBreakMode.WordWrap;
                lblTitleExpenseType.Font = UIFont.FromName(Constants.NUNITO_BOLD, 13);
                lblTitleExpenseType.TextColor = Colors.DARK_GREY_COLOR;

                lblTitleClaimAmount = new UILabel();
                lblTitleClaimAmount.BackgroundColor = Colors.Clear;
                lblTitleClaimAmount.TextAlignment = UITextAlignment.Left;
                lblTitleClaimAmount.Lines = 1;
                lblTitleClaimAmount.LineBreakMode = UILineBreakMode.WordWrap;
                lblTitleClaimAmount.Font = UIFont.FromName(Constants.NUNITO_BOLD, 13);
                lblTitleClaimAmount.TextColor = Colors.DARK_GREY_COLOR;

                lblTitleAmtPreviouslyPaid = new UILabel();
                lblTitleAmtPreviouslyPaid.Text = "PaidByInsurerGovt".tr();
                lblTitleAmtPreviouslyPaid.BackgroundColor = Colors.Clear;
                lblTitleAmtPreviouslyPaid.TextAlignment = UITextAlignment.Left;
                lblTitleAmtPreviouslyPaid.Lines = 0;
                lblTitleAmtPreviouslyPaid.LineBreakMode = UILineBreakMode.WordWrap;
                lblTitleAmtPreviouslyPaid.Font = UIFont.FromName(Constants.NUNITO_BOLD, 13);
                lblTitleAmtPreviouslyPaid.TextColor = Colors.DARK_GREY_COLOR;

                lblTitleClaimAmount.TextAlignment = UITextAlignment.Right;
                lblTitleAmtPreviouslyPaid.TextAlignment = UITextAlignment.Right;

                vewTableTitle.AddSubview(lblTitleDateOfExpense);
                vewTableTitle.AddSubview(lblTitleExpenseType);
                vewTableTitle.AddSubview(lblTitleClaimAmount);
                vewTableTitle.AddSubview(lblTitleAmtPreviouslyPaid);

                scrollableContainer.AddSubview(vewTableTitle);
                scrollableContainer.AddSubview(lblGreenLine);
                scrollableContainer.AddSubview(lblGrayLine);
            }

            claimsTableView = new UITableView();
            claimsTableView.SeparatorColor = Colors.Clear;

            lblTotal = new UILabel();
            lblTotal.Text = "TOTAL".tr();
            lblTotal.BackgroundColor = Colors.Clear;
            lblTotal.TextAlignment = UITextAlignment.Left;
            lblTotal.Lines = 1;
            lblTotal.LineBreakMode = UILineBreakMode.WordWrap;

            totalView = new UIView();

            if (Constants.IsPhone())
            {
                totalView.Layer.BorderColor = Colors.DARK_GREY_COLOR.CGColor;
                totalView.Layer.BorderWidth = 2.0f;
                lblTotal.TextColor = Colors.DARK_GREY_COLOR;
                lblTotal.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, 24);
            }
            else
            {
                lblTotal.TextColor = Colors.DarkGrayColor;
                lblTotal.Font = UIFont.FromName(Constants.NUNITO_BOLD, 12);
            }

            lblClaimAmount = new UILabel();
            lblClaimAmount.Text = "ClaimAmount";
            lblClaimAmount.BackgroundColor = Colors.Clear;
            lblClaimAmount.TextColor = Colors.DARK_GREY_COLOR;
            lblClaimAmount.TextAlignment = UITextAlignment.Left;
            lblClaimAmount.Lines = 0;
            lblClaimAmount.LineBreakMode = UILineBreakMode.WordWrap;
            lblClaimAmount.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);

            lblClaimAmountValue = new UILabel();
            lblClaimAmountValue.BackgroundColor = Colors.Clear;
            lblClaimAmountValue.TextColor = Colors.DarkGrayColor;
            lblClaimAmountValue.TextAlignment = UITextAlignment.Left;
            lblClaimAmountValue.Lines = 1;
            lblClaimAmountValue.LineBreakMode = UILineBreakMode.WordWrap;
            lblClaimAmountValue.Font = UIFont.FromName(Constants.NUNITO_BOLD, 12);

            lblAmtPreviouslyPaid = new UILabel();
            lblAmtPreviouslyPaid.BackgroundColor = Colors.Clear;
            lblAmtPreviouslyPaid.TextColor = Colors.DARK_GREY_COLOR;
            lblAmtPreviouslyPaid.TextAlignment = UITextAlignment.Left;
            lblAmtPreviouslyPaid.Lines = 0;
            lblAmtPreviouslyPaid.LineBreakMode = UILineBreakMode.WordWrap;
            lblAmtPreviouslyPaid.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);

            lblAmtPreviouslyPaidValue = new UILabel();
            lblAmtPreviouslyPaidValue.BackgroundColor = Colors.Clear;
            lblAmtPreviouslyPaidValue.TextColor = Colors.DarkGrayColor;
            lblAmtPreviouslyPaidValue.TextAlignment = UITextAlignment.Left;
            lblAmtPreviouslyPaidValue.Lines = 1;
            lblAmtPreviouslyPaidValue.LineBreakMode = UILineBreakMode.WordWrap;
            lblAmtPreviouslyPaidValue.Font = UIFont.FromName(Constants.NUNITO_BOLD, 12);

            descriptionBackgroundView = new UIView();

            lblDescription = new UILabel();
            lblDescription.Text = "submittedToGSC".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada);
            lblDescription.TextColor = Colors.DARK_GREY_COLOR;
            lblDescription.Lines = 0;
            lblDescription.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, (nfloat)Constants.TEXTVIEW_FONT_SIZE);

            btnSubmitClaim = new GSButton();
            btnSubmitClaim.TitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.GREEN_BUTTON_FONT_SIZE);// (nfloat)Constants.GREEN_BUTTON_FONT_SIZE);
            btnSubmitClaim.SetTitle("submitClaim".tr(), UIControlState.Normal);

            var source = new ClaimsConfirmationTableViewSource(_model, claimsTableView);
            claimsTableView.Source = source;
            claimsTableView.RowHeight = UITableView.AutomaticDimension;
            scrollableContainer.AddSubview(claimsTableView);

            if (Constants.IsPhone())
            {
                scrollableContainer.AddSubview(lblTotal);
                totalView.AddSubview(lblClaimAmount);
                totalView.AddSubview(lblAmtPreviouslyPaid);
            }
            else
            {
                claimsTableView.SeparatorColor = Colors.Clear;
                lblClaimAmountValue.TextAlignment = UITextAlignment.Right;
                lblAmtPreviouslyPaidValue.TextAlignment = UITextAlignment.Right;
                totalView.AddSubview(lblTotal);
            }
            totalView.AddSubview(lblClaimAmountValue);
            totalView.AddSubview(lblAmtPreviouslyPaidValue);

            scrollableContainer.AddSubview(totalView);
            descriptionBackgroundView.AddSubview(lblDescription);
            scrollableContainer.AddSubview(descriptionBackgroundView);
            scrollableContainer.AddSubview(btnSubmitClaim);

            if (_model.IsReferrerVisible)
            {
                referralTypeVisibleNo = 1;
            }
            else
            {
                referralTypeVisibleNo = 0;
            }

            _model.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "IsReferrerVisible")
                {
                    if (_model.IsReferrerVisible)
                    {
                        referralTypeVisibleNo = 1;
                    }
                    else
                    {
                        referralTypeVisibleNo = 0;
                    }
                }
                SetConstraints();
            };

            SetConstraints();
            var set = this.CreateBindingSet<ClaimConfirmationHCSAView, ClaimConfirmationHCSAViewModel>();
            set.Bind(source).To(vm => vm.Claim.Details);

            set.Bind(lblPlanInformation).To(vm => vm.PlanInfoLabel).WithConversion("StringCase");
            set.Bind(lblGreenShieldId).To(vm => vm.IDNumberLabel);
            set.Bind(lblParticipentName).To(vm => vm.ParticipantLabel);
            set.Bind(lblProfessionalName).To(vm => vm.RefferedLabel);
            set.Bind(lblClaimDetails).To(vm => vm.DetailsLabel).WithConversion("StringCase");
            set.Bind(lblTotal).To(vm => vm.TotalLabel).WithConversion("StringCase");
            set.Bind(lblClaimAmount).To(vm => vm.ClaimedAmountLabel);
            set.Bind(lblTitleAmtPreviouslyPaid).To(vm => vm.OtherPaidAmountLabel);

            if (!Constants.IsPhone())
            {
                set.Bind(lblTitleExpenseType).To(vm => vm.TypeExpenseLabel);
                set.Bind(lblTitleDateOfExpense).To(vm => vm.DateOfExpenseLabel);
                set.Bind(lblTitleClaimAmount).To(vm => vm.ClaimedAmountLabel);
            }

            set.Bind(lblGreenShieldIdValue).To(vm => vm.Participant.PlanMemberID);
            set.Bind(lblParticipentNameValue).To(vm => vm.Participant.FullName);
            set.Bind(lblProfessionalNameValue).To(vm => vm.ReferralType.Text);
            set.Bind(lblProfessionalNameValue).For(b => b.Hidden).To(vm => vm.IsReferrerVisible).WithConversion("BoolOpposite");
            set.Bind(lblProfessionalName).For(b => b.Hidden).To(vm => vm.IsReferrerVisible).WithConversion("BoolOpposite");

            set.Bind(lblClaimAmountValue).To(vm => vm.TotalClaimAmount).WithConversion("DollarSignDoublePrefix");
            set.Bind(lblAmtPreviouslyPaid).To(vm => vm.OtherPaidAmountLabel);
            set.Bind(lblAmtPreviouslyPaidValue).To(vm => vm.OtherPaidTotalAmount).WithConversion("DollarSignDoublePrefix");
            set.Bind(this.btnSubmitClaim).To(vm => vm.SubmitClaimCommand);
            set.Bind(this.btnSubmitClaim).For("Title").To(vm => vm.SubmitButtonLabel).WithConversion("StringCase");

            set.Apply();
            btnSubmitClaim.TitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.GREEN_BUTTON_FONT_SIZE);
        }


        public override void WillAnimateRotation(UIInterfaceOrientation toInterfaceOrientation, double duration)
        {
            base.WillAnimateRotation(toInterfaceOrientation, duration);
            if (Constants.IsPhone())
            {
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
        }

        void HandleCancelButton(object sender, EventArgs e)
        {
            int backTo = 2;
            if (base.NavigationController.ViewControllers[base.NavigationController.ViewControllers.Length - backTo].GetType() == typeof(ClaimSubmitTermsAndConditionsView))
            {
                backTo++;
            }
            base.NavigationController.PopToViewController(base.NavigationController.ViewControllers[base.NavigationController.ViewControllers.Length - backTo], true);
        }

        public class ClaimsConfirmationTableViewSource : MvxTableViewSource
        {
            private ClaimConfirmationHCSAViewModel _model;

            public ClaimsConfirmationTableViewSource(ClaimConfirmationHCSAViewModel _model, UITableView tableView) : base(tableView)
            {
                this._model = _model;
                tableView.RegisterClassForCellReuse(typeof(ClaimConfirmationHCSATableViewCell), new NSString("ClaimConfirmationHCSATableViewCell"));
            }

            protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
            {
                return (UITableViewCell)tableView.DequeueReusableCell("ClaimConfirmationHCSATableViewCell");
            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                if (!Constants.IsPhone())
                {
                    if (_model.Claim.Details[indexPath.Row].ExpenseTypeDescription.Length > 50)
                    {
                        return 60;
                    }
                    else
                    {
                        return 37;
                    }
                }
                else
                {
                    if (Helpers.IsInLandscapeMode())
                    {
                        return 120;
                    }
                    else
                    {
                        if (_model.Claim.Details[indexPath.Row].ExpenseTypeDescription.Length > 25)
                        {
                            return 190;
                        }
                        else
                        {
                            return 150;
                        }
                    }
                }
            }
        }

        private void SetConstraints()
        {
            lblDescription.BackgroundColor = Colors.LightGrayColor;
            descriptionBackgroundView.BackgroundColor = Colors.LightGrayColor;

            tableFrame = claimsTableView.Frame;
            if (!Constants.IsPhone())
            {
                tableFrame.Height = (nfloat)(37 * _model.Claim.Details.Count);

                if (_model.Claim.Details != null)
                {
                    foreach (ClaimDetail cd in _model.Claim.Details)
                    {
                        if (!string.IsNullOrEmpty(cd.ExpenseTypeDescription))
                        {
                            if (cd.ExpenseTypeDescription.Length > 50)
                            {
                                tableFrame.Height += 60;
                            }
                            else
                            {
                                tableFrame.Height += 37;
                            }
                        }
                        else
                        {
                            tableFrame.Height += 37;
                        }
                    }
                }
            }
            else
            {
                tableFrame.Height = 0;
                if (!isiPadlandscape)
                {
                    if (_model.Claim.Details != null)
                    {
                        foreach (ClaimDetail cd in _model.Claim.Details)
                        {
                            if (!string.IsNullOrEmpty(cd.ExpenseTypeDescription))
                            {
                                if (cd.ExpenseTypeDescription.Length > 25)
                                {
                                    tableFrame.Height += 190;
                                }
                                else
                                {
                                    tableFrame.Height += 155;
                                }
                            }
                            else
                            {
                                tableFrame.Height += 155;
                            }
                        }
                    }
                }
                else
                    tableFrame.Height = (nfloat)(125 * _model.Claim.Details.Count);
            }
            claimsTableView.Frame = tableFrame;

            View.RemoveConstraints(View.Constraints);
            scrollableContainer.RemoveConstraints(scrollableContainer.Constraints);
            totalView.RemoveConstraints(totalView.Constraints);
            descriptionBackgroundView.RemoveConstraints(descriptionBackgroundView.Constraints);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            scrollableContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            totalView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            descriptionBackgroundView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            if (Constants.IsPhone())
            {
                float yOffset = Constants.TOP_PADDING;
                if (Helpers.IsInLandscapeMode())
                {
                    yOffset = 40;
                }

                if (referralTypeVisibleNo == 1)
                {
                    if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
                    {
                        View.AddConstraints(
                            scrollableContainer.AtTopOf(View, View.SafeAreaInsets.Top + 10),
                            scrollableContainer.AtLeftOf(View, View.SafeAreaInsets.Left),
                            scrollableContainer.AtRightOf(View, View.SafeAreaInsets.Right),
                            scrollableContainer.WithSameWidth(View).Minus(View.SafeAreaInsets.Left * 2),
                            scrollableContainer.AtBottomOf(View, View.SafeAreaInsets.Bottom + Constants.NAV_BUTTON_SIZE_IPHONE),

                            lblPlanInformation.AtTopOf(scrollableContainer, 0),
                            lblPlanInformation.AtLeftOf(scrollableContainer, 20),
                            lblPlanInformation.AtRightOf(View, 20),

                            lblGreenShieldId.Below(lblPlanInformation, 12),
                            lblGreenShieldId.AtLeftOf(scrollableContainer, 20),
                            lblGreenShieldId.WithRelativeWidth(View, 0.5f).Minus(15),

                            lblGreenShieldIdValue.Below(lblPlanInformation, 12),
                            lblGreenShieldIdValue.ToRightOf(lblGreenShieldId, 10),
                            lblGreenShieldIdValue.AtRightOf(View, 15),
                            lblGreenShieldIdValue.WithSameHeight(lblGreenShieldId),

                            lblParticipentName.Below(lblGreenShieldId, 9),
                            lblParticipentName.AtLeftOf(scrollableContainer, 20),
                            lblParticipentName.WithSameHeight(lblParticipentNameValue),
                            lblParticipentName.Width().EqualTo(150),
                            lblParticipentName.WithRelativeWidth(View, 0.5f).Minus(15),

                            lblParticipentNameValue.Below(lblGreenShieldId, 9),
                            lblParticipentNameValue.ToRightOf(lblParticipentName, 10),
                            lblParticipentNameValue.AtRightOf(View, 20),

                            lblProfessionalName.Below(lblParticipentName, 9),
                            lblProfessionalName.AtLeftOf(scrollableContainer, 20),
                            lblProfessionalName.WithRelativeWidth(View, 0.5f).Minus(15),

                            lblProfessionalNameValue.Below(lblParticipentName, 9),
                            lblProfessionalNameValue.ToRightOf(lblProfessionalName, 10),//10
                            lblProfessionalNameValue.AtRightOf(View, 20),
                            lblProfessionalNameValue.WithSameHeight(lblProfessionalName),

                            lblClaimDetails.Below(lblProfessionalName, 30),
                            lblClaimDetails.AtLeftOf(scrollableContainer, 20),
                            lblClaimDetails.AtRightOf(View, 20),

                            claimsTableView.Below(lblClaimDetails, 12),
                            claimsTableView.AtLeftOf(scrollableContainer, 20),
                            claimsTableView.AtRightOf(scrollableContainer, 20),
                            claimsTableView.Height().EqualTo(tableFrame.Height),

                            lblTotal.Below(claimsTableView, 12),
                            lblTotal.AtLeftOf(scrollableContainer, 20),
                            lblTotal.AtRightOf(View, 20),

                            totalView.Below(lblTotal, 10),
                            totalView.AtLeftOf(scrollableContainer, 20),
                            totalView.AtRightOf(scrollableContainer, 20),

                            lblClaimAmount.AtTopOf(totalView, 9),
                            lblClaimAmount.AtLeftOf(totalView, 5),
                            lblClaimAmount.WithRelativeWidth(scrollableContainer, 0.5f).Minus(25),

                            lblClaimAmountValue.AtTopOf(totalView, 9),
                            lblClaimAmountValue.ToRightOf(lblClaimAmount, 6),//17
                            lblClaimAmountValue.AtRightOf(totalView, 5),
                            lblClaimAmountValue.WithSameHeight(lblClaimAmount),
                            lblClaimAmountValue.WithSameWidth(lblClaimAmount).Minus(20),

                            lblAmtPreviouslyPaid.Below(lblClaimAmount, 9),
                            lblAmtPreviouslyPaid.AtLeftOf(totalView, 5),
                            lblAmtPreviouslyPaid.WithRelativeWidth(scrollableContainer, 0.5f).Minus(25),

                            lblAmtPreviouslyPaidValue.Below(lblClaimAmount, 9),
                            lblAmtPreviouslyPaidValue.ToRightOf(lblAmtPreviouslyPaid, 6),
                            lblAmtPreviouslyPaidValue.AtRightOf(totalView, 5),
                            lblAmtPreviouslyPaidValue.WithSameHeight(lblAmtPreviouslyPaid),
                            lblAmtPreviouslyPaidValue.WithSameWidth(lblAmtPreviouslyPaid).Minus(20),
                            lblAmtPreviouslyPaidValue.AtBottomOf(totalView, 15),

                            descriptionBackgroundView.Below(totalView, 14),
                            descriptionBackgroundView.AtLeftOf(scrollableContainer, 20),
                            descriptionBackgroundView.AtRightOf(scrollableContainer, 20),

                            lblDescription.AtTopOf(descriptionBackgroundView, 10),
                            lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                            lblDescription.AtRightOf(descriptionBackgroundView, 10),
                            lblDescription.AtBottomOf(descriptionBackgroundView, 10),

                            btnSubmitClaim.Below(descriptionBackgroundView, 25),
                            btnSubmitClaim.WithSameCenterX(scrollableContainer),
                            btnSubmitClaim.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                            btnSubmitClaim.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                            btnSubmitClaim.AtBottomOf(scrollableContainer, 100));
                    }
                    else
                    {
                        View.AddConstraints(
                            scrollableContainer.AtTopOf(View, yOffset),
                            scrollableContainer.AtLeftOf(View),
                            scrollableContainer.WithSameWidth(View),
                            scrollableContainer.AtBottomOf(View),

                            lblPlanInformation.AtTopOf(scrollableContainer, 0),
                            lblPlanInformation.AtLeftOf(scrollableContainer, 20),
                            lblPlanInformation.AtRightOf(View, 20),

                            lblGreenShieldId.Below(lblPlanInformation, 12),
                            lblGreenShieldId.AtLeftOf(scrollableContainer, 20),
                            lblGreenShieldId.WithRelativeWidth(View, 0.5f).Minus(15),

                            lblGreenShieldIdValue.Below(lblPlanInformation, 12),
                            lblGreenShieldIdValue.ToRightOf(lblGreenShieldId, 10),
                            lblGreenShieldIdValue.AtRightOf(View, 15),
                            lblGreenShieldIdValue.WithSameHeight(lblGreenShieldId),

                            lblParticipentName.Below(lblGreenShieldId, 9),
                            lblParticipentName.AtLeftOf(scrollableContainer, 20),
                            lblParticipentName.WithSameHeight(lblParticipentNameValue),
                            lblParticipentName.Width().EqualTo(150),
                            lblParticipentName.WithRelativeWidth(View, 0.5f).Minus(15),

                            lblParticipentNameValue.Below(lblGreenShieldId, 9),
                            lblParticipentNameValue.ToRightOf(lblParticipentName, 10),
                            lblParticipentNameValue.AtRightOf(View, 20),

                            lblProfessionalName.Below(lblParticipentName, 9),
                            lblProfessionalName.AtLeftOf(scrollableContainer, 20),
                            lblProfessionalName.WithRelativeWidth(View, 0.5f).Minus(15),

                            lblProfessionalNameValue.Below(lblParticipentName, 9),
                            lblProfessionalNameValue.ToRightOf(lblProfessionalName, 10),
                            lblProfessionalNameValue.AtRightOf(View, 20),
                            lblProfessionalNameValue.WithSameHeight(lblProfessionalName),

                            lblClaimDetails.Below(lblProfessionalName, 30),
                            lblClaimDetails.AtLeftOf(scrollableContainer, 20),
                            lblClaimDetails.AtRightOf(View, 20),

                            claimsTableView.Below(lblClaimDetails, 12),
                            claimsTableView.AtLeftOf(scrollableContainer, 20),
                            claimsTableView.AtRightOf(View, 20),
                            claimsTableView.Height().EqualTo(tableFrame.Height),

                            lblTotal.Below(claimsTableView, 12),
                            lblTotal.AtLeftOf(scrollableContainer, 20),
                            lblTotal.AtRightOf(View, 20),

                            totalView.Below(lblTotal, 10),
                            totalView.AtLeftOf(scrollableContainer, 20),
                            totalView.AtRightOf(View, 20),

                            lblClaimAmount.AtTopOf(totalView, 9),
                            lblClaimAmount.AtLeftOf(totalView, 5),
                            lblClaimAmount.WithRelativeWidth(View, 0.5f).Minus(15),

                            lblClaimAmountValue.AtTopOf(totalView, 9),
                            lblClaimAmountValue.ToRightOf(lblClaimAmount, 6),
                            lblClaimAmountValue.AtRightOf(totalView, 5),
                            lblClaimAmountValue.WithSameHeight(lblClaimAmount),

                            lblAmtPreviouslyPaid.Below(lblClaimAmount, 9),
                            lblAmtPreviouslyPaid.AtLeftOf(totalView, 5),
                            lblAmtPreviouslyPaid.WithRelativeWidth(View, 0.5f).Minus(15),

                            lblAmtPreviouslyPaidValue.Below(lblClaimAmount, 9),
                            lblAmtPreviouslyPaidValue.ToRightOf(lblAmtPreviouslyPaid, 6),
                            lblAmtPreviouslyPaidValue.AtRightOf(totalView, 5),
                            lblAmtPreviouslyPaidValue.WithSameHeight(lblAmtPreviouslyPaid),
                            lblAmtPreviouslyPaidValue.AtBottomOf(totalView, 15),


                            descriptionBackgroundView.Below(totalView, 14),
                            descriptionBackgroundView.AtLeftOf(scrollableContainer, 20),
                            descriptionBackgroundView.AtRightOf(View, 20),

                            lblDescription.AtTopOf(descriptionBackgroundView, 8),
                            lblDescription.AtLeftOf(descriptionBackgroundView, 8),
                            lblDescription.AtRightOf(descriptionBackgroundView, 8),
                            lblDescription.AtBottomOf(descriptionBackgroundView, 10),

                            btnSubmitClaim.Below(descriptionBackgroundView, 25),
                            btnSubmitClaim.WithSameCenterX(scrollableContainer),
                            btnSubmitClaim.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                            btnSubmitClaim.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                            btnSubmitClaim.AtBottomOf(scrollableContainer, 100));
                        }
                    }
                else
                {
                    if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
                    {
                        View.AddConstraints(
                            scrollableContainer.AtTopOf(View, View.SafeAreaInsets.Top + 10),
                            scrollableContainer.AtLeftOf(View, View.SafeAreaInsets.Left),
                            scrollableContainer.AtRightOf(View, View.SafeAreaInsets.Right),
                            scrollableContainer.WithSameWidth(View).Minus(View.SafeAreaInsets.Left * 2),
                            scrollableContainer.AtBottomOf(View, View.SafeAreaInsets.Bottom + Constants.NAV_BUTTON_SIZE_IPHONE),

                            lblPlanInformation.AtTopOf(scrollableContainer, 0),
                            lblPlanInformation.AtLeftOf(scrollableContainer, 20),
                            lblPlanInformation.AtRightOf(View, 20),

                            lblGreenShieldId.Below(lblPlanInformation, 12),
                            lblGreenShieldId.AtLeftOf(scrollableContainer, 20),
                            lblGreenShieldId.WithRelativeWidth(View, 0.5f).Minus(15),

                            lblGreenShieldIdValue.Below(lblPlanInformation, 12),
                            lblGreenShieldIdValue.ToRightOf(lblGreenShieldId, 10),
                            lblGreenShieldIdValue.AtRightOf(View, 15),
                            lblGreenShieldIdValue.WithSameHeight(lblGreenShieldId),

                            lblParticipentName.Below(lblGreenShieldId, 9),
                            lblParticipentName.AtLeftOf(scrollableContainer, 20),
                            lblParticipentName.WithSameHeight(lblParticipentNameValue),
                            lblParticipentName.Width().EqualTo(150),
                            lblParticipentName.WithRelativeWidth(View, 0.5f).Minus(15),

                            lblParticipentNameValue.Below(lblGreenShieldId, 9),
                            lblParticipentNameValue.ToRightOf(lblParticipentName, 10),
                            lblParticipentNameValue.AtRightOf(View, 20),

                            lblProfessionalName.Below(lblParticipentName, 9),
                            lblProfessionalName.AtLeftOf(scrollableContainer, 20),
                            lblProfessionalName.WithRelativeWidth(View, 0.5f).Minus(15),

                            lblProfessionalNameValue.Below(lblParticipentName, 9),
                            lblProfessionalNameValue.ToRightOf(lblProfessionalName, 10),
                            lblProfessionalNameValue.AtRightOf(View, 20),
                            lblProfessionalNameValue.WithSameHeight(lblProfessionalName),

                            lblClaimDetails.Below(lblParticipentNameValue, 30),
                            lblClaimDetails.AtLeftOf(scrollableContainer, 20),
                            lblClaimDetails.AtRightOf(View, 20),

                            claimsTableView.Below(lblClaimDetails, 12),
                            claimsTableView.AtLeftOf(scrollableContainer, 20),
                            claimsTableView.AtRightOf(scrollableContainer, 20),
                            claimsTableView.Height().EqualTo(tableFrame.Height),

                            lblTotal.Below(claimsTableView, 12),
                            lblTotal.AtLeftOf(scrollableContainer, 20),
                            lblTotal.AtRightOf(View, 20),

                            totalView.Below(lblTotal, 10),
                            totalView.AtLeftOf(scrollableContainer, 20),
                            totalView.AtRightOf(scrollableContainer, 20),

                            lblClaimAmount.AtTopOf(totalView, 9),
                            lblClaimAmount.AtLeftOf(totalView, 5),
                            lblClaimAmount.WithRelativeWidth(scrollableContainer, 0.5f).Minus(25),

                            lblClaimAmountValue.AtTopOf(totalView, 9),
                            lblClaimAmountValue.ToRightOf(lblClaimAmount, 6),//17
                            lblClaimAmountValue.AtRightOf(totalView, 5),
                            lblClaimAmountValue.WithSameHeight(lblClaimAmount),
                            lblClaimAmountValue.WithSameWidth(lblClaimAmount).Minus(20),

                            lblAmtPreviouslyPaid.Below(lblClaimAmount, 9),
                            lblAmtPreviouslyPaid.AtLeftOf(totalView, 5),
                            lblAmtPreviouslyPaid.WithRelativeWidth(scrollableContainer, 0.5f).Minus(25),

                            lblAmtPreviouslyPaidValue.Below(lblClaimAmount, 9),
                            lblAmtPreviouslyPaidValue.ToRightOf(lblAmtPreviouslyPaid, 6),
                            lblAmtPreviouslyPaidValue.AtRightOf(totalView, 5),
                            lblAmtPreviouslyPaidValue.WithSameHeight(lblAmtPreviouslyPaid),
                            lblAmtPreviouslyPaidValue.WithSameWidth(lblAmtPreviouslyPaid).Minus(20),
                            lblAmtPreviouslyPaidValue.AtBottomOf(totalView, 15),

                            descriptionBackgroundView.Below(totalView, 14),
                            descriptionBackgroundView.AtLeftOf(scrollableContainer, 20),
                            descriptionBackgroundView.AtRightOf(scrollableContainer, 20),

                            lblDescription.AtTopOf(descriptionBackgroundView, 10),
                            lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                            lblDescription.AtRightOf(descriptionBackgroundView, 10),
                            lblDescription.AtBottomOf(descriptionBackgroundView, 10),

                            btnSubmitClaim.Below(descriptionBackgroundView, 25),
                            btnSubmitClaim.WithSameCenterX(scrollableContainer),
                            btnSubmitClaim.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                            btnSubmitClaim.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                            btnSubmitClaim.AtBottomOf(scrollableContainer, 100));
                    }
                    else
                    {
                        View.AddConstraints(
                            scrollableContainer.AtTopOf(View, yOffset),
                            scrollableContainer.AtLeftOf(View),
                            scrollableContainer.WithSameWidth(View),
                            scrollableContainer.AtBottomOf(View),

                            lblPlanInformation.AtTopOf(scrollableContainer, 0),
                            lblPlanInformation.AtLeftOf(scrollableContainer, 20),
                            lblPlanInformation.AtRightOf(View, 20),

                            lblGreenShieldId.Below(lblPlanInformation, 12),
                            lblGreenShieldId.AtLeftOf(scrollableContainer, 20),
                            lblGreenShieldId.WithRelativeWidth(View, 0.5f).Minus(15),

                            lblGreenShieldIdValue.Below(lblPlanInformation, 12),
                            lblGreenShieldIdValue.ToRightOf(lblGreenShieldId, 10),
                            lblGreenShieldIdValue.AtRightOf(View, 15),
                            lblGreenShieldIdValue.WithSameHeight(lblGreenShieldId),

                            lblParticipentName.Below(lblGreenShieldId, 9),
                            lblParticipentName.AtLeftOf(scrollableContainer, 20),
                            lblParticipentName.WithSameHeight(lblParticipentNameValue),
                            lblParticipentName.Width().EqualTo(150),
                            lblParticipentName.WithRelativeWidth(View, 0.5f).Minus(15),

                            lblParticipentNameValue.Below(lblGreenShieldId, 9),
                            lblParticipentNameValue.ToRightOf(lblParticipentName, 10),
                            lblParticipentNameValue.AtRightOf(View, 20),
                         
                            lblProfessionalName.Below(lblParticipentName, 9),
                            lblProfessionalName.AtLeftOf(scrollableContainer, 20),
                            lblProfessionalName.WithRelativeWidth(View, 0.5f).Minus(15),

                            lblProfessionalNameValue.Below(lblParticipentName, 9),
                            lblProfessionalNameValue.ToRightOf(lblProfessionalName, 10),
                            lblProfessionalNameValue.AtRightOf(View, 20),
                            lblProfessionalNameValue.WithSameHeight(lblProfessionalName),

                            lblClaimDetails.Below(lblParticipentNameValue, 30),
                            lblClaimDetails.AtLeftOf(scrollableContainer, 20),
                            lblClaimDetails.AtRightOf(View, 20),

                            claimsTableView.Below(lblClaimDetails, 12),
                            claimsTableView.AtLeftOf(scrollableContainer, 20),
                            claimsTableView.AtRightOf(View, 20),
                            claimsTableView.Height().EqualTo(tableFrame.Height),

                            lblTotal.Below(claimsTableView, 12),
                            lblTotal.AtLeftOf(scrollableContainer, 20),
                            lblTotal.AtRightOf(View, 20),

                            totalView.Below(lblTotal, 10), 
                            totalView.AtLeftOf(scrollableContainer, 20),
                            totalView.AtRightOf(View, 20),

                            lblClaimAmount.AtTopOf(totalView, 9),
                            lblClaimAmount.AtLeftOf(totalView, 5),
                            lblClaimAmount.WithRelativeWidth(View, 0.5f).Minus(15),

                            lblClaimAmountValue.AtTopOf(totalView, 9),
                            lblClaimAmountValue.ToRightOf(lblClaimAmount, 6),
                            lblClaimAmountValue.AtRightOf(totalView, 5),
                            lblClaimAmountValue.WithSameHeight(lblClaimAmount),

                            lblAmtPreviouslyPaid.Below(lblClaimAmount, 9),
                            lblAmtPreviouslyPaid.AtLeftOf(totalView, 5),
                            lblAmtPreviouslyPaid.WithRelativeWidth(View, 0.5f).Minus(15),

                            lblAmtPreviouslyPaidValue.Below(lblClaimAmount, 9),
                            lblAmtPreviouslyPaidValue.ToRightOf(lblAmtPreviouslyPaid, 6),
                            lblAmtPreviouslyPaidValue.AtRightOf(totalView, 5),
                            lblAmtPreviouslyPaidValue.WithSameHeight(lblAmtPreviouslyPaid),
                            lblAmtPreviouslyPaidValue.AtBottomOf(totalView, 15),

                            descriptionBackgroundView.Below(totalView, 14),
                            descriptionBackgroundView.AtLeftOf(scrollableContainer, 20),
                            descriptionBackgroundView.AtRightOf(View, 20),

                            lblDescription.AtTopOf(descriptionBackgroundView, 8),
                            lblDescription.AtLeftOf(descriptionBackgroundView, 8),
                            lblDescription.AtRightOf(descriptionBackgroundView, 8),
                            lblDescription.AtBottomOf(descriptionBackgroundView, 10),

                            btnSubmitClaim.Below(descriptionBackgroundView, 25),
                            btnSubmitClaim.WithSameCenterX(scrollableContainer),
                            btnSubmitClaim.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                            btnSubmitClaim.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                            btnSubmitClaim.AtBottomOf(scrollableContainer, 100));
                    }
                }
            }
            else
            {
                lblDescription.BackgroundColor = Colors.LightGrayColor;
                vewTableTitle.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

                if (referralTypeVisibleNo == 1)
                {
                    View.AddConstraints(
                        scrollableContainer.AtTopOf(View, Constants.TOP_PADDING),
                        scrollableContainer.AtLeftOf(View),
                        scrollableContainer.WithSameWidth(View),
                        scrollableContainer.AtBottomOf(View),

                        lblPlanInformation.AtTopOf(scrollableContainer, 0),
                        lblPlanInformation.AtLeftOf(scrollableContainer, 20),
                        lblPlanInformation.AtRightOf(View, 20),

                        lblGreenShieldId.Below(lblPlanInformation, 12),
                        lblGreenShieldId.AtLeftOf(scrollableContainer, 35),
                        lblGreenShieldId.WithRelativeWidth(scrollableContainer, 0.5f).Minus(20),

                        lblGreenShieldIdValue.Below(lblPlanInformation, 12),
                        lblGreenShieldIdValue.ToRightOf(lblGreenShieldId, 30),
                        lblGreenShieldIdValue.AtRightOf(View, 16),
                        lblGreenShieldIdValue.WithSameHeight(lblGreenShieldId),

                        lblParticipentName.Below(lblGreenShieldId, 9),
                        lblParticipentName.AtLeftOf(scrollableContainer, 35),
                        lblParticipentName.WithSameHeight(lblParticipentNameValue),
                        lblParticipentName.WithRelativeWidth(scrollableContainer, 0.5f).Minus(20),

                        lblParticipentNameValue.Below(lblGreenShieldId, 9),
                        lblParticipentNameValue.ToRightOf(lblParticipentName, 30),
                        lblParticipentNameValue.AtRightOf(View, 20),
                        lblProfessionalName.Below(lblParticipentName, 9),
                        lblProfessionalName.AtLeftOf(scrollableContainer, 35),
                        lblProfessionalName.WithRelativeWidth(scrollableContainer, 0.5f).Minus(20),

                        lblProfessionalNameValue.Below(lblParticipentName, 9),
                        lblProfessionalNameValue.ToRightOf(lblProfessionalName, 30),
                        lblProfessionalNameValue.AtRightOf(View, 20),
                        lblProfessionalNameValue.WithSameHeight(lblProfessionalName),

                        lblClaimDetails.Below(lblProfessionalName, 35),
                        lblClaimDetails.Height().EqualTo(28),
                        lblClaimDetails.AtLeftOf(scrollableContainer, 20),
                        lblClaimDetails.AtRightOf(View, 20),

                        vewTableTitle.Below(lblClaimDetails, 12),
                        vewTableTitle.AtLeftOf(scrollableContainer, 35),
                        vewTableTitle.AtRightOf(View, 20),

                        lblTitleDateOfExpense.AtTopOf(vewTableTitle, 2),
                        lblTitleDateOfExpense.AtLeftOf(vewTableTitle, 2),
                        lblTitleDateOfExpense.Height().EqualTo(18),
                        lblTitleDateOfExpense.Width().EqualTo(150),

                        lblTitleExpenseType.AtTopOf(vewTableTitle, 2),
                        lblTitleExpenseType.ToRightOf(lblTitleDateOfExpense, 15),
                        lblTitleExpenseType.Height().EqualTo(18),
                        lblTitleExpenseType.Width().EqualTo(220),

                        lblTitleAmtPreviouslyPaid.AtTopOf(vewTableTitle, 2),
                        lblTitleAmtPreviouslyPaid.AtRightOf(vewTableTitle, 10),
                        lblTitleAmtPreviouslyPaid.Width().EqualTo(230),
                        lblTitleAmtPreviouslyPaid.AtBottomOf(vewTableTitle, 3),

                        lblTitleClaimAmount.AtTopOf(vewTableTitle, 2),
                        lblTitleClaimAmount.ToLeftOf(lblTitleAmtPreviouslyPaid, 10),
                        lblTitleClaimAmount.Width().EqualTo(150),

                        lblGreenLine.Below(vewTableTitle, 4),
                        lblGreenLine.AtLeftOf(scrollableContainer, 35),
                        lblGreenLine.AtRightOf(View, 20),
                        lblGreenLine.Height().EqualTo(1),

                        claimsTableView.Below(lblGreenLine, 5),
                        claimsTableView.AtLeftOf(scrollableContainer, 35),
                        claimsTableView.AtRightOf(View, 20),
                        claimsTableView.WithSameWidth(scrollableContainer).Minus(50),
                        claimsTableView.Height().EqualTo(tableFrame.Height),

                        lblGrayLine.Below(claimsTableView, 12),
                        lblGrayLine.AtLeftOf(scrollableContainer, 35),
                        lblGrayLine.AtRightOf(View, 20),
                        lblGrayLine.Height().EqualTo(1),

                        totalView.Below(lblGrayLine, 14),
                        totalView.AtLeftOf(scrollableContainer, 35),
                        totalView.AtRightOf(View, 20),

                        lblTotal.AtTopOf(totalView, 2),
                        lblTotal.AtLeftOf(totalView, 2),
                        lblTotal.Height().EqualTo(18),
                        lblTotal.Width().EqualTo(150),

                        lblClaimAmountValue.AtTopOf(totalView, 2),
                        lblClaimAmountValue.WithSameWidth(lblTitleClaimAmount),
                        lblClaimAmountValue.ToLeftOf(lblAmtPreviouslyPaidValue, 15),
                        lblClaimAmountValue.Height().EqualTo(18),

                        lblAmtPreviouslyPaidValue.AtTopOf(totalView, 2),
                        lblAmtPreviouslyPaidValue.WithSameWidth(lblTitleAmtPreviouslyPaid),
                        lblAmtPreviouslyPaidValue.AtRightOf(totalView, 10),
                        lblAmtPreviouslyPaidValue.Height().EqualTo(18),
                        lblAmtPreviouslyPaidValue.AtBottomOf(totalView, 3),

                        descriptionBackgroundView.Below(totalView, 50),
                        descriptionBackgroundView.AtLeftOf(scrollableContainer, 20),
                        descriptionBackgroundView.AtRightOf(View, 20),

                        lblDescription.AtTopOf(descriptionBackgroundView, 16),
                        lblDescription.AtLeftOf(descriptionBackgroundView, 16),
                        lblDescription.AtRightOf(descriptionBackgroundView, 16),
                        lblDescription.AtBottomOf(descriptionBackgroundView, 16),

                        btnSubmitClaim.Below(descriptionBackgroundView, 40),
                        btnSubmitClaim.WithSameCenterX(scrollableContainer),
                        btnSubmitClaim.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                        btnSubmitClaim.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                        btnSubmitClaim.AtBottomOf(scrollableContainer, 100));
                }
                else
                {
                    View.AddConstraints(
                        scrollableContainer.AtTopOf(View, Constants.TOP_PADDING),
                        scrollableContainer.AtLeftOf(View),
                        scrollableContainer.WithSameWidth(View),
                        scrollableContainer.AtBottomOf(View),

                        lblPlanInformation.AtTopOf(scrollableContainer, 0),
                        lblPlanInformation.AtLeftOf(scrollableContainer, 20),
                        lblPlanInformation.AtRightOf(View, 20),
                        lblGreenShieldId.Below(lblPlanInformation, 12),
                        lblGreenShieldId.AtLeftOf(scrollableContainer, 35),
                        lblGreenShieldId.WithRelativeWidth(scrollableContainer, 0.5f).Minus(20),

                        lblGreenShieldIdValue.Below(lblPlanInformation, 12),
                        lblGreenShieldIdValue.ToRightOf(lblGreenShieldId, 30),
                        lblGreenShieldIdValue.AtRightOf(View, 16),
                        lblGreenShieldIdValue.WithSameHeight(lblGreenShieldId),

                        lblParticipentName.Below(lblGreenShieldId, 9),
                        lblParticipentName.AtLeftOf(scrollableContainer, 35),
                        lblParticipentName.WithSameHeight(lblParticipentNameValue),
                        lblParticipentName.WithRelativeWidth(scrollableContainer, 0.5f).Minus(20),

                        lblParticipentNameValue.Below(lblGreenShieldId, 9),
                        lblParticipentNameValue.ToRightOf(lblParticipentName, 30),
                        lblParticipentNameValue.AtRightOf(View, 20),
                        lblProfessionalName.Below(lblParticipentName, 9),
                        lblProfessionalName.AtLeftOf(scrollableContainer, 35),
                        lblProfessionalName.WithRelativeWidth(scrollableContainer, 0.5f).Minus(20),

                        lblProfessionalNameValue.Below(lblParticipentName, 9),
                        lblProfessionalNameValue.ToRightOf(lblProfessionalName, 30),
                        lblProfessionalNameValue.AtRightOf(View, 20),
                        lblProfessionalNameValue.WithSameHeight(lblProfessionalName),

                        lblClaimDetails.Below(lblParticipentNameValue, 35),
                        lblClaimDetails.Height().EqualTo(28),
                        lblClaimDetails.AtLeftOf(scrollableContainer, 20),
                        lblClaimDetails.AtRightOf(View, 20),

                        vewTableTitle.Below(lblClaimDetails, 12),
                        vewTableTitle.AtLeftOf(scrollableContainer, 35),
                        vewTableTitle.AtRightOf(View, 20),

                        lblTitleDateOfExpense.AtTopOf(vewTableTitle, 2),
                        lblTitleDateOfExpense.AtLeftOf(vewTableTitle, 2),
                        lblTitleDateOfExpense.Height().EqualTo(18),
                        lblTitleDateOfExpense.Width().EqualTo(150),

                        lblTitleExpenseType.AtTopOf(vewTableTitle, 2),
                        lblTitleExpenseType.ToRightOf(lblTitleDateOfExpense, 15),
                        lblTitleExpenseType.Height().EqualTo(18),
                        lblTitleExpenseType.Width().EqualTo(220),

                        lblTitleAmtPreviouslyPaid.AtTopOf(vewTableTitle, 2),
                        lblTitleAmtPreviouslyPaid.AtRightOf(vewTableTitle, 10),
                        lblTitleAmtPreviouslyPaid.Width().EqualTo(230),
                        lblTitleAmtPreviouslyPaid.AtBottomOf(vewTableTitle, 3),

                        lblTitleClaimAmount.AtTopOf(vewTableTitle, 2),
                        lblTitleClaimAmount.ToLeftOf(lblTitleAmtPreviouslyPaid, 15),
                        lblTitleClaimAmount.Width().EqualTo(100),

                        lblGreenLine.Below(vewTableTitle, 4),
                        lblGreenLine.AtLeftOf(scrollableContainer, 35),
                        lblGreenLine.AtRightOf(View, 20),
                        lblGreenLine.Height().EqualTo(1),

                        claimsTableView.Below(lblGreenLine, 5),
                        claimsTableView.AtLeftOf(scrollableContainer, 35),
                        claimsTableView.AtRightOf(View, 20),
                        claimsTableView.WithSameWidth(scrollableContainer).Minus(50),
                        claimsTableView.Height().EqualTo(tableFrame.Height),

                        lblGrayLine.Below(claimsTableView, 12),
                        lblGrayLine.AtLeftOf(scrollableContainer, 35),
                        lblGrayLine.AtRightOf(View, 20),
                        lblGrayLine.Height().EqualTo(1),

                        totalView.Below(lblGrayLine, 14),
                        totalView.AtLeftOf(scrollableContainer, 35),
                        totalView.AtRightOf(View, 20),

                        lblTotal.AtTopOf(totalView, 2),
                        lblTotal.AtLeftOf(totalView, 2),
                        lblTotal.Height().EqualTo(18),
                        lblTotal.Width().EqualTo(150),

                        lblClaimAmountValue.AtTopOf(totalView, 2),
                        lblClaimAmountValue.WithSameWidth(lblTitleClaimAmount),
                        lblClaimAmountValue.ToLeftOf(lblAmtPreviouslyPaidValue, 15),
                        lblClaimAmountValue.Height().EqualTo(18),

                        lblAmtPreviouslyPaidValue.AtTopOf(totalView, 2),
                        lblAmtPreviouslyPaidValue.WithSameWidth(lblTitleAmtPreviouslyPaid),
                        lblAmtPreviouslyPaidValue.AtRightOf(totalView, 10),
                        lblAmtPreviouslyPaidValue.Height().EqualTo(18),
                        lblAmtPreviouslyPaidValue.AtBottomOf(totalView, 3),

                        descriptionBackgroundView.Below(totalView, 50),
                        descriptionBackgroundView.AtLeftOf(scrollableContainer, 20),
                        descriptionBackgroundView.AtRightOf(View, 20),

                        lblDescription.AtTopOf(descriptionBackgroundView, 16),
                        lblDescription.AtLeftOf(descriptionBackgroundView, 16),
                        lblDescription.AtRightOf(descriptionBackgroundView, 16),
                        lblDescription.AtBottomOf(descriptionBackgroundView, 16),

                        btnSubmitClaim.Below(descriptionBackgroundView, 40),
                        btnSubmitClaim.WithSameCenterX(scrollableContainer),
                        btnSubmitClaim.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                        btnSubmitClaim.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                        btnSubmitClaim.AtBottomOf(scrollableContainer, 100));
                }

            }
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetConstraints();
        }
    }
}