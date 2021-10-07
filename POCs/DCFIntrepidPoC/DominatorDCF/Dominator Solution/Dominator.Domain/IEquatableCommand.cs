using System;
using Dominator.Domain.Enums;

namespace Dominator.Domain
{
    public interface IEquatableCommand : IEquatable<IEquatableCommand>, ICommand
    {
        CommandType CommandType { get; }
        bool IsRedundant { get; }
    }
}