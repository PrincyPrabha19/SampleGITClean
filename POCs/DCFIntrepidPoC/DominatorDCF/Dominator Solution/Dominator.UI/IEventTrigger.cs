using Dominator.Domain;

namespace Dominator.UI
{
    public interface IEventTrigger
    {
        void Fire(IEquatableCommand equatableCommand);
    }
}