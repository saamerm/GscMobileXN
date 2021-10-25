using System;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels
{
    public class SearchHealthProviderViewModel : MvxViewModel
    {
        private string _searchQuery;

        public event EventHandler ClearTextBox;

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    ClearTextBox?.Invoke(this, EventArgs.Empty);
                }
                SetProperty(ref _searchQuery, value);
            }
        }

        public IMvxAsyncCommand<bool> PerformSearchCommand { get; set; }

        public IMvxCommand ShowRefineSearchCommand { get; set; }

        public string SearchHintText => Resource.SearchHint;
    }
}
