using System;
using System.Reflection;
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;

namespace AlienLabs.AlienAdrenaline.Domain.Helpers
{
    public static class EnumHelper
    {
        public static R GetAttributeValue<T, R>(Enum _enum)
        {
            R attributeValue = default(R);

            if (_enum != null)
            {
                FieldInfo fi = _enum.GetType().GetField(_enum.ToString());
                if (fi != null)
                {
                    T[] attributes = fi.GetCustomAttributes(typeof(T), false) as T[];
                    if (attributes != null && attributes.Length > 0)
                    {
                        BaseAttribute<R> attribute = attributes[0] as BaseAttribute<R>;
                        if (attribute != null)
                            attributeValue = attribute.Value;
                    }
                }
            }

            return attributeValue;
        }
    }
}