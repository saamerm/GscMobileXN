using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS
{
    [Register("EligibilityCheckDREView")]
    public class EligibilityCheckDREView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
    {
        protected EligibilityCheckDREViewModel _model;

        protected UIBarButtonItem cancelButton;

        protected UIScrollView scrollableContainer;

        protected UITableView submissionTableView;

        protected UILabel eligibilityDRENote;

        protected UILabel eligibilityNote1;
        protected UILabel eligibilityNote2;

        protected UILabel inquiryDateLabel;

        protected UILabel headerLabel;

        GSButton submitButton;

        protected GSButton doneButton;

        public EligibilityCheckDREView()
        {
        }

        public override void ViewDidLoad()
        {
            View = new GSCBaseView
            {
                BackgroundColor = Colors.BACKGROUND_COLOR
            };

            base.ViewDidLoad();

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = "eligibilityResults".tr();

            _model = (EligibilityCheckDREViewModel)ViewModel;

            this.AutomaticallyAdjustsScrollViewInsets = false;

            base.NavigationItem.SetHidesBackButton(true, false);

            scrollableContainer = ((GSCBaseView)View).baseScrollContainer;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollableContainer);

            ((GSCBaseView)View).ViewTapped += HandleViewTapped;

            headerLabel = new UILabel();
            headerLabel.Text = "dateOfEligiblity".tr();
            headerLabel.BackgroundColor = Colors.Clear;
            headerLabel.TextColor = Colors.DARK_GREY_COLOR;
            headerLabel.TextAlignment = UITextAlignment.Left;
            headerLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.LG_HEADING_FONT_SIZE);
            scrollableContainer.AddSubview(headerLabel);

            submissionTableView = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Plain);
            submissionTableView.RowHeight = 55 * 3 + 50 + 10;
            submissionTableView.TableHeaderView = new UIView();
            submissionTableView.SeparatorColor = Colors.Clear;
            submissionTableView.ShowsVerticalScrollIndicator = true;
            submissionTableView.UserInteractionEnabled = false;
            scrollableContainer.AddSubview(submissionTableView);

            eligibilityDRENote = new UILabel();
            eligibilityDRENote.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.SMALL_FONT_SIZE);
            eligibilityDRENote.TextColor = Colors.DARK_GREY_COLOR;
            eligibilityDRENote.TextAlignment = UITextAlignment.Left;
            eligibilityDRENote.BackgroundColor = Colors.Clear;
            eligibilityDRENote.LineBreakMode = UILineBreakMode.WordWrap;
            eligibilityDRENote.Lines = 0;
            eligibilityDRENote.Text = "eligibilityDRENote".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada);
            scrollableContainer.AddSubview(eligibilityDRENote);

            eligibilityNote1 = new UILabel();
            eligibilityNote1.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.SMALL_FONT_SIZE);
            eligibilityNote1.TextColor = Colors.DARK_GREY_COLOR;
            eligibilityNote1.TextAlignment = UITextAlignment.Left;
            eligibilityNote1.BackgroundColor = Colors.Clear;
            eligibilityNote1.LineBreakMode = UILineBreakMode.WordWrap;
            eligibilityNote1.Lines = 0;

            if (!_model.PhoneBusy)
            {
                if (!_model.NoPhoneAlteration)
                {
                    eligibilityNote1.Text = LocalizableBrand.EligibilityInquiryNote
                        .FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada, _model.PhoneNumber.Text);
                }
                else
                {
                    eligibilityNote1.Text = LocalizableBrand.EligibilityInquiryNote.
                    FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada, LocalizableBrand.PhoneNumber);
#if FPPM
                    eligibilityNote1.Text = LocalizableBrand.EligibilityInquiryNote.
                    FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada, LocalizableBrand.PhoneNumberFPPM);
