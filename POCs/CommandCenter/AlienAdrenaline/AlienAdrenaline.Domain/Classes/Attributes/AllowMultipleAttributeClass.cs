using System;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Attributes
{
    public sealed class AllowMultipleAttributeClass : Attribute, BaseAttribute<bool>
    {
        private readonly bool value;

        public AllowMultipleAttributeClass(bool value)
        {
            this.value = value;
        }

        public bool Value
        {
            get { return value; }
        }
    }
}