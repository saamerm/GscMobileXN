using System;
using System.Collections.Generic;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Newtonsoft.Json;

namespace MobileClaims.Core.ViewModels
{
    public class HealthProviderTypeViewModel : MvxNotifyPropertyChanged, IHierarchicalItem<HealthProviderTypeViewModel>, 
        IEquatable<HealthProviderTypeViewModel>
    {
        public HealthProviderTypeViewModel()
        {
            ChildItems = new List<HealthProviderTypeViewModel>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public int? ParentId { get; set; }

        public string ProviderTypeCodes { get; set; }

        public string ImageUrl { get; set; }

        public int SortOrder { get; set; }

        public string LineOfBusinessCode { get; set; }

        public IList<HealthProviderTypeViewModel> ChildItems { get; set; }

        bool _isSelected;
        
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public bool DisplayRating { get; set; }

        public bool IsSearchable { get; set; }

        [JsonIgnore]
        public IMvxCommand SelectHealthProviderTypeCommand { get; set; }

        public int GetHashCode(HealthProviderTypeViewModel obj)
        {
            return obj.Id;
        }

        public bool Equals(HealthProviderTypeViewModel other)
        {
            return Id == other.Id;
        }     
    }

    public interface IHierarchicalItem<T> {
        int Id { get; }
        int? ParentId { get; }
        string Title { get; }
        IList<T> ChildItems { get; }
        bool IsSelected { get; set; }
    }
}
