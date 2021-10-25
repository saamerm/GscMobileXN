using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using System;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.Views;
using System.Collections.Generic;
using MobileClaims.Core.Entities;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class ClaimServiceProvidersView : BaseFragment<ClaimServiceProvidersViewModel>
    {
        private ClaimServiceProvidersViewModel _model;
        private bool dialogShown;
        LinearLayout _linearListLayout;

        private List<ServiceProvider> _serviceProviders;
        public List<ServiceProvider> ServiceProviders
        {
            get => _serviceProviders;
            set
            {
                _serviceProviders = value;

                if (_linearListLayout != null)
                    _linearListLayout.Visibility = (_serviceProviders == null || _serviceProviders?.Count == 0) ? ViewStates.Gone : ViewStates.Visible;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            view = this.BindingInflate(Resource.Layout.ClaimServiceProvidersView, null);
            var set = SetBinding<ClaimServiceProvidersView, ClaimServiceProvidersViewModel>();
            set.Bind().For(fr => fr.ServiceProviders).To(vm => vm.ServiceProviders);
            set.Apply();
            return view;
        }        

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ClaimServiceProvidersViewModel)ViewModel;
            var providerlist = Activity.FindViewById<MvxListView>(Resource.Id.claim_type_list);
            _linearListLayout = view.FindViewById<LinearLayout>(Resource.Id.linearListLayout);

            if (ServiceProviders == null || ServiceProviders?.Count == 0)
                _linearListLayout.Visibility = ViewStates.Gone;
            else
                _linearListLayout.Visibility = ViewStates.Visible;

            try
            {
                ProgressDialog progressDialog = null;
                if (_model.Busy)
                {
                    Activity.RunOnUiThread(() =>
                    {
                        progressDialog = ProgressDialog.Show(Activity, "", Resources.GetString(Resource.String.loadingIndicator), true);
                    });
                }
                if (_model.Searching)
                {
                    Activity.RunOnUiThread(() =>
                    {
                        progressDialog = ProgressDialog.Show(Activity, "", Resources.GetString(Resource.String.loadingIndicator), true);
                    });
                }

                _model.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "Busy")
                    {
                        if (_model.Busy)
                        {
                            Activity.RunOnUiThread(() =>
                            {
                                if (progressDialog == null)
                                {
                                    progressDialog = ProgressDialog.Show(Activity, "", Resources.GetString(Resource.String.loadingIndicator), true);
                                }
                                else
                                {
                                    if (!progressDialog.IsShowing)
                                    {
                                        progressDialog.Show();
                                    }
                                }
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

                };

                _model.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "Searching")
                    {
                        if (_model.Searching)
                        {
                            Activity.RunOnUiThread(() =>
                            {
                                if (progressDialog == null)
                                {
                                    progressDialog = ProgressDialog.Show(Activity, "", Resources.GetString(Resource.String.loadingIndicator), true);
                                }
                                else
                                {
                                    if (!progressDialog.IsShowing)
                                    {
                                        progressDialog.Show();
                                    }
                                }
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
                };

                _model.OnNoResults += ShowNoSearchResultError;

                var selectedServiceProvider = _model.SelectedServiceProvider;
                if (selectedServiceProvider != null)
                {
                    int index = _model.ServiceProviders.FindIndex(x => x.ID == selectedServiceProvider.ID);
                    providerlist.SetItemChecked(index, true);
                }

                var lookupProviderByInintialValue = Activity.FindViewById<EditText>(Resource.Id.lookupProviderByInintialValue);
                var lookupProviderByLastNameValue = Activity.FindViewById<EditText>(Resource.Id.lookupProviderByLastNameValue);
                var lookupProviderByPhoneValue = Activity.FindViewById<EditText>(Resource.Id.lookupProviderByPhoneValue);

                var searchByNameBtn = View.FindViewById<Button>(Resource.Id.searchByNameBtn);
                searchByNameBtn.Click += (newSender, newE) =>
                {
                    lookupProviderByInintialValue.ClearFocus();
                    lookupProviderByLastNameValue.ClearFocus();
                    lookupProviderByPhoneValue.ClearFocus();
                    _model.SearchByNameCommand.Execute(null);
                };

                var searchByPhoneBtn = View.FindViewById<Button>(Resource.Id.searchByPhoneBtn);
                searchByPhoneBtn.Click += (newSender, newE) =>
                {
                    lookupProviderByInintialValue.ClearFocus();
                    lookupProviderByLastNameValue.ClearFocus();
                    lookupProviderByPhoneValue.ClearFocus();
                    _model.SearchByPhoneCommand.Execute(null);
                };

                var hideKeyboardButton = View.FindViewById<EditText>(Resource.Id.lookupProviderByLastNameValue);
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

                var hideKeyboardButton1 = View.FindViewById<EditText>(Resource.Id.lookupProviderByPhoneValue);
                hideKeyboardButton1.KeyPress += (sender, e) =>
                {
                    e.Handled = false;
                    if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                    {
                        InputMethodManager manager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
                        manager.HideSoftInputFromWindow(hideKeyboardButton1.WindowToken, 0);
                        e.Handled = true;
                        //_model.LoginForDroidCommand.Execute(null);
                    }
                };
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }
        }

        private void ShowNoSearchResultError(object s, EventArgs e)
        {
            if (!dialogShown)
            {
                Activity.RunOnUiThread(() =>
                    {
                        dialogShown = true;
                        var builder = new AlertDialog.Builder(Activity);
                        builder.SetTitle(string.Format(Resources.GetString(Resource.String.claimNoProviderErrorTitle)));
                        builder.SetMessage(string.Format(Resources.GetString(Resource.String.claimNoProviderErrorDesc)));
                        builder.SetCancelable(false);
                        builder.SetPositiveButton("OK", delegate { dialogShown = false; });
                        builder.Show();
                    }
                );
            }
        }
    }
}