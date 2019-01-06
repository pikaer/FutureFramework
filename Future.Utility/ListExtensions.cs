using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Future.Utility
{
    public static class ListExtensions
    {
       

        public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> source, Func<T, Guid> keySelector)
        {
            return source.GroupBy(keySelector).Select(group => group.First());
        }

        public static IEnumerable<Dictionary<string, object>> ToDicList<T>(this IEnumerable<T> source)
        {
            List<Dictionary<string, object>> res = new List<Dictionary<string, object>>();
            foreach (var tt in source)
            {
                res.Add(tt.ToDictionary());
            }
            return res;
        }
        
    }
}
