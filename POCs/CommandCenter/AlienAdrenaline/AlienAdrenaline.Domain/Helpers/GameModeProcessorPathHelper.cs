

using System;
using AlienLabs.CommandCenter.Tools.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Helpers
{
    public abstract class GameModeProcessorPathHelper
    {        
#if DEBUG
        public string GameModeProcessorFile = @"C:\DEV\CommandCenter\DEV\DEV_INT\source\AWCC 4.0\AWCC 4.0 Release\AlienAdrenaline\AlienAdrenaline.GameModeProcessor\bin\Debug\GameModeProcessor.exe";
#else
        public string GameModeProcessorFile = String.Format(@"{0}\GameModeProcessor.exe", ApplicationSettings.StartupPath); 
#endif
    }
}
