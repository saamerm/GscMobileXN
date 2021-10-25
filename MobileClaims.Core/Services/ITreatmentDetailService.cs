using MobileClaims.Core.Services.Requests;
using MobileClaims.Core.Services.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileClaims.Core.Services
{
    public interface ITreatmentDetailService
    {
        Task<List<ValidateDentalTreatmentResponse>> ValidateDentalTreatmentAsync(ValidateDentalTreatmentRequest request);
    }
}