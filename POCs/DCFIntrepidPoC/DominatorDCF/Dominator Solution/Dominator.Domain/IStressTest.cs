using System;

namespace Dominator.Domain
{
    public interface IBasicStressTest
    {                
        int Duration { get; }
        int CPUUsage { get; set; }

        event Action<int> ProgressChanged;
        event Action Completed;

        void Start();
        void Stop();        
    }
}