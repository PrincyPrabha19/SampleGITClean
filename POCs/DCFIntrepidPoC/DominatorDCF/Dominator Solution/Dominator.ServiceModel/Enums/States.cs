namespace Dominator.ServiceModel.Enums
{
    public enum States
    {        
        ImportSucceeded = 3,
        ProfileAlreadyExist = 8,                    
        OverclockingLocked = 4,
        EistDisabled = 5,
        TurboDisabled = 6,
        ImportInvalidPlatform = 7,
        ImportCanceled = 0,
        InvalidFile = 1,
        ProcessorMismatch = 2,
        RestartRequired = 20,
        Unknown = 255
    }
}
