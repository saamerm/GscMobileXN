using System;
using System.Collections.Generic;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using System.Threading.Tasks;
using System.Net.Http;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Services
{
    public class ProviderLookupService : ServiceBase, IProviderLookupService
    {
        private readonly IMvxMessenger _messenger;
        private readonly ILoginService _loginservice;
        private readonly ILanguageService _languageservice;
        private readonly IDataService _dataservice;
        private const string SERVICE_BASE_URL = GSCHelper.GSC_SERVICE_BASE_URL;
        private MvxSubscriptionToken _languageupdated;

#if FakingIt
        private int _counter = 0;
#endif

        public ProviderLookupService(IMvxMessenger messenger, ILoginService loginservice, ILanguageService languageservice, IDataService dataservice)
        {
            _messenger = messenger;
            _loginservice = loginservice;
            _languageservice = languageservice;
            _dataservice = dataservice;

            ServiceProviderProvinces = new List<ServiceProviderProvince>();

            GetServiceProviderProvinces(null, null);

            Claim claim = _dataservice.GetClaim(); //used for rehydration
            if (claim != null)
            {
                if (claim.PreviousServiceProviders != null && claim.PreviousServiceProviders.Count > 0) this.PreviousClaimServiceProviders = claim.PreviousServiceProviders;
                if (claim.ServiceProviderSearchResults != null && claim.ServiceProviderSearchResults.Count > 0) this.ClaimSearchResults = claim.ServiceProviderSearchResults;
            }
        }

        public List<ServiceProvider> ClaimSearchResults { get; set; }
        public List<ServiceProvider> PreviousServiceProviders { get; private set; }
        public List<ServiceProvider> PreviousClaimServiceProviders { get; set; }
        public List<ServiceProviderProvince> ServiceProviderProvinces { get; private set; }

        public async Task GetPreviousServiceProviders(string planMemberID)
        {
            if (string.IsNullOrEmpty(planMemberID))
            {
                _messenger.Publish<GetPreviousServiceProvidersError>(new GetPreviousServiceProvidersError(this) { Message = "planMemberID is null." });
                return;
            }
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("planMemberId", planMemberID);
            
            var client = new ApiClient<List<ServiceProvider>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, "/"+ GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{planMemberId}/serviceprovider?lastused=5", parameters);
            try
            {
                PreviousServiceProviders = null;
                PreviousServiceProviders = await client.ExecuteRequest();
                _messenger.Publish<GetPreviousServiceProvidersComplete>(new GetPreviousServiceProvidersComplete(this));
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetPreviousServiceProvidersError>(new GetPreviousServiceProvidersError(this) { Message = ex.Message });
            }
        }

        public async Task GetClaimPreviousServiceProviders(string planMemberID, string claimSubmissionTypeID, int lastUsed = 5)
        {

            if (planMemberID.IndexOf('-') > -1)
                planMemberID = planMemberID.Split('-')[0];

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("planMemberId", planMemberID);
            parameters.Add("providerTypeId", claimSubmissionTypeID);
            parameters.Add("lastUsed", lastUsed.ToString());
            var client = new ApiClient<List<ServiceProvider>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, "/"+ GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{planMemberId}/claimserviceprovider?type={providerTypeId}&lastused={lastUsed}", parameters);
            try
            {
                PreviousClaimServiceProviders = await client.ExecuteRequest();
                _messenger.Publish<GetClaimPreviousServiceProvidersComplete>(new GetClaimPreviousServiceProvidersComplete(this));
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetClaimPreviousServiceProvidersError>(new GetClaimPreviousServiceProvidersError(this) { Message = ex.Message });
            }
        }

        public async Task GetServiceProviderByName(string claimSubmissionTypeID, string lastName, string firstInitial)
        {

            MvxSubscriptionToken _refreshed = null;
            MvxSubscriptionToken _refreshFailed = null;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("claimSubmissionTypeID", claimSubmissionTypeID);
            parameters.Add("lastName", lastName);
            parameters.Add("firstInitial", firstInitial);
            var client = new ApiClient<List<ClaimServiceProvider>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, "/"+ GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/claimserviceprovider?type={claimSubmissionTypeID}&lname={lastName}&finitial={firstInitial}", parameters);
            try
            {
                ClaimSearchResults = ConvertToServiceProvidersList(await client.ExecuteRequest());
                _messenger.Publish<SearchForServiceProvidersComplete>(new SearchForServiceProvidersComplete(this));
            }
            catch (Exception ex)
            {
                _messenger.Publish<SearchForServiceProvidersError>(new SearchForServiceProvidersError(this) { Message = ex.Message });
            }
        }

        public async Task GetServiceProviderByPhoneNumber(string claimSubmissionTypeID, string phoneNumber)
        {
            MvxSubscriptionToken _refreshed = null;
            MvxSubscriptionToken _refreshFailed = null;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("providerTypeId", claimSubmissionTypeID.ToString());
            parameters.Add("phone", phoneNumber);
            var client = new ApiClient<List<ClaimServiceProvider>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, "/"+ GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/claimserviceprovider?type={providerTypeId}&phone={phone}", parameters);
            ClaimSearchResults = null; // clear old results
            try
            {
                ClaimSearchResults = ConvertToServiceProvidersList(await client.ExecuteRequest());
                _messenger.Publish<SearchForServiceProvidersComplete>(new SearchForServiceProvidersComplete(this));
            }
            catch(Exception ex)
            {
                _messenger.Publish<SearchForServiceProvidersError>(new SearchForServiceProvidersError(this) { Message = ex.Message });
            }
        }

        public async Task GetServiceProviderProvinces(Action success, Action<string, int> failure)
        {
            MvxSubscriptionToken _refreshed = null;
            MvxSubscriptionToken _refreshFailed = null;
            var client = new ApiClient<List<ServiceProviderProvince>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/serviceproviderprovince", null, true);
            try
            {
                List<ServiceProviderProvince> provincesList = await client.ExecuteRequest();
                provincesList.Insert(0, new ServiceProviderProvince { ID = "", Name = "" });
                ServiceProviderProvinces = provincesList;
                _messenger.Publish<RetrievedServiceProviderTypes>(new RetrievedServiceProviderTypes(this));
                if(success != null) success();
            } 
            catch (Exception ex)
            {  
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                if (failure != null) failure(ex.Message, (int)System.Net.HttpStatusCode.NotFound);
            }
        }

        private string GetLocationStringForUri(string address = "", string postalCode = "", double? lat = null, double? lng = null)
        {
            string locationString = string.Empty;

            if (lat != null && lng != null && lat.HasValue && lng.HasValue)
                locationString = string.Format("&latitude={0}&longitude={1}", "{latitude}", "{longitude}");
            else if (!string.IsNullOrEmpty(postalCode))
                locationString = string.Format("&postalcode={0}", "{postalcode}");    
            else if (!string.IsNullOrEmpty(address))
                locationString = string.Format("&address={0}", "{address}");

            return locationString;
        }

        private List<ServiceProvider> GetDummyListOfProviders()
        {
            List<ServiceProvider> listOfProviders = new List<ServiceProvider>();

            ServiceProvider provider1 = new ServiceProvider();
            provider1.BusinessName = "A WONG";
            provider1.Address = "3390 9TH AVE, APT #219";
            provider1.City = "CALGARY";
            provider1.Province = "AB";
            provider1.Phone = "(780) 433-5771";

            ServiceProvider provider2 = new ServiceProvider();
            provider2.BusinessName = "B LEVI";
            provider2.Address = "123 MAIN ST.";
            provider2.City = "BOULTON";
            provider2.Province = "ON";
            provider2.Phone = "(905) 111-2222";

            ServiceProvider provider3 = new ServiceProvider();
            provider3.BusinessName = "S CARSTAIRS";
            provider3.Address = "290 QUEEN ST.";
            provider3.City = "OSHAWA";
            provider3.Province = "ON";
            provider3.Phone = "(905) 222-1111";

            listOfProviders.Add(provider1);
            listOfProviders.Add(provider2);
            listOfProviders.Add(provider3);

            return listOfProviders;
        }

        private List<ServiceProvider> ConvertToServiceProvidersList(List<ClaimServiceProvider> claimServiceProviders)
        {
            List<ServiceProvider> serviceProviders = new List<ServiceProvider>();

            foreach (ClaimServiceProvider csp in claimServiceProviders)
            {
                ServiceProvider sp = new ServiceProvider();
                sp.ID = (int)(csp.ID);
                sp.BusinessName = csp.Name;
                sp.Address = csp.Address;
                if (!string.IsNullOrEmpty(csp.Address2)) sp.Address += " " + csp.Address2;
                sp.City = csp.City;
                sp.Province = csp.ProvinceCode;
                sp.PostalCode = csp.PostalCode;
                sp.Phone = csp.Phone;
                sp.RegistrationNumber = csp.RegistrationNumber;

                serviceProviders.Add(sp);
            }

            return serviceProviders;
        }

        public void ClearOldClaimData()
        {
            this.ClaimSearchResults = null;
            this.PreviousClaimServiceProviders = null;
        }
    }
}
