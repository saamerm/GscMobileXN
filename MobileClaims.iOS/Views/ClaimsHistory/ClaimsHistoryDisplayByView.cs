using System;
using Foundation;
using UIKit;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using Cirrious.FluentLayouts.Touch;
using MobileClaims.Core;
using System.ComponentModel;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS.Views.ClaimsHistory
{
    [Register("ClaimsHistoryDisplayByView")]
    public class ClaimsHistoryDisplayByView : GSCBaseViewController
    {
        #region declare components
        private UIScrollView rootScrollView;
        private GreenNavButton yearToDateButt;
        private GreenNavButton rangeButt;
        private GreenNavButton yearButt;
        private int displayByCount;
        private GSButton24 doneButt;
        private UIView tabContentContainer;

        private UILabel yearToDateLab;

        private bool isPhone = Constants.IsPhone();
        private bool isSelectStartDate;
        private ClaimForLabel startDateLab;
        private ClaimForLabel endDateLab;
        private ErrorButton startDateErrorButt;
        private ErrorButton endDateErrorButt;
        private DatePickerLabel startDateTxt;
        private DatePickerLabel endDateTxt;
        private UIDatePicker startDatePickerPhone;
        private UIDatePicker endDatePickerPhone;
        private UIView datePickerContainer;
        private UIButton datePickerCloseButt;

        private ClaimTreatmentDetailsTitleAndDatePicker startDatePicker;
        private ClaimTreatmentDetailsTitleAndDatePicker endDatePicker;

        private YearPickerView yearPicker;
        private YearPickerSource pickerViewModel;
        private ErrorButton yearErrorButt;
        private UILabel yearErrorLab;


        //inputArea if for test the iPad key board pop controller
        //private IPadNumericKeyBoardComponent inputArea; 
        #endregion

        private ClaimsHistoryDisplayByViewModel model;

        #region declare Margin
        private float topMargin = Constants.NAV_HEIGHT + 20f;
        private float leftMargin = 20f;
        private float bottomMargin = Helpers.BottomNavHeight() + 10f;
        private float belowMargin = 10f;

        private float dateTabPercent = 0.3333f;//because three buttons
        private float threeButtonsHeight = 60f;
        private float threeButtonsSpace = 6f;//value must be times of 3 because there are three buttons
        private float buttWidthMinus = 0f;

        private float tabContentContainerHeight = 280f;//=startEndDateLabelHight +datePickerContainerHeight
        private float datePickerContainerHeight = 0f;// =closeButtHeight+startDatePickerPhoneHeight
        private float closeButtHeight = 30f;
        private float startDatePickerPhoneHeight = 200f;
        private float startEndDateLabelWidth = 0.5f;
        private float startEndDateLabelHeight = 48f;
        private float startDatePickerHeight = 20f;// data picker for tablet 
        private float yearPickerHight = 220f;
        private float doneButtTopMargin = 0f;
        private float doneButtHeight = 70f;//Constants.DEFAULT_BUTTON_HEIGHT; 
        private float tabContentContainerRegularHeight = 240f;
        private float tabContentContinerTallestHeight = 400f;
        #endregion

        public ClaimsHistoryDisplayByView()
        {
        }

        public override void ViewDidLoad()
        {
            View = new GSCFluentLayoutBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            base.ViewDidLoad();
            model = this.ViewModel as ClaimsHistoryDisplayByViewModel;
            model.DisplayByEntryComplete += ModelDisplayByEntryComplete;
            model.PropertyChanged += Model_PropertyChanged;
            displayByCount = model.DisplayBy.Count;
            View.UserInteractionEnabled = true;

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.SetHidesBackButton(true, false);
            base.NavigationItem.Title = model.DisplayByLabel.ToUpper();

            #region Layout Components
            rootScrollView = ((GSCFluentLayoutBaseView)View).baseScrollContainer;
            rootScrollView.ScrollEnabled = true;
            rootScrollView.UserInteractionEnabled = true;
            View.AddSubview(rootScrollView);

            yearToDateButt = new GreenNavButton(true);
            yearToDateButt.SetTitle(model.DisplayBy[0].Value.ToUpper(), UIControlState.Normal);
            yearToDateButt.TouchUpInside += YearToDateButtonClick;
            rootScrollView.AddSubview(yearToDateButt);

            rangeButt = new GreenNavButton(true);
            rangeButt.SetTitle(model.DisplayBy[1].Value.ToUpper(), UIControlState.Normal);
            rangeButt.TouchUpInside += RangeButtonClick;
            rootScrollView.AddSubview(rangeButt);

            if (displayByCount == 3)
            {
                yearButt = new GreenNavButton(true);
                yearButt.SetTitle(model.DisplayBy[2].Value.ToUpper(), UIControlState.Normal);
                yearButt.TouchUpInside += YearButtonClick;
                rootScrollView.AddSubview(yearButt);
            }
            tabContentContainer = new UIView();
            tabContentContainer.UserInteractionEnabled = true;
            rootScrollView.AddSubview(tabContentContainer);

            yearToDateLab = new UILabel();
            yearToDateLab.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.TEXT_FIELD_SUB_HEADING_SIZE);
            yearToDateLab.TextColor = Colors.DARK_GREY_COLOR;
            yearToDateLab.LineBreakMode = UILineBreakMode.WordWrap;
            yearToDateLab.Lines = 0;
            yearToDateLab.Text = model.YearToDateLabel;
            yearToDateLab.TextAlignment = UITextAlignment.Center;
            yearToDateLab.Hidden = true;
            tabContentContainer.AddSubview(yearToDateLab);
            if (isPhone)
            {
                #region date picker for phone 
                startDateLab = new ClaimForLabel();
                startDateLab.Text = model.StartLabel;
                tabContentContainer.AddSubview(startDateLab);

                endDateLab = new ClaimForLabel();
                endDateLab.Text = model.EndLabel;
                tabContentContainer.AddSubview(endDateLab);

                startDateErrorButt = new ErrorButton();
                startDateErrorButt.Hidden = true;
                startDateErrorButt.TouchUpInside += startDateErrorButtTouch;
                tabContentContainer.AddSubview(startDateErrorButt);

                endDateErrorButt = new ErrorButton();
                endDateErrorButt.Hidden = true;
                endDateErrorButt.TouchUpInside += endDateErrorButtTouch;
                tabContentContainer.AddSubview(endDateErrorButt);

                startDateTxt = new DatePickerLabel();
                startDateTxt.ShowDatePickerEvent += StartDateTxtTouchDown;
                tabContentContainer.AddSubview(startDateTxt);

                endDateTxt = new DatePickerLabel();
                endDateTxt.ShowDatePickerEvent += EndDateTxtTouchDown;
                tabContentContainer.AddSubview(endDateTxt);

                datePickerContainer = new UIView() { BackgroundColor = Colors.LightGrayColor };
                tabContentContainer.AddSubview(datePickerContainer);

                datePickerCloseButt = new UIButton();
                datePickerCloseButt.Hidden = true;
                datePickerCloseButt.SetTitle(model.CloseLabel, UIControlState.Normal);
                datePickerCloseButt.SetTitleColor(Colors.HIGHLIGHT_COLOR, UIControlState.Normal);
                datePickerCloseButt.TouchUpInside += datePickerCloseButtTouch;
                datePickerContainer.AddSubview(datePickerCloseButt);

                DateTime dateMin = DateTime.Parse(Constants.MIN_DATE);
                NSDate nsDateMin = (NSDate)DateTime.SpecifyKind(dateMin, DateTimeKind.Utc);
                DateTime dateMax = DateTime.Now;
                NSDate nsDateMax = (NSDate)DateTime.SpecifyKind(dateMax, DateTimeKind.Utc);

                startDatePickerPhone = new UIDatePicker();
                startDatePickerPhone.BackgroundColor = Colors.LightGrayColor;
                startDatePickerPhone.Mode = UIDatePickerMode.Date;
                startDatePickerPhone.MinimumDate = nsDateMin;
                startDatePickerPhone.MaximumDate = nsDateMax;
                startDatePickerPhone.Hidden = true;
                datePickerContainer.AddSubview(startDatePickerPhone);

                endDatePickerPhone = new UIDatePicker();
                endDatePickerPhone.BackgroundColor = Colors.LightGrayColor;
                endDatePickerPhone.Mode = UIDatePickerMode.Date;
                endDatePickerPhone.MinimumDate = nsDateMin;
                endDatePickerPhone.MaximumDate = nsDateMax;
                endDatePickerPhone.Hidden = true;
                datePickerContainer.AddSubview(endDatePickerPhone);
                #endregion
            }
            else
            {
                #region date picker for tablet
                startDatePicker = new ClaimTreatmentDetailsTitleAndDatePicker(this, model.StartLabel);
                startDatePicker.DefaultCurrentDate = false;
                startDatePicker.DateSet += StartDatePicker_SetDate;
                startDatePicker.IsUsedInClaimHistoryDisplayByView = true;
                tabContentContainer.AddSubview(startDatePicker);

                endDatePicker = new ClaimTreatmentDetailsTitleAndDatePicker(this, model.EndLabel);
                endDatePicker.IsUsedInClaimHistoryDisplayByView = true;
                endDatePicker.DefaultCurrentDate = false;
                endDatePicker.DateSet += EndDatePicker_SetDate;
                tabContentContainer.AddSubview(endDatePicker);
                #endregion
            }

            if (displayByCount == 3 || model.IsYearsVisible)
            {
                yearPicker = new YearPickerView();
                yearPicker.UserInteractionEnabled = true;
                yearPicker.HideErrorButtonEvent += HideYearErrorButton;
                pickerViewModel = new YearPickerSource(yearPicker);
                yearPicker.Model = pickerViewModel;
                yearPicker.ShowSelectionIndicator = true;
                yearPicker.BackgroundColor = Colors.LightGrayColor;
                tabContentContainer.AddSubview(yearPicker);

                yearErrorButt = new ErrorButton();
                yearErrorButt.UserInteractionEnabled = true;
                // yearErrorButt.TouchUpInside+=YearErrorButtTouch;
                tabContentContainer.AddSubview(yearErrorButt);

                yearErrorLab = new UILabel();
                yearErrorLab.TextColor = Colors.DARK_GREY_COLOR;
                tabContentContainer.AddSubview(yearErrorLab);
            }

            if (model.SelectedDisplayBy.Key.Equals(GSCHelper.DisplayByYearToDateKey, StringComparison.OrdinalIgnoreCase))
            {
                yearToDateLab.Hidden = false;
            }

            doneButt = new GSButton24();
            doneButt.SetTitle(model.DoneLabel.ToUpper(), UIControlState.Normal);
            rootScrollView.AddSubview(doneButt);
            //          if(!Constants.IsPhone()) //code for test the iPad key board pop controller
            //            {
            //            inputArea=new IPadNumericKeyBoardComponent (this);
            //            inputArea .BackgroundColor =Constants .LIGHT_GREY_COLOR;
            //            rootScrollView .AddSubview (inputArea );
            //            } 
            #endregion

            #region Set Data Binding
            var set = this.CreateBindingSet<ClaimsHistoryDisplayByView, ClaimsHistoryDisplayByViewModel>();
            set.Bind(yearToDateButt).For(b => b.Selected).To(vm => vm.SelectedDisplayBy).WithConversion("ClaimHistoryDisplayByHighlight", "1");
            //et.Bind(yearToDateLab).For(l=>l.Hidden).To(vm => vm.
            set.Bind(rangeButt).For(b => b.Selected).To(vm => vm.SelectedDisplayBy).WithConversion("ClaimHistoryDisplayByHighlight", "2");

            if (isPhone)
            {
                set.Bind(startDateTxt).To(vm => vm.SelectedStartDate).WithConversion("DefaultDateBlank").OneWay();
                set.Bind(endDateTxt).To(vm => vm.SelectedEndDate).WithConversion("DefaultDateBlank").OneWay();
                set.Bind(startDatePickerPhone).To(vm => vm.SelectedStartDate);
                set.Bind(endDatePickerPhone).To(vm => vm.SelectedEndDate);
                set.Bind(startDateErrorButt).For(b => b.Hidden).To(vm => vm.StartDateValidationMessage).WithConversion("ErrorMessageToHideShow");
                set.Bind(endDateErrorButt).For(b => b.Hidden).To(vm => vm.EndDateValidationMessage).WithConversion("ErrorMessageToHideShow");
                set.Bind(startDateTxt).For(l => l.Hidden).To(vm => vm.IsDateRangeVisible).WithConversion("BoolOpposite");
                set.Bind(endDateTxt).For(l => l.Hidden).To(vm => vm.IsDateRangeVisible).WithConversion("BoolOpposite");
                set.Bind(startDateLab).For(l => l.Hidden).To(vm => vm.IsDateRangeVisible).WithConversion("BoolOpposite");
                set.Bind(endDateLab).For(l => l.Hidden).To(vm => vm.IsDateRangeVisible).WithConversion("BoolOpposite");

            }
            else
            {
                set.Bind(startDatePicker.dateController.datePicker).To(vm => vm.SelectedStartDate);
                set.Bind(startDatePicker.detailsLabel).To(vm => vm.SelectedStartDate).WithConversion("DateToString").OneWay();
                set.Bind(endDatePicker.dateController.datePicker).To(vm => vm.SelectedEndDate);
                set.Bind(endDatePicker.detailsLabel).To(vm => vm.SelectedEndDate).WithConversion("DateToString").OneWay();
            }

            if (displayByCount == 3)
            {
                set.Bind(yearButt).For(b => b.Selected).To(vm => vm.SelectedDisplayBy).WithConversion("ClaimHistoryDisplayByHighlight", "3");
                set.Bind(yearPicker).For(p => p.Hidden).To(vm => vm.IsYearsVisible).WithConversion("BoolOpposite");
                set.Bind(pickerViewModel).For(p => p.ItemsSource).To(vm => vm.Years);
                set.Bind(pickerViewModel).For(p => p.SelectedItem).To(vm => vm.SelectedYear);
                set.Bind(yearErrorButt).For(b => b.Hidden).To(vm => vm.YearValidationMessage).WithConversion("ErrorMessageToHideShow");
                set.Bind(yearErrorLab).For(b => b.Hidden).To(vm => vm.YearValidationMessage).WithConversion("ErrorMessageToHideShow");
                set.Bind(yearErrorLab).To(vm => vm.YearValidationMessage);
            }
            set.Bind(doneButt).To(vm => vm.DoneCommand);
            set.Apply();
            #endregion

            if (!string.IsNullOrEmpty(model.SelectedDisplayBy.Key))
            {
                SetConstraints(model.SelectedDisplayBy.Key);
            }

        }

        private void SetConstraints(string displayByKey)
        {
            #region caculate Done Button top margin and DisplayBy buttons width
            if (Constants.IsPhone())
            {
                topMargin = (Helpers.IsInPortraitMode()) ? Constants.NAV_HEIGHT + 20f : Constants.NAV_HEIGHT;

            }

            doneButtTopMargin = (float)(topMargin + threeButtonsHeight + belowMargin + tabContentContainerHeight);
            if (displayByCount == 3)
            {
                dateTabPercent = 0.3333f;
                buttWidthMinus = threeButtonsSpace * 2 / 3 + leftMargin * 2 / 3;
            }
            else
            {
                dateTabPercent = 0.5f;
                buttWidthMinus = threeButtonsSpace / 2 + leftMargin;
            }
            #endregion

            #region set constraints for DisplayBy buttons and Done button
            View.RemoveConstraints(View.Constraints);
            rootScrollView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            tabContentContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                View.AddConstraints(

                rootScrollView.AtTopOf(View, View.SafeAreaInsets.Top + 10),
                rootScrollView.AtLeftOf(View, View.SafeAreaInsets.Left),
                rootScrollView.AtRightOf(View, View.SafeAreaInsets.Right),
                rootScrollView.AtBottomOf(View, View.SafeAreaInsets.Bottom),
                rootScrollView.WithSameHeight(View).Minus(topMargin)

            );

                View.AddConstraints(

                    yearToDateButt.AtTopOf(rootScrollView),
                    yearToDateButt.AtLeftOf(rootScrollView, leftMargin),
                    yearToDateButt.WithRelativeWidth(rootScrollView, dateTabPercent).Minus(buttWidthMinus),
                    yearToDateButt.Height().EqualTo(threeButtonsHeight),

                    rangeButt.WithSameCenterY(yearToDateButt),
                    rangeButt.ToRightOf(yearToDateButt).Plus(threeButtonsSpace),
                    rangeButt.WithSameWidth(yearToDateButt),
                    rangeButt.WithSameHeight(yearToDateButt),

                    tabContentContainer.WithSameLeft(rootScrollView),
                    tabContentContainer.WithSameWidth(rootScrollView),
                    tabContentContainer.Below(yearToDateButt),
                    tabContentContainer.Height().EqualTo(tabContentContainerHeight),

                    doneButt.WithSameLeft(yearToDateButt),
                    doneButt.WithSameWidth(rootScrollView).Minus(leftMargin * 2),
                    doneButt.Height().EqualTo(doneButtHeight),
                    doneButt.Below(tabContentContainer, belowMargin * 2),
                    doneButt.AtBottomOf(rootScrollView, belowMargin) //for test the iPad key board pop controller please comment out this line and add comma in line above!

                );
            }
            else
            {
                View.AddConstraints(
                rootScrollView.AtTopOf(View, topMargin),
                rootScrollView.AtLeftOf(View),
                rootScrollView.WithSameWidth(View),
                rootScrollView.WithSameHeight(View).Minus(topMargin),

                yearToDateButt.AtTopOf(rootScrollView),
                yearToDateButt.AtLeftOf(rootScrollView, leftMargin),
                yearToDateButt.WithRelativeWidth(rootScrollView, dateTabPercent).Minus(buttWidthMinus),
                yearToDateButt.Height().EqualTo(threeButtonsHeight),

                rangeButt.WithSameCenterY(yearToDateButt),
                rangeButt.ToRightOf(yearToDateButt).Plus(threeButtonsSpace),
                rangeButt.WithSameWidth(yearToDateButt),
                rangeButt.WithSameHeight(yearToDateButt),

                tabContentContainer.WithSameLeft(rootScrollView),
                tabContentContainer.WithSameWidth(rootScrollView),
                tabContentContainer.Below(yearToDateButt),
                tabContentContainer.Height().EqualTo(tabContentContainerHeight),

                doneButt.WithSameLeft(yearToDateButt),
                doneButt.WithSameWidth(rootScrollView).Minus(leftMargin * 2),
                doneButt.Height().EqualTo(doneButtHeight),
                doneButt.Below(tabContentContainer, belowMargin * 2),
                doneButt.AtBottomOf(rootScrollView, belowMargin) //for test the iPad key board pop controller please comment out this line and add comma in line above!

            );
            }

            //            if(!Constants.IsPhone ())//code for test the iPad key board pop controller
            //            {
            //                View.AddConstraints(
            //                    inputArea .Below (doneButt ),
            //                    inputArea .WithSameLeft (doneButt ),
            //                    inputArea .Width().EqualTo (150),
            //                    inputArea .Height ().EqualTo (30),
            //                    inputArea .AtBottomOf (rootScrollView ,belowMargin));
            //
            //            }  
            if (displayByCount == 3)
            {
                View.AddConstraints(
                    yearButt.WithSameCenterY(rangeButt),
                    yearButt.ToRightOf(rangeButt).Plus(threeButtonsSpace),
                    yearButt.WithSameWidth(rangeButt),
                    yearButt.WithSameHeight(rangeButt)
                );
            }
            #endregion

            #region Set Constraints for tab content base on DisplayBy key
            switch (displayByKey)
            {
                case GSCHelper.DisplayByYearToDateKey:
                    View.AddConstraints(
                        yearToDateLab.AtTopOf(tabContentContainer, belowMargin * 4),
                        yearToDateLab.AtLeftOf(tabContentContainer, leftMargin),
                        yearToDateLab.WithSameWidth(tabContentContainer).Minus(leftMargin * 2)
                    );
                    break;
                case GSCHelper.DisplayByDateRangeKey:
                    if (isPhone)
                    {
                        startDateTxt.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
                        datePickerContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
                        View.AddConstraints(
                            startDateLab.AtTopOf(tabContentContainer, (nfloat)(belowMargin * 3.5)),
                            startDateLab.AtLeftOf(tabContentContainer, leftMargin),

                            startDateErrorButt.ToLeftOf(startDateTxt),
                            startDateErrorButt.WithSameCenterY(startDateLab),

                            startDateTxt.WithSameCenterY(startDateLab),
                            startDateTxt.WithRelativeWidth(tabContentContainer, startEndDateLabelWidth),
                            startDateTxt.Height().EqualTo(startEndDateLabelHeight),
                            startDateTxt.Right().EqualTo().RightOf(tabContentContainer).Minus(leftMargin),

                            endDateLab.WithSameLeft(startDateLab),
                            endDateLab.Below(startDateLab, belowMargin * 5),

                            endDateErrorButt.ToLeftOf(endDateTxt),
                            endDateErrorButt.WithSameCenterY(endDateLab),

                            endDateTxt.WithSameCenterY(endDateLab),
                            endDateTxt.WithRelativeWidth(tabContentContainer, startEndDateLabelWidth),
                            endDateTxt.Height().EqualTo(startEndDateLabelHeight),
                            endDateTxt.WithSameRight(startDateTxt),

                            datePickerContainer.WithSameCenterX(tabContentContainer),
                            datePickerContainer.WithSameWidth(tabContentContainer).Minus(leftMargin * 2),
                            datePickerContainer.Height().EqualTo(datePickerContainerHeight),
                            datePickerContainer.Below(endDateTxt, belowMargin),

                            datePickerCloseButt.WithSameRight(datePickerContainer).Minus(leftMargin),
                            datePickerCloseButt.WithSameTop(datePickerContainer),
                            datePickerCloseButt.Height().EqualTo(closeButtHeight)
                        );

                        if (isSelectStartDate)
                        {
                            View.AddConstraints(
                                startDatePickerPhone.AtLeftOf(datePickerContainer),
                                startDatePickerPhone.WithSameWidth(datePickerContainer),
                                startDatePickerPhone.Height().EqualTo(startDatePickerPhoneHeight),
                                startDatePickerPhone.Below(datePickerCloseButt, belowMargin)
                            );
                        }
                        else
                        {
                            View.AddConstraints(
                                endDatePickerPhone.AtLeftOf(datePickerContainer),
                                endDatePickerPhone.WithSameWidth(datePickerContainer),
                                endDatePickerPhone.Height().EqualTo(startDatePickerPhoneHeight),
                                endDatePickerPhone.Below(datePickerCloseButt, belowMargin)
                            );
                        }
                    }
                    else
                    {
                        View.AddConstraints(
                            startDatePicker.AtTopOf(tabContentContainer, (nfloat)(belowMargin * 3.5)),
                            startDatePicker.AtLeftOf(tabContentContainer, leftMargin),
                            startDatePicker.WithSameWidth(tabContentContainer).Minus(leftMargin * 2),
                            startDatePicker.Height().EqualTo(startDatePickerHeight),

                            endDatePicker.WithSameLeft(startDatePicker),
                            endDatePicker.WithSameWidth(startDatePicker),
                            endDatePicker.Height().EqualTo(startDatePickerHeight),
                            endDatePicker.Below(startDatePicker, (nfloat)(belowMargin * 3))
                        );
                    }
                    break;
                case GSCHelper.DisplayByYearKey:
                    View.AddConstraints(
                        yearPicker.WithSameCenterX(tabContentContainer),
                        yearPicker.WithSameCenterY(tabContentContainer),//.Minus(belowMargin),
                        yearPicker.WithSameWidth(tabContentContainer).Minus(leftMargin * 2),
                        yearPicker.Height().EqualTo(yearPickerHight),

                        yearErrorButt.WithSameLeft(yearPicker).Plus(leftMargin),
                        yearErrorButt.Below(yearPicker),

                        yearErrorLab.ToRightOf(yearErrorButt, leftMargin / 5),
                        yearErrorLab.WithSameCenterY(yearErrorButt)
                    );
                    break;
            }
            #endregion
        }

        #region private methods to reset constraints 
        private void YearToDateButtonClick(object sender, EventArgs e)
        {
            ResetConstraintsByTabSelected(0);
        }

        private void RangeButtonClick(object sender, EventArgs e)
        {
            ResetConstraintsByTabSelected(1);
        }

        private void YearButtonClick(object sender, EventArgs e)
        {
            ResetConstraintsByTabSelected(2);
        }

        private void ResetConstraintsByTabSelected(int tabIndex)
        {
            model.SelectedDisplayBy = model.DisplayBy[tabIndex];
            tabContentContainerHeight = tabContentContainerRegularHeight;
            switch (tabIndex)
            {
                case 0:
                    yearToDateLab.Hidden = false;
                    if (isPhone)
                        HideDatePickerForPhone();
                    break;
                case 1:
                    var date = DateTime.Today;
                    //  model.SelectedStartDate = date;
                    // model.SelectedEndDate = date;
                    yearToDateLab.Hidden = true;
                    if (isPhone)
                    {
                        if (!startDatePickerPhone.Hidden || !endDatePickerPhone.Hidden)
                            tabContentContainerHeight = tabContentContinerTallestHeight;
                    }
                    break;
                case 2:
                    yearToDateLab.Hidden = true;
                    if (isPhone)
                        HideDatePickerForPhone();
                    break;

            }
            SetConstraints(model.SelectedDisplayBy.Key);
        }

        private void StartDateTxtTouchDown(object sender, EventArgs e)
        {
            isSelectStartDate = true;
            var date = DateTime.Today;
            model.SelectedStartDate = date;
            SetShowHideDatePicker(isSelectStartDate);
        }

        private void EndDateTxtTouchDown(object sender, EventArgs e)
        {
            isSelectStartDate = false;
            var date = DateTime.Today;
            model.SelectedEndDate = date;
            SetShowHideDatePicker(isSelectStartDate);

        }

        private void SetShowHideDatePicker(bool isChooseStartDate)
        {
            if (isChooseStartDate)
            {
                //                if (!startDateErrorButt.Hidden)
                //                    startDateErrorButt.Hidden = true;
                startDatePickerPhone.Hidden = false;
                endDatePickerPhone.Hidden = true;
            }
            else
            {
                //                if (!endDateErrorButt.Hidden)
                //                    endDateErrorButt.Hidden = true;
                endDatePickerPhone.Hidden = false;
                startDatePickerPhone.Hidden = true;
            }
            tabContentContainerHeight = tabContentContinerTallestHeight;
            datePickerCloseButt.Hidden = false;
            datePickerContainerHeight = closeButtHeight + startDatePickerPhoneHeight;
            SetConstraints(model.SelectedDisplayBy.Key);
        }

        private void HideDatePickerForPhone()
        {
            if (isSelectStartDate)
            {
                startDatePickerPhone.Hidden = true;
            }
            else
            {
                endDatePickerPhone.Hidden = true;
            }
            datePickerCloseButt.Hidden = true;
            datePickerContainerHeight = 0f;
        }

        private void datePickerCloseButtTouch(object sender, EventArgs e)
        {
            HideDatePickerForPhone();
            tabContentContainerHeight = tabContentContainerRegularHeight;
            SetConstraints(model.SelectedDisplayBy.Key);
        }
        #endregion

        #region Validation Methods
        private void startDateErrorButtTouch(object sender, EventArgs e)
        {
            UIAlertView _error = new UIAlertView("", model.StartDateValidationMessage, null, "ok".tr(), null);
            _error.Show();
        }

        private void endDateErrorButtTouch(object sender, EventArgs e)
        {
            UIAlertView _error = new UIAlertView("", model.EndDateValidationMessage, null, "ok".tr(), null);
            _error.Show();
        }

        private void YearErrorButtTouch(object sender, EventArgs e)
        {
            UIAlertView _error = new UIAlertView("", model.YearValidationMessage, null, "ok".tr(), null);
            _error.Show();
        }

        private void HideYearErrorButton(object sender, EventArgs e)
        {
            if (!yearErrorButt.Hidden)
            {
                yearErrorButt.Hidden = true;
            }
            if (!yearErrorLab.Hidden)
            {
                yearErrorLab.Hidden = true;
            }
        }
        #endregion

        private void StartDatePicker_SetDate(object sender, EventArgs e)
        {
            if (model.SelectedStartDate == null || model.SelectedStartDate == DateTime.MaxValue)
                model.SelectedStartDate = DateTime.Now;
        }

        private void EndDatePicker_SetDate(object sender, EventArgs e)
        {
            if (model.SelectedEndDate == null || model.SelectedEndDate == DateTime.MaxValue)
                model.SelectedEndDate = DateTime.Now;
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetConstraints(model.SelectedDisplayBy.Key);
        }



        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            base.NavigationItem.Title = " ".tr().ToUpper();
        }

        void ModelDisplayByEntryComplete(object sender, EventArgs e)
        {
            int backTo = 2;
            base.NavigationController.PopToViewController(base.NavigationController.ViewControllers[base.NavigationController.ViewControllers.Length - backTo], true);
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            SetConstraints(model.SelectedDisplayBy.Key);
            base.DidRotate(fromInterfaceOrientation);
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedStartDate")
            {
                if (Constants.IsPhone())
                {
                    if (model.SelectedStartDate != null)
                    {
                        startDateTxt.Text = ((DateTime)(model.SelectedStartDate)).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        startDateTxt.Text = string.Empty;
                    }
                }
            }
            if (e.PropertyName == "SelectedEndDate")
            {
                if (Constants.IsPhone())
                {
                    if (model.SelectedStartDate != null)
                    {
                        endDateTxt.Text = ((DateTime)(model.SelectedEndDate)).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        endDateTxt.Text = string.Empty;
                    }
                }
            }
            if (!Constants.IsPhone())
            {
                if (e.PropertyName == "EndDateValidationMessage")
                {
                    // if(!string.IsNullOrEmpty (model.EndDateValidationMessage))
                    endDatePicker.showError(model.EndDateValidationMessage);
                }

                if (e.PropertyName == "StartDateValidationMessage")
                {
                    //if(!string.IsNullOrEmpty (model.StartDateValidationMessage))
                    startDatePicker.showError(model.StartDateValidationMessage);
                }
            }
        }
    }
}

