
namespace AlienAdrenaline.SupportService
{
    public interface AlienFXSupportProcessorService
    {
		void SetActiveTheme(string fullName);
        void EnableAlienFXAPI();
		void DisableAlienFXAPI();
		void GoLight();
        void GoDark();
    }
}
