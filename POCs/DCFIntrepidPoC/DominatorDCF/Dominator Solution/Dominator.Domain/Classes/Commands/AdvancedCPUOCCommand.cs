using System;
using Dominator.Domain.Enums;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes.Commands
{
    public class AdvancedCPUOCCommand : IEquatableCommand
    {
        public CommandType CommandType => CommandType.AdvancedCPUOC;
        public IAdvancedOCModel AdvancedOCModel { get; set; }

        public decimal Frequency { get; set; }
        public decimal Voltage { get; set; }
        public decimal VoltageOffset { get; set; }
        public decimal VoltageMode { get; set; }

        public void Execute()
        {
            AdvancedOCModel.AddControl(HWComponentType.CPU, SettingType.ActualFrequency, Frequency);
            AdvancedOCModel.AddControl(HWComponentType.CPU, SettingType.ActualVoltage, Voltage);
            AdvancedOCModel.AddControl(HWComponentType.CPU, SettingType.VoltageOffset, VoltageOffset);
            AdvancedOCModel.AddControl(HWComponentType.CPU, SettingType.VoltageMode, VoltageMode);
        }

        public bool IsRedundant
        {
            get
            {
                decimal? currentFrequency = !AdvancedOCModel.IsNewProfile ? AdvancedOCModel.GetInitialFrequency() : (decimal?) null;
                var currentVoltage = AdvancedOCModel.IsNewProfile ? -1 : AdvancedOCModel.GetInitialVoltage();
                var currentVoltageOffset = AdvancedOCModel.IsNewProfile ? -1 : AdvancedOCModel.GetInitialVoltageOffset();
                var currentVoltageMode = AdvancedOCModel.IsNewProfile ? -1 : AdvancedOCModel.GetInitialVoltageMode();
                return currentFrequency == Math.Round(Frequency,1) && currentVoltage == Math.Round(Voltage,2) && currentVoltageOffset == Math.Round(VoltageOffset,0) && currentVoltageMode == VoltageMode;
            }
        }

        public virtual bool Equals(IEquatableCommand equatableCommand)
        {
            if ((equatableCommand == null) || (GetType() != equatableCommand.GetType()))
                return false;

            if (ReferenceEquals(this, equatableCommand))
                return true;

            var command = equatableCommand as AdvancedCPUOCCommand;
            return CommandType == command?.CommandType;
        }
    }
}