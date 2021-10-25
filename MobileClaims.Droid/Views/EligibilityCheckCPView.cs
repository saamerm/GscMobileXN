using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using DatePickerDialog;
using MobileClaims.Core.ViewModels;
using System;
using Android.InputMethodServices;
using Android.Views.InputMethods;
using MobileClaims.Droid.Helpers;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class EligibilityCheckCPView : BaseFragment, Android.App.DatePickerDialog.IOnDateSetListener
    {
        private EligibilityCheckCPViewModel _model;
        private View _view;
        private Button _claimSaveClaimBtn;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            _view = this.BindingInflate(Resource.Layout.EligibilityCheckCPView, null);
            return _view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (EligibilityCheckCPViewModel)ViewModel;

            _claimSaveClaimBtn = _view.FindViewById<Button>(Resource.Id.claimSaveClaimBtn);
            _claimSaveClaimBtn.Click += ClaimSaveClaimBtnOnClick;

            _model.DateOfTreatment = DateTime.Now;
            ProgressDialog progressDialog = null;
            if (_model.Busy)
            {
                Activity.RunOnUiThread(() =>
                {
                    progressDialog = ProgressDialog.Show(Activity, "", Resources.GetString(Resource.String.loadingIndicator), true);
                });
            }

            TextView checkeligibilitySubtitle = Activity.FindViewById<TextView>(Resource.Id.checkeligibilitySubtitle);
            if (_model.SelectedParticipant != null)
            {
                checkeligibilitySubtitle.Text = string.Format(Resources.GetString(Resource.String.checkeligibilitySubtitle), (_model.EligibilityCheckType.Name).ToLower(), _model.SelectedParticipant.FullName);
            }
            else
            {
                checkeligibilitySubtitle.Text = string.Format(Resources.GetString(Resource.String.checkeligibilitySubtitleSec), (_model.EligibilityCheckType.Name).ToLower());
            }

            TextView DateOfTreatment = Activity.FindViewById<TextView>(Resource.Id.DateOfTreatment);
            DateOfTreatment.Click += (sender, args) =>
            {
                var frag = new DatePickerDialogFragment(Activity, DateTime.Now, this);
                frag.Show(FragmentManager, null);

            };


            NunitoTextView checkeligibilityNotes1 = view.FindViewById<NunitoTextView>(Resource.Id.checkeligibilityNotes1);
            checkeligibilityNotes1.Text = Resources.FormatterBrandKeywords(Resource.String.checkeligibilityNotes1, new string[] { Resources.GetString(Resource.String.greenshieldcanada) });

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
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var date = new DateTime(year, monthOfYear + 1, dayOfMonth);
            _model.DateOfTreatment = date;
        }

        private void ClaimSaveClaimBtnOnClick(object sender, EventArgs e)
        {
            var imm = (InputMethodManager)Activity.GetSystemService(InputMethodService.InputMethodService);
            imm.HideSoftInputFromWindow(Activity?.CurrentFocus?.WindowToken, 0);
        }
    }
}