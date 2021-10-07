using System;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Attributes
{
    public sealed class DefaultIconAttributeClass : Attribute, BaseAttribute<string>
    {
        private readonly string value;

        public DefaultIconAttributeClass(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }
    }
}