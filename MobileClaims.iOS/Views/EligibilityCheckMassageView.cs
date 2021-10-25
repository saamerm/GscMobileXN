using System;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views
{
    public partial class EligibilityCheckMassageView : GSCBaseViewController<EligibilityCheckMassageViewModel>
    {
        protected EligibilityCheckMassageViewModel _model;

        public EligibilityCheckMassageView()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = "checkEligibilityTitle".tr();
            base.NavigationItem.SetHidesBackButton(true, false);

            base.NavigationItem.SetHidesBackButton(false, false);

            _model = (EligibilityCheckMassageViewModel)ViewModel;

            planParticipantLabel.Text = "checkEligibilityFor".tr() + " " + _model.EligibilityCheckType.Name.ToUpper() + " " + "for".tr() + " " + _model.SelectedParticipant.FullName.ToUpper();

            typeOfTreatmentList._parentController = this;
            lengthOfTreatmentList._parentController = this;
            dateOfTreatmentComponent._parentController = this;
            serviceProvinceList._parentController = this;

            DismmssVCTableViewSource typeTableSource = new DismmssVCTableViewSource(typeOfTreatmentList.popoverController, typeOfTreatmentList.listController.tableView, "ClaimSubmissionBenefitCell", typeof(ClaimSubmissionBenefitCell));
            typeOfTreatmentList.listController.tableView.Source = typeTableSource;

            DismmssVCTableViewSource lengthTableSource = new DismmssVCTableViewSource(lengthOfTreatmentList.popoverController, lengthOfTreatmentList.listController.tableView, "EligibilityOptionTableCell", typeof(EligibilityOptionTableCell));
            lengthOfTreatmentList.listController.tableView.Source = lengthTableSource;

            DismmssVCTableViewSource provinceTableSource = new DismmssVCTableViewSource(serviceProvinceList.popoverController, serviceProvinceList.listController.tableView, "EligibilityProvinceTableCell", typeof(EligibilityProvinceTableCell));
            serviceProvinceList.listController.tableView.Source = provinceTableSource;

            eligibilityNote1.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.SMALL_FONT_SIZE);
            eligibilityNote1.TextColor = Colors.DARK_GREY_COLOR;
            eligibilityNote1.TextAlignment = UITextAlignment.Left;
            eligibilityNote1.BackgroundColor = Colors.Clear;
            eligibilityNote1.LineBreakMode = UILineBreakMode.WordWrap;
            eligibilityNote1.Lines = 0;
            eligibilityNote1.Text = "eligibilityNote1".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada);

            eligibilityNote2.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.SMALL_FONT_SIZE);
            eligibilityNote2.TextColor = Colors.DARK_GREY_COLOR;
            eligibilityNote2.TextAlignment = UITextAlignment.Left;
            eligibilityNote2.BackgroundColor = Colors.Clear;
            eligibilityNote2.LineBreakMode = UILineBreakMode.WordWrap;
            eligibilityNote2.Lines = 0;
            eligibilityNote2.Text = "eligibilityNote2".tr();

            submitButton.SetTitle("checkEligibilityTitle".tr(), UIControlState.Normal);
            submitButton.TouchUpInside += HandleTouchUpInside;

            _model.OnInvalidEligibilityCheck += HandleOnInvalidEligibilityCheck;

            var set = this.CreateBindingSet<EligibilityCheckMassageView, Core.ViewModels.EligibilityCheckMassageViewModel>();
            set.Bind(dateOfTreatmentComponent.dateController.datePicker).To(vm => vm.DateOfTreatment);
            set.Bind(dateOfTreatmentComponent.detailsLabel).To(vm => vm.DateOfTreatment).WithConversion("DateToString").OneWay();
            set.Bind(totalAmountOfVisit.textField).To(vm => vm.TotalAmountOfVisit);
            set.Bind(typeTableSource).To(vm => vm.TypesOfTreatment);
            set.Bind(typeTableSource).For(s => s.SelectedItem).To(vm => vm.TypeOfTreatment);
            set.Bind(typeOfTreatmentList.detailsLabel).To(vm => vm.TypeOfTreatment.Name);
            set.Bind(lengthTableSource).To(vm => vm.LengthsOfTreatment);
            set.Bind(lengthTableSource).For(s => s.SelectedItem).To(vm => vm.LengthOfTreatment);
            set.Bind(lengthOfTreatmentList.detailsLabel).To(vm => vm.LengthOfTreatment.Name);
            set.Bind(provinceTableSource).To(vm => vm.ServiceProvinces);
            set.Bind(provinceTableSource).For(s => s.SelectedItem).To(vm => vm.ProvinceOfService);
            set.Bind(serviceProvinceList.detailsLabel).To(vm => vm.ProvinceOfService.Name);
            set.Bind(this.submitButton).To(vm => vm.SubmitEligibilityCheckCommand);
            set.Apply();

            typeOfTreatmentList.listController.tableView.ReloadData();
            lengthOfTreatmentList.listController.tableView.ReloadData();
            serviceProvinceList.listController.tableView.ReloadData();

            _model.DateOfTreatment = DateTime.Now;
        }

        void HandleOnInvalidEligibilityCheck(object sender, EventArgs e)
        {
            if (_model.InvalidTotalAmount || _model.MissingTotalAmount)
                totalAmountOfVisit.showError(_model.InvalidTotalAmount ? "eligibilityTotalAmountError".tr() : "eligibilityTotalAmountMissing".tr());
            else
                totalAmountOfVisit.hideError();

            if (_model.InvalidDateOfTreatment)
                dateOfTreatmentComponent.showError("dateofTreatment12Months".tr());
            else
                dateOfTreatmentComponent.hideError();

        }

        void HandleTouchUpInside(object sender, EventArgs e)
        {
            dismissKeyboard();
        }

        void HandleViewTapped(object sender, EventArgs e)
        {
            dismissKeyboard();
        }

        void dismissKeyboard()
        {
            this.View.EndEditing(true);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            dismissKeyboard();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

