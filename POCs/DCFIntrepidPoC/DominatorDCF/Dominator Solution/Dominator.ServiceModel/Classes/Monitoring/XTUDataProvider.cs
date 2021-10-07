namespace Dominator.ServiceModel.Classes.Monitoring
{
    public class XTUDataProvider : IDataProvider<decimal>
    {
        public IXTUService XTUService { private get; set; }

        public decimal GetControlValue(uint controlID)
        {
            return XTUService.GetValueOfControl(controlID);
        }

        public decimal[] GetAllControl(uint[] controlIDs)
        {
            return XTUService.GetAllControlValue(controlIDs);
        }
    }
}
