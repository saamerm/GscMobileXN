using Newtonsoft.Json;
using System.ComponentModel;

namespace MobileClaims.Core.Entities
{
    public class SelectableItem : NotifyingBase
    {
        public SelectableItem() : base() { }

        private bool _selected;
        [JsonIgnore]
        public bool Selected
        {
            get { return _selected; }
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
