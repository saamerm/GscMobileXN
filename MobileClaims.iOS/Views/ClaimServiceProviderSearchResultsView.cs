using System;
using CoreGraphics;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS
{
    [Foundation.Register("ClaimServiceProviderSearchResultsView")]
    public class ClaimServiceProviderSearchResultsView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
    {
        protected UIScrollView scrollContainer;
        protected UITableView submissionTableView;
        protected UIScrollView providerScrollView;
        protected UILabel submissionTypeLabel;
        protected UILabel submissionDesLabel;
        protected GSButton continueButton;

        protected UILabel noResultsLabel;

        private float BUTTON_WIDTH = Constants.IsPhone() ? 270 : 400;
        private float BUTTON_HEIGHT = Constants.IsPhone() ? 40 : 60;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ClaimServiceProviderSearchResultsViewModel _model = (ClaimServiceProviderSearchResultsViewModel)ViewModel;

            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = "searchResults".tr().ToUpper();

            base.NavigationController.NavigationBar.TintColor = Colors.HIGHLIGHT_COLOR;
            base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
            base.NavigationController.View.BackgroundColor = Colors.BACKGROUND_COLOR;
            this.AutomaticallyAdjustsScrollViewInsets = false;

            scrollContainer = new UIScrollView();
            ((GSCBaseView)View).baseContainer.AddSubview(scrollContainer);

            submissionTableView = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Plain);
            submissionTableView.RowHeight = Constants.SINGLE_SELECTION_ACCENTED_TITLE_CELL_HEIGHT;
            submissionTableView.TableHeaderView = new UIView();
            submissionTableView.SeparatorColor = Colors.Clear;
            submissionTableView.ShowsVerticalScrollIndicator = true;
            MvxDeleteTableViewSource providerSource = new MvxDeleteTableViewSource(_model, submissionTableView, "ClaimSubmissionOneTitleThreeSubtitlesTableCell", typeof(ClaimSubmissionOneTitleThreeSubtitlesTableCell));
            submissionTableView.Source = providerSource;
            scrollContainer.AddSubview(submissionTableView);

            submissionTypeLabel = new UILabel();
            submissionTypeLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            submissionTypeLabel.TextColor = Colors.DARK_GREY_COLOR;
            submissionTypeLabel.TextAlignment = UITextAlignment.Left;
            submissionTypeLabel.Lines = 0;
            submissionTypeLabel.LineBreakMode = UILineBreakMode.WordWrap;
            submissionTypeLabel.Text = "BLANK";
            submissionTypeLabel.BackgroundColor = Colors.Clear;

            scrollContainer.AddSubview(submissionTypeLabel);

            noResultsLabel = new UILabel();
            noResultsLabel.Text = "noMatchingProviders".tr();
            noResultsLabel.BackgroundColor = Colors.Clear;
            noResultsLabel.TextColor = Colors.DARK_GREY_COLOR;
            noResultsLabel.TextAlignment = UITextAlignment.Center;
            noResultsLabel.Lines = 0;
            noResultsLabel.LineBreakMode = UILineBreakMode.WordWrap;
            noResultsLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);

            scrollContainer.AddSubview(noResultsLabel);

            noResultsLabel.Hidden = true;

            submissionDesLabel = new UILabel();
            submissionDesLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SUB_HEADING_FONT_SIZE);
            submissionDesLabel.Text = "BLANK";
            submissionDesLabel.Lines = 0;
            submissionDesLabel.LineBreakMode = UILineBreakMode.WordWrap;
            submissionDesLabel.TextColor = Colors.DARK_GREY_COLOR;
            submissionDesLabel.TextAlignment = UITextAlignment.Left;
            submissionDesLabel.BackgroundColor = Colors.Clear;
            scrollContainer.AddSubview(submissionDesLabel);

            continueButton = new GSButton();
            continueButton.SetTitle("useThisProvider".tr(), UIControlState.Normal);
            scrollContainer.AddSubview(continueButton);

            var set = this.CreateBindingSet<ClaimServiceProviderSearchResultsView, ClaimServiceProviderProvideInformationViewModel>();
            set.Bind(continueButton).To(vm => vm.EnterServiceProviderCommand);
            set.Apply();

            SetLabels();

            var _set = this.CreateBindingSet<ClaimServiceProviderSearchResultsView, ClaimServiceProviderSearchResultsViewModel>();
            _set.Bind(providerSource).To(vm => vm.ServiceProviderSearchResults);
            this.CreateBinding(providerSource).For(s => s.SelectionChangedCommand).To<ClaimServiceProviderSearchResultsViewModel>(vm => vm.ServiceProviderSelectedCommand).Apply();
            _set.Apply();

            submissionTableView.ReloadData();

            _model.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case "ServiceProviderSearchResults":
                        SetFrames();
                        if (_model.ServiceProviderSearchResults == null || _model.ServiceProviderSearchResults.Count < 1)
                        {
                            setNoResultsLabel(true);
                        }
                        else
                        {
                            setNoResultsLabel(false);
                        }
                        break;
                    case "ProviderType":
                        SetFrames();
                        SetLabels();
                        break;
                    default:
                        break;
                }
            };

            if (_model.ServiceProviderSearchResults == null || _model.ServiceProviderSearchResults.Count < 1)
            {
                setNoResultsLabel(true);
            }

        }

        private bool noResults = false;
        private void setNoResultsLabel(bool visible)
        {

            noResults = visible;

            if (visible)
            {
                noResultsLabel.Hidden = false;
                submissionTypeLabel.Hidden = submissionTableView.Hidden = true;
            }
            else
            {
                noResultsLabel.Hidden = true;
                scrollContainer.Hidden = submissionTypeLabel.Hidden = submissionTableView.Hidden = false;
            }

            View.SetNeedsLayout();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetFrames();

        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            ((GSCBaseView)View).unsubscribeFromBusyIndicator();
        }

        private void SetFrames()
        {
            base.ViewDidLayoutSubviews();
            if (View.Superview == null)
            {
                return;
            }

            ClaimServiceProviderSearchResultsViewModel _model = (ClaimServiceProviderSearchResultsViewModel)ViewModel;
            float startY = ViewContentYPositionPadding;
            float contentHeight = 0.0f;
            float innerPadding = 20;
            float contentPadding = Constants.IsPhone() ? 20 : 30;

            submissionTypeLabel.Frame = new CGRect(Constants.CONTENT_SIDE_PADDING, startY, ViewContainerWidth - Constants.CONTENT_SIDE_PADDING * 2, (float)submissionTypeLabel.Frame.Height);
            submissionTypeLabel.SizeToFit();

            float listY = (float)submissionTypeLabel.Frame.Y + (float)submissionTypeLabel.Frame.Height + innerPadding;

            if (_model.ServiceProviderSearchResults != null)
            {
                float listHeight = _model.ServiceProviderSearchResults.Count * (Constants.SINGLE_SELECTION_ACCENTED_TITLE_CELL_HEIGHT) + Constants.SINGLE_SELECTION_VERTICAL_CELL_PADDING;
                submissionTableView.Frame = new CGRect(Constants.SINGLE_SELECTION_TOP_TABLE_PADDING, listY, ViewContainerWidth - Constants.SINGLE_SELECTION_TOP_TABLE_PADDING * 2, listHeight);
                contentHeight = (float)submissionTableView.Frame.Height + (float)submissionTableView.Frame.Y + 10;
            }

            noResultsLabel.Frame = new CGRect(10, startY, ViewContainerWidth - 20, (float)noResultsLabel.Frame.Height);
            noResultsLabel.SizeToFit();
            noResultsLabel.Frame = new CGRect(10, startY, ViewContainerWidth - 20, (float)noResultsLabel.Frame.Height);

            float submissionDesYPos = noResults ? (float)noResultsLabel.Frame.Y + (float)noResultsLabel.Frame.Height + contentPadding : listY + (float)submissionTableView.Frame.Height + contentPadding;

            submissionDesLabel.SizeToFit();
            submissionDesLabel.Frame = new CGRect(Constants.CONTENT_SIDE_PADDING, submissionDesYPos, ViewContainerWidth - Constants.CONTENT_SIDE_PADDING * 2, (float)submissionDesLabel.Frame.Height);
            submissionDesLabel.SizeToFit();

            continueButton.Frame = new CGRect(ViewContainerWidth / 2 - BUTTON_WIDTH / 2, (float)submissionDesLabel.Frame.Y + (float)submissionDesLabel.Frame.Height + contentPadding, BUTTON_WIDTH, BUTTON_HEIGHT);
            contentHeight = (float)continueButton.Frame.Height + (float)continueButton.Frame.Y;

            float bottomPadding = Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;

            scrollContainer.Frame = new CGRect(0, 0, ViewContainerWidth, ViewContainerHeight);
            scrollContainer.ContentSize = new CGSize(ViewContainerWidth, contentHeight + bottomPadding);
        }

        public float GetViewContainerWidth()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            }
            else
            {
                return (float)View.Superview.Frame.Width;
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
                return (float)View.Superview.Frame.Height - Helpers.BottomNavHeight();
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

        private void SetLabels()
        {
            ClaimServiceProviderSearchResultsViewModel _model = (ClaimServiceProviderSearchResultsViewModel)ViewModel;

            submissionTypeLabel.Text = "selectAProviderFromTheList".tr();

            submissionDesLabel.Text = "stillCantFindYourProvider".tr() + "\r\n\r\n";
            submissionDesLabel.Text += "ifYouCannotFindYourProvider".FormatWithBrandKeywords(LocalizableBrand.GSC);
            continueButton.SetTitle("provideInformationAboutMyProvider".tr(), UIControlState.Normal);

            View.SetNeedsLayout();
        }
    }
}