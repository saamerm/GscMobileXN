using CoreGraphics;
using Foundation;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Plugin.Messenger;
using UIKit;

namespace MobileClaims.iOS
{
    [Register("EligibilityParticipantsView")]
    public class EligibilityParticipantsView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
    {
        protected EligibilityParticipantsViewModel _model;

        protected UIScrollView scrollableContainer;

        protected UILabel planParticpantLabel;

        protected UITableView participantsList;

        protected UIView providerContainer;

        private IMvxMessenger _messenger;

        public EligibilityParticipantsView()
        {
        }

        public override void ViewDidLoad()
        {
            View = new GSCBaseView() 
            {
                BackgroundColor = Colors.BACKGROUND_COLOR 
            };

            base.ViewDidLoad();

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = "planParticipant".tr();
            base.NavigationItem.SetHidesBackButton(false, false);

            _model = (EligibilityParticipantsViewModel)ViewModel;
            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();

            this.AutomaticallyAdjustsScrollViewInsets = false;

            scrollableContainer = ((GSCBaseView)View).baseScrollContainer;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollableContainer);

            participantsList = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Plain);
            participantsList.RowHeight = Constants.SINGLE_SELECTION_CELL_HEIGHT;
            participantsList.TableHeaderView = new UIView();
            participantsList.SeparatorColor = Colors.Clear;
            participantsList.ShowsVerticalScrollIndicator = false;
            participantsList.ScrollEnabled = false;

            scrollableContainer.AddSubview(participantsList);

            planParticpantLabel = new UILabel();
            planParticpantLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.LG_HEADING_FONT_SIZE);
            planParticpantLabel.TextColor = Colors.DARK_GREY_COLOR;
            planParticpantLabel.BackgroundColor = Colors.Clear;
            planParticpantLabel.LineBreakMode = UILineBreakMode.WordWrap;
            planParticpantLabel.Lines = 0;
            planParticpantLabel.Text = "eligibilityPlanParticipantMessage".tr();
            planParticpantLabel.TextAlignment = UITextAlignment.Left;
            scrollableContainer.AddSubview(planParticpantLabel);

            MvxDeleteTableViewSource tableSource = new MvxDeleteTableViewSource(_model, participantsList, "ClaimParticipantTableCell", typeof(ClaimParticipantTableCell));
            participantsList.Source = tableSource;

            participantsList.ReloadData();
            var set = this.CreateBindingSet<EligibilityParticipantsView, EligibilityParticipantsViewModel>();
            set.Bind(tableSource).To(vm => vm.ParticipantsViewModel.ParticipantsActive);
           
            this.CreateBinding(tableSource).For(s => s.SelectionChangedCommand).To<EligibilityParticipantsViewModel>(vm => vm.ChangeParticipantCommand).Apply();
            set.Apply();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ((GSCBaseView)View).subscribeToBusyIndicator();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ((GSCBaseView)View).unsubscribeFromBusyIndicator();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            float listWidth = ViewContainerWidth - (Constants.DRUG_LOOKUP_SIDE_PADDING * 2);

            float centerX = ViewContainerWidth / 2;
            float yPos = ViewContentYPositionPadding;
           
            scrollableContainer.Frame = new CGRect(0, 0, ViewContainerWidth, ViewContainerHeight);

            planParticpantLabel.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, yPos, ViewContainerHeight - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, (float)planParticpantLabel.Frame.Height);
            planParticpantLabel.SizeToFit();
            yPos += (float)planParticpantLabel.Frame.Height + Constants.CLAIMS_DETAILS_COMPONENT_PADDING;

            float listHeight = _model.ParticipantsViewModel.Participants.Count * Constants.SINGLE_SELECTION_CELL_HEIGHT + Constants.SINGLE_SELECTION_VERTICAL_CELL_PADDING;

            participantsList.Frame = new CGRect(ViewContainerWidth / 2 - listWidth / 2, yPos, listWidth, listHeight);

            yPos += (float)participantsList.Frame.Height;

            scrollableContainer.ContentSize = new CGSize(ViewContainerWidth, yPos + GetBottomPadding());
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
                return Constants.CLAIMS_DETAILS_COMPONENT_PADDING;
            }
            else
            {
                return Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;;
            }
        }
    }
}