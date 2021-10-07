using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Dominator.Domain;
using Dominator.Domain.Classes.Factories;
using Dominator.Domain.Enums;
using Dominator.UI.Classes;
using Dominator.UI.Classes.Enums;
using Dominator.UI.Classes.Helpers;
using Dominator.UI.Controls;
using Dominator.UI.Views;
using DesignerProperties = Dominator.UI.Classes.Helpers.DesignerProperties;

namespace Dominator.UI.ViewModels
{
    public class AdvancedOCViewModel : ViewModelBase
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

        private string profileName;
        public string ProfileName
        {
            get { return profileName; }
            set
            {
                SetProperty(ref profileName, value, "ProfileName");
                ViewActionCommand?.Execute(CommandFactory.NewProfileNameCommand(profileName)); //new
            }
        }

        private string riskInformation;
        public string RiskInformation
        {
            get { return riskInformation;}
            set
            {
                SetProperty(ref riskInformation, value, "RiskInformation");
            }
        }
        private bool isNewProfile;
        public bool IsNewProfile
        {
            get { return isNewProfile; }
            set { SetProperty(ref isNewProfile, value, "IsNewProfile"); }
        }

        private CategoryNavigationViewModel categoryNavigationViewModel;
        public CategoryNavigationViewModel CategoryNavigationViewModel
        {
            get { return categoryNavigationViewModel; }
            set { SetProperty(ref categoryNavigationViewModel, value, "CategoryNavigationViewModel"); }
        }

        private ViewModelBase categoryDetailsViewModel;
        public ViewModelBase CategoryDetailsViewModel
        {
            get { return categoryDetailsViewModel; }
            set { SetProperty(ref categoryDetailsViewModel, value, "CategoryDetailsViewModel"); }
        }

        private bool isProfileSaved;
        public bool IsProfileSaved
        {
            get { return isProfileSaved; }
            set { SetProperty(ref isProfileSaved, value, "IsProfileSaved"); }
        }

        private bool isSummaryOpen;
        public bool IsSummaryOpen
        {
            get { return isSummaryOpen; }
            set { SetProperty(ref isSummaryOpen, value, "IsSummaryOpen"); }
        }
         
        private List<ColourSliderRange> overallOCRanges;
        public List<ColourSliderRange> OverallOCRanges
        {
            get { return overallOCRanges; }
            set { SetProperty(ref overallOCRanges, value, "OverallOCRanges"); }
        }

        private int decimalsInOverallOC;
        public int DecimalsInOverallOC
        {
            get { return decimalsInOverallOC; }
            set { SetProperty(ref decimalsInOverallOC, value, "DecimalsInOverallOC"); }
        }

        private KeyValuePair<int, decimal> currentOverallOC;
        public KeyValuePair<int, decimal> CurrentOverallOC
         {
            get { return currentOverallOC; }
            set { SetProperty(ref currentOverallOC, value, "CurrentOverallOC"); }
        }

        public IAdvancedOCModel AdvancedOCModel { get; set; }
        public IProfileNameRepository ProfileNameRepository { get; set; }
        public ICommandContainer CommandContainer { get; private set; }

        public ViewModelBase CPUCategoryDetailsViewModel { get; private set; }
        public ViewModelBase MemoryCategoryDetailsViewModel { get; private set; }
        public ViewModelBase SystemThermalCategoryDetailsViewModel { get; private set; }

        public RelayCommand<object> ShowCPUCategoryViewCommand { get; set; }
        public RelayCommand<object> ShowMemoryCategoryViewCommand { get; set; }
        public RelayCommand<object> ShowSystemThermalCategoryViewCommand { get; set; }
        
        public RelayCommand<object> SaveCommand { get; set; }
        public RelayCommand<object> DeleteCommand { get; set; }
        public RelayCommand<object> CancelCommand { get; set; }
        public RelayCommand<object> CloseCommand { get; set; }

        public RelayCommand<IEquatableCommand> ViewActionCommand { get; set; }

        private bool isSummaryWindowOpen;
        private bool isMemoryCategoryVisited;
        #endregion

        #region Constructor
        public AdvancedOCViewModel()
        {
            initializeCommands();
        }
        #endregion

        #region Methods
        public override void Initialize()
        {
            CommandContainer = CommandContainerFactory.CommandContainer;
            CategoryNavigationViewModel?.Initialize();
        }

        private void riskLevelChanged()
        {
            updateOverallOC();
        }

        private void updateOverallOC()
        {
            if (CPUCategoryDetailsViewModel.RiskInfo.RiskLevel == 1)
                CurrentOverallOC = new KeyValuePair<int, decimal>(0, CPUCategoryDetailsViewModel.RiskInfo.Value); 
            if (CPUCategoryDetailsViewModel.RiskInfo.RiskLevel == 2)
                CurrentOverallOC = new KeyValuePair<int, decimal>(1, CPUCategoryDetailsViewModel.RiskInfo.Value); 
            if (CPUCategoryDetailsViewModel.RiskInfo.RiskLevel == 3)
                CurrentOverallOC = new KeyValuePair<int, decimal>(2, CPUCategoryDetailsViewModel.RiskInfo.Value);
        }

