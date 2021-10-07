namespace Dominator.Domain
{
    public interface IMonitorManager
    {
        void Start();
        void Stop();
        void AddElement(uint elementID);
        void AddElements(uint[] elementIDs);
        decimal GetElementValue(uint elementID);
        decimal[] GetAllElementValues(uint[] elementIDs);
        void RemoveElement(uint elementID);
    }
}
