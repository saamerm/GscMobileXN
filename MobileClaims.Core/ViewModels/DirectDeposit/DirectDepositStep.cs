using System;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels.DirectDeposit
{
    public class DirectDepositStep : MvxNotifyPropertyChanged
    {
        private bool _isStepCompleted;
        private bool _isExpanded;
        private string _stepNumber;
        private string _stepName;

        public event EventHandler<ShouldExpandSectionEventArgs> ShouldExpandSectionEvent;

        public bool IsStepCompleted
        {
            get => _isStepCompleted;
            set => SetProperty(ref _isStepCompleted, value);
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        public string StepNumber
        {
            get => _stepNumber;
            set => SetProperty(ref _stepNumber, value);
        }

        public string StepName
        {
            get => _stepName;
            set => SetProperty(ref _stepName, value);
        }

        internal protected void InvokeShouldExpandSectionEvent(bool shouldExpand, int section)
        {
            ShouldExpandSectionEvent?.Invoke(this, new ShouldExpandSectionEventArgs() { ShouldExpand = shouldExpand, StepNumber = section });
        }
    }
}
