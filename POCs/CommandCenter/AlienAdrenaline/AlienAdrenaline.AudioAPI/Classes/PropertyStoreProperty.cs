
using AlienLabs.AlienAdrenaline.AudioAPI.Structs;

namespace AlienLabs.AlienAdrenaline.AudioAPI.Classes
{
    public class PropertyStoreProperty
    {
        private PROPERTYKEY propertyKey;
        private PROPVARIANT propertyValue;

        internal PropertyStoreProperty(PROPERTYKEY key, PROPVARIANT value)
        {
            propertyKey = key;
            propertyValue = value;
        }

        public PROPERTYKEY Key
        {
            get
            {
                return propertyKey;
            }
        }

        public object Value
        {
            get
            {
                return propertyValue.Value;
            }
        }
    }
}
