using System;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Attributes
{
    public sealed class BrowserMultiTabParameterAttributeClass : Attribute, BaseAttribute<string>
    {
        private readonly string value;

        public BrowserMultiTabParameterAttributeClass(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }
    }
}