using System;
using MobileClaims.Core.ViewModels;
using UIKit;
using CoreGraphics;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("ClaimTreatmentDetailsEntryOMFView")]
    public class ClaimTreatmentDetailsEntryOMFView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
	{
		protected ClaimTreatmentDetailsEntryOMFViewModel _model;

		protected UIScrollView scrollContainer;

		ClaimTreatmentDetailsTitleAndDatePicker dateOfTreatmentComponent;

		ClaimTreatmentDetailsTitleAndTextField monthlyFee;

		ClaimTreatmentDetailsTitleAndTextField amountPaidByAlternateCarrier;

		UIBarButtonItem addEntryButton;
		protected UIBarButtonItem cancelButton;

		GSButton deleteButton;

		public ClaimTreatmentDetailsEntryOMFView ()
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

			_model = (ClaimTreatmentDetailsEntryOMFViewModel)this.ViewModel;

			scrollContainer = ((GSCBaseView)View).baseScrollContainer;
			((GSCBaseView)View).baseContainer.AddSubview (scrollContainer);

			((GSCBaseView)View).ViewTapped += HandleViewTapped;

			dateOfTreatmentComponent = new ClaimTreatmentDetailsTitleAndDatePicker (this, "dateOfMonthlyPaymentTitle".tr());
			scrollContainer.AddSubview (dateOfTreatmentComponent);

			monthlyFee = new ClaimTreatmentDetailsTitleAndTextField ("omf".tr());
			scrollContainer.AddSubview (monthlyFee);

			amountPaidByAlternateCarrier = new ClaimTreatmentDetailsTitleAndTextField ("amountPaidAlt".tr());

			if (_model.AmountPaidByAlternateCarrierVisible)
				scrollContainer.AddSubview (amountPaidByAlternateCarrier);

			deleteButton = new GSButton ();
			deleteButton.SetTitle ("deleteEntry".tr(), UIControlState.Normal);

			if (_model.EditMode) {
				scrollContainer.AddSubview (deleteButton);
			}

			_model.OnInvalidClaimDetails += HandleOnInvalidClaimDetails;

			var set = this.CreateBindingSet<ClaimTreatmentDetailsEntryOMFView, Core.ViewModels.ClaimTreatmentDetailsEntryOMFViewModel>();
			set.Bind(dateOfTreatmentComponent.dateController.datePicker).To(vm => vm.DateOfMonthlyTreatment);
			set.Bind (dateOfTreatmentComponent.detailsLabel).To (vm => vm.DateOfMonthlyTreatment).WithConversion ("DateToString").OneWay();
			set.Bind (monthlyFee.textField).To (vm => vm.OrthodonticMonthlyFee);
			set.Bind (amountPaidByAlternateCarrier.textField).To (vm => vm.AmountPaidByAlternateCarrier);//.WithConversion ("DollarSignPrefix");
			set.Bind (this.deleteButton).To (vm => vm.DeleteEntryCommand);
			set.Apply ();

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

			if (_model.InvalidOrthodonticMonthlyFee || _model.MissingOrthodonticMonthlyFee)
				monthlyFee.showError ( _model.InvalidOrthodonticMonthlyFee ? "invalidOrthodonticMonthlyFee".tr() : "missingOrthodonticMonthlyFee".tr());
			else
				monthlyFee.hideError ();

			if (_model.InvalidDateOfMonthlyTreatment || _model.InvalidDateOfMonthlyTreatment || _model.DateTooOld)
				dateOfTreatmentComponent.showError ("dateofPayment24Months".tr());
			else
				dateOfTreatmentComponent.hideError ();

			if (_model.InvalidAC || _model.MissingAC || _model.BadValueAC) {

				if (_model.BadValueAC)
					amountPaidByAlternateCarrier.showError ("altCarrierTooHigh".tr() );
				else
					amountPaidByAlternateCarrier.showError (_model.InvalidAC ? "invalidAlternateCarrier".tr() : "specifyAlternateCarrier".tr() );

			}
			else
				amountPaidByAlternateCarrier.hideError ();


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

			dateOfTreatmentComponent.Frame = new CGRect (sidePadding, yPos, contentWidth, dateOfTreatmentComponent.ComponentHeight);

			yPos += (float)dateOfTreatmentComponent.Frame.Height;

			monthlyFee.Frame = new CGRect (sidePadding, yPos, contentWidth, monthlyFee.ComponentHeight);
			
			yPos += (float)monthlyFee.Frame.Height;

			if (_model.AmountPaidByAlternateCarrierVisible) {
				amountPaidByAlternateCarrier.Frame = new CGRect (sidePadding, yPos, contentWidth, amountPaidByAlternateCarrier.ComponentHeight);

				yPos += amountPaidByAlternateCarrier.ComponentHeight + itemPadding;
			} else {
				yPos += itemPadding;
			}

			deleteButton.Frame = new CGRect (ViewContainerWidth/2 - Constants.DEFAULT_BUTTON_WIDTH/2, yPos, Constants.DEFAULT_BUTTON_WIDTH, Constants.DEFAULT_BUTTON_HEIGHT);
		
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
	}
}
