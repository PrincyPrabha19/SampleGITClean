using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Dominator.UI.Classes;
using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Controls
{
    public partial class CPUCategory : INotifyPropertyChanged
    {
        private static readonly DependencyProperty infoProperty = DependencyProperty.Register("Info", typeof(CPUCategoryInfo), typeof(CPUCategory));
        public CPUCategoryInfo Info
        {
            get { return (CPUCategoryInfo)GetValue(infoProperty); }
            set { SetValue(infoProperty, value); }
        }

        private static readonly DependencyProperty cpuOCRebootRequired = DependencyProperty.Register("CPUOCRebootRequired", typeof(bool), typeof(CPUCategory));
        public bool CPUOCRebootRequired
        {
            get { return (bool)GetValue(cpuOCRebootRequired); }
            set { SetValue(cpuOCRebootRequired, value); }
        }

        private static readonly DependencyProperty isCelsiusSelected = DependencyProperty.Register("IsCelsiusSelected", typeof(bool), typeof(CPUCategory));
        public bool IsCelsiusSelected
        {
            get { return (bool)GetValue(isCelsiusSelected); }
            set { SetValue(isCelsiusSelected, value); }
        }

        private static readonly DependencyProperty isCPUToggleOnOffVisibleProperty = DependencyProperty.Register("IsCPUToggleOnOffVisible", typeof(bool), typeof(CPUCategory));
        public bool IsCPUToggleOnOffVisible
        {
            get { return (bool)GetValue(isCPUToggleOnOffVisibleProperty); }
            set { SetValue(isCPUToggleOnOffVisibleProperty, value); }
        }

        private static readonly DependencyProperty isMaximumFrequencyVisibleProperty = DependencyProperty.Register("IsMaximumFrequencyVisible", typeof(bool), typeof(CPUCategory));
        public bool IsMaximumFrequencyVisible
        {
            get { return (bool)GetValue(isMaximumFrequencyVisibleProperty); }
            set { SetValue(isMaximumFrequencyVisibleProperty, value); }
        }

        private static readonly DependencyProperty isVoltageModeVisibleProperty = DependencyProperty.Register("IsVoltageModeVisible", typeof(bool), typeof(CPUCategory));
        public bool IsVoltageModeVisible
        {
            get { return (bool)GetValue(isVoltageModeVisibleProperty); }
            set { SetValue(isVoltageModeVisibleProperty, value); }
        }

        private static readonly DependencyProperty isCategoryEnabledProperty = DependencyProperty.Register("IsCategoryEnabled", typeof(bool), typeof(CPUCategory),
                        new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                            delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
                            {
                                var obj = d as CPUCategory;
                                if (obj != null)
                                    obj.OpacityLevel = Convert.ToBoolean(e.NewValue) ? 1m : 0.4m;
                            }));
        public bool IsCategoryEnabled
        {
            get { return (bool)GetValue(isCategoryEnabledProperty); }
            set { SetValue(isCategoryEnabledProperty, value); }
        }

        private static readonly DependencyProperty opacityLevelProperty = DependencyProperty.Register("OpacityLevel", typeof(decimal), typeof(CPUCategory), new PropertyMetadata(1m));
        public decimal OpacityLevel
        {
            get { return (decimal)GetValue(opacityLevelProperty); }
            set { SetValue(opacityLevelProperty, value); }
        }

        private static readonly DependencyProperty toggleCategoryStatusCommandProperty = DependencyProperty.Register("ToggleCategoryStatusCommand", typeof(ICommand), typeof(CPUCategory));
        public ICommand ToggleCategoryStatusCommand
        {
            get { return (ICommand)GetValue(toggleCategoryStatusCommandProperty); }
            set { SetValue(toggleCategoryStatusCommandProperty, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public CPUCategory()
        {
            ResourceDictionaryLoader.LoadInto(Resources);
            ResourceDictionaryLoader.LoadInto(Resources, "/Converters/ConverterDictionary.xaml");
            InitializeComponent();
        }
    }
}
