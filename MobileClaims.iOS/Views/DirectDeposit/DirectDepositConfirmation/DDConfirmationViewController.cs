using System;
using MobileClaims.Core.ViewModels.DirectDeposit;
using MvvmCross.Binding.BindingContext;
using UIKit;
using MobileClaims.iOS.Extensions;
using Foundation;

namespace MobileClaims.iOS.Views.DirectDeposit.DirectDepositConfirmation
{
    public partial class DDConfirmationViewController : GSCBaseViewController<DirectDepositConfirmationViewModel>
    {
        private bool _isAuthorisedDepositFund;

        public bool IsAuthorisedDepositFund
        {
            get => _isAuthorisedDepositFund;
            set
            {
                _isAuthorisedDepositFund = value;
                Highlight(_isAuthorisedDepositFund);
            }
        }
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            NavigationController.NavigationBarHidden = false;
            NavigationItem.SetHidesBackButton(false, false);
            base.NavigationItem.BackBarButtonItem = new UIBarButtonItem(" ".tr(), UIBarButtonItemStyle.Plain, null, null);

            SelectAuthoriseDepositFund.TouchUpInside += AuthoriseDepositFund_TouchUpInside;
            ContinueButton.TouchUpInside += ContinueButton_TouchUpInside;
            ChangeInformationButton.TouchUpInside += ChangeInformationButton_TouchUpInside;

            SetBindings();
            SetFont();
            SetTextColor();

            AuthoriseDepositFundLabel.SetLabel(Constants.NUNITO_SEMIBOLD, 14.0f, UIColor.FromRGB(27, 27, 27));
            AuthoriseDepositFundContainerView.BackgroundColor = Colors.BACKGROUND_COLOR;
            AuthoriseDepositFundContainerView.Layer.BorderColor = UIColor.FromRGB(0.88f, 0.88f, 0.88f).CGColor;
            AuthoriseDepositFundContainerView.Layer.BorderWidth = 1.0f;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void SetBindings()
        {
            var set = this.CreateBindingSet<DDConfirmationViewController, DirectDepositConfirmationViewModel>();

            set.Bind(ConfirmDDUpdateLabel).For(x => x.Text).To(vm => vm.ConfirmDDUpdateLabel);
            
            set.Bind(AuthoriseDepositFundLabel).For(x => x.Text).To(vm => vm.AuthoriseDepositFundLabel);

            set.Bind(BankAccountInformationLabel).For(x => x.Text).To(vm => vm.BankInfoTitle);
            set.Bind(TransitNumberLabel).For(x => x.Text).To(vm => vm.TransitNumberTitle);
            set.Bind(BankNumberLabel).For(x => x.Text).To(vm => vm.BankNumberTitle);
            set.Bind(AccountNumberLabel).For(x => x.Text).To(vm => vm.AccountNumberTitle);

            set.Bind(AccountNumber).For(x => x.Text).To(vm => vm.AccountNumber);
            set.Bind(BankNumber).To(vm => vm.BankNumberWithName);
            set.Bind(TransitNumber).To(vm => vm.TransitNumber);

            set.Bind(ContinueButton).For("Title").To(model => model.ContinueTitle);
            Title = MobileClaims.Core.Resource.DirectDeposit;
            set.Bind(ChangeInformationButton).For("Title").To(model => model.ChangeInformationTitle);

            set.Bind(this).For(x => x.IsAuthorisedDepositFund).To(model => model.IsAuthorisedDepositFund);

            set.Apply();
          
        }
        private void SetFont()
        {
            ConfirmDDUpdateLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, 14);
            AuthoriseDepositFundLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14);
            BankAccountInformationLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, 14);

            TransitNumberLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14);
            BankNumberLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14);
            AccountNumberLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14);

            TransitNumber.Font = UIFont.FromName(Constants.NUNITO_BOLD, 14);
            BankNumber.Font = UIFont.FromName(Constants.NUNITO_BOLD, 14);
            AccountNumber.Font = UIFont.FromName(Constants.NUNITO_BOLD, 14);
        }
        private void SetTextColor()
        {
            BankAccountInformationLabel.TextColor = Colors.HIGHLIGHT_COLOR;
            

            var highlightFontAttribute = new UIStringAttributes
            {
                ForegroundColor = Colors.HIGHLIGHT_COLOR,
                Font =  UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE)
            };
            var notConfirmationString = new NSMutableAttributedString(string.Format("{0} {1} {2} {3}", ViewModel.NoteConfirmation1Label, ViewModel.NoteConfirmation2Label, ViewModel.NoteConfirmation3Label, ViewModel.NoteConfirmation4Label));
            notConfirmationString.SetAttributes(highlightFontAttribute, new NSRange(ViewModel.NoteConfirmation1Label.Length + 1, ViewModel.NoteConfirmation2Label.Length));

            notConfirmationString.SetAttributes(highlightFontAttribute, new NSRange(ViewModel.NoteConfirmation1Label.Length + 1 + ViewModel.NoteConfirmation2Label.Length + 1 + ViewModel.NoteConfirmation3Label.Length + 1, ViewModel.NoteConfirmation4Label.Length-1));

            NoteConfirmationLabel.Lines = 0;
            NoteConfirmationLabel.LineBreakMode = UILineBreakMode.WordWrap;
            NoteConfirmationLabel.AttributedText = notConfirmationString;
        }

        private void AuthoriseDepositFund_TouchUpInside(object sender, EventArgs e)
        {
            ViewModel.SelectAuthoriseDepositFundCommand.Execute();
        }

        private void ContinueButton_TouchUpInside(object sender, EventArgs e)
        {
            ViewModel.ContinueCommand.Execute();
        }

        private void ChangeInformationButton_TouchUpInside(object sender, EventArgs e)
        {
            ViewModel.ChangeInformationCommand.Execute();
        }
        
        internal void Highlight(bool highlight)
        {
            AuthoriseDepositFundRoundImageView.Highlighted = highlight;
            
            if (highlight)
            {
                AuthoriseDepositFundLabel.TextColor = Colors.BACKGROUND_COLOR;
                AuthoriseDepositFundContainerView.BackgroundColor = Colors.HIGHLIGHT_COLOR;
            }
            else
            {
                AuthoriseDepositFundLabel.TextColor = UIColor.FromRGB(27, 27, 27);
                AuthoriseDepositFundContainerView.BackgroundColor = Colors.BACKGROUND_COLOR;              
            }
        }
    }
}

