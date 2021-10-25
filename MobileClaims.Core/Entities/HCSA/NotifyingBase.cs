using System.ComponentModel;
using MvvmCross;
using MvvmCross.Base;

namespace MobileClaims.Core.Entities.HCSA
{
    public class NotifyingBase : INotifyPropertyChanged
    {
        public NotifyingBase()
        {
            dispatcher = Mvx.IoCProvider.Resolve<IMvxMainThreadDispatcher>();
        }
        #region INotifyPropertyChanged Stuff
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                dispatcher.RequestMainThreadAction(() =>PropertyChanged(this, e));
            }
        }
        #endregion

        protected IMvxMainThreadDispatcher dispatcher; //Don't be a jerk - prevent threading exceptions
    }
}
