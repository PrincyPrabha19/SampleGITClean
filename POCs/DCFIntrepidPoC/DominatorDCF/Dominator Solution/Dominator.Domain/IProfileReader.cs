using System.Collections.Generic;
using Dominator.ServiceModel;

namespace Dominator.Domain
{
    public interface IProfileReader
    {
        IXTUService XTUService { set; }
        IDataHeader DataHeader { get; }
        List<IDataComponent> DataComponents { get; }
        ICryptoManager CryptoManager { get; set; }

        bool Load(string path, out bool loadProfileValuesFailed);
    }
}