using System;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Attributes
{
    public sealed class AddOnProfileCreationAttributeClass : Attribute, BaseAttribute<bool>
    {
        private readonly bool value;

        public AddOnProfileCreationAttributeClass(bool value)
        {
            this.value = value;
        }

        public bool Value
        {
            get { return value; }
        }
    }
}