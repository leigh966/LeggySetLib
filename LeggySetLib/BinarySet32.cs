using System.Collections;

namespace LeggySetLib
{
    public class BinarySet32 : ISet<int>
    {
        private int minNum, maxNum;
        private uint setBin;
        public BinarySet32(int minimumNumber, int maximumNumber)
        {
            this.minNum = minimumNumber;
            this.maxNum = maximumNumber;

            int length = maximumNumber - minimumNumber + 1;
            if (length < 1)
            {
                throw new ArgumentException("Set length cannot be less than 1");
            }
            if (length > 32)
            {
                throw new ArgumentException("BinarySet32 cannot handle a range of more than 32 numbers");
            }
        }

        private uint GetRepresentationOfCollection(IEnumerable<int> collection, bool ignoreException)
        {
            uint rep = 0;
            foreach (int item in collection)
            { 
                if(ignoreException && (item < minNum || item > maxNum))
                {
                    continue;
                }
                rep |= GetRepresentingBinary(item);        
            }
            return rep;
        }

        public int Count
        {
            get
            {
                int count = 0;
                uint binaryNumber = setBin;
                while (binaryNumber != 0)
                {
                    binaryNumber &= (binaryNumber - 1);
                    count++;
                }
                return count;
            }
        }

        public bool IsReadOnly { get { return false; } }

        private uint GetRepresentingBinary(int number)
        {
            if (number > maxNum || number < minNum)
                throw new ArgumentException("Number " + number.ToString() + " not in range " + minNum.ToString() + " - " + maxNum.ToString());
            int position = number - minNum;
            return (uint)(1 << position);
        }

        public bool Add(int item)
        {
            uint rep = GetRepresentingBinary(item);
            bool contains = ContainsRepresentation(rep);
            setBin |= rep;
            return !contains;
        }

        public void Clear()
        {
            setBin = 0;
        }

        private bool ContainsRepresentation(uint rep)
        {
            return (setBin & rep) != 0;
        }

        public bool Contains(int item)
        {
            uint rep = GetRepresentingBinary(item);
            return ContainsRepresentation(rep);
        }

        public int[] ToArray()
        {
            List<int> list = new List<int>();
            for(int i = minNum; i <= maxNum; i++)
            {
                if(Contains(i)) list.Add(i);
            }
            return list.ToArray();
        }

        public void CopyTo(int[] array, int arrayIndex)
        {
            int[] from = ToArray();
            for(int index = 0; index < from.Length; index++)
            {
                array[arrayIndex+ index] = from[index];
            }
        }

        public void ExceptWith(IEnumerable<int> other)
        {
            uint rep = GetRepresentationOfCollection(other, true);
            setBin &= ~rep;
        }

        public IEnumerator<int> GetEnumerator()
        {
            foreach(int item in ToArray())
            {
                yield return item;
            }
        }

        public void IntersectWith(IEnumerable<int> other)
        {
            uint rep = GetRepresentationOfCollection(other, true);
            setBin &= rep;
        }

        public bool IsProperSubsetOf(IEnumerable<int> other)
        {
            return IsSubsetOf(other) && Count < other.Count();
        }

        public bool IsProperSupersetOf(IEnumerable<int> other)
        {
            return IsSupersetOf(other) && Count > other.Count();
        }

        public bool IsSubsetOf(IEnumerable<int> other)
        {
            if(Count > other.Count()) return false;
            foreach(int item in ToArray())
            {
                if(!other.Contains(item)) return false;
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

        public bool Overlaps(IEnumerable<int> other)
        {
            uint rep = GetRepresentationOfCollection(other, true);
            return ContainsRepresentation(rep);
        }

        public bool Remove(int item)
        {
            uint rep = GetRepresentingBinary(item);
            bool contains = ContainsRepresentation(rep);
            setBin &= ~rep;
            return contains;
        }

        public bool SetEquals(IEnumerable<int> other)
        {
            if (other.Count() != Count) return false;
            try
            {
                uint rep = GetRepresentationOfCollection(other, false);
                return rep == setBin;
            }
            catch(ArgumentException)
            {
                return false;
            }

        }

        public void SymmetricExceptWith(IEnumerable<int> other)
        {
            uint rep = GetRepresentationOfCollection(other, false);
            setBin ^= rep;
        }

        public void UnionWith(IEnumerable<int> other)
        {

            foreach (var item in other)
            {
                setBin |= GetRepresentingBinary(item);
            }

        }

        void ICollection<int>.Add(int item)
        {
            Add(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
