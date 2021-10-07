using System.Collections.Generic;

namespace AlienAdrenaline.SupportService
{
    public interface AlienFXSupportReaderService
    {
        List<Theme> GetAllThemes();
        string GetActiveTheme();
        bool IsGoDark();
        bool IsAlienFXAPIEnabled();
    }
}
