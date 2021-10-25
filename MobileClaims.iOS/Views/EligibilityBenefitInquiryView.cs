using System;
using System.Collections.Generic;
using CoreGraphics;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS
{
    [Foundation.Register("EligibilityBenefitInquiryView")]
    public class EligibilityBenefitInquiryView: GSCBaseViewController
	{
		protected EligibilityBenefitInquiryViewModel _model;

		protected UIScrollView scrollableContainer;

		protected UILabel eligibilityNote1;
		protected UILabel mandatoryLabel;
		protected UILabel toLabel;
		protected UILabel fromLabel;
		protected UILabel emailLabel;
		protected UILabel eligibilityNote2;


		protected UILabel legalPracticionerNote;
		protected UILabel enterInquiryNote;
		protected UILabel counterLabel;

		LimitTextView textArea;

		protected UILabel expectReplyLabel1;
		protected UILabel expectReplyLabel2;

		GSButton submitButton;
		GSButton submitAnotherButton;
		GSButton cancelButton;

		bool isDRE = false;

		protected UITableView submissionTableView;

		public EligibilityBenefitInquiryView ()
		{
		}

		public override void ViewDidLoad()
		{
			View = new GSCBaseView () { BackgroundColor = Colors.BACKGROUND_COLOR };

			base.ViewDidLoad ();

			_model = (EligibilityBenefitInquiryViewModel)ViewModel;

			base.NavigationController.NavigationBarHidden = false;
			if ("locale".tr () == "en") {
				base.NavigationItem.Title = _model.EligibilityCheckType.Name.ToUpper() + "elegibilityInquiryTitle".tr();
			} else {
				base.NavigationItem.Title =  "elegibilityInquiryTitle".tr() + _model.EligibilityCheckType.Name.ToUpper();
			}
			base.NavigationItem.SetHidesBackButton (true, false);

			if(Constants.IS_OS_7_OR_LATER())
				this.AutomaticallyAdjustsScrollViewInsets = false;

			scrollableContainer = ((GSCBaseView)View).baseScrollContainer;
			((GSCBaseView)View).baseContainer.AddSubview (scrollableContainer);

			eligibilityNote1 = new UILabel();
			eligibilityNote1.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
			eligibilityNote1.TextColor = Colors.DARK_GREY_COLOR;
			eligibilityNote1.TextAlignment = UITextAlignment.Left;
			eligibilityNote1.BackgroundColor = Colors.Clear;
			eligibilityNote1.LineBreakMode = UILineBreakMode.WordWrap;
			eligibilityNote1.Lines = 0;
			eligibilityNote1.Text = "eligibilityInquiryInfo1".tr();
			scrollableContainer.AddSubview (eligibilityNote1);

			mandatoryLabel = new UILabel();
			mandatoryLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
			mandatoryLabel.TextColor = Colors.DARK_GREY_COLOR;
			mandatoryLabel.TextAlignment = UITextAlignment.Left;
			mandatoryLabel.BackgroundColor = Colors.Clear;
			mandatoryLabel.LineBreakMode = UILineBreakMode.WordWrap;
			mandatoryLabel.Lines = 0;
			mandatoryLabel.Text = "indicatesAMantatoryField".tr();
			scrollableContainer.AddSubview (mandatoryLabel);

			toLabel = new UILabel();
			toLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
			toLabel.TextColor = Colors.DARK_GREY_COLOR;
			toLabel.TextAlignment = UITextAlignment.Left;
			toLabel.BackgroundColor = Colors.Clear;
			toLabel.LineBreakMode = UILineBreakMode.WordWrap;
			toLabel.Lines = 0;
            toLabel.Text = "toGSCRepresentative".FormatWithBrandKeywords(LocalizableBrand.GreenShield);
			scrollableContainer.AddSubview (toLabel);

			fromLabel = new UILabel();
			fromLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
			fromLabel.TextColor = Colors.DARK_GREY_COLOR;
			fromLabel.TextAlignment = UITextAlignment.Left;
			fromLabel.BackgroundColor = Colors.Clear;
			fromLabel.LineBreakMode = UILineBreakMode.WordWrap;
			fromLabel.Lines = 0;
			fromLabel.Text = "from".tr() + _model.PlanMember.PlanMemberID + ":" + _model.PlanMember.FullName;
			scrollableContainer.AddSubview (fromLabel);

			emailLabel = new UILabel();
			emailLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
			emailLabel.TextColor = Colors.DARK_GREY_COLOR;
			emailLabel.TextAlignment = UITextAlignment.Left;
			emailLabel.BackgroundColor = Colors.Clear;
			emailLabel.LineBreakMode = UILineBreakMode.WordWrap;
			emailLabel.Lines = 0;
			emailLabel.Text = "yourEmailAddress".tr() + " " + _model.PlanMember.Email;
			scrollableContainer.AddSubview (emailLabel);

			eligibilityNote2 = new UILabel();
			eligibilityNote2.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
			eligibilityNote2.TextColor = Colors.DARK_GREY_COLOR;
			eligibilityNote2.TextAlignment = UITextAlignment.Left;
			eligibilityNote2.BackgroundColor = Colors.Clear;
			eligibilityNote2.LineBreakMode = UILineBreakMode.WordWrap;
			eligibilityNote2.Lines = 0;
			eligibilityNote2.Text = "eligibilityInquiryInfo2".tr();
			scrollableContainer.AddSubview (eligibilityNote2);

			float rowHeight = 0;

			if (_model.EligibilityCheckType.ID == "RECALLEXAM") {
				isDRE = true;
				rowHeight = 55 * 3 + 30 + 10;
			} else {
				rowHeight = 55 * 1 + 30 + 10;
			}

			submissionTableView = new UITableView(new CGRect(0,0,0,0), UITableViewStyle.Plain);
			submissionTableView.RowHeight = rowHeight;
			submissionTableView.TableHeaderView = new UIView ();
			submissionTableView.SeparatorColor = Colors.VERY_DARK_GREY_COLOR;
			submissionTableView.ShowsVerticalScrollIndicator = true;
			submissionTableView.AllowsMultipleSelection = true;
			submissionTableView.UserInteractionEnabled = false;
			scrollableContainer.AddSubview (submissionTableView);

			legalPracticionerNote = new UILabel();
			legalPracticionerNote.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
			legalPracticionerNote.TextColor = Colors.DARK_GREY_COLOR;
			legalPracticionerNote.TextAlignment = UITextAlignment.Left;
			legalPracticionerNote.BackgroundColor = Colors.Clear;
			legalPracticionerNote.LineBreakMode = UILineBreakMode.WordWrap;
			legalPracticionerNote.Lines = 0;
			legalPracticionerNote.Text = "legalPracticionerNote".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada);
			scrollableContainer.AddSubview (legalPracticionerNote);

			enterInquiryNote = new UILabel();
			enterInquiryNote.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
			enterInquiryNote.TextColor = Colors.DARK_GREY_COLOR;
			enterInquiryNote.TextAlignment = UITextAlignment.Left;
			enterInquiryNote.BackgroundColor = Colors.Clear;
			enterInquiryNote.LineBreakMode = UILineBreakMode.WordWrap;
			enterInquiryNote.Lines = 0;
			enterInquiryNote.Text = "enterInquiry".tr();
			scrollableContainer.AddSubview (enterInquiryNote);

			counterLabel = new UILabel();
			counterLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
			counterLabel.TextColor = Colors.DARK_GREY_COLOR;
			counterLabel.TextAlignment = UITextAlignment.Left;
			counterLabel.BackgroundColor = Colors.Clear;
			counterLabel.LineBreakMode = UILineBreakMode.WordWrap;
			counterLabel.Lines = 0;
			counterLabel.Text = "counter".tr() + 0.ToString();
			scrollableContainer.AddSubview (counterLabel);

			textArea = new LimitTextView ();
			textArea.Changed += HandleTextAreaChanged;
			textArea.LimitReached += HandleLimitReached;
			textArea.ReturnKeyType = UIReturnKeyType.Done;
			scrollableContainer.AddSubview (textArea);

			expectReplyLabel1 = new UILabel();
			expectReplyLabel1.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
			expectReplyLabel1.TextColor = Colors.DARK_GREY_COLOR;
			expectReplyLabel1.TextAlignment = UITextAlignment.Left;
			expectReplyLabel1.BackgroundColor = Colors.Clear;
			expectReplyLabel1.LineBreakMode = UILineBreakMode.WordWrap;
			expectReplyLabel1.Lines = 0;
			expectReplyLabel1.Text = "expectReplyNote".tr();
			scrollableContainer.AddSubview (expectReplyLabel1);

			expectReplyLabel2 = new UILabel();
			expectReplyLabel2.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
			expectReplyLabel2.TextColor = Colors.HIGHLIGHT_COLOR;
			expectReplyLabel2.TextAlignment = UITextAlignment.Left;
			expectReplyLabel2.BackgroundColor = Colors.Clear;
			expectReplyLabel2.LineBreakMode = UILineBreakMode.WordWrap;
			expectReplyLabel2.Lines = 0;
			expectReplyLabel2.Text = "expectReplyNote2".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada);
            scrollableContainer.AddSubview (expectReplyLabel2);

			submitButton = new GSButton ();
			submitButton.SetTitle ("submitInquiry".tr(), UIControlState.Normal);
			scrollableContainer.AddSubview (submitButton);

			submitButton = new GSButton ();
			submitButton.SetTitle ("checkEligibilityTitle".tr(), UIControlState.Normal);
			scrollableContainer.AddSubview (submitButton);

			submitAnotherButton = new GSButton ();
			submitAnotherButton.SetTitle ("submitAnotherEligibilityCheck".tr(), UIControlState.Normal);
			scrollableContainer.AddSubview (submitAnotherButton);

			cancelButton = new GSButton ();
			cancelButton.SetTitle ("cancelCaps".tr(), UIControlState.Normal);
			scrollableContainer.AddSubview (cancelButton);

			var set = this.CreateBindingSet<EligibilityBenefitInquiryView, Core.ViewModels.EligibilityBenefitInquiryViewModel>();
			set.Bind (this).For (v => v.ParticipantEligibilityResults).To (vm => vm.SelectedParticipants);
			set.Bind (this.submitButton).To (vm => vm.SubmitBenefitInquiryCommand);
			set.Bind (this.submitAnotherButton).To (vm => vm.SubmitAnotherEligibilityCheckCommand);
			set.Bind (this.cancelButton).To (vm => vm.CancelBenefitInquiryCommand);
			set.Bind (this).For (v => v.InquirySubmitted).To (vm => vm.InquirySubmitted);
			set.Apply ();
		}

		void HandleLimitReached (object sender, EventArgs e)
		{
			UIAlertView _error = new UIAlertView ("", "exceededCharactersError".tr(), null, "ok".tr(), null);
			_error.Show ();
		}

		void HandleTextAreaChanged (object sender, EventArgs e)
		{
			counterLabel.Text = "counter".tr() + textArea.Text.Length.ToString();
			this.View.SetNeedsLayout ();
//			if(textArea.Text.Length - rang
//
//			System.Console.WriteLine ();
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			((GSCBaseView)View).subscribeToBusyIndicator ();
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			((GSCBaseView)View).unsubscribeFromBusyIndicator ();
		}

		public override void ViewDidLayoutSubviews(){

			base.ViewDidLayoutSubviews ();

			float viewWidth = (float)base.View.Bounds.Width;
			float viewHeight = (float)base.View.Bounds.Height - Helpers.BottomNavHeight();

			float centerX = viewWidth / 2;
			float yPos = Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;;

			float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
			float indentPadding = sidePadding + Constants.DRUG_LOOKUP_SIDE_PADDING;
			float itemPadding = Constants.CLAIMS_DETAILS_COMPONENT_PADDING;

			scrollableContainer.Frame = new CGRect (0, 0, viewWidth, viewHeight);

			eligibilityNote1.Frame = new CGRect (sidePadding,yPos, viewWidth - sidePadding*2, (float)eligibilityNote1.Frame.Height);
			eligibilityNote1.SizeToFit ();
			yPos += (float)eligibilityNote1.Frame.Height + itemPadding;

			mandatoryLabel.Frame = new CGRect (indentPadding,yPos, viewWidth - sidePadding*2, (float)mandatoryLabel.Frame.Height);
			mandatoryLabel.SizeToFit ();
			yPos += (float)mandatoryLabel.Frame.Height;

			toLabel.Frame = new CGRect (indentPadding,yPos, viewWidth - sidePadding*2, (float)toLabel.Frame.Height);
			toLabel.SizeToFit ();
			yPos += (float)toLabel.Frame.Height;

			fromLabel.Frame = new CGRect (indentPadding,yPos, viewWidth - sidePadding*2, (float)fromLabel.Frame.Height);
			fromLabel.SizeToFit ();
			yPos += (float)fromLabel.Frame.Height + itemPadding;

			emailLabel.Frame = new CGRect (sidePadding,yPos, viewWidth - sidePadding*2, (float)emailLabel.Frame.Height);
			emailLabel.SizeToFit ();
			yPos += (float)emailLabel.Frame.Height + itemPadding;

			eligibilityNote2.Frame = new CGRect (sidePadding,yPos, viewWidth - sidePadding*2, (float)eligibilityNote2.Frame.Height);
			eligibilityNote2.SizeToFit ();
			yPos += (float)eligibilityNote2.Frame.Height + itemPadding;

			float rowHeight = 0;

			if (isDRE) {
				rowHeight = 55 * 3 + 30 + 10;
			} else {
				rowHeight = 55 * 1 + 30 + 10;
			}

			if (_model.SelectedParticipants != null ) {
				float listY = yPos;
				float listHeight = _model.SelectedParticipants.Count * rowHeight;
				submissionTableView.Frame = new CGRect (0, listY, viewWidth, listHeight);
				yPos = (float)submissionTableView.Frame.Height + (float)submissionTableView.Frame.Y + 10;
			}

			legalPracticionerNote.Frame = new CGRect (sidePadding,yPos, viewWidth - sidePadding*2, (float)legalPracticionerNote.Frame.Height);
			legalPracticionerNote.SizeToFit ();
			yPos += (float)legalPracticionerNote.Frame.Height + itemPadding;

			enterInquiryNote.Frame = new CGRect (sidePadding,yPos, viewWidth - sidePadding*2, (float)enterInquiryNote.Frame.Height);
			enterInquiryNote.SizeToFit ();
			yPos += (float)enterInquiryNote.Frame.Height;

			counterLabel.Frame = new CGRect (sidePadding,yPos, viewWidth - sidePadding*2, (float)counterLabel.Frame.Height);
			counterLabel.SizeToFit ();
			yPos += (float)counterLabel.Frame.Height;

			textArea.Frame = new CGRect (sidePadding,yPos, viewWidth - sidePadding*2, 150);
			yPos += (float)textArea.Frame.Height + itemPadding;

			expectReplyLabel1.Frame = new CGRect (sidePadding,yPos, viewWidth - sidePadding*2, (float)expectReplyLabel1.Frame.Height);
			expectReplyLabel1.SizeToFit ();
			yPos += (float)expectReplyLabel1.Frame.Height + itemPadding;

			expectReplyLabel2.Frame = new CGRect (sidePadding,yPos, viewWidth - sidePadding*2, (float)expectReplyLabel2.Frame.Height);
			expectReplyLabel2.SizeToFit ();
			yPos += (float)expectReplyLabel2.Frame.Height+ itemPadding;

			submitButton.Frame = new CGRect (viewWidth/2 - Constants.DEFAULT_BUTTON_WIDTH/2, yPos, Constants.DEFAULT_BUTTON_WIDTH, Constants.DEFAULT_BUTTON_HEIGHT);
            yPos += (float)submitButton.Frame.Height + itemPadding;

			submitAnotherButton.Frame = new CGRect (viewWidth/2 - Constants.DEFAULT_BUTTON_WIDTH/2, yPos, Constants.DEFAULT_BUTTON_WIDTH, Constants.DEFAULT_BUTTON_HEIGHT);
            yPos += (float)submitAnotherButton.Frame.Height + itemPadding;

			cancelButton.Frame = new CGRect (viewWidth/2 - Constants.DEFAULT_BUTTON_WIDTH/2, yPos, Constants.DEFAULT_BUTTON_WIDTH, Constants.DEFAULT_BUTTON_HEIGHT);
            yPos += (float)cancelButton.Frame.Height + itemPadding;

			scrollableContainer.ContentSize = new CGSize (viewWidth, yPos);
		}

		private List<ParticipantEligibilityResult> _participantEligibilityResults;
		public List<ParticipantEligibilityResult> ParticipantEligibilityResults
		{
			get
			{
				return _participantEligibilityResults;
			}
			set
			{
				_participantEligibilityResults = value;
				if (value != null) {
					EligibilityCheckMultipleSelectionSource submissionSource;

					if (isDRE) {
						submissionSource = new EligibilityCheckMultipleSelectionSource (ParticipantEligibilityResults,submissionTableView,"EligibilityCheckParticipantTableCell",typeof(EligibilityCheckParticipantTableCell));
					} else {
						submissionSource = new EligibilityCheckMultipleSelectionSource (ParticipantEligibilityResults,submissionTableView,"EligibilityCheckParticipantCFOTableCell",typeof(EligibilityCheckParticipantCFOTableCell));
					}

					submissionTableView.Source = submissionSource;
					var set = this.CreateBindingSet<EligibilityBenefitInquiryView, Core.ViewModels.EligibilityBenefitInquiryViewModel>();
					set.Bind (submissionSource).To (vm => vm.SelectedParticipants);
					set.Apply ();

					submissionTableView.ReloadData ();
				}

			}
		}

		private bool _inquirySubmitted = false;
		public bool InquirySubmitted
		{
			get
			{
				return _inquirySubmitted;
			}
			set
			{
				_inquirySubmitted = value;
				if (value) {
					InvokeOnMainThread ( () => {
						UIAlertView _alert = new UIAlertView ("", "inquiryRecieved".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada), null, "ok".tr(), null);
						_alert.Show ();
					});
				}

			}
		}
	}
}

