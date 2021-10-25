using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MobileClaims.Core.Services.Requests;
using MobileClaims.Core.Services.Responses;

namespace MobileClaims.Core.Services
{
    public class TreatmentDetailService : ApiClientHelper, ITreatmentDetailService
    {
        public TreatmentDetailService()
        {
        }

        public async Task<List<ValidateDentalTreatmentResponse>> ValidateDentalTreatmentAsync(ValidateDentalTreatmentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var apiClient = new ApiClient<List<ValidateDentalTreatmentResponse>>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
                HttpMethod.Post,
                $"{GSCHelper.GSC_SERVICE_BASE_URL_SUB}/api/DentalTreatmentValidation",
                apiBody: request);

            return await ExecuteRequestWithRetry(apiClient);
        }
    }
}