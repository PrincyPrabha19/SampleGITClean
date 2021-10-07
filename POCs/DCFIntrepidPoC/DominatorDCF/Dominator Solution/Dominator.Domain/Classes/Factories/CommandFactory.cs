using Dominator.Domain.Classes.Commands;

namespace Dominator.Domain.Classes.Factories
{
    public class CommandFactory
    {
        public static IEquatableCommand NewAdvancedCPUOCCommand(decimal frequency, decimal voltage, decimal voltageOffset, decimal voltageMode)
        {
            return new AdvancedCPUOCCommand()
            {
                AdvancedOCModel = AdvancedOCFactory.NewAdvancedOCModel(),
                Frequency = frequency,
                Voltage = voltage,
                VoltageOffset = voltageOffset,
                VoltageMode = voltageMode
            };
        }

        public static IEquatableCommand NewAdvancedMEMOCCommand(int profileID)
        {
            return new AdvancedMEMOCCommand()
            {
                AdvancedOCModel = AdvancedOCFactory.NewAdvancedOCModel(),
                ProfileID = profileID
            };
        }

        public static IEquatableCommand NewProfileNameCommand(string profileName)
        {
            return new ProfileNameCommand()
            {
                AdvancedOCModel = AdvancedOCFactory.NewAdvancedOCModel(),
                ProfileName = profileName
            };
        }
    }
}