#endif
                }
            }
            else
            {
                ((GSCBaseView)View).startLoading();
            }
            scrollableContainer.AddSubview(eligibilityNote1);

            eligibilityNote2 = new UILabel();
            eligibilityNote2.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.SMALL_FONT_SIZE);
            eligibilityNote2.TextColor = Colors.DARK_GREY_COLOR;
            eligibilityNote2.TextAlignment = UITextAlignment.Left;
            eligibilityNote2.BackgroundColor = Colors.Clear;
            eligibilityNote2.LineBreakMode = UILineBreakMode.WordWrap;
            eligibilityNote2.Lines = 0;
            eligibilityNote2.Text = "eligibilityInquiryNote2".tr();
            scrollableContainer.AddSubview(eligibilityNote2);

            inquiryDateLabel = new UILabel();
            inquiryDateLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.SMALL_FONT_SIZE);
            inquiryDateLabel.TextColor = Colors.DARK_GREY_COLOR;
            inquiryDateLabel.TextAlignment = UITextAlignment.Right;
            inquiryDateLabel.BackgroundColor = Colors.Clear;
            inquiryDateLabel.LineBreakMode = UILineBreakMode.WordWrap;
            inquiryDateLabel.Lines = 0;
            inquiryDateLabel.Text = "dateOfInquiry".tr() + " " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString() + " " + (TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now) ? TimeZone.CurrentTimeZone.DaylightName : TimeZone.CurrentTimeZone.StandardName);
            scrollableContainer.AddSubview(inquiryDateLabel);

            doneButton = new GSButton();
            doneButton.SetTitle("submitAnotherEligibilityCheck".tr(), UIControlState.Normal);
            scrollableContainer.AddSubview(doneButton);

            var set = this.CreateBindingSet<EligibilityCheckDREView, Core.ViewModels.EligibilityCheckDREViewModel>();
            set.Bind(this).For(v => v.ParticipantEligibilityResults).To(vm => vm.EligibilityCheckResults.Result.ParticipantEligibilityResults);
            set.Bind(this.doneButton).To(vm => vm.SubmitAnotherEligibilityCheckCommand);
            set.Bind(this).For(v => v.PhoneBusy).To(vm => vm.PhoneBusy);
            set.Apply();
        }

        public void layoutComponents()
        {
            if (!_model.NoPhoneAlteration)
            {
                eligibilityNote1.Text = LocalizableBrand.EligibilityInquiryNote.
                    FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada, _model.PhoneNumber.Text);
            }
            else
            {
                eligibilityNote1.Text = LocalizableBrand.EligibilityInquiryNote.
                    FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada, LocalizableBrand.PhoneNumber);
#if FPPM
                eligibilityNote1.Text =LocalizableBrand.EligibilityInquiryNote.
                    FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada, LocalizableBrand.PhoneNumberFPPM);
