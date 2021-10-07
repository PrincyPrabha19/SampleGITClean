using StructureMap;

namespace DI.Console.Host
{
    public class DependencyRegistry: Registry
    {
        public DependencyRegistry()
        {
            Scan(scan =>
            {
                scan.Assembly("DI.Domain");
                scan.Assembly("DI.UI");
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });

            //In case we don't follow default convension, we need explicit registration like this
            //For<IDisplayManager>().Use<DisplayManager>();
        }
    }
}
