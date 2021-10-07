using System;
using DI.Domain;
using DI.UI;

namespace DI.Console.Host
{
    public class BasicCalculator
    {
        private readonly IDisplayManager _displayManager;
        private readonly IOperationManager _operationManager;
        
        public BasicCalculator(IDisplayManager displayManager, IOperationManager operationManager)
        {
            _displayManager = displayManager;
            _operationManager = operationManager;
        }

        public void Run()
        {
            string option = _displayManager.ShowMenu();
            while(option != "0")
            {
                var optEnum = (OperationType)Enum.Parse(typeof(OperationType), option);
                if ((optEnum == OperationType.Divide || optEnum == OperationType.Modulo))
                {
                    var inputs = _displayManager.ShowInstruction<double>(option);
                    var result = _operationManager.Calculate<double>(option, inputs);
                    option = _displayManager.ShowResult(result.ToString());
                }
                else
                {
                    var inputs = _displayManager.ShowInstruction<long>(option);
                    var result = _operationManager.Calculate<long>(option, inputs);
                    option = _displayManager.ShowResult(result.ToString());
                }
            }
        }
    }
}
