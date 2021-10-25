using System;
namespace MobileClaims.Droid.Views.DirectDeposit
{
    public class DirectDepositInvoker
    {
        public DirectDepositInvoker()
        {
        }

        public void Invoke(IDirectDepositCommand directDepositCommand, bool isCompleted)
        {
            directDepositCommand.Execute(isCompleted);
        }
    }
}
