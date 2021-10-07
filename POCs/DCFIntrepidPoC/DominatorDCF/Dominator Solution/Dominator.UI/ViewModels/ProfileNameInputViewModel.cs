using System.Collections.Generic;
using System.Windows;
using Dominator.Domain;
using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.ViewModels
{
    public class ProfileNameInputViewModel : ViewModelBase
    {
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
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private bool isProfileNameInUse;
        public bool IsProfileNameInUse
        {
            get { return isProfileNameInUse; }
            set { SetProperty(ref isProfileNameInUse, value, "IsProfileNameInUse"); }
        }

        private List<string> profileNameList;        

        public List<string> ProfileNameList
        {
            get { return profileNameList; }
            set { SetProperty(ref profileNameList, value, "ProfileNameList"); }
        }

        private string profileNameSelected;
        public string ProfileNameSelected
        {
            get { return profileNameSelected; }
            set
            {
                SetProperty(ref profileNameSelected, value, "ProfileNameSelected");
                ProfileName = profileNameSelected;
            }
        }

        private IProfileManager profileManager;

        public IProfileManager ProfileManager
        {
            get { return profileManager; }
            set
            {
                profileManager = value;
                ProfileNameList = profileManager.GetCustomProfileNameList();
            }
        }

        public RelayCommand<object> SaveCommand { get; set; }
        public RelayCommand<object> CancelCommand { get; set; }

        public ProfileNameInputViewModel()
        {
            initializeCommands();            
        }

        private void initializeCommands()
        {
            SaveCommand = new RelayCommand<object>(executeSave, canExecuteSave);
            CancelCommand = new RelayCommand<object>(executeCancel, canExecuteCancel);
        }

        private bool canExecuteSave(object obj)
        {
            isProfileNameInUse = false;
            return !string.IsNullOrEmpty(ProfileName);
        }

        private void executeSave(object obj)
        {
            if (!ProfileManager.IsValidProfileName(ProfileName) &&
                MessageBox.Show(Properties.Resources.ProfileNameInUse, "", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
                return;

            (View as Window).DialogResult = true;
        }

        private bool canExecuteCancel(object obj)
        {
            return true;
        }

        private void executeCancel(object obj)
        {
            (View as Window).DialogResult = false;
        }

        public bool ShowView()
        {
            var result = (View as Window)?.ShowDialog();
            return result.HasValue && result.Value;
        }
    }
}
