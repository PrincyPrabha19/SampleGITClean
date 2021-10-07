using System;

namespace DI.Domain
{
    public interface IOperationManager
    {
        T Calculate<T>(string operation, Tuple<T, T> inputs);
    }
}
