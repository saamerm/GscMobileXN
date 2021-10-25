using System;
using Foundation;
using MobileClaims.Core.ViewModels.DirectDeposit;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.DirectDeposit.DirectDepositError
{
    public partial class DDValidationErrorViewController : GSCBaseViewController<DDValidationErrorViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            base.NavigationItem.BackBarButtonItem = new UIBarButtonItem(" ".tr(), UIBarButtonItemStyle.Plain, null, null);
            NavigationController.NavigationBarHidden = false;

            base.NavigationItem.Title = MobileClaims.Core.Resource.DirectDeposit;
            SetBindings();
            SetFont();
            SetTextColor();
            ContinueButton.TouchUpInside += ContinueButton_TouchUpInside;
            ChangeInformationButton.TouchUpInside += ChangeInformationButton_TouchUpInside;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void SetBindings()
        {
            var set = this.CreateBindingSet<DDValidationErrorViewController, DDValidationErrorViewModel>();
            set.Bind(WarningLabel).For(x => x.Text).To(vm => vm.Warningmsg);
            set.Bind(NoteLabel).For(x => x.Text).To(vm => vm.ErrNote);
            set.Bind(NoteContactLabel).For(x => x.Text).To(vm => vm.ErrNoteContact);
            set.Bind(ExplainationLabel).For(x => x.Text).To(vm => vm.ErrExplaination);
            set.Bind(BankInfoLabel).For(x => x.Text).To(vm => vm.ErrBankInfo);
            set.Bind(TransitNumberLabel).For(x => x.Text).To(vm => vm.TransitNumberTitle);
            set.Bind(BankNumberLabel).For(x => x.Text).To(vm => vm.BankNumberTitle);
            set.Bind(AccountNumberLabel).For(x => x.Text).To(vm => vm.AccountNumberTitle);

            set.Bind(AccountNumber).To(vm => vm.AccountNumber);
            set.Bind(BankNumber).To(vm => vm.BankNumber);
            set.Bind(TransitNumber).To(vm => vm.TransitNumber);

            set.Bind(ContinueButton).For("Title").To(model => model.ContinueTitle);
            set.Bind(ChangeInformationButton).For("Title").To(model => model.ChangeInformationTitle);

            set.Apply();
        }

        private void SetFont()
        {
            BankInfoLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, 14);
            WarningLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14);
            ValidationErrorLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, 14);
            NoteContactLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14);
            NoteLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14);

            TransitNumberLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14);
            BankNumberLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14);
            AccountNumberLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14);

            TransitNumber.Font = UIFont.FromName(Constants.NUNITO_BOLD, 14);
            BankNumber.Font = UIFont.FromName(Constants.NUNITO_BOLD, 14);
            AccountNumber.Font = UIFont.FromName(Constants.NUNITO_BOLD, 14); 
        }

        private void SetTextColor()
        {
            BankInfoLabel.TextColor = Colors.HIGHLIGHT_COLOR;
            
            var highlightFontAttribute = new UIStringAttributes
            {
                ForegroundColor = Colors.DARK_RED
            };
            var errorNoteString = new NSMutableAttributedString(string.Format("{0} {1} {2} {3} {4}", ViewModel.ErrorTitle1, ViewModel.ErrorTitle2, ViewModel.ErrorTitle3, ViewModel.ErrorTitle4, ViewModel.ErrorTitle5));
            errorNoteString.SetAttributes(highlightFontAttribute, new NSRange(ViewModel.ErrorTitle1.Length + 1, ViewModel.ErrorTitle2.Length));

            errorNoteString.SetAttributes(highlightFontAttribute, new NSRange(ViewModel.ErrorTitle1.Length + 1 + ViewModel.ErrorTitle2.Length + 1 + ViewModel.ErrorTitle3.Length + 1, ViewModel.ErrorTitle4.Length));

            ValidationErrorLabel.Lines = 0;
            ValidationErrorLabel.LineBreakMode = UILineBreakMode.WordWrap;
            ValidationErrorLabel.AttributedText = errorNoteString;

            var highlightFontAttrib = new UIStringAttributes
            {
                ForegroundColor = Colors.HIGHLIGHT_COLOR,
                Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE)
            };
            var noteConfirmationString = new NSMutableAttributedString(string.Format("{0} {1} {2} {3}", ViewModel.ErrNoteContinue1, ViewModel.ContinueTitle, ViewModel.ErrNoteContinue3, ViewModel.ErrNoteContinue4));
            noteConfirmationString.SetAttributes(highlightFontAttrib, new NSRange(ViewModel.ErrNoteContinue1.Length + 1, ViewModel.ContinueTitle.Length));

            noteConfirmationString.SetAttributes(highlightFontAttrib, new NSRange(ViewModel.ErrNoteContinue1.Length + 1 + ViewModel.ContinueTitle.Length + 1 + ViewModel.ErrNoteContinue3.Length + 1, ViewModel.ErrNoteContinue4.Length - 1));

            NoteContinueLabel.Lines = 0;
            NoteContinueLabel.LineBreakMode = UILineBreakMode.WordWrap;
            NoteContinueLabel.AttributedText = noteConfirmationString;
        }
        
        private void ContinueButton_TouchUpInside(object sender, EventArgs e)
        {
            ViewModel.ContinueCommand.Execute();
        }

        private void ChangeInformationButton_TouchUpInside(object sender, EventArgs e)
        {
            ViewModel.ChangeInformationCommand.Execute();
        }

        public float GetViewContainerHeight()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return (float)((GSCBaseView)View).baseContainer.Bounds.Height - Helpers.BottomNavHeight();
            }
            else
            {
                return (float)base.View.Bounds.Height - Helpers.BottomNavHeight();
            }
        }
    }
}

