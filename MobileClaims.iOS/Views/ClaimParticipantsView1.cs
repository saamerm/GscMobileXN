using System;
using System.Linq;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugin.Messenger;
using UIKit;

namespace MobileClaims.iOS
{
    [Foundation.Register("ClaimParticipantsView1")]
    public class ClaimParticipantsView1 : GSCBaseViewPaddingController, IRehydrating, IGSCBaseViewImplementor
    {
        #region IRehydrating
        public bool Rehydrating
        {
            get;
            set;
        }
        public bool FinishedRehydrating
        {
            get;
            set;
        }
        #endregion
        protected ClaimParticipantsViewModel _model;

        protected UITableView tableView;

        protected UIView providerContainer;

        protected UIScrollView scrollContainer;

        protected UILabel providerTitle;

        protected UILabel providerName;
        protected UILabel providerAddress;
        protected UILabel providerCity;
        protected UILabel providerPhone;
        protected UILabel registrationNumber;

        protected UILabel planParticpantLabel;
        bool hasSelectedParticipantInThisViewAppearingCycle;
        private float COMPONENT_HEIGHT = 200;

        private MvxSubscriptionToken _claimparticipantchangerequested;
        private IMvxMessenger _messenger;

        private MvxSubscriptionToken _claimDetailsMessage;
        private MvxSubscriptionToken _treatmentDetailsListMessage;

        public ClaimParticipantsView1()
        {
            var rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
        }

        public override void ViewDidLoad()
        {
            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            base.ViewDidLoad();

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = "claimParticipantTitle".tr();

            base.NavigationController.NavigationBar.TintColor = Colors.HIGHLIGHT_COLOR;
            base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
            base.NavigationController.View.BackgroundColor = Colors.BACKGROUND_COLOR;
            this.AutomaticallyAdjustsScrollViewInsets = false;

            _model = (ClaimParticipantsViewModel)this.ViewModel;
            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();

            scrollContainer = new UIScrollView();
            scrollContainer.BackgroundColor = Colors.Clear;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollContainer);

            UIBarButtonItem barButton = new UIBarButtonItem();
            barButton.Title = " ".tr();
            this.NavigationController.NavigationBar.TopItem.BackBarButtonItem = barButton;

            tableView = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Plain);
            tableView.RowHeight = Constants.SINGLE_SELECTION_CELL_HEIGHT;
            tableView.TableHeaderView = new UIView();
            tableView.SeparatorColor = Colors.Clear;
            tableView.ShowsVerticalScrollIndicator = false;
            tableView.ScrollEnabled = false;

            scrollContainer.AddSubview(tableView);

            planParticpantLabel = new UILabel();
            planParticpantLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            planParticpantLabel.TextColor = Colors.DARK_GREY_COLOR;
            planParticpantLabel.BackgroundColor = Colors.Clear;
            planParticpantLabel.LineBreakMode = UILineBreakMode.WordWrap;
            planParticpantLabel.Lines = 0;
            planParticpantLabel.TextAlignment = UITextAlignment.Left;
            scrollContainer.AddSubview(planParticpantLabel);

            MvxDeleteTableViewSource tableSource = new MvxDeleteTableViewSource(_model, tableView, "ClaimParticipantTableCell", typeof(ClaimParticipantTableCell));
            tableView.Source = tableSource;

            var set = this.CreateBindingSet<ClaimParticipantsView1, Core.ViewModels.ClaimParticipantsViewModel>();
            set.Bind(tableSource).To(vm => vm.ParticipantsViewModel.Participants);
            set.Bind(planParticpantLabel).To(vm => vm.ClaimSubmissionType.Name).WithConversion("ChoosePlanParticipant");

            set.Bind(providerTitle).To(vm => vm.ClaimSubmissionType.Name).WithConversion("ClaimServiceProvider");
            set.Bind(providerName).To(vm => vm.ServiceProvider.BusinessName);
            set.Bind(providerAddress).To(vm => vm.ServiceProvider.Address);
            set.Bind(providerCity).To(vm => vm.ServiceProvider.FormattedAddress);
            set.Bind(providerPhone).To(vm => vm.ServiceProvider.Phone).WithConversion("PhonePrefix");
            set.Bind(registrationNumber).To(vm => vm.ServiceProvider.RegistrationNumber).WithConversion("RegistrationNumPrefix");

            this.CreateBinding(tableSource).For(s => s.SelectionChangedCommand).To<ClaimParticipantsViewModel>(vm => vm.RequestChangeParticipantCommand).Apply();

            set.Apply();

