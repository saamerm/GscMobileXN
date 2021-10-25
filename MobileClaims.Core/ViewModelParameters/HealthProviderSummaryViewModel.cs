using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using MvvmCross.ViewModels;
using Newtonsoft.Json;
using PhoneNumbers;

namespace MobileClaims.Core.ViewModelParameters
{
    public class HealthProviderSummaryModel : MvxNotifyPropertyChanged, IEqualityComparer
    {
        public const string BundleKey = "data";
        public const string SelectedProviderBundleKey = "selectedProvider";

        public HealthProviderSummaryModel(HealthProviderSummary model, ISearchHealthProviderService providerService)
        {
            Model = model;
            DisplayRating =
                providerService?.ProviderTypes?
                    .SingleOrDefault(x => x.Title == model.ProviderTypeLabel)?.DisplayRating ?? false;
        }

        [JsonConstructor]
        public HealthProviderSummaryModel(HealthProviderSummary model,
            IEnumerable<HealthProviderTypeViewModel> providerTypes)
        {
            Model = model;
            DisplayRating = providerTypes?.SingleOrDefault(x => x.Title == model.ProviderTypeLabel)?.DisplayRating ??
                            false;
        }
        
        public HealthProviderSummary Model { get; set; }

        public string PhoneFormatted
        {
            get
            {
                try
                {
                    var phone = PhoneNumberUtil.GetInstance().Parse(Model.Phone, "CA");
                    return PhoneNumberUtil.GetInstance().Format(phone, PhoneNumberFormat.NATIONAL);
                }
                catch (NumberParseException)
                {
                    return string.Empty;
                }
            }
        }

        public string DistanceText => $"{Model.Distance:0.00} KM";

        public bool IsFavourite
        {
            get => Model.FavouriteProviderIndicator == "Y";
            set
            {
                Model.FavouriteProviderIndicator = value ? "Y" : "N";
                RaisePropertyChanged(() => IsFavourite);
                RaisePropertyChanged(() => HeartImageUrl);
                RaisePropertyChanged(() => AlternateHeartImageUrl);
            }
        }

        public bool DisplayRating { get; set; }

        public bool DisplayRatingAndRatingAvailable => DisplayRating && Model.OverallScore >= 0.5;

        public bool DisplayRatingAndRatingNotAvailable => DisplayRating && Model.OverallScore < 0.5;

        public string FullAddress => GetFullAddress(true);

        public string FullAddressMultiline => GetFullAddress(false);

        public string ScoreString => Model.OverallScore.ToString("0.0");

        public string HeartImageUrl => IsFavourite ? "heart_on" : "heart_off";

        public string AlternateHeartImageUrl => IsFavourite ? "HeartOn" : "HeartOff";

        public bool IsDirectBill => Model.DbAdherenceInd == "Y";

        public string ProviderType => Model.ProviderTypeLabel.ToUpper();

        public string Hyperlink => Model.GooglePlaceData?.Website ?? string.Empty;

        public string OpeningHoursText
        {
            get
            {
                if (string.IsNullOrEmpty(Model.OperatingHoursLabel))
                {
                    return string.Empty;
                }

                return Resource.Open + Model.OperatingHoursLabel;
            }
        }

        public string WeekdayText
        {
            get
            {
                if (Model?.GooglePlaceData?.OpeningHours?.WeekdayText == null)
                {
                    return string.Empty;
                }
                else
                {
                    return string.Join("\n", Model.GooglePlaceData.OpeningHours.WeekdayText);
                }
            }
        }

        bool IEqualityComparer.Equals(object x, object y)
        {
            return ((HealthProviderSummaryModel) x).Model.ProviderId ==
                   ((HealthProviderSummaryModel) y).Model.ProviderId;
        }

        public int GetHashCode(object obj)
        {
            return ((HealthProviderSummaryModel) obj).Model.ProviderId;
        }

        private string GetFullAddress(bool oneLineAddress)
        {
            string fullAddress;

            if (!string.IsNullOrWhiteSpace(Model.AddressLine1))
            {
                if (!string.IsNullOrWhiteSpace(Model.AddressLine2))
                {
                    fullAddress = Model.AddressLine1.Trim()
                                  + (oneLineAddress ? ", " : Environment.NewLine)
                                  + Model.AddressLine2.Trim();
                }
                else
                {
                    fullAddress = Model.AddressLine1.Trim();
                }
            }
            else
            {
                fullAddress = Model.AddressLine2.Trim();
            }

            if (oneLineAddress)
            {
                fullAddress += ", " + GetSingleLineCityProvincePostalCode();
            }
            else
            {
                fullAddress += Environment.NewLine + GetSingleLineCityProvincePostalCode();
            }

            return fullAddress;
        }

        private string GetSingleLineCityProvincePostalCode()
        {
            var list = new[] {Model.City, Model.Province, Model.PostalCode}.Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
            return string.Join(", ", list.ToArray());
        }
    }
}