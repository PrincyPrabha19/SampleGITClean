using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dominator.UI.Classes;
using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Controls
{
    public partial class MemoryCategory : INotifyPropertyChanged
    {
        private static readonly DependencyProperty infoProperty = DependencyProperty.Register("Info", typeof(MemoryCategoryInfo), typeof(MemoryCategory));
        public MemoryCategoryInfo Info
        {
            get { return (MemoryCategoryInfo)GetValue(infoProperty); }
            set { SetValue(infoProperty, value); }
        }

        private static readonly DependencyProperty isMemoryToggleOnOffVisibleProperty = DependencyProperty.Register("IsMemoryToggleOnOffVisible", typeof(bool), typeof(MemoryCategory));
        public bool IsMemoryToggleOnOffVisible
        {
            get { return (bool)GetValue(isMemoryToggleOnOffVisibleProperty); }
            set { SetValue(isMemoryToggleOnOffVisibleProperty, value); }
        }

        private static readonly DependencyProperty memoryOCRebootRequired = DependencyProperty.Register("MemoryOCRebootRequired", typeof(bool), typeof(MemoryCategory));
        public bool MemoryOCRebootRequired
        {
            get { return (bool)GetValue(memoryOCRebootRequired); }
            set { SetValue(memoryOCRebootRequired, value); }
        }

        private static readonly DependencyProperty isXMPSupportedProperty = DependencyProperty.Register("IsXMPSupported", typeof(bool), typeof(MemoryCategory));
        public bool IsXMPSupported
        {
            get { return (bool)GetValue(isXMPSupportedProperty); }
            set { SetValue(isXMPSupportedProperty, value); }
        }

        private static readonly DependencyProperty isCategoryEnabledProperty = DependencyProperty.Register("IsCategoryEnabled", typeof(bool), typeof(MemoryCategory),
                        new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                            delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
                            {
                                var obj = (d as MemoryCategory);
                                if (obj != null)
                                    obj.OpacityLevel = Convert.ToBoolean(e.NewValue) ? 1m : 0.4m;
                            }));
        public bool IsCategoryEnabled
        {
            get { return (bool)GetValue(isCategoryEnabledProperty); }
            set { SetValue(isCategoryEnabledProperty, value); }
        }

        private static readonly DependencyProperty opacityLevelProperty = DependencyProperty.Register("OpacityLevel", typeof(decimal), typeof(MemoryCategory), new PropertyMetadata(1m));
        public decimal OpacityLevel
        {
            get { return (decimal)GetValue(opacityLevelProperty); }
            set { SetValue(opacityLevelProperty, value); }
        }

        private static readonly DependencyProperty toggleCategoryStatusCommandProperty = DependencyProperty.Register("ToggleCategoryStatusCommand", typeof(ICommand), typeof(MemoryCategory));
        public ICommand ToggleCategoryStatusCommand
        {
            get { return (ICommand)GetValue(toggleCategoryStatusCommandProperty); }
            set { SetValue(toggleCategoryStatusCommandProperty, value); }
        }

        private ICommand xmpProfileIDSelectedCommand;
        public ICommand XMPProfileIDSelectedCommand
        {
            get { return xmpProfileIDSelectedCommand; }
            set
            {
                xmpProfileIDSelectedCommand = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("XMPProfileIDSelectedCommand"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public MemoryCategory()
        {
            ResourceDictionaryLoader.LoadInto(Resources);
            ResourceDictionaryLoader.LoadInto(Resources, "/Converters/ConverterDictionary.xaml");
            InitializeComponent();
        }

        private void comboBoxXmps_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
