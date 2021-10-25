using System;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views
{
    public partial class EligibilityCheckCPView : GSCBaseViewController<EligibilityCheckCPViewModel>
    {

        protected EligibilityCheckCPViewModel _model;

        public EligibilityCheckCPView()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = "checkEligibilityTitle".tr();

            _model = (EligibilityCheckCPViewModel)ViewModel;

            base.NavigationItem.SetHidesBackButton(false, false);

            planParticipantLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            planParticipantLabel.TextColor = Colors.DARK_GREY_COLOR;
            planParticipantLabel.BackgroundColor = Colors.Clear;
            planParticipantLabel.TextAlignment = UITextAlignment.Left;
            planParticipantLabel.LineBreakMode = UILineBreakMode.WordWrap;
            planParticipantLabel.Lines = 0;
            planParticipantLabel.Text = "checkEligibilityFor".tr() + " " + _model.EligibilityCheckType.Name.ToUpper() + " " + "for".tr() + " " + _model.SelectedParticipant.FullName.ToUpper();

            typeOfTreatmentList._parentController = this;
            dateOfTreatmentComponent._parentController = this;
            serviceProvinceList._parentController = this;


            DismmssVCTableViewSource typeTableSource = new DismmssVCTableViewSource(typeOfTreatmentList.popoverController, typeOfTreatmentList.listController.tableView, "ClaimSubmissionBenefitCell", typeof(ClaimSubmissionBenefitCell));
            typeOfTreatmentList.listController.tableView.Source = typeTableSource;

            DismmssVCTableViewSource provinceTableSource = new DismmssVCTableViewSource(serviceProvinceList.popoverController, serviceProvinceList.listController.tableView, "EligibilityProvinceTableCell", typeof(EligibilityProvinceTableCell));
            serviceProvinceList.listController.tableView.Source = provinceTableSource;

            eligibilityNote1.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            eligibilityNote1.TextColor = Colors.DARK_GREY_COLOR;
            eligibilityNote1.TextAlignment = UITextAlignment.Left;
            eligibilityNote1.BackgroundColor = Colors.Clear;
            eligibilityNote1.LineBreakMode = UILineBreakMode.WordWrap;
            eligibilityNote1.Lines = 0;
            eligibilityNote1.Text = "eligibilityNote1".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada);

            eligibilityNote2.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            eligibilityNote2.TextColor = Colors.DARK_GREY_COLOR;
            eligibilityNote2.TextAlignment = UITextAlignment.Left;
            eligibilityNote2.BackgroundColor = Colors.Clear;
            eligibilityNote2.LineBreakMode = UILineBreakMode.WordWrap;
            eligibilityNote2.Lines = 0;
            eligibilityNote2.Text = "eligibilityNote2".tr();

            submitButton.SetTitle("checkEligibilityTitle".tr(), UIControlState.Normal);
            submitButton.TouchUpInside += HandleTouchUpInside;

            _model.OnInvalidEligibilityCheck += HandleOnInvalidEligibilityCheck;

            var set = this.CreateBindingSet<EligibilityCheckCPView, Core.ViewModels.EligibilityCheckCPViewModel>();
            set.Bind(dateOfTreatmentComponent.dateController.datePicker).To(vm => vm.DateOfTreatment);
            set.Bind(dateOfTreatmentComponent.detailsLabel).To(vm => vm.DateOfTreatment).WithConversion("DateToString").OneWay();
            set.Bind(totalAmountOfVisit.textField).To(vm => vm.TotalAmountOfVisit);
            set.Bind(typeTableSource).To(vm => vm.TypesOfTreatment);
            set.Bind(typeTableSource).For(s => s.SelectedItem).To(vm => vm.TypeOfTreatment);
            set.Bind(typeOfTreatmentList.detailsLabel).To(vm => vm.TypeOfTreatment.Name);
            set.Bind(provinceTableSource).To(vm => vm.ServiceProvinces);
            set.Bind(provinceTableSource).For(s => s.SelectedItem).To(vm => vm.ProvinceOfService);
            set.Bind(serviceProvinceList.detailsLabel).To(vm => vm.ProvinceOfService.Name);
            set.Bind(this.submitButton).To(vm => vm.SubmitEligibilityCheckCommand);
            set.Apply();

            typeOfTreatmentList.listController.tableView.ReloadData();
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

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            dismissKeyboard();
        }
    }
}