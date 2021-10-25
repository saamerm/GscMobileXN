using System;

namespace MobileClaims.Core.Attributes
{
    public class RequiresAuthentication : Attribute
    {
        private bool _value;
        public bool Value
        {
            get => _value;
            set => _value = value;
        }

        public RequiresAuthentication(bool value)
        {
            _value = value;
        }
    }
}
