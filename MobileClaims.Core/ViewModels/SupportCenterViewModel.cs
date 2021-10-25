using System.Threading.Tasks;

namespace MobileClaims.Core.ViewModels
{
    public class SupportCenterViewModel : ViewModelBase
    {
        private bool _busy;
        private string _uri;

        public string Uri
        {
            get => _uri;
            set
            {
                SetProperty(ref _uri, value);
            }
        }

        public bool Busy
        {
            get => _busy;
            set
            {
                SetProperty(ref _busy, value);
            }
        }

        public SupportCenterViewModel()
        {
        }

        public override Task Initialize()
        {
            Busy = true;
            // Only GSC, WWL, ARTA has Support Center, the others dont have SupportCenterLink in the resources, so it causes a Build error
#if GSC || WWL || ARTA
            Uri = BrandResource.SupportCenterLink;
#endif
            return base.Initialize();
        }
    }
}
