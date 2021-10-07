using System;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Attributes
{
    public sealed class AllowCachingAttributeClass : Attribute, BaseAttribute<bool>
    {
        private readonly bool value;

        public AllowCachingAttributeClass(bool value)
        {
            this.value = value;
        }

        public bool Value
        {
            get { return value; }
        }
    }
}