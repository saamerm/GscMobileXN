using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using DatePickerDialog;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class EligibilityCheckEyeView : BaseFragment, Android.App.DatePickerDialog.IOnDateSetListener
    {
        private EligibilityCheckEyeViewModel _model;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.EligibilityCheckEyeView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (EligibilityCheckEyeViewModel)ViewModel;
            _model.DateOfPurchaseOrService = DateTime.Now;
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

            TextView DateOfPurchaseOrService = Activity.FindViewById<TextView>(Resource.Id.DateOfPurchaseOrService);
            DateOfPurchaseOrService.Click += (sender, args) =>
            {
                var frag = new DatePickerDialogFragment(Activity, DateTime.Now, this);
                frag.Show(FragmentManager, null);

            };

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

            NunitoTextView checkeligibilityTotalChargeLenses = (NunitoTextView)view.FindViewById(Resource.Id.checkeligibilityTotalChargeLenses);
            NunitoTextView checkeligibilityTotalChargeGlasses = (NunitoTextView)view.FindViewById(Resource.Id.checkeligibilityTotalChargeGlasses);
            NunitoTextView checkeligibilityTotalChargeExamination = (NunitoTextView)view.FindViewById(Resource.Id.checkeligibilityTotalChargeExamination);

            if (_model.EligibilityCheckType.ID == "CONTACTS")
            {
                checkeligibilityTotalChargeLenses.Visibility = ViewStates.Visible;
                checkeligibilityTotalChargeGlasses.Visibility = ViewStates.Gone;
                checkeligibilityTotalChargeExamination.Visibility = ViewStates.Gone;
            }
            else if (_model.EligibilityCheckType.ID == "GLASSES")
            {
                checkeligibilityTotalChargeLenses.Visibility = ViewStates.Gone;
                checkeligibilityTotalChargeGlasses.Visibility = ViewStates.Visible;
                checkeligibilityTotalChargeExamination.Visibility = ViewStates.Gone;
            }
            else if (_model.EligibilityCheckType.ID == "EYEEXAM")
            {
                checkeligibilityTotalChargeLenses.Visibility = ViewStates.Gone;
                checkeligibilityTotalChargeGlasses.Visibility = ViewStates.Gone;
                checkeligibilityTotalChargeExamination.Visibility = ViewStates.Visible;
            }

            NunitoTextView checkeligibilityNotes1 = view.FindViewById<NunitoTextView>(Resource.Id.checkeligibilityNotes1);
            checkeligibilityNotes1.Text = Resources.FormatterBrandKeywords(Resource.String.checkeligibilityNotes1, new string[] { Resources.GetString(Resource.String.greenshieldcanada) });

        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var date = new DateTime(year, monthOfYear + 1, dayOfMonth);
            _model.DateOfPurchaseOrService = date;

        }
    }
}