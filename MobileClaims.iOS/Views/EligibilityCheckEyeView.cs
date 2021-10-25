using System;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views
{
    public partial class EligibilityCheckEyeView : GSCBaseViewController<EligibilityCheckEyeViewModel>
    {
        protected EligibilityCheckEyeViewModel _model;

        public EligibilityCheckEyeView()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = "checkEligibilityTitle".tr();

            base.NavigationItem.SetHidesBackButton(false, false);

            _model = (EligibilityCheckEyeViewModel)ViewModel;

            this.AutomaticallyAdjustsScrollViewInsets = false;

            planParticipantLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.LG_HEADING_FONT_SIZE);
            planParticipantLabel.TextColor = Colors.DARK_GREY_COLOR;
            planParticipantLabel.BackgroundColor = Colors.Clear;
            planParticipantLabel.TextAlignment = UITextAlignment.Left;
            planParticipantLabel.LineBreakMode = UILineBreakMode.WordWrap;
            planParticipantLabel.Lines = 0;
            planParticipantLabel.Text = "checkEligibilityFor".tr() + " " + _model.EligibilityCheckType.Name.ToUpper() + " " + "for".tr() + " " + _model.SelectedParticipant.FullName.ToUpper();

            string totalChargeString = "totalChargeOfExamination".tr();

            switch (_model.EligibilityCheckType.ID)
            {
                case "GLASSES":
                    totalChargeString = "totalChargeForGlasses".tr();
                    break;
                case "CONTACTS":
                    totalChargeString = "totalForLenses".tr();
                    break;
                case "EYEEXAM":
                    totalChargeString = "totalChargeOfExamination".tr();
                    break;
            }

            dateOfPurchaseOrServiceComponent._parentController = this;
            serviceProvinceList._parentController = this;
            lensTypesList._parentController = this;

            totalCharge.TitleTextLabel = totalChargeString;

            DismmssVCTableViewSource provinceTableSource = new DismmssVCTableViewSource(serviceProvinceList.popoverController, serviceProvinceList.listController.tableView, "EligibilityProvinceTableCell", typeof(EligibilityProvinceTableCell));
            serviceProvinceList.listController.tableView.Source = provinceTableSource;

            DismmssVCTableViewSource lensTableSource = new DismmssVCTableViewSource(lensTypesList.popoverController, lensTypesList.listController.tableView, "EligibilityOptionTableCell", typeof(EligibilityOptionTableCell));
            lensTypesList.listController.tableView.Source = lensTableSource;

            eligibilityNote1.Text = "eligibilityNote1".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada);
            eligibilityNote2.Text = "eligibilityNote2".tr();

            submitButton.SetTitle("checkEligibilityTitle".tr(), UIControlState.Normal);
            submitButton.TouchUpInside += HandleTouchUpInside;

            _model.OnInvalidEligibilityCheck += HandleOnInvalidEligibilityCheck;

            var set = this.CreateBindingSet<EligibilityCheckEyeView, Core.ViewModels.EligibilityCheckEyeViewModel>();
            set.Bind(serviceDescriptionComponent.detailsLabel).To(vm => vm.EligibilityCheckType.Name);
            set.Bind(dateOfPurchaseOrServiceComponent.dateController.datePicker).To(vm => vm.DateOfPurchaseOrService);
            set.Bind(dateOfPurchaseOrServiceComponent.detailsLabel).To(vm => vm.DateOfPurchaseOrService).WithConversion("DateToString").OneWay();
            set.Bind(totalCharge.textField).To(vm => vm.TotalCharge);
            set.Bind(provinceTableSource).To(vm => vm.ServiceProvinces);
            set.Bind(provinceTableSource).For(s => s.SelectedItem).To(vm => vm.ProvinceOfService);
            set.Bind(serviceProvinceList.detailsLabel).To(vm => vm.ProvinceOfService.Name);
            set.Bind(this.submitButton).To(vm => vm.SubmitEligibilityCheckCommand);
            set.Bind(lensTableSource).To(vm => vm.LensTypes);
            set.Bind(lensTableSource).For(s => s.SelectedItem).To(vm => vm.LensType);
            set.Bind(lensTypesList.detailsLabel).To(vm => vm.LensType.Name);
            set.Apply();

            _model.DateOfPurchaseOrService = DateTime.Now;
        }

        void HandleOnInvalidEligibilityCheck(object sender, EventArgs e)
        {
            if (_model.InvalidTotalAmount || _model.MissingTotalAmount)
                totalCharge.showError(_model.InvalidTotalAmount ? "invalidTotalAmountCharged".tr() : "sepcifyTotalAmountCharged".tr());
            else
                totalCharge.hideError();

            if (_model.InvalidDateOfPurchaseOrService)
                dateOfPurchaseOrServiceComponent.showError("dateofPurchase12Months".tr());
            else
                dateOfPurchaseOrServiceComponent.hideError();

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

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            if (!_model.IsLensTypeVisible)
            {
                lensTypesList.Hidden = true;
            }

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}