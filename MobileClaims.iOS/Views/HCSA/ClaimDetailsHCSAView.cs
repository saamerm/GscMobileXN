using System.ComponentModel;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels.HCSA;
using MobileClaims.iOS.UI;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Plugin.Messenger;
using UIKit;

namespace MobileClaims.iOS
{
    public class ClaimDetailsHCSAView : GSCBaseViewController, IRehydrating
    {
        protected UILabel lblEnterClaimDetails;
        protected GSButton btnSave;
        protected UIView OverlayView;

        ClaimTreatmentDetailsTitleAndTextField txtTotalOrignalAmount, txtPreviouslyPaidAmt;
        ClaimTreatmentDetailsTitleAndDatePicker dateOfExpense;
        ClaimDetailsHCSAViewModel _model;
        UIBarButtonItem deleteButton, cancelButton;
        public UIScrollView scrollableContainer;
        UIButton btnDel;
        private IMvxMessenger _messenger;

        public bool Rehydrating
        {
            get;
            set;
        }
        public bool FinishedRehydrating
        {
            get;
            set;
        }

        public ClaimDetailsHCSAView()
        {
        }

        public void handleGesture(UIGestureRecognizer gesture)
        {
            txtTotalOrignalAmount.textField.ResignFirstResponder();
            txtPreviouslyPaidAmt.textField.ResignFirstResponder();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            base.NavigationItem.Title = _model.ClaimDetailsTitle.ToUpper();
            SetEmptyForTextField();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            base.NavigationItem.Title = "back".tr();
            _messenger.Publish<OnClaimDetailsViewModelMessage>(new OnClaimDetailsViewModelMessage(this));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _model = (ClaimDetailsHCSAViewModel)this.ViewModel;
            _model.PropertyChanged += Model_PropertyChanged;
            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            var rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();

            if (Rehydrating)
            {
                rehydrationservice.Rehydrating = true;
            }

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.SetHidesBackButton(false, false);
            base.NavigationItem.Title = _model.ClaimDetailsTitle.ToUpper();

            if (_model.IsEditing)
            {
                btnDel = new UIButton(UIButtonType.Custom);
                btnDel.Frame = new CGRect(0, 0, 80, 35);
                btnDel.SetTitle("Delete".tr(), UIControlState.Normal);
                btnDel.TintColor = Colors.HIGHLIGHT_COLOR;
                btnDel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.NAV_BAR_BUTTON_SIZE);
                btnDel.BackgroundColor = Colors.HIGHLIGHT_COLOR;

                deleteButton = new UIBarButtonItem(btnDel);
                base.NavigationItem.RightBarButtonItem = deleteButton;

                cancelButton = new UIBarButtonItem();
                cancelButton.Style = UIBarButtonItemStyle.Plain;
                cancelButton.Title = "cancel".tr();
                cancelButton.TintColor = Colors.HIGHLIGHT_COLOR;
                UITextAttributes attributes1 = new UITextAttributes();
                attributes1.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.NAV_BAR_BUTTON_SIZE);
                cancelButton.SetTitleTextAttributes(attributes1, UIControlState.Normal);
                base.NavigationItem.LeftBarButtonItem = cancelButton;
            }

            View = new UIView()
            {
                BackgroundColor = Colors.BACKGROUND_COLOR
            };

            scrollableContainer = new UIScrollView();
            scrollableContainer.ScrollEnabled = true;

            dateOfExpense = new ClaimTreatmentDetailsTitleAndDatePicker(this, "Date of Expense");
            txtTotalOrignalAmount = new ClaimTreatmentDetailsTitleAndTextField("Total Orignal ");
            txtPreviouslyPaidAmt = new ClaimTreatmentDetailsTitleAndTextField("Total");

