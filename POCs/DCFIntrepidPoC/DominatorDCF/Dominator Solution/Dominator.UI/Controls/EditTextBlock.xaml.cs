using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Controls
{
    public partial class EditTextBlock : INotifyPropertyChanged
    {
        private static readonly DependencyProperty isEditingProperty = DependencyProperty.Register("IsEditing", typeof(bool), typeof(EditTextBlock), new PropertyMetadata(false));
        public bool IsEditing
        {
            get { return (bool)GetValue(isEditingProperty); }
            set { SetValue(isEditingProperty, value); }
        }

        private static readonly DependencyProperty valueProperty = DependencyProperty.Register("Value", typeof(decimal), typeof(EditTextBlock),
            new FrameworkPropertyMetadata(
                delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
                {
                    var editableLabel = d as EditTextBlock;
                    if (editableLabel != null)
                        editableLabel.Text = !string.IsNullOrEmpty(editableLabel.ValueFormat) ? string.Format(editableLabel.ValueFormat, e.NewValue) : e.NewValue.ToString();
                }));
        public decimal Value
        {
            get { return (decimal)GetValue(valueProperty); }
            set { SetValue(valueProperty, value); }
        }

        private static readonly DependencyProperty valueFormatProperty = DependencyProperty.Register("ValueFormat", typeof(string), typeof(EditTextBlock));
        public string ValueFormat
        {
            get { return (string)GetValue(valueFormatProperty); }
            set { SetValue(valueFormatProperty, value); }
        }

        private static readonly DependencyProperty textProperty = DependencyProperty.Register("Text", typeof(string), typeof(EditTextBlock));
        public string Text
        {
            get { return (string)GetValue(textProperty); }
            set { SetValue(textProperty, value); }
        }

        private static readonly DependencyProperty editingTextProperty = DependencyProperty.Register("EditingText", typeof(string), typeof(EditTextBlock),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                delegate(DependencyObject d, DependencyPropertyChangedEventArgs e) { (d as EditTextBlock)?.ApplyCommand.RaiseCanExecuteChanged(); }));
        public string EditingText
        {
            get { return (string)GetValue(editingTextProperty); }
            set { SetValue(editingTextProperty, value); }
        }

        private static readonly DependencyProperty editedCommandProperty = DependencyProperty.Register("EditedCommand", typeof(ICommand), typeof(EditTextBlock));
        public ICommand EditedCommand
        {
            get { return (ICommand)GetValue(editedCommandProperty); }
            set { SetValue(editedCommandProperty, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public RelayCommand<object> EditingCommand { get; private set; }
        public RelayCommand<object> ApplyCommand { get; private set; }
        public RelayCommand<object> CancelCommand { get; private set; }

        public EditTextBlock()
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
            return obj is TextBox;
        }

        private void executeEditing(object obj)
        {
            EditingText = Value.ToString("0.0");
            IsEditing = true;
            (obj as TextBox)?.Focus();
        }

        private bool canExecuteApply(object obj)
        {
            decimal value;
            return decimal.TryParse(EditingText, out value);
        }

        private void executeApply(object obj)
        {
            IsEditing = false;
            Text = EditingText;
            EditedCommand?.Execute(EditingText);
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
