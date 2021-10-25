using System;
namespace MobileClaims.Droid.Views.DirectDeposit
{
    public interface IDirectDepositCommand
    {
        void Execute(bool isCompleted);
    }
}
