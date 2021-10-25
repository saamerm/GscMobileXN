using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Widget;
using DatePickerDialog;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using System;
using System.ComponentModel;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using Object = Java.Lang.Object;
using Orientation = Android.Widget.Orientation;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.full_region, true, BackstackTypes.ADD)]
    public class ClaimsHistoryDisplayByView : BaseFragment, Android.App.DatePickerDialog.IOnDateSetListener, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private Button btn1, btn2, btn3;
        private LinearLayout layout_btn3;
        private SmallEditText Date_start, Date_end;
        private LinearLayout layout_btn1, layout_btn2;
        private ImageButton errorbutton1, errorbutton2, errorbutton3;
        private FrameLayout MainNavLayout, startDateFameLayout, endDateFameLayout;
        private LinearLayout StartDateLinearLayout, startDateTextParent, endDateLinearLayout, endDateTextParent;
        private NunitoTextView start_nunito, end_nunito;
        private View rootView;
        private bool dialogShown;

        public static ClaimsHistoryDisplayByViewModel _model;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            return this.BindingInflate(Resource.Layout.ClaimHistoryDisplayByYear, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ClaimsHistoryDisplayByViewModel)ViewModel;

            errorbutton1 = Activity.FindViewById<ImageButton>(Resource.Id.errorButtonClick1);
            errorbutton2 = Activity.FindViewById<ImageButton>(Resource.Id.errorButtonClick2);
            errorbutton3 = Activity.FindViewById<ImageButton>(Resource.Id.errorButtonClick4);

            layout_btn1 = Activity.FindViewById<LinearLayout>(Resource.Id.frag_text);
            layout_btn2 = Activity.FindViewById<LinearLayout>(Resource.Id.frag_date);
            layout_btn3 = Activity.FindViewById<LinearLayout>(Resource.Id.frag_spinner);
            btn1 = Activity.FindViewById<Button>(Resource.Id.bn1_year_to_date);
            btn2 = Activity.FindViewById<Button>(Resource.Id.bn2_range);
            btn3 = Activity.FindViewById<Button>(Resource.Id.bn3_year);
            start_nunito = Activity.FindViewById<NunitoTextView>(Resource.Id.nunito_start_text);
            StartDateLinearLayout = Activity.FindViewById<LinearLayout>(Resource.Id.start_date_parent);
            startDateFameLayout = Activity.FindViewById<FrameLayout>(Resource.Id.start_frame_layout);
            startDateTextParent = Activity.FindViewById<LinearLayout>(Resource.Id.start_text_parent);

            end_nunito = Activity.FindViewById<NunitoTextView>(Resource.Id.nunito_end_text);
            endDateLinearLayout = Activity.FindViewById<LinearLayout>(Resource.Id.end_date_parent);
            endDateFameLayout = Activity.FindViewById<FrameLayout>(Resource.Id.end_frame_layout);
            endDateTextParent = Activity.FindViewById<LinearLayout>(Resource.Id.end_text_parent);

            btn1.Click -= HandleBtn1Click;
            btn2.Click -= HandleBtn2Click;
            btn3.Click -= HandleBtn3Click;

            btn1.Click += HandleBtn1Click;
            btn2.Click += HandleBtn2Click;
            btn3.Click += HandleBtn3Click;

            var selectedDisplayBy = _model.SelectedDisplayBy;
            if (selectedDisplayBy.Key == "YTD")
                btn1.PerformClick();
            else if (selectedDisplayBy.Key == "DR")
                btn2.PerformClick();

            if (_model.DisplayBy.Count > 2 && selectedDisplayBy.Key == "YR")
                btn3.PerformClick();
            else if (_model.DisplayBy.Count == 2)
                btn3.Visibility = ViewStates.Gone;


            Date_start = Activity.FindViewById<SmallEditText>(Resource.Id.txtDate_stat);
            Date_start.Click += (sender, args) =>
            {
                var frag = new DatePickerDialogFragment(Activity, DateTime.Now, this);
                frag.Show(FragmentManager, null);

            };

            Date_end = Activity.FindViewById<SmallEditText>(Resource.Id.txtDate_end);
            Date_end.Click += (sender, args) =>
            {
                var frag = new DatePickerDialogFragment(Activity, DateTime.Now, new DateSelectedListener(Activity));
                frag.Show(FragmentManager, null);

            };
            _model.DisplayByEntryComplete -= HandleDisplayByEntryComplete;
            _model.DisplayByEntryComplete += HandleDisplayByEntryComplete;

            _model.PropertyChanged -= HandlePropertyChanged;
            _model.PropertyChanged += HandlePropertyChanged;
            rootView = view;

            rootView.ViewTreeObserver.AddOnGlobalLayoutListener(this);
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EndDateValidationMessage")
            {
                if (errorbutton1 != null)
                    errorbutton1.Tag = string.Format("{0}", _model.EndDateValidationMessage);
            }

            if (e.PropertyName == "StartDateValidationMessage")
            {
                if (errorbutton2 != null)
                    errorbutton2.Tag = string.Format("{0}", _model.StartDateValidationMessage);
            }
            if (e.PropertyName == "YearValidationMessage")
            {
                if (errorbutton3 != null)
                    errorbutton3.Tag = string.Format("{0}", _model.YearValidationMessage);
            }

        }

        private void HandleDisplayByEntryComplete(object sender, EventArgs e)
        {
            Activity.RunOnUiThread(() =>
                {
                    Activity.FragmentManager.PopBackStack();
                    ((ClaimsHistoryResultsCountView)Activity).CustomFragmentsBackStack.RemoveAt(((ClaimsHistoryResultsCountView)Activity).CustomFragmentsBackStack.Count - 1);
                });
        }

        public class DateSelectedListener : Object, Android.App.DatePickerDialog.IOnDateSetListener
        {
            private Context _context;

            public DateSelectedListener(Context context)
            {
                _context = context;

            }

            public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
            {
                var date = new DateTime(year, monthOfYear + 1, dayOfMonth);
                _model.SelectedEndDate = date;

            }


        }
        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var date = new DateTime(year, monthOfYear + 1, dayOfMonth);
            _model.SelectedStartDate = date;

        }

        public void OnGlobalLayout()
        {
            if (Activity.Resources.GetBoolean(Resource.Boolean.isTablet))
            {
                int margin = (int)(20 * Resources.DisplayMetrics.Density);
                var surfaceOrientation = Activity.WindowManager.DefaultDisplay.Rotation;
                if (surfaceOrientation == SurfaceOrientation.Rotation0 || surfaceOrientation == SurfaceOrientation.Rotation180)
                {
                    //  landscape 
                    StartDateLinearLayout.Orientation = Orientation.Horizontal;
                    StartDateLinearLayout.SetGravity(GravityFlags.Center);
                    LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, (int)(Activity.Resources.GetDimension(Resource.Dimension.list_item_height_drugConfirm) + margin));
                    lp.SetMargins(margin, 0, margin, margin);
                    StartDateLinearLayout.LayoutParameters = lp;

                    startDateTextParent.LayoutParameters = startDateTextParent.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, 1.1f);
                    start_nunito.LayoutParameters = start_nunito.LayoutParameters = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1.0f);
                    Date_start.SetHeight((int)Activity.Resources.GetDimension(Resource.Dimension.list_item_height_drugConfirm));
                    LinearLayout.LayoutParams frame_start = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1.0f);
                    frame_start.SetMargins(10, 0, 0, 0);
                    startDateFameLayout.LayoutParameters = frame_start;

                    endDateLinearLayout.Orientation = Orientation.Horizontal;
                    endDateLinearLayout.SetGravity(GravityFlags.Center);
                    endDateLinearLayout.LayoutParameters = lp;

                    endDateTextParent.LayoutParameters = endDateTextParent.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, 1.1f);
                    end_nunito.LayoutParameters = end_nunito.LayoutParameters = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1.0f);
                    Date_end.SetHeight((int)Activity.Resources.GetDimension(Resource.Dimension.list_item_height_drugConfirm));
                    LinearLayout.LayoutParams frame_end = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1.0f);
                    frame_end.SetMargins(10, 0, 0, 0);
                    endDateFameLayout.LayoutParameters = frame_end;
                }
                else
                {
                    //potrait
                    StartDateLinearLayout.Orientation = Orientation.Vertical;
                    StartDateLinearLayout.SetGravity(GravityFlags.NoGravity);
                    LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                    lp.SetMargins(margin, 0, margin, margin);
                    StartDateLinearLayout.LayoutParameters = lp;

                    LinearLayout.LayoutParams startText = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, 0);
                    startText.SetMargins(0, 0, 0, 5);
                    startDateTextParent.LayoutParameters = startText;
                    startDateTextParent.Orientation = Orientation.Horizontal;
                    start_nunito.LayoutParameters = start_nunito.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, 0);
                    Date_start.SetHeight((int)Activity.Resources.GetDimension(Resource.Dimension.list_item_height_drugConfirm));

                    LinearLayout.LayoutParams frame_start = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 0);
                    frame_start.SetMargins(0, 0, 0, 0);
                    startDateFameLayout.LayoutParameters = frame_start;

                    endDateLinearLayout.Orientation = Orientation.Vertical;
                    endDateLinearLayout.SetGravity(GravityFlags.NoGravity);
                    endDateLinearLayout.LayoutParameters = lp;

                    LinearLayout.LayoutParams endText = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, 0);
                    endText.SetMargins(0, 0, 0, 5);
                    endDateTextParent.LayoutParameters = endText;
                    endDateTextParent.Orientation = Orientation.Horizontal;
                    //text
                    end_nunito.LayoutParameters = end_nunito.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, 0);
                    Date_end.SetHeight((int)Activity.Resources.GetDimension(Resource.Dimension.list_item_height_drugConfirm));

                    LinearLayout.LayoutParams frame_end = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 0);
                    frame_end.SetMargins(0, 0, 0, 0);
                    endDateFameLayout.LayoutParameters = frame_end;
                }
            }

            rootView.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            rootView.ViewTreeObserver.AddOnGlobalLayoutListener(this);
        }

        private void HandleBtn1Click(object sender, EventArgs e)
        {
            btn1.SetTextColor(Resources.GetColor(Resource.Color.highlight_color));
            btn2.SetTextColor(Resources.GetColor(Resource.Color.claim_text));
            btn3.SetTextColor(Resources.GetColor(Resource.Color.claim_text));
            layout_btn1.Visibility = ViewStates.Visible;
            layout_btn2.Visibility = ViewStates.Invisible;
            layout_btn3.Visibility = ViewStates.Invisible;
            _model.SelectedDisplayBy = _model.DisplayBy[0];
        }

        private void HandleBtn2Click(object sender, EventArgs e)
        {
            btn1.SetTextColor(Resources.GetColor(Resource.Color.claim_text));
            btn2.SetTextColor(Resources.GetColor(Resource.Color.highlight_color));
            btn3.SetTextColor(Resources.GetColor(Resource.Color.claim_text));
            layout_btn1.Visibility = ViewStates.Invisible;
            layout_btn2.Visibility = ViewStates.Visible;
            layout_btn3.Visibility = ViewStates.Invisible;
            _model.SelectedDisplayBy = _model.DisplayBy[1];
        }

        private void HandleBtn3Click(object sender, EventArgs e)
        {
            btn1.SetTextColor(Resources.GetColor(Resource.Color.claim_text));
            btn2.SetTextColor(Resources.GetColor(Resource.Color.claim_text));
            btn3.SetTextColor(Resources.GetColor(Resource.Color.highlight_color));
            layout_btn1.Visibility = ViewStates.Invisible;
            layout_btn2.Visibility = ViewStates.Invisible;
            layout_btn3.Visibility = ViewStates.Visible;
            _model.SelectedDisplayBy = _model.DisplayBy[2];
        }
    }
}