            lblEnterClaimDetails = new UILabel();
            lblEnterClaimDetails.BackgroundColor = Colors.Clear;
            lblEnterClaimDetails.TextColor = Colors.DARK_GREY_COLOR;
            lblEnterClaimDetails.TextAlignment = UITextAlignment.Left;
            lblEnterClaimDetails.Lines = 0;
            lblEnterClaimDetails.LineBreakMode = UILineBreakMode.WordWrap;
            lblEnterClaimDetails.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.HEADING_SIZE);

            txtTotalOrignalAmount = new ClaimTreatmentDetailsTitleAndTextField(_model.TotalAmountClaimedLabel);
            txtPreviouslyPaidAmt = new ClaimTreatmentDetailsTitleAndTextField(_model.AmountPreviouslyPaidLabel);

            txtTotalOrignalAmount.textField.TextAlignment = UITextAlignment.Left;
            txtTotalOrignalAmount.textField.KeyboardType = UIKeyboardType.DecimalPad;
            txtPreviouslyPaidAmt.textField.KeyboardType = UIKeyboardType.DecimalPad;
            txtPreviouslyPaidAmt.textField.TextAlignment = UITextAlignment.Left;

            scrollableContainer.AddSubview(dateOfExpense);
            scrollableContainer.AddSubview(txtTotalOrignalAmount);
            scrollableContainer.AddSubview(txtPreviouslyPaidAmt);
            scrollableContainer.AddSubview(lblEnterClaimDetails);

            btnSave = new GSButton();

            scrollableContainer.AddSubview(btnSave);
            View.AddSubview(scrollableContainer);

            if (!Constants.IsPhone())
            {
                OverlayView = new UIView();
                OverlayView.BackgroundColor = Colors.LightGrayColor;
                OverlayView.Alpha = 0.5f;
                View.AddSubview(OverlayView);
            }

            _model.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "ExpenseDateValidationMessage")
                {
                    if (_model.IsInvalidExpenseDate)
                        dateOfExpense.showError(_model.ExpenseDateValidationMessage);
                    else
                        dateOfExpense.hideError();
                }
                else if (e.PropertyName == "ClaimAmountValidationMesssage")
                {
                    if (_model.IsInvalidClaimAmount)
                        txtTotalOrignalAmount.showError(_model.ClaimAmountValidationMesssage);
                    else
                        txtTotalOrignalAmount.hideError();
                }
                else if (e.PropertyName == "OtherPaidAmountValidationMessage")
                {
                    if (_model.IsInvalidOtherPaidAmount)
                        txtPreviouslyPaidAmt.showError(_model.OtherPaidAmountValidationMessage);
                    else
                        txtPreviouslyPaidAmt.hideError();
                }
            };

            txtTotalOrignalAmount.titleLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.TEXTVIEW_FONT_SIZE);
            txtPreviouslyPaidAmt.titleLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.TEXTVIEW_FONT_SIZE);
            dateOfExpense.titleLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.TEXTVIEW_FONT_SIZE);

            UIGestureRecognizer gesture = new UITapGestureRecognizer(handleGesture);
            this.View.AddGestureRecognizer(gesture);

            var set = this.CreateBindingSet<ClaimDetailsHCSAView, ClaimDetailsHCSAViewModel>();
            set.Bind(lblEnterClaimDetails).To(vm => vm.EnterDetailLabel);
            if (!Constants.IsPhone())
            {
                set.Bind(OverlayView).For(b => b.Hidden).To(vm => vm.ClaimParticipantHasBeenSelected);
            }

            set.Bind(lblEnterClaimDetails).To(vm => vm.EnterDetailLabel).WithConversion("StringCase");
            set.Bind(txtTotalOrignalAmount.titleLabel).To(vm => vm.TotalAmountClaimedLabel);
            set.Bind(txtPreviouslyPaidAmt.titleLabel).To(vm => vm.AmountPreviouslyPaidLabel);
            set.Bind(dateOfExpense.titleLabel).To(vm => vm.DateOfExpenseLabel);
            set.Bind(txtTotalOrignalAmount.textField).To(vm => vm.ClaimDetails.ClaimAmountString);
            set.Bind(txtPreviouslyPaidAmt.textField).To(vm => vm.ClaimDetails.OtherPaidAmountString);
            set.Bind(dateOfExpense.dateController.datePicker).To(vm => vm.ClaimDetails.ExpenseDate);
            set.Bind(dateOfExpense.detailsLabel).To(vm => vm.ClaimDetails.ExpenseDate).WithConversion("StringFromDate");
            set.Bind(this.btnSave).To(vm => vm.SaveClaimDetailsCommand);
            set.Bind(this.btnSave).For("Title").To(vm => vm.SaveLabel).WithConversion("StringCase");
            set.Bind(dateOfExpense.titleLabel).For(b => b.TextColor).To(vm => vm.IsInvalidExpenseDate).WithConversion("TextColorError");

            if (_model.IsEditing)
            {
                set.Bind(btnDel).To(vm => vm.DeleteCommand);
                set.Bind(cancelButton).To(vm => vm.CancelCommand);
            }

            set.Apply();
            SetConstraints();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetConstraints();
        }

        private void SetConstraints()
        {
            View.RemoveConstraints(View.Constraints);
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            scrollableContainer.RemoveConstraints(scrollableContainer.Constraints);
            scrollableContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            if (!Constants.IsPhone())
            {
                if (Constants.IS_OS_VERSION_OR_LATER(12, 0))
                {
                    View.AddConstraints(
                        scrollableContainer.AtTopOf(View, View.SafeAreaInsets.Top + 10),
                        scrollableContainer.AtLeftOf(View, View.SafeAreaInsets.Left),
                        scrollableContainer.AtRightOf(View, View.SafeAreaInsets.Right),
                        scrollableContainer.WithSameWidth(View).Minus(View.SafeAreaInsets.Left * 2),
                        scrollableContainer.AtBottomOf(View, View.SafeAreaInsets.Bottom + Constants.NAV_BUTTON_SIZE_IPHONE),

                        lblEnterClaimDetails.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                        lblEnterClaimDetails.AtRightOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                        lblEnterClaimDetails.AtTopOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                        lblEnterClaimDetails.WithSameWidth(scrollableContainer).Minus(Constants.LABEL_SIDE_PADDING * 2),

                        dateOfExpense.AtLeftOf(scrollableContainer, Constants.BUTTON_SIDE_PADDING),
                        dateOfExpense.AtRightOf(scrollableContainer, Constants.BUTTON_SIDE_PADDING),
                        dateOfExpense.Below(lblEnterClaimDetails, Constants.DRUG_LOOKUP_TOP_PADDING),
                        dateOfExpense.Height().EqualTo(Constants.TEXT_FIELD_HEIGHT + 10),

                        txtTotalOrignalAmount.AtLeftOf(scrollableContainer, Constants.BUTTON_SIDE_PADDING),
                        txtTotalOrignalAmount.AtRightOf(scrollableContainer, Constants.BUTTON_SIDE_PADDING),
                        txtTotalOrignalAmount.Below(dateOfExpense, Constants.DRUG_LOOKUP_TOP_PADDING + 10),
                        txtTotalOrignalAmount.Height().EqualTo(Constants.TEXT_FIELD_HEIGHT + 10),

                        txtPreviouslyPaidAmt.AtLeftOf(scrollableContainer, Constants.BUTTON_SIDE_PADDING),
                        txtPreviouslyPaidAmt.AtRightOf(scrollableContainer, Constants.BUTTON_SIDE_PADDING),
                        txtPreviouslyPaidAmt.Below(txtTotalOrignalAmount, Constants.DRUG_LOOKUP_TOP_PADDING),//+10
                        txtPreviouslyPaidAmt.Height().EqualTo(Constants.TEXT_FIELD_HEIGHT + 30),

                        btnSave.Below(txtPreviouslyPaidAmt, 60),
                        btnSave.WithSameCenterX(scrollableContainer),
                        btnSave.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                        btnSave.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                        btnSave.AtBottomOf(scrollableContainer, 100),

                        OverlayView.AtLeftOf(View, 0),
                        OverlayView.AtRightOf(View, 0),
                        OverlayView.AtBottomOf(View, 0),
                        OverlayView.AtTopOf(View, 0));
                }
                else
                {
                    View.AddConstraints(
                        scrollableContainer.AtTopOf(View),
                        scrollableContainer.AtLeftOf(View),
                        scrollableContainer.WithSameWidth(View),
                        scrollableContainer.AtBottomOf(View),

                        lblEnterClaimDetails.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                        lblEnterClaimDetails.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                        lblEnterClaimDetails.AtTopOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),

                        dateOfExpense.AtLeftOf(scrollableContainer, Constants.BUTTON_SIDE_PADDING),
                        dateOfExpense.AtRightOf(View, Constants.BUTTON_SIDE_PADDING),
                        dateOfExpense.Below(lblEnterClaimDetails, Constants.DRUG_LOOKUP_TOP_PADDING),
                        dateOfExpense.Height().EqualTo(Constants.TEXT_FIELD_HEIGHT + 10),

                        txtTotalOrignalAmount.AtLeftOf(scrollableContainer, Constants.BUTTON_SIDE_PADDING),
                        txtTotalOrignalAmount.AtRightOf(View, Constants.BUTTON_SIDE_PADDING),
                        txtTotalOrignalAmount.Below(dateOfExpense, Constants.DRUG_LOOKUP_TOP_PADDING + 10),
                        txtTotalOrignalAmount.Height().EqualTo(Constants.TEXT_FIELD_HEIGHT + 10),

                        txtPreviouslyPaidAmt.AtLeftOf(scrollableContainer, Constants.BUTTON_SIDE_PADDING),
                        txtPreviouslyPaidAmt.AtRightOf(View, Constants.BUTTON_SIDE_PADDING),
                        txtPreviouslyPaidAmt.Below(txtTotalOrignalAmount, Constants.DRUG_LOOKUP_TOP_PADDING),//+10
                        txtPreviouslyPaidAmt.Height().EqualTo(Constants.TEXT_FIELD_HEIGHT + 30),

                        btnSave.Below(txtPreviouslyPaidAmt, 60),
                        btnSave.WithSameCenterX(scrollableContainer),
                        btnSave.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                        btnSave.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                        btnSave.AtBottomOf(scrollableContainer, 100),

                        OverlayView.AtLeftOf(View, 0),
                        OverlayView.AtRightOf(View, 0),
                        OverlayView.AtBottomOf(View, 0),
                        OverlayView.AtTopOf(View, 0)
                    );
                }
            }
            else
            {
                float previouslyPaidOffset = Constants.DRUG_LOOKUP_TOP_PADDING + 10;
                if (_model.AmountPreviouslyPaidLabel.Length > 35)
                    previouslyPaidOffset = 5;

                if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
                {
                    View.AddConstraints(
                        scrollableContainer.AtTopOf(View, View.SafeAreaInsets.Top + 10),
                        scrollableContainer.AtLeftOf(View, View.SafeAreaInsets.Left),
                        scrollableContainer.AtRightOf(View, View.SafeAreaInsets.Right),
                        scrollableContainer.WithSameWidth(View).Minus(View.SafeAreaInsets.Left * 2),
                        scrollableContainer.AtBottomOf(View, View.SafeAreaInsets.Bottom + Constants.NAV_BUTTON_SIZE_IPHONE),

                        lblEnterClaimDetails.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                        lblEnterClaimDetails.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                        lblEnterClaimDetails.AtTopOf(scrollableContainer, 30),

                        dateOfExpense.AtLeftOf(scrollableContainer, Constants.BUTTON_SIDE_PADDING),
                        dateOfExpense.AtRightOf(View, Constants.BUTTON_SIDE_PADDING),
                        dateOfExpense.WithSameWidth(scrollableContainer).Minus(Constants.BUTTON_SIDE_PADDING * 2),
                        dateOfExpense.Below(lblEnterClaimDetails, Constants.DRUG_LOOKUP_TOP_PADDING),
                        dateOfExpense.Height().EqualTo(Constants.TEXT_FIELD_HEIGHT + 30),

                        txtTotalOrignalAmount.AtLeftOf(scrollableContainer, Constants.BUTTON_SIDE_PADDING),
                        txtTotalOrignalAmount.AtRightOf(View, Constants.BUTTON_SIDE_PADDING),
                        txtTotalOrignalAmount.WithSameWidth(scrollableContainer).Minus(Constants.BUTTON_SIDE_PADDING * 2),
                        txtTotalOrignalAmount.Below(dateOfExpense, Constants.DRUG_LOOKUP_TOP_PADDING),
                        txtTotalOrignalAmount.Height().EqualTo(Constants.TEXT_FIELD_HEIGHT + 30),

                        txtPreviouslyPaidAmt.AtLeftOf(scrollableContainer, Constants.BUTTON_SIDE_PADDING),
                        txtPreviouslyPaidAmt.AtRightOf(View, Constants.BUTTON_SIDE_PADDING),
                        txtPreviouslyPaidAmt.WithSameWidth(scrollableContainer).Minus(Constants.BUTTON_SIDE_PADDING * 2),
                        txtPreviouslyPaidAmt.Below(txtTotalOrignalAmount, previouslyPaidOffset),
                        txtPreviouslyPaidAmt.Height().EqualTo(Constants.TEXT_FIELD_HEIGHT + 70),

                        btnSave.Below(txtPreviouslyPaidAmt, 50),
                        btnSave.WithSameCenterX(scrollableContainer),//view
                        btnSave.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                        btnSave.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                        btnSave.AtBottomOf(scrollableContainer, 180)
                    );
                }
                else
                {
                    View.AddConstraints(

                       scrollableContainer.AtTopOf(View),
                       scrollableContainer.AtLeftOf(View),
                       scrollableContainer.WithSameWidth(View),
                       scrollableContainer.AtBottomOf(View),

                       lblEnterClaimDetails.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                       lblEnterClaimDetails.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                       lblEnterClaimDetails.AtTopOf(scrollableContainer, 30),

                       dateOfExpense.AtLeftOf(scrollableContainer, Constants.BUTTON_SIDE_PADDING),
                       dateOfExpense.AtRightOf(View, Constants.BUTTON_SIDE_PADDING),
                       dateOfExpense.Below(lblEnterClaimDetails, Constants.DRUG_LOOKUP_TOP_PADDING),
                       dateOfExpense.Height().EqualTo(Constants.TEXT_FIELD_HEIGHT + 30),

                       txtTotalOrignalAmount.AtLeftOf(scrollableContainer, Constants.BUTTON_SIDE_PADDING),
                       txtTotalOrignalAmount.AtRightOf(View, Constants.BUTTON_SIDE_PADDING),
                       txtTotalOrignalAmount.Below(dateOfExpense, Constants.DRUG_LOOKUP_TOP_PADDING),
                       txtTotalOrignalAmount.Height().EqualTo(Constants.TEXT_FIELD_HEIGHT + 30),

                       txtPreviouslyPaidAmt.AtLeftOf(scrollableContainer, Constants.BUTTON_SIDE_PADDING),
                       txtPreviouslyPaidAmt.AtRightOf(View, Constants.BUTTON_SIDE_PADDING),
                       txtPreviouslyPaidAmt.Below(txtTotalOrignalAmount, previouslyPaidOffset),
                       txtPreviouslyPaidAmt.Height().EqualTo(Constants.TEXT_FIELD_HEIGHT + 70),

                       btnSave.Below(txtPreviouslyPaidAmt, 50),
                       btnSave.WithSameCenterX(scrollableContainer),
                       btnSave.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                       btnSave.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                       btnSave.AtBottomOf(scrollableContainer, 180));
                }
            }
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {

            txtTotalOrignalAmount.textField.ResignFirstResponder();
            txtPreviouslyPaidAmt.textField.ResignFirstResponder();
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ClaimDetails")
            {
                SetEmptyForTextField();
            }
        }

        private void SetEmptyForTextField()
        {
            if (_model.ClaimDetails.ClaimAmount == 0)
                txtTotalOrignalAmount.textField.Text = string.Empty;
            if (_model.ClaimDetails.OtherPaidAmount == 0)
                txtPreviouslyPaidAmt.textField.Text = string.Empty;
        }
    }
}