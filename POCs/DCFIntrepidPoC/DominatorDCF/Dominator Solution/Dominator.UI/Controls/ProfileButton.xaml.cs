using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Controls
{
    public partial class ProfileButton : INotifyPropertyChanged
    {
        private static readonly DependencyProperty profileNameProperty = DependencyProperty.Register("ProfileName", typeof(string), typeof(ProfileButton));
        public string ProfileName
        {
            get { return (string)GetValue(profileNameProperty); }
            set { SetValue(profileNameProperty, value); }
        }

        private static readonly DependencyProperty isPredefinedProfileProperty = DependencyProperty.Register("IsPredefinedProfile", typeof(bool), typeof(ProfileButton));
        public bool IsPredefinedProfile
        {
            get { return (bool)GetValue(isPredefinedProfileProperty); }
            set { SetValue(isPredefinedProfileProperty, value); }
        }

        private static readonly DependencyProperty isSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(ProfileButton));
        public bool IsSelected
        {
            get { return (bool)GetValue(isSelectedProperty); }
            set { SetValue(isSelectedProperty, value); }
        }

        private static readonly DependencyProperty isValidProperty = DependencyProperty.Register("IsValid", typeof(bool), typeof(ProfileButton));
        public bool IsValid
        {
            get { return (bool)GetValue(isValidProperty); }
            set { SetValue(isValidProperty, value); }
        }

        private static readonly DependencyProperty areActionButtonsVisibleProperty = DependencyProperty.Register("AreActionButtonsVisible", typeof(bool), typeof(ProfileButton));
        public bool AreActionButtonsVisible
        {
            get { return (bool)GetValue(areActionButtonsVisibleProperty); }
            set { SetValue(areActionButtonsVisibleProperty, value); }
        }

        private static readonly DependencyProperty editProfileCommandProperty = DependencyProperty.Register("EditProfileCommand", typeof(ICommand), typeof(ProfileButton));
        public ICommand EditProfileCommand
        {
            get { return (ICommand)GetValue(editProfileCommandProperty); }
            set { SetValue(editProfileCommandProperty, value); }
        }

        private static readonly DependencyProperty activateProfileCommandProperty = DependencyProperty.Register("ActivateProfileCommand", typeof(ICommand), typeof(ProfileButton));
        public ICommand ActivateProfileCommand
        {
            get { return (ICommand)GetValue(activateProfileCommandProperty); }
            set { SetValue(activateProfileCommandProperty, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ProfileButton()
        {
            ResourceDictionaryLoader.LoadInto(Resources);
            ResourceDictionaryLoader.LoadInto(Resources, "/Converters/ConverterDictionary.xaml");
            InitializeComponent();
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            AreActionButtonsVisible = !IsPredefinedProfile;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            AreActionButtonsVisible = false;
        }
    }
}
