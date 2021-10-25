using System;
using Foundation;
using MobileClaims.Core;
using MobileClaims.Core.Converters;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.AuditClaims
{
    public partial class AuditListView : GSCBaseViewController
    {
        private UIStringAttributes _auditListInstructionAttributes;

        private AuditListViewModel _viewModel;
        private UICollectionViewFlowLayout _layout;
        private AuditClaimsCollectionViewSource _auditClaimsCollectionViewSource;

        public AuditListView()
            : base()
        {
            _layout = new UICollectionViewFlowLayout()
            {
                SectionInset = new UIEdgeInsets(0, 0, 0, 0),
                MinimumLineSpacing = Constants.RecentClaimsCollectionViewCellSpacing
            };
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationController.SetNavigationBarHidden(false, false);
            NavigationItem.SetHidesBackButton(true, false);

            var bnbHeight = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;
            ScrollViewBottomConstraint.Constant = -((Constants.Bottom / 2) + bnbHeight);
            AuditClaimsCollection.ReloadData();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationController.NavigationBarHidden = false;
            NavigationItem.SetHidesBackButton(true, false);

            _viewModel = (AuditListViewModel)ViewModel;
            _viewModel.ClaimsFetched += OnClaimsFetched;

            SetFonts();

            AuditClaimsCollection.AllowsSelection = true;
            AuditClaimsCollection.AllowsMultipleSelection = false;
            AuditClaimsCollection.CollectionViewLayout = _layout;
            AuditClaimsCollection.RegisterNibForCell(AuditClaimCellView.Nib, AuditClaimCellView.Key);

            SetBindings();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            _viewModel.ClaimsFetched -= OnClaimsFetched;
        }

        private void SetBindings()
        {
            var boolOppositeValueConverter = new BoolOppositeValueConverter();

            _auditClaimsCollectionViewSource = new AuditClaimsCollectionViewSource(AuditClaimsCollection, _viewModel);
            AuditClaimsCollection.Source = _auditClaimsCollectionViewSource;

            var set = this.CreateBindingSet<AuditListView, AuditListViewModel>();
            set.Bind(NavigationItem).For(x => x.Title).To(vm => vm.Title);           
            set.Bind(AuditListInstructionLabel).For(x => x.AttributedText)
                .To(vm => vm.PromptTextTop)
                .WithConversion("StringToAttributedString", _auditListInstructionAttributes);

            set.Bind(AuditListLabel).To(vm => vm.Title);

            set.Bind(_auditClaimsCollectionViewSource).To(vm => vm.AuditClaims);
            set.Bind(_auditClaimsCollectionViewSource).For(source => source.SelectionChangedCommand).To(vm => vm.AuditSelectedCommand);
            set.Apply();

            AuditClaimsCollection.ReloadData();
            SetRecentClaimsCollectionHeightConstraint();
        }

        private void SetRecentClaimsCollectionHeightConstraint()
        {
            nfloat height = 0;
            if (Constants.IsPhone())
            {
                var rowCount = _viewModel.AuditClaims.Count;
                height = rowCount * Constants.AuditClaimsCellHeight;
                if (rowCount > 0)
                {
                    height += (rowCount - 1) * Constants.RecentClaimsCollectionViewCellSpacing;
                }
            }
            else
            {
                var rowCount = (int)Math.Ceiling((double)_viewModel.AuditClaims.Count / (double)2);
                height = rowCount * Constants.AuditClaimsCellHeight + (rowCount - 1) * Constants.RecentClaimsCollectionViewCellSpacing;
            }
            AuditClaimsCollectionHeightConstraint.Constant = height;
        }

        private void OnClaimsFetched(object sender, EventArgs e)
        {
            SetRecentClaimsCollectionHeightConstraint();
        }

        private void SetFonts()
        {
            _auditListInstructionAttributes = new UIStringAttributes
            {
                ForegroundColor = Colors.Black,
                Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.AuditListInstructionFontSize),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.AuditListInstructionLineSpacing,
                    LineHeightMultiple = Constants.AuditListInstructionLineSpacing
                }
            };

            AuditListLabel.TextColor = Colors.DARK_RED;
            AuditListLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.AuditListSectionHeaderFontSize);
            NoAuditLabel.Font = UIFont.FromName(Constants.NUNITO_BLACK, Constants.HEADING_FONT_SIZE);
            SetAuditListLabelAttributes();
        }

        private void SetAuditListLabelAttributes()
        {
            var auditListNotesParagraphAttributes = new UIStringAttributes
            {
                ForegroundColor = Colors.DarkGrayColor,
                Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.AuditListNotesFontSize),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.AuditListNotesLineSpacing,
                    LineHeightMultiple = Constants.AuditListNotesLineSpacing
                }
            };

            var auditListNotesFontAttributes = new UIStringAttributes
            {
                ForegroundColor = Colors.DarkGrayColor,
                Font = UIFont.FromName(Constants.NUNITO_BLACK, Constants.AuditListNotesFontSize),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.AuditListNotesLineSpacing,
                    LineHeightMultiple = Constants.AuditListNotesLineSpacing
                }
            };

            var mutableAttributeString = new NSMutableAttributedString($"{Resource.AuditListImportantNotesLabel} {Resource.AuditListNotes}");

            mutableAttributeString.SetAttributes(auditListNotesFontAttributes,
                new NSRange(0, Resource.AuditListImportantNotesLabel.Length));

            mutableAttributeString.SetAttributes(auditListNotesParagraphAttributes,
              new NSRange(Resource.AuditListImportantNotesLabel.Length + 1, Resource.AuditListNotes.Length));

            AuditListNotes.AttributedText = mutableAttributeString;
        }
    }
}