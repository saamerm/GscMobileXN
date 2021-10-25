using System;
using System.Globalization;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MobileClaims.Core.Converters;
using MobileClaims.Core.Entities.ClaimsHistory;
using MobileClaims.Core.Services;
using MobileClaims.iOS.Converters;
using MvvmCross;
using UIKit;

namespace MobileClaims.iOS.UI.ClaimHistoryComponents
{
    public class ClaimHistoryDetailComponent : UIView
    {
        public ClaimState Result;

        public delegate void EventHandler(ClaimState sender, EventArgs e);

        public EventHandler ShowPaymentInformationEvent;

        private UIScrollView borderScrollView;
        private ClaimForLabel serviceDateLab;
        private ClaimForTxtLabel serviceDateTxt;
        private ClaimForLabel claimFormNumberLab;
        private ClaimForTxtLabel claimFormNumberTxt;
        private ClaimForLabel serviceDescriptionLab;
        private ClaimForTxtLabel serviceDescriptionTxt;

        private ClaimForLabel quantityLab;
        //show only for drug claim
        private ClaimForTxtLabel quantityTxt;

        private ClaimForLabel claimedAmountLab;
        private ClaimForTxtLabel claimedAmountTxt;
        private ClaimForLabel otherpaidAmountLab;
        private ClaimForTxtLabel otherpaidAmountTxt;
        private ClaimForLabel paidAmountLab;
        private ClaimForTxtLabel paidAmountTxt;
        private ClaimForLabel copayDeductibleLab;
        private ClaimForTxtLabel copayDeductibleTxt;
        private ClaimForLabel paymentDateLab;
        private ClaimForTxtLabel paymentDateTxt;
        private ClaimForLabel paidToLab;
        private ClaimForTxtLabel paidToTxt;
        private UILabel lastEmptyLabel;
        private ClaimForLabel eOBLab;
        private NUNITOBOLD12 eOBTxt;
        private string eOBContent = string.Empty;
        private UIStringAttributes AttrStrikeThrough;

        private GSButton paymentButt;
        private UILabel RequiresCOPLabel;
        private GSButton ShowConfirmationOfPaymentViewButton;

        private ClaimForLabel errorMessageLab;

        private ClaimForLabel statusLab;
        private ClaimForTxtLabel statusTxt;

        private bool hasSearchResult;

        private float leftMargin = 10f;
        private float belowMargin = 10f;
        private float leftSidePercentage = 0.55f;

        private float pageIndicatorHeight = 10f;
        private float borderContentTopMargin = 10f;
        private float paymentButtWidth = 60f;
        private float paymentButtHeight = 40f;
        private string language;

        public ClaimHistoryDetailComponent(ClaimState ResultItem, string paymentInfoButtTitle)
        {
            var langService = Mvx.IoCProvider.Resolve<ILanguageService>();
            language = langService.CurrentLanguage;
            Result = new ClaimState();
            Result = ResultItem;

            if (Result.EOBMessages != null)
            {
                int countMessages = Result.EOBMessages.Count;
                if (countMessages > 0)
                {
                    for (int i = 0; i < countMessages; i++)
                    {
                        eOBContent += "\n" + Result.EOBMessages[i].Message;
                    }
                    eOBContent = eOBContent.Substring(1, eOBContent.Length - 1);
                }
            }

            if (Result.IsStricken)
            {
                AttrStrikeThrough = new UIStringAttributes
                {
                    Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON),
                    StrokeWidth = 0,
                    StrikethroughStyle = NSUnderlineStyle.Single,
                    StrikethroughColor = Colors.Black
                };
            }

            borderScrollView = new UIScrollView();
            borderScrollView.ScrollEnabled = true;
            borderScrollView.Layer.BorderColor = Colors.LightGrayColor.CGColor;
            borderScrollView.Layer.BorderWidth = 1f;
            Add(borderScrollView);

            paymentDateLab = new ClaimForLabel();
            paymentDateLab.Text = Result.PaymentDateLabel;
            borderScrollView.AddSubview(paymentDateLab);

            paymentDateTxt = new ClaimForTxtLabel();
            if (Result.IsStricken)
            {
                var textAttributed = new NSMutableAttributedString(Result.ProcessedDateAsString, AttrStrikeThrough);
                paymentDateTxt.AttributedText = textAttributed;
            }
            else
            {
                paymentDateTxt.Text = Result.ProcessedDateAsString;
            }
            borderScrollView.AddSubview(paymentDateTxt);


