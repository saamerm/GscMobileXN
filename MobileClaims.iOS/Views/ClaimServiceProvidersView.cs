using System;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Platform;
using UIKit;

namespace MobileClaims.iOS
{
    [Register("ClaimServiceProvidersView")]
    public class ClaimServiceProvidersView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
    {
        protected UIScrollView scrollContainer;
        protected UITableView submissionTableView;
        protected UIScrollView providerScrollView;
        protected UILabel submissionTypeLabel;
        protected UILabel byNameLabel, byPhoneLabel;
        protected DefaultTextField initialText, lastNameText, phoneText;
        protected GSButton byNameButton, byPhoneButton;

        protected ClaimServiceProvidersViewModel _model;

        private float BUTTON_WIDTH = Constants.IsPhone() ? 220 : 300;
        private float BUTTON_HEIGHT = Constants.IsPhone() ? 40 : 60;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _model = (ClaimServiceProvidersViewModel)ViewModel;

            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = "healthProviders".tr();

            this.AutomaticallyAdjustsScrollViewInsets = false;

            scrollContainer = ((GSCBaseView)View).baseScrollContainer;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollContainer);
            ((GSCBaseView)View).ViewTapped += HandleViewTapped;

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
            submissionTypeLabel.TextAlignment = UITextAlignment.Center;
            submissionTypeLabel.LineBreakMode = UILineBreakMode.WordWrap;

            submissionTypeLabel.Text = "BLANK";
            submissionTypeLabel.BackgroundColor = Colors.Clear;
            submissionTypeLabel.LineBreakMode = UILineBreakMode.WordWrap;
            submissionTypeLabel.Lines = 0;
            scrollContainer.AddSubview(submissionTypeLabel);

            var set = this.CreateBindingSet<ClaimServiceProvidersView, ClaimServiceProvidersViewModel>();

            byNameLabel = new UILabel();
            byNameLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            byNameLabel.TextColor = Colors.MED_GREY_COLOR;
            byNameLabel.TextAlignment = UITextAlignment.Left;
            byNameLabel.Lines = 0;
            byNameLabel.LineBreakMode = UILineBreakMode.WordWrap;
            byNameLabel.Text = "searchByName".tr();
            byNameLabel.BackgroundColor = Colors.Clear;
            scrollContainer.AddSubview(byNameLabel);

            initialText = new DefaultTextField();
            initialText.Placeholder = "initial".tr();
            scrollContainer.AddSubview(initialText);

            lastNameText = new DefaultTextField();
            lastNameText.Placeholder = "lastName".tr();
            scrollContainer.AddSubview(lastNameText);

            byNameButton = new GSButton();
            byNameButton.SetTitle("searchByNameButton".tr(), UIControlState.Normal);
            scrollContainer.AddSubview(byNameButton);

            byNameButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                _model.SearchByNameCommand.Execute(null);
            };

            byPhoneLabel = new UILabel();
            byPhoneLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            byPhoneLabel.TextColor = Colors.MED_GREY_COLOR;
            byPhoneLabel.TextAlignment = UITextAlignment.Left;
            byPhoneLabel.Lines = 0;
            byPhoneLabel.LineBreakMode = UILineBreakMode.WordWrap;
            byPhoneLabel.Text = "searchByPhone".tr();
            byPhoneLabel.BackgroundColor = Colors.Clear;
            scrollContainer.AddSubview(byPhoneLabel);

            phoneText = new DefaultTextField();
            phoneText.Placeholder = "phoneNumber".tr();
            scrollContainer.AddSubview(phoneText);

            byPhoneButton = new GSButton();
            byPhoneButton.SetTitle("searchByPhoneButton".tr(), UIControlState.Normal);
            scrollContainer.AddSubview(byPhoneButton);
            byPhoneButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                _model.SearchByPhoneCommand.Execute(null);
            };

            _model.OnNoResults += (object sender, EventArgs e) =>
            {
                InvokeOnMainThread(() =>
                {
                    UIAlertView _error = new UIAlertView("", "noMatchingProviders".tr(), null, "ok".tr(), null);
                    _error.Show();
                });

            };

            set.Bind(initialText).To(vm => vm.Initial);
            set.Bind(lastNameText).To(vm => vm.LastName);
            set.Bind(phoneText).To(vm => vm.PhoneNumber);


            set.Bind(providerSource).To(vm => vm.ServiceProviders);
            this.CreateBinding(providerSource).For(s => s.SelectionChangedCommand).To<ClaimServiceProvidersViewModel>(vm => vm.ServiceProviderSelectedCommand).Apply();
            set.Apply();

            submissionTableView.ReloadData();

            SetLabels();

            _model.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case "ServiceProviders":
                        SetFrames();
                        break;
                    case "SelectedServiceProvider":
                        SetLabels();
                        break;
                    default:
                        break;
                }
            };

        }

        void HandleViewTapped(object sender, EventArgs e)
        {
            dismissKeyboard();
        }

        void dismissKeyboard()
        {
            this.View.EndEditing(true);
        }

        bool hasAppeared = false;
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            hasAppeared = true;
            View.SetNeedsLayout();
            ((GSCBaseView)View).subscribeToBusyIndicator();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (_model.SelectedServiceProvider != null && submissionTableView != null)
            {
                try
                {
                    int participantIndex = _model.ServiceProviders.IndexOf(_model.SelectedServiceProvider);
                    NSIndexPath path = NSIndexPath.FromRowSection((nint)participantIndex, (nint)0);
                    submissionTableView.SelectRow(path, false, UITableViewScrollPosition.None);
                }
                catch (Exception ex)
                {
                    MvxTrace.Trace(MvxTraceLevel.Error, ex.ToString());
                }
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            dismissKeyboard();
            ((GSCBaseView)View).unsubscribeFromBusyIndicator();
        }

        public override void ViewDidLayoutSubviews()
        {
            SetFrames();
            base.ViewDidLayoutSubviews();
        }

        private void SetFrames()
        {
            base.ViewDidLayoutSubviews();

            if (View.Superview == null)
                return;
            float startY = Helpers.StatusBarHeight();
            float contentHeight = 0.0f;
            float itemPadding = Constants.CLAIMS_DETAILS_COMPONENT_PADDING;

            float yPos = ViewContentYPositionPadding;
            float extraPos = startY;

            float entryHeight = Constants.ENTRY_HEIGHT;

            submissionTypeLabel.Frame = new CGRect(Constants.CONTENT_SIDE_PADDING, yPos, ViewContainerWidth - Constants.CONTENT_SIDE_PADDING * 2, (float)submissionTypeLabel.Frame.Height);
            submissionTypeLabel.SizeToFit();

            yPos += (float)submissionTypeLabel.Frame.Height + Constants.CLAIMS_TOP_PADDING;
            if (_model.ServiceProviders != null)
            {
                float listY = yPos;
                float listHeight = _model.ServiceProviders.Count * (Constants.SINGLE_SELECTION_ACCENTED_TITLE_CELL_HEIGHT + Constants.SINGLE_SELECTION_VERTICAL_CELL_PADDING);
                submissionTableView.Frame = new CGRect(Constants.SINGLE_SELECTION_TOP_TABLE_PADDING, listY, ViewContainerWidth - Constants.SINGLE_SELECTION_TOP_TABLE_PADDING * 2, listHeight);
                contentHeight = (float)submissionTableView.Frame.Height + (float)submissionTableView.Frame.Y + 10 + Helpers.BottomNavHeight();
                yPos += (float)submissionTableView.Frame.Height;
            }

            float initialWidth = (float)(ViewContainerWidth * 0.4 - 10 - Constants.DRUG_LOOKUP_SIDE_PADDING);
            float lastNameWidth = (float)(ViewContainerWidth * 0.6 - Constants.DRUG_LOOKUP_SIDE_PADDING);

            byNameLabel.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, yPos + 10, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, (float)byNameLabel.Frame.Height);
            byNameLabel.SizeToFit();
            initialText.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, (float)byNameLabel.Frame.Y + (float)byNameLabel.Frame.Height + 5, initialWidth, entryHeight);
            lastNameText.Frame = new CGRect(initialText.Frame.X + initialText.Frame.Width + 10, (float)byNameLabel.Frame.Y + (float)byNameLabel.Frame.Height + 5, lastNameWidth, entryHeight);
            byNameButton.Frame = new CGRect(ViewContainerWidth / 2 - BUTTON_WIDTH / 2, lastNameText.Frame.Y + lastNameText.Frame.Height + 20, BUTTON_WIDTH, BUTTON_HEIGHT);
            byPhoneLabel.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, byNameButton.Frame.Y + byNameButton.Frame.Height + 30, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, (float)byPhoneLabel.Frame.Height);
            byPhoneLabel.SizeToFit();
            phoneText.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, (float)byPhoneLabel.Frame.Y + (float)byPhoneLabel.Frame.Height + 5, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, entryHeight);
            byPhoneButton.Frame = new CGRect(ViewContainerWidth / 2 - BUTTON_WIDTH / 2, phoneText.Frame.Y + phoneText.Frame.Height + 20, BUTTON_WIDTH, BUTTON_HEIGHT);
            contentHeight = (float)(byPhoneButton.Frame.Height + byPhoneButton.Frame.Y + 10 + Helpers.BottomNavHeight());

            scrollContainer.Frame = (CGRect)View.Frame;
            scrollContainer.ContentSize = new CGSize(ViewContainerWidth, contentHeight + GetBottomPadding());
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
                return (float)((GSCBaseView)View).baseContainer.Bounds.Height;
            }
            else
            {
                return (float)base.View.Bounds.Height;
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
            submissionTypeLabel.Text = "selectFromTheListOrLookup".tr();
        }

    }

    [Foundation.Register("ClaimSubmissionOneTitleThreeSubtitlesTableCell")]
    public class ClaimSubmissionOneTitleThreeSubtitlesTableCell : SingleSelectionAccentedTitleThreeSubtitlesViewCell
    {
        public ClaimSubmissionOneTitleThreeSubtitlesTableCell() : base() { }
        public ClaimSubmissionOneTitleThreeSubtitlesTableCell(IntPtr handle) : base(handle) { }

        public override void InitializeBindings()
        {
            this.DelayBind(() =>
                {
                    var set = this.CreateBindingSet<ClaimSubmissionOneTitleThreeSubtitlesTableCell, ServiceProvider>();
                    set.Bind(this.title).To(item => item.DoctorName).WithConversion("StringCase").OneWay();
                    set.Bind(this.label).To(item => item.Address).WithConversion("StringCase").OneWay();
                    set.Bind(this.label2).To(item => item.FormattedCityProv).WithConversion("StringCase").OneWay();
                    set.Bind(this.label3).To(item => item.Phone).WithConversion("StringCase,PhonePrefix").OneWay();
                    set.Apply();
                });
        }
    }
}