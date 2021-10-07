using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AlienLabs.AlienAdrenaline.Domain.Helpers
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

        //public static bool Exists<T>(this ObservableCollection<T> collection, T value)
        //{
        //    var list = (from p in collection 
        //                where p.Equals(value) 
        //                select true).FirstOrDefault(); 
        //    return list;
        //}
    }
}