#endif
            }
            View.SetNeedsLayout();
        }

        void HandleViewTapped(object sender, EventArgs e)
        {
            dismissKeyboard();
        }

        void dismissKeyboard()
        {
            this.View.EndEditing(true);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ((GSCBaseView)View).subscribeToBusyIndicator();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ((GSCBaseView)View).unsubscribeFromBusyIndicator();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
            float itemPadding = Constants.CLAIMS_DETAILS_COMPONENT_PADDING;

            float centerX = ViewContainerWidth / 2;
            float yPos = ViewContentYPositionPadding;

            headerLabel.Frame = new CGRect(Constants.CONTENT_SIDE_PADDING, yPos, ViewContainerWidth - Constants.CONTENT_SIDE_PADDING * 2, Constants.DRUG_LOOKUP_LABEL_HEIGHT);

            yPos += (float)headerLabel.Frame.Height;

            if (_model.EligibilityCheckResults != null || _model.EligibilityCheckResults.Result != null || _model.EligibilityCheckResults.Result.ParticipantEligibilityResults != null)
            {
                float listY = yPos;
                float listHeight = _model.EligibilityCheckResults.Result.ParticipantEligibilityResults.Count * (55 * 3 + 50 + 10);
                submissionTableView.Frame = new CGRect(0, listY, ViewContainerWidth, listHeight);
                yPos = (float)submissionTableView.Frame.Height + (float)submissionTableView.Frame.Y + 10;
            }

            eligibilityDRENote.SizeToFit();
            eligibilityDRENote.Frame = new CGRect(sidePadding, yPos + 5, ViewContainerWidth - sidePadding * 2, (float)eligibilityDRENote.Frame.Height + 5);
            yPos += (float)eligibilityDRENote.Frame.Height + itemPadding;

            eligibilityNote1.Frame = new CGRect(sidePadding, yPos, ViewContainerWidth - sidePadding * 2, (float)eligibilityNote1.Frame.Height);
            eligibilityNote1.SizeToFit();
            yPos += (float)eligibilityNote1.Frame.Height + itemPadding;

            eligibilityNote2.Frame = new CGRect(sidePadding, yPos, ViewContainerWidth - sidePadding * 2, (float)eligibilityNote2.Frame.Height);
            eligibilityNote2.SizeToFit();
            yPos += (float)eligibilityNote2.Frame.Height + itemPadding;

            inquiryDateLabel.Frame = new CGRect(sidePadding, yPos, ViewContainerWidth - sidePadding * 2, (float)inquiryDateLabel.Frame.Height);
            inquiryDateLabel.SizeToFit();
            yPos += (float)inquiryDateLabel.Frame.Height + itemPadding;

            doneButton.Frame = new CGRect(ViewContainerWidth / 2 - Constants.DEFAULT_BUTTON_WIDTH / 2, yPos, Constants.DEFAULT_BUTTON_WIDTH, Constants.DEFAULT_BUTTON_HEIGHT);
            yPos += (float)doneButton.Frame.Height + itemPadding;

            scrollableContainer.Frame = new CGRect(0, 0, ViewContainerWidth, ViewContainerHeight);
            scrollableContainer.ContentSize = new CGSize(ViewContainerWidth, yPos + GetBottomPadding());
        }

        private List<ParticipantEligibilityResult> _participantEligibilityResults;
        public List<ParticipantEligibilityResult> ParticipantEligibilityResults
        {
            get
            {
                return _participantEligibilityResults;
            }
            set
            {
                _participantEligibilityResults = value;
                if (value != null)
                {
                    EligibilityCheckMultipleSelectionSource submissionSource = new EligibilityCheckMultipleSelectionSource(ParticipantEligibilityResults, submissionTableView, "EligibilityCheckParticipantTableCell", typeof(EligibilityCheckParticipantTableCell));
                    submissionSource.SourceAssetAdded += HandleSourceAssetAdded;
                    submissionSource.SourceAssetsRemoved += HandleSourceAssetRemoved;
                    submissionTableView.Source = submissionSource;
                    var set = this.CreateBindingSet<EligibilityCheckDREView, Core.ViewModels.EligibilityCheckDREViewModel>();
                    set.Bind(submissionSource).To(vm => vm.EligibilityCheckResults.Result.ParticipantEligibilityResults);
                    set.Apply();

                    submissionTableView.ReloadData();
                }

            }
        }

        void HandleSourceAssetAdded(object sender, AssetEventArgs e)
        {
            System.Console.WriteLine(e.Asset.ParticipantFullName);

            if (_model != null)
            {

                if (!_model.SelectedParticipants.Contains(e.Asset))
                {
                    _model.SelectedParticipants.Add(e.Asset);
                }
            }

        }

        void HandleSourceAssetRemoved(object sender, AssetEventArgs e)
        {
            System.Console.WriteLine(e.Asset.ParticipantFullName);

            if (_model != null)
            {

                if (_model.SelectedParticipants.Contains(e.Asset))
                {
                    _model.SelectedParticipants.Remove(e.Asset);
                }
            }

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
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0) && Constants.IsPhone())
            {
                return Constants.CLAIMS_DETAILS_COMPONENT_PADDING;
            }
            else
            {
                return Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;
            }
        }

        private bool _phonebusy = true;
        public bool PhoneBusy
        {
            get
            {
                return _phonebusy;
            }
            set
            {
                _phonebusy = value;
                if (!_phonebusy)
                {
                    layoutComponents();
                    InvokeOnMainThread(() =>
                    {
                        ((GSCBaseView)View).stopLoading();
                    });
                }
                else
                {
                    InvokeOnMainThread(() =>
                    {
                        ((GSCBaseView)View).startLoading();
                    });
                }

            }
        }
    }
}