            if (Result.IsServiceDateVisible)
            {
                serviceDateLab = new ClaimForLabel();
                serviceDateLab.Text = Result.ServiceDateLabel;//model.ServiceDate;
                borderScrollView.AddSubview(serviceDateLab);

                serviceDateTxt = new ClaimForTxtLabel();
                if (Result.IsStricken)
                {
                    var textAttributed = new NSMutableAttributedString(Result.ServiceDate.ToShortDateString(), AttrStrikeThrough);
                    serviceDateTxt.AttributedText = textAttributed;
                }
                else
                {
                    serviceDateTxt.Text = Result.ServiceDate.ToShortDateString();
                }

                borderScrollView.AddSubview(serviceDateTxt);

                claimFormNumberLab = new ClaimForLabel();
                claimFormNumberLab.Text = Result.ClaimFormNumberLabel;
                borderScrollView.AddSubview(claimFormNumberLab);

                claimFormNumberTxt = new ClaimForTxtLabel();
                if (Result.IsStricken)
                {
                    var textAttributed = new NSMutableAttributedString(Result.ClaimFormID.ToString(), AttrStrikeThrough);
                    claimFormNumberTxt.AttributedText = textAttributed;
                }
                else
                {
                    claimFormNumberTxt.Text = Result.ClaimFormID.ToString();
                }
                borderScrollView.AddSubview(claimFormNumberTxt);

                serviceDescriptionLab = new ClaimForLabel();
                serviceDescriptionLab.Text = Result.ServiceDescriptionLabel;
                borderScrollView.AddSubview(serviceDescriptionLab);

                serviceDescriptionTxt = new ClaimForTxtLabel();
                if (Result.IsStricken)
                {
                    var textAttributed = new NSMutableAttributedString(Result.ServiceDescription, AttrStrikeThrough);
                    serviceDescriptionTxt.AttributedText = textAttributed;
                }
                else
                {
                    serviceDescriptionTxt.Text = Result.ServiceDescription;
                }
                borderScrollView.AddSubview(serviceDescriptionTxt);

                if (Result.IsQuantityVisible)
                {

                    quantityLab = new ClaimForLabel();
                    quantityLab.Text = Result.QuantityLabel;
                    borderScrollView.AddSubview(quantityLab);

                    quantityTxt = new ClaimForTxtLabel();
                    if (Result.IsStricken)
                    {
                        var textAttributed = new NSMutableAttributedString(Result.Quantity.ToString(), AttrStrikeThrough);
                        quantityTxt.AttributedText = textAttributed;
                    }
                    else
                    {
                        quantityTxt.Text = Result.Quantity.ToString();
                    }
                    borderScrollView.AddSubview(quantityTxt);
                }

                claimedAmountLab = new ClaimForLabel();
                claimedAmountLab.Text = Result.ClaimedAmountLabel;
                borderScrollView.AddSubview(claimedAmountLab);

                claimedAmountTxt = new ClaimForTxtLabel();
                if (Result.IsStricken)
                {
                    var textAttributed = new NSMutableAttributedString("$" + Result.ClaimedAmount, AttrStrikeThrough);
                    claimedAmountTxt.AttributedText = textAttributed;
                }
                else
                {
                    claimedAmountTxt.Text = "$" + Result.ClaimedAmount;
                }
                borderScrollView.AddSubview(claimedAmountTxt);

                otherpaidAmountLab = new ClaimForLabel();
                otherpaidAmountLab.Text = Result.OtherPaidAmountLabel;
                borderScrollView.AddSubview(otherpaidAmountLab);

                otherpaidAmountTxt = new ClaimForTxtLabel();
                if (Result.IsStricken)
                {
                    var textAttributed = new NSMutableAttributedString("$" + Result.OtherPaidAmount, AttrStrikeThrough);
                    otherpaidAmountTxt.AttributedText = textAttributed;
                }
                else
                {
                    otherpaidAmountTxt.Text = "$" + Result.OtherPaidAmount;
                }
                borderScrollView.AddSubview(otherpaidAmountTxt);

                paidAmountLab = new ClaimForLabel();
                paidAmountLab.Text = Result.PaidAmountLabel;
                borderScrollView.AddSubview(paidAmountLab);

                paidAmountTxt = new ClaimForTxtLabel();
                string paidAmountStr = Result.PaidAmount.ToString();
                if (!string.IsNullOrEmpty(paidAmountStr))
                    paidAmountStr = "$" + paidAmountStr;
                if (Result.IsStricken)
                {
                    var textAttributed = new NSMutableAttributedString(paidAmountStr, AttrStrikeThrough);
                    paidAmountTxt.AttributedText = textAttributed;
                }
                else
                {
                    paidAmountTxt.Text = paidAmountStr;
                }
                borderScrollView.AddSubview(paidAmountTxt);

                copayDeductibleLab = new ClaimForLabel();
                copayDeductibleLab.Text = Result.CopayLabel;
                borderScrollView.AddSubview(copayDeductibleLab);

                copayDeductibleTxt = new ClaimForTxtLabel();
                string copayAmountStr = Result.CopayAmount.ToString();
                if (!string.IsNullOrEmpty(copayAmountStr))
                    copayAmountStr = "$" + copayAmountStr;
                if (Result.IsStricken)
                {
                    var textAttributed = new NSMutableAttributedString(copayAmountStr, AttrStrikeThrough);
                    copayDeductibleTxt.AttributedText = textAttributed;
                }
                else
                {
                    copayDeductibleTxt.Text = copayAmountStr;
                }
                borderScrollView.AddSubview(copayDeductibleTxt);

                paidToLab = new ClaimForLabel();
                paidToLab.Text = Result.PaidToLabel;
                borderScrollView.AddSubview(paidToLab);

                paidToTxt = new ClaimForTxtLabel();
                if (Result.IsStricken)
                {
                    var textAttributed = new NSMutableAttributedString(Result.Payee.Name, AttrStrikeThrough);
                    paidToTxt.AttributedText = textAttributed;
                }
                else
                {
                    paidToTxt.Text = Result.Payee.Name;
                }
                borderScrollView.AddSubview(paidToTxt);

                eOBLab = new ClaimForLabel();
                eOBLab.Text = Result.EOBLabel;
                borderScrollView.AddSubview(eOBLab);

                eOBTxt = new NUNITOBOLD12();
                eOBTxt.Text = eOBContent;
                eOBTxt.TextContent = eOBContent;
                borderScrollView.AddSubview(eOBTxt);
            }
            else
            {
                statusLab = new ClaimForLabel();
                statusLab.Text = Result.ClaimStatusLabel;
                borderScrollView.AddSubview(statusLab);

                statusTxt = new ClaimForTxtLabel();
                if (Result.IsStricken)
                {
                    var textAttributed = new NSMutableAttributedString(Result.Status, AttrStrikeThrough);
                    statusTxt.AttributedText = textAttributed;
                }
                else
                {
                    statusTxt.Text = Result.Status;
                }
                borderScrollView.AddSubview(statusTxt);
            }

