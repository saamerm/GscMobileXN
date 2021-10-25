using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels.DirectDeposit;

namespace MobileClaims.Droid.Views.DirectDeposit
{
    public class Step2Command : IDirectDepositCommand
    {
        private View m_view = null;
        private ImageView _stepTwoImageView;
        private DirectDepositViewModel _viewModel;
        ImageView _expandCollapseStep2;
        LinearLayout _stepTwoLinearLayout;

        public Step2Command(View view, DirectDepositViewModel viewModel)
        {
            m_view = view;
            _viewModel = viewModel;
        }

        public void Execute(bool isCompleted)
        {
            _stepTwoImageView = m_view.FindViewById<ImageView>(Resource.Id.stepTwoIcon);
            var step2ContinueButton = m_view.FindViewById<Button>(Resource.Id.stepTwoContinueButton);
            step2ContinueButton.Click -= Step2ContinueButton_Click;
            step2ContinueButton.Click += Step2ContinueButton_Click;

            if (isCompleted)
                _stepTwoImageView.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.round_checkbox_selected));
            else
            {
                _stepTwoImageView.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.round_checkbox_unselected));
                m_view.FindViewById<TextView>(Resource.Id.account_number_error).Text = "";
                m_view.FindViewById<TextView>(Resource.Id.transit_number_error).Text = "";
                m_view.FindViewById<TextView>(Resource.Id.bank_number_error).Text = "";
            }
            _expandCollapseStep2 = m_view.FindViewById<ImageView>(Resource.Id.step2ImageView);
            _stepTwoLinearLayout = m_view.FindViewById<LinearLayout>(Resource.Id.StepTwoLinearLayout);
            _expandCollapseStep2.Click -= ExpandCollapseStep2_Click;
            _expandCollapseStep2.Click += ExpandCollapseStep2_Click;
            if (_viewModel.ShouldExpandAllSections)
            {
                ExpandWhileComplete();
            }
        }

        private void ExpandWhileComplete()
        {
            _stepTwoLinearLayout.Visibility = ViewStates.Visible;
            _expandCollapseStep2.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.arrow_up_gray));
        }

        private void ExpandCollapseStep2_Click(object sender, System.EventArgs e)
        {
            if (_stepTwoLinearLayout.IsShown)
            {
                _stepTwoLinearLayout.Visibility = ViewStates.Gone;
                _expandCollapseStep2.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.arrow_down_gray));
            }
            else
            {
                _stepTwoLinearLayout.Visibility = ViewStates.Visible;
                _expandCollapseStep2.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.arrow_up_gray));
            }
        }

        private void Step2ContinueButton_Click(object sender, System.EventArgs e)
        {
            if (_viewModel.IsBankNumberValid == true && _viewModel.IsAccountNumberValid == true && _viewModel.IsTransitNumberValid == true)
            {
                _stepTwoImageView.SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.round_checkbox_selected));
                _viewModel.IsStep2Completed = true;

                _viewModel.ValidateAndSaveBankingInfoCommand.Execute();
            }
        }
    }
}
