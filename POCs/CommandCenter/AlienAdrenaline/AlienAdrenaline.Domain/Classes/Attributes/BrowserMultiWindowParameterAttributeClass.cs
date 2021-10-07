using System;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Attributes
{
    public sealed class BrowserMultiWindowParameterAttributeClass : Attribute, BaseAttribute<string>
    {
        private readonly string value;

        public BrowserMultiWindowParameterAttributeClass(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }
    }
}