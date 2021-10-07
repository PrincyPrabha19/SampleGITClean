using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Dominator.UI.Classes.Helpers
{
    public static class CollectionExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> value)
        {
            var observableCollection = new ObservableCollection<T>();
            foreach (var element in value)
                observableCollection.Add(element);
            return observableCollection;
        }
    }
}