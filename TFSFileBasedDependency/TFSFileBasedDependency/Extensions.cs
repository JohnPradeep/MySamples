using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependentTFSTracking
{
    public static class Extensions
    {
        public static DependencyList<T> ToDependencyListNavigator<T>(this IEnumerable<T> collection)
        {
            DependencyList<T> dependencyListNavigator = new DependencyList<T>();
            foreach (object obj in collection)
            {
                dependencyListNavigator.Add((T)obj);
            }
            return dependencyListNavigator;
        }
    }
}
