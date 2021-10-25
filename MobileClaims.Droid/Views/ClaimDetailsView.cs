using Android.App;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using DatePickerDialog;
using MobileClaims.Core.ViewModels;
using System;
using MobileClaims.Droid.Helpers;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class ClaimDetailsView : BaseFragment, Android.App.DatePickerDialog.IOnDateSetListener
    {
        private ClaimDetailsViewModel _model;
        private bool _dialogShown;
        private bool _firsttimeDateReferal;
        private int _btntag;
        private bool _dateReferalVisible;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.ClaimDetailsView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _model = (ClaimDetailsViewModel)ViewModel;
            var atv = view.FindViewById<NunitoTextView>(Resource.Id.claimOtherBenefitsView_CoverageWithGSCId);
            atv.Text = Resources.FormatterBrandKeywords(Resource.String.claimOtherBenefitsView_CoverageWithGSC, new string[] { Resources.GetString(Resource.String.gsc) });

            var atvUnpaid = view.FindViewById<NunitoTextView>(Resource.Id.claimOtherBenefitsView_UnpaidGSCPlanId);
            atvUnpaid.Text = Resources.FormatterBrandKeywords(Resource.String.claimOtherBenefitsView_UnpaidGSCPlan, new string[] { Resources.GetString(Resource.String.gsc) });

            var atvNumber = view.FindViewById<NunitoTextView>(Resource.Id.claimOtherBenefitsView_OtherGSCNumberId);
            atvNumber.Text = Resources.FormatterBrandKeywords(Resource.String.claimOtherBenefitsView_OtherGSCNumber, new string[] { Resources.GetString(Resource.String.gsc) });

            var errorButtonClick = view.FindViewById<ImageButton>(Resource.Id.errorButtonClick1);
            errorButtonClick.Tag = Resources.FormatterBrandKeywords(Resource.String.claimSpecifyGSCNumberError, new string[] { Resources.GetString(Resource.String.gsc) });

            var errorButtonClick2 = view.FindViewById<ImageButton>(Resource.Id.errorButtonClick2);
            errorButtonClick2.Tag = Resources.FormatterBrandKeywords(Resource.String.claimSpecifyGSCNumberErrorInvalid, new string[] { Resources.GetString(Resource.String.gsc) });

            var motorVehicleAccidentDate = Activity.FindViewById<TextView>(Resource.Id.motorVehicleAccidentDate);
            motorVehicleAccidentDate.Click += (sender, args) =>
            {
                _btntag = 0;
                var frag = new DatePickerDialogFragment(Activity, DateTime.Now, this);
                frag.Show(FragmentManager, null);
            };

            var workRelatedInjuryDate = Activity.FindViewById<TextView>(Resource.Id.workRelatedInjuryDate);
            workRelatedInjuryDate.Click += (sender, args) =>
            {
                _btntag = 1;
                var frag = new DatePickerDialogFragment(Activity, DateTime.Now, this);
                frag.Show(FragmentManager, null);
            };

            var claimreferraldate = Activity.FindViewById<Button>(Resource.Id.claimreferraldate);
            claimreferraldate.Click += (sender, args) =>
            {
                _btntag = 2;
                var frag = new DatePickerDialogFragment(Activity, DateTime.Now, this);
                frag.Show(FragmentManager, null);
            };

            var referralErrorBtn = Activity.FindViewById<ImageButton>(Resource.Id.referralErrorBtn);
            referralErrorBtn.Click += (sender, args) =>
            {

                if (_model.InvalidDateOfReferral && _model.DateOfReferralError)
                {

                    Error_Button_Click(Resources.GetString(Resource.String.claimSpecifyPrescriptionNoFutureError));
                }
                if (!_model.InvalidDateOfReferral && _model.DateOfReferralError)
                {

                    Error_Button_Click(Resources.GetString(Resource.String.claimSpecifyPrescriptionTooOldError));
                }
            };

            //dateReferalVisible
            var claimreferraldateclear = Activity.FindViewById<Button>(Resource.Id.claimreferraldateclear);
            claimreferraldateclear.Click += (sender, args) =>
            {
                _model.ClaimMedicalItemViewModel.DateOfReferral = DateTime.MinValue;
                claimreferraldate.SetTextColor(ContextCompat.GetColorStateList(Context, Resource.Color.off_white));
                claimreferraldate.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Android.Resource.Drawable.IcMenuToday, 0);
                _dateReferalVisible = false;
                _firsttimeDateReferal = false;
                claimreferraldateclear.Visibility = ViewStates.Gone;
            };

            if (_dateReferalVisible)
            {
                claimreferraldate.SetCompoundDrawablesWithIntrinsicBounds(0, 0, 0, 0);
                claimreferraldateclear.Visibility = ViewStates.Visible;
            }
            else
            {
                claimreferraldateclear.Visibility = ViewStates.Gone;
            }

            if (_model.Participant != null)
            {
                var claimDetailsTitle = Activity.FindViewById<TextView>(Resource.Id.claim_details_title);
                claimDetailsTitle.Text = string.Format(Resources.GetString(Resource.String.claimDetailsTitle), _model.Participant.FullName);

                var claim_service_provider = Activity.FindViewById<TextView>(Resource.Id.claim_service_provider);
                claim_service_provider.Text = string.Format(Resources.GetString(Resource.String.claimDetailsServiceProvider), (_model.ClaimSubmissionType.Name).ToLower());

                var claimTreatmentDetailsLabel = Activity.FindViewById<Button>(Resource.Id.claimTreatmentDetailsLabel);
                claimTreatmentDetailsLabel.Text = string.Format(Resources.GetString(Resource.String.claimTreatmentDetailsLabel), _model.Participant.FullName);
            }

            if (!_firsttimeDateReferal)
            {
                claimreferraldate.SetTextColor(ContextCompat.GetColorStateList(Context, Resource.Color.off_white));
            }

            Switch AccidentSwitch = view.FindViewById<Switch>(Resource.Id.claimResultOfAnAccident_toggle);
            Switch MotorVehicleAccidentSwitch = view.FindViewById<Switch>(Resource.Id.claimMotorVehicleView_MotorVehicleAccident_toggle);
            Switch WorkAccidentSwitch = view.FindViewById<Switch>(Resource.Id.claimWorkInjuryView_WorkRelatedInjury_toggle);
            Switch OtherAccidentSwitch = view.FindViewById<Switch>(Resource.Id.claim_Other_type_of_accident_toggle);


            AccidentSwitch.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs e) {
                if (!e.IsChecked)
                {
                    WorkAccidentSwitch.Checked = false;
                    OtherAccidentSwitch.Checked = false;
                    MotorVehicleAccidentSwitch.Checked = false;
                }
            };

            MotorVehicleAccidentSwitch.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs e) {
                if (e.IsChecked)
                {
                    WorkAccidentSwitch.Checked = false;
                    OtherAccidentSwitch.Checked = false;
                }
            };

            WorkAccidentSwitch.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs e) {
                if (e.IsChecked)
                {
                    MotorVehicleAccidentSwitch.Checked = false;
                    OtherAccidentSwitch.Checked = false;
                }
            };

            OtherAccidentSwitch.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs e) {
                if (e.IsChecked)
                {
                    MotorVehicleAccidentSwitch.Checked = false;
                    WorkAccidentSwitch.Checked = false;
                }
            };
        }

        private void CheckIfOnlyOneSwitchIsOn()
        {

        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var date = new DateTime(year, monthOfYear + 1, dayOfMonth);
            switch (_btntag)
            {
                case 0:
                   {
                        _model.ClaimMotorVehicleViewModel.DateOfMotorVehicleAccident = date;
                        break;
                    }
                case 1:
                    {
                        _model.ClaimWorkInjuryViewModel.DateOfWorkRelatedInjury = date;
                        break;
                    }
                case 2:
                    {
                        _model.ClaimMedicalItemViewModel.DateOfReferral = date;
                        var referralErrorBtn = Activity.FindViewById<ImageButton>(Resource.Id.referralErrorBtn);
                        if (!_firsttimeDateReferal)
                        {
                            var claimreferraldate = Activity.FindViewById<Button>(Resource.Id.claimreferraldate);
                            var claimreferraldateclear = Activity.FindViewById<Button>(Resource.Id.claimreferraldateclear);
                            _firsttimeDateReferal = true;
                            claimreferraldate.SetTextColor(ContextCompat.GetColorStateList(Context, Resource.Color.dark_grey));
                            claimreferraldate.SetCompoundDrawablesWithIntrinsicBounds(0, 0, 0, 0);
                            _dateReferalVisible = true;
                            claimreferraldateclear.Visibility = ViewStates.Visible;
                        }

                        break;
                    }
            }
        }

        private void Error_Button_Click(String s)
        {
            if (!_dialogShown)
            {
                if (s != null)
                {
                    _dialogShown = true;
                    var res = Resources;
                    var builder = new AlertDialog.Builder(Activity);
                    builder.SetTitle(string.Format(res.GetString(Resource.String.claimServiceProviderErrorMsgTitle)));
                    builder.SetMessage(s);
                    builder.SetCancelable(true);
                    builder.SetPositiveButton(string.Format(res.GetString(Resource.String.OK)), delegate
                    {
                        _dialogShown = false;
                    });
                    builder.Show();
                }
            }
        }
    }
}