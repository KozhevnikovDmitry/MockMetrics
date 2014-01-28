using System.Collections.Generic;

using SpecManager.BL.Interface;

namespace SpecManager.BL.Model
{
    public class DomainObjectComparer<T> : IEqualityComparer<T> where T : IDomainObject
    {
        public bool Equals(T x, T y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(T obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
