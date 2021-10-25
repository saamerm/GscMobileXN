using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MobileClaims.Core.Services.Requests;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Interfaces;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.full_region, BackstackBehaviour = BackstackTypes.ADD)]
    public class RefineSearchView : BaseFragment<RefineSearchViewModel>
    {
        private View _view;
        private TextView _refineSearchTextView;
        private TextView _providerTypeTextView;
        private TextView _locationTextView;
        private TextView _searchLocationEditText;
        private TextView _filterTextView;
        private LinearLayout _starRatingLayout;
        private ToggleButton _starRatingToggleButton;
        private TextView _qualityRatingTextView;
        private RatingBar _starRatingBar;
        private ToggleButton _directBillToggleButton;
        private ToggleButton _recentlyVisitedToggleButton;
        private LinearLayout _recentlyVisitedToggleButtonLayout;
        private TextView _sortTextView;
        private RadioGroup _sortNonPharmacyRadioGroup;
        private RadioButton _distanceNonPharmacyRadioButton;
        private RadioButton _nameNonPharmacyRadioButton;
        private RadioGroup _sortRadioGroup;
        private RadioButton _distanceRadioButton;
        private RadioButton _ratingAndDistanceRadioButton;
        private RadioButton _nameRadioButton;
        private RadioButton _ratingAndNameRadioButton;
        private Dictionary<RadioButton, SortByChoice> _radioButtonToSortByChoiceDic;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);

            _view = this.BindingInflate(Resource.Layout.RefineSearchLayout, null);
            return _view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            Init();
            SetupEvents();
            LoadPreviousPreferences();
        }

        private void Init()
        {
            _radioButtonToSortByChoiceDic = new Dictionary<RadioButton, SortByChoice>();
            var typeface = Typeface.CreateFromAsset(Activity.Assets, "fonts/LeagueGothic.ttf");

            _refineSearchTextView = _view.FindViewById<TextView>(Resource.Id.refineSearchTextView);
            _refineSearchTextView.SetTypeface(typeface, TypefaceStyle.Normal);

            _locationTextView = _view.FindViewById<TextView>(Resource.Id.locationTextView);
            _locationTextView.SetTypeface(typeface, TypefaceStyle.Normal);

            _searchLocationEditText = _view.FindViewById<TextView>(Resource.Id.searchLocationEditText);

            _providerTypeTextView = _view.FindViewById<TextView>(Resource.Id.providerTypeTextView);
            _providerTypeTextView.SetTypeface(typeface, TypefaceStyle.Normal);

            _filterTextView = _view.FindViewById<TextView>(Resource.Id.filterTextView);
            _filterTextView.SetTypeface(typeface, TypefaceStyle.Normal);

            _starRatingLayout = _view.FindViewById<LinearLayout>(Resource.Id.starRatingLayout);
            _starRatingLayout.Visibility = ViewModel.ViewModelParameter.IsDisplayRating ? ViewStates.Visible : ViewStates.Gone;

            _starRatingToggleButton = _view.FindViewById<ToggleButton>(Resource.Id.starRatingToggleButton);

            _qualityRatingTextView = _view.FindViewById<TextView>(Resource.Id.qualityRatingTextView);

            _starRatingBar = _view.FindViewById<RatingBar>(Resource.Id.starRatingBar);

            _directBillToggleButton = _view.FindViewById<ToggleButton>(Resource.Id.directBillToggleButton);
            _recentlyVisitedToggleButton = _view.FindViewById<ToggleButton>(Resource.Id.recentlyVisitedToggleButton);
            _recentlyVisitedToggleButtonLayout = _view.FindViewById<LinearLayout>(Resource.Id.recentlyVisitedToggleButtonLayout);

            _sortTextView = _view.FindViewById<TextView>(Resource.Id.sortTextView);
            _sortTextView.SetTypeface(typeface, TypefaceStyle.Normal);

            _sortNonPharmacyRadioGroup = _view.FindViewById<RadioGroup>(Resource.Id.sortNonPharmacyRadioGroup);
            _distanceNonPharmacyRadioButton = _view.FindViewById<RadioButton>(Resource.Id.distanceNonPharmacyRadioButton);
            _nameNonPharmacyRadioButton = _view.FindViewById<RadioButton>(Resource.Id.nameNonPharmacyRadioButton);

            _sortRadioGroup = _view.FindViewById<RadioGroup>(Resource.Id.sortRadioGroup);
            _distanceRadioButton = _view.FindViewById<RadioButton>(Resource.Id.distanceRadioButton);
            _ratingAndDistanceRadioButton = _view.FindViewById<RadioButton>(Resource.Id.ratingAndDistanceRadioButton);
            _nameRadioButton = _view.FindViewById<RadioButton>(Resource.Id.nameRadioButton);
            _ratingAndNameRadioButton = _view.FindViewById<RadioButton>(Resource.Id.ratingAndNameRadioButton);
            
            if (ViewModel.ViewModelParameter.IsDisplayRating)
            {
                _sortNonPharmacyRadioGroup.Visibility = ViewStates.Gone;
                _sortRadioGroup.Visibility = ViewStates.Visible;
                _radioButtonToSortByChoiceDic.Add(_distanceRadioButton, SortByChoice.SortByDistanceAsc);
                _radioButtonToSortByChoiceDic.Add(_ratingAndDistanceRadioButton, SortByChoice.SortByRatingDescAndDistanceAsc);
                _radioButtonToSortByChoiceDic.Add(_nameRadioButton, SortByChoice.SortByProviderNamesAsc);
                _radioButtonToSortByChoiceDic.Add(_ratingAndNameRadioButton, SortByChoice.SortByRatingDescAndProviderNameAsc);
            }
            else
            {
                _sortRadioGroup.Visibility = ViewStates.Gone;
                _sortNonPharmacyRadioGroup.Visibility = ViewStates.Visible;
                _radioButtonToSortByChoiceDic.Add(_distanceNonPharmacyRadioButton, SortByChoice.SortByDistanceAsc);
                _radioButtonToSortByChoiceDic.Add(_nameNonPharmacyRadioButton, SortByChoice.SortByProviderNamesAsc);
            }
        }

        private void SetupEvents()
        {
            _searchLocationEditText.EditorAction += SearchLocationEditTextOnEditorAction;
            _searchLocationEditText.FocusChange += SearchLocationEditTextOnFocusChange;
            _starRatingToggleButton.CheckedChange += StarRatingToggleButtonOnCheckedChange;
            _starRatingBar.RatingBarChange += StarRatingBarOnRatingBarChange;
            _directBillToggleButton.CheckedChange += DirectBillToggleButtonOnCheckedChange;
            _recentlyVisitedToggleButton.CheckedChange += RecentlyVisitedToggleButtonOnCheckedChange;
            _sortRadioGroup.CheckedChange += SortRadioGroupOnCheckedChange;
            _sortNonPharmacyRadioGroup.CheckedChange += SortRadioGroupOnCheckedChange;
            ViewModel.OnProviderTypeChanged += ProviderTypeChanged;
        }

        void ProviderTypeChanged()
        {
            _recentlyVisitedToggleButtonLayout.Visibility = ViewModel.ViewModelParameter.SelectedProviderType.Id == (int)ProvidersId.Favourites ? ViewStates.Gone : ViewStates.Visible;
            Init();
        }

        private void LoadPreviousPreferences()
        {
            _starRatingToggleButton.Checked = ViewModel.ViewModelParameter.IsStarRating;
            _directBillToggleButton.Checked = ViewModel.ViewModelParameter.IsDirectBill;
            _recentlyVisitedToggleButton.Checked = ViewModel.ViewModelParameter.IsRecentlyVisited;

            RadioButton checkedRadioButton;
            if (ViewModel.ViewModelParameter.IsDisplayRating)
            {
                _starRatingBar.Rating = (float)ViewModel.ViewModelParameter.StarRating;

                checkedRadioButton = _radioButtonToSortByChoiceDic
                    .FirstOrDefault(x => x.Value == ViewModel.ViewModelParameter.SortByChoicePharmacy).Key;
                checkedRadioButton.Checked = true;
            }
            else
            {
                checkedRadioButton = _radioButtonToSortByChoiceDic
                    .FirstOrDefault(x => x.Value == ViewModel.ViewModelParameter.SortByChoice).Key;
                if (checkedRadioButton != null)
                {
                    checkedRadioButton.Checked = true;
                }
                else
                {
                    _radioButtonToSortByChoiceDic.FirstOrDefault().Key.Checked = true;
                }
            }
        }

        private void SearchLocationEditTextOnEditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            if (e.ActionId == ImeAction.Done || e.ActionId == ImeAction.ImeNull)
            {
                HideKeyboardAndClearFocus();
            }
        }

        private void SearchLocationEditTextOnFocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                (sender as EditText)?.SetCursorVisible(true);
            }
            else
            {
                HideKeyboardAndClearFocus();
            }
        }

        private void HideKeyboardAndClearFocus()
        {
            _searchLocationEditText.ClearFocus();
            _searchLocationEditText.SetCursorVisible(false);
            var imm = (InputMethodManager)Context.GetSystemService(Android.Content.Context.InputMethodService);
            imm.HideSoftInputFromWindow(_searchLocationEditText.WindowToken, 0);
        }

        private void DirectBillToggleButtonOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            ViewModel.ViewModelParameter.IsDirectBill = e.IsChecked;
        }

        private void RecentlyVisitedToggleButtonOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            ViewModel.ViewModelParameter.IsRecentlyVisited = e.IsChecked;
        }

        private void StarRatingBarOnRatingBarChange(object sender, RatingBar.RatingBarChangeEventArgs e)
        {
            ViewModel.ViewModelParameter.StarRating = e.Rating;
        }

        private void StarRatingToggleButtonOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
            {
                _qualityRatingTextView.Visibility = ViewStates.Gone;
                _starRatingBar.Visibility = ViewStates.Visible;
                _starRatingBar.Rating = (float)ViewModel.ViewModelParameter.StarRating;
            }
            else
            {
                _qualityRatingTextView.Visibility = ViewStates.Visible;
                _starRatingBar.Visibility = ViewStates.Gone;
            }

            ViewModel.ViewModelParameter.IsStarRating = e.IsChecked;
        }

        private void SortRadioGroupOnCheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            var isRadioButtonVisible = _radioButtonToSortByChoiceDic.Select(x => x.Key.Id).Contains(e.CheckedId);
            if (!isRadioButtonVisible)
            {
                return;
            }
            UncheckAllButThis(e.CheckedId);
        }

        private void UncheckAllButThis(int eCheckedId)
        {
            foreach (var radioButton in _radioButtonToSortByChoiceDic)
            {
                if (radioButton.Key.Id == eCheckedId)
                {
                    if (ViewModel.ViewModelParameter.IsDisplayRating)
                    {
                        ViewModel.ViewModelParameter.SortByChoicePharmacy = radioButton.Value;
                    }
                    else
                    {
                        ViewModel.ViewModelParameter.SortByChoice = radioButton.Value;
                    }
                    radioButton.Key.Checked = true;
                }
                else
                {
                    radioButton.Key.Checked = false;
                }
            }
        }

        public void OnBackPressed()
        {
            BackPressHandled = true;
            ViewModel.GoBackCommand.Execute(null);
        }
        public bool BackPressHandled { get; set; }
    }
}