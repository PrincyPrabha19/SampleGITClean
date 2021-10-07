using System;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Attributes
{
    public sealed class AllowCreationAttributeClass : Attribute, BaseAttribute<bool>
    {
        private readonly bool value;

        public AllowCreationAttributeClass(bool value)
        {
            this.value = value;
        }

        public bool Value
        {
            get { return value; }
        }
    }
}