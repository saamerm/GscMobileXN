using System;
using Foundation;
using MobileClaims.Core.Converters;
using MobileClaims.Core.Validators;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Converters;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.TrreatmentDetails
{
    public partial class ClaimTreatmentDetailsEntryDentalView : GSCBaseViewController
    {
        private ClaimTreatmentDetailsEntryDentalViewModel _viewModel;

        public UIBarButtonItem SaveButton { get; private set; }
        public UIBarButtonItem CancelButton { get; private set; }

        public ClaimTreatmentDetailsEntryDentalView()
            : base()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetNavigationBarButton();

            _viewModel = (ClaimTreatmentDetailsEntryDentalViewModel)ViewModel;

            var maxDate = (NSDate)DateTime.Today;
            var minDate = (NSDate)DateTime.Today.AddMonths(TreatmentDetailsEntryValidationParams.TREATMENT_DATE_ALLOWED_WITHHIN_MONTHS);
            TreatmentDatePicker.DatePickerController.datePicker.MaximumDate = maxDate;
            TreatmentDatePicker.DatePickerController.datePicker.MinimumDate = minDate;

            ProcedureCodeTextField.KeyboardType = UIKeyboardType.NumberPad;
            ProcedureCodeTextField.MaxLength = 5;

            ToothCodeTextField.KeyboardType = UIKeyboardType.NumberPad;
            ToothCodeTextField.MaxLength = 2;

            ToothSurfaceTextField.KeyboardType = UIKeyboardType.Default;
            ToothSurfaceTextField.MaxLength = 5;

            DentistFeeTextField.KeyboardType = UIKeyboardType.DecimalPad;
            DentistFeeTextField.MaxLength = 7;

            LaboratoryChargeTextField.KeyboardType = UIKeyboardType.DecimalPad;
            LaboratoryChargeTextField.MaxLength = 7;

            AmountPaidTextField.KeyboardType = UIKeyboardType.DecimalPad;
            AmountPaidTextField.MaxLength = 7;

            SetBinding();
        }

        private void SetBinding()
        {
            var boolToCGColorValueConverter = new BoolToCGColorValueConverter();
            var invertedBoolValueConverter = new BoolOppositeValueConverter();
            var dateToStringValueConverter = new DateToStringValueConverter();
            var nullableValueConverter = new NullableValueConverter();

            var set = this.CreateBindingSet<ClaimTreatmentDetailsEntryDentalView, ClaimTreatmentDetailsEntryDentalViewModel>();
            set.Bind(NavigationItem).To(x => x.Title).For(vm => vm.Title);
            set.Bind(SaveButton).For("Title").To(vm => vm.SaveLabel);
            set.Bind(CancelButton).For("Title").To(vm => vm.CancelLabel);

            set.Bind(TreatmentDatePicker.Label).For(x => x.Text).To(vm => vm.DateOfTreatmentLabel);
            set.Bind(TreatmentDatePicker.DatePickerController.datePicker).To(vm => vm.DateOfTreatment);
            set.Bind(TreatmentDatePicker.DetailsLabel).To(vm => vm.DateOfTreatment).WithConversion(dateToStringValueConverter).OneWay();
            set.Bind(TreatmentDatePicker).For(x => x.ErrorText).To(vm => vm.DateOfTreatmentErrorText);
            set.Bind(TreatmentDatePicker).For(x => x.ShouldShowError).To(vm => vm.DateValid).WithConversion(invertedBoolValueConverter);

            set.Bind(ProcedureCodeTextField).For(x => x.Placeholder).To(vm => vm.PlaceHolderText);
            set.Bind(ProcedureCodeTextField).For(x => x.Text).To(vm => vm.ProcedureCodeLabel);
            set.Bind(ProcedureCodeTextField.TextField).To(vm => vm.ProcedureCode).WithConversion(nullableValueConverter);
            set.Bind(ProcedureCodeTextField).For(x => x.ErrorText).To(vm => vm.ProcedureCodeErrorText);
            set.Bind(ProcedureCodeTextField).For(x => x.ShouldShowError).To(vm => vm.ProcedureCodeValid).WithConversion(invertedBoolValueConverter);

            set.Bind(ToothCodeTextField).For(x => x.Enabled).To(vm => vm.IsToothCodeRequired);
            set.Bind(ToothCodeTextField).For(x => x.Placeholder).To(vm => vm.PlaceHolderText);
            set.Bind(ToothCodeTextField).For(x => x.Text).To(vm => vm.ToothCodeLabel);
            set.Bind(ToothCodeTextField.TextField).To(vm => vm.ToothCode).WithConversion(nullableValueConverter);
            set.Bind(ToothCodeTextField).For(x => x.ErrorText).To(vm => vm.ToothCodeErrorText);
            set.Bind(ToothCodeTextField).For(x => x.ShouldShowError).To(vm => vm.ToothCodeValid).WithConversion(invertedBoolValueConverter);

            set.Bind(ToothSurfaceTextField).For(x => x.Enabled).To(vm => vm.IsToothSurfaceRequired);
            set.Bind(ToothSurfaceTextField).For(x => x.Placeholder).To(vm => vm.PlaceHolderText);
            set.Bind(ToothSurfaceTextField.Label).For(x => x.Text).To(vm => vm.ToothSurfacesLabel);
            set.Bind(ToothSurfaceTextField.TextField).To(vm => vm.ToothSurfaces);
            set.Bind(ToothSurfaceTextField).For(x => x.ErrorText).To(vm => vm.ToothSurfaceErrorText);
            set.Bind(ToothSurfaceTextField).For(x => x.ShouldShowError).To(vm => vm.ToothSurfacesValid).WithConversion(invertedBoolValueConverter);

            set.Bind(DentistFeeTextField).For(x => x.Placeholder).To(vm => vm.PlaceHolderText);
            set.Bind(DentistFeeTextField.Label).For(x => x.Text).To(vm => vm.DentistsFeeLabel);
            set.Bind(DentistFeeTextField.TextField).To(vm => vm.DentistsFee).WithConversion(nullableValueConverter);
            set.Bind(DentistFeeTextField).For(x => x.ErrorText).To(vm => vm.DentistFeesErrorText);
            set.Bind(DentistFeeTextField).For(x => x.ShouldShowError).To(vm => vm.DentistsFeeValid).WithConversion(invertedBoolValueConverter);

            set.Bind(LaboratoryChargeTextField).For(x => x.Enabled).To(vm => vm.IsLabChargeRequired);
            set.Bind(LaboratoryChargeTextField).For(x => x.Placeholder).To(vm => vm.PlaceHolderText);
            set.Bind(LaboratoryChargeTextField.Label).For(x => x.Text).To(vm => vm.LaboratoryChargeLabel);
            set.Bind(LaboratoryChargeTextField.TextField).To(vm => vm.LaboratoryCharge).WithConversion(nullableValueConverter);
            set.Bind(LaboratoryChargeTextField).For(x => x.ErrorText).To(vm => vm.LabChargesErrorText);
            set.Bind(LaboratoryChargeTextField).For(x => x.ShouldShowError).To(vm => vm.LaboratoryChargeValid).WithConversion(invertedBoolValueConverter);
            set.Bind(LaboratoryChargeTextField.TextField.Layer).For(x => x.BorderColor).To(vm => vm.LaboratoryChargeValid).WithConversion(boolToCGColorValueConverter);

            set.Bind(AmountPaidTextField).For(x => x.Hidden).To(vm => vm.IsAlternateCarrierAmountVisible).WithConversion(invertedBoolValueConverter);
            set.Bind(AmountPaidTextField).For(x => x.Placeholder).To(vm => vm.PlaceHolderText);
            set.Bind(AmountPaidTextField.Label).For(x => x.Text).To(vm => vm.AlternateCarrierAmountLabel);
            set.Bind(AmountPaidTextField.TextField).To(vm => vm.AlternateCarrierAmount).WithConversion(nullableValueConverter);
            set.Bind(AmountPaidTextField).For(x => x.ErrorText).To(vm => vm.AlternateCarrierAmountErrorText);
            set.Bind(AmountPaidTextField).For(x => x.ShouldShowError).To(vm => vm.AlternateCarrierAmountValid).WithConversion(invertedBoolValueConverter);

            set.Bind(DeleteButton).For("Title").To(vm => vm.DeleteLabel);
            set.Bind(DeleteButton).To(vm => vm.DeleteTreatmentCommand);
            set.Bind(DeleteButton).For(x => x.Hidden).To(vm => vm.EditMode).WithConversion(invertedBoolValueConverter);

            set.Apply();
        }

        private void SetNavigationBarButton()
        {
            base.NavigationItem.SetHidesBackButton(true, false);

            CancelButton = CreateNavigationBarButton();
            CancelButton.Clicked += CancelButton_Clicked;

            SaveButton = CreateNavigationBarButton();
            SaveButton.Clicked += SaveButton_Clicked;

            base.NavigationItem.LeftBarButtonItem = CancelButton;
            base.NavigationItem.RightBarButtonItem = SaveButton;
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            _viewModel.CancelCommand.Execute(null);
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            _viewModel.SaveTreatmentCommand.Execute(null);
        }

        private UIBarButtonItem CreateNavigationBarButton()
        {
            var attributes = new UITextAttributes
            {
                Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.NAV_BAR_BUTTON_SIZE)
            };

            var button = new UIBarButtonItem
            {
                Style = UIBarButtonItemStyle.Plain,
                TintColor = Colors.HIGHLIGHT_COLOR
            };
            button.SetTitleTextAttributes(attributes, UIControlState.Normal);
            button.SetTitleTextAttributes(attributes, UIControlState.Highlighted);
            return button;
        }
    }
}