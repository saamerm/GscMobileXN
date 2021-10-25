using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MobileClaims.Core.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> destination, IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                destination.Add(item);
            }
        }
    }
}