using System;
using System.Net.Http;
using System.Threading.Tasks;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Extensions;
using MobileClaims.Core.Services.Requests;
using MobileClaims.Core.Services.Responses;

namespace MobileClaims.Core.Services
{
    public class DirectDepositService : ApiClientHelper, IDirectDepositService
    {
        private readonly ILoginService _loginservice;

        public DirectDepositService(ILoginService loginservice)
        {
            _loginservice = loginservice;
        }

        private bool _isDirectDepositAuthorized= false;
        /// <summary>
        /// Function to check If the Plan Member has EFT Info available. If Not call API to check.
        /// Call Direct Deposit API every time when Plan Member does not have Direct Deposit Info.
        /// </summary>
        /// <returns>True if DirectDeposit has PaymentMethod as EF else False</returns>
        public async Task<bool> GetHasEFTInfoAsync()
        {
            if (_isDirectDepositAuthorized)
            {
                return true;
            }
            else
            {
                var directDepositInfo = await GetDirectDepositInfoAsync(_loginservice.GroupPlanNumber);
                _isDirectDepositAuthorized = directDepositInfo.IsDirectDepositAuthorized;
                return _isDirectDepositAuthorized;
            }
        }

        public async Task<DirectDepositInfo> GetDirectDepositInfoAsync(string planMemberId)
        {
            if (string.IsNullOrWhiteSpace(planMemberId))
            {
                throw new ArgumentNullException(nameof(planMemberId));
            }

            var apiClient = new ApiClient<DirectDepositResponse>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
                HttpMethod.Get,
                $"{GSCHelper.GSC_SERVICE_BASE_URL_SUB}/api/planmember/{planMemberId}/directdeposit");

            var response = await ExecuteRequestWithRetry(apiClient);
            return response.ToDirectDepositInfo();
        }

        public async Task<DirectDepositBankDetails> ValidateUserBankDetails(string planMemberId, BankValidationRequest requestedBankingData)
        {
            if (string.IsNullOrWhiteSpace(planMemberId))
            {
                throw new ArgumentNullException(nameof(planMemberId));
            }

            var apiClient = new ApiClient<DirectDepositBankDetailsResponse>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
               HttpMethod.Post,
               apiPath: $"{GSCHelper.GSC_SERVICE_BASE_URL_SUB}/api/directdepositvalidation",
               apiBody: requestedBankingData,
               useDefaultHeaders: true);

            var response = await ExecuteRequestWithRetry(apiClient);
            return response.ToDirectDepositBankDetails();
        }

        public async Task<DirectDepositInfo> SubmitDirectDepositDetails(string planMemberId, DirectDepositInfo _directDepositInfo)
        {
            if (string.IsNullOrWhiteSpace(planMemberId))
            {
                throw new ArgumentNullException(nameof(planMemberId));
            }

            var submitDirectDepositRequest = new SubmitDirectDepositRequest()
            {
                PaymentMethod = (_directDepositInfo.IsDirectDepositAuthorized)? "EF" : "CH",
                EftEmailIndicator = (_directDepositInfo.IsEnrolledForEmailNotification) ? "Y":"N",
                TransitNumber = _directDepositInfo.TransitNumber,
                BankNumber = _directDepositInfo.BankNumber,
                AccountNumber = _directDepositInfo.AccountNumber
            };

            var apiClient = new ApiClient<DirectDepositResponse>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
               HttpMethod.Put,
               apiPath: $"{GSCHelper.GSC_SERVICE_BASE_URL_SUB}/api/planmember/{planMemberId}/directdeposit",
               apiBody: submitDirectDepositRequest,
               useDefaultHeaders: true);

            var response = await ExecuteRequestWithRetry(apiClient);
            return response.ToDirectDepositInfo();
        }
    }
}
