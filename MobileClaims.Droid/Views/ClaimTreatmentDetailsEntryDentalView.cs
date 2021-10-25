using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using DatePickerDialog;
using MobileClaims.Core.Converters;
using MobileClaims.Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class ClaimTreatmentDetailsEntryDentalView : BaseFragment, Android.App.DatePickerDialog.IOnDateSetListener, View.IOnClickListener
    {
        private bool _alternateCarrierAmountInalid;
        private bool _dateInvalid;
        private bool _dentistsFeeInvalid;
        private bool _laboratoryChargeInvalid;
        private bool _procedureCodeInvalid;
        private bool _toothCodeInvalid;
        private bool _toothSurfacesInvalid;

        private ClaimTreatmentDetailsEntryDentalViewModel _model;

        public ImageButton AlternateCarrierAmountError { get; set; }
        public ImageButton LaboratoryChargeError { get; set; }
        public ImageButton DentistsFeeError { get; set; }
        public ImageButton ToothSurfacesError { get; set; }
        public ImageButton ToothCodeError { get; set; }
        public ImageButton ProcedureCodeError { get; set; }
        public ImageButton DateError { get; set; }

        public bool AlternateCarrierAmountInalid
        {
            get => _alternateCarrierAmountInalid;
            set
            {
                _alternateCarrierAmountInalid = value;
                AlternateCarrierAmountError.Visibility = _alternateCarrierAmountInalid ? ViewStates.Visible : ViewStates.Invisible;
            }
        }
        public bool LaboratoryChargeInvalid
        {
            get => _laboratoryChargeInvalid;
            set
            {
                _laboratoryChargeInvalid = value;
                LaboratoryChargeError.Visibility = _laboratoryChargeInvalid && IsLabChargeRequired ? ViewStates.Visible : ViewStates.Invisible;
            }
        }

        public bool DentistsFeeInvalid
        {
            get => _dentistsFeeInvalid;
            set
            {
                _dentistsFeeInvalid = value;
                DentistsFeeError.Visibility = _dentistsFeeInvalid ? ViewStates.Visible : ViewStates.Invisible;
            }
        }

        public bool ToothSurfacesInvalid
        {
            get => _toothSurfacesInvalid;
            set
            {
                _toothSurfacesInvalid = value;
                ToothSurfacesError.Visibility = _toothSurfacesInvalid && IsToothSurfaceRequired ? ViewStates.Visible : ViewStates.Invisible;
            }
        }
        public bool ToothCodeInvalid
        {
            get => _toothCodeInvalid;
            set
            {
                _toothCodeInvalid = value;
                ToothCodeError.Visibility = _toothCodeInvalid && IsToothCodeRequired ? ViewStates.Visible : ViewStates.Invisible;
            }
        }

        public bool ProcedureCodeInvalid
        {
            get => _procedureCodeInvalid;
            set
            {
                _procedureCodeInvalid = value;
                ProcedureCodeError.Visibility = _procedureCodeInvalid ? ViewStates.Visible : ViewStates.Invisible;
            }
        }

        public bool DateInvalid
        {
            get => _dateInvalid;
            set
            {
                _dateInvalid = value;
                DateError.Visibility = _dateInvalid ? ViewStates.Visible : ViewStates.Invisible;
            }
        }

        public bool IsLabChargeRequired { get; set; }

        public bool IsToothSurfaceRequired { get; set; }

        public bool IsToothCodeRequired { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.ClaimTreatmentDetailsEntryDentalView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ClaimTreatmentDetailsEntryDentalViewModel)ViewModel;
            _model.ClaimTreatmentEntrySuccess += HandleClaimTreatmentEntrySuccess;

            var DateOfTreatment = Activity.FindViewById<TextView>(Resource.Id.DateOfTreatment);
            DateOfTreatment.Click += (sender, args) =>
            {
                var frag = new DatePickerDialogFragment(Activity, DateTime.Now, this, true);
                frag.Show(FragmentManager, null);
            };

            DateError = Activity.FindViewById<ImageButton>(Resource.Id.DateError);
            ProcedureCodeError = Activity.FindViewById<ImageButton>(Resource.Id.ProcedureCodeError);
            ToothCodeError = Activity.FindViewById<ImageButton>(Resource.Id.ToothCodeError);
            ToothSurfacesError = Activity.FindViewById<ImageButton>(Resource.Id.ToothSurfacesError);
            DentistsFeeError = Activity.FindViewById<ImageButton>(Resource.Id.DentistsFeeError);
            LaboratoryChargeError = Activity.FindViewById<ImageButton>(Resource.Id.LaboratoryChargeError);
            AlternateCarrierAmountError = Activity.FindViewById<ImageButton>(Resource.Id.AlternateCarrierAmountError);

            DateError.SetOnClickListener(this);
            ProcedureCodeError.SetOnClickListener(this);
            ToothCodeError.SetOnClickListener(this);
            ToothSurfacesError.SetOnClickListener(this);
            DentistsFeeError.SetOnClickListener(this);
            LaboratoryChargeError.SetOnClickListener(this);
            AlternateCarrierAmountError.SetOnClickListener(this);
            
            var invertedBoolValueConverter = new BoolOppositeValueConverter();

            var set = this.CreateBindingSet<ClaimTreatmentDetailsEntryDentalView, ClaimTreatmentDetailsEntryDentalViewModel>();
            set.Bind(this).For(v => v.DateInvalid).To(vm => vm.DateValid).WithConversion(invertedBoolValueConverter);
            set.Bind(this).For(v => v.ProcedureCodeInvalid).To(vm => vm.ProcedureCodeValid).WithConversion(invertedBoolValueConverter);
            set.Bind(this).For(v => v.ToothCodeInvalid).To(vm => vm.ToothCodeValid).WithConversion(invertedBoolValueConverter);
            set.Bind(this).For(v => v.ToothSurfacesInvalid).To(vm => vm.ToothSurfacesValid).WithConversion(invertedBoolValueConverter);
            set.Bind(this).For(v => v.DentistsFeeInvalid).To(vm => vm.DentistsFeeValid).WithConversion(invertedBoolValueConverter);
            set.Bind(this).For(v => v.LaboratoryChargeInvalid).To(vm => vm.LaboratoryChargeValid).WithConversion(invertedBoolValueConverter);
            set.Bind(this).For(v => v.AlternateCarrierAmountInalid).To(vm => vm.AlternateCarrierAmountValid).WithConversion(invertedBoolValueConverter);

            set.Bind(this).For(v => v.IsToothCodeRequired).To(vm => vm.IsToothCodeRequired);
            set.Bind(this).For(v => v.IsToothSurfaceRequired).To(vm => vm.IsToothSurfaceRequired);
            set.Bind(this).For(v => v.IsLabChargeRequired).To(vm => vm.IsLabChargeRequired);
            
            set.Apply();

            DateError.Visibility = ViewStates.Invisible;
            ProcedureCodeError.Visibility = ViewStates.Invisible;
            ToothCodeError.Visibility = ViewStates.Invisible;
            ToothSurfacesError.Visibility = ViewStates.Invisible;
            DentistsFeeError.Visibility = ViewStates.Invisible;
            LaboratoryChargeError.Visibility = ViewStates.Invisible;
            AlternateCarrierAmountError.Visibility = ViewStates.Invisible;
        }
        
        public void OnClick(View v)
        {
            var errorText = "";
            switch (v.Id)
            {
                case Resource.Id.DateError:
                    errorText = _model.DateOfTreatmentErrorText;
                    break;
                case Resource.Id.ProcedureCodeError:
                    errorText = _model.ProcedureCodeErrorText;
                    break;
                case Resource.Id.ToothCodeError:
                    errorText = _model.ToothCodeErrorText;
                    break;
                case Resource.Id.ToothSurfacesError:
                    errorText = _model.ToothSurfaceErrorText;
                    break;
                case Resource.Id.DentistsFeeError:
                    errorText = _model.DentistFeesErrorText;
                    break;
                case Resource.Id.LaboratoryChargeError:
                    errorText = _model.LabChargesErrorText;
                    break;
                case Resource.Id.AlternateCarrierAmountError:
                    errorText = _model.AlternateCarrierAmountErrorText;
                    break;
            }

            var toast = Toast.MakeText(Context, errorText, ToastLength.Long);
            toast.SetGravity(Gravity.GetAbsoluteGravity(GravityFlags.Center, GravityFlags.Center), 0, 0);
            toast.Show();
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var date = new DateTime(year, monthOfYear + 1, dayOfMonth);
            _model.DateOfTreatment = date;
        }

        private void HandleClaimTreatmentEntrySuccess(object sender, EventArgs e)
        {
            Activity.RunOnUiThread(() =>
                {
                    Activity.FragmentManager.PopBackStack();
                    if (((ActivityBase) Activity).CustomFragmentsBackStack.Count > 0)
                    {
                        ((ActivityBase) Activity).CustomFragmentsBackStack.RemoveAt(
                            ((ActivityBase) Activity).CustomFragmentsBackStack.Count - 1);
                    }
                }
            );
        }
    }
}