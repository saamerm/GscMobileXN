using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;


using MobileClaims.Core.ViewModels;
using System;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.Views;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class DrugLookupByNameView : BaseFragment
    {
        private DrugLookupByNameViewModel _model;
        private bool dialogShown;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            return this.BindingInflate(Resource.Layout.DrugLookupByNameFragment, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _model = (DrugLookupByNameViewModel)ViewModel;

            var list = Activity.FindViewById(Resource.Id.participant_list) as MvxListView;
            list.Adapter = new SingleSelectionMvxAdapter(Activity, (IMvxAndroidBindingContext)BindingContext);

            if (list.Count > 0)
            {
                Utility.setListViewHeightBasedOnChildren(list);
                list.SetItemChecked(0, true);
                _model.SelectedParticipant = _model.Participants[0];
            }

            ProgressDialog progressDialog = null;
            _model.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Searching")
                {
                    if (_model.Searching)
                    {
                        Activity.RunOnUiThread(() =>
                        {
                            progressDialog = ProgressDialog.Show(Activity, "", Activity.Resources.GetString(Resource.String.searchingIndicator), true);
                        });
                    }
                    else
                    {
                        Activity.RunOnUiThread(() =>
                        {
                            if (progressDialog != null && progressDialog.IsShowing)
                                progressDialog.Dismiss();
                        });
                    }
                }
                else if (e.PropertyName == "DrugName" || e.PropertyName == "SelectedParticipant")
                {
                    var theButton = View.FindViewById(Resource.Id.button_search_name);
                    if (_model.SelectedParticipant != null && !string.IsNullOrEmpty(_model.DrugName))
                    {
                        theButton.Enabled = true;
                    }
                    else
                    {
                        theButton.Enabled = false;
                    }
                }
                else if (e.PropertyName == "ErrorInSearch" && _model.ErrorInSearch)
                {
                    ShowSearchError();
                }

            };
            _model.OnInvalidDrugName += ShowInvalidDrugName;
            _model.OnMissingDrugName += ShowMissingDrugName;

            EditText hideKeyboardButton = View.FindViewById<EditText>(Resource.Id.edit_text_drug_name);
            hideKeyboardButton.KeyPress += (sender, e) =>
            {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    InputMethodManager manager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
                    manager.HideSoftInputFromWindow(hideKeyboardButton.WindowToken, 0);
                    e.Handled = true;
                    //_model.LoginForDroidCommand.Execute(null);
                }
            };

            Button button_search_name = View.FindViewById<Button>(Resource.Id.button_search_name);
            button_search_name.Click += (sender, e) =>
            {
                InputMethodManager manager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
                manager.HideSoftInputFromWindow(hideKeyboardButton.WindowToken, 0);
            };

        }

        void ShowSearchError()
        {
            if (!dialogShown)
            {

                Activity.RunOnUiThread(() =>
                    {
                        dialogShown = true;
                        Resources res = Resources;
                        AlertDialog.Builder builder;
                        builder = new AlertDialog.Builder(Activity);
                        builder.SetTitle(string.Format(res.GetString(Resource.String.noresults)));
                        builder.SetMessage(string.Format(res.GetString(Resource.String.noresultsdetails)));
                        builder.SetCancelable(false);
                        builder.SetPositiveButton("OK", delegate { dialogShown = false; });
                        builder.Show();
                    }
                );
            }
        }

        void ShowInvalidDrugName(object s, EventArgs e)
        {
            if (!dialogShown)
            {

                Activity.RunOnUiThread(() =>
                    {
                        dialogShown = true;
                        Resources res = Resources;
                        AlertDialog.Builder builder;
                        builder = new AlertDialog.Builder(Activity);
                        builder.SetTitle(string.Format(res.GetString(Resource.String.noresults)));
                        builder.SetMessage(string.Format(res.GetString(Resource.String.nameLengthError)));
                        builder.SetCancelable(false);
                        builder.SetPositiveButton("OK", delegate { dialogShown = false; });
                        builder.Show();
                    }
                );
            }
        }

        void ShowMissingDrugName(object s, EventArgs e)
        {
            if (!dialogShown)
            {
                Activity.RunOnUiThread(() =>
                    {
                        dialogShown = true;
                        Resources res = Resources;
                        AlertDialog.Builder builder;
                        builder = new AlertDialog.Builder(Activity);
                        builder.SetTitle(string.Format(res.GetString(Resource.String.noresults)));
                        builder.SetMessage(string.Format(res.GetString(Resource.String.nameError)));
                        builder.SetCancelable(false);
                        builder.SetPositiveButton("OK", delegate { dialogShown = false; });
                        builder.Show();
                    }
                );
            }
        }
    }
}