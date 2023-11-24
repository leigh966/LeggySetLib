using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeggySetLib
{
    public abstract class BinarySet : ISet<int>
    {
        public abstract int NUMBER_OF_BITS {  get; } 

        protected int minNum, maxNum;
        public abstract int Count { get; }
        public bool IsReadOnly { get { return false; } }

        #region Initialization
        protected void Init(int minimumNumber, int maximumNumber)
        {
            this.minNum = minimumNumber;
            this.maxNum = maximumNumber;

            int length = maximumNumber - minimumNumber + 1;
            if (length < 1)
            {
                throw new ArgumentException("Set length cannot be less than 1");
            }
            if (length > NUMBER_OF_BITS)
            {
                throw new ArgumentException("BinarySet32 cannot handle a range of more than 32 numbers");
            }
        }

        public BinarySet(int minNum, int maxNum)
        {
            Init(minNum, maxNum);
        }

        public BinarySet(int minimumNumber, int maximumNumber, IEnumerable<int> ints)
        {
            Init(minNum, maxNum);
            UnionWith(ints);
        }


        #endregion

        public abstract bool Add(int item);
        public abstract void Clear();
        public abstract bool Contains(int item);
        public abstract void ExceptWith(IEnumerable<int> other);
        public abstract void IntersectWith(IEnumerable<int> other);

        public bool IsProperSubsetOf(IEnumerable<int> other)
        {
            return IsSubsetOf(other) && Count < other.Count();
        }

        public bool IsProperSupersetOf(IEnumerable<int> other)
        {
            return IsSupersetOf(other) && Count > other.Count();
        }
        public int[] ToArray()
        {
            List<int> list = new List<int>();
            for (int i = minNum; i <= maxNum; i++)
            {
                if (Contains(i)) list.Add(i);
            }
            return list.ToArray();
        }

        public void CopyTo(int[] array, int arrayIndex)
        {
            int[] from = ToArray();
            for (int index = 0; index < from.Length; index++)
            {
                array[arrayIndex + index] = from[index];
            }
        }

        public bool IsSubsetOf(IEnumerable<int> other)
        {
            if (Count > other.Count()) return false;
            foreach (int item in ToArray())
            {
                if (!other.Contains(item)) return false;
            }
            return true;
        }

        public bool IsSupersetOf(IEnumerable<int> other)
        {
            if (other.Count() > Count) return false;
            foreach (var item in other)
            {
                if (!Contains(item)) return false;
            }
            return true;
        }

        public abstract bool Overlaps(IEnumerable<int> other);
        public abstract bool Remove(int item);
        public abstract bool SetEquals(IEnumerable<int> other);
        public abstract void SymmetricExceptWith(IEnumerable<int> other);
        public abstract void UnionWith(IEnumerable<int> other);

        #region Get_Enumerator
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<int> GetEnumerator()
        {
            foreach (int item in ToArray())
            {
                yield return item;
            }
        }

        void ICollection<int>.Add(int item)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
