using CoreGraphics;
using Foundation;
using MobileClaims.Core.Services.Requests;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Converters;
using MobileClaims.iOS.UI;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using System;
using MobileClaims.Core.Converters;
using UIKit;

namespace MobileClaims.iOS.Views
{
    [FromLeftToRightTransition]
    public partial class RefineSearchViewController : GSCBaseViewController
    {
        private RefineSearchViewModel _viewModel;
        private bool viewWasCreated;

        public RefineSearchViewController() : base()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _viewModel = (RefineSearchViewModel)ViewModel;

            float navigationBarHeight = Constants.IsPhone() ? 0 : Constants.NAV_BUTTON_SIZE_IPAD;
            bottomScrollViewMargin.Constant = navigationBarHeight;

            SetFonts();
            SetTexts();
            SetHandlers();
            SetBinding();

            doneRefineButton.SetTitleColor(Colors.HIGHLIGHT_COLOR, UIControlState.Normal);
            doneRefineButton.SetTitleColor(Colors.DARK_GREY_COLOR, UIControlState.Highlighted);

            locationTextField.BackgroundColor = Colors.RefineSearchMediumGrayColorWithAlpha;
            locationTextField.LeftView = new UIView(new CGRect(0, 0, 10, 20));
            locationTextField.LeftViewMode = UITextFieldViewMode.Always;

            directBillSwitch.OnTintColor = Colors.HIGHLIGHT_COLOR;
            directBillSwitch.TintColor = Colors.RefineSearchMediumGrayColor;
            directBillSwitch.BackgroundColor = Colors.RefineSearchMediumGrayColor;
            directBillSwitch.Layer.CornerRadius = 16;

            recentlyVisitedSwitch.OnTintColor = Colors.HIGHLIGHT_COLOR;
            recentlyVisitedSwitch.TintColor = Colors.RefineSearchMediumGrayColor;
            recentlyVisitedSwitch.BackgroundColor = Colors.RefineSearchMediumGrayColor;
            recentlyVisitedSwitch.Layer.CornerRadius = 16;

            ratingSwitch.OnTintColor = Colors.HIGHLIGHT_COLOR;
            ratingSwitch.TintColor = Colors.RefineSearchMediumGrayColor;
            ratingSwitch.BackgroundColor = Colors.RefineSearchMediumGrayColor;
            ratingSwitch.Layer.CornerRadius = 16;

            ratingSwitch.Transform = CGAffineTransform.MakeScale(0.7f, 0.7f);
            directBillSwitch.Transform = CGAffineTransform.MakeScale(0.7f, 0.7f);
            recentlyVisitedSwitch.Transform = CGAffineTransform.MakeScale(0.7f, 0.7f);

            locationTextField.onSelect = (string text, NSIndexPath row) => _viewModel.SelectedLocation = _viewModel.LocationSugestions[row.Row];
            locationTextField.autoCompleteTextColor = Colors.HIGHLIGHT_COLOR;
            locationTextField.ClearButtonMode = UITextFieldViewMode.Always;

            distanceButton.TintColor = Colors.HIGHLIGHT_COLOR;
            distanceButton.SetTitleColor(Colors.DarkGrayColor, UIControlState.Normal);

            nameButton.TintColor = Colors.HIGHLIGHT_COLOR;
            nameButton.SetTitleColor(Colors.DarkGrayColor, UIControlState.Normal);

            ratingAndDistanceButton.TintColor = Colors.HIGHLIGHT_COLOR;
            ratingAndDistanceButton.SetTitleColor(Colors.DarkGrayColor, UIControlState.Normal);

            ratingAndNameButton.TintColor = Colors.HIGHLIGHT_COLOR;
            ratingAndNameButton.SetTitleColor(Colors.DarkGrayColor, UIControlState.Normal);

            _distanceNonPharmacyButton.TintColor = Colors.HIGHLIGHT_COLOR;
            _distanceNonPharmacyButton.SetTitleColor(Colors.DarkGrayColor, UIControlState.Normal);

            _nameNonPharmacyButton.TintColor = Colors.HIGHLIGHT_COLOR;
            _nameNonPharmacyButton.SetTitleColor(Colors.DarkGrayColor, UIControlState.Normal);

            locationTextField.ShouldReturn = (text =>
            {
                text.ResignFirstResponder();
                return true;
            });

            viewWasCreated = true;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var boolOppositeValueConverter = new BoolOppositeValueConverter();
            var set = this.CreateBindingSet<RefineSearchViewController, RefineSearchViewModel>();

