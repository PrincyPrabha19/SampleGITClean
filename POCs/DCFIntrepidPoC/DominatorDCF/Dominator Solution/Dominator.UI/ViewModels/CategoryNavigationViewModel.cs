using System.Collections.Generic;
using System.Windows;
using Dominator.Domain;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.UI.Classes.Helpers;
using ICommand = System.Windows.Input.ICommand;

namespace Dominator.UI.ViewModels
{
    public class CategoryNavigationViewModel : ViewModelBase
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
                {
                    view.DataContext = this;
                    view.Loaded += viewWasLoaded;
                }
            }
        }

        public List<IProfileInfo> PredefinedProfileList { get; private set; }
        public List<IProfileInfo> CustomProfileList { get; private set; }

        private bool isOverclockingEnabled;
        public bool IsOverclockingEnabled
        {
            get { return isOverclockingEnabled; }
            set
            {
                SetProperty(ref isOverclockingEnabled, value, "IsOverclockingEnabled");
                ProfileListOpacity = isOverclockingEnabled ? 1m : 0.4m;
            }
        }

        private Visibility isMemorySupported;
        public Visibility IsMemorySupported
        {
            get { return isMemorySupported; }
            set { SetProperty(ref isMemorySupported, value, "IsMemorySupported"); }
        }

        private bool isMemoryCategorySelected;
        public bool IsMemoryCategorySelected
        {
            get { return isMemoryCategorySelected; }
            set { SetProperty(ref isMemoryCategorySelected, value, "IsMemoryCategorySelected"); }
        }

        private bool cpuCategorySelected;
        public bool CPUCategorySelected
        {
            get { return cpuCategorySelected; }
            set { SetProperty(ref cpuCategorySelected, value, "CPUCategorySelected"); }
        }

        private decimal profileListOpacity = 0.4m;
        public decimal ProfileListOpacity
        {
            get { return profileListOpacity; }
            set { SetProperty(ref profileListOpacity, value, "ProfileListOpacity"); }
        }

        private bool memoryArrowEnabled;

        public bool MemoryArrowEnabled
        {
            get { return memoryArrowEnabled;}
            set { SetProperty(ref memoryArrowEnabled, value, "MemoryArrowEnabled");}
        }
        public RelayCommand<object> ToggleOCStatusCommand { get; set; }

        public ICommand ShowCPUCategoryViewCommand => (HostViewModel as AdvancedOCViewModel)?.ShowCPUCategoryViewCommand;
        public ICommand ShowMemoryCategoryViewCommand => (HostViewModel as AdvancedOCViewModel)?.ShowMemoryCategoryViewCommand;
        public ICommand ShowSystemThermalCategoryViewCommand => (HostViewModel as AdvancedOCViewModel)?.ShowSystemThermalCategoryViewCommand;
        #endregion

        #region Constructor
        public CategoryNavigationViewModel()
        {
            initializeCommands();
        }
        #endregion

        #region Methods
        public override void Initialize()
        {
            //if (SystemInfoRepository.Instance.MemoryInfoData.IsMemoryOCSupported != null && (!SystemInfoRepository.Instance.MemoryInfoData.IsMemoryOCSupported.Value && !SystemInfoRepository.Instance.XMPInfoData.IsXMPSupported))
            if(!SystemInfoRepository.Instance.XMPInfoData.IsXMPSupported)
                IsMemorySupported = Visibility.Collapsed;
        }

        public override void Load()
        {
            CPUCategorySelected = true;
            ShowCPUCategoryViewCommand?.Execute(null);
        }

        private void initializeCommands()
        {
        }
        #endregion

        private void viewWasLoaded(object sender, RoutedEventArgs e)
        {
            MemoryArrowEnabled = false;
            MemoryArrowEnabled = true;
        }
    }
}