using MobileClaims.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileClaims.Core.Services
{
    public interface IDrugLookupService
    {
        List<DrugInfo> SearchResult { get; }
        DrugInfo SearchByDINResult { get; set; }
        DrugInfo EligibilityResult { get; }
        string SpecialAuthorizationFormPath { get; }

        Task GetByName(string DrugName);
        Task GetByDIN(int DIN);
        Task CheckEligibility(Participant participant, DrugInfo drug);
        Task GetSpecialAuthorizationForm(string formPath);
        Task EmailSpecialAuthorizationForm(string formPath, EmailRequest er);
    }
}
