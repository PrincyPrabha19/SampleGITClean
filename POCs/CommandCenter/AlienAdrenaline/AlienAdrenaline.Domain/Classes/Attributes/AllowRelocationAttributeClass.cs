using System;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Attributes
{
    public sealed class AllowRelocationAttributeClass : Attribute, BaseAttribute<bool>
    {
        private readonly bool value;

        public AllowRelocationAttributeClass(bool value)
        {
            this.value = value;
        }

        public bool Value
        {
            get { return value; }
        }
    }
}