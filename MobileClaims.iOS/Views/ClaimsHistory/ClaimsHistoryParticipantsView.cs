using System;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using UIKit;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Entities;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS.Views.ClaimsHistory
{
    [Register("ClaimsHistoryParticipantsView")]
    public class ClaimsHistoryParticipantsView : GSCBaseViewController
    {
        private LeagueGothic24Label participantLab;
        private UITableView participantsTable;
        private ClaimsHistoryParticipantsViewModel model;
        private UIScrollView rootScrollView;
        private GSButton24 doneButt;

        private float topMarginForScrollView =  Constants.NAV_HEIGHT + 10f;
        private float topMargin = 10f;
        private float bottomMargin = Helpers.BottomNavHeight() + 10f;
        private float dropdownSidePadding = Constants.DROPDOWN_SIDE_PADDING;
        private float participantsTableHeight = 0f;
        private float doneButtHeight = 60f;


        public ClaimsHistoryParticipantsView()
        {
        }

        public override void ViewDidLoad()
        {
            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            base.ViewDidLoad();
            model = this.ViewModel as ClaimsHistoryParticipantsViewModel;
            model.ParticipantSelectionComplete += ModelParticipantSelectionComplete;
            participantsTableHeight = model.Participants.Count * (Constants.SINGLE_SELECTION_CELL_HEIGHT) + Constants.SINGLE_SELECTION_VERTICAL_CELL_PADDING;

            rootScrollView = ((GSCBaseView)View).baseScrollContainer;
           
            ((GSCBaseView)View).baseContainer.AddSubview(rootScrollView);

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.SetHidesBackButton(true, false);
            base.NavigationItem.Title = model.ParticipantLabel.ToUpper();

            participantLab = new LeagueGothic24Label();
            participantLab.Text = model.SelectThePlanParticipantLabel;
            participantLab.TextColor = Colors.DARK_GREY_COLOR;
            rootScrollView.AddSubview(participantLab);

            participantsTable = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Plain);
            participantsTable.RowHeight = Constants.SINGLE_SELECTION_CELL_HEIGHT;
            participantsTable.TableHeaderView = new UIView();
            participantsTable.SeparatorColor = Colors.Clear;
            participantsTable.ShowsVerticalScrollIndicator = false;
            participantsTable.ScrollEnabled = false;
            rootScrollView.AddSubview(participantsTable);

            doneButt = new GSButton24();
            doneButt.SetTitle(model.DoneLabel.ToUpper(), UIControlState.Normal);
            rootScrollView.AddSubview(doneButt);

            MvxDeleteTableViewSource tableSource = new MvxDeleteTableViewSource(model, participantsTable, "ClaimHistoryParticipantCell", typeof(ClaimHistoryParticipantCell));
            participantsTable.Source = tableSource;

            var set = this.CreateBindingSet<ClaimsHistoryParticipantsView, ClaimsHistoryParticipantsViewModel>();
            set.Bind(tableSource).To(vm => vm.Participants);
            set.Bind(tableSource).For(s => s.SelectedItem).To(vm => vm.SelectedParticipant);
            set.Bind(doneButt).To(vm => vm.DoneCommand);
            set.Apply();

            if (model.Participants != null && model.Participants.Count > 0)
            {
                if (model.SelectedParticipant != null)
                {
                    int selectedRow = 0;
                    for (int i = 0; i < model.Participants.Count; i++)
                    {
                        Participant p = model.Participants[i] as Participant;
                        if (p.PlanMemberID == model.SelectedParticipant.PlanMemberID)
                        {
                            selectedRow = i;
                            tableSource.selectedRow = NSIndexPath.FromRowSection((nint)selectedRow, (nint)0);
                        }
                    }


                }
            }
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            var width = (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            var height = (float)((GSCBaseView)View).baseContainer.Bounds.Height;
            rootScrollView.Frame = new CGRect(0, 10, width, height);

            float yPos = 10;

            participantLab.Frame = new CGRect(Constants.CONTENT_SIDE_PADDING, 10, width - Constants.CONTENT_SIDE_PADDING * 2, participantLab.Frame.Height);
            participantLab.SizeToFit();

            yPos += (float)participantLab.Frame.Height + 10;

            participantsTable.Frame = new CGRect(Constants.CONTENT_SIDE_PADDING, yPos, width - Constants.CONTENT_SIDE_PADDING * 2, participantsTableHeight);

            yPos += participantsTableHeight + 10;

            doneButt.Frame = new CGRect(Constants.CONTENT_SIDE_PADDING, yPos, width - Constants.CONTENT_SIDE_PADDING * 2, doneButtHeight);

            yPos += (float)doneButt.Frame.Height + 10;

            rootScrollView.ContentSize = new CGSize(width, yPos + 10);
        }

        void ModelParticipantSelectionComplete(object sender, EventArgs e)
        {
            int backTo = 2;
            base.NavigationController.PopToViewController(base.NavigationController.ViewControllers[base.NavigationController.ViewControllers.Length - backTo], true);
        }
    }
}