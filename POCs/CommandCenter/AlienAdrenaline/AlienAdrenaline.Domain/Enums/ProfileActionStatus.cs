
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;

namespace AlienLabs.AlienAdrenaline.Domain.Enums
{
    public enum ProfileActionStatus
    {
        [ResourceKeyAttributeClass("")]
        None,

        [ResourceKeyAttributeClass("ActionNotReadyText")]
        NotReady,

        [ResourceKeyAttributeClass("ActionExecutionStartedText")]
        ExecutionStarted,

        [ResourceKeyAttributeClass("ActionExecutionSucceededText")]
        ExecutionSucceeded,

        [ResourceKeyAttributeClass("ActionExecutionSucceededText")]
        ExecutionSucceededDidNotWait,

        [ResourceKeyAttributeClass("ActionExecutionFailedText")]
        ExecutionFailed,

        [ResourceKeyAttributeClass("ActionRollbackingStartedText")]
        RollbackingStarted,

        [ResourceKeyAttributeClass("ActionRollbackingSucceededText")]
        RollbackingSucceeded,

        [ResourceKeyAttributeClass("ActionRollbackingFailedText")]
        RollbackingFailed
    }
}
