using System;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Attributes
{
    public sealed class AllowRollbackAttributeClass : Attribute, BaseAttribute<bool>
    {
        private readonly bool value;

        public AllowRollbackAttributeClass(bool value)
        {
            this.value = value;
        }

        public bool Value
        {
            get { return value; }
        }
    }
}