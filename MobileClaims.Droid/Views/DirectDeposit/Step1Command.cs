using System;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels.DirectDeposit;

namespace MobileClaims.Droid.Views.DirectDeposit
{
    public class Step1Command : IDirectDepositCommand
    {
        private View m_view = null;
        private ImageView _expandCollapseStep1;
        private LinearLayout _stepOneLinearLayout;
        private LinearLayout _stepThreeLinearLayout;
        private LinearLayout _stepTwoLinearLayout;
        private LinearLayout _consentLayoutView1;
        private ImageView _consentStep1Indicator;
        private ImageView _expandCollapseStep2;
        private ImageView _expandCollapseStep3;
        private ImageView _stepOneImageView;
        DirectDepositViewModel _viewModel;

        public Step1Command(View view, DirectDepositViewModel viewModel)
        {
            m_view = view;
            _viewModel = viewModel;
        }

        public void Execute(bool isCompleted)
        {
            _consentStep1Indicator = m_view.FindViewById<ImageView>(Resource.Id.StepOneConsentIndictor);
            _stepOneImageView = m_view.FindViewById<ImageView>(Resource.Id.stepOneIcon);

            _expandCollapseStep1 = m_view.FindViewById<ImageView>(Resource.Id.step1ImageView);
            _stepOneLinearLayout = m_view.FindViewById<LinearLayout>(Resource.Id.StepOneLinearLayout);
            _stepThreeLinearLayout = m_view.FindViewById<LinearLayout>(Resource.Id.StepThreeLinearLayout);
            _stepTwoLinearLayout = m_view.FindViewById<LinearLayout>(Resource.Id.StepTwoLinearLayout);
            _consentLayoutView1 = m_view.FindViewById<LinearLayout>(Resource.Id.StepOneConsentLayout1);
            _expandCollapseStep2 = m_view.FindViewById<ImageView>(Resource.Id.step2ImageView);
            _expandCollapseStep3 = m_view.FindViewById<ImageView>(Resource.Id.step3ImageView);
            if (!isCompleted)
            {
                _stepOneImageView.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.round_checkbox_unselected));
                _consentStep1Indicator.SetBackgroundDrawable(ContextCompat.GetDrawable(m_view.Context, Resource.Drawable.large_round_checkbox_unselected));
                _stepTwoLinearLayout.Visibility = ViewStates.Gone;
                _stepThreeLinearLayout.Visibility = ViewStates.Gone;
                var stepThreeImageView = m_view.FindViewById<ImageView>(Resource.Id.stepThreeIcon);
                stepThreeImageView.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.round_checkbox_unselected));
            }
            else
            {
                _stepOneImageView.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.round_checkbox_selected));
                SetConsentLayoutBackgroundColor(_consentLayoutView1);
                _consentStep1Indicator.SetBackgroundDrawable(ContextCompat.GetDrawable(m_view.Context, Resource.Drawable.large_round_checkbox_selected));
            }
            _expandCollapseStep1.Click -= Step1_CollapseButton_Click;
            _expandCollapseStep1.Click += Step1_CollapseButton_Click;

            _consentLayoutView1.Click -= Step1_ConsentLayout1_Click;
            _consentLayoutView1.Click += Step1_ConsentLayout1_Click;

            if (_viewModel.ShouldExpandAllSections)
            {
                ExpandWhileComplete();
            }
        }

        private void SetConsentLayoutBackgroundColor(LinearLayout ConsentLayoutView)
        {
            int color = ContextCompat.GetColor(m_view.Context, Resource.Color.highlight_color);
            ConsentLayoutView.SetBackgroundColor(new Android.Graphics.Color(color));
        }

        private void ExpandWhileComplete()
        {
            SetConsentLayoutBackgroundColor(_consentLayoutView1);
            _consentStep1Indicator.SetBackgroundDrawable(ContextCompat.GetDrawable(m_view.Context, Resource.Drawable.large_round_checkbox_selected));
            _stepOneImageView.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.round_checkbox_selected));
            int color = ContextCompat.GetColor(m_view.Context, Resource.Color.background_color);
            _stepOneLinearLayout.Visibility = ViewStates.Visible;
            _expandCollapseStep1.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.arrow_up_gray));
        }

        private void Step1_ConsentLayout1_Click(object sender, EventArgs e)
        {
            SetConsentLayoutBackgroundColor((LinearLayout)sender);
            _consentStep1Indicator.SetBackgroundDrawable(ContextCompat.GetDrawable(m_view.Context, Resource.Drawable.large_round_checkbox_selected));
            _stepOneImageView.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.round_checkbox_selected));
            _viewModel.IsStep1Completed = true;
            _viewModel.SelectAuthorizeDirectDepositCommand.Execute();
            _expandCollapseStep2.Clickable = true;

            _expandCollapseStep3.Clickable = true;

            int color = ContextCompat.GetColor(m_view.Context, Resource.Color.background_color);
            m_view.FindViewById<LinearLayout>(Resource.Id.idStep2Layout).SetBackgroundColor(new Android.Graphics.Color(color));

            m_view.FindViewById<LinearLayout>(Resource.Id.idStep3Layout).SetBackgroundColor(new Android.Graphics.Color(color));
        }

        private void Step1_ConsentLayout2_Click(object sender, EventArgs e)
        {
            SetConsentLayoutBackgroundColor((LinearLayout)sender);
            _consentStep1Indicator.SetBackgroundDrawable(ContextCompat.GetDrawable(m_view.Context, Resource.Drawable.large_round_checkbox_unselected));

            m_view.FindViewById<ImageView>(Resource.Id.stepTwoIcon).SetBackgroundDrawable(ContextCompat.GetDrawable(m_view.Context, Resource.Drawable.round_checkbox_unselected));

            _expandCollapseStep2.Clickable = false;

            _expandCollapseStep3.Clickable = false;

            _stepTwoLinearLayout.Visibility = ViewStates.Gone;
            _expandCollapseStep2.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.arrow_down_gray));
            _stepThreeLinearLayout.Visibility = ViewStates.Gone;
            _expandCollapseStep3.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.arrow_down_gray));


            int color = ContextCompat.GetColor(m_view.Context, Resource.Color.light_grey);
            m_view.FindViewById<LinearLayout>(Resource.Id.idStep2Layout).SetBackgroundColor(new Android.Graphics.Color(color));

            m_view.FindViewById<LinearLayout>(Resource.Id.idStep3Layout).SetBackgroundColor(new Android.Graphics.Color(color));

        }

        private void Step1_CollapseButton_Click(object sender, EventArgs e)
        {
            if (_stepOneLinearLayout.IsShown)
            {
                _viewModel.Steps[0].IsExpanded = false;

                _stepOneLinearLayout.Visibility = ViewStates.Gone;
                _expandCollapseStep1.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.arrow_down_gray));
            }
            else
            {
                _viewModel.Steps[0].IsExpanded = true;

                _stepOneLinearLayout.Visibility = ViewStates.Visible;
                _expandCollapseStep1.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.arrow_up_gray));
            }
        }
    }
}