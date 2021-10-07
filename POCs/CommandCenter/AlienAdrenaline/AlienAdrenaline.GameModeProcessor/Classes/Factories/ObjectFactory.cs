using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.GameModeProcessor.Presenters;

namespace AlienLabs.AlienAdrenaline.GameModeProcessor.Classes.Factories
{
    public class ObjectFactory
    {
        #region Public Methods
        public static MainPresenter NewMainPresenter(MainView view)
        {
            return new MainPresenter
            {
                View = view,
                Model = ServiceFactory.GameModeProcessorService
            };
        }
        #endregion
    }
}
