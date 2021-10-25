using System;
using CoreGraphics;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS
{
    [Foundation.Register("AuditInformationView")]
    public class AuditInformationView: GSCBaseViewPaddingController, IGSCBaseViewImplementor
	{
		protected AuditInformationViewModel _model;

		protected UIScrollView scrollContainer;

		UIBarButtonItem doneButton;

		UIView auditContainer;
		UIView auditInfoContainer;
		UILabel formReferenceLabel;
		UILabel auditNotificationLabel;
		protected GSButton auditInstructionsButton;

		private ClaimFieldResultDisplay gscIDResult;
		private ClaimFieldResultDisplay participantNameResult;
		private ClaimFieldResultDisplay submissionDateResult;

        UIView vewSubscription;
        UILabel planInformation;

		protected GSButton submitAnotherButton;

		protected UITableView tableView;

		private float BUTTON_WIDTH = Constants.IsPhone() ? 225 : 250;
		private float BUTTON_HEIGHT = Constants.IsPhone() ? 40 : 60;

		protected UILabel awaitingPaymentNote;

		protected UIScrollView scrollableContainer;

		private ClaimAuditModal auditInstructionsController;

		public AuditInformationView ()
		{
		}

		public override void ViewDidLoad ()
		{
            View = new GSCBaseView () { BackgroundColor = Colors.BACKGROUND_COLOR };

			base.ViewDidLoad ();
			base.NavigationItem.SetHidesBackButton (false, false);
			base.NavigationItem.Title = "auditNotificationTitle".tr();

			if (Constants.IS_OS_7_OR_LATER()) {
				base.NavigationController.NavigationBar.TintColor = Colors.HIGHLIGHT_COLOR;
				base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
				base.NavigationController.View.BackgroundColor = Colors.BACKGROUND_COLOR;
			} else {
				base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
			}

			_model = (AuditInformationViewModel)this.ViewModel;

			scrollContainer = new UIScrollView ();
			scrollContainer.BackgroundColor = Colors.Clear;
			((GSCBaseView)View).baseContainer.AddSubview(scrollContainer);

			//			doneButton = new UIBarButtonItem();
			//			doneButton.Style = UIBarButtonItemStyle.Plain;
			//			doneButton.Clicked += HandleDone;
			//			doneButton.Title = "done".tr ();
			//			doneButton.TintColor = Colors.HIGHLIGHT_COLOR;
			//			UITextAttributes attributes = new UITextAttributes ();
			//			attributes.Font = UIFont.FromName (Constants.NUNITO_REGULAR, 18.0f);
			//			doneButton.SetTitleTextAttributes (attributes, UIControlState.Normal);
			//			base.NavigationItem.RightBarButtonItem = doneButton;

			if(Constants.IS_OS_7_OR_LATER())
				this.AutomaticallyAdjustsScrollViewInsets = false;


			auditContainer = new UIView ();
			scrollContainer.AddSubview (auditContainer);

			auditInfoContainer = new UIView ();
			auditInfoContainer.BackgroundColor = Colors.LightGrayColor;
			auditContainer.AddSubview (auditInfoContainer);

			formReferenceLabel = new UILabel();
			formReferenceLabel.TextColor = Colors.DARK_GREY_COLOR;
			formReferenceLabel.BackgroundColor = Colors.Clear;
			formReferenceLabel.TextAlignment = UITextAlignment.Center;
			formReferenceLabel.LineBreakMode = UILineBreakMode.WordWrap;
			formReferenceLabel.Lines = 0;
			formReferenceLabel.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
			auditInfoContainer.AddSubview(formReferenceLabel);

			auditNotificationLabel = new UILabel();
			auditNotificationLabel.TextColor = Colors.DARK_GREY_COLOR;
			auditNotificationLabel.BackgroundColor = Colors.Clear;
			auditNotificationLabel.TextAlignment = UITextAlignment.Center;
			auditNotificationLabel.LineBreakMode = UILineBreakMode.WordWrap;
			auditNotificationLabel.Lines = 0;
			auditNotificationLabel.Text = "auditNotification".tr ();
			auditNotificationLabel.Font = UIFont.FromName (Constants.NUNITO_BLACK, (nfloat)Constants.HEADING_FONT_SIZE);
			auditInfoContainer.AddSubview(auditNotificationLabel);

			auditInstructionsButton = new GSButton ();
			auditInstructionsButton.SetTitle ("auditInstructionsButton".tr(), UIControlState.Normal);
			auditInstructionsButton.TouchUpInside += HandleAuditInstructions;
			auditContainer.AddSubview  (auditInstructionsButton);

			auditContainer.Alpha = 1;

            // Plan Information Title
            planInformation = new UILabel();
            planInformation.TextColor = Colors.DARK_GREY_COLOR;
            planInformation.BackgroundColor = Colors.Clear;
            planInformation.TextAlignment = UITextAlignment.Left;
            planInformation.Text = "planInformationTitle".tr();
            planInformation.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.LG_HEADING_FONT_SIZE);
            scrollContainer.AddSubview(planInformation);

			gscIDResult = new ClaimFieldResultDisplay ("greenShieldId".FormatWithBrandKeywords(LocalizableBrand.GreenShield), _model.ClaimResult.PlanMemberDisplayID,false);
			scrollContainer.AddSubview (gscIDResult);

			participantNameResult = new ClaimFieldResultDisplay ("participantName".tr (), _model.ClaimResult.ParticipantFullName,false);
			scrollContainer.AddSubview (participantNameResult);

			submissionDateResult = new ClaimFieldResultDisplay ("submissionDate".tr (), _model.ClaimResult.SubmissionDate.ToShortDateString(),false);
			scrollContainer.AddSubview (submissionDateResult);

			submitAnotherButton = new GSButton ();
			submitAnotherButton.SetTitle ("done".tr(), UIControlState.Normal);
			scrollContainer.AddSubview  (submitAnotherButton);

            vewSubscription = new UIView();
            scrollContainer.AddSubview(vewSubscription);          

            // Lets loop through all the claim results to plot the values
            int tag = 100;
            double total = 0;
            for (int i = 0; i < _model.ClaimResult.ClaimResultDetails.Count;i++)
            {
                ClaimResultDetailGSC detailGSC = _model.ClaimResult.ClaimResultDetails[i];

                ClaimFieldResultDisplay serviceDate = new ClaimFieldResultDisplay(detailGSC.HasHCSADetails ? "dateOfExpense".tr() : "serviceDate".tr(), detailGSC.ServiceDate.ToShortDateString(),false);
                vewSubscription.AddSubview(serviceDate);
                serviceDate.Tag = tag++;

                ClaimFieldResultDisplay serviceDes = new ClaimFieldResultDisplay(detailGSC.HasHCSADetails ? "typeOfExpense".tr() : "serviceDescription".tr(), detailGSC.ServiceDescription,false);
                vewSubscription.AddSubview(serviceDes);
                serviceDes.Tag = tag++;

                string value = "$" + detailGSC.ClaimedAmount.ToString();
                ClaimFieldResultDisplay claimedAmt = new ClaimFieldResultDisplay("claimedAmount".tr(), value,false);
                vewSubscription.AddSubview(claimedAmt);
                claimedAmt.Tag = tag++;
                total += detailGSC.ClaimedAmount;
            }
            string tValue = "$"+total.ToString();
            ClaimFieldResultDisplay totalTitle = new ClaimFieldResultDisplay("totals".tr(),tValue,true);
            vewSubscription.AddSubview(totalTitle);
            totalTitle.Tag = tag++;

			var set = this.CreateBindingSet<AuditInformationView, AuditInformationViewModel> ();
			set.Bind(this.formReferenceLabel).To(item => item.ClaimResult.ClaimFormID).WithConversion("FormNumberLongPrefix").OneWay();		
			set.Bind (this.submitAnotherButton).To (vm => vm.DoneCommand);
			set.Apply ();

		}

		void HandleAuditInstructions (object sender, EventArgs e)
		{
			float auditControllerYPos = (Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING);

			auditInstructionsController = new ClaimAuditModal ();


			//auditInstructionsController.View = new UIView (new CGRect (0, 0, 5, 5));
            auditInstructionsController.View = new GSCBaseView();
			auditInstructionsController.View.BackgroundColor = Colors.BACKGROUND_COLOR;

			auditInstructionsController.scrollContainer = new UIScrollView ();
			auditInstructionsController.scrollContainer.BackgroundColor = Colors.Clear;
			auditInstructionsController.View.AddSubview (auditInstructionsController.scrollContainer);

            auditInstructionsController.auditHeadingLabel = new UILabel();
            auditInstructionsController.auditHeadingLabel.BackgroundColor = Colors.Clear;
            auditInstructionsController.auditHeadingLabel.TextColor = Colors.HIGHLIGHT_COLOR;
            auditInstructionsController.auditHeadingLabel.BackgroundColor = Colors.Clear;
            auditInstructionsController.auditHeadingLabel.TextAlignment = UITextAlignment.Center;
            auditInstructionsController.auditHeadingLabel.LineBreakMode = UILineBreakMode.WordWrap;
            auditInstructionsController.auditHeadingLabel.Lines = 0;
            auditInstructionsController.auditHeadingLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            auditInstructionsController.auditHeadingLabel.Text = "auditNotificationInstruction".tr();
            auditInstructionsController.scrollContainer.AddSubview(auditInstructionsController.auditHeadingLabel);

			auditInstructionsController.auditInstructionsTextArea = new UILabel ();
			auditInstructionsController.auditInstructionsTextArea.BackgroundColor = Colors.Clear;
			auditInstructionsController.auditInstructionsTextArea.TextColor = Colors.DARK_GREY_COLOR;
			auditInstructionsController.auditInstructionsTextArea.BackgroundColor = Colors.Clear;
			auditInstructionsController.auditInstructionsTextArea.TextAlignment = UITextAlignment.Left;
			auditInstructionsController.auditInstructionsTextArea.LineBreakMode = UILineBreakMode.WordWrap;
			auditInstructionsController.auditInstructionsTextArea.Lines = 0;
			auditInstructionsController.auditInstructionsTextArea.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
			auditInstructionsController.auditInstructionsTextArea.Text = "auditInstructions".FormatWithBrandKeywords(LocalizableBrand.GSC);
			auditInstructionsController.scrollContainer.AddSubview (auditInstructionsController.auditInstructionsTextArea);

			auditInstructionsController.dismissButton = new GSButton ();
			auditInstructionsController.dismissButton.SetTitle ("ok".tr(), UIControlState.Normal);
			auditInstructionsController.scrollContainer.AddSubview (auditInstructionsController.dismissButton);
			auditInstructionsController.dismissButton.TouchUpInside += HandleDismissButton;

			UIWindow keyWindow = UIApplication.SharedApplication.KeyWindow;

			auditInstructionsController.ModalInPopover = true;
			auditInstructionsController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
			keyWindow.RootViewController.PresentViewController(auditInstructionsController, true, null);
		}

		void HandleDismissButton (object sender, EventArgs e)
		{
			auditInstructionsController.DismissViewController (true, null);
		}

		int redrawCount = 0;
		public override void ViewDidLayoutSubviews ()
		{
			base.ViewDidLayoutSubviews ();

            float viewWidth = (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            float viewHeight = (float)((GSCBaseView)View).baseContainer.Bounds.Height - Helpers.BottomNavHeight();
			float contentWidth = (float)this.View.Frame.Width;
			float tableWidth = contentWidth;
			float itemPadding = 10;

            float yPos = itemPadding;
            float startY = ViewContentYPositionPadding;
            float extraPos = (Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING);
			float auditYPos = itemPadding;
			float auditInfoYPos = itemPadding;

			float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;

			scrollContainer.Frame =  new CGRect (0, 0, viewWidth , viewHeight);

            float auditWidth = viewWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 4;
            float auditInnerWidth = viewWidth/3 * 2;
            float xPos = viewWidth / 2 - auditWidth / 2;
			float innerXPos = auditWidth/2 - auditInnerWidth/2;

			formReferenceLabel.Frame = new CGRect (auditWidth/2 - (float)formReferenceLabel.Frame.Width/2, auditInfoYPos, auditWidth, (float)formReferenceLabel.Frame.Height);
			formReferenceLabel.SizeToFit ();

			auditInfoYPos += (float)formReferenceLabel.Frame.Height + itemPadding;

			auditNotificationLabel.Frame = new CGRect (innerXPos, auditInfoYPos, auditInnerWidth, (float)auditNotificationLabel.Frame.Height);
			auditNotificationLabel.SizeToFit ();

			auditInfoYPos += (float)auditNotificationLabel.Frame.Height + itemPadding;

			auditInfoContainer.Frame = new CGRect (xPos, auditYPos, auditWidth, auditInfoYPos);

			auditYPos += (float)auditInfoContainer.Frame.Height + itemPadding;

			auditInstructionsButton.Frame = new CGRect (contentWidth/2 - BUTTON_WIDTH/2, auditYPos, BUTTON_WIDTH, BUTTON_HEIGHT);

            auditYPos += (float)(auditInstructionsButton.Frame.Height + itemPadding);

            auditContainer.Frame = new CGRect (0, startY, contentWidth, auditYPos);

            yPos = startY + (float)auditContainer.Frame.Height;

			gscIDResult.Frame = new CGRect (0, yPos, viewWidth, gscIDResult.ComponentHeight);
            yPos += (float)gscIDResult.ComponentHeight + 3;
			participantNameResult.Frame = new CGRect (0, yPos, viewWidth, participantNameResult.ComponentHeight);
            yPos += (float)participantNameResult.ComponentHeight;
            submissionDateResult.Frame = new CGRect (0, yPos, viewWidth, submissionDateResult.ComponentHeight);
            yPos += ((float)submissionDateResult.ComponentHeight + 2 * itemPadding) + 3;

            float vewY = 0;
            int tag = 100;
            for (int i = 0; i < _model.ClaimResult.ClaimResultDetails.Count; i++)
            {
                ClaimResultDetailGSC detailGSC = _model.ClaimResult.ClaimResultDetails[i];

                ClaimFieldResultDisplay serviceDate = (ClaimFieldResultDisplay)vewSubscription.ViewWithTag(tag++);
                serviceDate.Frame = new CGRect(0, vewY, viewWidth, serviceDate.ComponentHeight);
                vewY += serviceDate.ComponentHeight + 3;

                ClaimFieldResultDisplay serviceDes = (ClaimFieldResultDisplay)vewSubscription.ViewWithTag(tag++);
                serviceDes.Frame = new CGRect(0, vewY, viewWidth, serviceDes.ComponentHeight);
                vewY += serviceDes.ComponentHeight + 3;

                ClaimFieldResultDisplay claimedAmt = (ClaimFieldResultDisplay)vewSubscription.ViewWithTag(tag++);
                claimedAmt.Frame = new CGRect(0, vewY, viewWidth, claimedAmt.ComponentHeight);
                vewY += (claimedAmt.ComponentHeight + 2*itemPadding) + 3;
            }
            ClaimFieldResultDisplay total = (ClaimFieldResultDisplay)vewSubscription.ViewWithTag(tag);
            total.Frame = new CGRect(0, vewY, viewWidth, total.ComponentHeight);
            vewY += (total.ComponentHeight + itemPadding) + 3;

            vewSubscription.Frame = new CGRect(0, yPos, viewWidth, vewY);
            yPos += vewY;

            submitAnotherButton.Frame = new CGRect(viewWidth / 2 - BUTTON_WIDTH / 2, yPos, BUTTON_WIDTH, BUTTON_HEIGHT);
            yPos += (float)submitAnotherButton.Frame.Height + itemPadding ;

            scrollContainer.ContentSize = new CGSize(viewWidth, yPos + GetBottomPadding());


			if (redrawCount < 3) {
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

				if (value) {
					auditContainer.Alpha = 1;
				} else {

					auditContainer.Alpha = 0;
				}

				this.View.SetNeedsLayout ();
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

		private string htmlFormatString( string content)
		{
			return "<html> \n <head> \n <style type=\"text/css\"> \n body {font-family: \"" + Constants.HTML_FONT + "\"; font-size:" + Constants.TERMS_TEXT_FONT_SIZE + "; padding-left:" + Constants.TERMS_SIDE_PADDING +"px; padding-right:" + Constants.TERMS_SIDE_PADDING +"px; }\n </style> \n </head> \n <body>" + content + "</body> \n </html>";

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
                return (float)base.View.Frame.Height - Helpers.BottomNavHeight();;
            }
        }

        float IGSCBaseViewImplementor.ViewContentYPositionPadding()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return Constants.IsPhone() ? 10 : Constants.NAV_BUTTON_SIZE_IPAD;
            }
            else
            {
                return (Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING);
            }

        }

	}
}

