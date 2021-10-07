using System.Collections.Generic;

namespace Dominator.Domain.Classes.Models
{
    public class AdvancedSetting<T> : IAdvancedSetting<T>
    {
        public uint Id { get; set; }
        public T MinValue { get; set; }
        public T MaxValue { get; set; }
        public List<T> SupportedValuesList { get; set; }
        public int NumOfDecimals { get; set; }
    }
}