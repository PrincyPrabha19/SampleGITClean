using System;
using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface EquatableCommand : IEquatable<EquatableCommand>, Command
    {
        CommandType CommandType { get; }
        bool IsRedundant { get; }
    }
}