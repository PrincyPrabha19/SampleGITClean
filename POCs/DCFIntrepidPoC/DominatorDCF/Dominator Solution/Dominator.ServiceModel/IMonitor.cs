using System;
using System.ServiceModel;

namespace Dominator.ServiceModel
{
    [ServiceContract]
    public interface IMonitor<T>
    {
        [OperationContract]
        string Ping();

        [OperationContract(IsOneWay = true)]
        void Start(Guid clientID);

        [OperationContract(IsOneWay = true)]
        void Stop(Guid clientID);

        [OperationContract(IsOneWay = true)]
        void AddElement(uint elementID);

        [OperationContract(IsOneWay = true)]
        void AddElements(uint[] elementIDs);

        [OperationContract]
        T GetElementValue(uint elementID);

        [OperationContract]
        T[] GetAllElementValues(uint[] elementIDs);

        [OperationContract(IsOneWay = true)]
        void RemoveElement(uint elementID);
    }
}