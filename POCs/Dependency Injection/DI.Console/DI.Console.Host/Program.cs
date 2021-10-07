using StructureMap;

namespace DI.Console.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = Container.For<DependencyRegistry>();
            var calculator = container.GetInstance<BasicCalculator>();

            calculator.Run();
        }
    }
}