            //Catch for if ViewDidAppear does not fire
            if (_model.SelectedParticipant != null)
            {
                try
                {
                    Participant sp = _model.ParticipantsViewModel.Participants.Where(p => p.PlanMemberID.Equals(_model.ParticipantsViewModel.SelectedParticipant.PlanMemberID)).FirstOrDefault();
                    int rowNum = _model.ParticipantsViewModel.Participants.IndexOf(sp);
                    tableView.SelectRow(NSIndexPath.FromRowSection((nint)rowNum, (nint)0), false, UITableViewScrollPosition.None);

                }
                catch (Exception ex)
                {
                    MvxTrace.Trace(MvxTraceLevel.Error, ex.ToString());
                }
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ((GSCBaseView)View).subscribeToBusyIndicator();
            hasSelectedParticipantInThisViewAppearingCycle = false;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            _claimparticipantchangerequested = _messenger.Subscribe<ClaimParticipantChangeRequested>((message) =>
            {
                showParticipantChangedAlert();
            });

            tableView.ReloadData();
            if (_model.SelectedParticipant != null)
            {
                try
                {
                    Participant sp = _model.ParticipantsViewModel.Participants.Where(p => p.PlanMemberID.Equals(_model.ParticipantsViewModel.SelectedParticipant.PlanMemberID)).FirstOrDefault();
                    int rowNum = _model.ParticipantsViewModel.Participants.IndexOf(sp);
                    if (!hasSelectedParticipantInThisViewAppearingCycle)
                    {
                        tableView.ReloadData();
                        try
                        {
                            if (!tableView.VisibleCells[rowNum].Selected)
                            {
                                tableView.SelectRow(NSIndexPath.FromRowSection((nint)rowNum, (nint)0), false, UITableViewScrollPosition.None);
                            }
                        }
                        catch
                        {
                            tableView.SelectRow(NSIndexPath.FromRowSection((nint)rowNum, (nint)0), false, UITableViewScrollPosition.None);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MvxTrace.Trace(MvxTraceLevel.Error, ex.ToString());
                }
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            _messenger.Unsubscribe<ClaimParticipantChangeRequested>(_claimparticipantchangerequested);

            ((GSCBaseView)View).unsubscribeFromBusyIndicator();
        }

        protected void backButtonHides(bool hides)
        {
            if (hides)
            {
                base.NavigationItem.SetHidesBackButton(true, true);
            }
            else
            {
                base.NavigationItem.SetHidesBackButton(false, true);
            }
        }

        protected void showParticipantChangedAlert()
        {
            int buttonClicked = -1;

            UIAlertView alert = new UIAlertView()
            {
                Title = "participantChangedTitle".tr(),
                Message = "participantChangedDetail".tr() + " " + _model.RequestedParticipant.FullName + ". " + "participantChangedDetail2".tr()
            };
            alert.AddButton("ok".tr());
            alert.AddButton("cancelCaps".tr());

            alert.DismissWithClickedButtonIndex((nint)1, true);

            alert.Clicked += delegate (object a, UIButtonEventArgs b)
            {
                if ((int)b.ButtonIndex == 0)
                    _model.ChangeParticipantCommand.Execute(_model.RequestedParticipant);
                else if ((int)b.ButtonIndex == 1)
                    tableView.SelectRow(NSIndexPath.FromRowSection((nint)_model.ParticipantsViewModel.Participants.IndexOf(_model.SelectedParticipant), (nint)0), false, UITableViewScrollPosition.None);

                Participant sp = _model.ParticipantsViewModel.Participants.Where(p => p.PlanMemberID.Equals(_model.ParticipantsViewModel.SelectedParticipant.PlanMemberID)).FirstOrDefault();
                tableView.SelectRow(NSIndexPath.FromRowSection((nint)_model.ParticipantsViewModel.Participants.IndexOf(_model.ParticipantsViewModel.Participants.Where(p => p.PlanMemberID.Equals(_model.ParticipantsViewModel.SelectedParticipant.PlanMemberID)).FirstOrDefault()), (nint)0), false, UITableViewScrollPosition.None);


            };
            alert.Show();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            float tableWidth = ViewContainerWidth - (Constants.CONTENT_SIDE_PADDING * 2);

            float yPos = ViewContentYPositionPadding;
            float providerYPos = 0;
            float bottomPadding = Constants.IOS_7_TOP_PADDING;

            planParticpantLabel.Frame = new CGRect(Constants.CONTENT_SIDE_PADDING, yPos, ViewContainerWidth - Constants.CONTENT_SIDE_PADDING * 2, (float)planParticpantLabel.Frame.Height);
            planParticpantLabel.SizeToFit();
            yPos += (float)planParticpantLabel.Frame.Height + Constants.CLAIMS_TOP_PADDING;

            float listHeight = _model.ParticipantsViewModel.Participants.Count * (Constants.SINGLE_SELECTION_CELL_HEIGHT) + Constants.SINGLE_SELECTION_VERTICAL_CELL_PADDING;

            tableView.Frame = new CGRect(ViewContainerWidth / 2 - tableWidth / 2, yPos, tableWidth, listHeight);

            tableView.SetNeedsLayout();
            foreach (SingleSelectionTableViewCell tvCell in tableView.VisibleCells)
            {
                if (tvCell != null && tvCell.Selected)
                {
                    tvCell.SetSelected(true, false);
                    tvCell.SetNeedsLayout();
                }
            }

            yPos += listHeight;
            scrollContainer.Frame = new CGRect(0, 0, ViewContainerWidth, ViewContainerHeight);
            scrollContainer.ContentSize = new CGSize(ViewContainerWidth, yPos + bottomPadding);

            CGRect listFrame = (CGRect)tableView.Frame;
        }

        public float GetViewContainerWidth()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            }
            else
            {
                return (float)base.View.Bounds.Width;
            }
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

        float IGSCBaseViewImplementor.ViewContentYPositionPadding()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return 10;
            }
            else
            {
                return Constants.IOS_7_TOP_PADDING;
            }

        }

        private Participant _selectedParticipant;
        public Participant SelectedParticipant
        {
            get
            {
                return _selectedParticipant;
            }
            set
            {
                if (value != null)
                {
                    if (_selectedParticipant != null && _selectedParticipant != value)
                    {
                        UIAlertView _error = new UIAlertView("participantChangedTitle".tr(), "participantChangedDetail".tr() + _model.SelectedParticipant.FullName + "." + "participantChangedDetail2".tr(), null, "ok".tr(), null);
                        _error.Show();
                    }
                    _selectedParticipant = value;
                }
            }
        }
    }
}