        private List<ColourSliderRange> createRanges(List<IConfiguration> settings )
        {
            var sliderRange = new List<ColourSliderRange>();
            foreach (var setting in settings)
                {
                sliderRange.Add(new ColourSliderRange
            {
                    Min = setting.Min,
                    Max = setting.Max,
                    Color = (setting.RiskLevel == 1) ? Color.FromRgb(0x66, 0x99, 0x00) :
                            (setting.RiskLevel == 2) ? Color.FromRgb(0xCC, 0x99, 0x00) :
                                                       Color.FromRgb(0xCC, 0x00, 0x00)
                });
            }
            return sliderRange;
        }

        public override void Load()
        {
            CategoryNavigationViewModel?.Load();

            DecimalsInOverallOC = 0;
            var overallOCSettings = AdvancedOCModel?.OverallOCSettings();
            OverallOCRanges = createRanges(overallOCSettings);
            
            CPUCategoryDetailsViewModel.RiskChanged += riskLevelChanged;
            riskLevelChanged();
            IsNewProfile = AdvancedOCModel.IsNewProfile;
            IsProfileSaved = false;
            ProfileName = IsNewProfile ? ProfileNameRepository.GenerateNewCustomProfileName(Properties.Resources.NewCustomProfileNameFormat) : 
                AdvancedOCModel.GetExistingProfileName();
            ViewActionCommand?.Execute(CommandFactory.NewProfileNameCommand(profileName));

            IsSummaryOpen = false;
            isMemoryCategoryVisited = false;

        }

        public override void Unload()
        {
            CategoryNavigationViewModel?.Unload();
            CategoryDetailsViewModel?.Unload();
        }

        private void initializeCommands()
        {
            ShowCPUCategoryViewCommand = new RelayCommand<object>(executeShowCPUCategoryView, canExecuteShowCPUCategoryView);
            ShowMemoryCategoryViewCommand = new RelayCommand<object>(executeShowMemoryCategoryView, canExecuteShowMemoryCategoryView);
            ShowSystemThermalCategoryViewCommand = new RelayCommand<object>(executeShowSystemThermalCategoryView, canExecuteShowSystemThermalCategoryView);
            SaveCommand = new RelayCommand<object>(executeSave, canExecuteSave);
            DeleteCommand = new RelayCommand<object>(executeDelete, canExecuteDelete);
            CancelCommand = new RelayCommand<object>(executeCancel, canExecuteCancel);
            CloseCommand = new RelayCommand<object>(executeClose, canExecuteClose);
            ViewActionCommand = new RelayCommand<IEquatableCommand>(executeViewAction, canExecuteViewAction);            
        }

        private bool canExecuteViewAction(IEquatableCommand obj)
        {
            return true;
        }

        private void executeViewAction(IEquatableCommand obj)
        {
            CommandContainer.Add(obj);
            SaveCommand.RaiseCanExecuteChanged();
        }

        private bool canExecuteSave(object obj)
        {                        
            return !CommandContainer.IsEmpty && !isSummaryWindowOpen; 
        }

        private void executeSave(object obj)
        {
            CategoryNavigationViewModel.Load();

            foreach (var command in CommandContainer.Commands)
                command.Execute();

            AdvancedOCModel.SetCurrentProfileValues();

            isSummaryWindowOpen = true;
            IsSummaryOpen = true;

            SaveCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();

            bool isValidationRequired = CommandContainer.Commands.FirstOrDefault(command => command.CommandType == CommandType.AdvancedCPUOC) != null;
            CommandContainer.Clear();
            var summaryWindow = createAdvancedOCSummaryViewModel() as AdvancedOCSummaryViewModel;
            if (summaryWindow != null && summaryWindow.ShowView(isValidationRequired))
            {
                isSummaryWindowOpen = false;
                IsSummaryOpen = false;
                var summaryResult = summaryWindow.SummaryResult;
                if (summaryResult == SummaryResult.ValidationCancel)
                {
                    SaveCommand.RaiseCanExecuteChanged();
                    DeleteCommand.RaiseCanExecuteChanged();
                    CancelCommand.RaiseCanExecuteChanged();
                    return;
                }

                IsProfileSaved = true;
                var cpuCategoryDetailsViewModel = CPUCategoryDetailsViewModel as CPUCategoryDetailsViewModel;
                if (cpuCategoryDetailsViewModel != null)
                    cpuCategoryDetailsViewModel.CPUValidationStatus = (summaryResult == SummaryResult.ValidationPassed) ? ValidationControlStatus.Passed : ValidationControlStatus.Failed;
            }
        }

