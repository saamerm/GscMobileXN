using System;
using System.Linq;
using Foundation;
using MobileClaims.Core.Converters;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Extensions;
using MobileClaims.iOS.UI;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugin.Messenger;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimParticipants
{
    public partial class ClaimParticipantsView : GSCBaseViewController
    {
        private UIStringAttributes _auditListInstructionAttributes;

        private ClaimParticipantsViewModel _viewModel;
        private UICollectionViewFlowLayout _claimParticipantsFlowLayout;
        private ClaimParticipantCollectionViewSource _participantCollectionViewSource;
        private ClaimParticipantCollectionViewSource _otherParticipantCollectionViewSource;

        private MvxSubscriptionToken _claimparticipantchangerequested;
        private IMvxMessenger _messenger;
        private bool _hasSelectedParticipantInThisViewAppearingCycle;

        public ClaimParticipantsView()
            : base()
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            var bnbHeight = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;
            ScrollViewBottomConstraint.Constant = -((Constants.Bottom / 2) + bnbHeight);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            _claimparticipantchangerequested = _messenger.Subscribe<ClaimParticipantChangeRequested>((message) =>
            {
                ShowParticipantChangedAlert();
            });

            SelectCollectionViewItemIfNeeded();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            _messenger.Unsubscribe<ClaimParticipantChangeRequested>(_claimparticipantchangerequested);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            _viewModel = (ClaimParticipantsViewModel)this.ViewModel;

            SetFonts();

            SetupParticipantCollectionView();

            SetupOtherParticipantCollectionView();

            SetBindings();

            SelectCollectionViewItemIfNeeded();
        }

        protected void ShowParticipantChangedAlert()
        {
            UIAlertView alert = new UIAlertView()
            {
                Title = "participantChangedTitle".tr(),
                Message = "participantChangedDetail".tr() + " " + _viewModel.RequestedParticipant.FullName + ". " + "participantChangedDetail2".tr()
            };

            alert.AddButton("ok".tr());
            alert.AddButton("cancelCaps".tr());
            alert.DismissWithClickedButtonIndex(1, true);
            alert.Clicked += Alert_Clicked;
            alert.Show();
        }

        private void SelectCollectionViewItemIfNeeded()
        {
            if (_viewModel.SelectedParticipant != null)
            {
                try
                {
                    int rowNum = 0;
                    var sp = _viewModel.ParticipantsViewModel.Participants
                        .Where(p => p.PlanMemberID.Equals(_viewModel.ParticipantsViewModel.SelectedParticipant.PlanMemberID))
                        .FirstOrDefault();
                    if (sp == null)
                    {
                        sp = _viewModel.ParticipantsViewModel.OtherParticipants
                            .Where(p => p.PlanMemberID.Equals(_viewModel.ParticipantsViewModel.SelectedParticipant.PlanMemberID))
                            .FirstOrDefault();
                        rowNum = _viewModel.ParticipantsViewModel.OtherParticipants.IndexOf(sp);
                        SelectCollectionViewItem(OtherParticipantsCollectionView, ParticipantsCollectionView, rowNum);
                    }
                    else
                    {
                        rowNum = _viewModel.ParticipantsViewModel.Participants.IndexOf(sp);
                        SelectCollectionViewItem(ParticipantsCollectionView, OtherParticipantsCollectionView, rowNum);
                    }

                }
                catch (Exception ex)
                {
                    MvxTrace.Trace(MvxTraceLevel.Error, ex.ToString());
                }
            }
        }

        private void SelectCollectionViewItem(UICollectionView collectionView, UICollectionView secondaryCollectionView, int rowNum)
        {
            if (!_hasSelectedParticipantInThisViewAppearingCycle)
            {
                collectionView.ReloadData();
                collectionView.LayoutIfNeeded();
                try
                {
                    if (!collectionView.VisibleCells[rowNum].Selected)
                    {
                        collectionView.SelectItem(NSIndexPath.FromRowSection(rowNum, 0), false, UICollectionViewScrollPosition.None);
                        var list = secondaryCollectionView.GetIndexPathsForSelectedItems();
                        foreach (var index in list)
                        {
                            secondaryCollectionView.DeselectItem(index, false);
                        }
                    }
                }
                catch
                {
                    collectionView.SelectItem(NSIndexPath.FromRowSection(rowNum, 0), false, UICollectionViewScrollPosition.None);
                }
            }
        }

        private void Alert_Clicked(object sender, UIButtonEventArgs eventArgs)
        {
            var alert = (UIAlertView)sender;
            alert.Clicked -= Alert_Clicked;

            if (eventArgs.ButtonIndex == 0)
            {
                _viewModel.ChangeParticipantCommand.Execute(_viewModel.RequestedParticipant);
            }
            else if (eventArgs.ButtonIndex == 1)
            {
                SelectCollectionViewItemIfNeeded();
            }
        }

        private void SetFonts()
        {
            SetAuditListLabelAttributes();
            OtherParticipantsHeaderLabel.SetLabel(Constants.LEAGUE_GOTHIC, Constants.ClaimSummarySectionHeaderFontSize, Colors.HIGHLIGHT_COLOR);
            CommonParticipantHeaderLabel.SetLabel(Constants.LEAGUE_GOTHIC, Constants.LG_HEADING_FONT_SIZE, Colors.DARK_GREY_COLOR);
        }

        private void SetAuditListLabelAttributes()
        {
            var programNameAttributes = new UIStringAttributes
            {
                ForegroundColor = Colors.HIGHLIGHT_COLOR,
                Font = UIFont.FromName("NunitoSans-BoldItalic", Constants.OtherParticipantsNotesFontSize),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.OtherParticipantsNotesLineSpacing,
                    LineHeightMultiple = Constants.OtherParticipantsNotesLineSpacing
                }
            };

            var otherParticipantsNotesAttributes = new UIStringAttributes
            {
                ForegroundColor = Colors.DARK_GREY_COLOR,
                Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.OtherParticipantsNotesFontSize),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.OtherParticipantsNotesLineSpacing,
                    LineHeightMultiple = Constants.OtherParticipantsNotesLineSpacing
                }
            };

            var additionalInfoMessageAttributes = new UIStringAttributes
            {
                ForegroundColor = Colors.HIGHLIGHT_COLOR,
                Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.OtherParticipantsNotesFontSize),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.OtherParticipantsNotesLineSpacing,
                    LineHeightMultiple = Constants.OtherParticipantsNotesLineSpacing
                }
            };

            var mutableAttributeString = new NSMutableAttributedString(_viewModel.VisionEnhancementsMessage);
            mutableAttributeString.SetAttributes(otherParticipantsNotesAttributes,
                new NSRange(0, _viewModel.VisionEnhancementsMessage.Length));

            var indexOfProgramName = _viewModel.VisionEnhancementsMessage.IndexOf(_viewModel.ProgramName);
            mutableAttributeString.SetAttributes(programNameAttributes,
                new NSRange(indexOfProgramName, _viewModel.ProgramName.Length));

            var indexOfAdditionalInfo = _viewModel.VisionEnhancementsMessage.IndexOf(_viewModel.ProgramAdditionalInfo);
            mutableAttributeString.SetAttributes(additionalInfoMessageAttributes,
              new NSRange(indexOfAdditionalInfo, _viewModel.ProgramAdditionalInfo.Length));

            OtherParticipantsNotes.AttributedText = mutableAttributeString;
        }

        private void SetupParticipantCollectionView()
        {
            _claimParticipantsFlowLayout = new UICollectionViewFlowLayout()
            {
                SectionInset = new UIEdgeInsets(0, 0, 0, 0),
                MinimumLineSpacing = Constants.RecentClaimsCollectionViewCellSpacing,
            };

            ParticipantsCollectionView.AllowsSelection = true;
            ParticipantsCollectionView.AllowsMultipleSelection = false;
            ParticipantsCollectionView.RegisterNibForCell(ClaimParticipantsCollectionViewCell.Nib, ClaimParticipantsCollectionViewCell.Key);
            ParticipantsCollectionView.CollectionViewLayout = _claimParticipantsFlowLayout;
        }

        private void SetupOtherParticipantCollectionView()
        {
            var _otherClaimParticipantFlowLayout = new UICollectionViewFlowLayout()
            {
                SectionInset = new UIEdgeInsets(0, 0, 0, 0),
                MinimumLineSpacing = Constants.RecentClaimsCollectionViewCellSpacing,
            };

            OtherParticipantsCollectionView.AllowsSelection = true;
            OtherParticipantsCollectionView.AllowsMultipleSelection = false;
            OtherParticipantsCollectionView.RegisterNibForCell(ClaimParticipantsCollectionViewCell.Nib, ClaimParticipantsCollectionViewCell.Key);
            OtherParticipantsCollectionView.CollectionViewLayout = _otherClaimParticipantFlowLayout;
        }

        private void SetBindings()
        {
            _participantCollectionViewSource = new ClaimParticipantCollectionViewSource(ParticipantsCollectionView,
                _viewModel.ParticipantsViewModel.Participants);
            ParticipantsCollectionView.Source = _participantCollectionViewSource;

            _otherParticipantCollectionViewSource = new ClaimParticipantCollectionViewSource(OtherParticipantsCollectionView,
                _viewModel.ParticipantsViewModel.OtherParticipants);
            OtherParticipantsCollectionView.Source = _otherParticipantCollectionViewSource;

            var boolOppositeValueConverter = new BoolOppositeValueConverter();

            var set = this.CreateBindingSet<ClaimParticipantsView, ClaimParticipantsViewModel>();
            set.Bind(NavigationItem).For(x => x.Title).To(vm => vm.Title);
            set.Bind(CommonParticipantHeaderLabel).To(vm => vm.ClaimSubmissionType.Name).WithConversion("ChoosePlanParticipant");
            set.Bind(OtherParticipantsHeaderLabel).To(vm => vm.Important);
         
            set.Bind(OtherParticipantsHeaderLabel).For(x => x.Hidden).To(vm => vm.IsVisionEnhancementApplicable).WithConversion(boolOppositeValueConverter, null);
            set.Bind(OtherParticipantsNotes).For(x => x.Hidden).To(vm => vm.IsVisionEnhancementApplicable).WithConversion(boolOppositeValueConverter, null);

            set.Bind(_participantCollectionViewSource)
                .To(vm => vm.ParticipantsViewModel.Participants);

            set.Bind(_participantCollectionViewSource)
                .For(s => s.SelectionChangedCommand)
                .To(vm => vm.RequestChangeParticipantCommand);

            set.Bind(_otherParticipantCollectionViewSource)
               .To(vm => vm.ParticipantsViewModel.OtherParticipants);

            set.Bind(_otherParticipantCollectionViewSource)
                .For(s => s.SelectionChangedCommand)
                .To(vm => vm.RequestChangeParticipantCommand);

            set.Apply();

            ParticipantsCollectionView.ReloadData();
            OtherParticipantsCollectionView.ReloadData();
            SetClaimParticipantCollectionHeightConstraint();
            SetClaimOtherParticipantCollectionHeightConstraint();
        }

        private void SetClaimParticipantCollectionHeightConstraint()
        {
            nfloat height = 0;

            var rowCount = _viewModel.ParticipantsViewModel.Participants.Count;
            height = rowCount * Constants.SINGLE_SELECTION_CELL_HEIGHT;
            if (rowCount > 0)
            {
                height += (rowCount - 1) * Constants.RecentClaimsCollectionViewCellSpacing;
            }
            ParticipantCollectionViewHeightConstraint.Constant = height;
        }

        private void SetClaimOtherParticipantCollectionHeightConstraint()
        {
            nfloat height = 0;

            var rowCount = _viewModel.ParticipantsViewModel.OtherParticipants.Count;
            height = rowCount * Constants.SINGLE_SELECTION_CELL_HEIGHT;
            if (rowCount > 0)
            {
                height += (rowCount - 1) * Constants.RecentClaimsCollectionViewCellSpacing;
            }
            OtherParticipantsCollectionViewHeightConstraint.Constant = height;
        }
    }
}