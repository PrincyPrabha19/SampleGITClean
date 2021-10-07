using System;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Attributes
{
    public sealed class InitializeOnCreationAttributeClass : Attribute, BaseAttribute<bool>
    {
        private readonly bool value;

        public InitializeOnCreationAttributeClass(bool value)
        {
            this.value = value;
        }

        public bool Value
        {
            get { return value; }
        }
    }
}