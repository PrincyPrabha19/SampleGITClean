using System.Collections.Generic;

namespace LoadingExternalPackage.Domain
{
    public class GroupMapping
    {
        public string Group { get; set; }
        public List<Mask> Masks { get; set; }
        public Hotspot Hotspot { get; set; }
        public List<Led> Leds { get; set; }
    }
}
