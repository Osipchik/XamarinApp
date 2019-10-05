using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Labs.Data;

namespace Labs.Helpers
{
    public static class Extensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection) =>
            new ObservableCollection<T>(collection);

        public static TestModel ToTestModel(this IQueryable<TestModel> queryable) => queryable.FirstOrDefault(); 

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string TimeToString(this TimeSpan timeSpan) => timeSpan.ToString(@"hh\:mm\:ss");
    }
}
