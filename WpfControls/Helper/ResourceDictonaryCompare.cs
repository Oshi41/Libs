using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfControls.Helper
{
    class ResourceDictonaryCompare : IEqualityComparer<ResourceDictionary>
    {
        public bool Equals(ResourceDictionary x, ResourceDictionary y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            if (x.Count != y.Count)
                return false;

            if (x.Source != y.Source)
                return false;

            var allX = Helper.SelectRecursive(x.MergedDictionaries,
                dictionary => dictionary.MergedDictionaries);
            var allY = Helper.SelectRecursive(y.MergedDictionaries,
                dictionary => dictionary.MergedDictionaries);

            return allY.SequenceEqual(allX, new ResourceDictonaryCompare());
        }

        public int GetHashCode(ResourceDictionary obj)
        {
            return obj.Count.GetHashCode() ^ obj.Source.GetHashCode();
        }
    }
}
