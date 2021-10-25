using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.ViewModelParameters
{
    public class WebAgreementViewModelParameters
    {
        public NavigationCatalog Catalog { get; set; }

        public WebAgreementViewModelParameters(NavigationCatalog catalog)
        {
            Catalog = catalog;
        }
    }
}
