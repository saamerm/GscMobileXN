using System;
using MobileClaims.Core.ViewModels;
using UIKit;
using CoreGraphics;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("ClaimTreatmentDetailsEntryMIView")]
    public class ClaimTreatmentDetailsEntryMIView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
	{
		protected ClaimTreatmentDetailsEntryMIViewModel _model;

		protected UIScrollView scrollContainer;

		ClaimTreatmentDetailsTitleAndList itemDescriptionList;

		ClaimTreatmentDetailsTitleAndDatePicker pickupDateComponent;

		ClaimTreatmentDetailsTitleAndTextField quantityField;

		ClaimTreatmentDetailsTitleAndTextField totalAmountCharged;

		ClaimTreatmentDetailsTitleAndTextField amountPaidByAlternateCarrier;

		ClaimTreatmentDetailsTitleAndToggle gstIncluded;

		ClaimTreatmentDetailsTitleAndToggle pstIncluded;

		ClaimTreatmentDetailsTitleAndToggle referralSubmitted;

		ClaimTreatmentDetailsTitleAndDatePicker medicalReferralDate;

		ClaimTreatmentDetailsTitleAndList medicalItemProfessional;

		UIBarButtonItem addEntryButton;
		protected UIBarButtonItem cancelButton;

		GSButton deleteButton;

		public ClaimTreatmentDetailsEntryMIView ()
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

			_model = (ClaimTreatmentDetailsEntryMIViewModel)this.ViewModel;

			scrollContainer = ((GSCBaseView)View).baseScrollContainer;
			((GSCBaseView)View).baseContainer.AddSubview (scrollContainer);

			((GSCBaseView)View).ViewTapped += HandleViewTapped;

			itemDescriptionList = new ClaimTreatmentDetailsTitleAndList (this, "itemDescription".tr());
			scrollContainer.AddSubview (itemDescriptionList);

			pickupDateComponent = new ClaimTreatmentDetailsTitleAndDatePicker (this, "pickupDate".tr());
			scrollContainer.AddSubview (pickupDateComponent);

			quantityField = new ClaimTreatmentDetailsTitleAndTextField ("quantity".tr());
			scrollContainer.AddSubview (quantityField);

			totalAmountCharged = new ClaimTreatmentDetailsTitleAndTextField ("totalAmountMedicalItems".tr());
			scrollContainer.AddSubview (totalAmountCharged);

			amountPaidByAlternateCarrier = new ClaimTreatmentDetailsTitleAndTextField ("amountPaidAlt".tr());

			if (_model.AmountPaidByAlternateCarrierVisible) {
				scrollContainer.AddSubview (amountPaidByAlternateCarrier);
			}

			gstIncluded = new ClaimTreatmentDetailsTitleAndToggle ("gstIncluded".tr());
			scrollContainer.AddSubview (gstIncluded);

			pstIncluded = new ClaimTreatmentDetailsTitleAndToggle ("pstIncluded".tr());
			scrollContainer.AddSubview (pstIncluded);

			referralSubmitted = new ClaimTreatmentDetailsTitleAndToggle ("medicalItemTitle".tr());
			scrollContainer.AddSubview (referralSubmitted);
			referralSubmitted.SwitchToggled += HandleReferallSubmittedToggled;

			medicalReferralDate = new ClaimTreatmentDetailsTitleAndDatePicker (this, "medicalItemReferralDate".tr(), true);
			scrollContainer.AddSubview (medicalReferralDate);
			medicalReferralDate.DateSet += HandleDateSet;
			medicalReferralDate.DateCleared += HandleDateCleared;

			medicalItemProfessional = new ClaimTreatmentDetailsTitleAndList (this, "medicalItemProfessional".tr(), true);
			scrollContainer.AddSubview (medicalItemProfessional);
			medicalItemProfessional.ItemSet += HandleItemSet;;
			medicalItemProfessional.ItemCleared += HandleItemCleared;;


			deleteButton = new GSButton ();
			deleteButton.SetTitle ("deleteEntry".tr(), UIControlState.Normal);

			if (_model.EditMode) {
				scrollContainer.AddSubview (deleteButton);
			}

			DismmssVCTableViewSource descriptionTableSource = new DismmssVCTableViewSource (itemDescriptionList.popoverController,itemDescriptionList.listController.tableView,"ClaimSubmissionBenefitCell",typeof(ClaimSubmissionBenefitCell));
			itemDescriptionList.listController.tableView.Source = descriptionTableSource;

			DismmssVCTableViewSource professionalTableSource = new DismmssVCTableViewSource (medicalItemProfessional.popoverController,medicalItemProfessional.listController.tableView,"ClaimOptionTableCell",typeof(ClaimOptionTableCell));
			medicalItemProfessional.listController.tableView.Source = professionalTableSource;

			_model.OnInvalidClaimDetails += HandleOnInvalidClaimDetails;

			var set = this.CreateBindingSet<ClaimTreatmentDetailsEntryMIView, Core.ViewModels.ClaimTreatmentDetailsEntryMIViewModel>();
			set.Bind (descriptionTableSource).To (vm => vm.ItemDescriptions);
			set.Bind(descriptionTableSource).For(s => s.SelectedItem).To (vm => vm.ItemDescription);
			set.Bind(itemDescriptionList.detailsLabel).To(vm => vm.ItemDescription.Name);
			set.Bind(pickupDateComponent.dateController.datePicker).To(vm => vm.PickupDate);
			set.Bind (pickupDateComponent.detailsLabel).To (vm => vm.PickupDate).WithConversion ("DateToString").OneWay();
			set.Bind (quantityField.textField).To (vm => vm.Quantity);
			set.Bind (totalAmountCharged.textField).To (vm => vm.TotalAmountCharged);
			set.Bind (amountPaidByAlternateCarrier.textField).To (vm => vm.AmountPaidByAlternateCarrier);
			set.Bind(gstIncluded.toggleSwitch).To(vm => vm.GSTHSTIncludedInTotal);
			set.Bind(gstIncluded.toggleSwitch).To(vm => vm.GSTHSTIncludedInTotal);
			set.Bind(pstIncluded.toggleSwitch).To(vm => vm.PSTIncludedInTotal);
			set.Bind(referralSubmitted.toggleSwitch).To(vm => vm.HasReferralBeenPreviouslySubmitted);
			set.Bind (professionalTableSource).To (vm => vm.TypesOfMedicalProfessional);
			set.Bind(professionalTableSource).For(s => s.SelectedItem).To (vm => vm.TypeOfMedicalProfessional);
			set.Bind(medicalItemProfessional.detailsLabel).To(vm => vm.TypeOfMedicalProfessional.Name);
			set.Bind(medicalReferralDate.dateController.datePicker).To(vm => vm.DateOfReferral);
			set.Bind (medicalReferralDate.detailsLabel).To (vm => vm.DateOfReferral).WithConversion ("DateToString").OneWay();
			set.Bind (this.deleteButton).To (vm => vm.DeleteEntryCommand);
			set.Apply ();

			itemDescriptionList.listController.tableView.ReloadData ();
			medicalItemProfessional.listController.tableView.ReloadData ();


			if (!_model.HasReferralBeenPreviouslySubmitted) {
				medicalReferralDate.setIsEnabled (false, false);
				medicalItemProfessional.setIsEnabled (false, false);
                medicalItemProfessional.Hidden = true;
                medicalReferralDate.Hidden = true;
            }

			if (_model.IsDateOfReferralSetByUser) {
				medicalReferralDate.ShowDate ();
			}

			if (_model.TypeOfMedicalProfessional != null) {
				medicalItemProfessional.ShowDetails ();
			}
		}

        void HandleReferallSubmittedToggled(object sender, EventArgs e)
        {
            if (referralSubmitted.toggleSwitch.On)
            {
                medicalItemProfessional.Hidden = false;
                medicalReferralDate.Hidden = false;
                medicalReferralDate.setIsEnabled(true, true);
                medicalItemProfessional.setIsEnabled(true, true);
                ViewDidLayoutSubviews();

            }
            else
            {
                medicalReferralDate.setIsEnabled(false, true);
                medicalItemProfessional.setIsEnabled(false, true);
                medicalItemProfessional.Hidden = true;
                medicalReferralDate.Hidden = true;
                ViewDidLayoutSubviews();
            }
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

			if (_model.MissingItemDescription)
				itemDescriptionList.showError ("missingItemDescription".tr());
			else
				itemDescriptionList.hideError ();

			if (_model.InvalidPickupDate || _model.MissingPickupDate || _model.DateTooOld)
				pickupDateComponent.showError ("dateofTreatment24Months".tr());
			else
				pickupDateComponent.hideError ();

			if (_model.InvalidTotalAmount || _model.MissingTotalAmount)
				totalAmountCharged.showError (_model.InvalidTotalAmount ? "invalidTotalAmountCharged".tr() : "sepcifyTotalAmountCharged".tr());
			else
				totalAmountCharged.hideError ();

			if (_model.InvalidQuantity || _model.MissingQuantity)
				quantityField.showError ( _model.InvalidQuantity ? "invalidQuantity".tr() : "missingQuantity".tr());
			else
				quantityField.hideError ();

			if (_model.EmptyTypeOfMedicalProfessional) 
				medicalItemProfessional.showError ( "medicalProfessionalError".tr());
			else
				medicalItemProfessional.hideError ();

			if (_model.InvalidAC || _model.MissingAC || _model.BadValueAC) {

				if (_model.BadValueAC)
					amountPaidByAlternateCarrier.showError ("altCarrierTooHigh".tr() );
				else
					amountPaidByAlternateCarrier.showError (_model.InvalidAC ? "invalidAlternateCarrier".tr() : "specifyAlternateCarrier".tr() );

			}
			else
				amountPaidByAlternateCarrier.hideError ();

			if (_model.DateOfReferralTooOld) 
				medicalReferralDate.showError ("dateofPrescription12Months".tr ());
			else
				medicalReferralDate.hideError ();

		}

		void HandleDateCleared (object sender, EventArgs e)
		{
			_model.DateOfReferral = DateTime.MinValue;
		}

		void HandleDateSet (object sender, EventArgs e)
		{
			_model.DateOfReferral = DateTime.Now;
		}

		void HandleItemCleared (object sender, EventArgs e)
		{
			_model.TypeOfMedicalProfessional = null;
		}

		void HandleItemSet (object sender, EventArgs e)
		{

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

            //float ViewContainerWidth = (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            //float ViewContainerHeight = (float)((GSCBaseView)View).baseContainer.Bounds.Height - Helpers.BottomNavHeight();
			float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
			float contentWidth = ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2;

			float itemPadding = Constants.CLAIMS_DETAILS_COMPONENT_PADDING;

			scrollContainer.Frame =  new CGRect (0, 0, ViewContainerWidth , ViewContainerHeight);
            float yPos = ViewContentYPositionPadding;
            float extraPos = Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;

			itemDescriptionList.Frame = new CGRect (sidePadding, yPos, contentWidth, itemDescriptionList.ComponentHeight);

			yPos += (float)itemDescriptionList.Frame.Height;

			pickupDateComponent.Frame = new CGRect (sidePadding, yPos, contentWidth, pickupDateComponent.ComponentHeight);

			yPos += (float)pickupDateComponent.Frame.Height;

			quantityField.Frame = new CGRect (sidePadding, yPos, contentWidth, quantityField.ComponentHeight);

			yPos += (float)quantityField.Frame.Height;

			totalAmountCharged.Frame = new CGRect (sidePadding, yPos, contentWidth, totalAmountCharged.ComponentHeight);

			yPos += (float)totalAmountCharged.Frame.Height;

			if (_model.AmountPaidByAlternateCarrierVisible) {
				amountPaidByAlternateCarrier.Frame = new CGRect (sidePadding, yPos, contentWidth, amountPaidByAlternateCarrier.ComponentHeight);

				yPos += (float)amountPaidByAlternateCarrier.Frame.Height;
			}

			gstIncluded.Frame = new CGRect (sidePadding, yPos, contentWidth, gstIncluded.ComponentHeight);

			yPos += (float)gstIncluded.Frame.Height;

			pstIncluded.Frame = new CGRect (sidePadding, yPos, contentWidth, pstIncluded.ComponentHeight);

			yPos += pstIncluded.ComponentHeight + itemPadding;

			referralSubmitted.Frame = new CGRect (sidePadding, yPos, contentWidth, referralSubmitted.ComponentHeight);

			yPos += referralSubmitted.ComponentHeight + itemPadding;

            
            medicalReferralDate.Frame = new CGRect (sidePadding, yPos, contentWidth, medicalReferralDate.ComponentHeight);

			yPos += medicalReferralDate.ComponentHeight + itemPadding;

			medicalItemProfessional.Frame = new CGRect (sidePadding, yPos, contentWidth, medicalItemProfessional.ComponentHeight);

			
            if (referralSubmitted.toggleSwitch.On)
            {

                yPos += medicalItemProfessional.ComponentHeight + itemPadding;

                deleteButton.Frame = new CGRect(ViewContainerWidth / 2 - Constants.DEFAULT_BUTTON_WIDTH / 2, yPos, Constants.DEFAULT_BUTTON_WIDTH, Constants.DEFAULT_BUTTON_HEIGHT);

                yPos += (float)deleteButton.Frame.Height + itemPadding;

            } else
            {
                yPos -= medicalReferralDate.ComponentHeight + itemPadding;
            }

            deleteButton.Frame = new CGRect(ViewContainerWidth / 2 - Constants.DEFAULT_BUTTON_WIDTH / 2, yPos, Constants.DEFAULT_BUTTON_WIDTH, Constants.DEFAULT_BUTTON_HEIGHT);

            yPos += (float)deleteButton.Frame.Height + itemPadding;

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

        void HandleDeleteButton()
        {

        }
	}
}
