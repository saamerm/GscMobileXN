using System.Threading.Tasks;

namespace MobileClaims.Core.ViewModels
{
    public class SureHealthViewModel : ViewModelBase
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

        public SureHealthViewModel()
        {
        }

        public override Task Initialize()
        {
            Busy = true;
            Uri = Resource.SureHealthLink;
            return base.Initialize();
        }
    }
}