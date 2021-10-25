using System;

namespace MobileClaims.iOS.Helper
{
    public static class AttributeHelper
    {
        public static bool HasAttribute(this Type type, Type attributeType, bool isInherited = false)
        {
            var attributes = type.GetCustomAttributes(attributeType, isInherited);

            return attributes != null && attributes.Length > 0;
        }
    }
}