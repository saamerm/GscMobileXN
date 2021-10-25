using System.Windows.Input;

namespace MobileClaims.Core.ViewModels
{
    public interface IRemove
    {
        ICommand RemoveCommand { get; }
    }
}
