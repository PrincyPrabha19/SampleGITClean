using System;

namespace DI.UI
{
    public interface IDisplayManager
    {
        string ShowMenu();
        Tuple<T, T> ShowInstruction<T>(string option);
        string ShowResult(string result);
    }
}