            if (Result.Payment != null)
            {
                paymentButt = new GSButton();
                paymentButt.TouchUpInside += PayMentTouch;
                paymentButt.SetTitle(paymentInfoButtTitle, UIControlState.Normal);
                borderScrollView.AddSubview(paymentButt);
            }

            RequiresCOPLabel = new UILabel();
            var copToInfoConverter = new RequiresActionToTextConverter();
            var requiresActionToTextColorConverter = new RequiresActionToTextColorConverter();
            RequiresCOPLabel.Text = copToInfoConverter.Convert(Result.ClaimActionStatus, null, null, CultureInfo.CurrentCulture) as string;
            RequiresCOPLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_FONT_SIZE);
            RequiresCOPLabel.TextColor = (UIColor)requiresActionToTextColorConverter.Convert(Result.ClaimActionStatus, null, null, CultureInfo.CurrentUICulture);
            RequiresCOPLabel.Lines = 0;
            RequiresCOPLabel.LineBreakMode = UILineBreakMode.WordWrap;
            RequiresCOPLabel.TextAlignment = UITextAlignment.Center;
            RequiresCOPLabel.UserInteractionEnabled = false;
            RequiresCOPLabel.BackgroundColor = Colors.Clear;
            borderScrollView.AddSubview(RequiresCOPLabel);

