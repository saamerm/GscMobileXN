using System;
using MobileClaims.Core.Services.Requests;
using MobileClaims.Core.Services.Responses;
using MobileClaims.Core.ViewModels;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModelParameters
{
    public class SearchFilterData : MvxNotifyPropertyChanged
    {
        public const string BundleKeyFilters = "filters";
        public const string BundleKeySelectedProvider = "selectedProvider";
        public event EventHandler SearchFilterDataChanged;

        private HealthProviderTypeViewModel _selectedProviderType;

        public HealthProviderTypeViewModel SelectedProviderType
        {
            get => _selectedProviderType;
            set
            {
                SetProperty(ref _selectedProviderType, value);
                SearchFilterDataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool IsDisplayRating => SelectedProviderType != null && SelectedProviderType.DisplayRating;

        private bool _isDirectBill;

        public bool IsDirectBill
        {
            get => _isDirectBill;
            set
            {
                SetProperty(ref _isDirectBill, value);
                SearchFilterDataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool _isRecentlyVisited;

        public bool IsRecentlyVisited
        {
            get => _isRecentlyVisited;
            set
            {
                SetProperty(ref _isRecentlyVisited, value);
                SearchFilterDataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool _isStarRating;

        public bool IsStarRating
        {
            get => _isStarRating;
            set
            {
                SetProperty(ref _isStarRating, value);
                SearchFilterDataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private double _starRating = 1;

        public double StarRating
        {
            get => _starRating;
            set
            {
                SetProperty(ref _starRating, value);
                SearchFilterDataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private SortByChoice _sortByChoicePharmacy = SortByChoice.SortByRatingDescAndDistanceAsc;

        public SortByChoice SortByChoicePharmacy
        {
            get => _sortByChoicePharmacy;
            set
            {
                SetProperty(ref _sortByChoicePharmacy, value);
                SearchFilterDataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private SortByChoice _sortByChoice = SortByChoice.SortByDistanceAsc;

        public SortByChoice SortByChoice
        {
            get => _sortByChoice;
            set
            {
                SetProperty(ref _sortByChoice, value);
                SearchFilterDataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private Location _location;

        public Location Location
        {
            get => _location;
            set
            {
                SetProperty(ref _location, value);
                SearchFilterDataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private string _locationName;

        public string LocationName
        {
            get => _locationName;
            set => SetProperty(ref _locationName, value);
        }
    }
}