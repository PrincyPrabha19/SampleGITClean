using System;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Attributes
{
    public sealed class ResourceKeyAttributeClass : Attribute, BaseAttribute<string>
    {
        private readonly string value;

        public ResourceKeyAttributeClass(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }
    }
}