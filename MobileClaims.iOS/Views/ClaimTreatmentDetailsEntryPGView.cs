using System;
using MobileClaims.Core.ViewModels;
using UIKit;
using CoreGraphics;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("ClaimTreatmentDetailsEntryPGView")]
    public class ClaimTreatmentDetailsEntryPGView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
	{
		protected ClaimTreatmentDetailsEntryPGViewModel _model;

		protected UIScrollView scrollContainer;

		ClaimTreatmentDetailsTitleAndDatePicker dateOfPurchaseComponent;

		ClaimTreatmentDetailsTitleAndList typeOfEyewearList;

		ClaimTreatmentDetailsTitleAndList typeOfLensList;

		ClaimTreatmentDetailsTitleAndTextField frameAmount;

		ClaimTreatmentDetailsTitleAndTextField eyeglassLensAmount;

		ClaimTreatmentDetailsTitleAndTextField feeAmount;

		ClaimTreatmentDetailsTitleAndTextField totalAmountCharged;

		UILabel prescriptionDetailsLabel;

		ClaimTreatmentDetailsTitleAndList rightSphereList;
		ClaimTreatmentDetailsTitleAndList leftSphereList;
		ClaimTreatmentDetailsTitleAndList rightCylinderList;
		ClaimTreatmentDetailsTitleAndList leftCylinderList;
		ClaimTreatmentDetailsTitleAndList rightAxisList;
		ClaimTreatmentDetailsTitleAndList leftAxisList;
		ClaimTreatmentDetailsTitleAndList rightPrismList;
		ClaimTreatmentDetailsTitleAndList leftPrismList;
		ClaimTreatmentDetailsTitleAndList rightBifocalList;
		ClaimTreatmentDetailsTitleAndList leftBifocalList;

		ClaimTreatmentDetailsTitleAndToggle acknowledgeCorrective;

		ClaimTreatmentDetailsTitleAndTextField amountPaidByAlternateCarrier;

		ClaimTreatmentDetailsTitleAndTextField amountPaidByPPorGP;

		UIBarButtonItem addEntryButton;
		protected UIBarButtonItem cancelButton;

		GSButton deleteButton;

		public ClaimTreatmentDetailsEntryPGView ()
		{
		}
		public override void ViewDidLoad ()
		{
			View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
			base.ViewDidLoad();

			base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = "treatmentTitle".tr();

			if (Constants.IS_OS_7_OR_LATER()) {
				this.AutomaticallyAdjustsScrollViewInsets = false;
				base.NavigationController.NavigationBar.TintColor = Colors.HIGHLIGHT_COLOR;
				base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
				base.NavigationController.View.BackgroundColor = Colors.BACKGROUND_COLOR;
			} else {
				base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
			}

			base.NavigationItem.SetHidesBackButton (true, false);
			cancelButton = new UIBarButtonItem();
			cancelButton.Style = UIBarButtonItemStyle.Plain;
			cancelButton.Clicked += HandleCancelButton;
			cancelButton.Title = "cancel".tr ();
			cancelButton.TintColor = Colors.HIGHLIGHT_COLOR;
			UITextAttributes attributes = new UITextAttributes ();
			attributes.Font = UIFont.FromName (Constants.NUNITO_REGULAR, (nfloat)Constants.NAV_BAR_BUTTON_SIZE);
			cancelButton.SetTitleTextAttributes (attributes, UIControlState.Normal);
			base.NavigationItem.LeftBarButtonItem = cancelButton;

			addEntryButton = new UIBarButtonItem();
			addEntryButton.Style = UIBarButtonItemStyle.Plain;
			addEntryButton.Clicked += HandleAddEntry;
			addEntryButton.Title = "save".tr ();
			addEntryButton.TintColor = Colors.HIGHLIGHT_COLOR;
			addEntryButton.SetTitleTextAttributes (attributes, UIControlState.Normal);
			base.NavigationItem.RightBarButtonItem = addEntryButton;

			_model = (ClaimTreatmentDetailsEntryPGViewModel)this.ViewModel;

			scrollContainer = ((GSCBaseView)View).baseScrollContainer;
			((GSCBaseView)View).baseContainer.AddSubview (scrollContainer);

			((GSCBaseView)View).ViewTapped += HandleViewTapped;

			dateOfPurchaseComponent = new ClaimTreatmentDetailsTitleAndDatePicker (this, "dateOfPurchaseService".tr());
			scrollContainer.AddSubview (dateOfPurchaseComponent);

			typeOfEyewearList = new ClaimTreatmentDetailsTitleAndList (this, "typeOfEyewear".tr());
			scrollContainer.AddSubview (typeOfEyewearList);

			typeOfLensList = new ClaimTreatmentDetailsTitleAndList (this, "typeOfLens".tr());
			scrollContainer.AddSubview (typeOfLensList);

			frameAmount = new ClaimTreatmentDetailsTitleAndTextField ("frameAmount".tr());
			scrollContainer.AddSubview (frameAmount);

			eyeglassLensAmount = new ClaimTreatmentDetailsTitleAndTextField ("lensAmount".tr());
			scrollContainer.AddSubview (eyeglassLensAmount);

			feeAmount = new ClaimTreatmentDetailsTitleAndTextField ("feeAmount".tr());
			scrollContainer.AddSubview (feeAmount);

			totalAmountCharged = new ClaimTreatmentDetailsTitleAndTextField ("totalAmountCharged".tr());
			scrollContainer.AddSubview (totalAmountCharged);

			acknowledgeCorrective = new ClaimTreatmentDetailsTitleAndToggle ("eyewearCorrective".tr());
			scrollContainer.AddSubview (acknowledgeCorrective);

			DismmssVCTableViewSource rightSphereSource;
			DismmssVCTableViewSource leftSphereSource;
			DismmssVCTableViewSource rightCylinderSource;
			DismmssVCTableViewSource leftCylinderSource;
			DismmssVCTableViewSource rightAxisSource;
			DismmssVCTableViewSource leftAxisSource;
			DismmssVCTableViewSource rightPrismSource;
			DismmssVCTableViewSource leftPrismSource;
			DismmssVCTableViewSource rightBifocalSource;
			DismmssVCTableViewSource leftBifocalSource;

			if (_model.IsPrescriptionDetailsVisible) {

				prescriptionDetailsLabel = new UILabel();
				prescriptionDetailsLabel.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
				prescriptionDetailsLabel.TextColor = Colors.DARK_GREY_COLOR;			
				prescriptionDetailsLabel.BackgroundColor = Colors.Clear;
				prescriptionDetailsLabel.TextAlignment = UITextAlignment.Left;
				prescriptionDetailsLabel.LineBreakMode = UILineBreakMode.WordWrap;
				prescriptionDetailsLabel.Lines = 0;
				prescriptionDetailsLabel.Text = "prescriptionDetails".tr ();
				scrollContainer.AddSubview(prescriptionDetailsLabel);

				rightSphereList = new ClaimTreatmentDetailsTitleAndList (this, "rightSphere".tr());
				scrollContainer.AddSubview (rightSphereList);

				leftSphereList = new ClaimTreatmentDetailsTitleAndList (this, "leftSphere".tr());
				scrollContainer.AddSubview (leftSphereList);

				rightCylinderList = new ClaimTreatmentDetailsTitleAndList (this, "rightCylinder".tr());
				scrollContainer.AddSubview (rightCylinderList);

				leftCylinderList = new ClaimTreatmentDetailsTitleAndList (this, "leftCylinder".tr());
				scrollContainer.AddSubview (leftCylinderList);

				rightAxisList = new ClaimTreatmentDetailsTitleAndList (this, "rightAxis".tr());
				scrollContainer.AddSubview (rightAxisList);

				leftAxisList = new ClaimTreatmentDetailsTitleAndList (this, "leftAxis".tr());
				scrollContainer.AddSubview (leftAxisList);

				rightPrismList = new ClaimTreatmentDetailsTitleAndList (this, "rightPrism".tr());
				scrollContainer.AddSubview (rightPrismList);

				leftPrismList = new ClaimTreatmentDetailsTitleAndList (this, "leftPrism".tr());
				scrollContainer.AddSubview (leftPrismList);

				rightBifocalList = new ClaimTreatmentDetailsTitleAndList (this, "rightBifocal".tr());
				scrollContainer.AddSubview (rightBifocalList);

				leftBifocalList = new ClaimTreatmentDetailsTitleAndList (this, "leftBifocal".tr());
				scrollContainer.AddSubview (leftBifocalList);
			}

			amountPaidByAlternateCarrier = new ClaimTreatmentDetailsTitleAndTextField ("amountPaidAlt".tr());
			amountPaidByPPorGP = new ClaimTreatmentDetailsTitleAndTextField(_model.AmountPaidByPPorGPText);

			if (_model.AmountPaidByAlternateCarrierVisible)
			{
				scrollContainer.AddSubview(amountPaidByAlternateCarrier);
			}

			if (_model.IsAmountPaidByPPorGPVisible)
			{
				scrollContainer.AddSubview(amountPaidByPPorGP);
			}

			deleteButton = new GSButton ();
			deleteButton.SetTitle ("deleteEntry".tr(), UIControlState.Normal);

			if (_model.EditMode) {
				scrollContainer.AddSubview (deleteButton);
			}

			DismmssVCTableViewSource eyewearTableSource = new DismmssVCTableViewSource (typeOfEyewearList.popoverController,typeOfEyewearList.listController.tableView,"ClaimSubmissionBenefitCell",typeof(ClaimSubmissionBenefitCell));
			typeOfEyewearList.listController.tableView.Source = eyewearTableSource;

			DismmssVCTableViewSource lensTableSource = new DismmssVCTableViewSource (typeOfLensList.popoverController,typeOfLensList.listController.tableView,"ClaimSubmissionLensCell",typeof(ClaimSubmissionLensCell));
			typeOfLensList.listController.tableView.Source = lensTableSource;

			_model.OnInvalidClaimDetails += HandleOnInvalidClaimDetails;

			var set = this.CreateBindingSet<ClaimTreatmentDetailsEntryPGView, Core.ViewModels.ClaimTreatmentDetailsEntryPGViewModel>();
			set.Bind(dateOfPurchaseComponent.dateController.datePicker).To(vm => vm.DateOfPurchase);
			set.Bind (dateOfPurchaseComponent.detailsLabel).To (vm => vm.DateOfPurchase).WithConversion ("DateToString").OneWay();
			set.Bind (eyewearTableSource).To (vm => vm.TypesOfEyewear);
			set.Bind(eyewearTableSource).For(s => s.SelectedItem).To (vm => vm.TypeOfEyewear);
			set.Bind(typeOfEyewearList.detailsLabel).To(vm => vm.TypeOfEyewear.Name);
			set.Bind (lensTableSource).To (vm => vm.TypesOfLens);
			set.Bind(lensTableSource).For(s => s.SelectedItem).To (vm => vm.TypeOfLens);
			set.Bind(typeOfLensList.detailsLabel).To(vm => vm.TypeOfLens.Name);

			set.Bind (frameAmount.textField).To (vm => vm.FrameAmount);
			set.Bind(frameAmount).For(v => v.Hidden).To(vm => vm.IsFrameLensAndFeeEntryVisible).WithConversion("BoolOpposite");
			set.Bind (frameAmount).For (v => v.Enabled).To (vm => vm.IsFrameAmountEnabled);
			set.Bind (eyeglassLensAmount.textField).To (vm => vm.EyeglassLensesAmount);
			set.Bind(eyeglassLensAmount).For(v => v.Hidden).To(vm => vm.IsFrameLensAndFeeEntryVisible).WithConversion("BoolOpposite");
			set.Bind (eyeglassLensAmount).For (v => v.Enabled).To (vm => vm.IsEyeglassLensesAmountEnabled);
			set.Bind (feeAmount.textField).To (vm => vm.FeeAmount);
			set.Bind(feeAmount).For(v => v.Hidden).To(vm => vm.IsFrameLensAndFeeEntryVisible).WithConversion("BoolOpposite");
			set.Bind (totalAmountCharged.textField).To (vm => vm.TotalAmountCharged);
			set.Bind(totalAmountCharged).For(v => v.Hidden).To(vm => vm.IsTotalAmountChargedVisible).WithConversion("BoolOpposite");

			set.Bind(acknowledgeCorrective.toggleSwitch).To(vm => vm.AcknowledgeEyewareIsCorrective);
			set.Bind(acknowledgeCorrective).For(v => v.Hidden).To(vm => vm.IsAcknowledgeEyewearIsCorrectiveVisible).WithConversion("BoolOpposite");

			if (_model.IsPrescriptionDetailsVisible) {

				rightSphereSource = new DismmssVCTableViewSource (rightSphereList.popoverController,rightSphereList.listController.tableView,"ClaimOptionTableCell",typeof(ClaimOptionTableCell));
				rightSphereList.listController.tableView.Source = rightSphereSource;
				leftSphereSource = new DismmssVCTableViewSource (leftSphereList.popoverController,leftSphereList.listController.tableView,"ClaimOptionTableCell",typeof(ClaimOptionTableCell));
				leftSphereList.listController.tableView.Source = leftSphereSource;

				rightCylinderSource = new DismmssVCTableViewSource (rightCylinderList.popoverController,rightCylinderList.listController.tableView,"ClaimOptionTableCell",typeof(ClaimOptionTableCell));
				rightCylinderList.listController.tableView.Source = rightCylinderSource;
				leftCylinderSource = new DismmssVCTableViewSource (leftCylinderList.popoverController,leftCylinderList.listController.tableView,"ClaimOptionTableCell",typeof(ClaimOptionTableCell));
				leftCylinderList.listController.tableView.Source = leftCylinderSource;

				rightAxisSource = new DismmssVCTableViewSource (rightAxisList.popoverController,rightAxisList.listController.tableView,"ClaimOptionTableCell",typeof(ClaimOptionTableCell));
				rightAxisList.listController.tableView.Source = rightAxisSource;
				leftAxisSource = new DismmssVCTableViewSource (leftAxisList.popoverController,leftAxisList.listController.tableView,"ClaimOptionTableCell",typeof(ClaimOptionTableCell));
				leftAxisList.listController.tableView.Source = leftAxisSource;

				rightPrismSource = new DismmssVCTableViewSource (rightPrismList.popoverController,rightPrismList.listController.tableView,"ClaimOptionTableCell",typeof(ClaimOptionTableCell));
				rightPrismList.listController.tableView.Source = rightPrismSource;
				leftPrismSource = new DismmssVCTableViewSource (leftPrismList.popoverController,leftPrismList.listController.tableView,"ClaimOptionTableCell",typeof(ClaimOptionTableCell));
				leftPrismList.listController.tableView.Source = leftPrismSource;

				rightBifocalSource = new DismmssVCTableViewSource (rightBifocalList.popoverController,rightBifocalList.listController.tableView,"ClaimOptionTableCell",typeof(ClaimOptionTableCell));
				rightBifocalList.listController.tableView.Source = rightBifocalSource;
				leftBifocalSource = new DismmssVCTableViewSource (leftBifocalList.popoverController,leftBifocalList.listController.tableView,"ClaimOptionTableCell",typeof(ClaimOptionTableCell));
				leftBifocalList.listController.tableView.Source = leftBifocalSource;

				set.Bind (rightSphereSource).To (vm => vm.VisionSpheres);
				set.Bind(rightSphereSource).For(s => s.SelectedItem).To (vm => vm.RightSphere);
				set.Bind(rightSphereList.detailsLabel).To(vm => vm.RightSphere.Name);
				set.Bind (rightSphereList).For (v => v.Enabled).To (vm => vm.IsRightSphereEnabled);

				set.Bind (leftSphereSource).To (vm => vm.VisionSpheres);
				set.Bind(leftSphereSource).For(s => s.SelectedItem).To (vm => vm.LeftSphere);
				set.Bind(leftSphereList.detailsLabel).To(vm => vm.LeftSphere.Name);
				set.Bind (leftSphereList).For (v => v.Enabled).To (vm => vm.IsLeftSphereEnabled);

				set.Bind (rightCylinderSource).To (vm => vm.VisionCylinders);
				set.Bind(rightCylinderSource).For(s => s.SelectedItem).To (vm => vm.RightCylinder);
				set.Bind(rightCylinderList.detailsLabel).To(vm => vm.RightCylinder.Name);
				set.Bind (rightCylinderList).For (v => v.Enabled).To (vm => vm.IsRightCylinderEnabled);

				set.Bind (leftCylinderSource).To (vm => vm.VisionCylinders);
				set.Bind(leftCylinderSource).For(s => s.SelectedItem).To (vm => vm.LeftCylinder);
				set.Bind(leftCylinderList.detailsLabel).To(vm => vm.LeftCylinder.Name);
				set.Bind (leftCylinderList).For (v => v.Enabled).To (vm => vm.IsLeftCylinderEnabled);

				set.Bind (rightAxisSource).To (vm => vm.VisionAxes);
				set.Bind(rightAxisSource).For(s => s.SelectedItem).To (vm => vm.RightAxis);
				set.Bind(rightAxisList.detailsLabel).To(vm => vm.RightAxis.Name);
				set.Bind (rightAxisList).For (v => v.Enabled).To (vm => vm.IsRightAxisEnabled);

				set.Bind (leftAxisSource).To (vm => vm.VisionAxes);
				set.Bind(leftAxisSource).For(s => s.SelectedItem).To (vm => vm.LeftAxis);
				set.Bind(leftAxisList.detailsLabel).To(vm => vm.LeftAxis.Name);
				set.Bind (leftAxisList).For (v => v.Enabled).To (vm => vm.IsLeftAxisEnabled);

				set.Bind (rightPrismSource).To (vm => vm.VisionPrisms);
				set.Bind(rightPrismSource).For(s => s.SelectedItem).To (vm => vm.RightPrism);
				set.Bind(rightPrismList.detailsLabel).To(vm => vm.RightPrism.Name);
				set.Bind (rightPrismList).For (v => v.Enabled).To (vm => vm.IsRightPrismEnabled);

				set.Bind (leftPrismSource).To (vm => vm.VisionPrisms);
				set.Bind(leftPrismSource).For(s => s.SelectedItem).To (vm => vm.LeftPrism);
				set.Bind(leftPrismList.detailsLabel).To(vm => vm.LeftPrism.Name);
				set.Bind (leftPrismList).For (v => v.Enabled).To (vm => vm.IsLeftPrismEnabled);

				set.Bind (rightBifocalSource).To (vm => vm.VisionBifocals);
				set.Bind(rightBifocalSource).For(s => s.SelectedItem).To (vm => vm.RightBifocal);
				set.Bind(rightBifocalList.detailsLabel).To(vm => vm.RightBifocal.Name);
				set.Bind (rightBifocalList).For (v => v.Enabled).To (vm => vm.IsRightBifocalEnabled);

				set.Bind (leftBifocalSource).To (vm => vm.VisionBifocals);
				set.Bind(leftBifocalSource).For(s => s.SelectedItem).To (vm => vm.LeftBifocal);
				set.Bind(leftBifocalList.detailsLabel).To(vm => vm.LeftBifocal.Name);
				set.Bind (leftBifocalList).For (v => v.Enabled).To (vm => vm.IsLeftBifocalEnabled);
			}

			set.Bind (amountPaidByAlternateCarrier.textField).To (vm => vm.AmountPaidByAlternateCarrier);//.WithConversion ("DollarSignPrefix");
			set.Bind(amountPaidByPPorGP.textField).To(vm => vm.AmountPaidByPPorGP);
			set.Bind (this.deleteButton).To (vm => vm.DeleteEntryCommand);
			set.Apply ();

			typeOfEyewearList.listController.tableView.ReloadData ();

		}

		void HandleAddEntry (object sender, EventArgs e)
		{
			if (_model.EditMode) {
				_model.SaveEntryCommand.Execute (null);
			} else {
				_model.SubmitEntryCommand.Execute (null);
			}
		}

		void HandleOnInvalidClaimDetails (object sender, EventArgs e)
		{
			System.Console.WriteLine ("invalid claim details");

			if (_model.MissingTypeOfEyewear)
				typeOfEyewearList.showError ("missingTypeOfEyewear".tr());
			else
				typeOfEyewearList.hideError ();

			if (_model.MissingTypeOfLens)
				typeOfLensList.showError ("missingTypeOfLens".tr());
			else
				typeOfLensList.hideError ();

			if (_model.InvalidDateOfPurchase || _model.DateOfPurchaseError || _model.DateTooOld)
				dateOfPurchaseComponent.showError ("dateofPurchase24Months".tr());
			else
				dateOfPurchaseComponent.hideError ();

			if (_model.InvalidFrameAmount || _model.MissingFrameAmount)
				frameAmount.showError (_model.InvalidFrameAmount ? "invalidFrameAmount".tr() : "specifyFrameAmount".tr());
			else
				frameAmount.hideError ();

			if (_model.InvalidEyeglassLensesAmount || _model.MissingEyeglassLensesAmount)
				eyeglassLensAmount.showError ( _model.InvalidEyeglassLensesAmount ? "invalidEyeglassLensAmount".tr() : "specifyEyeglassLensAmount".tr());
			else
				eyeglassLensAmount.hideError ();

			if (_model.MissingTotalAmountCharged || _model.InvalidTotalAmountCharged)
				totalAmountCharged.showError ( _model.MissingTotalAmountCharged ? "sepcifyTotalAmountCharged".tr() : "invalidTotalAmountCharged".tr());
			else
				totalAmountCharged.hideError ();

			if (_model.AcknowledgedEyewareIsCorrective) {
				acknowledgeCorrective.showError ("mustSpecifyCorrective".tr ());
			} else {
				acknowledgeCorrective.hideError ();
			}

			if (_model.IsPrescriptionDetailsVisible) {


				if (_model.MissingRightSphere) {
					rightSphereList.showError ("prescriptionDetailsIncomplete".tr ());
				} else {
					rightSphereList.hideError ();
				}

				if (_model.MissingLeftSphere) {
					leftSphereList.showError ("prescriptionDetailsIncomplete".tr ());
				} else {
					leftSphereList.hideError ();
				}

				if (_model.MissingRightCylinder) {
					rightCylinderList.showError ("prescriptionDetailsIncomplete".tr ());
				} else {
					rightCylinderList.hideError ();
				}

				if (_model.MissingLeftCylinder) {
					leftCylinderList.showError ("prescriptionDetailsIncomplete".tr ());
				} else {
					leftCylinderList.hideError ();
				}

				if (_model.MissingRightAxis) {
					rightAxisList.showError ("prescriptionDetailsIncomplete".tr ());
				} else {
					rightAxisList.hideError ();
				}

				if (_model.MissingLeftAxis) {
					leftAxisList.showError ("prescriptionDetailsIncomplete".tr ());
				} else {
					leftAxisList.hideError ();
				}

				if (_model.MissingRightPrism) {
					rightPrismList.showError ("prescriptionDetailsIncomplete".tr ());
				} else {
					rightPrismList.hideError ();
				}

				if (_model.MissingLeftPrism) {
					leftPrismList.showError ("prescriptionDetailsIncomplete".tr ());
				} else {
					leftPrismList.hideError ();
				}

				if (_model.MissingRightBifocal) {
					rightBifocalList.showError ("prescriptionDetailsIncomplete".tr ());
				} else {
					rightBifocalList.hideError ();
				}

				if (_model.MissingLeftBifocal) {
					leftBifocalList.showError ("prescriptionDetailsIncomplete".tr ());
				} else {
					leftBifocalList.hideError ();
				}

			}
				
			if (_model.InvalidAC || _model.MissingAC || _model.BadValueAC) {

				if (_model.BadValueAC)
					amountPaidByAlternateCarrier.showError ("altCarrierTooHigh".tr() );
				else
					amountPaidByAlternateCarrier.showError (_model.InvalidAC ? "invalidAlternateCarrier".tr() : "specifyAlternateCarrier".tr() );

			}
			else
				amountPaidByAlternateCarrier.hideError ();


			if (_model.InvalidAmountPaidByPPorGP || _model.MissingAmountPaidByPPorGP || _model.BadAmountPaidByPPorGP)
			{
				if (_model.BadAmountPaidByPPorGP)
				{
					amountPaidByPPorGP.showError(_model.InvalidAmountPaidByPpOrGpErrorMessage);
				}
				else
				{
					amountPaidByPPorGP.showError(_model.InvalidAmountPaidByPPorGP
						? _model.InvalidAmountPaidByPpOrGpErrorMessage : _model.MissingAmountPaidByPpOrGpErrorMessage);
				}
			}
			else
			{
				amountPaidByPPorGP.hideError();
			}
		}

		void HandleCancelButton (object sender, EventArgs e)
		{
			int backTo = 2;

			if (_model.TreatmentDetailsCount < 1 ) {
				if (base.NavigationController.ViewControllers [base.NavigationController.ViewControllers.Length - backTo].GetType () == typeof(ClaimTreatmentDetailsListView))
					backTo++;
			}

			base.NavigationController.PopToViewController( base.NavigationController.ViewControllers[base.NavigationController.ViewControllers.Length - backTo], true);

		}

		void HandleViewTapped (object sender, EventArgs e)
		{
			dismissKeyboard ();
		}

		void dismissKeyboard()
		{
			this.View.EndEditing (true);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			redrawCount = 0;
			View.SetNeedsLayout ();
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

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			dismissKeyboard ();
		}

		int redrawCount = 0;
		public override void ViewDidLayoutSubviews ()
		{
			base.ViewDidLayoutSubviews ();

			float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
			float contentWidth = ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2;

			float itemPadding = Constants.CLAIMS_DETAILS_COMPONENT_PADDING;
            			
            float yPos = ViewContentYPositionPadding; 

			dateOfPurchaseComponent.Frame = new CGRect (sidePadding, yPos, contentWidth, dateOfPurchaseComponent.ComponentHeight);

			yPos += (float)dateOfPurchaseComponent.Frame.Height;

			typeOfEyewearList.Frame = new CGRect (sidePadding, yPos, contentWidth, typeOfEyewearList.ComponentHeight);

			yPos += (float)typeOfEyewearList.Frame.Height;

			typeOfLensList.Frame = new CGRect (sidePadding, yPos, contentWidth, typeOfLensList.ComponentHeight);

			yPos += (float)typeOfLensList.Frame.Height;

			if (!acknowledgeCorrective.Hidden) {
				acknowledgeCorrective.Frame = new CGRect (sidePadding, yPos, contentWidth, acknowledgeCorrective.ComponentHeight);
				yPos += (float)acknowledgeCorrective.Frame.Height;
			}

			if (!frameAmount.Hidden) {
				frameAmount.Frame = new CGRect (sidePadding, yPos, contentWidth, frameAmount.ComponentHeight);
				yPos += (float)frameAmount.Frame.Height;
			}

			if (!eyeglassLensAmount.Hidden) {
				eyeglassLensAmount.Frame = new CGRect (sidePadding, yPos, contentWidth, eyeglassLensAmount.ComponentHeight);
				yPos += (float)eyeglassLensAmount.Frame.Height;
			}

			if (!feeAmount.Hidden) {
				feeAmount.Frame = new CGRect (sidePadding, yPos, contentWidth, feeAmount.ComponentHeight);
				yPos += (float)feeAmount.Frame.Height;
			}

			if (!totalAmountCharged.Hidden) {
				totalAmountCharged.Frame = new CGRect (sidePadding, yPos, contentWidth, totalAmountCharged.ComponentHeight);
				yPos += (float)totalAmountCharged.Frame.Height;
			}

			if (_model.AmountPaidByAlternateCarrierVisible) {
				amountPaidByAlternateCarrier.Frame = new CGRect (sidePadding, yPos, contentWidth, amountPaidByAlternateCarrier.ComponentHeight);

				yPos += amountPaidByAlternateCarrier.ComponentHeight;
			}

			if (_model.IsAmountPaidByPPorGPVisible)
			{
				amountPaidByPPorGP.Frame = new CGRect(sidePadding, yPos, contentWidth, amountPaidByPPorGP.ComponentHeight);

				yPos += amountPaidByPPorGP.ComponentHeight;
			}

			if (_model.IsPrescriptionDetailsVisible) {

				yPos += itemPadding;

				prescriptionDetailsLabel.Frame = new CGRect (Constants.DRUG_LOOKUP_SIDE_PADDING, yPos, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING *2, (float)prescriptionDetailsLabel.Frame.Height);
				prescriptionDetailsLabel.SizeToFit ();
				yPos += (float)prescriptionDetailsLabel.Frame.Height + itemPadding;

				rightSphereList.Frame = new CGRect (sidePadding, yPos, contentWidth, rightSphereList.ComponentHeight);
				yPos += rightSphereList.ComponentHeight;

				leftSphereList.Frame = new CGRect (sidePadding, yPos, contentWidth, leftSphereList.ComponentHeight);
				yPos += leftSphereList.ComponentHeight;

				rightCylinderList.Frame = new CGRect (sidePadding, yPos, contentWidth, rightCylinderList.ComponentHeight);
				yPos += rightCylinderList.ComponentHeight;

				leftCylinderList.Frame = new CGRect (sidePadding, yPos, contentWidth, leftCylinderList.ComponentHeight);
				yPos += leftCylinderList.ComponentHeight;

				rightAxisList.Frame = new CGRect (sidePadding, yPos, contentWidth, rightAxisList.ComponentHeight);
				yPos += rightAxisList.ComponentHeight;

				leftAxisList.Frame = new CGRect (sidePadding, yPos, contentWidth, leftAxisList.ComponentHeight);
				yPos += leftAxisList.ComponentHeight;

				rightPrismList.Frame = new CGRect (sidePadding, yPos, contentWidth, rightPrismList.ComponentHeight);
				yPos += rightPrismList.ComponentHeight;

				leftPrismList.Frame = new CGRect (sidePadding, yPos, contentWidth, leftPrismList.ComponentHeight);
				yPos += leftPrismList.ComponentHeight;

				rightBifocalList.Frame = new CGRect (sidePadding, yPos, contentWidth, rightBifocalList.ComponentHeight);
				yPos += rightBifocalList.ComponentHeight;

				leftBifocalList.Frame = new CGRect (sidePadding, yPos, contentWidth, leftBifocalList.ComponentHeight);
				yPos += leftBifocalList.ComponentHeight;

			}

			yPos += itemPadding;

			deleteButton.Frame = new CGRect (ViewContainerWidth/2 - Constants.DEFAULT_BUTTON_WIDTH/2, yPos, Constants.DEFAULT_BUTTON_WIDTH, Constants.DEFAULT_BUTTON_HEIGHT);

            if (_model.EditMode)
            {
                yPos += (float)(deleteButton.Frame.Height + itemPadding);
            }

            float bottomPadding = Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;

            scrollContainer.Frame = new CGRect(0, 0, ViewContainerWidth, ViewContainerHeight);
            scrollContainer.ContentSize = new CGSize (ViewContainerWidth, yPos + GetBottomPadding());
			if (redrawCount < 2) {
				redrawCount++;
				this.View.SetNeedsLayout();
			}
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