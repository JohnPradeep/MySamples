using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependentTFSTracking
{
    public class ImpactFileInfoEqualityComparer : IEqualityComparer<ImpactFileInfo>
    {
        public bool Equals(ImpactFileInfo x, ImpactFileInfo y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.FileName.Equals(y.FileName))
                return true;
            else
            {
                Console.Write("{0}:{1}", x.FileName, y.FileName);
                return false;
            }
        }
        public int GetHashCode(ImpactFileInfo info)
        {
            return info.FileName.GetHashCode();

        }
    }

    public class TfsIdEqualityComparer : IEqualityComparer<TfsItem>
    {
        public bool Equals(TfsItem item1, TfsItem item2)
        {
            if (item1 == null && item2 == null)
                return true;
            else if (item1 == null || item2 == null)
                return false;
            else if (item1.TfsID == item2.TfsID)
                return true;
            else
                return false;
        }

        public int GetHashCode(TfsItem item)
        {
            return item.TfsID.GetHashCode();
        }
    }

    public class GenericEqualityComparer<TItem, TKey> : EqualityComparer<TItem>
    {
        private readonly Func<TItem, TKey> getKey;
        private readonly EqualityComparer<TKey> keyComparer;
        public GenericEqualityComparer(Func<TItem, TKey> getKey)
        {
            this.getKey = getKey;
            keyComparer = EqualityComparer<TKey>.Default;
        }

        public override bool Equals(TItem x, TItem y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;
            return keyComparer.Equals(getKey(x), getKey(y));
        }

        public override int GetHashCode(TItem obj)
        {
            if (obj == null)
                return 0;
            return keyComparer.GetHashCode(getKey(obj));
        }
    }
}
