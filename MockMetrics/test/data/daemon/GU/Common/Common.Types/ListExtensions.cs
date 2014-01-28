using System.Collections.Generic;
using System.Linq;

namespace Common.Types
{
    public static class ListExtensions
    {
        public static T Next<T>(this List<T> list, T item) where T : class
        {
            if (! list.Contains(item))
            {
                return null;
            }

            return list.ElementAt(list.IndexOf(item) + 1);
        }
    }
}
