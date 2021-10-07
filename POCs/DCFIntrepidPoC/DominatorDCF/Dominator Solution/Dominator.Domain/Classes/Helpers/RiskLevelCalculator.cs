using System.Linq;

namespace Dominator.Domain.Classes.Helpers
{
    public class RiskLevelCalculator
    {
        public IRiskData ComputeRiskLevel(uint[] riskLevel)
        {
            var level = riskLevel.Max();
            uint count = 0;
            foreach (var element in riskLevel)
                if (element == level)
                    count++;
            if(count == 1)
                return new RiskData { RiskLevel = level, Value = 50 };
            if (count == 2)
                return new RiskData { RiskLevel = level, Value = 75 };
            if (count == riskLevel.Length)
            {
                if(level == (uint)RiskLevel.Red)
                    return new RiskData { RiskLevel = level, Value = 100 };
                if (level == (uint)RiskLevel.Green || level == (uint)RiskLevel.Yellow)
                    return new RiskData { RiskLevel = level, Value = 50 };
            }

            return new RiskData { RiskLevel = level, Value = 50 };
        }
    }

    public enum RiskLevel
    {
        Green   = 1,
        Yellow  = 2,
        Red     = 3
    }
}