            ShowConfirmationOfPaymentViewButton = new GSButton();
            ShowConfirmationOfPaymentViewButton.SetTitle(Result.UploadDocumentsLabel, UIControlState.Normal);
            ShowConfirmationOfPaymentViewButton.TouchUpInside += ShowConfirmationOfPaymentViewButton_TouchUpInside;
            borderScrollView.AddSubview(ShowConfirmationOfPaymentViewButton);

            lastEmptyLabel = new UILabel();
            borderScrollView.AddSubview(lastEmptyLabel);

            SetConstraints();
        }

        private void ShowConfirmationOfPaymentViewButton_TouchUpInside(object sender, EventArgs e)
        {
            Result.UploadDocumentsCommand.Execute();
            ShowConfirmationOfPaymentViewButton.TouchUpInside -= ShowConfirmationOfPaymentViewButton_TouchUpInside;
        }

        private void SetConstraints()
        {
            if (Result.Payment == null)
            {
                paymentButtHeight = 0f;
            }

            this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            borderScrollView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            this.AddConstraints(
                borderScrollView.AtLeftOf(this, leftMargin),
                borderScrollView.WithSameWidth(this).Minus(leftMargin * 2),
                borderScrollView.AtTopOf(this, belowMargin),
                borderScrollView.AtBottomOf(this));

            if (Result.IsServiceDateVisible)  //Constraints for No PDT and MI Types
            {

                this.AddConstraints(
                    serviceDateLab.AtTopOf(borderScrollView, borderContentTopMargin),
                    serviceDateLab.AtLeftOf(borderScrollView, leftMargin),
                    serviceDateLab.WithRelativeWidth(borderScrollView, leftSidePercentage).Minus(leftMargin),

                    serviceDateTxt.WithSameTop(serviceDateLab).Plus(4),
                    serviceDateTxt.ToRightOf(serviceDateLab),
                    serviceDateTxt.WithRelativeWidth(borderScrollView, (1 - leftSidePercentage)).Minus(leftMargin),


                    claimFormNumberLab.WithSameLeft(serviceDateLab),
                    claimFormNumberLab.WithSameWidth(serviceDateLab),
                    claimFormNumberLab.Below(serviceDateLab, belowMargin).Plus(4),

                    claimFormNumberTxt.ToRightOf(claimFormNumberLab),
                    claimFormNumberTxt.WithRelativeWidth(borderScrollView, (1 - leftSidePercentage)).Minus(leftMargin),
                    claimFormNumberTxt.WithSameTop(claimFormNumberLab).Plus(4),

                    serviceDescriptionLab.WithSameLeft(serviceDateLab),
                    serviceDescriptionLab.WithSameWidth(serviceDateLab),
                    serviceDescriptionLab.Below(claimFormNumberLab, belowMargin),

                    serviceDescriptionTxt.ToRightOf(serviceDescriptionLab),
                    serviceDescriptionTxt.WithRelativeWidth(borderScrollView, (1 - leftSidePercentage)).Minus(leftMargin),
                    serviceDescriptionTxt.Below(claimFormNumberLab, belowMargin).Plus(4)
                );
                if (Result.IsQuantityVisible)
                {
                    this.AddConstraints(
                        quantityLab.WithSameLeft(serviceDateLab),
                        quantityLab.WithSameWidth(serviceDateLab),
                        quantityLab.Below(serviceDescriptionTxt, belowMargin),

                        quantityTxt.ToRightOf(quantityLab),
                        quantityTxt.WithRelativeWidth(borderScrollView, (1 - leftSidePercentage)).Minus(leftMargin),
                        quantityTxt.WithSameTop(quantityLab),

                        claimedAmountLab.WithSameLeft(serviceDateLab),
                        claimedAmountLab.WithSameWidth(serviceDateLab),
                        claimedAmountLab.Below(quantityLab, belowMargin)
                    );
                }
                else
                {

                    this.AddConstraints(
                        claimedAmountLab.WithSameLeft(serviceDateLab),
                        claimedAmountLab.WithSameWidth(serviceDateLab),
                        claimedAmountLab.Below(serviceDescriptionTxt, belowMargin)
                    );
                }

                this.AddConstraints(
                    claimedAmountTxt.ToRightOf(claimedAmountLab),
                    claimedAmountTxt.WithRelativeWidth(borderScrollView, (1 - leftSidePercentage)).Minus(leftMargin),
                    claimedAmountTxt.WithSameTop(claimedAmountLab).Plus(4),

                    otherpaidAmountLab.WithSameLeft(serviceDateLab),
                    otherpaidAmountLab.WithSameWidth(serviceDateLab),
                    otherpaidAmountLab.Below(claimedAmountLab, belowMargin),

                    otherpaidAmountTxt.ToRightOf(otherpaidAmountLab),
                    otherpaidAmountTxt.WithRelativeWidth(borderScrollView, (1 - leftSidePercentage)).Minus(leftMargin),
                    otherpaidAmountTxt.WithSameTop(otherpaidAmountLab).Plus(4),

                    paidAmountLab.WithSameLeft(serviceDateLab),
                    paidAmountLab.WithSameWidth(serviceDateLab),
                    paidAmountLab.Below(otherpaidAmountLab, belowMargin),

                    paidAmountTxt.ToRightOf(paidAmountLab),
                    paidAmountTxt.WithRelativeWidth(borderScrollView, (1 - leftSidePercentage)).Minus(leftMargin),
                    paidAmountTxt.WithSameTop(paidAmountLab).Plus(4),

                    copayDeductibleLab.WithSameLeft(serviceDateLab),
                    copayDeductibleLab.WithSameWidth(serviceDateLab),
                    copayDeductibleLab.Below(paidAmountLab, belowMargin),

                    copayDeductibleTxt.ToRightOf(copayDeductibleLab),
                    copayDeductibleTxt.WithRelativeWidth(borderScrollView, (1 - leftSidePercentage)).Minus(leftMargin),
                    copayDeductibleTxt.WithSameTop(copayDeductibleLab).Plus(4),

                    paymentDateLab.WithSameLeft(serviceDateLab),
                    paymentDateLab.WithSameWidth(serviceDateLab),
                    paymentDateLab.Below(copayDeductibleLab, belowMargin),

                    paymentDateTxt.ToRightOf(paymentDateLab),
                    paymentDateTxt.WithRelativeWidth(borderScrollView, (1 - leftSidePercentage)).Minus(leftMargin),
                    paymentDateTxt.WithSameTop(paymentDateLab).Plus(4),

                    paidToLab.WithSameLeft(serviceDateLab),
                    paidToLab.WithSameWidth(serviceDateLab));

                if (Constants.IsPhone())
                {
                    if (Helpers.IsInPortraitMode())
                    {
                        if (language.Contains("fr") || language.Contains("Fr"))
                        {
                            if (Result.ProcessedDateAsString.Contains("En cours de"))
                            {
                                this.AddConstraints(paidToLab.Below(paymentDateTxt, belowMargin));
                            }
                            else
                            {
                                this.AddConstraints(paidToLab.Below(paymentDateLab, belowMargin));
                            }
                        }
                        else
                        {
                            this.AddConstraints(paidToLab.Below(paymentDateLab, belowMargin));
                        }

                    }
                    else
                    {
                        this.AddConstraints(paidToLab.Below(paymentDateLab, belowMargin));
                    }
                }
                else
                {
                    this.AddConstraints(paidToLab.Below(paymentDateLab, belowMargin));
                }

                this.AddConstraints(
                    paidToTxt.ToRightOf(paidToLab),
                    paidToTxt.WithRelativeWidth(borderScrollView, (1 - leftSidePercentage)).Minus(leftMargin),
                    paidToTxt.WithSameTop(paidToLab).Plus(4),

                    eOBLab.WithSameLeft(paidToLab),
                    eOBLab.WithSameWidth(paidToLab),
                    eOBLab.Below(paidToLab, belowMargin),

                    eOBTxt.ToRightOf(eOBLab),
                    eOBTxt.WithRelativeWidth(borderScrollView, (1 - leftSidePercentage)).Minus(leftMargin),
                    eOBTxt.WithSameTop(eOBLab).Plus(4)
                );

            }
            else  //Constraints for PDT and MI Types
            {
                this.AddConstraints(
                    paymentDateLab.AtTopOf(borderScrollView, borderContentTopMargin),
                    paymentDateLab.AtLeftOf(borderScrollView, leftMargin),
                    paymentDateLab.WithRelativeWidth(borderScrollView, leftSidePercentage).Minus(leftMargin),

                    paymentDateTxt.ToRightOf(paymentDateLab),
                    paymentDateTxt.WithRelativeWidth(borderScrollView, (1 - leftSidePercentage)).Minus(leftMargin),
                    paymentDateTxt.WithSameTop(paymentDateLab).Plus(4),

                    statusLab.WithSameLeft(paymentDateLab),
                    statusLab.WithSameWidth(paymentDateLab),
                    statusLab.Below(paymentDateLab, belowMargin),

                    statusTxt.ToRightOf(statusLab),
                    statusTxt.WithRelativeWidth(borderScrollView, (1 - leftSidePercentage)).Minus(leftMargin),
                    statusTxt.WithSameTop(statusLab).Plus(4)
                );
            }
            if (Result.Payment != null)
            {
                this.AddConstraints(
                    paymentButt.WithSameCenterX(borderScrollView),
                    paymentButt.WithSameWidth(borderScrollView).Minus(leftMargin),
                    paymentButt.Height().EqualTo(paymentButtHeight),

                    lastEmptyLabel.AtLeftOf(paymentButt),
                    lastEmptyLabel.WithSameWidth(paymentButt),
                    lastEmptyLabel.Height().EqualTo(belowMargin),
                    lastEmptyLabel.Below(paymentButt)
                );
                if (string.IsNullOrEmpty(eOBContent))
                {
                    this.AddConstraints(paymentButt.Below(eOBLab, belowMargin));
                }
                else
                {
                    int lineLength = Helpers.IsInLandscapeMode() ? Constants.EOBMESSAGELIMATELENGTH_LANDSCAPE : Constants.EOBMESSAGELIMATELENGTH_PORTRAIT;
                    if (eOBContent.Length <= lineLength)
                    {
                        this.AddConstraints(paymentButt.Below(eOBLab, belowMargin));
                    }
                    else
                    {
                        this.AddConstraints(paymentButt.Below(eOBTxt, belowMargin));
                    }
                }

            }
            else
            {
                if (Result.IsServiceDateVisible)
                {
                    this.AddConstraints(
                        lastEmptyLabel.AtLeftOf(borderScrollView),
                        lastEmptyLabel.WithSameWidth(borderScrollView),
                        lastEmptyLabel.Height().EqualTo(belowMargin)
                    );

                    if (string.IsNullOrEmpty(eOBContent))
                    {
                        this.AddConstraints(lastEmptyLabel.Below(eOBLab));
                    }
                    else
                    {
                        this.AddConstraints(lastEmptyLabel.Below(eOBTxt));
                    }

                }
                else
                {
                    this.AddConstraints(
                        lastEmptyLabel.AtLeftOf(borderScrollView),
                        lastEmptyLabel.WithSameWidth(borderScrollView),
                        lastEmptyLabel.Height().EqualTo(belowMargin),
                        lastEmptyLabel.Below(statusLab)
                    );
                }

            }
            this.AddConstraints(
                        RequiresCOPLabel.WithSameTop(lastEmptyLabel).Plus(30),
                        RequiresCOPLabel.WithSameCenterX(borderScrollView),
                        RequiresCOPLabel.WithSameWidth(borderScrollView).Minus(40),

                        ShowConfirmationOfPaymentViewButton.Below(RequiresCOPLabel, belowMargin).Plus(4),
                        ShowConfirmationOfPaymentViewButton.WithSameCenterX(borderScrollView),
                        ShowConfirmationOfPaymentViewButton.WithSameWidth(borderScrollView).Minus(leftMargin),
                        ShowConfirmationOfPaymentViewButton.Height().GreaterThanOrEqualTo(40),
                        ShowConfirmationOfPaymentViewButton.AtBottomOf(borderScrollView, borderContentTopMargin));
        }

        private void PayMentTouch(object sender, EventArgs e)
        {
            ShowPaymentInformationEvent?.Invoke(Result, e);
        }

        protected override void Dispose(bool disposing)
        {
            if (paymentButt != null)
            {
                paymentButt.TouchUpInside -= PayMentTouch;
            }
            base.Dispose(disposing);
        }
    }
}