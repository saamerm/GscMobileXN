using System.Collections.ObjectModel;
using MobileClaims.Core.Entities;
using MvvmCross.Commands;

namespace MobileClaims.Core.ViewModels.Interfaces
{
    public interface IFileNamesContainer
    {
        ObservableCollection<DocumentInfo> Attachments { get; }
    }

    public interface ICanDeleteFile
    {
        IMvxCommand DeleteCommand { get; }
    }
}