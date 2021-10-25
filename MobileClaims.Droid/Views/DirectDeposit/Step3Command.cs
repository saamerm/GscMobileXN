using System;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels.DirectDeposit;

namespace MobileClaims.Droid.Views.DirectDeposit
{
    public class Step3Command : BaseDirectDepositCommand, IDirectDepositCommand
    {
        private View m_view;
        private DirectDepositViewModel _viewModel;
        public Step3Command(View view, DirectDepositViewModel viewModel) : base(view)
        {
            m_view = view;
            _viewModel = viewModel;
        }

        public void Execute(bool isCompleted)
        {
            if (_viewModel.IsStep1Completed != null && _viewModel.IsStep1Completed.Value)
                ShowLayout();
        }

        protected ImageView GetConsentIndicator1Instance => m_view.FindViewById<ImageView>(Resource.Id.StepThreeConsentIndictor);

        protected ImageView GetConsentIndicator2Instance => m_view.FindViewById<ImageView>(Resource.Id.StepThreeConsentIndictor2);

        protected ImageView GetIconIndicatorInstance => m_view.FindViewById<ImageView>(Resource.Id.stepThreeIcon);

        protected ImageView GetChevronInstance => m_view.FindViewById<ImageView>(Resource.Id.step3ImageView);

        protected LinearLayout GetConsentContainer => m_view.FindViewById<LinearLayout>(Resource.Id.StepThreeLinearLayout);

        protected LinearLayout GetConsentContainer1 => m_view.FindViewById<LinearLayout>(Resource.Id.StepThreeConsentLayout1);

        protected LinearLayout GetConsentContainer2 => m_view.FindViewById<LinearLayout>(Resource.Id.StepThreeConsentLayout2);

        protected void ShowLayout()
        {
            GetConsentContainer.Visibility = ViewStates.Visible;
            GetIconIndicatorInstance.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.round_checkbox_selected));

            if (_viewModel.Step3Model.IsOptedForNotification)
                ShouldReceiveNotificationsSelected(GetConsentContainer1);
            else
                ShouldNotReceiveNotificationsSelected(GetConsentContainer2);
            GetConsentIndicator1Instance.SetBackgroundDrawable(ContextCompat.GetDrawable(m_view.Context, Resource.Drawable.large_round_checkbox_selected));

            GetChevronInstance.Click -= Step3_CollapseButton_Click;
            GetChevronInstance.Click += Step3_CollapseButton_Click;

            GetConsentContainer1.Click -= Step3_ConsentLayout1_Click;
            GetConsentContainer1.Click += Step3_ConsentLayout1_Click;

            GetConsentContainer2.Click -= Step3_ConsentLayout2_Click;
            GetConsentContainer2.Click += Step3_ConsentLayout2_Click;
            if (_viewModel.ShouldExpandAllSections)
            {
                ExpandWhileComplete();
            }
        }

        private void ExpandWhileComplete()
        {
            if (_viewModel.Step3Model.IsOptedForNotification)
                ShouldReceiveNotificationsSelected(GetConsentContainer1);
            else
                ShouldNotReceiveNotificationsSelected(GetConsentContainer2);
            GetConsentContainer.Visibility = ViewStates.Visible;
            GetChevronInstance.SetImageDrawable(m_view.Context.ApplicationContext.GetDrawable(Resource.Drawable.arrow_up_gray));
        }

        private void SetConsentLayoutBackgroundColor(LinearLayout ConsentLayoutView)
        {
            int color = ContextCompat.GetColor(m_view.Context, Resource.Color.highlight_color);
            ConsentLayoutView.SetBackgroundColor(new Android.Graphics.Color(color));
        }

        private void ResetConsentLayoutBackgroundColor(LinearLayout ConsentLayoutView)
        {
            int color = ContextCompat.GetColor(m_view.Context, Resource.Color.gsc_transparent);
            ConsentLayoutView.SetBackgroundColor(new Android.Graphics.Color(color));
        }

        private void ShouldReceiveNotificationsSelected(LinearLayout layout)
        {
            SetConsentLayoutBackgroundColor(layout);
            ResetConsentLayoutBackgroundColor(GetConsentContainer2);
            GetConsentIndicator1Instance.SetBackgroundDrawable(ContextCompat.GetDrawable(m_view.Context, Resource.Drawable.large_round_checkbox_selected));
        }

        private void ShouldNotReceiveNotificationsSelected(LinearLayout layout)
        {
            SetConsentLayoutBackgroundColor(layout);
            ResetConsentLayoutBackgroundColor(GetConsentContainer1);
            GetConsentIndicator2Instance.SetBackgroundDrawable(ContextCompat.GetDrawable(m_view.Context, Resource.Drawable.large_round_checkbox_selected));
        }

        private void Step3_ConsentLayout1_Click(object sender, EventArgs e)
        {
            ShouldReceiveNotificationsSelected((LinearLayout)sender);
            _viewModel.Step3Model.IsOptedForNotification = true;
        }

        private void Step3_ConsentLayout2_Click(object sender, EventArgs e)
        {
            ShouldNotReceiveNotificationsSelected((LinearLayout)sender);
            _viewModel.Step3Model.IsOptedForNotification = false;
        }

        private void Step3_CollapseButton_Click(object sender, EventArgs e)
        {
            if (GetConsentContainer.IsShown)
            {
                GetConsentContainer.Visibility = ViewStates.Gone;
                GetChevronInstance.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.arrow_down_gray));
            }
            else
            {
                GetConsentContainer.Visibility = ViewStates.Visible;
                GetChevronInstance.SetImageDrawable(m_view.Context.ApplicationContext.GetDrawable(Resource.Drawable.arrow_up_gray));
            }
        }
    }
}
