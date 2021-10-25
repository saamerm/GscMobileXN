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
    [Register("ClaimsHistoryResultsCountView")]
    public class ClaimsHistoryResultsCountView : GSCBaseViewController
    {
        private UIScrollView rootScrollView;
        private GSButtonGray searchCriteriaButt;
        private ClaimForLabel claimsForLab;
        private UILabel claimTypeLab;
        private ClaimResultCountTxt claimTypeTxt;
        private ClaimForLabel participantLab;
        private ClaimResultCountTxt claimForTxt;
        private ClaimForLabel lineOfBusinessLab;
        private ClaimResultCountTxt lineOfBusinessTxt;
        private ClaimForLabel periodLab;
        private ClaimResultCountTxt claimForDateTxt;
        private ClaimsHistoryResultsCountViewModel _model;
        private ClaimHistoryCountTableViewSource<ClaimHistoryCountTableViewCell> itemsSource;
        private UITableView searchResultsSummaryTable;
        private ClaimForLabel dateInquiryLab;
     
        private MvxSubscriptionToken _claimHistoryResultListView, _claimHistoryResultDetailView;
        private IMvxMessenger _messenger;
        private float topMargin = 10f + Constants.NAV_HEIGHT;
        private float leftMargin = 20f;
        private float rightMargin = 10f;
        private float belowMargin = 10f;
        private float bottomMargin = Helpers.BottomNavHeight() + 10f;
        private float searchButtonHeight = 44f;
        private float tableCellBorderWidth = 5f;
        private float claimForLabelWidth = 0.4f;

        public ClaimsHistoryResultsCountView()
        {
        }

        public override void ViewDidLoad()
        {
            View = new GSCFluentLayoutBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            base.ViewDidLoad();
            _model = this.ViewModel as ClaimsHistoryResultsCountViewModel;
            _model.PropertyChanged += Model_PropertyChanged;

            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = _model.ClaimsHistory.ToUpper();

            UIBarButtonItem barButton = new UIBarButtonItem();
            barButton.Title = " ".tr();
            this.NavigationController.NavigationBar.TopItem.BackBarButtonItem = barButton;

            rootScrollView = ((GSCFluentLayoutBaseView)View).baseScrollContainer;
            rootScrollView.BackgroundColor = Colors.BACKGROUND_COLOR;
            rootScrollView.ScrollEnabled = true;
            View.Add(rootScrollView);

            searchCriteriaButt = new GSButtonGray();
            searchCriteriaButt.SetTitle(_model.SearchCriteria.ToUpper(), UIControlState.Normal);
            rootScrollView.AddSubview(searchCriteriaButt);

            claimsForLab = new ClaimForLabel();
            claimsForLab.Text = _model.ClaimsFor.ToUpper();
            rootScrollView.AddSubview(claimsForLab);

            claimTypeLab = new ClaimForLabel();
            claimTypeLab.BackgroundColor = Colors.Clear;
            rootScrollView.AddSubview(claimTypeLab);

            claimTypeTxt = new ClaimResultCountTxt();
            rootScrollView.AddSubview(claimTypeTxt);

            participantLab = new ClaimForLabel();
            rootScrollView.AddSubview(participantLab);

            claimForTxt = new ClaimResultCountTxt();
            rootScrollView.AddSubview(claimForTxt);

            lineOfBusinessLab = new ClaimForLabel();
            rootScrollView.AddSubview(lineOfBusinessLab);

            lineOfBusinessTxt = new ClaimResultCountTxt();
            rootScrollView.AddSubview(lineOfBusinessTxt);

            periodLab = new ClaimForLabel();
            rootScrollView.AddSubview(periodLab);

            claimForDateTxt = new ClaimResultCountTxt();
            rootScrollView.AddSubview(claimForDateTxt);

            searchResultsSummaryTable = new UITableView();
            searchResultsSummaryTable.ScrollEnabled = false;
            searchResultsSummaryTable.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            itemsSource = new ClaimHistoryCountTableViewSource<ClaimHistoryCountTableViewCell>(searchResultsSummaryTable);
            searchResultsSummaryTable.Source = itemsSource;

            rootScrollView.AddSubview(searchResultsSummaryTable);

            dateInquiryLab = new ClaimForLabel();
            dateInquiryLab.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, (nfloat)11f);//Constants.SELECTION_ITEM_FONT_SIZE); 
            rootScrollView.AddSubview(dateInquiryLab);

            var set = this.CreateBindingSet<ClaimsHistoryResultsCountView, ClaimsHistoryResultsCountViewModel>();
            set.Bind(itemsSource).To(vm => vm.SearchResultsSummary);
            set.Bind(itemsSource).For(s => s.SelectionChangedCommand).To(vm => vm.SelectSearchResultTypeCommand);
            set.Bind(this).For(p => p.SearchResultsSummaryTableHeight).To(vm => vm.SearchResultsSummary).WithConversion("ClaimHistorySearchResultSummaryTableHeight");
            set.Bind(claimTypeTxt).To(vm => vm.ClaimType);
            set.Bind(claimForTxt).To(vm => vm.SelectedParticipant.FullName).WithConversion("FullNameToCaptialize").OneWay();
            set.Bind(lineOfBusinessTxt).To(vm => vm.LinesOfBusiness);
            set.Bind(claimForDateTxt).To(vm => vm.Period);
            set.Bind(searchCriteriaButt).To(vm => vm.SearchCriteriaCommand);
            set.Bind(claimTypeLab).To(vm => vm.ClaimTypeLabel).WithConversion("NonBreakingSpace");
            set.Bind(participantLab).To(vm => vm.ParticipantLabel).WithConversion("NonBreakingSpace");
            set.Bind(lineOfBusinessLab).To(vm => vm.LineOfBusinessLabel).WithConversion("NonBreakingSpace");
            set.Bind(periodLab).To(vm => vm.PeriodLabel).WithConversion("NonBreakingSpace");
            set.Bind(dateInquiryLab).To(vm => vm.DateOfInquiry).WithConversion("NonBreakingSpace");
            set.Apply();

            SetSelectedRowForSearchResultSummaryTable();

            SetConstraints();
        }

        private void SetConstraints()
        {
            string currentCulture = System.Globalization.CultureInfo.CurrentUICulture.Name.ToString();
            View.RemoveConstraints(View.Constraints);
            topMargin = (Constants.IsPhone()) ? (Helpers.IsInPortraitMode() ? 20f + Constants.NAV_HEIGHT : Constants.NAV_HEIGHT - 15f) : (20f + Constants.NAV_HEIGHT);
            View.RemoveConstraints(View.Constraints);
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            rootScrollView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                View.AddConstraints(

                    rootScrollView.AtTopOf(View, View.SafeAreaInsets.Top + 10),
                    rootScrollView.AtLeftOf(View, View.SafeAreaInsets.Left),
                    rootScrollView.AtRightOf(View, View.SafeAreaInsets.Right),
                    rootScrollView.WithSameHeight(View).Minus(topMargin + bottomMargin + belowMargin * 2)
                );

                View.AddConstraints(

                    searchCriteriaButt.AtTopOf(rootScrollView),
                    searchCriteriaButt.AtLeftOf(rootScrollView, leftMargin),
                    searchCriteriaButt.Height().EqualTo(searchButtonHeight),
                    searchCriteriaButt.WithSameWidth(rootScrollView).Minus(leftMargin * 2),

                    claimsForLab.WithSameLeft(searchCriteriaButt),
                    claimsForLab.Below(searchCriteriaButt, belowMargin),

                    claimTypeLab.WithSameLeft(claimsForLab),
                    claimTypeLab.WithRelativeWidth(rootScrollView, claimForLabelWidth),
                    claimTypeLab.Below(claimsForLab, belowMargin),

                    claimTypeTxt.ToRightOf(claimTypeLab, rightMargin),
                    claimTypeTxt.WithSameTop(claimTypeLab),
                    claimTypeTxt.WithRelativeWidth(rootScrollView, (1 - claimForLabelWidth)).Minus(rightMargin * 4),

                    participantLab.WithSameLeft(claimTypeLab),
                    participantLab.WithSameWidth(claimTypeLab)
                );
            }
            else
            {
                View.AddConstraints(
                rootScrollView.AtTopOf(View, topMargin),
                rootScrollView.AtLeftOf(View),
                rootScrollView.WithSameWidth(View),
                rootScrollView.WithSameHeight(View).Minus(topMargin + bottomMargin + belowMargin * 2),

                searchCriteriaButt.AtTopOf(rootScrollView),
                searchCriteriaButt.AtLeftOf(rootScrollView, leftMargin),
                searchCriteriaButt.Height().EqualTo(searchButtonHeight),
                searchCriteriaButt.WithSameWidth(rootScrollView).Minus(leftMargin * 2),

                claimsForLab.WithSameLeft(searchCriteriaButt),
                claimsForLab.Below(searchCriteriaButt, belowMargin),

                claimTypeLab.WithSameLeft(claimsForLab),
                claimTypeLab.WithRelativeWidth(rootScrollView, claimForLabelWidth),
                claimTypeLab.Below(claimsForLab, belowMargin),

                claimTypeTxt.ToRightOf(claimTypeLab, rightMargin),
                claimTypeTxt.WithSameTop(claimTypeLab),
                claimTypeTxt.WithRelativeWidth(rootScrollView, (1 - claimForLabelWidth)).Minus(rightMargin * 4),

                participantLab.WithSameLeft(claimTypeLab),
                participantLab.WithSameWidth(claimTypeLab)
            );
            }
            #region layout participant line base on lines of claim type value 
            if (_model != null && !string.IsNullOrEmpty(_model.ClaimType))
            {
                if (currentCulture.Contains("fr") || currentCulture.Contains("Fr"))
                {
                    if (Constants.IsPhone())
                    {
                        if (Helpers.IsInPortraitMode())
                        {
                            if (!string.IsNullOrEmpty(_model.ClaimType))
                            {
                                if (_model.ClaimType.Length <= Constants.CLAIMHISTORY_LINEOFBUNESSLIMATE)
                                {
                                    View.AddConstraints(participantLab.Below(claimTypeLab, belowMargin));
                                }
                                else
                                {
                                    View.AddConstraints(participantLab.Below(claimTypeTxt, belowMargin));
                                }
                            }
                            else
                            {
                                View.AddConstraints(participantLab.Below(claimTypeLab, belowMargin));
                            }
                        }
                        else
                        {
                            View.AddConstraints(participantLab.Below(claimTypeTxt, belowMargin));
                        }
                    }
                    else
                    {
                        if (_model.ClaimType.Length <= Constants.CLAIMHISTORY_LINEOFBUNESSLIMATE)
                        {
                            View.AddConstraints(participantLab.Below(claimTypeLab, belowMargin));
                        }
                        else
                        {
                            View.AddConstraints(participantLab.Below(claimTypeTxt, belowMargin));
                        }
                    }
                }
                else
                {
                    View.AddConstraints(participantLab.Below(claimTypeTxt, belowMargin));
                }
            }
            else
            {
                View.AddConstraints(participantLab.Below(claimTypeLab, belowMargin));
            }
            #endregion

            View.AddConstraints(
                claimForTxt.WithSameTop(participantLab),
                claimForTxt.WithSameLeft(claimTypeTxt),
                claimForTxt.WithSameWidth(claimTypeTxt),

                lineOfBusinessLab.WithSameLeft(participantLab),
                lineOfBusinessLab.WithSameWidth(participantLab)
            );
            #region layout line of business line base on lines of participant value 
            if (_model != null && !string.IsNullOrEmpty(_model.SelectedParticipant.FullName))
            {
                if (currentCulture.Contains("fr") || currentCulture.Contains("Fr"))
                {
                    if (Constants.IsPhone())
                    {
                        if (Helpers.IsInPortraitMode())
                        {
                            if (_model.SelectedParticipant.FullName.Length <= Constants.CLAIMHISTORY_PARTICIPANTLIMATE)
                            {
                                View.AddConstraints(lineOfBusinessLab.Below(participantLab, belowMargin));
                            }
                            else
                            {
                                View.AddConstraints(lineOfBusinessLab.Below(claimForTxt, belowMargin));
                            }
                        }
                        else
                        {
                            View.AddConstraints(lineOfBusinessLab.Below(claimForTxt, belowMargin));
                        }
                    }
                    else
                    {
                        if (_model.SelectedParticipant.FullName.Length <= Constants.CLAIMHISTORY_PARTICIPANTLIMATE)
                        {
                            View.AddConstraints(lineOfBusinessLab.Below(participantLab, belowMargin));
                        }
                        else
                        {
                            View.AddConstraints(lineOfBusinessLab.Below(claimForTxt, belowMargin));
                        }
                    }
                }
                else
                {
                    View.AddConstraints(lineOfBusinessLab.Below(claimForTxt, belowMargin));
                }
            }
            else
            {
                View.AddConstraints(lineOfBusinessLab.Below(participantLab, belowMargin));
            }
            #endregion

            View.AddConstraints(
                lineOfBusinessTxt.WithSameLeft(claimForTxt),
                lineOfBusinessTxt.WithSameWidth(claimForTxt),
                lineOfBusinessTxt.WithSameTop(lineOfBusinessLab),

                periodLab.WithSameLeft(lineOfBusinessLab),
                periodLab.WithSameWidth(lineOfBusinessLab)
            );
            #region layout period line base on lines of line of business value
            if (_model != null && !string.IsNullOrEmpty(_model.LinesOfBusiness))
            {
                if (currentCulture.Contains("fr") || currentCulture.Contains("Fr"))
                {
                    if (Constants.IsPhone())
                    {
                        if (Helpers.IsInPortraitMode())
                        {
                            if (_model.LinesOfBusiness.Length <= Constants.CLAIMHISTORY_LINEOFBUNESSLIMATE)
                            {
                                View.AddConstraints(periodLab.Below(lineOfBusinessLab, belowMargin));
                            }
                            else
                            {
                                View.AddConstraints(periodLab.Below(lineOfBusinessTxt, belowMargin));
                            }
                        }
                        else
                        {
                            View.AddConstraints(periodLab.Below(lineOfBusinessTxt, belowMargin));
                        }
                    }
                    else
                    {
                        if (_model.LinesOfBusiness.Length <= Constants.CLAIMHISTORY_LINEOFBUNESSLIMATE)
                        {
                            View.AddConstraints(periodLab.Below(lineOfBusinessLab, belowMargin));
                        }
                        else
                        {
                            View.AddConstraints(periodLab.Below(lineOfBusinessTxt, belowMargin));
                        }
                    }
                }
                else
                {
                    View.AddConstraints(periodLab.Below(lineOfBusinessTxt, belowMargin));
                }
            }
            else
            {
                View.AddConstraints(periodLab.Below(lineOfBusinessLab, belowMargin));
            }
            #endregion
            View.AddConstraints(
                claimForDateTxt.WithSameTop(periodLab),
                claimForDateTxt.WithSameLeft(lineOfBusinessTxt),
                claimForDateTxt.WithSameWidth(lineOfBusinessTxt),

                searchResultsSummaryTable.WithSameLeft(searchCriteriaButt).Minus(tableCellBorderWidth),
                searchResultsSummaryTable.WithSameWidth(searchCriteriaButt).Plus(tableCellBorderWidth * 2),
                searchResultsSummaryTable.Height().EqualTo(SearchResultsSummaryTableHeight),
                searchResultsSummaryTable.Below(claimForDateTxt, belowMargin),

                dateInquiryLab.WithSameLeft(periodLab),
                dateInquiryLab.WithSameWidth(searchResultsSummaryTable),
                dateInquiryLab.Below(searchResultsSummaryTable, belowMargin),
                dateInquiryLab.AtBottomOf(rootScrollView, belowMargin)
            );
        }

        public override void ViewWillAppear(bool animated)
        {
            base.NavigationItem.Title = _model.ClaimsHistory.ToUpper();
            base.ViewWillAppear(animated);
         
            var root = UIApplication.SharedApplication.KeyWindow.RootViewController as IContentPresenter;
            root?.ShowNavigation();

            claimForTxt.Text = _model.SelectedParticipant.FullName;
            SetConstraints();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetConstraints();
        }

        /// <summary>
        /// SearchResultsSummaryTableHeight must have a height and the height value base on collection count
        /// </summary>
        private float _searchResultsSummaryTableHeight;
        public float SearchResultsSummaryTableHeight
        {
            get
            {
                return _searchResultsSummaryTableHeight;
            }
            set
            {
                _searchResultsSummaryTableHeight = value;
                SetConstraints();
            }
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SearchResultsSummary")
            {
                var h = SearchResultsSummaryTableHeight;
                SetSelectedRowForSearchResultSummaryTable();
                SetConstraints();
            }

            if (e.PropertyName == "SelectedParticipant")
            {
                claimForTxt.Text = _model.SelectedParticipant.FullName;
                SetConstraints();
            }

            if (e.PropertyName == "LinesOfBusiness")
            {
                SetConstraints();
            }

            if (e.PropertyName == "ClaimType")
            {
                claimTypeTxt.Text = _model.ClaimType;
                SetConstraints();
            }
        }

        private void SetSelectedRowForSearchResultSummaryTable()
        {
            if (!_model.IsSearchResultsSummarySelected)
                itemsSource.SelectedRow = null;
            searchResultsSummaryTable.ReloadData();

            if (_model.SearchResultsSummary != null && _model.SearchResultsSummary.Count > 0 && _model.IsSearchResultsSummarySelected)
            {
                IClaimsHistoryService claimshistoryservice = Mvx.IoCProvider.Resolve<IClaimsHistoryService>();
                int selectedRow = 0;
                if (claimshistoryservice.SelectedSearchResultType != null)
                {
                    for (int i = 0; i < _model.SearchResultsSummary.Count; i++)
                    {
                        ClaimHistorySearchResultSummary p = _model.SearchResultsSummary[i] as ClaimHistorySearchResultSummary;
                        if (p.BenefitID == claimshistoryservice.SelectedSearchResultType.BenefitID)
                        {
                            selectedRow = i;
                            itemsSource.SelectedRow = NSIndexPath.FromRowSection((nint)selectedRow, (nint)0);
                        }
                    }
                }
            }
        }
    }
}