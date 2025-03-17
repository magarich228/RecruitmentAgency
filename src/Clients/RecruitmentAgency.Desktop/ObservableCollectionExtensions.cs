using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RecruitmentAgency.Desktop
{
    internal static class ObservableCollectionExtensions
    {
        public static void ReplaceWith<T>(this ObservableCollection<T> source, IEnumerable<T> newItems)
        {
            source.Clear();

            foreach (var item in newItems)
            {
                source.Add(item);
            }
        }
    }
}
