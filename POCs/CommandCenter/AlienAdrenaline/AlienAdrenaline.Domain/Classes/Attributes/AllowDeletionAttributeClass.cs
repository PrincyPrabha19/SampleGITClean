using System;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Attributes
{
    public sealed class AllowDeletionAttributeClass : Attribute, BaseAttribute<bool>
    {
        private readonly bool value;

        public AllowDeletionAttributeClass(bool value)
        {
            this.value = value;
        }

        public bool Value
        {
            get { return value; }
        }
    }
}