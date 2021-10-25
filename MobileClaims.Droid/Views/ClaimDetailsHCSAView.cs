using Android.App;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using DatePickerDialog;
using MobileClaims.Core.ViewModels.HCSA;
using System;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Views;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class ClaimDetailsHCSAView : BaseFragment, IMvxView, Android.App.DatePickerDialog.IOnDateSetListener, ITextWatcher
    {
        ClaimDetailsHCSAViewModel _model;
        FrameLayout layout;
        ImageButton error_button1, error_button2, error_button3;
        SmallEditText motorVehicleAccidentDate;
        bool layoutgreyedout;
        int btntag;
        bool dialogShown;
        SmallEditText ClaimAmount;
        SmallEditText OtherPaidAmount;
        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            //var registry = Mvx.IoCProvider.Resolve<IMvxValueConverterLookup>();
            // var f = registry.Find("AmountZeroToBlankString");
            return this.BindingInflate(Resource.Layout.ClaimDetailsHCSAView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            //var registry = Mvx.IoCProvider.Resolve<IMvxValueConverterLookup>();
            //var f = registry.Find("AmountZeroToBlankString");
            base.OnViewCreated(view, savedInstanceState);
            _model = (ClaimDetailsHCSAViewModel)ViewModel;

            error_button1 = this.Activity.FindViewById<ImageButton>(Resource.Id.errorButtonClick1);
            error_button2 = this.Activity.FindViewById<ImageButton>(Resource.Id.errorButtonClick2);
            error_button3 = this.Activity.FindViewById<ImageButton>(Resource.Id.errorButtonClick3);

            ClaimAmount = view.FindViewById<SmallEditText>(Resource.Id.claim_amount);
            OtherPaidAmount = view.FindViewById<SmallEditText>(Resource.Id.other_claim_amount);
            if (Java.Util.Locale.Default.Language.Contains("fr"))
            {

                ClaimAmount.AddTextChangedListener(this);
                OtherPaidAmount.AddTextChangedListener(this);

            }
            //else
            //{
            //    ClaimAmount.KeyListener = DigitsKeyListener.GetInstance("0123456789.");
            //    OtherPaidAmount.KeyListener = DigitsKeyListener.GetInstance("0123456789.");
            //}


            _model.ClaimTreatmentEntrySuccess += HandleClaimTreatmentEntrySuccess;
            var Currentdate = DateTime.Now;
            _model.ClaimDetails.ExpenseDate = Currentdate;

            if (_model.ClaimParticipantHasBeenSelected == false)
            {
                //layout = this.Activity.FindViewById<FrameLayout>(Resource.Id.overlay_right_region);
                //TextView message = this.Activity.FindViewById<TextView>(Resource.Id.overlay_message);
                //message.Text = "";
                layout.Visibility = ViewStates.Visible;
                layoutgreyedout = true;
            }
            else
            {
                layoutgreyedout = false;
            }

            motorVehicleAccidentDate = this.Activity.FindViewById<SmallEditText>(Resource.Id.txtDateOfExpense);
            motorVehicleAccidentDate.Click += MotorVehicleAccidentDate_Click;
            if (_model != null)
                _model.PropertyChanged += _model_PropertyChanged;
        }

        private void MotorVehicleAccidentDate_Click(object sender, EventArgs e)
        {
            btntag = 0;
            var frag = new DatePickerDialogFragment(this.Activity, DateTime.Now, this);
            frag.Show(FragmentManager, null);
        }

        private void HandleClaimTreatmentEntrySuccess(object sender, EventArgs e)
        {
            this.Activity.RunOnUiThread(() =>
            {   
                if (_model.NumberOfClaimDetails == 1 && _model.IsEditing )
                {
                    int backStackCount = Activity.FragmentManager.BackStackEntryCount; 
                    String prevFragmentTag = this.Activity.FragmentManager.GetBackStackEntryAt(backStackCount - 3).Name;
                    MvxFragment prevFragment = (MvxFragment)this.Activity.FragmentManager.FindFragmentByTag(prevFragmentTag);
                    if (prevFragment != null)
                    {
                        if (prevFragment.GetType() == typeof(ClaimReviewAndEditView))
                        {
                            this.Activity.FragmentManager.PopBackStack();
                            ((ActivityBase)Activity).CustomFragmentsBackStack.RemoveAt(((ActivityBase)Activity).CustomFragmentsBackStack.Count - 1);
                        }
                    }
                }
                this.Activity.FragmentManager.PopBackStack(); 
                _model.ClaimTreatmentEntrySuccess -= HandleClaimTreatmentEntrySuccess;
                _model.PropertyChanged -= _model_PropertyChanged;
                motorVehicleAccidentDate.Click -= MotorVehicleAccidentDate_Click;
            });
        }

        void _model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ClaimParticipantHasBeenSelected")
            {
                if (_model.ClaimParticipantHasBeenSelected != null)
                {
                    if (_model.ClaimParticipantHasBeenSelected == true && !_model.CreatedFromRehydration)
                    {

                        if (layout != null)
                        {
                            layout.Visibility = ViewStates.Gone;
                            if (this.Activity != null && layoutgreyedout == true)
                            {
                                int backStackCount = Activity.FragmentManager.BackStackEntryCount;
                                String prevFragmentTag = this.Activity.FragmentManager.GetBackStackEntryAt(backStackCount - 1).Name;
                                MvxFragment prevFragment = (MvxFragment)this.Activity.FragmentManager.FindFragmentByTag(prevFragmentTag);
                                if (prevFragment.GetType() == typeof(ClaimDetailsHCSAView))
                                {
                                    this.Activity.RunOnUiThread(() =>
                                    {
                                        this.Activity.FragmentManager.PopBackStack();
                                        try
                                        {
                                            _model.ClaimTreatmentEntrySuccess -= HandleClaimTreatmentEntrySuccess;
                                            _model.PropertyChanged -= _model_PropertyChanged;
                                            motorVehicleAccidentDate.Click -= MotorVehicleAccidentDate_Click;
                                        }
                                        catch { }
                                    });
                                    layoutgreyedout = false;

                                }
                            }
                        }
                    }
                }

            }

            if (e.PropertyName == "ExpenseDateValidationMessage")
            {
                if (error_button1 != null)
                {
                    // error_button1.Tag = string.Format(this.Activity.GetString(Resource.String.error_message), _model.ExpenseDateValidationMessage);
                    error_button1.Tag = string.Format(_model.ErrorMessageLabel, _model.ExpenseDateValidationMessage);
                }

            }

            if (e.PropertyName == "ClaimAmountValidationMesssage")
            {
                if (error_button2 != null)
                {
                    // error_button2.Tag = string.Format(this.Activity.GetString(Resource.String.error_message), _model.ClaimAmountValidationMesssage);
                    error_button2.Tag = string.Format(_model.ErrorMessageLabel, _model.ClaimAmountValidationMesssage);
                }

            }

            if (e.PropertyName == "OtherPaidAmountValidationMessage")
            {
                if (error_button3 != null)
                {

                    // error_button3.Tag = string.Format(this.Activity.GetString(Resource.String.error_message), _model.OtherPaidAmountValidationMessage);
                    error_button3.Tag = string.Format(_model.ErrorMessageLabel, _model.OtherPaidAmountValidationMessage);
                }

            }


        }
        public override void OnDestroy()
        {

            if (layout != null)
                layout.Visibility = ViewStates.Gone;

            _model.PropertyChanged -= _model_PropertyChanged;
            base.OnDestroy();

        }
        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var date = new DateTime(year, monthOfYear + 1, dayOfMonth);
            _model.ClaimDetails.ExpenseDate = date;

        }
        void Error_Button_Click(String s)
        {

            if (!dialogShown)
            {

                //this.Activity.RunOnUiThread(() =>
                //{
                if (s != null)
                {
                    this.dialogShown = true;
                    Android.Content.Res.Resources res = this.Resources;
                    AlertDialog.Builder builder;
                    builder = new AlertDialog.Builder(this.Activity);
                    builder.SetTitle(string.Format(res.GetString(Resource.String.claimServiceProviderErrorMsgTitle)));
                    builder.SetMessage(s);
                    builder.SetCancelable(true);
                    builder.SetPositiveButton(string.Format(res.GetString(Resource.String.OK)), delegate
                    {
                        dialogShown = false;
                    });
                    builder.Show();
                }
                //}
                //);
            }
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            try
            {
                _model.ClaimTreatmentEntrySuccess -= HandleClaimTreatmentEntrySuccess;
                _model.PropertyChanged -= _model_PropertyChanged;
                motorVehicleAccidentDate.Click -= MotorVehicleAccidentDate_Click;
            }
            catch { }
        }

        public void AfterTextChanged(IEditable s)
        {

        }

        public void BeforeTextChanged(Java.Lang.ICharSequence s, int start, int count, int after)
        {

        }

        public void OnTextChanged(Java.Lang.ICharSequence s, int start, int before, int count)
        {
            if (ClaimAmount.HasFocus && s.ToString().Length > 0)
            {
                ClaimAmount.RemoveTextChangedListener(this);
                ClaimAmount.Text = s.ToString().Replace(".", ",");
                if (start > count && start < s.ToString().Length && (start + count) != s.ToString().Length)
                {
                    ClaimAmount.SetSelection(start + 1);
                }
                else
                    ClaimAmount.SetSelection(s.ToString().Length);
                ClaimAmount.AddTextChangedListener(this);
            }
            else if (OtherPaidAmount.HasFocus && s.ToString().Length > 0)
            {
                OtherPaidAmount.RemoveTextChangedListener(this);
                OtherPaidAmount.Text = s.ToString().Replace(".", ",");
                if (start > count && start < s.ToString().Length && (start+count)!=s.ToString().Length)

                    OtherPaidAmount.SetSelection(start);


                else
                    OtherPaidAmount.SetSelection(s.ToString().Length);
                OtherPaidAmount.AddTextChangedListener(this);
            }
        }
    }
}