        private bool canExecuteDelete(object obj)
        {
            return !isSummaryWindowOpen;
        }

        private void executeDelete(object obj)
        {
            var result = new Notifier().Show(Properties.Resources.Delete.ToUpper(), Properties.Resources.DeleteProfileWarningMessage, NotifierIcon.Question, NotifierButtons.YesNo);
            if (result == NotifierResult.Yes)
            {
                AdvancedOCModel.DeleteCurrentProfile();
                NavigationViewModel.HostView(ViewTypes.Main);
            }
        }

        private bool canExecuteCancel(object obj)
        {
            return !isSummaryWindowOpen;
        }

        private bool canExecuteClose(object obj)
        {
            return !isSummaryWindowOpen;
        }

        private void executeClose(object obj)
        {
            NavigationViewModel.HostView(ViewTypes.Main);
        }

        private void executeCancel(object obj)
        {
            AdvancedOCModel.CancelCurrrentProfile();
            CommandContainer.Clear();
            NavigationViewModel.HostView(ViewTypes.Main);
        }

        private bool canExecuteShowCPUCategoryView(object obj)
        {
            return true;
        }

        private void executeShowCPUCategoryView(object obj)
        {
            categoryNavigationViewModel.IsMemoryCategorySelected = false;
            CategoryDetailsViewModel?.Unload();
            CategoryDetailsViewModel = CPUCategoryDetailsViewModel ?? (CPUCategoryDetailsViewModel = createCPUCategoryDetailsViewModel());
            CategoryDetailsViewModel?.Load();
            if(!isMemoryCategoryVisited)
            CategoryNavigationViewModel.MemoryArrowEnabled = true;
        }

        private bool canExecuteShowMemoryCategoryView(object obj)
        {
            return true;
        }

        private void executeShowMemoryCategoryView(object obj)
        {
            isMemoryCategoryVisited = true;
            categoryNavigationViewModel.IsMemoryCategorySelected = true;
            CategoryDetailsViewModel?.Unload();
            CategoryDetailsViewModel = MemoryCategoryDetailsViewModel ?? (MemoryCategoryDetailsViewModel = createMemoryCategoryDetailsViewModel());
            CategoryDetailsViewModel?.Load();
            CategoryNavigationViewModel.MemoryArrowEnabled = false;
        }

        private bool canExecuteShowSystemThermalCategoryView(object obj)
        {
            return true;
        }

        private void executeShowSystemThermalCategoryView(object obj)
        {
            CategoryDetailsViewModel?.Unload();
            CategoryDetailsViewModel = SystemThermalCategoryDetailsViewModel ?? (SystemThermalCategoryDetailsViewModel = createSystemThermalCategoryDetailsViewModel());
            CategoryDetailsViewModel?.Load();
        }
 
        private ViewModelBase createCPUCategoryDetailsViewModel()
        {
            var viewModel = new CPUCategoryDetailsViewModel
            {
                View = new CPUCategoryDetailsView(),
                Model = OverclockingFactory.NewOverclockingModel(),
                AdvancedOCModel = AdvancedOCFactory.NewAdvancedOCModel(),                
                HostViewModel = this,
                ViewActionCommand = ViewActionCommand
            };
            
            viewModel.Initialize();
            return viewModel;
        }

        private ViewModelBase createMemoryCategoryDetailsViewModel()
        {
            var viewModel = new MemoryCategoryDetailsViewModel
            {
                View = new MemoryCategoryDetailsView(),
                Model = OverclockingFactory.NewOverclockingModel(),
                AdvancedOCModel = AdvancedOCFactory.NewAdvancedOCModel(),
                HostViewModel = this,
                ViewActionCommand = ViewActionCommand
            };

            viewModel.Initialize();
            return viewModel;
        }

        private ViewModelBase createAdvancedOCSummaryViewModel()
        {
            var viewModel = new AdvancedOCSummaryViewModel
            {
                View = new AdvancedOCSummaryView(),
                Model = OverclockingFactory.NewOverclockingModel(),
                AdvancedOCModel = AdvancedOCFactory.NewAdvancedOCModel(),
                HostViewModel = this
            };

            viewModel.Initialize();
            return viewModel;
        }

        private ViewModelBase createSystemThermalCategoryDetailsViewModel()
        {
            //var viewModel = new MemoryCategoryDetailsViewModel
            //{
            //    View = new MemoryCategoryDetailsView(),
            //    Model = OverclockingFactory.NewOverclockingModel(),
            //    AdvancedOCModel = AdvancedOCFactory.NewAdvancedOCModel(),
            //    HostViewModel = this
            //};

            //viewModel.Initialize();
            //return viewModel;
            return null;
        }
        #endregion
    }
}