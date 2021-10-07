namespace Dominator.ServiceModel
{
    public interface IDataProvider<out T>
    {
        T GetControlValue(uint controlID);
        T[] GetAllControl(uint[] controlIDs);
    }
}
