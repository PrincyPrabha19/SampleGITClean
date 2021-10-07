using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.ViewModels
{
    public class StartupOCViewModel : ViewModelBase
    {
        #region Properties
        private IViewWithDataContextAndVisibility view;

        public IViewWithDataContextAndVisibility View
        {
            get { return view; }
            set
            {
                if (view == value) return;

                view = value;
                if (!DesignerProperties.IsInDesignMode)
                    view.DataContext = this;
            }
        }       
        #endregion

        #region Constructor
        public StartupOCViewModel()
        {
        }
        #endregion
    }
}