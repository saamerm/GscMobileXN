using System;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Converters;
using MobileClaims.Core.ViewModels.DirectDeposit;
using MobileClaims.iOS.Extensions;
using MobileClaims.iOS.UI;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace MobileClaims.iOS.Views.DirectDeposit
{
    public partial class DirectDepositView : GSCBaseViewController<DirectDepositViewModel>
    {
        private ExpandableTableSource _directDepositSource;

        public DirectDepositView()
            : base()
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            var bnbHeight = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;
            UITableViewBottomConstraint.Constant = -((Constants.Bottom / 2) + bnbHeight);
            NavigationController.SetNavigationBarHidden(false, false);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationController.NavigationBarHidden = false;
            NavigationItem.SetHidesBackButton(true, false);

            base.NavigationItem.Title = MobileClaims.Core.Resource.DirectDeposit;

            var attributes = new UIStringAttributes
            {
                ForegroundColor = Colors.HIGHLIGHT_COLOR,
                Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.NAV_BAR_FONT_SIZE)
            };

            GetPaidFasterLabel.SetLabel(Constants.NUNITO_REGULAR, 12, Colors.Black);
            SignUpForDDLabel.SetLabel(Constants.NUNITO_REGULAR, 12, Colors.Black);

            SetDirectDepositTableViewFooter();
            SetBindings();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            var width = DirectDepositTableView.Bounds.Size.Width;
            var footerView = DirectDepositTableView.TableFooterView;
            if (footerView == null)
            {
                return;
            }

            var size = footerView.SystemLayoutSizeFittingSize(new CGSize(width, UIView.UILayoutFittingCompressedSize.Height));
            if (footerView.Frame.Size.Height != size.Height)
            {
                footerView.Frame = new CGRect(0, 0, width, size.Height);
            }
        }

        private void SetDirectDepositTableViewFooter()
        {
            _directDepositSource = new ExpandableTableSource(DirectDepositTableView, ViewModel);
            DirectDepositTableView.Source = _directDepositSource;

            var finePrintsLabel = new UILabel();
            var finePrintsContentsLabel = new UILabel();

            finePrintsLabel.SetLabel(Constants.NUNITO_BOLD, 14, UIColor.FromRGB(21, 21, 21));
            finePrintsLabel.Lines = 0;
            finePrintsLabel.LineBreakMode = UILineBreakMode.WordWrap;
            finePrintsLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            finePrintsLabel.Text = ViewModel.DisclaimerTitle;

            var _finePrintMessageAttribute = new UIStringAttributes
            {
                ForegroundColor = UIColor.FromRGB(11, 11, 11),
                Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.IsPhone() ? (nfloat)1.36 : (nfloat)1.40,
                    LineHeightMultiple = Constants.IsPhone() ? (nfloat)1.36 : (nfloat)1.40,
                    HeadIndent = 20,
                    FirstLineHeadIndent = 20
                }
            };
            var highlightFontAttribute = new UIStringAttributes
            {
                ForegroundColor = UIColor.FromRGB(11, 11, 11),
                Font = UIFont.FromName(Constants.NUNITO_BOLD, 14),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.IsPhone() ? (nfloat)1.36 : (nfloat)1.40,
                    LineHeightMultiple = Constants.IsPhone() ? (nfloat)1.36 : (nfloat)1.40,
                    HeadIndent = 20,
                    FirstLineHeadIndent = 20
                }
            };

            var noteBoldString = new NSMutableAttributedString(string.Format("{0}", ViewModel.DiscalimerNote3B));
            noteBoldString.SetAttributes(highlightFontAttribute, new NSRange(0, ViewModel.DiscalimerNote3B.Length));
            
            var a = $"{ViewModel.DiscalimerNote1}\n{ViewModel.DiscalimerNote2}\n";
            var b = new NSMutableAttributedString(a, _finePrintMessageAttribute);

            var para3 = new NSMutableAttributedString($"{ViewModel.DiscalimerNote3}", _finePrintMessageAttribute);

            b.Append(noteBoldString);
            b.Append(para3);

            finePrintsContentsLabel.Lines = 0;
            finePrintsContentsLabel.LineBreakMode = UILineBreakMode.WordWrap;
            finePrintsContentsLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            finePrintsContentsLabel.AttributedText = b;

            var continueButton = new GSButton();
            continueButton.TranslatesAutoresizingMaskIntoConstraints = false;
            continueButton.SetTitle(ViewModel.ContinueButtonTitle, UIControlState.Normal);

            var view = new UIView(new CGRect(0, 0, 0, 0));
            view.BackgroundColor = Colors.BACKGROUND_COLOR;
            view.AddSubview(finePrintsLabel);
            view.AddSubview(finePrintsContentsLabel);
            view.AddSubview(continueButton);

            finePrintsLabel.LeadingAnchor.ConstraintEqualTo(view.LeadingAnchor, 20).Active = true;
            finePrintsLabel.TrailingAnchor.ConstraintEqualTo(view.TrailingAnchor, -20).Active = true;
            finePrintsLabel.TopAnchor.ConstraintEqualTo(view.TopAnchor, 25).Active = true;

            finePrintsContentsLabel.TopAnchor.ConstraintEqualTo(finePrintsLabel.BottomAnchor, 25).Active = true;
            finePrintsContentsLabel.LeadingAnchor.ConstraintEqualTo(view.LeadingAnchor, 20).Active = true;
            finePrintsContentsLabel.TrailingAnchor.ConstraintEqualTo(view.TrailingAnchor, -20).Active = true;

            continueButton.CenterXAnchor.ConstraintEqualTo(view.CenterXAnchor).Active = true;
            continueButton.WidthAnchor.ConstraintEqualTo(234).Active = true;
            continueButton.HeightAnchor.ConstraintEqualTo(44).Active = true;
            continueButton.TopAnchor.ConstraintEqualTo(finePrintsContentsLabel.BottomAnchor, 36).Active = true;
            continueButton.BottomAnchor.ConstraintEqualTo(view.BottomAnchor, -40).Active = true;

            continueButton.TouchUpInside += ContinueButton_TouchUpInside;

            DirectDepositTableView.TableFooterView = view;
        }

        private void ContinueButton_TouchUpInside(object sender, EventArgs e)
        {
            ViewModel.ContinueCommand.Execute();
        }

        private void SetBindings()
        {
            var boolOppositeValueConverter = new BoolOppositeValueConverter();

            var set = this.CreateBindingSet<DirectDepositView, DirectDepositViewModel>();

            set.Bind(GetPaidFasterLabel).For(x => x.Text).To(vm => vm.SubTitle);
            set.Bind(SignUpForDDLabel).To(vm => vm.SubTitle2);
            set.Bind(_directDepositSource).To(vm => vm.Steps);

            set.Apply();
        }
    }
}