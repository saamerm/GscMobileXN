using System;
using System.ComponentModel;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MobileClaims.Core.Entities.ClaimsHistory;
using MobileClaims.Core.Services.ClaimsHistory;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using MobileClaims.iOS.UI;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Plugin.Messenger;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimsHistory
{
    [Register("ClaimsHistoryResultsListView")]
    public class ClaimsHistoryResultsListView : GSCBaseViewController
    {
        private UIScrollView rootScrollView;
        private GSButtonGray searchCriteriaButt;
        private ClaimForLabel numberOfClaimsLab;
        private ClaimForLabel searchedForLabelLab;
        private ClaimForLabel claimTypeLab;
        private ClaimResultCountTxt claimTypeTxt;
        private ClaimForLabel participantLab;
        private ClaimResultCountTxt claimForTxt;
        private ClaimForLabel lineOfBusinessLab;
        private ClaimResultCountTxt lineOfBusinessTxt;
        private ClaimForLabel periodLab;
        private ClaimResultCountTxt claimForDateTxt;
        private ClaimsHistoryResultsListViewModel model;
        private ClaimHistoryResultListTableViewSource<ClaimHistoryResultListTableViewCell> itemSouce;
        private UITableView searchResultsTable;
        private UIView totalBorder;
        private UILabel totalLab;
        private ClaimForLabel totalClaimedAmountLab;
        private ClaimForTxtLabel totalClaimedAmountTxt;
        private ClaimForLabel totalOtherPaidAmountLab;
        private ClaimForTxtLabel totalOtherPaidAmountTxt;
        private ClaimForLabel totalPaidAmountLab;
        private ClaimForTxtLabel totalPaidAmountTxt;
        private ClaimForLabel totalCopyAmountLab;
        private ClaimForTxtLabel totalCopyAmountTxt;
        private ClaimForLabel dateInquiryLab;
        private GSCMessageLabel errorMessageLab;
        private IMvxMessenger _messenger;

        private float topMargin = 10f + Constants.NAV_HEIGHT;
        private float leftMargin = 20f;
        private float bottomMargin = Helpers.BottomNavHeight() + 10f;
        private float belowMargin = 10f;
        private float totalBorderWidth = 2f;
        private float totalBorderHeight = 150f;
        private float searchResultsTableCellBorderWidth = 5f;
        // Changed to 0 so as to compensate the hidden search button
        private float searchButtonHeight = 0f;

        public ClaimsHistoryResultsListView()
        {
        }

        /// <summary>
        /// searchResultTable must have a height and the height value base on collection count
        /// </summary>
        private float _searchResultsTableHeight;
        public float SearchResultsTableHeight
        {
            get
            {
                return _searchResultsTableHeight;
            }
            set
            {
                _searchResultsTableHeight = value;
            }
        }

        public override void ViewDidLoad()
        {
            View = new GSCFluentLayoutBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            base.ViewDidLoad();
            model = this.ViewModel as ClaimsHistoryResultsListViewModel;
            model.CloseClaimsHistoryResultsListVM += Model_CloseClaimsHistoryResultsListVM;
            model.PropertyChanged += Model_PropertyChanged;

            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.SetHidesBackButton(false, false);
            base.NavigationItem.Title = model.SearchResultType.BenefitName.ToUpper();

            rootScrollView = ((GSCFluentLayoutBaseView)View).baseScrollContainer;
            rootScrollView.BackgroundColor = Colors.BACKGROUND_COLOR;
            rootScrollView.ScrollEnabled = true;
            View.Add(rootScrollView);

            // This is to be removed because of the flow causing a crash.
            // However, due to the deep tie in, that would take a lot of time, so we hide it.
            searchCriteriaButt = new GSButtonGray();
            searchCriteriaButt.SetTitle(model.SearchCriteria.ToUpper(), UIControlState.Normal);
            searchCriteriaButt.Hidden = true;
            rootScrollView.AddSubview(searchCriteriaButt);

            searchResultsTable = new UITableView();
            searchResultsTable.ScrollEnabled = false;
            searchResultsTable.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            itemSouce = new ClaimHistoryResultListTableViewSource<ClaimHistoryResultListTableViewCell>(searchResultsTable);
            searchResultsTable.Source = itemSouce;
            rootScrollView.AddSubview(searchResultsTable);
            searchResultsTable.BackgroundColor = Colors.BACKGROUND_COLOR;

            totalBorder = new UIView();
            totalBorder.Layer.BorderWidth = totalBorderWidth;
            totalBorder.Layer.BorderColor = Colors.DARK_GREY_COLOR.CGColor;
            rootScrollView.AddSubview(totalBorder);

            totalLab = new UILabel();
            totalLab.Text = model.TotalsLabel;
            totalLab.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.LG_HEADING_FONT_SIZE);
            totalLab.TextColor = Colors.DARK_GREY_COLOR;
            totalBorder.AddSubview(totalLab);

            totalClaimedAmountLab = new ClaimForLabel();
            totalClaimedAmountLab.Text = model.ClaimedAmountLabel;
            totalBorder.AddSubview(totalClaimedAmountLab);

            totalClaimedAmountTxt = new ClaimForTxtLabel();
            totalBorder.AddSubview(totalClaimedAmountTxt);

            totalOtherPaidAmountLab = new ClaimForLabel();
            totalOtherPaidAmountLab.Text = model.OtherPaidAmountLabel;
            totalBorder.AddSubview(totalOtherPaidAmountLab);

            totalOtherPaidAmountTxt = new ClaimForTxtLabel();
            totalBorder.AddSubview(totalOtherPaidAmountTxt);

            totalPaidAmountLab = new ClaimForLabel();
            totalPaidAmountLab.Text = model.PaidAmountLabel;
            totalBorder.AddSubview(totalPaidAmountLab);

            totalPaidAmountTxt = new ClaimForTxtLabel();
            totalBorder.AddSubview(totalPaidAmountTxt);

            totalCopyAmountLab = new ClaimForLabel();
            totalCopyAmountLab.Text = model.CopayDeductibleLabel;
            totalBorder.AddSubview(totalCopyAmountLab);

            totalCopyAmountTxt = new ClaimForTxtLabel();
            totalBorder.AddSubview(totalCopyAmountTxt);

            errorMessageLab = new GSCMessageLabel();
            if (model.ErrorCode != 0 && !string.IsNullOrWhiteSpace(model.ErrorMessage))
            {
                errorMessageLab.Text = $"{model.ErrorMessage} {model.ErrorCode}";
            }
            rootScrollView.AddSubview(errorMessageLab);

            dateInquiryLab = new ClaimForLabel();
            dateInquiryLab.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, (nfloat)11f);
            dateInquiryLab.Text = "Date of inquiry";
            rootScrollView.AddSubview(dateInquiryLab);
            if (!Constants.IsPhone())
                dateInquiryLab.Hidden = true;

            var set = this.CreateBindingSet<ClaimsHistoryResultsListView, ClaimsHistoryResultsListViewModel>();
            set.Bind(itemSouce).To(vm => vm.SearchResults);
            set.Bind(itemSouce).For(s => s.SelectionChangedCommand).To(vm => vm.SelectSearchResultCommand);
            set.Bind(this).For(p => p.SearchResultsTableHeight).To(vm => vm.SearchResults).WithConversion("ClaimHistorySearchResultsToTableHight");
            set.Bind(totalClaimedAmountTxt).To(vm => vm.TotalClaimedAmount).WithConversion("DollarSignDoublePrefix");
            set.Bind(totalOtherPaidAmountTxt).To(vm => vm.TotalOtherPaidAmount).WithConversion("DollarSignDoublePrefix");
            set.Bind(totalPaidAmountTxt).To(vm => vm.TotalPaidAmount).WithConversion("DollarSignDoublePrefix");
            set.Bind(totalCopyAmountTxt).To(vm => vm.TotalCopay).WithConversion("DollarSignDoublePrefix");
            set.Bind(searchCriteriaButt).To(vm => vm.SearchCriteriaCommand);

            set.Bind(claimTypeTxt).To(vm => vm.ClaimType);
            set.Bind(claimForTxt).To(vm => vm.SelectedParticipant.FullName).WithConversion("FullNameToCaptialize").OneWay();
            set.Bind(lineOfBusinessTxt).To(vm => vm.LinesOfBusiness);
            set.Bind(claimForDateTxt).To(vm => vm.Period);

            set.Bind(dateInquiryLab).To(VM => VM.DateOfInquiry);
            set.Apply();
            SetSelectedRowForSearchResultSummaryTable();
            SetConstraints();
        }

        private void SetConstraints()
        {
            topMargin = 20f + Constants.NAV_HEIGHT;

            View.RemoveConstraints(View.Constraints);
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            totalBorder.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            rootScrollView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            if (Constants.IsPhone())
            {
                string currentCulture = System.Globalization.CultureInfo.CurrentUICulture.Name;

                if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
                {
                    View.AddConstraints(
                        rootScrollView.AtTopOf(View, topMargin + 10),
                        rootScrollView.AtLeftOf(View, View.SafeAreaInsets.Left),
                        rootScrollView.AtRightOf(View, View.SafeAreaInsets.Right),
                        rootScrollView.AtBottomOf(View, Helpers.BottomNavHeight()));

                    View.AddConstraints(
                        // Updated the values to compensate for the hidden search button
                        searchCriteriaButt.AtTopOf(rootScrollView).Plus(0),
                        searchCriteriaButt.AtLeftOf(rootScrollView, leftMargin),
                        searchCriteriaButt.Height().EqualTo(0),
                        searchCriteriaButt.WithSameWidth(rootScrollView).Minus(leftMargin * 2));
                }
                else
                {
                    View.AddConstraints(
                    rootScrollView.AtTopOf(View, topMargin),
                    rootScrollView.AtLeftOf(View, leftMargin),
                    rootScrollView.WithSameWidth(View).Minus(leftMargin * 2),
                    rootScrollView.AtBottomOf(View, Constants.NAV_BUTTON_SIZE_IPHONE),

                    searchCriteriaButt.WithSameTop(rootScrollView),
                    searchCriteriaButt.AtLeftOf(rootScrollView),
                    searchCriteriaButt.Height().EqualTo(searchButtonHeight),
                    searchCriteriaButt.WithSameWidth(rootScrollView));
                }

                if (string.IsNullOrEmpty(model.ErrorMessage))
                {
                    View.AddConstraints(
                        searchResultsTable.WithSameLeft(searchCriteriaButt).Minus(searchResultsTableCellBorderWidth),
                        searchResultsTable.WithSameWidth(searchCriteriaButt).Plus(searchResultsTableCellBorderWidth * 2),
                        searchResultsTable.Height().EqualTo(SearchResultsTableHeight),
                        searchResultsTable.Below(searchCriteriaButt, belowMargin * 2),

                        totalBorder.WithSameLeft(searchCriteriaButt),
                        totalBorder.WithSameWidth(searchCriteriaButt),
                        totalBorder.Below(searchResultsTable, belowMargin),

                        totalLab.AtTopOf(totalBorder, belowMargin),
                        totalLab.AtLeftOf(totalBorder, leftMargin / 2),

                        totalClaimedAmountLab.WithSameLeft(totalLab),
                        totalClaimedAmountLab.Below(totalLab, belowMargin),

                        totalClaimedAmountTxt.WithSameCenterY(totalClaimedAmountLab),
                        totalClaimedAmountTxt.AtRightOf(totalBorder).Minus(leftMargin / 2),

                        totalOtherPaidAmountLab.WithSameLeft(totalClaimedAmountLab),
                        totalOtherPaidAmountLab.Below(totalClaimedAmountLab, belowMargin / 2),

                        totalOtherPaidAmountTxt.WithSameCenterY(totalOtherPaidAmountLab),
                        totalOtherPaidAmountTxt.AtRightOf(totalBorder).Minus(leftMargin / 2),

                        totalPaidAmountLab.WithSameLeft(totalOtherPaidAmountLab),
                        totalPaidAmountLab.Below(totalOtherPaidAmountLab, belowMargin / 2),

                        totalPaidAmountTxt.WithSameCenterY(totalPaidAmountLab),
                        totalPaidAmountTxt.AtRightOf(totalBorder).Minus(leftMargin / 2),

                        totalCopyAmountLab.WithSameLeft(totalPaidAmountLab),
                        totalCopyAmountLab.Below(totalPaidAmountLab, belowMargin / 2),

                        totalCopyAmountTxt.WithSameCenterY(totalCopyAmountLab),
                        totalCopyAmountTxt.AtRightOf(totalBorder).Minus(leftMargin / 2),
                        totalCopyAmountTxt.AtBottomOf(totalBorder, belowMargin),

                        dateInquiryLab.WithSameLeft(totalBorder),
                        dateInquiryLab.WithSameWidth(totalBorder),
                        dateInquiryLab.Below(totalBorder, belowMargin),
                        dateInquiryLab.AtBottomOf(rootScrollView, belowMargin)
                    );
                }
                else
                {
                    View.AddConstraints(
                        errorMessageLab.WithSameCenterX(rootScrollView),
                        errorMessageLab.Below(claimForDateTxt, belowMargin * 2),

                        dateInquiryLab.AtLeftOf(rootScrollView, leftMargin),
                        dateInquiryLab.WithSameWidth(errorMessageLab),
                        dateInquiryLab.Below(errorMessageLab, belowMargin)
                    );
                }
            }
            else
            {
                View.AddConstraints(
                    rootScrollView.AtTopOf(View, topMargin),
                    rootScrollView.AtLeftOf(View, leftMargin),
                    rootScrollView.WithSameWidth(View).Minus(leftMargin * 2),
                    rootScrollView.WithSameHeight(View).Minus(topMargin + bottomMargin + belowMargin * 2)
                );

                View.AddConstraints(
                     searchCriteriaButt.AtTopOf(rootScrollView),
                     searchCriteriaButt.AtLeftOf(View, leftMargin),
                     searchCriteriaButt.Height().EqualTo(searchButtonHeight),
                     searchCriteriaButt.WithSameWidth(View).Minus(leftMargin * 2));

                if (string.IsNullOrEmpty(model.ErrorMessage))
                {
                    View.AddConstraints(
                        searchResultsTable.Below(searchCriteriaButt, belowMargin * 2),
                        searchResultsTable.WithSameLeft(rootScrollView).Minus(searchResultsTableCellBorderWidth),
                        searchResultsTable.WithSameWidth(rootScrollView).Plus(searchResultsTableCellBorderWidth * 2),
                        searchResultsTable.Height().EqualTo(SearchResultsTableHeight),

                        totalBorder.WithSameLeft(searchCriteriaButt),
                        totalBorder.WithSameWidth(searchCriteriaButt),
                        totalBorder.Below(searchResultsTable, belowMargin),
                        totalBorder.Height().EqualTo(totalBorderHeight),
                        totalBorder.AtBottomOf(rootScrollView),

                        totalLab.AtTopOf(totalBorder, belowMargin),
                        totalLab.AtLeftOf(totalBorder, leftMargin / 2),

                        totalClaimedAmountLab.WithSameLeft(totalLab),
                        totalClaimedAmountLab.Below(totalLab, belowMargin),

                        totalClaimedAmountTxt.WithSameCenterY(totalClaimedAmountLab),
                        totalClaimedAmountTxt.AtRightOf(totalBorder).Minus(leftMargin / 2),

                        totalOtherPaidAmountLab.WithSameLeft(totalClaimedAmountLab),
                        totalOtherPaidAmountLab.Below(totalClaimedAmountLab, belowMargin / 2),

                        totalOtherPaidAmountTxt.WithSameCenterY(totalOtherPaidAmountLab),
                        totalOtherPaidAmountTxt.AtRightOf(totalBorder).Minus(leftMargin / 2),

                        totalPaidAmountLab.WithSameLeft(totalOtherPaidAmountLab),
                        totalPaidAmountLab.Below(totalOtherPaidAmountLab, belowMargin / 2),

                        totalPaidAmountTxt.WithSameCenterY(totalPaidAmountLab),
                        totalPaidAmountTxt.AtRightOf(totalBorder).Minus(leftMargin / 2),

                        totalCopyAmountLab.WithSameLeft(totalPaidAmountLab),
                        totalCopyAmountLab.Below(totalPaidAmountLab, belowMargin / 2),

                        totalCopyAmountTxt.WithSameCenterY(totalCopyAmountLab),
                        totalCopyAmountTxt.AtRightOf(totalBorder).Minus(leftMargin / 2)
                    );
                }
                else
                {
                    View.AddConstraints(
                        errorMessageLab.WithSameCenterX(rootScrollView),
                        errorMessageLab.Below(claimForDateTxt, belowMargin * 2)
                    );
                }
            }
        }

        private void Model_CloseClaimsHistoryResultsListVM(object sender, EventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                if (base.NavigationController != null)
                {
                    if (base.NavigationController.ViewControllers != null)
                        if (base.NavigationController.ViewControllers[base.NavigationController.ViewControllers.Length - 3].GetType() == typeof(ClaimsHistoryResultsCountView))
                        {
                            base.NavigationController.PopToViewController(base.NavigationController.ViewControllers[base.NavigationController.ViewControllers.Length - 3], true);
                        }
                        else
                        {
                            base.NavigationController.PopToViewController(base.NavigationController.ViewControllers[base.NavigationController.ViewControllers.Length - 2], true);
                        }

                }
            });
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedParticipant")
            {
                claimForTxt.Text = model.SelectedParticipant.FullName;
                SetConstraints();
            }
            if (e.PropertyName == "LinesOfBusiness")
            {
                SetConstraints();
            }

            if (e.PropertyName == "ClaimType")
            {
                SetConstraints();
            }
        }

        private void SetSelectedRowForSearchResultSummaryTable()
        {
            searchResultsTable.ReloadData();
            if (model.SearchResults != null && model.SearchResults.Count > 0)
            {
                IClaimsHistoryService claimshistoryservice = Mvx.IoCProvider.Resolve<IClaimsHistoryService>();
                int selectedRow = 0;
                if (claimshistoryservice.SelectedSearchResult != null)
                {
                    for (int i = 0; i < model.SearchResults.Count; i++)
                    {
                        ClaimState p = model.SearchResults[i] as ClaimState;
                        if (p.BenefitID == claimshistoryservice.SelectedSearchResult.BenefitID)
                        {
                            selectedRow = i;
                            itemSouce.SelectedRow = NSIndexPath.FromRowSection((nint)selectedRow, (nint)0);
                        }
                    }
                }
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.NavigationItem.Title = model.SearchResultType.BenefitName.ToUpper();
            base.ViewWillAppear(animated);

            var root = UIApplication.SharedApplication.KeyWindow.RootViewController as IContentPresenter;
            root?.ShowNavigation();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            base.NavigationItem.Title = "back".tr();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetConstraints();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            model.CloseClaimsHistoryResultsListVM -= Model_CloseClaimsHistoryResultsListVM;
            model.PropertyChanged -= Model_PropertyChanged;
        }
    }
}