            set.Bind(RecentlyVisitedView).For(x => x.Hidden).To(vm => vm.IsProviderNotMyFavorites).WithConversion(boolOppositeValueConverter);

            set.Apply();

            SetHandlers();

            if (_viewModel.ViewModelParameter != null && _viewModel.ViewModelParameter.IsDisplayRating)
            {
                ratingView.Hidden = false;
                _sortGroupNonPharmacy.Hidden = true;
                _sortGroupPharmacy.Hidden = false;

                if (!viewWasCreated)
                {
                    SetBindingForRatingStars();
                }
            }
            else
            {
                ratingView.Hidden = true;
                _sortGroupNonPharmacy.Hidden = false;
                _sortGroupPharmacy.Hidden = true;
            }

            viewWasCreated = false;
        }

        private void SetBindingForRatingStars()
        {
            var set = this.CreateBindingSet<RefineSearchViewController, RefineSearchViewModel>();

            var scoreConverter = new ScoreToUIImageConverter();

            set.Bind(star1Button.ImageView)
               .For(x => x.Image)
               .To(vm => vm.ViewModelParameter.StarRating)
               .Mode(MvxBindingMode.OneWay)
               .WithConversion(scoreConverter, 0);
            set.Bind(star2Button.ImageView)
               .For(x => x.Image)
               .To(vm => vm.ViewModelParameter.StarRating)
               .Mode(MvxBindingMode.OneWay)
               .WithConversion(scoreConverter, 1);
            set.Bind(star3Button.ImageView)
               .For(x => x.Image)
               .To(vm => vm.ViewModelParameter.StarRating)
               .Mode(MvxBindingMode.OneWay)
               .WithConversion(scoreConverter, 2);
            set.Bind(star4Button.ImageView)
               .For(x => x.Image)
               .To(vm => vm.ViewModelParameter.StarRating)
               .Mode(MvxBindingMode.OneWay)
               .WithConversion(scoreConverter, 3);
            set.Bind(star5Button.ImageView)
               .For(x => x.Image)
               .To(vm => vm.ViewModelParameter.StarRating)
               .Mode(MvxBindingMode.OneWay)
               .WithConversion(scoreConverter, 4);

            set.Apply();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            this.ClearBindings(star1Button.ImageView);
            this.ClearBindings(star2Button.ImageView);
            this.ClearBindings(star3Button.ImageView);
            this.ClearBindings(star4Button.ImageView);
            this.ClearBindings(star5Button.ImageView);

            UnsetHandlers();
        }

