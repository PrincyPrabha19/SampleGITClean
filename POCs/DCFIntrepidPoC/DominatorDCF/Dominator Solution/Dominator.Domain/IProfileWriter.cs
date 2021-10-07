using System.Collections.Generic;
using Dominator.ServiceModel;

namespace Dominator.Domain
{
    public interface IProfileWriter
    {
        IXTUService XTUService { set; }
        ICryptoManager CryptoManager { get; set; }

        bool Save(string profilePath, IDataHeader dataHeader, List<IDataComponent> dataComponents);
    }
}