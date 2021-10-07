using System;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Attributes
{
    public sealed class RequireThermalCapabilitiesAttributeClass : Attribute, BaseAttribute<bool>
    {
        private readonly bool value;

        public RequireThermalCapabilitiesAttributeClass(bool value)
        {
            this.value = value;
        }

        public bool Value
        {
            get { return value; }
        }
    }
}