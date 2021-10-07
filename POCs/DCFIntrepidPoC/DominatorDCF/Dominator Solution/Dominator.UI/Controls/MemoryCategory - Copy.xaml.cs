using System.ComponentModel;
using System.Windows;
using Dominator.UI.Classes;
using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Controls
{
    public partial class MemoryCategory : INotifyPropertyChanged
    {
        private static readonly DependencyProperty isCategoryEnabledProperty = DependencyProperty.Register("IsCategoryEnabled", typeof(bool), typeof(MemoryCategory));
        public bool IsCategoryEnabled
        {
            get { return (bool)GetValue(isCategoryEnabledProperty); }
            set { SetValue(isCategoryEnabledProperty, value); }
        }

        private static readonly DependencyProperty infoProperty = DependencyProperty.Register("Info", typeof(MemoryCategoryInfo), typeof(MemoryCategory));
        public MemoryCategoryInfo Info
        {
            get { return (MemoryCategoryInfo)GetValue(infoProperty); }
            set { SetValue(infoProperty, value); }
        }


        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public MemoryCategory()
        {
            ResourceDictionaryLoader.LoadInto(Resources);
            ResourceDictionaryLoader.LoadInto(Resources, "/Converters/ConverterDictionary.xaml");
            InitializeComponent();
        }
    }
}
