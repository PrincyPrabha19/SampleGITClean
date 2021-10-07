using System;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Attributes
{
    public sealed class BrowserProgIdAttributeClass : Attribute, BaseAttribute<string>
    {
        private readonly string value;

        public BrowserProgIdAttributeClass(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }
    }
}