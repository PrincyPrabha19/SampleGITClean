using System.Collections.Generic;

namespace Dominator.Domain
{
    public interface IAdvancedSetting<T>
    {
        uint Id { get; set; }
        T MinValue { get; set; }
        T MaxValue { get; set; }
        List<T> SupportedValuesList { get; set; }
        int NumOfDecimals { get; set; }
    }
}
