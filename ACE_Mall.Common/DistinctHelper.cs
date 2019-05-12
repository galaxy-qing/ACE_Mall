using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE_Mall.Common
{ 
    public delegate bool CompareDelegate<T>(T x, T y);
    public class DistinctHelper<T>: IEqualityComparer<T>
    {
        private CompareDelegate<T> _compare;
        public DistinctHelper(CompareDelegate<T> d)
            {
                this._compare = d;
            }
        public bool Equals(T x, T y)
            {
                if (_compare != null)
                {
                    return this._compare(x, y);
                }
                else
                {
                    return false;
                }
            }
        public int GetHashCode(T obj)
            {
                return obj.ToString().GetHashCode();
            }
        
    }
}
