using System;
using System.Collections.ObjectModel;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Entities.HCSA;
using MobileClaims.Core.ViewModels.HCSA;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS
{
    public class ClaimReviewAndEditView : GSCBaseViewController
    {
        public UITableView claimsTableView;
        public UILabel lblClaimsFor;
        public UILabel lblNameOfClaimer;
        public UILabel lblSwipeLeft;
        protected GSButton btnContinue;
        public UIScrollView scrollableContainer;
        public CGRect tableFrame;
        UIBarButtonItem addClaimButton;
        public ClaimReviewAndEditViewModel _model;
        private int countOfDetails;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _model = (ClaimReviewAndEditViewModel)this.ViewModel;
            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = _model.TitleLabel;

            View = new UIView()
            {
                BackgroundColor = Colors.BACKGROUND_COLOR
            };

            this.AutomaticallyAdjustsScrollViewInsets = false;

            scrollableContainer = new UIScrollView();
            scrollableContainer.ScrollEnabled = true;
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                scrollableContainer.ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Never;
            }

            UIButton btnAdd = new UIButton(UIButtonType.Custom);
            btnAdd.TouchUpInside += HandleAddEntry;
            if (Constants.IsPhone())
            {
                btnAdd.Frame = new CGRect(0, 0, 80, 30);
            }
            else
            {
                btnAdd.Frame = new CGRect(0, 0, 90, 35);
            }

            btnAdd.SetTitle("addEntry".tr(), UIControlState.Normal);
            btnAdd.TintColor = Colors.HIGHLIGHT_COLOR;
            btnAdd.Font = UIFont.FromName(Constants.NUNITO_REGULAR, (nfloat)Constants.NAV_BAR_BUTTON_SIZE);
            btnAdd.BackgroundColor = Colors.HIGHLIGHT_COLOR;
            btnAdd.ContentEdgeInsets = new UIEdgeInsets(0, 5, 0, 5);
            addClaimButton = new UIBarButtonItem(btnAdd);
            base.NavigationItem.RightBarButtonItem = addClaimButton;

            View.AddSubview(scrollableContainer);

            lblClaimsFor = new UILabel();
            lblClaimsFor.BackgroundColor = Colors.Clear;
            lblClaimsFor.TextAlignment = UITextAlignment.Left;
            lblClaimsFor.Lines = 0;
            lblClaimsFor.LineBreakMode = UILineBreakMode.WordWrap;

            string text = "claimsFor".tr();
            string name = " ";
            if (_model.Participant != null && _model.Participant.FullName != null)
                name = name + _model.Participant.FullName;
            text = text + name;
            var textAttributed = new NSMutableAttributedString(text, new UIStringAttributes() { ForegroundColor = Colors.DARK_GREY_COLOR, Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, 24) });
            var colourAttribute = new UIStringAttributes()
            {
                ForegroundColor = Colors.Black,
                Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, 24)
            };
            textAttributed.SetAttributes(colourAttribute, new NSRange(text.Length - name.Length, name.Length));
            lblClaimsFor.AttributedText = textAttributed;

            lblSwipeLeft = new UILabel();
            lblSwipeLeft.Text = "swipeNotification".tr();
            lblSwipeLeft.BackgroundColor = Colors.Clear;
            lblSwipeLeft.TextColor = Colors.Black;
            lblSwipeLeft.TextAlignment = UITextAlignment.Left;
            lblSwipeLeft.Lines = 0;
            lblSwipeLeft.LineBreakMode = UILineBreakMode.WordWrap;
            lblSwipeLeft.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 12);

            claimsTableView = new UITableView();
            claimsTableView.SeparatorColor = Colors.DARK_GREY_COLOR;

            UIView vewFooter = new UIView();
            claimsTableView.TableFooterView = vewFooter;


            btnContinue = new GSButton();
            btnContinue.SetTitle("continueButton".tr(), UIControlState.Normal);
            btnContinue.TitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.GREEN_BUTTON_FONT_SIZE);

            var source = new ClaimsListTableViewSource(_model, claimsTableView); ///

            claimsTableView.Source = source;

            scrollableContainer.AddSubview(lblClaimsFor);
            scrollableContainer.AddSubview(lblSwipeLeft);
            scrollableContainer.AddSubview(claimsTableView);
            scrollableContainer.AddSubview(btnContinue);


            var set = this.CreateBindingSet<ClaimReviewAndEditView, ClaimReviewAndEditViewModel>();

            set.Bind(source).To(vm => vm.Claim.Details);
            set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.EditCommand);

            set.Bind(this.btnContinue).To(vm => vm.ConfirmClaimSummaryCommand);
            set.Bind(this).For(v => v.Details).To(vm => vm.Claim.Details);
            set.Apply();

            _model.OnInvalidClaim += (object sender, EventArgs e) =>
            {
                InvokeOnMainThread(() =>
                {
                    UIAlertView _error = new UIAlertView("", "noClaimDetailEntered".tr(), null, "ok".tr(), null);
                    _error.Show();

                });
            };


        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            tableFrame = claimsTableView.Frame;
            tableFrame.Height = (nfloat)(120 * _model.Claim.Details.Count);
            claimsTableView.Frame = tableFrame;
            claimsTableView.BackgroundColor = Colors.BACKGROUND_COLOR;
            SetConstraints();
        }

        private ObservableCollection<ClaimDetail> _details;
        public ObservableCollection<ClaimDetail> Details
        {
            get { return _details; }
            set
            {
                _details = value;

                if (_model != null && countOfDetails != _model.Claim.Details.Count)
                {
                    SetConstraints();
                }
            }
        }

        private void HandleAddEntry(object sender, EventArgs e)
        {
            if (_model.Claim.Details.Count >= 5)
            {
                InvokeOnMainThread(() =>
                {
                    UIAlertView _error = new UIAlertView("", "maximumFiveClaims".tr(), null, "ok".tr(), null);
                    _error.Show();
                });

            }
            else
            {
                _model.AddCommand.Execute(null);
            }
        }

        private void SetConstraints()
        {
            tableFrame = claimsTableView.Frame;
            tableFrame.Height = (nfloat)(120 * _model.Claim.Details.Count);

            if (Constants.IsPhone())
            {
                tableFrame.Height = 0;
                if (Helpers.IsInPortraitMode())
                {
                    if (_model.Claim.Details != null)
                    {
                        foreach (ClaimDetail cd in _model.Claim.Details)
                        {
                            if (!string.IsNullOrEmpty(cd.ExpenseTypeDescription))
                            {
                                if (cd.ExpenseTypeDescription.Length > 25)
                                {
                                    tableFrame.Height += 140;
                                }
                                else
                                {
                                    tableFrame.Height += 120;
                                }
                            }
                            else
                            {
                                tableFrame.Height += 120;
                            }
                        }
                    }
                }
                else
                {
                    tableFrame.Height = (nfloat)(120 * _model.Claim.Details.Count);
                }
            }
            else
            {

                if (Helpers.IsInPortraitMode())
                {
                    tableFrame.Height = 0;
                    if (_model.Claim.Details != null)
                    {
                        foreach (ClaimDetail cd in _model.Claim.Details)
                        {
                            if (!string.IsNullOrEmpty(cd.ExpenseTypeDescription))
                            {
                                if (cd.ExpenseTypeDescription.Length > 35)
                                {
                                    tableFrame.Height += 160;
                                }
                                else
                                {
                                    tableFrame.Height += 120;
                                }
                            }
                            else
                            {
                                tableFrame.Height += 120;
                            }
                        }
                    }
                }

            }
            claimsTableView.Frame = tableFrame;
            float Side_Spacing = 20;



            View.RemoveConstraints(View.Constraints);

            scrollableContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            btnContinue.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            if (Constants.IS_OS_VERSION_OR_LATER(11, 0) && (UIScreen.MainScreen.NativeBounds.Height == 2436))
            {
                View.AddConstraints(

                    scrollableContainer.AtTopOf(View, View.SafeAreaInsets.Top),
                    scrollableContainer.AtLeftOf(View, View.SafeAreaInsets.Left),
                    scrollableContainer.AtRightOf(View, View.SafeAreaInsets.Right),
                    scrollableContainer.AtBottomOf(View, View.SafeAreaInsets.Bottom)

                );

                View.AddConstraints(

                lblClaimsFor.AtTopOf(scrollableContainer, 10),
                lblClaimsFor.AtLeftOf(scrollableContainer, Side_Spacing),
                lblClaimsFor.AtRightOf(scrollableContainer, Side_Spacing),

                lblSwipeLeft.AtLeftOf(scrollableContainer, Side_Spacing),
                lblSwipeLeft.Below(lblClaimsFor, 2),
                lblSwipeLeft.AtRightOf(scrollableContainer, Constants.BUTTON_SIDE_PADDING),

                claimsTableView.Below(lblSwipeLeft, Side_Spacing),
                claimsTableView.WithSameWidth(scrollableContainer).Minus(40),
                claimsTableView.WithSameCenterX(scrollableContainer),
                claimsTableView.Height().EqualTo(tableFrame.Height),

                btnContinue.Below(claimsTableView, 30),
                btnContinue.WithSameCenterX(scrollableContainer),
                btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                btnContinue.AtBottomOf(scrollableContainer, 80)

            );

            }
            else
            {
                View.AddConstraints(

               scrollableContainer.AtTopOf(View, 10),
               scrollableContainer.AtLeftOf(View),
               scrollableContainer.AtRightOf(View, 0),
               scrollableContainer.AtBottomOf(View),

               lblClaimsFor.AtTopOf(scrollableContainer, Constants.TOP_PADDING),
               lblClaimsFor.AtLeftOf(scrollableContainer, Side_Spacing),
               lblClaimsFor.AtRightOf(View, Side_Spacing),

               lblSwipeLeft.AtLeftOf(scrollableContainer, Side_Spacing),
               lblSwipeLeft.Below(lblClaimsFor, 2),
               lblSwipeLeft.AtRightOf(View, Constants.BUTTON_SIDE_PADDING),

               claimsTableView.Below(lblSwipeLeft, Side_Spacing),
               claimsTableView.AtLeftOf(scrollableContainer, Side_Spacing),
               claimsTableView.AtRightOf(View, Constants.BUTTON_SIDE_PADDING),
               claimsTableView.Height().EqualTo(tableFrame.Height),

               btnContinue.Below(claimsTableView, 30),
               btnContinue.WithSameCenterX(scrollableContainer),
               btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
               btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
               btnContinue.AtBottomOf(scrollableContainer, 80)

           );

            }

        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetConstraints();
        }


        #region TableViewSource
        public class ClaimsListTableViewSource : MvxTableViewSource
        {
            public ClaimReviewAndEditViewModel model;
            public ClaimsListTableViewSource(ClaimReviewAndEditViewModel _model, UITableView tableView)
                : base(tableView)
            {
                model = _model;

                tableView.RegisterClassForCellReuse(typeof(ClaimListTableViewCell), new NSString("ClaimListTableViewCell"));

            }

            protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
            {
                UITableViewCell cell = (UITableViewCell)tableView.DequeueReusableCell("ClaimListTableViewCell");
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                return cell;
            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                tableView.SeparatorColor = Colors.Clear;
                float result = 120f;
                if (Constants.IsPhone())
                {
                    if (Helpers.IsInPortraitMode())
                    {
                        if (model.Claim.Details[indexPath.Row].ExpenseTypeDescription.Length > 25)
                        {
                            result = 140;
                        }
                    }
                }
                else
                {
                    if (Helpers.IsInPortraitMode())
                    {
                        if (model.Claim.Details[indexPath.Row].ExpenseTypeDescription.Length > 35)
                        {
                            result = 160;
                        }
                    }
                }
                if (Constants.IsPhone())
                {
                    if (tableView.VisibleCells != null && tableView.VisibleCells.Length > 0 && tableView.VisibleCells.Length > indexPath.Row)
                    {

                        ClaimListTableViewCell c = (ClaimListTableViewCell)tableView.VisibleCells[indexPath.Row];
                        if (c.previousMask == UITableViewCellState.ShowingDeleteConfirmationMask)
                        {
                            c.DidTransitionToState(c.previousMask);
                        }
                    }
                }

                return result;
            }
            public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
            {

                return true;
            }

            public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
            {
                tableView.Editing = false;
                var cell = this.GetCell(tableView, indexPath);
                var showDeleteButton = true;

                if (cell.GetType().GetMethod("ShowsDeleteButton") != null)
                {
                    showDeleteButton = ((ClaimListTableViewCell)cell).ShowsDeleteButton();
                }

                if (showDeleteButton)
                {
                    switch (editingStyle)
                    {
                        case UITableViewCellEditingStyle.Delete:
                            model.RemoveWithSwipeCommand.Execute(indexPath.Row);
                            break;

                        case UITableViewCellEditingStyle.None:
                            break;
                    }
                }
            }


            public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = this.GetCell(tableView, indexPath);
                var showDeleteButton = true;

                if (cell.GetType().GetMethod("ShowsDeleteButton") != null)
                {
                    showDeleteButton = ((ClaimListTableViewCell)cell).ShowsDeleteButton();
                }

                if (showDeleteButton)
                {
                    return UITableViewCellEditingStyle.Delete;
                }
                else
                {
                    return UITableViewCellEditingStyle.None;
                }
            }


            public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
            {
                return false;
            }
        }
        #endregion
    }
}
