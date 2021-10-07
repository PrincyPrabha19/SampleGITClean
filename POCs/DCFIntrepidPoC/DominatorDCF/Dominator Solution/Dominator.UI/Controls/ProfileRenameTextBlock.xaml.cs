using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dominator.Domain.Classes.Helpers;
using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Controls
{
    public partial class ProfileRenameTextBlock : INotifyPropertyChanged
    {
        private static readonly DependencyProperty isEditingProperty = DependencyProperty.Register("IsEditing", typeof(bool), typeof(ProfileRenameTextBlock), new PropertyMetadata(false));
        public bool IsEditing
        {
            get { return (bool)GetValue(isEditingProperty); }
            set { SetValue(isEditingProperty, value); }
        }

        private static readonly DependencyProperty textProperty = DependencyProperty.Register("Text", typeof(string), typeof(ProfileRenameTextBlock));        
        public string Text
        {
            get { return (string)GetValue(textProperty); }
            set { SetValue(textProperty, value); }
        }

        private static readonly DependencyProperty editingTextProperty = DependencyProperty.Register("EditingText", typeof(string), typeof(ProfileRenameTextBlock),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                delegate(DependencyObject d, DependencyPropertyChangedEventArgs e) { (d as ProfileRenameTextBlock)?.ApplyCommand.RaiseCanExecuteChanged(); }));
        public string EditingText
        {
            get { return (string)GetValue(editingTextProperty); }
            set { SetValue(editingTextProperty, value); }
        }

        private static readonly DependencyProperty editedCommandProperty = DependencyProperty.Register("EditedCommand", typeof(ICommand), typeof(ProfileRenameTextBlock));
        public ICommand EditedCommand
        {
            get { return (ICommand)GetValue(editedCommandProperty); }
            set { SetValue(editedCommandProperty, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public RelayCommand<object> EditingCommand { get; private set; }
        public RelayCommand<object> ApplyCommand { get; private set; }
        public RelayCommand<object> CancelCommand { get; private set; }

        public ProfileRenameTextBlock()
        {
            ResourceDictionaryLoader.LoadInto(Resources);
            ResourceDictionaryLoader.LoadInto(Resources, "/Converters/ConverterDictionary.xaml");
            initializeCommands();

            InitializeComponent();            
        }

        private void initializeCommands()
        {
            EditingCommand = new RelayCommand<object>(executeEditing, canExecuteEditing);
            ApplyCommand = new RelayCommand<object>(executeApply, canExecuteApply);
            CancelCommand = new RelayCommand<object>(executeCancel, canExecuteCancel);
        }

        private bool canExecuteEditing(object obj)
        {
            return true;
        }

        private void executeEditing(object obj)
        {
            EditingText = Text;
            IsEditing = true;
            (obj as TextBox)?.Focus();
        }

        private bool canExecuteApply(object obj)
        {
            return ProfileNameValidator.IsValidProfileName(EditingText);
        }

        private void executeApply(object obj)
        {
            IsEditing = false;
            Text = EditingText;
        }

        private bool canExecuteCancel(object obj)
        {
            return true;
        }

        private void executeCancel(object obj)
        {
            IsEditing = false;
        }
    }
}
