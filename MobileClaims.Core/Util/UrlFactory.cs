namespace MobileClaims.Core.Util
{
    public class UrlFactory
    {
        public class UriPaths
        {
            public const string ServiceProvider = ApiVersion + "/serviceprovider";

            public const string ServiceProviderDetails = ApiVersion + "/serviceprovider/{providerId}";

            public const string ProviderTypeOption = "providertypeoption/{planMemberId}";

            public const string ProviderFavourite = "providerfavourite/{providerId}";

            public const string ApiVersion = "v2";

            public const string PlaceAutocomplete = "placeautocomplete?address={address}";

            public const string PlaceGeocoding = "geocode?address={{address}}";
        }

        public static string GetSubUrl(string path)
        {
            return $"{GSCHelper.GSC_SERVICE_BASE_URL_SUB}/api/{path}";
        }
    }
}