        private void SetBinding()
        {
            var boolOppositeValueConverter = new BoolOppositeValueConverter();
            var set = this.CreateBindingSet<RefineSearchViewController, RefineSearchViewModel>();

            set.Bind(backToCurrentLocationButton).To(vm => vm.SetLocationAsCurrentLocationCommand);

            SetBindingForRatingStars();

            set.Bind(locationTextField).For(v => v.autoCompleteStrings).To(vm => vm.LocationSugestions);
            set.Bind(locationTextField).For(v => v.autoCompleteTableHeight).To(vm => vm.LocationSugestionsHeight);
            set.Bind(locationTextField).For(v => v.Placeholder).To(vm => vm.LocationAutocompleteHint);

            set.Bind(star1Button).To(vm => vm.SetRatingCommand).CommandParameter(1);
            set.Bind(star2Button).To(vm => vm.SetRatingCommand).CommandParameter(2);
            set.Bind(star3Button).To(vm => vm.SetRatingCommand).CommandParameter(3);
            set.Bind(star4Button).To(vm => vm.SetRatingCommand).CommandParameter(4);
            set.Bind(star5Button).To(vm => vm.SetRatingCommand).CommandParameter(5);
            set.Bind(chooseProviderTypeButton).To(vm => vm.ShowHealthProviderTypeListCommand);

            set.Bind(directBillSwitch).To(vm => vm.ViewModelParameter.IsDirectBill);
            set.Bind(recentlyVisitedSwitch).To(vm => vm.ViewModelParameter.IsRecentlyVisited);
            set.Bind(ratingSwitch).To(vm => vm.ViewModelParameter.IsStarRating);
            set.Bind(ratingView).For(x => x.Hidden).To(vm => vm.ViewModelParameter.IsDisplayRating).WithConversion(boolOppositeValueConverter);
            set.Bind(ratingStars).For(x => x.Hidden).To(vm => vm.ViewModelParameter.IsStarRating).WithConversion(boolOppositeValueConverter);
            set.Bind(ratingLabel).For(x => x.Hidden).To(vm => vm.ViewModelParameter.IsStarRating).WithConversion("InvertedVisibility");

            set.Bind(_sortGroupPharmacy).For(x => x.Hidden).To(vm => vm.ViewModelParameter.IsDisplayRating).WithConversion(boolOppositeValueConverter);
            set.Bind(_sortGroupNonPharmacy).For(x => x.Hidden).To(vm => vm.ViewModelParameter.IsDisplayRating).WithConversion("InvertedVisibility");

            var sortByChoiceConverter = new SortByChoiceToBoolValueConverter();

            set.Bind(distanceButton)
                   .For(x => x.Selected)
                   .To(vm => vm.ViewModelParameter.SortByChoicePharmacy)
                   .Mode(MvxBindingMode.OneWay)
                   .WithConversion(sortByChoiceConverter, SortByChoice.SortByDistanceAsc);
            set.Bind(ratingAndDistanceButton)
                   .For(x => x.Selected)
                   .To(vm => vm.ViewModelParameter.SortByChoicePharmacy)
                   .Mode(MvxBindingMode.OneWay)
                   .WithConversion(sortByChoiceConverter, SortByChoice.SortByRatingDescAndDistanceAsc);
            set.Bind(ratingAndNameButton)
                   .For(x => x.Selected)
                   .To(vm => vm.ViewModelParameter.SortByChoicePharmacy)
                   .Mode(MvxBindingMode.OneWay)
                   .WithConversion(sortByChoiceConverter, SortByChoice.SortByRatingDescAndProviderNameAsc);
            set.Bind(nameButton)
                   .For(x => x.Selected)
                   .To(vm => vm.ViewModelParameter.SortByChoicePharmacy)
                   .Mode(MvxBindingMode.OneWay)
                   .WithConversion(sortByChoiceConverter, SortByChoice.SortByProviderNamesAsc);

            set.Bind(_distanceNonPharmacyButton)
               .For(x => x.Selected)
               .To(vm => vm.ViewModelParameter.SortByChoice)
               .Mode(MvxBindingMode.OneWay)
               .WithConversion(sortByChoiceConverter, SortByChoice.SortByDistanceAsc);
            set.Bind(_nameNonPharmacyButton)
               .For(x => x.Selected)
               .To(vm => vm.ViewModelParameter.SortByChoice)
               .Mode(MvxBindingMode.OneWay)
               .WithConversion(sortByChoiceConverter, SortByChoice.SortByProviderNamesAsc);

            set.Bind(locationTextField).To(vm => vm.LocationAdress);
            set.Bind(doneRefineButton).To(vm => vm.GoBackCommand);
            doneRefineButton.TouchUpInside += (sender, e) => View.EndEditing(true);

            set.Bind(chooseProviderTypeButton).For("Title")
               .To(vm => vm.ViewModelParameter.SelectedProviderType.Title);

            set.Apply();

            ViewModelLocationTextChanged(this, EventArgs.Empty);

            stackViewContainer.BringSubviewToFront(locationContainer);
        }

        private void SetFonts()
        {
            headerTextView.Font = UIFont.FromName("LeagueGothic", 22.0f);
            providerTypeLabel.Font = UIFont.FromName("LeagueGothic", 18.0f);
            filterLabel.Font = UIFont.FromName("LeagueGothic", 18.0f);
            locationLabel.Font = UIFont.FromName("LeagueGothic", 18.0f);
            locationTextField.autoCompleteTextFont = UIFont.FromName("Roboto-Regular", 14.0f);
            locationTextField.Layer.ZPosition = 1;
            sortLabel.Font = UIFont.FromName("LeagueGothic", 18.0f);
            chooseProviderTypeButton.TitleLabel.Font = UIFont.FromName("Roboto-Regular", 14.0f);
            directBillLabel.Font = UIFont.FromName("Roboto-Regular", 14.0f);
            recentlyVisitedLabel.Font = UIFont.FromName("Roboto-Regular", 14.0f);
            ratingLabel.Font = UIFont.FromName("Roboto-Regular", 14.0f);
        }

