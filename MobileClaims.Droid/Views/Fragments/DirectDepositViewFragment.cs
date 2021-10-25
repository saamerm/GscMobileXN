using System;
using System.ComponentModel;
using System.Drawing;
using Android.OS;
using Android.Support.V4.Text;
using Android.Text;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels.DirectDeposit;
using MobileClaims.Droid.Views.DirectDeposit;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MobileClaims.Droid.Views.Fragments
{
    [Region(Resource.Id.phone_main_region)]
    public class DirectDepositViewFragment_ : BaseFragment
    {
        private View m_view;
        private DirectDepositInvoker directDepositInvoker = new DirectDepositInvoker();
        private Step1Command step1Command;
        private Step2Command step2Command;
        private Step3Command step3Command;
        private DirectDepositViewModel _viewModel;
        DirectDepositViewModel _model;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            m_view = this.BindingInflate(Resource.Layout.DirectDeposit, null);
            _viewModel = (DirectDepositViewModel)ViewModel;
            step1Command = new Step1Command(m_view, _viewModel);
            step2Command = new Step2Command(m_view, _viewModel);
            step3Command = new Step3Command(m_view, _viewModel);
            return m_view;
        }

        private void PopulateStep3DontForgetText(View view)
        {
            var dontForgetTextView = view.FindViewById<TextView>(Resource.Id.DisclaimerText3);
            var dontForgetText = "<b>" + _model.DiscalimerNote3B + "</b>" + _model.DiscalimerNote3;
            dontForgetTextView.TextFormatted = HtmlCompat.FromHtml(dontForgetText, HtmlCompat.FromHtmlModeLegacy);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (DirectDepositViewModel)ViewModel;
            PopulateStep3DontForgetText(view);
            _model.PropertyChanged -= model_propertyChanged;
            _model.PropertyChanged += model_propertyChanged;
        }

        private void model_propertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Step1 Region
            if (e.PropertyName.Equals(nameof(_model.ShouldExpandAllSections)))
            {
                if (_model.ShouldExpandAllSections)
                {
                    directDepositInvoker.Invoke(step1Command, true);
                    directDepositInvoker.Invoke(step2Command, true);
                    directDepositInvoker.Invoke(step3Command, true);
                }
            }
            if (e.PropertyName.Equals(nameof(_model.IsStep1Completed)))
            {
                if (_model.IsStep1Completed == null)
                    directDepositInvoker.Invoke(step1Command, false);
                else
                    directDepositInvoker.Invoke(step1Command, _model.IsStep1Completed.Value);

                if (_model.IsStep1Completed == true && _model.ShouldExpandAllSections == false)
                {
                    m_view.FindViewById<LinearLayout>(Resource.Id.StepOneLinearLayout).Visibility = ViewStates.Gone;
                    m_view.FindViewById<LinearLayout>(Resource.Id.StepTwoLinearLayout).Visibility = ViewStates.Visible;
                    m_view.FindViewById<ImageView>(Resource.Id.step1ImageView).SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.arrow_down_gray));                    
                    if (_model.IsStep2Completed == null)
                        directDepositInvoker.Invoke(step2Command, false);
                    else
                        directDepositInvoker.Invoke(step2Command, _model.IsStep2Completed.Value);
                }
            }

            if (e.PropertyName.Equals(nameof(_model.IsStep2Completed)))
            {
                if (_model.IsStep2Completed == null)
                    directDepositInvoker.Invoke(step2Command, false);
                else
                    directDepositInvoker.Invoke(step2Command, _model.IsStep2Completed.Value);

                if (_model.IsStep2Completed == true && _model.ShouldExpandAllSections == false)
                {
                    m_view.FindViewById<LinearLayout>(Resource.Id.StepTwoLinearLayout).Visibility = ViewStates.Gone;
                    m_view.FindViewById<LinearLayout>(Resource.Id.StepThreeLinearLayout).Visibility = ViewStates.Visible;
                    m_view.FindViewById<ImageView>(Resource.Id.step2ImageView).SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.arrow_down_gray));
                    if (_model.IsStep3Completed == null)
                        directDepositInvoker.Invoke(step3Command, false);
                    else
                        directDepositInvoker.Invoke(step3Command, _model.IsStep3Completed.Value);
                }
            }

            //Step2 Region
            if (e.PropertyName.Equals(nameof(_model.BankNumber)))
            {
                if (_model.IsStep2Completed == null)
                    directDepositInvoker.Invoke(step2Command, false);
                else
                    directDepositInvoker.Invoke(step2Command, _model.IsStep2Completed.Value);
            }

            //Step3 Region
            if (e.PropertyName.Equals(nameof(_model.IsStep3Completed)))
            {
                if (_model.IsStep3Completed == null)
                    directDepositInvoker.Invoke(step3Command, false);
                else
                    directDepositInvoker.Invoke(step3Command, _model.IsStep3Completed.Value);

                if (_model.IsStep3Completed == true && _model.ShouldExpandAllSections == false)
                {
                    m_view.FindViewById<LinearLayout>(Resource.Id.StepThreeLinearLayout).Visibility = ViewStates.Gone;
                    m_view.FindViewById<ImageView>(Resource.Id.step3ImageView).SetImageDrawable(m_view.Context.GetDrawable(Resource.Drawable.arrow_up_gray));
                }
            }
        }
    }
}