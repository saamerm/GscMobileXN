using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.ComponentModel;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels.HCSA
{
    public class HCSAViewModelBase : ViewModelBase, IDisposable
    {
        public void NotifyCommands()
        {
            //Define a query that will fetch all the commands in the passed-in instance
            var commands = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                       .Where(p => p.PropertyType == typeof(MvxCommand)
                                              || p.PropertyType.GetInterfaces().Contains(typeof(ICommand))
                                              || p.PropertyType == (typeof(ICommand))).ToList<PropertyInfo>();
            //Iterate over them
            foreach (PropertyInfo pi in commands)
            {
                //Safe-cast the current item to MvxCommand
                MvxCommand command = pi.GetValue(this) as MvxCommand;
                //Double-check it really *is* an MvxCommand by testing if the result is not null
                if (command != null)
                {
                    //We're good to go.  Tell the command it should re-check whether it can run or not
                    this.InvokeOnMainThread(() => command.RaiseCanExecuteChanged());
                }
            }
        }
        protected override MvxInpcInterceptionResult InterceptRaisePropertyChanged(PropertyChangedEventArgs changedArgs)
        {
            if (changedArgs.PropertyName != "IsContinueButtonVisible" && changedArgs.PropertyName != "IsAddButtonVisible")
            {
                NotifyCommands();
            }
            return base.InterceptRaisePropertyChanged(changedArgs);
        }

        private bool _isbusy;
        public bool IsBusy
        {
            get
            {
                return _isbusy;
            }
            set
            {
                _isbusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }


        protected virtual void PublishMessages()
        {

        }
        protected virtual void Unsubscribe()
        {

        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~HCSAViewModelBase() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public virtual void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
    }
}