        private void SetTexts()
        {
            doneRefineButton.SetTitle("DONE".tr(), UIControlState.Normal);
            headerTextView.Text = "REFINE YOUR SEARCH".tr();
            locationLabel.Text = "LOCATION".tr();
            providerTypeLabel.Text = "PROVIDER TYPE".tr();
            filterLabel.Text = "FILTER".tr();
            sortLabel.Text = "SORT".tr();
            directBillLabel.Text = "DIRECT BILL".tr();
            recentlyVisitedLabel.Text = "RECENTLY VISITED".tr();
            ratingLabel.Text = "QUALITY RATING".tr();
            distanceButton.SetTitle("Distance".tr(), UIControlState.Normal);
            nameButton.SetTitle("Name".tr(), UIControlState.Normal);
            ratingAndDistanceButton.SetTitle("Rating Distance".tr(), UIControlState.Normal);
            ratingAndNameButton.SetTitle("Rating Name".tr(), UIControlState.Normal);
            _nameNonPharmacyButton.SetTitle("Name".tr(), UIControlState.Normal);
            _distanceNonPharmacyButton.SetTitle("Distance".tr(), UIControlState.Normal);
        }

        private void SetHandlers()
        {
            _viewModel.LocationTextChanged += ViewModelLocationTextChanged;

            distanceButton.TouchUpInside += OnDistanceButtonOnTouchUpInside;
            nameButton.TouchUpInside += OnNameButtonOnTouchUpInside;
            ratingAndNameButton.TouchUpInside += OnRatingAndNameButtonOnTouchUpInside;
            ratingAndDistanceButton.TouchUpInside += OnRatingAndDistanceButtonOnTouchUpInside;
            _distanceNonPharmacyButton.TouchUpInside += OnDistanceNonPharmacyButtonOnTouchUpInside;
            _nameNonPharmacyButton.TouchUpInside += OnNameNonPharmacyButtonOnTouchUpInside;
        }

        private void OnNameNonPharmacyButtonOnTouchUpInside(object sender, EventArgs e)
        {
            _distanceNonPharmacyButton.Selected = false;
            _viewModel.ViewModelParameter.SortByChoice = SortByChoice.SortByProviderNamesAsc;
        }

        private void OnDistanceNonPharmacyButtonOnTouchUpInside(object sender, EventArgs e)
        {
            _nameNonPharmacyButton.Selected = false;
            _viewModel.ViewModelParameter.SortByChoice = SortByChoice.SortByDistanceAsc;
        }

        private void OnRatingAndDistanceButtonOnTouchUpInside(object sender, EventArgs e)
        {
            nameButton.Selected = false;
            distanceButton.Selected = false;
            ratingAndNameButton.Selected = false;
            _viewModel.ViewModelParameter.SortByChoicePharmacy = SortByChoice.SortByRatingDescAndDistanceAsc;
        }

        private void OnRatingAndNameButtonOnTouchUpInside(object sender, EventArgs e)
        {
            ratingAndDistanceButton.Selected = false;
            distanceButton.Selected = false;
            nameButton.Selected = false;
            _viewModel.ViewModelParameter.SortByChoicePharmacy = SortByChoice.SortByRatingDescAndProviderNameAsc;
        }

        private void OnNameButtonOnTouchUpInside(object sender, EventArgs e)
        {
            ratingAndDistanceButton.Selected = false;
            distanceButton.Selected = false;
            ratingAndNameButton.Selected = false;
            _viewModel.ViewModelParameter.SortByChoicePharmacy = SortByChoice.SortByProviderNamesAsc;
        }

        private void OnDistanceButtonOnTouchUpInside(object sender, EventArgs e)
        {
            ratingAndDistanceButton.Selected = false;
            nameButton.Selected = false;
            ratingAndNameButton.Selected = false;
            _viewModel.ViewModelParameter.SortByChoicePharmacy = SortByChoice.SortByDistanceAsc;
        }

        private void ViewModelLocationTextChanged(object sender, EventArgs e)
        {
            backToCurrentLocationButton.SetImage(
                string.IsNullOrEmpty(_viewModel.LocationAdress)
                    ? UIImage.FromBundle("LocationEnabled")
                    : UIImage.FromBundle("LocationDisabled"),
                UIControlState.Normal);
        }

        private void UnsetHandlers()
        {
            _viewModel.LocationTextChanged -= ViewModelLocationTextChanged;

            distanceButton.TouchUpInside -= OnDistanceButtonOnTouchUpInside;
            nameButton.TouchUpInside -= OnNameButtonOnTouchUpInside;
            ratingAndNameButton.TouchUpInside -= OnRatingAndNameButtonOnTouchUpInside;
            ratingAndDistanceButton.TouchUpInside -= OnRatingAndDistanceButtonOnTouchUpInside;
            _distanceNonPharmacyButton.TouchUpInside -= OnDistanceNonPharmacyButtonOnTouchUpInside;
            _nameNonPharmacyButton.TouchUpInside -= OnNameNonPharmacyButtonOnTouchUpInside;
        }
    }
}