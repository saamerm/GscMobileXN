using System;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.search_region)]
    public class SearchHealthProviderView : BaseFragment<SearchHealthProviderViewModel>
    {
        private View _view;
        private EditText _searchEditText;
        private Button _searchButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);

            _view = this.BindingInflate(Resource.Layout.SearchViewLayout, null);
            return _view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            Init();
        }

        private void Init()
        {
            _searchEditText = _view.FindViewById<EditText>(Resource.Id.searchEditText);
            _searchButton = _view.FindViewById<Button>(Resource.Id.searchButton);

            _searchEditText.EditorAction += SearchEditTextOnEditorAction;
            _searchEditText.FocusChange += SearchEditTextOnFocusChange;

            _searchButton.Click += SearchButtonOnClick;

            ViewModel.ClearTextBox += ViewModelOnClearTextBox;
        }

        private void ViewModelOnClearTextBox(object sender, EventArgs e)
        {
            _searchEditText.Text = null;
        }

        private void SearchButtonOnClick(object sender, EventArgs e)
        {
            HideKeyboardAndClearFocus();
            ViewModel.SearchQuery = _searchEditText.Text;
            ViewModel.PerformSearchCommand.Execute(false);
        }

        private bool _isFirstRun = true;

        private void SearchEditTextOnFocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (_isFirstRun)
            {
                _isFirstRun = false;
                return;
            }

            if (e.HasFocus)
            {
                (sender as EditText).SetCursorVisible(true);
            }
            else
            {
                HideKeyboardAndClearFocus();
            }
        }

        private void SearchEditTextOnEditorAction(object o, TextView.EditorActionEventArgs e)
        {
            if (e.ActionId == ImeAction.Done || e.ActionId == ImeAction.ImeNull)
            {
                HideKeyboardAndClearFocus();
                ViewModel.SearchQuery = (o as EditText).Text;
                ViewModel.PerformSearchCommand.Execute(false);
            }
        }

        private void HideKeyboardAndClearFocus()
        {
            _searchEditText.ClearFocus();
            _searchEditText.SetCursorVisible(false);
            var imm = (InputMethodManager)Context.GetSystemService(Android.Content.Context.InputMethodService);
            imm.HideSoftInputFromWindow(_searchEditText.WindowToken, 0);
        }
    }
}