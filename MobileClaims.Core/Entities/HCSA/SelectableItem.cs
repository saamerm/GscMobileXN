using Newtonsoft.Json;
using System.ComponentModel;

namespace MobileClaims.Core.Entities.HCSA
{
    public class SelectableItem : NotifyingBase
    {
        public SelectableItem() : base() { }
        [JsonIgnore]
        private bool _selected;

        public bool Selected
        {
            get => _selected;
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Selected"));
                }
            }
        }
    }
}
